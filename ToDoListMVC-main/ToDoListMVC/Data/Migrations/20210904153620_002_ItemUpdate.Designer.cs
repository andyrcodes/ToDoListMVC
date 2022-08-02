﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ToDoListMVC.Data;

namespace ToDoListMVC.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210904153620_002_ItemUpdate")]
    partial class _002_ItemUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ToDoListMVC.Models.ToDoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("boolean");

                    b.Property<int>("ListId")
                        .HasColumnType("integer");

                    b.Property<string>("Task")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ToDoListMVC.Models.ToDoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("ToDoListMVC.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ToDoListMVC.Models.ToDoItem", b =>
                {
                    b.HasOne("ToDoListMVC.Models.ToDoList", "List")
                        .WithMany("Items")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");
                });

            modelBuilder.Entity("ToDoListMVC.Models.ToDoList", b =>
                {
                    b.HasOne("ToDoListMVC.Models.User", "User")
                        .WithMany("Lists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ToDoListMVC.Models.ToDoList", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ToDoListMVC.Models.User", b =>
                {
                    b.Navigation("Lists");
                });
#pragma warning restore 612, 618
        }
    }
}
