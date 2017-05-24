USE [master]
GO
/****** Object:  Database [Files]    Script Date: 23.05.2017 16:41:57 ******/
CREATE DATABASE [Files]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Files', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Files.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Files_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Files_log.ldf' , SIZE = 1040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Files] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Files].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Files] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Files] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Files] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Files] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Files] SET ARITHABORT OFF 
GO
ALTER DATABASE [Files] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Files] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Files] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Files] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Files] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Files] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Files] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Files] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Files] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Files] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Files] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Files] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Files] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Files] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Files] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Files] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Files] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Files] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Files] SET  MULTI_USER 
GO
ALTER DATABASE [Files] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Files] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Files] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Files] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Files]
GO
/****** Object:  Table [dbo].[ContentTypes]    Script Date: 23.05.2017 16:41:58 ******/
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
/****** Object:  Table [dbo].[FileFormats]    Script Date: 23.05.2017 16:41:58 ******/
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
/****** Object:  Table [dbo].[Files]    Script Date: 23.05.2017 16:41:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Price] [float] NOT NULL,
	[ContentType_Id] [int] NOT NULL,
	[FileFormat_Id] [int] NOT NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Purchases]    Script Date: 23.05.2017 16:41:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchases](
	[Id] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[File_Id] [int] NOT NULL,
	[User_Id] [int] NOT NULL,
 CONSTRAINT [PK_Purchases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserFile]    Script Date: 23.05.2017 16:41:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFile](
	[Users_Id] [int] NOT NULL,
	[Files_Id] [int] NOT NULL,
 CONSTRAINT [PK_UserFile] PRIMARY KEY CLUSTERED 
(
	[Users_Id] ASC,
	[Files_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 23.05.2017 16:41:58 ******/
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
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (1, N'Sound')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (2, N'Music')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (3, N'Picture')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (4, N'Model')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (5, N'Font')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (6, N'Animation')
INSERT [dbo].[ContentTypes] ([Id], [Type]) VALUES (7, N'Film')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (1, N'png')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (2, N'jpg')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (3, N'psd')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (4, N'blend')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (5, N'3ds')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (6, N'fbx')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (7, N'mp3')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (8, N'ogg')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (9, N'flac')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (10, N'ttf')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (11, N'otf')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (12, N'gif')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (13, N'bpg')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (14, N'avi')
INSERT [dbo].[FileFormats] ([Id], [Format]) VALUES (15, N'mp4')
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (1, N'Book', N'Paper', N'D:\My directory\Library', 1.79, 3, 2)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (2, N'Birds singing', N'Beautiful', N'D:\My directory\Test', 2.25, 2, 8)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (3, N'Windows greeting', N'Notebook says', N'C:\Windows', 0.9, 1, 9)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (4, N'Chocolate', N'Delicious', N'D:\My directory\Instructions\Visio', 5.2, 4, 4)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (5, N'Lamp', N'Bright', N'D:\My directory\Icons', 4, 3, 1)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (6, N'Mario jumps', N'Skeletal animation', N'D:\My directory\Projects\Animations', 2, 6, 13)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (7, N'Unity logo', N'Game development', N'D:\My directory\Projects\Unity', 6, 7, 15)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (8, N'Train', N'Iron', N'D:\HSE\Third course', 3.5, 4, 5)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (9, N'Mario runs', N'Sprite animation', N'D:\My directory\Projects', 3, 6, 12)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (10, N'Suzanne', N'Monkey', N'C:\PABCWork.NET', 3, 4, 4)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (11, N'Polaroid', N'Glossy', N'D:\My directory\Photos', 2.99, 3, 3)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (12, N'Shot', N'Pistol says', N'D:\My directory\Sounds', 2, 1, 7)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (13, N'Alarm Clock', N'Loud', N'D:\My directory\Projects\My models\Blender', 7, 4, 6)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (14, N'Plane', N'Flies', N'D:\HSE', 1.25, 3, 3)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (15, N'Curve handwriting', N'Professor', N'C:\Windows\Fonts', 0.8, 5, 11)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (16, N'Meow', N'Cat says', N'D:\My directory\Sounds', 1, 1, 8)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (17, N'BioShock soundtrack', N'Cool', N'D:\Steam\BioShock', 8.99, 2, 7)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (18, N'Reloading', N'Pistol is reloaded', N'D:\My directory', 4, 1, 9)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (19, N'Calibri', N'Standard font', N'C:\Windows\Fonts', 1.2, 5, 10)
INSERT [dbo].[Files] ([Id], [Name], [Description], [Url], [Price], [ContentType_Id], [FileFormat_Id]) VALUES (20, N'Valve intro', N'Valve eye', N'D:\Steam\Half-Life 2', 2.7, 7, 15)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [User_Id]) VALUES (1, CAST(N'2016-12-20T00:00:00.000' AS DateTime), 15, 5)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [User_Id]) VALUES (2, CAST(N'2016-03-15T00:00:00.000' AS DateTime), 5, 7)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [User_Id]) VALUES (3, CAST(N'2016-08-17T00:00:00.000' AS DateTime), 19, 8)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [User_Id]) VALUES (4, CAST(N'2016-04-26T00:00:00.000' AS DateTime), 4, 1)
INSERT [dbo].[Purchases] ([Id], [Date], [File_Id], [User_Id]) VALUES (5, CAST(N'2016-04-01T00:00:00.000' AS DateTime), 9, 6)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (2, 1)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (7, 3)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (7, 4)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (2, 5)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (4, 6)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (7, 7)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (2, 9)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (4, 9)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (2, 11)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (4, 12)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (2, 13)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (1, 14)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (4, 14)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (6, 15)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (6, 16)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (1, 17)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (2, 19)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (6, 19)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (1, 20)
INSERT [dbo].[UserFile] ([Users_Id], [Files_Id]) VALUES (3, 20)
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (1, N'Isaac', N'isaac@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (2, N'Michael', N'michael@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (3, N'William', N'william@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (4, N'David', N'david@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (5, N'John', N'john@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (6, N'Jose', N'jose@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (7, N'Caleb', N'caleb@gmail.com')
INSERT [dbo].[Users] ([Id], [Name], [Email]) VALUES (8, N'Robert', N'robert@gmail.com')
/****** Object:  Index [IX_FK_ContentTypeFile]    Script Date: 23.05.2017 16:41:58 ******/
CREATE NONCLUSTERED INDEX [IX_FK_ContentTypeFile] ON [dbo].[Files]
(
	[ContentType_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_FileFormatFile]    Script Date: 23.05.2017 16:41:58 ******/
CREATE NONCLUSTERED INDEX [IX_FK_FileFormatFile] ON [dbo].[Files]
(
	[FileFormat_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_FilePurchase]    Script Date: 23.05.2017 16:41:58 ******/
CREATE NONCLUSTERED INDEX [IX_FK_FilePurchase] ON [dbo].[Purchases]
(
	[File_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserPurchase]    Script Date: 23.05.2017 16:41:58 ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserPurchase] ON [dbo].[Purchases]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserFile_File]    Script Date: 23.05.2017 16:41:58 ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserFile_File] ON [dbo].[UserFile]
(
	[Files_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_ContentTypeFile] FOREIGN KEY([ContentType_Id])
REFERENCES [dbo].[ContentTypes] ([Id])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_ContentTypeFile]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_FileFormatFile] FOREIGN KEY([FileFormat_Id])
REFERENCES [dbo].[FileFormats] ([Id])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_FileFormatFile]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_FilePurchase] FOREIGN KEY([File_Id])
REFERENCES [dbo].[Files] ([Id])
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_FilePurchase]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_UserPurchase] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_UserPurchase]
GO
ALTER TABLE [dbo].[UserFile]  WITH CHECK ADD  CONSTRAINT [FK_UserFile_File] FOREIGN KEY([Files_Id])
REFERENCES [dbo].[Files] ([Id])
GO
ALTER TABLE [dbo].[UserFile] CHECK CONSTRAINT [FK_UserFile_File]
GO
ALTER TABLE [dbo].[UserFile]  WITH CHECK ADD  CONSTRAINT [FK_UserFile_User] FOREIGN KEY([Users_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserFile] CHECK CONSTRAINT [FK_UserFile_User]
GO
USE [master]
GO
ALTER DATABASE [Files] SET  READ_WRITE 
GO
