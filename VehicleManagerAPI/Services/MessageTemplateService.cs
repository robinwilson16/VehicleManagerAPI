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

        public async Task<ModelResultModel> Update(MessageTemplateModel? updatedMessageTemplate, bool? save)
        {
            //Include any related entities
            MessageTemplateModel? recordToUpdate = await _context.MessageTemplate!
                .FirstOrDefaultAsync(m => m.MessageTemplateID == updatedMessageTemplate!.MessageTemplateID);

            if (recordToUpdate == null)
                return new ModelResultModel() { IsSuccessful = false };

            //Update IDs on related entities
            //Need to get full related entity as only the ID is set in the updated record so causes the rest of the fields to be wiped out
            //None

            _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedMessageTemplate!);

            //Update content of related entities
            //None

            //Ensures related entities are included in the save operation
            _context?.Update(recordToUpdate);

            if (save != false)
                await _context!.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> UpdateMany(List<MessageTemplateModel>? updatedMessageTemplates)
        {
            if (updatedMessageTemplates is null)
                return new ModelResultModel() { IsSuccessful = false };

            foreach (var updatedMessageTemplate in updatedMessageTemplates)
            {
                await Update(updatedMessageTemplate, false);
            }

            //Save all changes at the end to avoid multiple save operations
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
