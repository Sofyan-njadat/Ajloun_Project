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

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Artwork> Artworks { get; set; }

    public virtual DbSet<AssocEventRegistration> AssocEventRegistrations { get; set; }

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

    public virtual DbSet<HallBooking> HallBookings { get; set; }

    public virtual DbSet<Handicraft> Handicrafts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HJ7MGKN;Database=AjlounCultureDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4882C753F61");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E469159357").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D105343B78C6D2").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Admin");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__Articles__9C6270E86CDECB92");

            entity.Property(e => e.AuthorName).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(255);
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Artwork>(entity =>
        {
            entity.HasKey(e => e.ArtworkId).HasName("PK__Artworks__D073AE9B85FA5999");

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
            entity.HasKey(e => e.RegistrationId).HasName("PK__AssocEve__6EF58810B3565686");

            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.AssocEvent).WithMany(p => p.AssocEventRegistrations)
                .HasForeignKey(d => d.AssocEventId)
                .HasConstraintName("FK__AssocEven__Assoc__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.AssocEventRegistrations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AssocEven__UserI__5812160E");
        });

        modelBuilder.Entity<AssociationEvent>(entity =>
        {
            entity.HasKey(e => e.AssocEventId).HasName("PK__Associat__68ACACD3F0D8A62F");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Association).WithMany(p => p.AssociationEvents)
                .HasForeignKey(d => d.AssociationId)
                .HasConstraintName("FK__Associati__Assoc__52593CB8");
        });

        modelBuilder.Entity<AssociationJoinRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Associat__33A8517A4502FD94");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Association).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.AssociationId)
                .HasConstraintName("FK__Associati__Assoc__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Associati__UserI__4E88ABD4");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2070F447198");

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
                .HasConstraintName("FK__Books__CategoryI__6383C8BA");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__BookCate__19093A0B7682F89F");

            entity.HasIndex(e => e.Name, "UQ__BookCate__737584F63995A39E").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BookReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__BookRese__B7EE5F244178E3D4");

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
                .HasConstraintName("FK__BookReser__BookI__66603565");

            entity.HasOne(d => d.User).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BookReser__UserI__6754599E");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7EBFA6C77");

            entity.Property(e => e.AgeRange).HasMaxLength(50);
            entity.Property(e => e.Courseimg).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CourseApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__CourseAp__C93A4C9970D1D3F1");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseApplications)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CourseApp__Cours__412EB0B6");

            entity.HasOne(d => d.User).WithMany(p => p.CourseApplications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CourseApp__UserI__4222D4EF");
        });

        modelBuilder.Entity<CraftOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__CraftOrd__C3905BCF835FBAC2");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Craft).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.CraftId)
                .HasConstraintName("FK__CraftOrde__Craft__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CraftOrde__UserI__72C60C4A");
        });

        modelBuilder.Entity<CulturalAssociation>(entity =>
        {
            entity.HasKey(e => e.AssociationId).HasName("PK__Cultural__B51A182D53F9C0D5");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PresidentName).HasMaxLength(100);
            entity.Property(e => e.Region).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<CulturalEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Cultural__7944C8101A2F14D0");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PosterUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<CulturalEventReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Cultural__B7EE5F24D1407C65");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Event).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__CulturalE__Event__06CD04F7");

            entity.HasOne(d => d.User).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CulturalE__UserI__07C12930");
        });

        modelBuilder.Entity<Festival>(entity =>
        {
            entity.HasKey(e => e.FestivalId).HasName("PK__Festival__875D72CD27A47B83");

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
            entity.HasKey(e => e.ReservationId).HasName("PK__Festival__B7EE5F24F3A533BA");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Festival).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.FestivalId)
                .HasConstraintName("FK__FestivalR__Festi__00200768");

            entity.HasOne(d => d.User).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FestivalR__UserI__01142BA1");
        });

        modelBuilder.Entity<HallBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__HallBook__73951AEDA4CFDBAB");

            entity.Property(e => e.CoordinatorName).HasMaxLength(100);
            entity.Property(e => e.EventTitle).HasMaxLength(200);
            entity.Property(e => e.EventType).HasMaxLength(100);
            entity.Property(e => e.GuestOfHonor).HasMaxLength(200);
            entity.Property(e => e.HallType).HasMaxLength(50);
            entity.Property(e => e.NeedsPconStage).HasColumnName("NeedsPCOnStage");
            entity.Property(e => e.RequestingParty).HasMaxLength(200);
            entity.Property(e => e.ResponsibleName).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
        });

        modelBuilder.Entity<Handicraft>(entity =>
        {
            entity.HasKey(e => e.CraftId).HasName("PK__Handicra__6B18C69C237137F3");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF3BD462E09");

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
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C49A29142");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534CF82AE6B").IsUnique();

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
