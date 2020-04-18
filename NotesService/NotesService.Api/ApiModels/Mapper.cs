using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesService.Api.ApiModels
{
    public static class Mapper
    {
        public static Note MapNote(Core.Note note)
        {
            return new Note
            {
                Id = note.Id,
                AuthorId = note.Author.Id,
                Text = note.Text,
                DateModified = note.DateModified,
                Tags = note.Tags.ToList()
            };
        }
    }
}
