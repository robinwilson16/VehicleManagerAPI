using VehicleManagerAPI.Models;
using VehicleManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace VehicleManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _noteService;

        private ModelResultModel? _modelResult;

        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public ActionResult<List<NoteModel>?> GetAll() =>
            _noteService.GetAll();

        [HttpGet("{noteID}")]
        public ActionResult<NoteModel> Get(int noteID)
        {
            var note = _noteService.Get(noteID);

            if (note == null)
                return NotFound();

            return note;
        }

        [HttpGet("Vehicle/{vehicleID}")]
        public ActionResult<List<NoteModel>> GetByVehicle(int vehicleID)
        {
            var notes = _noteService.GetByVehicle(vehicleID);

            if (notes == null)
                return NotFound();

            return notes;
        }

        [HttpGet("Customer/{customerEmail}")]
        public ActionResult<List<NoteModel>> GetByCustomer(string customerEmail)
        {
            var notes = _noteService.GetByCustomer(customerEmail);

            if (notes == null)
                return NotFound();

            return notes;
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteModel newNote)
        {
            //Check this record does not already exist
            var existingRecord = _noteService.Get(newNote.NoteID);

            if (existingRecord is not null)
                return BadRequest("Record Exists");

            _modelResult = await _noteService.Add(newNote);

            return CreatedAtAction(nameof(Create), new { newNote.NoteID }, newNote);
        }

        [HttpPost("Many")]
        public async Task<IActionResult> CreateMany(List<NoteModel> newNotes)
        {
            _modelResult = await _noteService.AddMany(newNotes);

            var ids = string.Join(",", newNotes.Select(t => t.NoteID));

            return CreatedAtAction(nameof(CreateMany), new { ids }, newNotes);
        }

        [HttpPut("{noteID}")]
        public async Task<IActionResult> Update(int noteID, NoteModel updatedNote)
        {
            if (noteID != updatedNote.NoteID)
                return BadRequest();

            var existingRecord = _noteService.Get(noteID);
            if (existingRecord is null)
                return NotFound();

            await _noteService.Update(updatedNote);

            return AcceptedAtAction(nameof(Update), new { }, updatedNote);
        }

        [HttpPut("Many/{vehicleID}")]
        public async Task<IActionResult> UpdateMany(int vehicleID, List<NoteModel> updatedNotes)
        {
            if (updatedNotes == null)
                return BadRequest();

            var existingRecords = _noteService.GetByVehicle(vehicleID);
            if (existingRecords is null)
                return NotFound();

            await _noteService.UpdateMany(vehicleID, updatedNotes);

            return AcceptedAtAction(nameof(UpdateMany), new { }, updatedNotes);
        }

        [HttpDelete("{messageID}")]
        public async Task<IActionResult> Delete(int noteID)
        {
            var recordToDelete = _noteService.Get(noteID);

            if (recordToDelete is null)
                return NotFound();

            await _noteService.Delete(noteID);

            return AcceptedAtAction(nameof(Delete), new { }, recordToDelete);
        }

        [HttpDelete("Many/{vehicleID}")]
        public async Task<IActionResult> DeleteMany(int vehicleID, List<NoteModel> notesToDelete)
        {
            if (notesToDelete == null)
                return BadRequest();

            //Make sure records exist
            var recordsToDelete = _noteService.GetByVehicle(vehicleID);

            if (recordsToDelete is null)
                return NotFound();

            await _noteService.DeleteMany(vehicleID, notesToDelete);

            return AcceptedAtAction(nameof(DeleteMany), new { }, notesToDelete);
        }

        [HttpDelete("All/{vehicleID}")]
        public async Task<IActionResult> DeleteAll(int? vehicleID)
        {
            if (vehicleID is null)
                return NotFound();

            await _noteService.DeleteAll((int)vehicleID);

            return NoContent();
        }
    }
}
