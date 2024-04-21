using System.Net;
using Microsoft.AspNetCore.Mvc;
using NOS.Engineering.Challenge.API.Models;
using NOS.Engineering.Challenge.Managers;
using Microsoft.Extensions.Logging;

namespace NOS.Engineering.Challenge.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContentController : Controller
    {
        private readonly IContentsManager _manager;
        private readonly ILogger<ContentController> _logger;

        public ContentController(IContentsManager manager, ILogger<ContentController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetManyContents()
        {
            _logger.LogInformation("Getting many contents.");

            var contents = await _manager.GetManyContents().ConfigureAwait(false);

            if (!contents.Any())
            {
                _logger.LogWarning("No contents found.");
                return NotFound();
            }

            _logger.LogInformation("Returning {ContentCount} contents.", contents.Count());
            return Ok(contents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent(Guid id)
        {
            _logger.LogInformation("Getting content with ID {ContentId}.", id);

            var content = await _manager.GetContent(id).ConfigureAwait(false);

            if (content == null)
            {
                _logger.LogWarning("Content with ID {ContentId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Returning content with ID {ContentId}.", id);
            return Ok(content);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent(
            [FromBody] ContentInput content
        )
        {
            _logger.LogInformation("Creating new content.");

            var createdContent = await _manager.CreateContent(content.ToDto()).ConfigureAwait(false);

            if (createdContent == null)
            {
                _logger.LogError("Failed to create new content.");
                return Problem();
            }

            _logger.LogInformation("Created new content with ID {ContentId}.", createdContent.Id);
            return Ok(createdContent);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateContent(
            Guid id,
            [FromBody] ContentInput content
        )
        {
            _logger.LogInformation("Updating content with ID {ContentId}.", id);

            var updatedContent = await _manager.UpdateContent(id, content.ToDto()).ConfigureAwait(false);

            if (updatedContent == null)
            {
                _logger.LogWarning("Content with ID {ContentId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Updated content with ID {ContentId}.", id);
            return Ok(updatedContent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(
            Guid id
        )
        {
            _logger.LogInformation("Deleting content with ID {ContentId}.", id);

            var deletedId = await _manager.DeleteContent(id).ConfigureAwait(false);

            if (deletedId == null)
            {
                _logger.LogWarning("Content with ID {ContentId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Deleted content with ID {ContentId}.", id);
            return Ok(deletedId);
        }

        [HttpPost("{id}/genre")]
        public async Task<IActionResult> AddGenres(Guid id, [FromBody] IEnumerable<string> genres)
        {
            _logger.LogInformation("Adding genres to content with ID {ContentId}.", id);

            var contentInput = new ContentInput
            {
                GenreList = genres
            };

            var updatedContent = await _manager.UpdateContent(id, contentInput.ToDto()).ConfigureAwait(false);

            if (updatedContent == null)
            {
                _logger.LogWarning("Content with ID {ContentId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Added genres to content with ID {ContentId}.", id);
            return Ok(updatedContent);
        }

        [HttpDelete("{id}/genre")]
        public async Task<IActionResult> DeleteGenres(Guid id, [FromBody] IEnumerable<string> genres)
        {
            _logger.LogInformation("Deleting genres from content with ID {ContentId}.", id);

            var contentInput = new ContentInput
            {
                GenreList = genres
            };

            var updatedContent = await _manager.UpdateContent(id, contentInput.ToDto()).ConfigureAwait(false);

            if (updatedContent == null)
            {
                _logger.LogWarning("Content with ID {ContentId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Deleted genres from content with ID {ContentId}.", id);
            return Ok(updatedContent);
        }
    }
}