using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotesService.Core.Interfaces;
using NotesService.DataAccess.Model;

namespace NotesService.DataAccess
{
    public class NoteRepository : INoteRepository
    {
        private readonly NotesContext _context;

        public NoteRepository(NotesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Core.Note>> GetNotesAsync(DateTime? since = null, int? authorId = null)
        {
            IQueryable<Note> query = _context.Notes
                .Include(n => n.Author)
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag);

            if (authorId != null)
            {
                query = query.Where(n => n.Author.Id == authorId);
            }
            if (since.HasValue)
            {
                query = query.Where(n => n.DateModified >= since);
            }

            List<Note> notes = await query
                .OrderByDescending(n => n.DateModified)
                .ToListAsync();

            return notes.Select(MapNoteWithAuthorAndTags);
        }

        public async Task<Core.Note> GetNoteAsync(int id)
        {
            Note note = await _context.Notes
                .Include(n => n.Author)
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id);

            return MapNoteWithAuthorAndTags(note);
        }

        public async Task<IEnumerable<Core.User>> GetUsersAsync()
        {
            List<User> users = await _context.Users.ToListAsync();

            return users.Select(MapUser);
        }

        public async Task<Core.User> GetUserAsync(int id)
        {
            User user = await _context.Users.FindAsync(id);
            return MapUser(user);
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public Task<Core.Note> AddNoteAsync(Core.Note note)
        {
            if (note.Author is null)
            {
                throw new ArgumentException("Note must have author", nameof(note));
            }

            return AddNoteInternalAsync(note);

            async Task<Core.Note> AddNoteInternalAsync(Core.Note note)
            {
                User author = await _context.Users.FindAsync(note.Author.Id);

                if (author is null)
                {
                    throw new ArgumentException("Author does not exist", nameof(note));
                }

                List<string> tags = await _context.Tags.Select(t => t.Name).ToListAsync();

                var newNote = new Note
                {
                    Id = note.Id,
                    Author = author,
                    Text = note.Text
                };

                _context.Notes.Add(newNote);

                ISet<string> tagSet = tags.ToHashSet();
                for (int i = 0; i < note.Tags.Count; i++)
                {
                    if (!tagSet.Contains(note.Tags[i]))
                    {
                        _context.Tags.Add(new Tag { Name = note.Tags[i] });
                    }

                    newNote.NoteTags.Add(new NoteTag { TagName = note.Tags[i], Order = i });
                }

                await _context.SaveChangesAsync();

                return MapNoteWithAuthorAndTags(newNote);
            }
        }

        public async Task<bool> RemoveNoteAsync(int id)
        {
            Note note = await _context.Notes.FindAsync(id);

            if (note is null)
            {
                return false;
            }

            _context.Notes.Remove(note);
            int written = await _context.SaveChangesAsync();

            return written > 0;
        }

        private static Core.Note MapNoteWithAuthorAndTags(Note note)
        {
            return note is null ? null : new Core.Note
            {
                Id = note.Id,
                Author = new Core.User
                {
                    Id = note.Author.Id,
                    Name = note.Author.Name
                },
                Text = note.Text,
                DateModified = note.DateModified,
                Tags = note.NoteTags
                    .OrderBy(nt => nt.Order)
                    .Select(nt => nt.Tag.Name)
                    .ToList()
            };
        }

        private static Core.User MapUser(User user)
        {
            return user is null ? null : new Core.User
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
