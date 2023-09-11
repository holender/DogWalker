﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(WalkerPlanerDbContext))]
    [Migration("20230911181459_Refactor")]
    partial class Refactor
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("Infrastructure.Data.Dog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Dogs");
                });

            modelBuilder.Entity("Infrastructure.Data.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DogId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DogId");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("Infrastructure.Data.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(4000)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int?>("TrackId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackId");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("Infrastructure.Data.SessionDog", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DogId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SessionId", "DogId");

                    b.HasIndex("DogId");

                    b.ToTable("SessionDog");
                });

            modelBuilder.Entity("Infrastructure.Data.SessionWalker", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WalkerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SessionId", "WalkerId");

                    b.HasIndex("WalkerId");

                    b.ToTable("SessionWalker");
                });

            modelBuilder.Entity("Infrastructure.Data.Track", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("Infrastructure.Data.Walker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bio")
                        .HasMaxLength(4000)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("WebSite")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Walkers");
                });

            modelBuilder.Entity("Infrastructure.Data.Owner", b =>
                {
                    b.HasOne("Infrastructure.Data.Dog", null)
                        .WithMany("Owners")
                        .HasForeignKey("DogId");
                });

            modelBuilder.Entity("Infrastructure.Data.Session", b =>
                {
                    b.HasOne("Infrastructure.Data.Track", "Track")
                        .WithMany("Sessions")
                        .HasForeignKey("TrackId");

                    b.Navigation("Track");
                });

            modelBuilder.Entity("Infrastructure.Data.SessionDog", b =>
                {
                    b.HasOne("Infrastructure.Data.Dog", "Dog")
                        .WithMany("SessionDogs")
                        .HasForeignKey("DogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Data.Session", "Session")
                        .WithMany("SessionDogs")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dog");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Infrastructure.Data.SessionWalker", b =>
                {
                    b.HasOne("Infrastructure.Data.Session", "Session")
                        .WithMany("SessionWalkers")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Data.Walker", "Walker")
                        .WithMany("SessionWalkers")
                        .HasForeignKey("WalkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("Walker");
                });

            modelBuilder.Entity("Infrastructure.Data.Dog", b =>
                {
                    b.Navigation("Owners");

                    b.Navigation("SessionDogs");
                });

            modelBuilder.Entity("Infrastructure.Data.Session", b =>
                {
                    b.Navigation("SessionDogs");

                    b.Navigation("SessionWalkers");
                });

            modelBuilder.Entity("Infrastructure.Data.Track", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Infrastructure.Data.Walker", b =>
                {
                    b.Navigation("SessionWalkers");
                });
#pragma warning restore 612, 618
        }
    }
}
