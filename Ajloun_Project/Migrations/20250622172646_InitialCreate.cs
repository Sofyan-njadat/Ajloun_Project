using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ajloun_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Admin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admins__719FE488DC652FC8", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Articles__9C6270E8F94DD17C", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "Artworks",
                columns: table => new
                {
                    ArtworkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArtistName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArtistEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Artworks__D073AE9B3A56D2C0", x => x.ArtworkId);
                });

            migrationBuilder.CreateTable(
                name: "ArtWorkshops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ArtWorks__3214EC07F429006A", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssociationCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Associat__19093A0B16815FFE", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "BookCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BookCate__19093A0B12864F58", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Courseimg = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Courses__C92D71A765914984", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "CulturalEvents",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PosterUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Time = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cultural__7944C810377832BA", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Festivals",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FestivalType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BannerImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Festival__875D72CD3AFCAB34", x => x.FestivalId);
                });

            migrationBuilder.CreateTable(
                name: "GalleryImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GalleryI__3214EC071F27D28E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Handicrafts",
                columns: table => new
                {
                    CraftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Handicra__6B18C69C79D9535A", x => x.CraftId);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Published")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__News__954EBDF33FF1DC26", x => x.NewsId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProfileImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4C375C64C0", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CulturalAssociations",
                columns: table => new
                {
                    AssociationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PresidentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FoundedYear = table.Column<int>(type: "int", nullable: true),
                    MembersCount = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cultural__B51A182D69818CC2", x => x.AssociationId);
                    table.ForeignKey(
                        name: "FK__CulturalA__Categ__0B91BA14",
                        column: x => x.CategoryId,
                        principalTable: "AssociationCategories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PublishedYear = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AvailableCopies = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Books__3DE0C207B1E5AFF8", x => x.BookId);
                    table.ForeignKey(
                        name: "FK__Books__CategoryI__5FB337D6",
                        column: x => x.CategoryId,
                        principalTable: "BookCategories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "CourseApplications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    BirthCertificateImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agreement = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CourseAp__C93A4C998594821F", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK__CourseApp__Cours__3D5E1FD2",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK__CourseApp__UserI__3E52440B",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CraftOrders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CraftId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CraftOrd__C3905BCF1AECE223", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK__CraftOrde__Craft__6E01572D",
                        column: x => x.CraftId,
                        principalTable: "Handicrafts",
                        principalColumn: "CraftId");
                    table.ForeignKey(
                        name: "FK__CraftOrde__UserI__6EF57B66",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CulturalEventReservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    AttendanceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cultural__B7EE5F24FEEC2269", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK__CulturalE__Event__02FC7413",
                        column: x => x.EventId,
                        principalTable: "CulturalEvents",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK__CulturalE__UserI__03F0984C",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FestivalReservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FestivalId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    AttendanceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Festival__B7EE5F24652004C9", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK__FestivalR__Festi__7C4F7684",
                        column: x => x.FestivalId,
                        principalTable: "Festivals",
                        principalColumn: "FestivalId");
                    table.ForeignKey(
                        name: "FK__FestivalR__UserI__7D439ABD",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Draft"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserPost__AA12601821D6885E", x => x.PostId);
                    table.ForeignKey(
                        name: "FK__UserPosts__UserI__17F790F9",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "WorkshopRegistrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkshopId = table.Column<int>(type: "int", nullable: false),
                    BirthCertificateImage = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Workshop__3214EC07E8CD5DC1", x => x.Id);
                    table.ForeignKey(
                        name: "FK__WorkshopR__UserI__4D5F7D71",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK__WorkshopR__Works__4E53A1AA",
                        column: x => x.WorkshopId,
                        principalTable: "ArtWorkshops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssociationEvents",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssociationId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    PosterUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Associat__68ACACD3D8A09147", x => x.EventId);
                    table.ForeignKey(
                        name: "FK__Associati__Assoc__4E88ABD4",
                        column: x => x.AssociationId,
                        principalTable: "CulturalAssociations",
                        principalColumn: "AssociationId");
                });

            migrationBuilder.CreateTable(
                name: "AssociationJoinRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssociationId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Associat__33A8517A78397C99", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK__Associati__Assoc__49C3F6B7",
                        column: x => x.AssociationId,
                        principalTable: "CulturalAssociations",
                        principalColumn: "AssociationId");
                    table.ForeignKey(
                        name: "FK__Associati__UserI__4AB81AF0",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "HallBookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HallType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EventDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GuestOfHonor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccompanyingActivities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeedsScreens = table.Column<bool>(type: "bit", nullable: false),
                    NeedsProjector = table.Column<bool>(type: "bit", nullable: false),
                    NeedsAudienceInteraction = table.Column<bool>(type: "bit", nullable: false),
                    GuestCount = table.Column<int>(type: "int", nullable: true),
                    NeedsFilm = table.Column<bool>(type: "bit", nullable: false),
                    NeedsPCOnStage = table.Column<bool>(type: "bit", nullable: false),
                    NeedsDocumentation = table.Column<bool>(type: "bit", nullable: false),
                    OtherTechnicalNeeds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoordinatorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResponsibleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CultureOrgId = table.Column<int>(type: "int", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HallBook__73951AEDD43995BE", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_HallBookings_Associations",
                        column: x => x.CultureOrgId,
                        principalTable: "CulturalAssociations",
                        principalColumn: "AssociationId");
                    table.ForeignKey(
                        name: "FK_HallBookings_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "BookReservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    PickupDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReturnDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    Agreement = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BookRese__B7EE5F2412BFE50A", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK__BookReser__BookI__628FA481",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId");
                    table.ForeignKey(
                        name: "FK__BookReser__UserI__6383C8BA",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "AssocEventRegistrations",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssocEventId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    RegisteredAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AssocEve__6EF58810AC60C884", x => x.RegistrationId);
                    table.ForeignKey(
                        name: "FK__AssocEven__Assoc__534D60F1",
                        column: x => x.AssocEventId,
                        principalTable: "AssociationEvents",
                        principalColumn: "EventId");
                    table.ForeignKey(
                        name: "FK__AssocEven__UserI__5441852A",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Admins__536C85E4ACF02193",
                table: "Admins",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Admins__A9D10534CB632101",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssocEventRegistrations_AssocEventId",
                table: "AssocEventRegistrations",
                column: "AssocEventId");

            migrationBuilder.CreateIndex(
                name: "IX_AssocEventRegistrations_UserId",
                table: "AssocEventRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Associat__737584F670989B5A",
                table: "AssociationCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssociationEvents_AssociationId",
                table: "AssociationEvents",
                column: "AssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociationJoinRequests_AssociationId",
                table: "AssociationJoinRequests",
                column: "AssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociationJoinRequests_UserId",
                table: "AssociationJoinRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__BookCate__737584F6944A22CD",
                table: "BookCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_BookId",
                table: "BookReservations",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_UserId",
                table: "BookReservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseApplications_CourseId",
                table: "CourseApplications",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseApplications_UserId",
                table: "CourseApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CraftOrders_CraftId",
                table: "CraftOrders",
                column: "CraftId");

            migrationBuilder.CreateIndex(
                name: "IX_CraftOrders_UserId",
                table: "CraftOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalAssociations_CategoryId",
                table: "CulturalAssociations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalEventReservations_EventId",
                table: "CulturalEventReservations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_CulturalEventReservations_UserId",
                table: "CulturalEventReservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FestivalReservations_FestivalId",
                table: "FestivalReservations",
                column: "FestivalId");

            migrationBuilder.CreateIndex(
                name: "IX_FestivalReservations_UserId",
                table: "FestivalReservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HallBookings_CultureOrgId",
                table: "HallBookings",
                column: "CultureOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_HallBookings_UserId",
                table: "HallBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_UserId",
                table: "UserPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105345E76B9C9",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRegistrations_UserId",
                table: "WorkshopRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRegistrations_WorkshopId",
                table: "WorkshopRegistrations",
                column: "WorkshopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Artworks");

            migrationBuilder.DropTable(
                name: "AssocEventRegistrations");

            migrationBuilder.DropTable(
                name: "AssociationJoinRequests");

            migrationBuilder.DropTable(
                name: "BookReservations");

            migrationBuilder.DropTable(
                name: "CourseApplications");

            migrationBuilder.DropTable(
                name: "CraftOrders");

            migrationBuilder.DropTable(
                name: "CulturalEventReservations");

            migrationBuilder.DropTable(
                name: "FestivalReservations");

            migrationBuilder.DropTable(
                name: "GalleryImages");

            migrationBuilder.DropTable(
                name: "HallBookings");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "UserPosts");

            migrationBuilder.DropTable(
                name: "WorkshopRegistrations");

            migrationBuilder.DropTable(
                name: "AssociationEvents");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Handicrafts");

            migrationBuilder.DropTable(
                name: "CulturalEvents");

            migrationBuilder.DropTable(
                name: "Festivals");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ArtWorkshops");

            migrationBuilder.DropTable(
                name: "CulturalAssociations");

            migrationBuilder.DropTable(
                name: "BookCategories");

            migrationBuilder.DropTable(
                name: "AssociationCategories");
        }
    }
}
