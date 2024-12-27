using VehicleManagerAPI.Data;
using VehicleManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Net.Mail;
using System.Net;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Reflection;
using System.Text.Json.Serialization;

namespace VehicleManagerAPI.Services
{
    public class SystemEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public GraphAPITokenModel? GraphAPITokenModel { get; set; }

        public List<SystemEmailModel>? SystemEmails { get; }

        private SystemEmailModel? _systemEmail;
        private List<SystemEmailModel>? _systemEmails;

        public SystemEmailService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;

            SystemEmails = new List<SystemEmailModel>();
            _configuration = configuration;
        }

        public List<SystemEmailModel>? GetAll() => SystemEmails;

        public SystemEmailModel? Get(int systemEmailID) => SystemEmails?.FirstOrDefault(m => m.SystemEmailID == systemEmailID);

        public async Task<SystemEmailModel> SendEmail(SystemEmailModel systemEmail)
        {
            string emailService = _configuration["EmailService"] ?? "";
            if (emailService == "SMTP")
                _systemEmail = await SendEmailSMTP(systemEmail);
            else if (emailService == "GraphAPI")
                _systemEmail = await SendEmailGraphAPI(systemEmail);
            else
                _systemEmail = await SendEmailSMTP(systemEmail);

            return _systemEmail;
        }

        public async Task<List<SystemEmailModel>> SendEmails(List<SystemEmailModel> systemEmails)
        {
            _systemEmails = new List<SystemEmailModel>();
            if (systemEmails == null) return _systemEmails;
            foreach (var systemEmail in systemEmails)
            {
                _systemEmails.Add(await SendEmailSMTP(systemEmail));
            }

            return _systemEmails;
        }

        public async Task<SystemEmailModel> SendEmailSMTP(SystemEmailModel systemEmail)
        {
            var message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            message.From = new MailAddress(systemEmail.EmailFrom ?? "ProSolutionForms@shcg.ac.uk", systemEmail.EmailFromName ?? "ProSolution Forms");
            message.To.Add(new MailAddress(systemEmail.EmailTo ?? "ProSolutionForms@shcg.ac.uk", systemEmail.EmailToName ?? systemEmail.EmailTo ?? "Recipient Name"));

            if (!string.IsNullOrEmpty(systemEmail.EmailCC))
                message.CC.Add(systemEmail.EmailCC);
            if (!string.IsNullOrEmpty(systemEmail.EmailBCC))
                message.CC.Add(systemEmail.EmailBCC);

            message.Subject = systemEmail.EmailSubject;
            message.Body = systemEmail.EmailMessage;
            message.IsBodyHtml = systemEmail.IsEmailMessageHTML ?? false;

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("robin.wilson@robindigital.co.uk", "PASSWORD_HERE");
            smtpClient.EnableSsl = true;

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}");
            }

            return systemEmail;
        }

        public async Task<List<SystemEmailModel>> SendEmailsSMTP(List<SystemEmailModel> systemEmails)
        {
            if (systemEmails == null) return new List<SystemEmailModel>();
            foreach (var systemEmail in systemEmails)
            {
                await SendEmailSMTP(systemEmail);
            }

            return systemEmails;
        }

        public async Task<SystemEmailModel> SendEmailGraphAPI(SystemEmailModel systemEmail)
        {
            GraphAPITokenModel GraphAPIToken = new GraphAPITokenModel();
            GraphAPIToken = await GetGraphAPIToken();
            var graphAPI = _configuration.GetSection("GraphAPI");
            var send_email_endpoint = graphAPI["send_email_endpoint"];

            string? sendEmailEndPoint;
            sendEmailEndPoint = $"{send_email_endpoint}";

            string? contentType = "Text";
            if (systemEmail.IsEmailMessageHTML ?? false)
                contentType = "HTML";

            GraphAPIEmailModel GraphAPIEmail = new GraphAPIEmailModel()
            {
                Message = new GraphAPIEmailContentModel()
                {
                    Subject = systemEmail.EmailSubject,
                    Body = new GraphAPIEmailContentBodyModel()
                    {
                        ContentType = contentType,
                        Content = systemEmail.EmailMessage
                    },
                    ToRecipients = new List<GraphAPIEmailContentRecipientModel>()
                    {
                        new GraphAPIEmailContentRecipientModel()
                        {
                            EmailAddress = new GraphAPIEmailContentRecipientEmailModel()
                            {
                                Address = systemEmail.EmailTo,
                                Name = systemEmail.EmailToName
                            }
                        }
                    }
                },
                SaveToSentItems = true
            };

            if (systemEmail.EmailCC != null)
            {
                GraphAPIEmail.Message.CCRecipients = new List<GraphAPIEmailContentRecipientModel>()
                {
                    new GraphAPIEmailContentRecipientModel()
                    {
                        EmailAddress = new GraphAPIEmailContentRecipientEmailModel()
                        {
                            Address = systemEmail.EmailCC,
                            Name = systemEmail.EmailCC
                        }
                    }
                };
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GraphAPIToken?.AccessToken);
                    HttpResponseMessage formResponse = await httpClient.PostAsJsonAsync(sendEmailEndPoint, GraphAPIEmail);
                    var responseContent = await formResponse.Content.ReadAsStringAsync();

                    if (formResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Email Sent: " + responseContent);
                        systemEmail.IsSent = true;
                    }
                    else
                    {
                        throw new Exception($"Error sending email: {responseContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}");
            }

            return systemEmail;
        }

        public async Task<List<SystemEmailModel>> SendEmailsGraphAPI(List<SystemEmailModel> systemEmails)
        {
            if (systemEmails == null) return new List<SystemEmailModel>();
            foreach (var systemEmail in systemEmails)
            {
                await SendEmailGraphAPI(systemEmail);
            }

            return systemEmails;
        }

        public async Task<GraphAPITokenModel> GetGraphAPIToken()
        {
            var graphAPI = _configuration.GetSection("GraphAPI");
            var login_endpoint = graphAPI["login_endpoint"];
            var tenant = graphAPI["tenant"];
            var client_id = graphAPI["client_id"];
            var scope = graphAPI["scope"];
            var client_secret = graphAPI["client_secret"];
            var grant_type = graphAPI["grant_type"];

            string? loginEndPoint;
            loginEndPoint = $"{login_endpoint}/{tenant}/oauth2/v2.0/token";
            
            GraphAPIAuthorisationModel? GraphAPIAuthorisation = new GraphAPIAuthorisationModel()
            {
                ClientID = client_id,
                Scope = scope,
                ClientSecret = client_secret,
                GrantType = grant_type
            };

            GraphAPITokenModel? graphAPIToken = new GraphAPITokenModel();

            //var content = new FormUrlEncodedContent(
            //[
            //     new KeyValuePair<string, string>("client_id", client_id),
            //     new KeyValuePair<string, string>("scope", scope),
            //     new KeyValuePair<string, string>("client_secret", client_secret),
            //     new KeyValuePair<string, string>("grant_type", grant_type)
            //]);

            IList<KeyValuePair<string, string>> formParams = new List<KeyValuePair<string, string>>();

            if (GraphAPIAuthorisation != null)
            {
                foreach (var prop in GraphAPIAuthorisation.GetType().GetProperties())
                {

                    formParams.Add(new KeyValuePair<string, string>(prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? prop.Name, prop.GetValue(GraphAPIAuthorisation)?.ToString() ?? ""));
                }
            }

            var formParamsEncoded = new FormUrlEncodedContent(formParams.ToArray());

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //Is not JSON but x-www-form-urlencoded so needs PostAsync
                    HttpResponseMessage formResponse = await httpClient.PostAsync(loginEndPoint, formParamsEncoded);
                    var responseContent = await formResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("GraphAPIAuthorisation: " + JsonSerializer.Serialize(GraphAPIAuthorisation));
                    if (formResponse.IsSuccessStatusCode)
                    {
                        graphAPIToken = JsonSerializer.Deserialize<GraphAPITokenModel>(responseContent);
                    }
                    else
                    {
                        throw new Exception($"Error getting token: {responseContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting token: {ex.Message}");
            }

            return graphAPIToken ?? new();
        }
    }
}
