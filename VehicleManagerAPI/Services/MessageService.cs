using VehicleManagerAPI.Data;
using VehicleManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Runtime.Intrinsics.Arm;

namespace VehicleManagerAPI.Services
{
    public class MessageService
    {
        private readonly ApplicationDbContext _context;


        public List<MessageModel>? Messages { get; }

        public MessageService(ApplicationDbContext context)
        {
            _context = context;

            Messages = _context.Message!
                .Include(m => m.MessageTemplate)
                .ToList();
        }

        public List<MessageModel>? GetAll() => Messages;

        public MessageModel? Get(int messageID) => Messages?.FirstOrDefault(c => c.MessageID == messageID);

        public List<MessageModel>? GetByVehicle(int vehicleID) => Messages?
            .Where(m => m.Vehicle?.SubmissionID == vehicleID)
            .ToList();

        public List<MessageModel>? GetBySender(string emailAddress) => Messages?
            .Where(m => m.From == emailAddress)
            .ToList();

        public List<MessageModel>? GetByRecipient(string emailAddress) => Messages?
            .Where(m => m.To == emailAddress || m.CC == emailAddress || m.BCC == emailAddress)
            .ToList();

        public MessageModel? GetByGUID(Guid messageGUID) => Messages?.FirstOrDefault(c => c.MessageGUID == messageGUID);

        public async Task<ModelResultModel> Add(MessageModel newMessage)
        {
            if (newMessage?.MessageTemplate != null)
            {
                var existingTemplate = await _context.MessageTemplate!
                    .FirstOrDefaultAsync(t => t.MessageTemplateID == newMessage.MessageTemplate.MessageTemplateID);

                if (existingTemplate != null)
                {
                    //Attach the existing MessageTemplate to the context instead of trying to insert a new one
                    _context.Entry(existingTemplate).State = EntityState.Unchanged;

                    //Attach the existing MessageTemplate to the model so it is returned by the API in full rather than just the ID
                    newMessage.MessageTemplate = existingTemplate;
                }
            }

            _context.Message?.Add(newMessage);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> AddMany(List<MessageModel> newMessages)
        {
            if (newMessages != null)
            {
                foreach (var newMessage in newMessages)
                {
                    if (newMessage.MessageTemplate != null)
                    {
                        // Attach the existing MessageTemplate to the context instead of trying to insert a new one
                        _context.Entry(newMessage.MessageTemplate).State = EntityState.Unchanged;
                    }
                }
            }
            
            await _context.Message?.AddRangeAsync(newMessages)!;
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> Update(MessageModel updatedMessage, bool? save)
        {
            //Include any related entities
            MessageModel? recordToUpdate = await _context.Message!
                .Include(m => m.MessageTemplate)
                .FirstOrDefaultAsync(m => m.MessageID == updatedMessage!.MessageID);

            if (recordToUpdate == null)
                return new ModelResultModel() { IsSuccessful = false };

            if (updatedMessage?.MessageTemplate != null)
            {
                var existingTemplate = await _context.MessageTemplate!
                    .FirstOrDefaultAsync(t => t.MessageTemplateID == updatedMessage.MessageTemplate.MessageTemplateID);

                if (existingTemplate != null)
                {
                    // Attach the existing MessageTemplate to the context
                    _context.Entry(existingTemplate).State = EntityState.Unchanged;
                    recordToUpdate.MessageTemplate = existingTemplate;
                }
            }

            _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedMessage!);

            //Ensures related entities are included in the save operation
            _context?.Update(recordToUpdate);

            if (save != false)
                await _context!.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> UpdateMany(int vehicleID, List<MessageModel>? updatedMessages)
        {
            if (updatedMessages is null)
                return new ModelResultModel() { IsSuccessful = false };

            foreach (var updatedMessage in updatedMessages)
            {
                await Update(updatedMessage, false);
            }

            //Save all changes at the end to avoid multiple save operations
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> Delete(int messageID)
        {
            var recordToDelete = Get(messageID);
            if (recordToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.Message!.Remove(recordToDelete);
            await _context.SaveChangesAsync();

            // Check if the MessageTemplate is no longer referenced by any MessageModel
            //var messageTemplateID = recordToDelete.MessageTemplate?.MessageTemplateID;
            //if (messageTemplateID.HasValue)
            //{
            //    var isTemplateReferenced = await _context.Message!
            //        .AnyAsync(m => m.MessageTemplate!.MessageTemplateID == messageTemplateID.Value);

            //    if (!isTemplateReferenced)
            //    {
            //        var templateToDelete = await _context.MessageTemplate!
            //            .FirstOrDefaultAsync(t => t.MessageTemplateID == messageTemplateID.Value);

            //        if (templateToDelete != null)
            //        {
            //            _context.MessageTemplate!.Remove(templateToDelete);
            //            await _context.SaveChangesAsync();
            //        }
            //    }
            //}

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> DeleteMany(int vehicleID, List<MessageModel>? messagesToDelete)
        {
            if (messagesToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            //Check records belong to this vehicle - extra security step
            foreach (var messageToDelete in messagesToDelete)
            {
                MessageModel? recordToDelete = _context.Message!
                    .FirstOrDefault(m => m.MessageID == messageToDelete.MessageID);
                if (_context.Message == null)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
                else if (recordToDelete?.Vehicle?.SubmissionID != vehicleID)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
            }

            _context.Message!.RemoveRange(messagesToDelete);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> DeleteAll(int vehicleID)
        {
            var recordsToDelete = GetByVehicle(vehicleID);
            if (recordsToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.Message!.RemoveRange(recordsToDelete);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }
    }
}
