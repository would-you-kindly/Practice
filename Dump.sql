USE [master]
GO
/****** Object:  Database [Practice]    Script Date: 24.05.2017 16:29:18 ******/
CREATE DATABASE [Practice]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Practice', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Practice.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Practice_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Practice_log.ldf' , SIZE = 1040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Practice] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Practice].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Practice] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Practice] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Practice] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Practice] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Practice] SET ARITHABORT OFF 
GO
ALTER DATABASE [Practice] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Practice] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Practice] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Practice] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Practice] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Practice] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Practice] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Practice] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Practice] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Practice] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Practice] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Practice] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Practice] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Practice] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Practice] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Practice] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Practice] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Practice] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Practice] SET  MULTI_USER 
GO
ALTER DATABASE [Practice] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Practice] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Practice] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Practice] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Practice]
GO
/****** Object:  Table [dbo].[ContentTypes]    Script Date: 24.05.2017 16:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentTypes](
	[Id] [int] NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ContentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FileFormats]    Script Date: 24.05.2017 16:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileFormats](
	[Id] [int] NOT NULL,
	[Format] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_FileFormats] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Files]    Script Date: 24.05.2017 16:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Price] [float] NOT NULL,
	[ContentType_Id] [int] NOT NULL,
	[FileFormat_Id] [int] NOT NULL,
	[Creator_Id] [int] NOT NULL,
	[PdfFile_Id] [int] NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OwnerFile]    Script Date: 24.05.2017 16:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OwnerFile](
	[Owners_Id] [int] NOT NULL,
	[OwnedFiles_Id] [int] NOT NULL,
 CONSTRAINT [PK_OwnerFile] PRIMARY KEY CLUSTERED 
(
	[Owners_Id] ASC,
	[OwnedFiles_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Purchases]    Script Date: 24.05.2017 16:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchases](
	[Id] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[File_Id] [int] NOT NULL,
	[Purchaser_Id] [int] NOT NULL,
 CONSTRAINT [PK_Purchases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 24.05.2017 16:29:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (1, N'Description')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (2, N'Sound')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (3, N'Music')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (4, N'Picture')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (5, N'Model')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (6, N'Font')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (7, N'Animation')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (8, N'Film')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (1, N'pdf')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (2, N'png')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (3, N'jpg')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (4, N'psd')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (5, N'blend')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (6, N'3ds')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (7, N'fbx')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (8, N'mp3')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (9, N'ogg')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (10, N'flac')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (11, N'ttf')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (12, N'otf')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (13, N'gif')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (14, N'bpg')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (15, N'avi')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (16, N'mp4')
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (1, N'Plane', N'Description of plane', N'D:\HSE', 0, 1, 1, 5, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (2, N'Mario jumps', NULL, N'D:\My directory\Projects\Animations', 0, 1, 1, 7, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (3, N'Shot', N'Description of shot', N'D:\My directory\Sounds', 0, 1, 1, 8, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (4, N'Chocolate', N'Description', N'D:\My directory\Instructions\Visio', 0, 1, 1, 8, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (5, N'Book', N'Paper', N'D:\My directory\Library', 1.79, 4, 3, 1, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (6, N'Birds singing', N'Beautiful', N'D:\My directory\Test', 2.25, 3, 9, 1, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (7, N'Windows greeting', NULL, N'C:\Windows', 0.9, 2, 10, 4, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (8, N'Chocolate', N'Delicious', N'D:\My directory\Instructions\Visio', 5.2, 5, 5, 8, 4)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (9, N'Lamp', N'Bright', N'D:\My directory\Icons', 4, 4, 2, 3, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (10, N'Mario jumps', N'Skeletal animation', N'D:\My directory\Projects\Animations', 2, 7, 14, 7, 2)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (11, N'Unity logo', N'Game development', N'D:\My directory\Projects\Unity', 6, 8, 16, 8, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (12, N'Train', N'Iron', N'D:\HSE\Third course', 3.5, 5, 6, 6, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (13, N'Mario runs', N'Sprite animation', N'D:\My directory\Projects', 3, 7, 13, 1, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (14, N'Suzanne', NULL, N'C:\PABCWork.NET', 3, 5, 5, 5, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (15, N'Polaroid', N'Glossy', N'D:\My directory\Photos', 2.99, 4, 4, 6, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (16, N'Shot', N'Pistol says', N'D:\My directory\Sounds', 2, 2, 8, 8, 3)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (17, N'Alarm Clock', N'Loud', N'D:\My directory\Projects\My models\Blender', 7, 5, 7, 1, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (18, N'Plane', NULL, N'D:\HSE', 1.25, 4, 4, 5, 1)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (19, N'Curve handwriting', N'Professor', N'C:\Windows\Fonts', 0.8, 6, 12, 7, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (20, N'Meow', N'Cat says', N'D:\My directory\Sounds', 1, 2, 9, 3, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (21, N'BioShock soundtrack', NULL, N'D:\Steam\BioShock', 8.99, 3, 8, 3, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (22, N'Reloading', N'Pistol is reloaded', N'D:\My directory', 4, 2, 10, 8, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (23, N'Calibri', N'Standard font', N'C:\Windows\Fonts', 1.2, 6, 11, 4, NULL)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id], [Creator_Id], [PdfFile_Id]) VALUES (24, N'Valve intro', N'Valve eye', N'D:\Steam\Half-Life 2', 2.7, 8, 16, 7, NULL)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (2, 1)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (7, 3)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (7, 4)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (2, 5)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (4, 6)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (7, 7)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (2, 9)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (4, 9)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (2, 11)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (4, 12)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (2, 13)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (1, 14)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (4, 14)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (6, 15)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (6, 16)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (1, 17)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (2, 19)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (6, 19)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (1, 20)
INSERT [dbo].[OwnerFile] ([Owners_Id], [OwnedFiles_Id]) VALUES (3, 20)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [Purchaser_Id]) VALUES (1, CAST(N'2016-12-20T00:00:00.000' AS DateTime), 15, 5)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [Purchaser_Id]) VALUES (2, CAST(N'2016-03-15T00:00:00.000' AS DateTime), 5, 7)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [Purchaser_Id]) VALUES (3, CAST(N'2016-08-17T00:00:00.000' AS DateTime), 19, 8)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [Purchaser_Id]) VALUES (4, CAST(N'2016-04-26T00:00:00.000' AS DateTime), 4, 1)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [Purchaser_Id]) VALUES (5, CAST(N'2016-04-01T00:00:00.000' AS DateTime), 9, 6)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [Purchaser_Id]) VALUES (6, CAST(N'2016-04-26T00:00:00.000' AS DateTime), 24, 1)
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (1, N'Isaac', N'isaac@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (2, N'Michael', N'michael@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (3, N'William', N'william@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (4, N'David', N'david@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (5, N'John', N'john@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (6, N'Jose', N'jose@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (7, N'Caleb', N'caleb@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (8, N'Robert', N'robert@gmail.com')
/****** Object:  Index [IX_FK_ContentTypeFile]    Script Date: 24.05.2017 16:29:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_ContentTypeFile] ON [dbo].[Files]
(
	[ContentType_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_CreatorFile]    Script Date: 24.05.2017 16:29:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_CreatorFile] ON [dbo].[Files]
(
	[Creator_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_FileFile]    Script Date: 24.05.2017 16:29:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_FileFile] ON [dbo].[Files]
(
	[PdfFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_FileFormatFile]    Script Date: 24.05.2017 16:29:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_FileFormatFile] ON [dbo].[Files]
(
	[FileFormat_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_OwnerFile_File]    Script Date: 24.05.2017 16:29:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_OwnerFile_File] ON [dbo].[OwnerFile]
(
	[OwnedFiles_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_FilePurchase]    Script Date: 24.05.2017 16:29:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_FilePurchase] ON [dbo].[Purchases]
(
	[File_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserPurchase]    Script Date: 24.05.2017 16:29:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserPurchase] ON [dbo].[Purchases]
(
	[Purchaser_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_ContentTypeFile] FOREIGN KEY([ContentType_Id])
REFERENCES [dbo].[ContentTypes] ([Id])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_ContentTypeFile]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_CreatorFile] FOREIGN KEY([Creator_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_CreatorFile]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_FileFile] FOREIGN KEY([PdfFile_Id])
REFERENCES [dbo].[Files] ([Id])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_FileFile]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_FileFormatFile] FOREIGN KEY([FileFormat_Id])
REFERENCES [dbo].[FileFormats] ([Id])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_FileFormatFile]
GO
ALTER TABLE [dbo].[OwnerFile]  WITH CHECK ADD  CONSTRAINT [FK_OwnerFile_File] FOREIGN KEY([OwnedFiles_Id])
REFERENCES [dbo].[Files] ([Id])
GO
ALTER TABLE [dbo].[OwnerFile] CHECK CONSTRAINT [FK_OwnerFile_File]
GO
ALTER TABLE [dbo].[OwnerFile]  WITH CHECK ADD  CONSTRAINT [FK_OwnerFile_Owner] FOREIGN KEY([Owners_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[OwnerFile] CHECK CONSTRAINT [FK_OwnerFile_Owner]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_FilePurchase] FOREIGN KEY([File_Id])
REFERENCES [dbo].[Files] ([Id])
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_FilePurchase]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_UserPurchase] FOREIGN KEY([Purchaser_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_UserPurchase]
GO
USE [master]
GO
ALTER DATABASE [Practice] SET  READ_WRITE 
GO
