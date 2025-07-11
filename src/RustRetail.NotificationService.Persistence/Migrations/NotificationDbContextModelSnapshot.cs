﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RustRetail.NotificationService.Persistence.Database;

#nullable disable

namespace RustRetail.NotificationService.Persistence.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    partial class NotificationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ActionLink")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DismissedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("EntityId")
                        .HasColumnType("uuid");

                    b.Property<string>("EntityType")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("ReadAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("ScheduledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Subtype")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTimeOffset?>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.NotificationRecipient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Channel")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("DeliveredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FailureCount")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("LastAttemptAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("NotificationId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("StatusMessage")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId", "UserId")
                        .IsUnique();

                    b.ToTable("NotificationRecipients", (string)null);
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.NotificationTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<int>("Channel")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DefaultActionLink")
                        .HasColumnType("text");

                    b.Property<string>("Footer")
                        .HasColumnType("text");

                    b.Property<string>("Header")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subtype")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("NotificationTemplates", (string)null);
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.UserContactInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PreferredLanguage")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PushNotificationToken")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.ToTable("UserContactInfos", (string)null);
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.UserNotificationSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<int>("Channel")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Category", "Channel")
                        .IsUnique();

                    b.ToTable("UserNotificationSettings", (string)null);
                });

            modelBuilder.Entity("RustRetail.SharedKernel.Domain.Models.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("OccurredOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("ProcessedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", (string)null);
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.Notification", b =>
                {
                    b.HasOne("RustRetail.NotificationService.Domain.Entities.NotificationTemplate", "Template")
                        .WithMany("Notifications")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Template");
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.NotificationRecipient", b =>
                {
                    b.HasOne("RustRetail.NotificationService.Domain.Entities.Notification", "Notification")
                        .WithMany("Recipients")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.Notification", b =>
                {
                    b.Navigation("Recipients");
                });

            modelBuilder.Entity("RustRetail.NotificationService.Domain.Entities.NotificationTemplate", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
