﻿// <auto-generated />
using System;
using Ispit.Todo_GoranKolesar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ispit.Todo_GoranKolesar.Data.Migrations
{
    [DbContext(typeof(ToDoContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Ispit.Todo_GoranKolesar.Models.Category", b =>
                {
                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = "work",
                            Name = "Work"
                        },
                        new
                        {
                            CategoryId = "home",
                            Name = "Home"
                        },
                        new
                        {
                            CategoryId = "ex",
                            Name = "Exercise"
                        },
                        new
                        {
                            CategoryId = "shop",
                            Name = "Shopping"
                        },
                        new
                        {
                            CategoryId = "call",
                            Name = "Contact"
                        });
                });

            modelBuilder.Entity("Ispit.Todo_GoranKolesar.Models.Status", b =>
                {
                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatusId");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            StatusId = "open",
                            Name = "Open"
                        },
                        new
                        {
                            StatusId = "closed",
                            Name = "Completed"
                        });
                });

            modelBuilder.Entity("Ispit.Todo_GoranKolesar.Models.ToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DueDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("StatusId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StatusId");

                    b.ToTable("ToDos");
                });

            modelBuilder.Entity("Ispit.Todo_GoranKolesar.Models.ToDo", b =>
                {
                    b.HasOne("Ispit.Todo_GoranKolesar.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ispit.Todo_GoranKolesar.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Status");
                });
#pragma warning restore 612, 618
        }
    }
}
