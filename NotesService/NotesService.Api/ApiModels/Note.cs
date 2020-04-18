using System;
using System.Collections.Generic;

namespace NotesService.Api.ApiModels
{
    public class Note
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Text { get; set; }

        public DateTime DateModified { get; set; } = DateTime.Now;

        public List<string> Tags { get; set; }
    }
}
