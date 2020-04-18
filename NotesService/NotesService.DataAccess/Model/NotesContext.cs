using System;
using Microsoft.EntityFrameworkCore;

namespace NotesService.DataAccess.Model
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions<NotesContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<NoteTag> NoteTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .Property(n => n.Name)
                .IsRequired();

            modelBuilder.Entity<Note>()
                .Property(n => n.Text)
                .IsRequired();

            modelBuilder.Entity<Note>()
                .Property(n => n.DateModified)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()");

            modelBuilder.Entity<Tag>()
                .HasKey(n => n.Name);

            modelBuilder.Entity<NoteTag>()
                .HasKey(nt => new { nt.NoteId, nt.TagName });

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Harold" },
                new User { Id = 2, Name = "Nick" });

            modelBuilder.Entity<Note>().HasData(
                new Note { Id = 1, AuthorId = 2, Text = "REST stands for representational state transfer", DateModified = new DateTime(2020, 4, 1) },
                new Note { Id = 2, AuthorId = 1, Text = "C# is an OOP language", DateModified = new DateTime(2020, 4, 9) });

            modelBuilder.Entity<Tag>().HasData(
                new Tag { Name = "services" },
                new Tag { Name = "basic" },
                new Tag { Name = "advanced" });

            modelBuilder.Entity<NoteTag>().HasData(
                new NoteTag { NoteId = 1, TagName = "services" },
                new NoteTag { NoteId = 1, TagName = "basic" },
                new NoteTag { NoteId = 2, TagName = "basic" });
        }
    }
}
