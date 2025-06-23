IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Admins] (
    [AdminId] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(255) NOT NULL,
    [FullName] nvarchar(100) NULL,
    [Role] nvarchar(20) NOT NULL DEFAULT N'Admin',
    CONSTRAINT [PK__Admins__719FE488DC652FC8] PRIMARY KEY ([AdminId])
);

CREATE TABLE [Articles] (
    [ArticleId] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NULL,
    [Content] nvarchar(max) NULL,
    [Category] nvarchar(50) NULL,
    [AuthorName] nvarchar(100) NULL,
    [PublishDate] datetime NULL,
    [CoverImageUrl] nvarchar(255) NULL,
    CONSTRAINT [PK__Articles__9C6270E8F94DD17C] PRIMARY KEY ([ArticleId])
);

CREATE TABLE [Artworks] (
    [ArtworkId] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NULL,
    [ArtistName] nvarchar(100) NULL,
    [ArtistEmail] nvarchar(100) NULL,
    [PortfolioUrl] nvarchar(255) NULL,
    [Type] nvarchar(50) NULL,
    [Description] nvarchar(max) NULL,
    [ImageUrl] nvarchar(255) NULL,
    [Status] nvarchar(20) NULL DEFAULT N'Pending',
    CONSTRAINT [PK__Artworks__D073AE9B3A56D2C0] PRIMARY KEY ([ArtworkId])
);

CREATE TABLE [ArtWorkshops] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Category] nvarchar(100) NULL,
    [StartDate] date NOT NULL,
    [EndDate] date NOT NULL,
    [MinAge] int NULL,
    [ImageUrl] nvarchar(300) NULL,
    [IsHidden] bit NOT NULL,
    [MaxAge] int NULL,
    CONSTRAINT [PK__ArtWorks__3214EC07F429006A] PRIMARY KEY ([Id])
);

CREATE TABLE [AssociationCategories] (
    [CategoryId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK__Associat__19093A0B16815FFE] PRIMARY KEY ([CategoryId])
);

CREATE TABLE [BookCategories] (
    [CategoryId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK__BookCate__19093A0B12864F58] PRIMARY KEY ([CategoryId])
);

CREATE TABLE [Courses] (
    [CourseId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NULL,
    [Description] nvarchar(max) NULL,
    [AgeRange] nvarchar(50) NULL,
    [Courseimg] nvarchar(500) NULL,
    [IsVisible] bit NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK__Courses__C92D71A765914984] PRIMARY KEY ([CourseId])
);

CREATE TABLE [CulturalEvents] (
    [EventId] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NULL,
    [Description] nvarchar(max) NULL,
    [EventType] nvarchar(50) NULL,
    [Date] datetime NULL,
    [Location] nvarchar(100) NULL,
    [PosterUrl] nvarchar(255) NULL,
    [CreatedAt] datetime NULL,
    [Time] time NULL,
    CONSTRAINT [PK__Cultural__7944C810377832BA] PRIMARY KEY ([EventId])
);

CREATE TABLE [Festivals] (
    [FestivalId] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NULL,
    [Description] nvarchar(max) NULL,
    [FestivalType] nvarchar(50) NULL,
    [StartDate] datetime NULL,
    [EndDate] datetime NULL,
    [Location] nvarchar(100) NULL,
    [BannerImageUrl] nvarchar(255) NULL,
    [CreatedAt] datetime NULL,
    CONSTRAINT [PK__Festival__875D72CD3AFCAB34] PRIMARY KEY ([FestivalId])
);

CREATE TABLE [GalleryImages] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    CONSTRAINT [PK__GalleryI__3214EC071F27D28E] PRIMARY KEY ([Id])
);

CREATE TABLE [Handicrafts] (
    [CraftId] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NULL,
    [Description] nvarchar(max) NULL,
    [Price] decimal(10,2) NULL,
    [Stock] int NULL,
    [Category] nvarchar(50) NULL,
    [ImageUrl] nvarchar(255) NULL,
    [CreatedAt] datetime NULL,
    CONSTRAINT [PK__Handicra__6B18C69C79D9535A] PRIMARY KEY ([CraftId])
);

CREATE TABLE [News] (
    [NewsId] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(255) NULL,
    [Category] nvarchar(50) NULL,
    [PublishDate] datetime NULL DEFAULT ((getdate())),
    [Status] nvarchar(20) NULL DEFAULT N'Published',
    CONSTRAINT [PK__News__954EBDF33FF1DC26] PRIMARY KEY ([NewsId])
);

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(255) NOT NULL,
    [Phone] nvarchar(20) NULL,
    [Gender] nvarchar(10) NULL,
    [BirthDate] date NULL,
    [Address] nvarchar(200) NULL,
    [NationalId] nvarchar(20) NULL,
    [ProfileImage] nvarchar(255) NULL,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Users__1788CC4C375C64C0] PRIMARY KEY ([UserId])
);

CREATE TABLE [CulturalAssociations] (
    [AssociationId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NULL,
    [PresidentName] nvarchar(100) NULL,
    [Phone] nvarchar(20) NULL,
    [Region] nvarchar(50) NULL,
    [FoundedYear] int NULL,
    [MembersCount] int NULL,
    [Description] nvarchar(max) NULL,
    [Status] nvarchar(20) NULL,
    [CategoryId] int NULL,
    [Email] nvarchar(150) NULL,
    CONSTRAINT [PK__Cultural__B51A182D69818CC2] PRIMARY KEY ([AssociationId]),
    CONSTRAINT [FK__CulturalA__Categ__0B91BA14] FOREIGN KEY ([CategoryId]) REFERENCES [AssociationCategories] ([CategoryId])
);

CREATE TABLE [Books] (
    [BookId] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Author] nvarchar(100) NULL,
    [Description] nvarchar(max) NULL,
    [CoverImageUrl] nvarchar(255) NULL,
    [PublishedYear] int NULL,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [IsAvailable] bit NOT NULL DEFAULT CAST(1 AS bit),
    [AvailableCopies] int NOT NULL DEFAULT 1,
    [CategoryId] int NULL,
    CONSTRAINT [PK__Books__3DE0C207B1E5AFF8] PRIMARY KEY ([BookId]),
    CONSTRAINT [FK__Books__CategoryI__5FB337D6] FOREIGN KEY ([CategoryId]) REFERENCES [BookCategories] ([CategoryId])
);

CREATE TABLE [CourseApplications] (
    [ApplicationId] int NOT NULL IDENTITY,
    [CourseId] int NOT NULL,
    [UserId] int NULL,
    [BirthCertificateImage] nvarchar(max) NULL,
    [Agreement] bit NOT NULL,
    [Status] nvarchar(20) NULL DEFAULT N'Pending',
    [SubmittedAt] datetime NULL DEFAULT ((getdate())),
    [RejectionReason] nvarchar(max) NULL,
    CONSTRAINT [PK__CourseAp__C93A4C998594821F] PRIMARY KEY ([ApplicationId]),
    CONSTRAINT [FK__CourseApp__Cours__3D5E1FD2] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]),
    CONSTRAINT [FK__CourseApp__UserI__3E52440B] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [CraftOrders] (
    [OrderId] int NOT NULL IDENTITY,
    [CraftId] int NULL,
    [UserId] int NULL,
    [Quantity] int NULL,
    [OrderDate] datetime NULL,
    [Status] nvarchar(20) NULL,
    CONSTRAINT [PK__CraftOrd__C3905BCF1AECE223] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK__CraftOrde__Craft__6E01572D] FOREIGN KEY ([CraftId]) REFERENCES [Handicrafts] ([CraftId]),
    CONSTRAINT [FK__CraftOrde__UserI__6EF57B66] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [CulturalEventReservations] (
    [ReservationId] int NOT NULL IDENTITY,
    [EventId] int NULL,
    [UserId] int NULL,
    [ReservationDate] datetime NULL DEFAULT ((getdate())),
    [AttendanceDate] date NOT NULL,
    [Status] nvarchar(20) NULL DEFAULT N'Pending',
    CONSTRAINT [PK__Cultural__B7EE5F24FEEC2269] PRIMARY KEY ([ReservationId]),
    CONSTRAINT [FK__CulturalE__Event__02FC7413] FOREIGN KEY ([EventId]) REFERENCES [CulturalEvents] ([EventId]),
    CONSTRAINT [FK__CulturalE__UserI__03F0984C] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [FestivalReservations] (
    [ReservationId] int NOT NULL IDENTITY,
    [FestivalId] int NULL,
    [UserId] int NULL,
    [ReservationDate] datetime NULL DEFAULT ((getdate())),
    [AttendanceDate] date NOT NULL,
    [Status] nvarchar(20) NULL DEFAULT N'Pending',
    CONSTRAINT [PK__Festival__B7EE5F24652004C9] PRIMARY KEY ([ReservationId]),
    CONSTRAINT [FK__FestivalR__Festi__7C4F7684] FOREIGN KEY ([FestivalId]) REFERENCES [Festivals] ([FestivalId]),
    CONSTRAINT [FK__FestivalR__UserI__7D439ABD] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [UserPosts] (
    [PostId] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Category] nvarchar(100) NULL,
    [Status] nvarchar(50) NULL DEFAULT N'Draft',
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [UpdatedAt] datetime NULL,
    [FilePath] nvarchar(300) NOT NULL DEFAULT N'',
    CONSTRAINT [PK__UserPost__AA12601821D6885E] PRIMARY KEY ([PostId]),
    CONSTRAINT [FK__UserPosts__UserI__17F790F9] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [WorkshopRegistrations] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [WorkshopId] int NOT NULL,
    [BirthCertificateImage] nvarchar(300) NULL,
    [Status] nvarchar(50) NOT NULL DEFAULT N'Pending',
    [RejectionReason] nvarchar(500) NULL,
    [CreatedAt] datetime NOT NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Workshop__3214EC07E8CD5DC1] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__WorkshopR__UserI__4D5F7D71] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]),
    CONSTRAINT [FK__WorkshopR__Works__4E53A1AA] FOREIGN KEY ([WorkshopId]) REFERENCES [ArtWorkshops] ([Id])
);

CREATE TABLE [AssociationEvents] (
    [EventId] int NOT NULL IDENTITY,
    [AssociationId] int NULL,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Date] date NOT NULL,
    [StartTime] time NULL,
    [EndTime] time NULL,
    [Location] nvarchar(100) NULL,
    [Status] nvarchar(20) NULL DEFAULT N'Pending',
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [PosterUrl] nvarchar(255) NULL,
    [EventType] nvarchar(50) NULL,
    CONSTRAINT [PK__Associat__68ACACD3D8A09147] PRIMARY KEY ([EventId]),
    CONSTRAINT [FK__Associati__Assoc__4E88ABD4] FOREIGN KEY ([AssociationId]) REFERENCES [CulturalAssociations] ([AssociationId])
);

CREATE TABLE [AssociationJoinRequests] (
    [RequestId] int NOT NULL IDENTITY,
    [AssociationId] int NULL,
    [UserId] int NULL,
    [Status] nvarchar(20) NULL DEFAULT N'Pending',
    CONSTRAINT [PK__Associat__33A8517A78397C99] PRIMARY KEY ([RequestId]),
    CONSTRAINT [FK__Associati__Assoc__49C3F6B7] FOREIGN KEY ([AssociationId]) REFERENCES [CulturalAssociations] ([AssociationId]),
    CONSTRAINT [FK__Associati__UserI__4AB81AF0] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [HallBookings] (
    [BookingId] int NOT NULL IDENTITY,
    [HallType] nvarchar(50) NULL,
    [EventDate] date NULL,
    [StartTime] time NULL,
    [EndTime] time NULL,
    [EventType] nvarchar(100) NULL,
    [EventTitle] nvarchar(200) NULL,
    [GuestOfHonor] nvarchar(200) NULL,
    [AccompanyingActivities] nvarchar(max) NULL,
    [NeedsScreens] bit NOT NULL,
    [NeedsProjector] bit NOT NULL,
    [NeedsAudienceInteraction] bit NOT NULL,
    [GuestCount] int NULL,
    [NeedsFilm] bit NOT NULL,
    [NeedsPCOnStage] bit NOT NULL,
    [NeedsDocumentation] bit NOT NULL,
    [OtherTechnicalNeeds] nvarchar(max) NULL,
    [CoordinatorName] nvarchar(100) NULL,
    [ResponsibleName] nvarchar(100) NULL,
    [RequestDate] date NULL,
    [Status] nvarchar(20) NULL DEFAULT N'Pending',
    [UserId] int NULL,
    [CultureOrgId] int NULL,
    [ContactPhone] nvarchar(20) NULL,
    [AttachmentPath] nvarchar(500) NULL,
    CONSTRAINT [PK__HallBook__73951AEDD43995BE] PRIMARY KEY ([BookingId]),
    CONSTRAINT [FK_HallBookings_Associations] FOREIGN KEY ([CultureOrgId]) REFERENCES [CulturalAssociations] ([AssociationId]),
    CONSTRAINT [FK_HallBookings_Users] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [BookReservations] (
    [ReservationId] int NOT NULL IDENTITY,
    [BookId] int NOT NULL,
    [UserId] int NULL,
    [ReservationDate] datetime NULL DEFAULT ((getdate())),
    [PickupDate] date NULL,
    [ReturnDate] date NULL,
    [Status] nvarchar(20) NOT NULL DEFAULT N'Pending',
    [Agreement] bit NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK__BookRese__B7EE5F2412BFE50A] PRIMARY KEY ([ReservationId]),
    CONSTRAINT [FK__BookReser__BookI__628FA481] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]),
    CONSTRAINT [FK__BookReser__UserI__6383C8BA] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE TABLE [AssocEventRegistrations] (
    [RegistrationId] int NOT NULL IDENTITY,
    [AssocEventId] int NULL,
    [UserId] int NULL,
    [RegisteredAt] datetime NULL,
    CONSTRAINT [PK__AssocEve__6EF58810AC60C884] PRIMARY KEY ([RegistrationId]),
    CONSTRAINT [FK__AssocEven__Assoc__534D60F1] FOREIGN KEY ([AssocEventId]) REFERENCES [AssociationEvents] ([EventId]),
    CONSTRAINT [FK__AssocEven__UserI__5441852A] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);

CREATE UNIQUE INDEX [UQ__Admins__536C85E4ACF02193] ON [Admins] ([Username]);

CREATE UNIQUE INDEX [UQ__Admins__A9D10534CB632101] ON [Admins] ([Email]);

CREATE INDEX [IX_AssocEventRegistrations_AssocEventId] ON [AssocEventRegistrations] ([AssocEventId]);

CREATE INDEX [IX_AssocEventRegistrations_UserId] ON [AssocEventRegistrations] ([UserId]);

CREATE UNIQUE INDEX [UQ__Associat__737584F670989B5A] ON [AssociationCategories] ([Name]);

CREATE INDEX [IX_AssociationEvents_AssociationId] ON [AssociationEvents] ([AssociationId]);

CREATE INDEX [IX_AssociationJoinRequests_AssociationId] ON [AssociationJoinRequests] ([AssociationId]);

CREATE INDEX [IX_AssociationJoinRequests_UserId] ON [AssociationJoinRequests] ([UserId]);

CREATE UNIQUE INDEX [UQ__BookCate__737584F6944A22CD] ON [BookCategories] ([Name]);

CREATE INDEX [IX_BookReservations_BookId] ON [BookReservations] ([BookId]);

CREATE INDEX [IX_BookReservations_UserId] ON [BookReservations] ([UserId]);

CREATE INDEX [IX_Books_CategoryId] ON [Books] ([CategoryId]);

CREATE INDEX [IX_CourseApplications_CourseId] ON [CourseApplications] ([CourseId]);

CREATE INDEX [IX_CourseApplications_UserId] ON [CourseApplications] ([UserId]);

CREATE INDEX [IX_CraftOrders_CraftId] ON [CraftOrders] ([CraftId]);

CREATE INDEX [IX_CraftOrders_UserId] ON [CraftOrders] ([UserId]);

CREATE INDEX [IX_CulturalAssociations_CategoryId] ON [CulturalAssociations] ([CategoryId]);

CREATE INDEX [IX_CulturalEventReservations_EventId] ON [CulturalEventReservations] ([EventId]);

CREATE INDEX [IX_CulturalEventReservations_UserId] ON [CulturalEventReservations] ([UserId]);

CREATE INDEX [IX_FestivalReservations_FestivalId] ON [FestivalReservations] ([FestivalId]);

CREATE INDEX [IX_FestivalReservations_UserId] ON [FestivalReservations] ([UserId]);

CREATE INDEX [IX_HallBookings_CultureOrgId] ON [HallBookings] ([CultureOrgId]);

CREATE INDEX [IX_HallBookings_UserId] ON [HallBookings] ([UserId]);

CREATE INDEX [IX_UserPosts_UserId] ON [UserPosts] ([UserId]);

CREATE UNIQUE INDEX [UQ__Users__A9D105345E76B9C9] ON [Users] ([Email]);

CREATE INDEX [IX_WorkshopRegistrations_UserId] ON [WorkshopRegistrations] ([UserId]);

CREATE INDEX [IX_WorkshopRegistrations_WorkshopId] ON [WorkshopRegistrations] ([WorkshopId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250622172646_InitialCreate', N'9.0.4');

COMMIT;
GO

