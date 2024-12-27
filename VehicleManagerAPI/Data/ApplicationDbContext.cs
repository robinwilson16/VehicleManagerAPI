using VehicleManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace VehicleManagerAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration _configuration) : DbContext(options)
    {
        public IConfiguration configuration { get; } = _configuration;

        public DbSet<ConfigModel>? Config { get; set; }
        public DbSet<CustomerModel>? Customer { get; set; }
        public DbSet<GraphAPIAuthorisationModel>? GraphAPIAuthorisation { get; set; }
        public DbSet<GraphAPITokenModel>? GraphAPIToken { get; set; }
        public DbSet<MessageModel>? Message { get; set; }
        public DbSet<MessageTemplateModel>? MessageTemplate { get; set; }
        public DbSet<NoteModel>? Note { get; set; }
        public DbSet<SystemEmailModel>? SystemEmail { get; set; }
    }
}