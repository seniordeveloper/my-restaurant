﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyRestaurant.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyRestaurant.Data.Migrations
{
    [DbContext(typeof(RestaurantDbContext))]
    [Migration("20240621191907_InitialMigrations")]
    partial class InitialMigrations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyRestaurant.Data.Entities.AccommodatedClientsGroupEntity", b =>
                {
                    b.Property<int>("TableId")
                        .HasColumnType("integer");

                    b.Property<Guid>("ClientsGroupId")
                        .HasColumnType("uuid");

                    b.HasKey("TableId", "ClientsGroupId");

                    b.HasIndex("ClientsGroupId");

                    b.ToTable("AccommodatedClientsGroups", (string)null);
                });

            modelBuilder.Entity("MyRestaurant.Data.Entities.ClientsGroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("integer");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("ClientsGroups", (string)null);
                });

            modelBuilder.Entity("MyRestaurant.Data.Entities.CustomerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("MyRestaurant.Data.Entities.QueuedClientsGroupEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("ClientsGroupId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("QueuedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClientsGroupId");

                    b.ToTable("QueuedClientsGroups", (string)null);
                });

            modelBuilder.Entity("MyRestaurant.Data.Entities.TableEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Tables", (string)null);
                });

            modelBuilder.Entity("MyRestaurant.Data.Entities.AccommodatedClientsGroupEntity", b =>
                {
                    b.HasOne("MyRestaurant.Data.Entities.ClientsGroupEntity", "ClientsGroup")
                        .WithMany()
                        .HasForeignKey("ClientsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyRestaurant.Data.Entities.TableEntity", "Table")
                        .WithMany()
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientsGroup");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("MyRestaurant.Data.Entities.ClientsGroupEntity", b =>
                {
                    b.HasOne("MyRestaurant.Data.Entities.CustomerEntity", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("MyRestaurant.Data.Entities.QueuedClientsGroupEntity", b =>
                {
                    b.HasOne("MyRestaurant.Data.Entities.ClientsGroupEntity", "ClientsGroup")
                        .WithMany()
                        .HasForeignKey("ClientsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientsGroup");
                });
#pragma warning restore 612, 618
        }
    }
}
