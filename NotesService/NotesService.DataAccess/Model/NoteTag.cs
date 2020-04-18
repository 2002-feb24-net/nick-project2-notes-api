namespace NotesService.DataAccess.Model
{
    public class NoteTag
    {
        public int NoteId { get; set; }

        public string TagName { get; set; }

        public int Order { get; set; }

        public Note Note { get; set; }

        public Tag Tag { get; set; }
    }
}
