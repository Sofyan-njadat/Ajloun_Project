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

    public virtual DbSet<HallBooking> HallBookings { get; set; }

    public virtual DbSet<Handicraft> Handicrafts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPost> UserPosts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2RD3EN0;Database=AjlounCultureDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4885D6D65E8");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E484FF8795").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D10534488F0D98").IsUnique();

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
            entity.HasKey(e => e.ArticleId).HasName("PK__Articles__9C6270E894F44A42");

            entity.Property(e => e.AuthorName).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(255);
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Artwork>(entity =>
        {
            entity.HasKey(e => e.ArtworkId).HasName("PK__Artworks__D073AE9B618B413E");

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
            entity.HasKey(e => e.RegistrationId).HasName("PK__AssocEve__6EF58810C506225A");

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
            entity.HasKey(e => e.CategoryId).HasName("PK__Associat__19093A0B281BD5A0");

            entity.HasIndex(e => e.Name, "UQ__Associat__737584F646B95785").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<AssociationEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Associat__68ACACD3CB6CE5FA");

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
            entity.HasKey(e => e.RequestId).HasName("PK__Associat__33A8517A859167EB");

            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Association).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.AssociationId)
                .HasConstraintName("FK__Associati__Assoc__29221CFB");

            entity.HasOne(d => d.User).WithMany(p => p.AssociationJoinRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Associati__UserI__2A164134");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C207D719FCD8");

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
            entity.HasKey(e => e.CategoryId).HasName("PK__BookCate__19093A0B251C0492");

            entity.HasIndex(e => e.Name, "UQ__BookCate__737584F61F24F07C").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BookReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__BookRese__B7EE5F24075FE864");

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
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A75A7DCE68");

            entity.Property(e => e.AgeRange).HasMaxLength(50);
            entity.Property(e => e.Courseimg).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CourseApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__CourseAp__C93A4C997E318E70");

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
            entity.HasKey(e => e.OrderId).HasName("PK__CraftOrd__C3905BCF3FF27D0F");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Craft).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.CraftId)
                .HasConstraintName("FK__CraftOrde__Craft__6EF57B66");

            entity.HasOne(d => d.User).WithMany(p => p.CraftOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CraftOrde__UserI__6FE99F9F");
        });

        modelBuilder.Entity<CulturalAssociation>(entity =>
        {
            entity.HasKey(e => e.AssociationId).HasName("PK__Cultural__B51A182DEFCFB824");

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
            entity.HasKey(e => e.EventId).HasName("PK__Cultural__7944C8102C87CDFF");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PosterUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<CulturalEventReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Cultural__B7EE5F2431E04E06");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Event).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__CulturalE__Event__03F0984C");

            entity.HasOne(d => d.User).WithMany(p => p.CulturalEventReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CulturalE__UserI__04E4BC85");
        });

        modelBuilder.Entity<Festival>(entity =>
        {
            entity.HasKey(e => e.FestivalId).HasName("PK__Festival__875D72CD37AEC59A");

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
            entity.HasKey(e => e.ReservationId).HasName("PK__Festival__B7EE5F24E0FFF218");

            entity.Property(e => e.ReservationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Festival).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.FestivalId)
                .HasConstraintName("FK__FestivalR__Festi__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.FestivalReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FestivalR__UserI__7E37BEF6");
        });

        modelBuilder.Entity<HallBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__HallBook__73951AEDF0E52CFD");

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
            entity.HasKey(e => e.CraftId).HasName("PK__Handicra__6B18C69C2250D25B");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF396D22C0E");

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
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB044646B");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053487DD34DF").IsUnique();

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
            entity.HasKey(e => e.PostId).HasName("PK__UserPost__AA126018B2A03E79");

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
                .HasConstraintName("FK__UserPosts__FileP__30C33EC3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
