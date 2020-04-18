using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesService.Api.ApiModels;
using NotesService.Core.Interfaces;

namespace NotesService.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;

        public UsersController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<Core.User> users = await _noteRepository.GetUsersAsync();

            IEnumerable<User> resource = users.Select(u => new User
            {
                Id = u.Id,
                Name = u.Name
            });

            return Ok(resource);
        }

        [HttpGet("{userId}/notes")]
        [ProducesResponseType(typeof(IEnumerable<Note>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotesAsync(int userId)
        {
            if (!await _noteRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            IEnumerable<Core.Note> notes = await _noteRepository.GetNotesAsync(authorId: userId);

            IEnumerable<Note> resource = notes.Select(Mapper.MapNote);
            return Ok(resource);
        }
    }
}
