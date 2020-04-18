﻿using System.Collections.Generic;

namespace NotesService.DataAccess.Model
{
    public class Tag
    {
        public string Name { get; set; }

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
