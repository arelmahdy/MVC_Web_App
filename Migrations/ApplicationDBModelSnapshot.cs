﻿// <auto-generated />
using System;
using MVC_Web_App.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MVC_Web_App.Migrations
{
    [DbContext(typeof(ApplicationDB))]
    partial class ApplicationDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MVC_Web_App.Models.AppConfrim", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<DateTime>("DateConfrim")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AppConfrims");
                });

            modelBuilder.Entity("MVC_Web_App.Models.AppRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("NewID()")
                        .HasMaxLength(450);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("AppRoles");
                });

            modelBuilder.Entity("MVC_Web_App.Models.AppUsers", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("NewID()")
                        .HasMaxLength(450);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(650)")
                        .HasMaxLength(650);

                    b.Property<bool>("EmailConfirm")
                        .HasColumnType("bit");

                    b.Property<int>("ErrorLogCount")
                        .HasColumnType("int");

                    b.Property<bool>("LockOut")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LockTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("PasswordConfirm")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("AppUser");
                });

            modelBuilder.Entity("MVC_Web_App.Models.BillingAddress", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("Zip")
                        .HasColumnType("int")
                        .HasMaxLength(50);

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("id");

                    b.HasIndex("UserId");

                    b.ToTable("BillingAddresses");
                });

            modelBuilder.Entity("MVC_Web_App.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("MVC_Web_App.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CatName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MVC_Web_App.Models.Payment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("billingId")
                        .HasColumnType("int");

                    b.Property<string>("cardName")
                        .IsRequired()
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<long>("cardNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("cardType")
                        .IsRequired()
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<int>("cartId")
                        .HasColumnType("int");

                    b.Property<int>("cvv")
                        .HasColumnType("int");

                    b.Property<DateTime>("expiration")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("UserId");

                    b.HasIndex("billingId");

                    b.HasIndex("cartId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("MVC_Web_App.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<bool>("IsPublish")
                        .HasColumnType("bit");

                    b.Property<string>("LikeUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(4000)")
                        .HasMaxLength(4000);

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostImage")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<int>("PostLike")
                        .HasColumnType("int");

                    b.Property<int>("PostViews")
                        .HasColumnType("int");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(70)")
                        .HasMaxLength(70);

                    b.Property<int>("SubId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("SubId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("MVC_Web_App.Models.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CatId")
                        .HasColumnType("int");

                    b.Property<string>("SubCatName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("MVC_Web_App.Models.UserProfile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("PersonalWebUrl")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("UrlImage")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("userProfiles");
                });

            modelBuilder.Entity("MVC_Web_App.Models.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasDefaultValueSql("NewID()")
                        .HasMaxLength(450);

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("MVC_Web_App.Models.UserSetting", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MaxPassLength")
                        .HasColumnType("int");

                    b.Property<int>("MinimumPassLength")
                        .HasColumnType("int");

                    b.Property<bool>("SendWelcomeMessage")
                        .HasColumnType("bit");

                    b.Property<bool>("isDigit")
                        .HasColumnType("bit");

                    b.Property<bool>("isEmailConfirm")
                        .HasColumnType("bit");

                    b.Property<bool>("isRegisterOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isUpper")
                        .HasColumnType("bit");

                    b.HasKey("id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("MVC_Web_App.Models.AppConfrim", b =>
                {
                    b.HasOne("MVC_Web_App.Models.AppUsers", "AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MVC_Web_App.Models.BillingAddress", b =>
                {
                    b.HasOne("MVC_Web_App.Models.AppUsers", "appUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MVC_Web_App.Models.Cart", b =>
                {
                    b.HasOne("MVC_Web_App.Models.AppUsers", "appUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MVC_Web_App.Models.Payment", b =>
                {
                    b.HasOne("MVC_Web_App.Models.AppUsers", "appUser")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("MVC_Web_App.Models.BillingAddress", "billingAddress")
                        .WithMany()
                        .HasForeignKey("billingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC_Web_App.Models.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("cartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MVC_Web_App.Models.Post", b =>
                {
                    b.HasOne("MVC_Web_App.Models.SubCategory", "SubCategory")
                        .WithMany()
                        .HasForeignKey("SubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MVC_Web_App.Models.SubCategory", b =>
                {
                    b.HasOne("MVC_Web_App.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MVC_Web_App.Models.UserProfile", b =>
                {
                    b.HasOne("MVC_Web_App.Models.AppUsers", "AppUsers")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MVC_Web_App.Models.UserRole", b =>
                {
                    b.HasOne("MVC_Web_App.Models.AppRole", "appRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("MVC_Web_App.Models.AppUsers", "appUsers")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
