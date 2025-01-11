﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VehicleManagerAPI.Data;

#nullable disable

namespace VehicleManagerAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250110005132_Add Processed Message field to Message")]
    partial class AddProcessedMessagefieldtoMessage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VehicleManagerAPI.Models.ConfigModel", b =>
                {
                    b.Property<string>("ConfigID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasIndex("ConfigID")
                        .IsUnique();

                    b.ToTable("Config");
                });

            modelBuilder.Entity("VehicleManagerAPI.Models.MessageModel", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageID"));

                    b.Property<decimal?>("AmountOffered")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("BCC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("From")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsSent")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("MessageGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("MessageIsHTML")
                        .HasColumnType("bit");

                    b.Property<string>("MessageProcessed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MessageStatusID")
                        .HasColumnType("int");

                    b.Property<int?>("MessageTemplateID")
                        .HasColumnType("int");

                    b.Property<int?>("MessageTypeID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("To")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VehicleID")
                        .HasColumnType("int");

                    b.HasKey("MessageID");

                    b.HasIndex("MessageTemplateID");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("VehicleManagerAPI.Models.MessageTemplateModel", b =>
                {
                    b.Property<int>("MessageTemplateID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageTemplateID"));

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Sequence")
                        .HasColumnType("int");

                    b.Property<string>("TemplateContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateSubject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MessageTemplateID");

                    b.ToTable("MessageTemplate");
                });

            modelBuilder.Entity("VehicleManagerAPI.Models.NoteModel", b =>
                {
                    b.Property<int>("NoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NoteID"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsAlert")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoteText")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NoteID");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("VehicleManagerAPI.Models.MessageModel", b =>
                {
                    b.HasOne("VehicleManagerAPI.Models.MessageTemplateModel", "MessageTemplate")
                        .WithMany("Messages")
                        .HasForeignKey("MessageTemplateID");

                    b.Navigation("MessageTemplate");
                });

            modelBuilder.Entity("VehicleManagerAPI.Models.MessageTemplateModel", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
