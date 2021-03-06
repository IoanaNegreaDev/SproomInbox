// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproomInbox.API.Domain;

#nullable disable

namespace SproomInbox.API.Migrations
{
    [DbContext(typeof(SproomDocumentsDbContext))]
    [Migration("20220422231539_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SproomInbox.API.Domain.Models.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("StateId")
                        .HasColumnType("tinyint");

                    b.Property<byte>("TypeId")
                        .HasColumnType("tinyint");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("SproomInbox.API.Domain.Models.DocumentState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<Guid>("DocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("StateId")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.ToTable("DocumentState");
                });

            modelBuilder.Entity("SproomInbox.API.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SproomInbox.API.Domain.Models.Document", b =>
                {
                    b.HasOne("SproomInbox.API.Domain.Models.User", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Document_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SproomInbox.API.Domain.Models.DocumentState", b =>
                {
                    b.HasOne("SproomInbox.API.Domain.Models.Document", "Document")
                        .WithMany("StateHistory")
                        .HasForeignKey("DocumentId")
                        .IsRequired()
                        .HasConstraintName("FK_DocumentState_Document");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("SproomInbox.API.Domain.Models.Document", b =>
                {
                    b.Navigation("StateHistory");
                });

            modelBuilder.Entity("SproomInbox.API.Domain.Models.User", b =>
                {
                    b.Navigation("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}
