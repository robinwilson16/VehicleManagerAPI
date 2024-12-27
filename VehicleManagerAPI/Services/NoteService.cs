using VehicleManagerAPI.Data;
using VehicleManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VehicleManagerAPI.Services
{
    public class NoteService
    {
        private readonly ApplicationDbContext _context;


        public List<NoteModel>? Notes { get; }

        public NoteService(ApplicationDbContext context)
        {
            _context = context;

            Notes = _context.Note!
                .ToList();
        }

        public List<NoteModel>? GetAll() => Notes;

        public NoteModel? Get(int noteID) => Notes?.FirstOrDefault(c => c.NoteID == noteID);

        public List<NoteModel>? GetByVehicle(int vehicleID) => Notes?
            .Where(m => m.Vehicle?.SubmissionID == vehicleID)
            .ToList();

        public List<NoteModel>? GetByCustomer(string emailAddress) => Notes?
            .Where(m => m.Customer?.CustomerEmail == emailAddress)
            .ToList();

        public async Task<ModelResultModel> Add(NoteModel newNote)
        {
            _context.Note?.Add(newNote);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> AddMany(List<NoteModel> newNotes)
        {
            await _context.Note?.AddRangeAsync(newNotes)!;
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> Update(NoteModel? updatedNote)
        {
            NoteModel? recordToUpdate = _context.Note!
                .FirstOrDefault(m => m.NoteID == updatedNote!.NoteID);

            if (_context.Note == null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedNote!);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> UpdateMany(int vehicleID, List<NoteModel>? updatedNotes)
        {
            if (updatedNotes is null)
                return new ModelResultModel() { IsSuccessful = false };

            foreach (var updatedNote in updatedNotes)
            {
                NoteModel? recordToUpdate = _context.Note!
                    .FirstOrDefault(c => c.NoteID == updatedNote.NoteID);
                if (_context.Note == null)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
                else if (recordToUpdate?.Vehicle?.SubmissionID != vehicleID)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
                _context.Entry(recordToUpdate!).CurrentValues.SetValues(updatedNote);
            }

            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> Delete(int noteID)
        {
            var recordToDelete = Get(noteID);
            if (recordToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.Note!.Remove(recordToDelete);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> DeleteMany(int vehicleID, List<NoteModel>? notesToDelete)
        {
            if (notesToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            //Check records belong to this vehicle - extra security step
            foreach (var noteToDelete in notesToDelete)
            {
                NoteModel? recordToDelete = _context.Note!
                    .FirstOrDefault(m => m.NoteID == noteToDelete.NoteID);
                if (_context.Note == null)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
                else if (recordToDelete?.Vehicle?.SubmissionID != vehicleID)
                {
                    return new ModelResultModel() { IsSuccessful = false };
                }
            }

            _context.Note!.RemoveRange(notesToDelete);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }

        public async Task<ModelResultModel> DeleteAll(int vehicleID)
        {
            var recordsToDelete = GetByVehicle(vehicleID);
            if (recordsToDelete is null)
                return new ModelResultModel() { IsSuccessful = false };

            _context.Note!.RemoveRange(recordsToDelete);
            await _context.SaveChangesAsync();

            return new ModelResultModel() { IsSuccessful = true };
        }
    }
}
