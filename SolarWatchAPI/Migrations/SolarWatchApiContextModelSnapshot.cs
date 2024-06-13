// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolarWatchAPI.Data;

#nullable disable

namespace SolarWatchAPI.Migrations
{
    [DbContext(typeof(SolarWatchApiContext))]
    partial class SolarWatchApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SolarWatchAPI.Model.DataModels.City", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Coutry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Lat")
                        .HasColumnType("float");

                    b.Property<double>("Lon")
                        .HasColumnType("float");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Name");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("SolarWatchAPI.Model.DataModels.SunriseSunset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly>("Sunrise")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("Sunset")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("CityName");

                    b.ToTable("SunriseSunsets");
                });

            modelBuilder.Entity("SolarWatchAPI.Model.DataModels.SunriseSunset", b =>
                {
                    b.HasOne("SolarWatchAPI.Model.DataModels.City", null)
                        .WithMany("SunriseSunsets")
                        .HasForeignKey("CityName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SolarWatchAPI.Model.DataModels.City", b =>
                {
                    b.Navigation("SunriseSunsets");
                });
#pragma warning restore 612, 618
        }
    }
}
