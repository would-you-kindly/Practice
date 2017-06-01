USE [master]
GO
/****** Object:  Database [FlexberryPractice]    Script Date: 02.06.2017 2:43:43 ******/
CREATE DATABASE [FlexberryPractice]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FlexberryPractice', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\FlexberryPractice.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FlexberryPractice_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\FlexberryPractice_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FlexberryPractice] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FlexberryPractice].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FlexberryPractice] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FlexberryPractice] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FlexberryPractice] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FlexberryPractice] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FlexberryPractice] SET ARITHABORT OFF 
GO
ALTER DATABASE [FlexberryPractice] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FlexberryPractice] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FlexberryPractice] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FlexberryPractice] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FlexberryPractice] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FlexberryPractice] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FlexberryPractice] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FlexberryPractice] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FlexberryPractice] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FlexberryPractice] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FlexberryPractice] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FlexberryPractice] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FlexberryPractice] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FlexberryPractice] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FlexberryPractice] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FlexberryPractice] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FlexberryPractice] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FlexberryPractice] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FlexberryPractice] SET  MULTI_USER 
GO
ALTER DATABASE [FlexberryPractice] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FlexberryPractice] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FlexberryPractice] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FlexberryPractice] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [FlexberryPractice]
GO
/****** Object:  Table [dbo].[ApplicationLog]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationLog](
	[PrimaryKey] [int] NOT NULL,
	[Category] [varchar](64) NULL,
	[EventId] [int] NULL,
	[Priority] [int] NULL,
	[Severity] [varchar](32) NULL,
	[Title] [varchar](256) NULL,
	[Timestamp] [datetime] NULL,
	[MachineName] [varchar](32) NULL,
	[AppDomainName] [varchar](512) NULL,
	[ProcessId] [varchar](256) NULL,
	[ProcessName] [varchar](512) NULL,
	[ThreadName] [varchar](512) NULL,
	[Win32ThreadId] [varchar](128) NULL,
	[Message] [varchar](2500) NULL,
	[FormattedMessage] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContentTypes]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentTypes](
	[PrimaryKey] [int] NOT NULL,
	[Type] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Files]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[PrimaryKey] [int] NOT NULL,
	[Description] [varchar](255) NULL,
	[Url] [varchar](255) NOT NULL,
	[Price] [float] NOT NULL,
	[ContentType_Id] [int] NOT NULL,
	[PdfFile_Id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FileUser]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUser](
	[Files_Id] [int] NOT NULL,
	[Users_Id] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Operations]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operations](
	[PrimaryKey] [int] NOT NULL,
	[SomeValue] [int] NOT NULL,
	[SecondaryFile_Id] [int] NULL,
	[MainFile_Id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Purchases]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchases](
	[PrimaryKey] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[User_Id] [int] NOT NULL,
	[File_Id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Session]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Session](
	[PrimaryKey] [int] NOT NULL,
	[UserKey] [int] NULL,
	[StartedAt] [datetime] NULL,
	[LastAccess] [datetime] NULL,
	[Closed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMAC]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMAC](
	[PrimaryKey] [int] NOT NULL,
	[TypeAccess] [varchar](7) NULL,
	[Filter_m0] [int] NULL,
	[Permition_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMAdvLimit]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMAdvLimit](
	[PrimaryKey] [int] NOT NULL,
	[User] [varchar](255) NULL,
	[Published] [bit] NULL,
	[Module] [varchar](255) NULL,
	[Name] [varchar](255) NULL,
	[Value] [text] NULL,
	[HotKeyData] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMAG]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMAG](
	[PrimaryKey] [int] NOT NULL,
	[Name] [varchar](80) NOT NULL,
	[Login] [varchar](50) NULL,
	[Pwd] [varchar](50) NULL,
	[IsUser] [bit] NOT NULL,
	[IsGroup] [bit] NOT NULL,
	[IsRole] [bit] NOT NULL,
	[ConnString] [varchar](255) NULL,
	[Enabled] [bit] NULL,
	[Email] [varchar](80) NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMF]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMF](
	[PrimaryKey] [int] NOT NULL,
	[FilterText] [varchar](max) NULL,
	[Name] [varchar](255) NULL,
	[FilterTypeNView] [varchar](255) NULL,
	[Subject_m0] [int] NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMFILTERDETAIL]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMFILTERDETAIL](
	[PrimaryKey] [int] NOT NULL,
	[Caption] [varchar](255) NOT NULL,
	[DataObjectView] [varchar](255) NOT NULL,
	[ConnectMasterProp] [varchar](255) NOT NULL,
	[OwnerConnectProp] [varchar](255) NULL,
	[FilterSetting_m0] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMFILTERLOOKUP]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMFILTERLOOKUP](
	[PrimaryKey] [int] NOT NULL,
	[DataObjectType] [varchar](255) NOT NULL,
	[Container] [varchar](255) NULL,
	[ContainerTag] [varchar](255) NULL,
	[FieldsToView] [varchar](255) NULL,
	[FilterSetting_m0] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMFILTERSETTING]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMFILTERSETTING](
	[PrimaryKey] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[DataObjectView] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMI]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMI](
	[PrimaryKey] [int] NOT NULL,
	[User_m0] [int] NOT NULL,
	[Agent_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMLA]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMLA](
	[PrimaryKey] [int] NOT NULL,
	[View_m0] [int] NOT NULL,
	[Attribute_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMLG]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMLG](
	[PrimaryKey] [int] NOT NULL,
	[Group_m0] [int] NOT NULL,
	[User_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMLO]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMLO](
	[PrimaryKey] [int] NOT NULL,
	[Class_m0] [int] NOT NULL,
	[Operation_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMLR]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMLR](
	[PrimaryKey] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Agent_m0] [int] NOT NULL,
	[Role_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMLV]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMLV](
	[PrimaryKey] [int] NOT NULL,
	[Class_m0] [int] NOT NULL,
	[View_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMNETLOCKDATA]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMNETLOCKDATA](
	[LockKey] [varchar](300) NOT NULL,
	[UserName] [varchar](300) NOT NULL,
	[LockDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LockKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMP]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMP](
	[PrimaryKey] [int] NOT NULL,
	[Subject_m0] [int] NOT NULL,
	[Agent_m0] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMS]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMS](
	[PrimaryKey] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Type] [varchar](100) NULL,
	[IsAttribute] [bit] NOT NULL,
	[IsOperation] [bit] NOT NULL,
	[IsView] [bit] NOT NULL,
	[IsClass] [bit] NOT NULL,
	[SharedOper] [bit] NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](255) NULL,
	[EditTime] [datetime] NULL,
	[Editor] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMSETTINGS]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMSETTINGS](
	[PrimaryKey] [int] NOT NULL,
	[Module] [varchar](1000) NULL,
	[Name] [varchar](255) NULL,
	[Value] [text] NULL,
	[User] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STORMWEBSEARCH]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STORMWEBSEARCH](
	[PrimaryKey] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Order] [int] NOT NULL,
	[PresentView] [varchar](255) NOT NULL,
	[DetailedView] [varchar](255) NOT NULL,
	[FilterSetting_m0] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[PrimaryKey] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserSetting]    Script Date: 02.06.2017 2:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSetting](
	[PrimaryKey] [int] NOT NULL,
	[AppName] [varchar](256) NULL,
	[UserName] [varchar](512) NULL,
	[UserGuid] [int] NULL,
	[ModuleName] [varchar](1024) NULL,
	[ModuleGuid] [int] NULL,
	[SettName] [varchar](256) NULL,
	[SettGuid] [int] NULL,
	[SettLastAccessTime] [datetime] NULL,
	[StrVal] [varchar](256) NULL,
	[TxtVal] [varchar](max) NULL,
	[IntVal] [int] NULL,
	[BoolVal] [bit] NULL,
	[GuidVal] [int] NULL,
	[DecimalVal] [decimal](20, 10) NULL,
	[DateTimeVal] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[PrimaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (1, N'Description')
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (2, N'Sound')
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (3, N'Music')
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (4, N'Picture')
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (5, N'Model')
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (6, N'Font')
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (7, N'Animation')
INSERT [dbo].[ContentTypes] ([PrimaryKey], [Type]) VALUES (8, N'Film')
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (1, N'Description of plane', N'\Pictures\Plane.pdf', 0, 1, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (2, N'', N'\Animations\Mario jumps.pdf', 0, 1, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (3, N'Description of shot', N'\Sounds\Shot.pdf', 0, 1, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (4, N'Description', N'\Models\Chocolate.pdf', 0, 1, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (5, N'Paper', N'\Pictures\Book.jpg', 1.79, 4, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (6, N'Beautiful', N'\Sounds\Birds singing.ogg', 2.25, 3, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (7, N'', N'\Sounds\Windows greeting.flac', 0.9, 2, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (8, N'Delicious', N'\Models\Chocolate.blend', 5.2, 5, 4)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (9, N'Bright', N'\Pictures\Lamp.png', 4, 4, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (10, N'Skeletal animation', N'\Animations\Mario jumps.bpg', 2, 7, 2)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (11, N'Game development', N'\Animations\Films\Unity logo.mp4', 6, 8, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (12, N'Iron', N'\Models\Train.3ds', 3.5, 5, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (13, N'Sprite animation', N'\Animations\Mario runs.gif', 3, 7, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (14, N'', N'\Models\Suzanne.blend', 3, 5, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (15, N'Glossy', N'\Pictures\Polaroid.psd', 2.99, 4, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (16, N'Pistol says', N'\Sounds\Shot.mp3', 2, 2, 3)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (17, N'Loud', N'\Models\Alarm Clock.fbx', 7, 5, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (18, N'', N'\Pictures\Plane.psd', 1.25, 4, 1)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (19, N'Professor', N'\Curve handwriting.otf', 0.8, 6, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (20, N'Cat says', N'\Sounds\Meow.ogg', 1, 2, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (21, N'', N'\Sounds\Soundtracks\BioShock soundtrack.mp3', 8.99, 3, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (22, N'Pistol is reloaded', N'\Sounds\Reloading.flac', 4, 2, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (23, N'Standard font', N'\Calibri.ttf', 1.2, 6, NULL)
INSERT [dbo].[Files] ([PrimaryKey], [Description], [Url], [Price], [ContentType_Id], [PdfFile_Id]) VALUES (24, N'Valve eye', N'\Animations\Films\Valve intro.mp4', 2.7, 8, NULL)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (14, 4)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (16, 6)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (1, 2)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (20, 3)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (20, 1)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (9, 4)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (17, 1)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (13, 2)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (3, 7)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (6, 4)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (15, 6)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (7, 7)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (19, 6)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (11, 2)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (5, 2)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (4, 7)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (19, 2)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (12, 4)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (9, 2)
INSERT [dbo].[FileUser] ([Files_Id], [Users_Id]) VALUES (14, 1)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (1, 10, 9, 5)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (2, 12, 12, 8)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (3, 15, NULL, 12)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (4, 10, 16, 7)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (5, 8, NULL, 2)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (6, 10, 22, 20)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (7, 15, NULL, 19)
INSERT [dbo].[Operations] ([PrimaryKey], [SomeValue], [SecondaryFile_Id], [MainFile_Id]) VALUES (8, 20, 8, 14)
INSERT [dbo].[Purchases] ([PrimaryKey], [Date], [User_Id], [File_Id]) VALUES (1, CAST(N'2016-12-20T00:00:00.000' AS DateTime), 5, 15)
INSERT [dbo].[Purchases] ([PrimaryKey], [Date], [User_Id], [File_Id]) VALUES (2, CAST(N'2016-03-15T00:00:00.000' AS DateTime), 7, 5)
INSERT [dbo].[Purchases] ([PrimaryKey], [Date], [User_Id], [File_Id]) VALUES (3, CAST(N'2016-08-17T00:00:00.000' AS DateTime), 8, 19)
INSERT [dbo].[Purchases] ([PrimaryKey], [Date], [User_Id], [File_Id]) VALUES (4, CAST(N'2016-04-26T00:00:00.000' AS DateTime), 1, 4)
INSERT [dbo].[Purchases] ([PrimaryKey], [Date], [User_Id], [File_Id]) VALUES (5, CAST(N'2016-04-01T00:00:00.000' AS DateTime), 6, 9)
INSERT [dbo].[Purchases] ([PrimaryKey], [Date], [User_Id], [File_Id]) VALUES (6, CAST(N'2016-04-26T00:00:00.000' AS DateTime), 1, 24)
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (1, N'Isaac', N'isaac@gmail.com')
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (2, N'Michael', N'michael@gmail.com')
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (3, N'William', N'william@gmail.com')
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (4, N'David', N'david@gmail.com')
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (5, N'John', N'john@gmail.com')
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (6, N'Jose', N'jose@gmail.com')
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (7, N'Caleb', N'caleb@gmail.com')
INSERT [dbo].[Users] ([PrimaryKey], [Name], [Email]) VALUES (8, N'Robert', N'robert@gmail.com')
/****** Object:  Index [Files_IContentType_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [Files_IContentType_Id] ON [dbo].[Files]
(
	[ContentType_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Files_IPdfFile_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [Files_IPdfFile_Id] ON [dbo].[Files]
(
	[PdfFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [FileUser_IFiles_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [FileUser_IFiles_Id] ON [dbo].[FileUser]
(
	[Files_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [FileUser_IUsers_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [FileUser_IUsers_Id] ON [dbo].[FileUser]
(
	[Users_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Operations_IMainFile_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [Operations_IMainFile_Id] ON [dbo].[Operations]
(
	[MainFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Operations_ISecondaryFile_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [Operations_ISecondaryFile_Id] ON [dbo].[Operations]
(
	[SecondaryFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Purchases_IFile_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [Purchases_IFile_Id] ON [dbo].[Purchases]
(
	[File_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Purchases_IUser_Id]    Script Date: 02.06.2017 2:43:44 ******/
CREATE NONCLUSTERED INDEX [Purchases_IUser_Id] ON [dbo].[Purchases]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [Files_FContentTypes_0] FOREIGN KEY([ContentType_Id])
REFERENCES [dbo].[ContentTypes] ([PrimaryKey])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [Files_FContentTypes_0]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [Files_FFiles_0] FOREIGN KEY([PdfFile_Id])
REFERENCES [dbo].[Files] ([PrimaryKey])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [Files_FFiles_0]
GO
ALTER TABLE [dbo].[FileUser]  WITH CHECK ADD  CONSTRAINT [FileUser_FFiles_0] FOREIGN KEY([Files_Id])
REFERENCES [dbo].[Files] ([PrimaryKey])
GO
ALTER TABLE [dbo].[FileUser] CHECK CONSTRAINT [FileUser_FFiles_0]
GO
ALTER TABLE [dbo].[FileUser]  WITH CHECK ADD  CONSTRAINT [FileUser_FUsers_0] FOREIGN KEY([Users_Id])
REFERENCES [dbo].[Users] ([PrimaryKey])
GO
ALTER TABLE [dbo].[FileUser] CHECK CONSTRAINT [FileUser_FUsers_0]
GO
ALTER TABLE [dbo].[Operations]  WITH CHECK ADD  CONSTRAINT [Operations_FFiles_0] FOREIGN KEY([SecondaryFile_Id])
REFERENCES [dbo].[Files] ([PrimaryKey])
GO
ALTER TABLE [dbo].[Operations] CHECK CONSTRAINT [Operations_FFiles_0]
GO
ALTER TABLE [dbo].[Operations]  WITH CHECK ADD  CONSTRAINT [Operations_FFiles_1] FOREIGN KEY([MainFile_Id])
REFERENCES [dbo].[Files] ([PrimaryKey])
GO
ALTER TABLE [dbo].[Operations] CHECK CONSTRAINT [Operations_FFiles_1]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [Purchases_FFiles_0] FOREIGN KEY([File_Id])
REFERENCES [dbo].[Files] ([PrimaryKey])
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [Purchases_FFiles_0]
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [Purchases_FUsers_0] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([PrimaryKey])
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [Purchases_FUsers_0]
GO
ALTER TABLE [dbo].[STORMAC]  WITH CHECK ADD  CONSTRAINT [STORMAC_FSTORMF_0] FOREIGN KEY([Filter_m0])
REFERENCES [dbo].[STORMF] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMAC] CHECK CONSTRAINT [STORMAC_FSTORMF_0]
GO
ALTER TABLE [dbo].[STORMAC]  WITH CHECK ADD  CONSTRAINT [STORMAC_FSTORMP_0] FOREIGN KEY([Permition_m0])
REFERENCES [dbo].[STORMP] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMAC] CHECK CONSTRAINT [STORMAC_FSTORMP_0]
GO
ALTER TABLE [dbo].[STORMF]  WITH CHECK ADD  CONSTRAINT [STORMF_FSTORMS_0] FOREIGN KEY([Subject_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMF] CHECK CONSTRAINT [STORMF_FSTORMS_0]
GO
ALTER TABLE [dbo].[STORMFILTERDETAIL]  WITH CHECK ADD  CONSTRAINT [STORMFILTERDETAIL_FSTORMFILTERSETTING_0] FOREIGN KEY([FilterSetting_m0])
REFERENCES [dbo].[STORMFILTERSETTING] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMFILTERDETAIL] CHECK CONSTRAINT [STORMFILTERDETAIL_FSTORMFILTERSETTING_0]
GO
ALTER TABLE [dbo].[STORMFILTERLOOKUP]  WITH CHECK ADD  CONSTRAINT [STORMFILTERLOOKUP_FSTORMFILTERSETTING_0] FOREIGN KEY([FilterSetting_m0])
REFERENCES [dbo].[STORMFILTERSETTING] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMFILTERLOOKUP] CHECK CONSTRAINT [STORMFILTERLOOKUP_FSTORMFILTERSETTING_0]
GO
ALTER TABLE [dbo].[STORMI]  WITH CHECK ADD  CONSTRAINT [STORMI_FSTORMAG_0] FOREIGN KEY([User_m0])
REFERENCES [dbo].[STORMAG] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMI] CHECK CONSTRAINT [STORMI_FSTORMAG_0]
GO
ALTER TABLE [dbo].[STORMI]  WITH CHECK ADD  CONSTRAINT [STORMI_FSTORMAG_1] FOREIGN KEY([Agent_m0])
REFERENCES [dbo].[STORMAG] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMI] CHECK CONSTRAINT [STORMI_FSTORMAG_1]
GO
ALTER TABLE [dbo].[STORMLA]  WITH CHECK ADD  CONSTRAINT [STORMLA_FSTORMS_0] FOREIGN KEY([View_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLA] CHECK CONSTRAINT [STORMLA_FSTORMS_0]
GO
ALTER TABLE [dbo].[STORMLA]  WITH CHECK ADD  CONSTRAINT [STORMLA_FSTORMS_1] FOREIGN KEY([Attribute_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLA] CHECK CONSTRAINT [STORMLA_FSTORMS_1]
GO
ALTER TABLE [dbo].[STORMLG]  WITH CHECK ADD  CONSTRAINT [STORMLG_FSTORMAG_0] FOREIGN KEY([Group_m0])
REFERENCES [dbo].[STORMAG] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLG] CHECK CONSTRAINT [STORMLG_FSTORMAG_0]
GO
ALTER TABLE [dbo].[STORMLG]  WITH CHECK ADD  CONSTRAINT [STORMLG_FSTORMAG_1] FOREIGN KEY([User_m0])
REFERENCES [dbo].[STORMAG] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLG] CHECK CONSTRAINT [STORMLG_FSTORMAG_1]
GO
ALTER TABLE [dbo].[STORMLO]  WITH CHECK ADD  CONSTRAINT [STORMLO_FSTORMS_0] FOREIGN KEY([Class_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLO] CHECK CONSTRAINT [STORMLO_FSTORMS_0]
GO
ALTER TABLE [dbo].[STORMLO]  WITH CHECK ADD  CONSTRAINT [STORMLO_FSTORMS_1] FOREIGN KEY([Operation_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLO] CHECK CONSTRAINT [STORMLO_FSTORMS_1]
GO
ALTER TABLE [dbo].[STORMLR]  WITH CHECK ADD  CONSTRAINT [STORMLR_FSTORMAG_0] FOREIGN KEY([Agent_m0])
REFERENCES [dbo].[STORMAG] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLR] CHECK CONSTRAINT [STORMLR_FSTORMAG_0]
GO
ALTER TABLE [dbo].[STORMLR]  WITH CHECK ADD  CONSTRAINT [STORMLR_FSTORMAG_1] FOREIGN KEY([Role_m0])
REFERENCES [dbo].[STORMAG] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLR] CHECK CONSTRAINT [STORMLR_FSTORMAG_1]
GO
ALTER TABLE [dbo].[STORMLV]  WITH CHECK ADD  CONSTRAINT [STORMLV_FSTORMS_0] FOREIGN KEY([Class_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLV] CHECK CONSTRAINT [STORMLV_FSTORMS_0]
GO
ALTER TABLE [dbo].[STORMLV]  WITH CHECK ADD  CONSTRAINT [STORMLV_FSTORMS_1] FOREIGN KEY([View_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMLV] CHECK CONSTRAINT [STORMLV_FSTORMS_1]
GO
ALTER TABLE [dbo].[STORMP]  WITH CHECK ADD  CONSTRAINT [STORMP_FSTORMAG_0] FOREIGN KEY([Agent_m0])
REFERENCES [dbo].[STORMAG] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMP] CHECK CONSTRAINT [STORMP_FSTORMAG_0]
GO
ALTER TABLE [dbo].[STORMP]  WITH CHECK ADD  CONSTRAINT [STORMP_FSTORMS_0] FOREIGN KEY([Subject_m0])
REFERENCES [dbo].[STORMS] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMP] CHECK CONSTRAINT [STORMP_FSTORMS_0]
GO
ALTER TABLE [dbo].[STORMWEBSEARCH]  WITH CHECK ADD  CONSTRAINT [STORMWEBSEARCH_FSTORMFILTERSETTING_0] FOREIGN KEY([FilterSetting_m0])
REFERENCES [dbo].[STORMFILTERSETTING] ([PrimaryKey])
GO
ALTER TABLE [dbo].[STORMWEBSEARCH] CHECK CONSTRAINT [STORMWEBSEARCH_FSTORMFILTERSETTING_0]
GO
USE [master]
GO
ALTER DATABASE [FlexberryPractice] SET  READ_WRITE 
GO
