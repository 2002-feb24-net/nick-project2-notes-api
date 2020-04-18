using System;
using NotesService.Core;
using Xunit;

namespace NotesService.UnitTests.Core
{
    public class NoteTests
    {
        [Fact]
        public void NoteHasDateModifiedDefaultNow()
        {
            var before = DateTime.Now;

            var note = new Note();

            var after = DateTime.Now;
            Assert.InRange(note.DateModified, before, after);
        }
    }
}
