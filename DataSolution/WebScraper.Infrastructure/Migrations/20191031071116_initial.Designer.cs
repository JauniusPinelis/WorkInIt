﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebScraper.Infrastructure.Db;

namespace WebScraper.Infrastructure.Migrations
{
    [DbContext(typeof(JobDbContext))]
    [Migration("20191031071116_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebScraper.Core.Entities.JobInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasMaxLength(5)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HtmlCode")
                        .HasMaxLength(100000);

                    b.Property<int?>("JobUrlId");

                    b.HasKey("Id");

                    b.HasIndex("JobUrlId")
                        .IsUnique()
                        .HasFilter("[JobUrlId] IS NOT NULL");

                    b.ToTable("JobInfos");
                });

            modelBuilder.Entity("WebScraper.Core.Entities.JobUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasMaxLength(5)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("JobInfoId");

                    b.Property<string>("Url")
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.ToTable("JobUrls");
                });

            modelBuilder.Entity("WebScraper.Core.Entities.JobInfo", b =>
                {
                    b.HasOne("WebScraper.Core.Entities.JobUrl", "JobUrl")
                        .WithOne("JobInfo")
                        .HasForeignKey("WebScraper.Core.Entities.JobInfo", "JobUrlId")
                        .HasConstraintName("FK_tblData_JobInfo_tblDataUrl")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}