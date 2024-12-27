using VehicleManagerAPI.Data;
using VehicleManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VehicleManagerAPI.Services
{
    public class MessageTemplateService
    {
        private readonly ApplicationDbContext _context;


        public List<MessageTemplateModel>? MessageTemplates { get; }

        public MessageTemplateService(ApplicationDbContext context)
        {
            _context = context;

            MessageTemplates = _context.MessageTemplate!
                .ToList();
        }

        public List<MessageTemplateModel>? GetAll() => MessageTemplates;

        public MessageTemplateModel? Get(int messageTemplateID) => MessageTemplates?.FirstOrDefault(c => c.MessageTemplateID == messageTemplateID);

        public async Task<ModelResultModel> Add(MessageTemplateModel newMessageTemplate)
        {
            _context.MessageTemplate?.Add(newMessageTemplate);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> AddMany(List<MessageTemplateModel> newMessageTemplates)
        {
            await _context.MessageTemplate?.AddRangeAsync(newMessageTemplates)!;
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> Update(MessageTemplateModel? updatedMessageTemplate)
        {
            MessageTemplateModel? recordToUpdate = _context.MessageTemplate!
                .FirstOrDefault(m => m.MessageTemplateID == updatedMessageTemplate!.MessageTemplateID);

            if (_context.MessageTemplate == null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedMessageTemplate!);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> UpdateMany(List<MessageTemplateModel>? updatedMessageTemplates)
        {
            if (updatedMessageTemplates is null)
                return new ModelResultModel() { IsSuccessful = false };

            foreach (var updatedMessageTemplate in updatedMessageTemplates)
            {
                MessageTemplateModel? recordToUpdate = _context.MessageTemplate!
                    .FirstOrDefault(c => c.MessageTemplateID == updatedMessageTemplate.MessageTemplateID);
                if (_context.MessageTemplate == null)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
                _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedMessageTemplate);
            }

            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> Delete(int messageTemplateID)
        {
            var recordToDelete = Get(messageTemplateID);
            if (recordToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.MessageTemplate!.Remove(recordToDelete);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> DeleteMany(List<MessageTemplateModel>? messageTemplatesToDelete)
        {
            if (messageTemplatesToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.MessageTemplate!.RemoveRange(messageTemplatesToDelete);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }
    }
}
