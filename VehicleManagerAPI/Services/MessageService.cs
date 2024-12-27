using VehicleManagerAPI.Data;
using VehicleManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ModelResultModel> Add(MessageModel newMessage)
        {
            _context.Message?.Add(newMessage);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> AddMany(List<MessageModel> newMessages)
        {
            await _context.Message?.AddRangeAsync(newMessages)!;
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> Update(MessageModel? updatedMessage)
        {
            MessageModel? recordToUpdate = _context.Message!
                .FirstOrDefault(m => m.MessageID == updatedMessage!.MessageID);

            if (_context.Message == null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedMessage!);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> UpdateMany(int vehicleID, List<MessageModel>? updatedMessages)
        {
            if (updatedMessages is null)
                return new ModelResultModel() { IsSuccessful = false };

            foreach (var updatedMessage in updatedMessages)
            {
                MessageModel? recordToUpdate = _context.Message!
                    .FirstOrDefault(c => c.MessageID == updatedMessage.MessageID);
                if (_context.Message == null)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
                else if (recordToUpdate?.Vehicle?.SubmissionID != vehicleID)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
                _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedMessage);
            }

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
