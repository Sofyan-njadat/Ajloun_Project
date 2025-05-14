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
        => optionsBuilder.UseSqlServer("Server=DESKTOP-F4OJHS0;Database=AjlounCultureDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4886784BAB8");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E4140E56B7").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D1053423C49EBC").IsUnique();

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
            entity.HasKey(e => e.ArticleId).HasName("PK__Articles__9C6270E8515F5928");

            entity.Property(e => e.AuthorName).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(255);
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Artwork>(entity =>
        {
            entity.HasKey(e => e.ArtworkId).HasName("PK__Artworks__D073AE9BEAA7598E");

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
            entity.HasKey(e => e.RegistrationId).HasName("PK__AssocEve__6EF588108F06B138");

            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.AssocEvent).WithMany(p => p.AssocEventRegistrations)
                .HasForeignKey(d => d.AssocEventId)
                .HasConstraintName("FK__AssocEven__Assoc__5AEE82B9");

            entity.HasOne(d => d.User).WithMany(p => p.AssocEventRegistrations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AssocEven__UserI__5BE2A6F2");
        });

        modelBuilder.Entity<AssociationEvent>(entity =>
        {
            entity.HasKey(e => e.AssocEventId).HasName("PK__Associat__68ACACD317CEA8AC");

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
                .HasConstraintName("FK__Associati__Assoc__5629CD9C");
        });

        modelBuilder.Entity<AssociationJoinRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Associat__33A8517ACAEDC745");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Association).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.AssociationId)
                .HasConstraintName("FK__Associati__Assoc__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Associati__UserI__52593CB8");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2077936A994");

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
                .HasConstraintName("FK__Books__CategoryI__6754599E");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__BookCate__19093A0BC6598F46");

            entity.HasIndex(e => e.Name, "UQ__BookCate__737584F69594E6EF").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BookReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__BookRese__B7EE5F24972CF434");

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
                .HasConstraintName("FK__BookReser__BookI__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BookReser__UserI__6B24EA82");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A72F202A66");

            entity.Property(e => e.AgeRange).HasMaxLength(50);
            entity.Property(e => e.Courseimg).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CourseApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__CourseAp__C93A4C991C8F329A");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseApplications)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CourseApp__Cours__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.CourseApplications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CourseApp__UserI__45F365D3");
        });

        modelBuilder.Entity<CraftOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__CraftOrd__C3905BCF20DA874E");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Craft).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.CraftId)
                .HasConstraintName("FK__CraftOrde__Craft__75A278F5");

            entity.HasOne(d => d.User).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CraftOrde__UserI__76969D2E");
        });

        modelBuilder.Entity<CulturalAssociation>(entity =>
        {
            entity.HasKey(e => e.AssociationId).HasName("PK__Cultural__B51A182D3B8C912B");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PresidentName).HasMaxLength(100);
            entity.Property(e => e.Region).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<CulturalEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Cultural__7944C81055B1620D");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PosterUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<CulturalEventReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Cultural__B7EE5F245654FF3E");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Event).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__CulturalE__Event__0A9D95DB");

            entity.HasOne(d => d.User).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CulturalE__UserI__0B91BA14");
        });

        modelBuilder.Entity<Festival>(entity =>
        {
            entity.HasKey(e => e.FestivalId).HasName("PK__Festival__875D72CD4D8A8A25");

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
            entity.HasKey(e => e.ReservationId).HasName("PK__Festival__B7EE5F2452E1DBDC");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Festival).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.FestivalId)
                .HasConstraintName("FK__FestivalR__Festi__03F0984C");

            entity.HasOne(d => d.User).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FestivalR__UserI__04E4BC85");
        });

        modelBuilder.Entity<HallBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__HallBook__73951AEDF0DACC34");

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
            entity.HasKey(e => e.CraftId).HasName("PK__Handicra__6B18C69CD252226E");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF3EBF9A073");

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
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C5D6151DC");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105349330847D").IsUnique();

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
