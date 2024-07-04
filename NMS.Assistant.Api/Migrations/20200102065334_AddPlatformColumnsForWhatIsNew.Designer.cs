﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NMS.Assistant.Persistence;

namespace NMS.Assistant.Api.Migrations
{
    [DbContext(typeof(NmsAssistantContext))]
    [Migration("20200102065334_AddPlatformColumnsForWhatIsNew")]
    partial class AddPlatformColumnsForWhatIsNew
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.Contributor", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Contribution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SortRank")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("Contributors");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.Donation", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DonationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("Donations");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.Feedback", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.FeedbackAnswer", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnonymousUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AppType")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAnswered")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FeedbackGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FeedbackQuestionGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("FeedbackGuid");

                    b.HasIndex("FeedbackQuestionGuid");

                    b.ToTable("FeedbackAnswers");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.FeedbackQuestion", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FeedbackGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("FeedbackGuid");

                    b.ToTable("FeedbackQuestions");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.GuideMeta", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("GuideMetaDatas");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.Permission", b =>
                {
                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Type");

                    b.ToTable("Permission");

                    b.HasData(
                        new
                        {
                            Type = 0
                        },
                        new
                        {
                            Type = 1
                        },
                        new
                        {
                            Type = 2
                        },
                        new
                        {
                            Type = 3
                        },
                        new
                        {
                            Type = 4
                        },
                        new
                        {
                            Type = 5
                        },
                        new
                        {
                            Type = 6
                        },
                        new
                        {
                            Type = 7
                        },
                        new
                        {
                            Type = 8
                        },
                        new
                        {
                            Type = 9
                        },
                        new
                        {
                            Type = 10
                        },
                        new
                        {
                            Type = 11
                        },
                        new
                        {
                            Type = 12
                        },
                        new
                        {
                            Type = 13
                        },
                        new
                        {
                            Type = 14
                        },
                        new
                        {
                            Type = 15
                        },
                        new
                        {
                            Type = 16
                        },
                        new
                        {
                            Type = 17
                        },
                        new
                        {
                            Type = 18
                        });
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.Setting", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ActiveDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.Testimonial", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Review")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SortRank")
                        .HasColumnType("int");

                    b.Property<int>("Source")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("Testimonials");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.UserPermission", b =>
                {
                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PermissionType")
                        .HasColumnType("int");

                    b.HasKey("UserGuid", "PermissionType");

                    b.HasIndex("PermissionType");

                    b.ToTable("UserPermission");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.Version", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ActiveDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Android")
                        .HasColumnType("int");

                    b.Property<int>("Ios")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Web")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("Versions");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.WhatIsNew", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ActiveDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAndroid")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIos")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWeb")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWebApp")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("WhatIsNews");
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.FeedbackAnswer", b =>
                {
                    b.HasOne("NMS.Assistant.Persistence.Entity.Feedback", "Feedback")
                        .WithMany("Answers")
                        .HasForeignKey("FeedbackGuid")
                        .HasConstraintName("ForeignKey_FeedbackAnswers_Feedbacks")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NMS.Assistant.Persistence.Entity.FeedbackQuestion", "FeedbackQuestion")
                        .WithMany("Answers")
                        .HasForeignKey("FeedbackQuestionGuid")
                        .HasConstraintName("ForeignKey_FeedbackAnswers_FeedbackQuestions")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.FeedbackQuestion", b =>
                {
                    b.HasOne("NMS.Assistant.Persistence.Entity.Feedback", "Feedback")
                        .WithMany("Questions")
                        .HasForeignKey("FeedbackGuid")
                        .HasConstraintName("ForeignKey_FeedbackQuestions_Feedbacks")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NMS.Assistant.Persistence.Entity.UserPermission", b =>
                {
                    b.HasOne("NMS.Assistant.Persistence.Entity.Permission", "Permission")
                        .WithMany("UserPermissions")
                        .HasForeignKey("PermissionType")
                        .HasConstraintName("ForeignKey_UserPermissions_Permissions")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NMS.Assistant.Persistence.Entity.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserGuid")
                        .HasConstraintName("ForeignKey_UserPermissions_Users")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
