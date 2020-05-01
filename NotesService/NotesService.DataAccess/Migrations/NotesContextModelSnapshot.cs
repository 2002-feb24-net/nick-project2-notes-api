﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotesService.DataAccess.Model;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NotesService.DataAccess.Migrations
{
    [DbContext(typeof(NotesContext))]
    partial class NotesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("NotesService.DataAccess.Model.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TagName")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("TagName");

                    b.ToTable("Notes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 4,
                            DateModified = new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Text = "REST stands for representational state transfer"
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 3,
                            DateModified = new DateTime(2020, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Text = "C# is an OOP language"
                        });
                });

            modelBuilder.Entity("NotesService.DataAccess.Model.NoteTag", b =>
                {
                    b.Property<int>("NoteId")
                        .HasColumnType("integer");

                    b.Property<string>("TagName")
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("NoteId", "TagName");

                    b.HasIndex("TagName");

                    b.ToTable("NoteTags");

                    b.HasData(
                        new
                        {
                            NoteId = 1,
                            TagName = "services",
                            Order = 0
                        },
                        new
                        {
                            NoteId = 1,
                            TagName = "basic",
                            Order = 0
                        },
                        new
                        {
                            NoteId = 2,
                            TagName = "basic",
                            Order = 0
                        });
                });

            modelBuilder.Entity("NotesService.DataAccess.Model.Tag", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Name = "services"
                        },
                        new
                        {
                            Name = "basic"
                        },
                        new
                        {
                            Name = "advanced"
                        });
                });

            modelBuilder.Entity("NotesService.DataAccess.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Angular client"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ASP.NET Core MVC client"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Harold"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Nick"
                        });
                });

            modelBuilder.Entity("NotesService.DataAccess.Model.Note", b =>
                {
                    b.HasOne("NotesService.DataAccess.Model.User", "Author")
                        .WithMany("Notes")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotesService.DataAccess.Model.Tag", null)
                        .WithMany("Notes")
                        .HasForeignKey("TagName");
                });

            modelBuilder.Entity("NotesService.DataAccess.Model.NoteTag", b =>
                {
                    b.HasOne("NotesService.DataAccess.Model.Note", "Note")
                        .WithMany("NoteTags")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotesService.DataAccess.Model.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
