using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesService.Api.ApiModels;
using NotesService.Core.Interfaces;

namespace NotesService.Api.Controllers
{
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;

        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // GET: api/notes
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Note>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync([FromQuery] DateTime? since = null)
        {
            IEnumerable<Core.Note> notes = await _noteRepository.GetNotesAsync(since);

            IEnumerable<Note> resource = notes.Select(Mapper.MapNote);
            return Ok(resource);
        }

        // GET: api/notes/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (await _noteRepository.GetNoteAsync(id) is Core.Note note)
            {
                Note resource = Mapper.MapNote(note);
                return Ok(resource);
            }
            return NotFound();
        }

        // POST: api/notes
        [HttpPost]
        [ProducesResponseType(typeof(Note), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync(NewNote newNote)
        {
            if (await _noteRepository.GetUserAsync(newNote.AuthorId) is Core.User author)
            {
                var note = new Core.Note
                {
                    Author = author,
                    Text = newNote.Text,
                    Tags = newNote.Tags
                };

                Core.Note result = await _noteRepository.AddNoteAsync(note);

                Note resource = Mapper.MapNote(result);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = note.Id }, resource);
            }

            ModelState.AddModelError(nameof(newNote.AuthorId), "User does not exist");
            return BadRequest(ModelState);
        }

        // DELETE: api/notes/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _noteRepository.RemoveNoteAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
