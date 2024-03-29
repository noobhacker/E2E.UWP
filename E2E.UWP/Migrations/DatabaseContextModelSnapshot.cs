﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using E2E.UWP.Models;

namespace E2E.UWP.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("E2E.UWP.Models.MessageFrequency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Frequency");

                    b.Property<string>("Message");

                    b.HasKey("Id");

                    b.ToTable("MessageFrequencies");
                });

            modelBuilder.Entity("E2E.UWP.Models.WordFrequency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Frequency");

                    b.Property<string>("Word");

                    b.HasKey("Id");

                    b.ToTable("WordFrequencies");
                });
        }
    }
}
