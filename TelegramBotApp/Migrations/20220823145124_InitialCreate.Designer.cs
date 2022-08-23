﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TelegramBotApp.Database;

#nullable disable

namespace TelegramBotApp.Migrations
{
    [DbContext(typeof(LocationMessageDbContext))]
    [Migration("20220823145124_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TelegramBotApp.Model.LocationMessageModel", b =>
                {
                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("MessageText")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("MessageTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId");

                    b.ToTable("locationMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
