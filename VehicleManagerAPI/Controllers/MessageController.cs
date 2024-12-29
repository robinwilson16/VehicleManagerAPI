using VehicleManagerAPI.Models;
using VehicleManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace VehicleManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        private ModelResultModel? _modelResult;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public ActionResult<List<MessageModel>?> GetAll() =>
            _messageService.GetAll();

        [HttpGet("{messageID}")]
        public ActionResult<MessageModel> Get(int messageID)
        {
            var message = _messageService.Get(messageID);

            if (message == null)
                return NotFound();

            return message;
        }

        [HttpGet("Vehicle/{vehicleID}")]
        public ActionResult<List<MessageModel>> GetByVehicle(int vehicleID)
        {
            var messages = _messageService.GetByVehicle(vehicleID);

            if (messages == null)
                return NotFound();

            return messages;
        }

        [HttpGet("Sender/{emailAddress}")]
        public ActionResult<List<MessageModel>> GetBySender(string emailAddress)
        {
            var messages = _messageService.GetBySender(emailAddress);

            if (messages == null)
                return NotFound();

            return messages;
        }

        [HttpGet("Recipient/{emailAddress}")]
        public ActionResult<List<MessageModel>> GetByRecipient(string emailAddress)
        {
            var messages = _messageService.GetByRecipient(emailAddress);

            if (messages == null)
                return NotFound();

            return messages;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageModel newMessage)
        {
            //Check this record does not already exist
            var existingRecord = _messageService.Get(newMessage.MessageID);

            if (existingRecord is not null)
                return BadRequest("Record Exists");

            _modelResult = await _messageService.Add(newMessage);

            return CreatedAtAction(nameof(Create), new { newMessage.MessageID }, newMessage);
        }

        [HttpPost("Many")]
        public async Task<IActionResult> CreateMany(List<MessageModel> newMessages)
        {
            _modelResult = await _messageService.AddMany(newMessages);
            var ids = string.Join(",", newMessages.Select(t => t.MessageID));

            return CreatedAtAction(nameof(CreateMany), new { ids }, newMessages);
        }

        [HttpPut("{messageID}")]
        public async Task<IActionResult> Update(int messageID, MessageModel updatedMessage)
        {
            if (messageID != updatedMessage.MessageID)
                return BadRequest();

            var existingRecord = _messageService.Get(messageID);
            if (existingRecord is null)
                return NotFound();

            await _messageService.Update(updatedMessage, true);

            return AcceptedAtAction(nameof(Update), new { }, updatedMessage);
        }

        [HttpPut("Many/{vehicleID}")]
        public async Task<IActionResult> UpdateMany(int vehicleID, List<MessageModel> updatedMessages)
        {
            if (updatedMessages == null)
                return BadRequest();

            var existingRecords = _messageService.GetByVehicle(vehicleID);
            if (existingRecords is null)
                return NotFound();

            await _messageService.UpdateMany(vehicleID, updatedMessages);

            return AcceptedAtAction(nameof(UpdateMany), new { }, updatedMessages);
        }

        [HttpDelete("{messageID}")]
        public async Task<IActionResult> Delete(int messageID)
        {
            var recordToDelete = _messageService.Get(messageID);

            if (recordToDelete is null)
                return NotFound();

            await _messageService.Delete(messageID);

            return AcceptedAtAction(nameof(Delete), new { }, recordToDelete);
        }

        [HttpDelete("Many/{vehicleID}")]
        public async Task<IActionResult> DeleteMany(int vehicleID, List<MessageModel> messagesToDelete)
        {
            if (messagesToDelete == null)
                return BadRequest();

            //Make sure records exist
            var recordsToDelete = _messageService.GetByVehicle(vehicleID);

            if (recordsToDelete is null)
                return NotFound();

            await _messageService.DeleteMany(vehicleID, messagesToDelete);

            return AcceptedAtAction(nameof(DeleteMany), new { }, messagesToDelete);
        }

        [HttpDelete("All/{vehicleID}")]
        public async Task<IActionResult> DeleteAll(int? vehicleID)
        {
            if (vehicleID is null)
                return NotFound();

            await _messageService.DeleteAll((int)vehicleID);

            return NoContent();
        }
    }
}
