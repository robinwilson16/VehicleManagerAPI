using VehicleManagerAPI.Models;
using VehicleManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace VehicleManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageTemplateController : ControllerBase
    {
        private readonly MessageTemplateService _messageTemplateService;

        private ModelResultModel? _modelResult;

        public MessageTemplateController(MessageTemplateService messageTemplateService)
        {
            _messageTemplateService = messageTemplateService;
        }

        [HttpGet]
        public ActionResult<List<MessageTemplateModel>?> GetAll() =>
            _messageTemplateService.GetAll();

        [HttpGet("{messageTemplateID}")]
        public ActionResult<MessageTemplateModel> Get(int messageTemplateID)
        {
            var messageTemplate = _messageTemplateService.Get(messageTemplateID);

            if (messageTemplate == null)
                return NotFound();

            return messageTemplate;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageTemplateModel newMessageTemplate)
        {
            //Check this record does not already exist
            var existingRecord = _messageTemplateService.Get(newMessageTemplate.MessageTemplateID);

            if (existingRecord is not null)
                return BadRequest("Record Exists");

            _modelResult = await _messageTemplateService.Add(newMessageTemplate);

            return CreatedAtAction(nameof(Create), new { newMessageTemplate.MessageTemplateID }, newMessageTemplate);
        }

        [HttpPost("Many")]
        public async Task<IActionResult> CreateMany(List<MessageTemplateModel> newMessageTemplates)
        {
            _modelResult = await _messageTemplateService.AddMany(newMessageTemplates);
            var ids = string.Join(",", newMessageTemplates.Select(t => t.MessageTemplateID));

            return CreatedAtAction(nameof(CreateMany), new { ids }, newMessageTemplates);
        }

        [HttpPut("{messageTemplateID}")]
        public async Task<IActionResult> Update(int messageTemplateID, MessageTemplateModel updatedMessageTemplate)
        {
            if (messageTemplateID != updatedMessageTemplate.MessageTemplateID)
                return BadRequest();

            var existingRecord = _messageTemplateService.Get(messageTemplateID);
            if (existingRecord is null)
                return NotFound();

            await _messageTemplateService.Update(updatedMessageTemplate);

            return AcceptedAtAction(nameof(Update), new { }, updatedMessageTemplate);
        }

        [HttpPut("Many")]
        public async Task<IActionResult> UpdateMany(List<MessageTemplateModel> updatedMessageTemplates)
        {
            if (updatedMessageTemplates == null)
                return BadRequest();

            await _messageTemplateService.UpdateMany(updatedMessageTemplates);

            return AcceptedAtAction(nameof(UpdateMany), new { }, updatedMessageTemplates);
        }

        [HttpDelete("{messageTemplateID}")]
        public async Task<IActionResult> Delete(int messageTemplateID)
        {
            var recordToDelete = _messageTemplateService.Get(messageTemplateID);

            if (recordToDelete is null)
                return NotFound();

            await _messageTemplateService.Delete(messageTemplateID);

            return AcceptedAtAction(nameof(Delete), new { }, recordToDelete);
        }

        [HttpDelete("Many")]
        public async Task<IActionResult> DeleteMany(int vehicleID, List<MessageTemplateModel> messageTemplatesToDelete)
        {
            if (messageTemplatesToDelete == null)
                return BadRequest();

            await _messageTemplateService.DeleteMany(messageTemplatesToDelete);

            return AcceptedAtAction(nameof(DeleteMany), new { }, messageTemplatesToDelete);
        }
    }
}
