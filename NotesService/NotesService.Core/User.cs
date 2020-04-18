using System.Collections.Generic;

namespace NotesService.Core
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Note> Notes { get; set; }
    }
}
