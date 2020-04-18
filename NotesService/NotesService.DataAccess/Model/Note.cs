using System;
using System.Collections.Generic;

namespace NotesService.DataAccess.Model
{
    public class Note
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Text { get; set; }

        public DateTime DateModified { get; set; }

        public User Author { get; set; }

        public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }
}
