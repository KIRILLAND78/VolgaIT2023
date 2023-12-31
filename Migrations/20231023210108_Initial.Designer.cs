﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VolgaIT2023;

#nullable disable

namespace VolgaIT2023.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20231023210108_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VolgaIT2023.Models.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double>("Balance")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("VolgaIT2023.Models.Rent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double?>("FinalPrice")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceOfUnit")
                        .HasColumnType("double precision");

                    b.Property<string>("PriceType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TimeEnd")
                        .HasColumnType("text");

                    b.Property<string>("TimeStart")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("TransportId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TransportId");

                    b.HasIndex("UserId");

                    b.ToTable("Rents", t =>
                        {
                            t.HasCheckConstraint("Rents", "\"PriceType\" IN ('Days','Minutes')");
                        });
                });

            modelBuilder.Entity("VolgaIT2023.Models.Session", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("ExpireTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("VolgaIT2023.Models.Transport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("CanBeRented")
                        .HasColumnType("boolean");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double?>("DayPrice")
                        .HasColumnType("double precision");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("MinutePrice")
                        .HasColumnType("double precision");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<string>("TransportType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Transports", t =>
                        {
                            t.HasCheckConstraint("Transports", "\"TransportType\" IN ('Bike','Car','Scooter')");
                        });
                });

            modelBuilder.Entity("VolgaIT2023.Models.Rent", b =>
                {
                    b.HasOne("VolgaIT2023.Models.Transport", "RentedTransport")
                        .WithMany("Rents")
                        .HasForeignKey("TransportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VolgaIT2023.Models.Account", "User")
                        .WithMany("Rents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RentedTransport");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VolgaIT2023.Models.Session", b =>
                {
                    b.HasOne("VolgaIT2023.Models.Account", "Owner")
                        .WithMany("Sessions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("VolgaIT2023.Models.Transport", b =>
                {
                    b.HasOne("VolgaIT2023.Models.Account", "Owner")
                        .WithMany("Transports")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("VolgaIT2023.Models.Account", b =>
                {
                    b.Navigation("Rents");

                    b.Navigation("Sessions");

                    b.Navigation("Transports");
                });

            modelBuilder.Entity("VolgaIT2023.Models.Transport", b =>
                {
                    b.Navigation("Rents");
                });
#pragma warning restore 612, 618
        }
    }
}
