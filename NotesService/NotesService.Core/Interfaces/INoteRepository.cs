using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesService.Core.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetNotesAsync(DateTime? since = null, int? authorId = null);

        Task<Note> GetNoteAsync(int id);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(int id);

        Task<bool> UserExistsAsync(int id);

        Task<Note> AddNoteAsync(Note note);

        Task<bool> RemoveNoteAsync(int id);
    }
}
