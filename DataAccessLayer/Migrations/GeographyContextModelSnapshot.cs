﻿// <auto-generated />
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(GeographyContext))]
    partial class GeographyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CountryRiver", b =>
                {
                    b.Property<int>("CountriesCountryID")
                        .HasColumnType("int");

                    b.Property<int>("RiversRiverID")
                        .HasColumnType("int");

                    b.HasKey("CountriesCountryID", "RiversRiverID");

                    b.HasIndex("RiversRiverID");

                    b.ToTable("CountryRiver");
                });

            modelBuilder.Entity("DataAccessLayer.Model.City", b =>
                {
                    b.Property<int>("CityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryID")
                        .HasColumnType("int");

                    b.Property<bool>("IsCapital")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Population")
                        .HasColumnType("int");

                    b.HasKey("CityID");

                    b.HasIndex("CountryID");

                    b.ToTable("City");
                });

            modelBuilder.Entity("DataAccessLayer.Model.Continent", b =>
                {
                    b.Property<int>("ContinentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ContinentID");

                    b.ToTable("Continent");
                });

            modelBuilder.Entity("DataAccessLayer.Model.Country", b =>
                {
                    b.Property<int>("CountryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContinentID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Population")
                        .HasColumnType("int");

                    b.Property<double>("Surface")
                        .HasColumnType("float");

                    b.HasKey("CountryID");

                    b.HasIndex("ContinentID");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("DataAccessLayer.Model.River", b =>
                {
                    b.Property<int>("RiverID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Length")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("RiverID");

                    b.ToTable("River");
                });

            modelBuilder.Entity("CountryRiver", b =>
                {
                    b.HasOne("DataAccessLayer.Model.Country", null)
                        .WithMany()
                        .HasForeignKey("CountriesCountryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Model.River", null)
                        .WithMany()
                        .HasForeignKey("RiversRiverID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataAccessLayer.Model.City", b =>
                {
                    b.HasOne("DataAccessLayer.Model.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("DataAccessLayer.Model.Country", b =>
                {
                    b.HasOne("DataAccessLayer.Model.Continent", "Continent")
                        .WithMany("Countries")
                        .HasForeignKey("ContinentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Continent");
                });

            modelBuilder.Entity("DataAccessLayer.Model.Continent", b =>
                {
                    b.Navigation("Countries");
                });

            modelBuilder.Entity("DataAccessLayer.Model.Country", b =>
                {
                    b.Navigation("Cities");
                });
#pragma warning restore 612, 618
        }
    }
}
