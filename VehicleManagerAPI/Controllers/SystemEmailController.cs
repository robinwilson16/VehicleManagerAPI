using VehicleManagerAPI.Models;
using VehicleManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace VehicleManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemEmailController : ControllerBase
    {
        private readonly SystemEmailService _systemEmailService;
        private readonly IConfiguration _configuration;

        private SystemEmailModel? _systemEmail;
        private List<SystemEmailModel>? _systemEmails;

        public SystemEmailController(SystemEmailService systemEmailService, IConfiguration configuration)
        {
            _systemEmailService = systemEmailService;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<List<SystemEmailModel>?>? GetAll() =>
            _systemEmailService.GetAll();

        [HttpGet("{systemEmailID}")]
        public ActionResult<SystemEmailModel> Get(int systemEmailID)
        {
            var systemEmail = _systemEmailService.Get(systemEmailID);

            if (systemEmail == null)
                return NotFound();

            return systemEmail;
        }

        [HttpPost]
        public async Task<IActionResult> Send(SystemEmailModel systemEmail)
        {
            //Check this record does not already exist
            var existingRecord = _systemEmailService.Get(systemEmail.SystemEmailID);

            if (existingRecord is not null)
                return BadRequest("Record Exists");

            string? emailKey = _configuration.GetSection("Settings")["EmailKey"];

            if (emailKey != systemEmail.EmailKey)
                return Unauthorized("Email Key not valid");

            _systemEmail = await _systemEmailService.SendEmail(systemEmail);

            return CreatedAtAction(nameof(Send), new { }, _systemEmail);
        }

        [HttpPost("Many")]
        public async Task<IActionResult> SendMany(List<SystemEmailModel> systemEmails)
        {
            _systemEmails = await _systemEmailService.SendEmails(systemEmails);

            return CreatedAtAction(nameof(Send), new { }, _systemEmails);
        }
    }
}
