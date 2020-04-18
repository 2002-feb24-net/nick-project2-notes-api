using System;
using System.Collections.Generic;

namespace NotesService.Core
{
    public class Note
    {
        public int Id { get; set; }

        public User Author { get; set; }

        public string Text { get; set; }

        public DateTime DateModified { get; set; } = DateTime.Now;

        public IList<string> Tags { get; set; } = new List<string>();
    }
}
