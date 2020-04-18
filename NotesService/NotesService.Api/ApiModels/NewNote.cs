using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotesService.Api.ApiModels
{
    public class NewNote
    {
        public int AuthorId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public List<string> Tags { get; set; }
    }
}
