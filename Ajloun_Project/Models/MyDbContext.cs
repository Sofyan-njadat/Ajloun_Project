using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Project.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<ArtWorkshop> ArtWorkshops { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Artwork> Artworks { get; set; }

    public virtual DbSet<AssocEventRegistration> AssocEventRegistrations { get; set; }

    public virtual DbSet<AssociationCategory> AssociationCategories { get; set; }

    public virtual DbSet<AssociationEvent> AssociationEvents { get; set; }

    public virtual DbSet<AssociationJoinRequest> AssociationJoinRequests { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<BookReservation> BookReservations { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseApplication> CourseApplications { get; set; }

    public virtual DbSet<CraftOrder> CraftOrders { get; set; }

    public virtual DbSet<CulturalAssociation> CulturalAssociations { get; set; }

    public virtual DbSet<CulturalEvent> CulturalEvents { get; set; }

    public virtual DbSet<CulturalEventReservation> CulturalEventReservations { get; set; }

    public virtual DbSet<Festival> Festivals { get; set; }

    public virtual DbSet<FestivalReservation> FestivalReservations { get; set; }

    public virtual DbSet<GalleryImage> GalleryImages { get; set; }

    public virtual DbSet<HallBooking> HallBookings { get; set; }

    public virtual DbSet<Handicraft> Handicrafts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPost> UserPosts { get; set; }

    public virtual DbSet<WorkshopRegistration> WorkshopRegistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=AjlounCultureDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE488DC652FC8");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E4ACF02193").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534CB632101").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Admin");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<ArtWorkshop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArtWorks__3214EC07F429006A");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(300);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__Articles__9C6270E8F94DD17C");

            entity.Property(e => e.AuthorName).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(255);
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Artwork>(entity =>
        {
            entity.HasKey(e => e.ArtworkId).HasName("PK__Artworks__D073AE9B3A56D2C0");

            entity.Property(e => e.ArtistEmail).HasMaxLength(100);
            entity.Property(e => e.ArtistName).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.PortfolioUrl).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<AssocEventRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__AssocEve__6EF58810AC60C884");

            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.AssocEvent).WithMany(p => p.AssocEventRegistrations)
                .HasForeignKey(d => d.AssocEventId)
                .HasConstraintName("FK__AssocEven__Assoc__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.AssocEventRegistrations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AssocEven__UserI__5441852A");
        });

        modelBuilder.Entity<AssociationCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Associat__19093A0B16815FFE");

            entity.HasIndex(e => e.Name, "UQ__Associat__737584F670989B5A").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AssociationEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Associat__68ACACD3D8A09147");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PosterUrl).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Association).WithMany(p => p.AssociationEvents)
                .HasForeignKey(d => d.AssociationId)
                .HasConstraintName("FK__Associati__Assoc__4E88ABD4");
        });

        modelBuilder.Entity<AssociationJoinRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Associat__33A8517A78397C99");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Association).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.AssociationId)
                .HasConstraintName("FK__Associati__Assoc__49C3F6B7");

            entity.HasOne(d => d.User).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Associati__UserI__4AB81AF0");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C207B1E5AFF8");

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.AvailableCopies).HasDefaultValue(1);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Books__CategoryI__5FB337D6");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__BookCate__19093A0B12864F58");

            entity.HasIndex(e => e.Name, "UQ__BookCate__737584F6944A22CD").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BookReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__BookRese__B7EE5F2412BFE50A");

            entity.Property(e => e.Agreement).HasDefaultValue(false);
            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Book).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookReser__BookI__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BookReser__UserI__6383C8BA");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A765914984");

            entity.Property(e => e.AgeRange).HasMaxLength(50);
            entity.Property(e => e.Courseimg).HasMaxLength(500);
            entity.Property(e => e.IsVisible).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CourseApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__CourseAp__C93A4C998594821F");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseApplications)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CourseApp__Cours__3D5E1FD2");

            entity.HasOne(d => d.User).WithMany(p => p.CourseApplications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CourseApp__UserI__3E52440B");
        });

        modelBuilder.Entity<CraftOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__CraftOrd__C3905BCF1AECE223");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Craft).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.CraftId)
                .HasConstraintName("FK__CraftOrde__Craft__6E01572D");

            entity.HasOne(d => d.User).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CraftOrde__UserI__6EF57B66");
        });

        modelBuilder.Entity<CulturalAssociation>(entity =>
        {
            entity.HasKey(e => e.AssociationId).HasName("PK__Cultural__B51A182D69818CC2");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PresidentName).HasMaxLength(100);
            entity.Property(e => e.Region).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Category).WithMany(p => p.CulturalAssociations)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__CulturalA__Categ__0B91BA14");
        });

        modelBuilder.Entity<CulturalEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Cultural__7944C810377832BA");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PosterUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<CulturalEventReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Cultural__B7EE5F24FEEC2269");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Event).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__CulturalE__Event__02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CulturalE__UserI__03F0984C");
        });

        modelBuilder.Entity<Festival>(entity =>
        {
            entity.HasKey(e => e.FestivalId).HasName("PK__Festival__875D72CD3AFCAB34");

            entity.Property(e => e.BannerImageUrl).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FestivalType).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<FestivalReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Festival__B7EE5F24652004C9");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Festival).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.FestivalId)
                .HasConstraintName("FK__FestivalR__Festi__7C4F7684");

            entity.HasOne(d => d.User).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FestivalR__UserI__7D439ABD");
        });

        modelBuilder.Entity<GalleryImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GalleryI__3214EC071F27D28E");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<HallBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__HallBook__73951AEDD43995BE");

            entity.Property(e => e.AttachmentPath).HasMaxLength(500);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.CoordinatorName).HasMaxLength(100);
            entity.Property(e => e.EventTitle).HasMaxLength(200);
            entity.Property(e => e.EventType).HasMaxLength(100);
            entity.Property(e => e.GuestOfHonor).HasMaxLength(200);
            entity.Property(e => e.HallType).HasMaxLength(50);
            entity.Property(e => e.NeedsPconStage).HasColumnName("NeedsPCOnStage");
            entity.Property(e => e.ResponsibleName).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.CultureOrg).WithMany(p => p.HallBookings)
                .HasForeignKey(d => d.CultureOrgId)
                .HasConstraintName("FK_HallBookings_Associations");

            entity.HasOne(d => d.User).WithMany(p => p.HallBookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_HallBookings_Users");
        });

        modelBuilder.Entity<Handicraft>(entity =>
        {
            entity.HasKey(e => e.CraftId).HasName("PK__Handicra__6B18C69C79D9535A");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF33FF1DC26");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.PublishDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Published");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C375C64C0");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345E76B9C9").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.NationalId).HasMaxLength(20);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ProfileImage).HasMaxLength(255);
        });

        modelBuilder.Entity<UserPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__UserPost__AA12601821D6885E");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FilePath)
                .HasMaxLength(300)
                .HasDefaultValue("");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Draft");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserPosts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPosts__UserI__17F790F9");
        });

        modelBuilder.Entity<WorkshopRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Workshop__3214EC07E8CD5DC1");

            entity.Property(e => e.BirthCertificateImage).HasMaxLength(300);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.User).WithMany(p => p.WorkshopRegistrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkshopR__UserI__4D5F7D71");

            entity.HasOne(d => d.Workshop).WithMany(p => p.WorkshopRegistrations)
                .HasForeignKey(d => d.WorkshopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkshopR__Works__4E53A1AA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
