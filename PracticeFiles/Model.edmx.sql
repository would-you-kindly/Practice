
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/24/2017 15:45:15
-- Generated from EDMX file: D:\YandexDisk\Third course\Производственная практика\Practice\PracticeFiles\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Practice];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ContentTypeFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_ContentTypeFile];
GO
IF OBJECT_ID(N'[dbo].[FK_FileFormatFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_FileFormatFile];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatorFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_CreatorFile];
GO
IF OBJECT_ID(N'[dbo].[FK_FilePurchase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Purchases] DROP CONSTRAINT [FK_FilePurchase];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPurchase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Purchases] DROP CONSTRAINT [FK_UserPurchase];
GO
IF OBJECT_ID(N'[dbo].[FK_FileFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Files] DROP CONSTRAINT [FK_FileFile];
GO
IF OBJECT_ID(N'[dbo].[FK_OwnerFile_Owner]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OwnerFile] DROP CONSTRAINT [FK_OwnerFile_Owner];
GO
IF OBJECT_ID(N'[dbo].[FK_OwnerFile_File]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OwnerFile] DROP CONSTRAINT [FK_OwnerFile_File];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Files]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Files];
GO
IF OBJECT_ID(N'[dbo].[ContentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContentTypes];
GO
IF OBJECT_ID(N'[dbo].[FileFormats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FileFormats];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Purchases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Purchases];
GO
IF OBJECT_ID(N'[dbo].[OwnerFile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OwnerFile];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [Id] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Url] nvarchar(max)  NOT NULL,
    [Price] float  NOT NULL,
    [ContentType_Id] int  NOT NULL,
    [FileFormat_Id] int  NOT NULL,
    [Creator_Id] int  NOT NULL,
    [PdfFile_Id] int  NULL
);
GO

-- Creating table 'ContentTypes'
CREATE TABLE [dbo].[ContentTypes] (
    [Id] int  NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FileFormats'
CREATE TABLE [dbo].[FileFormats] (
    [Id] int  NOT NULL,
    [Format] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Purchases'
CREATE TABLE [dbo].[Purchases] (
    [Id] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [File_Id] int  NOT NULL,
    [Purchaser_Id] int  NOT NULL
);
GO

-- Creating table 'OwnerFile'
CREATE TABLE [dbo].[OwnerFile] (
    [Owners_Id] int  NOT NULL,
    [OwnedFiles_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [PK_Files]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContentTypes'
ALTER TABLE [dbo].[ContentTypes]
ADD CONSTRAINT [PK_ContentTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FileFormats'
ALTER TABLE [dbo].[FileFormats]
ADD CONSTRAINT [PK_FileFormats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Purchases'
ALTER TABLE [dbo].[Purchases]
ADD CONSTRAINT [PK_Purchases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Owners_Id], [OwnedFiles_Id] in table 'OwnerFile'
ALTER TABLE [dbo].[OwnerFile]
ADD CONSTRAINT [PK_OwnerFile]
    PRIMARY KEY CLUSTERED ([Owners_Id], [OwnedFiles_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ContentType_Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_ContentTypeFile]
    FOREIGN KEY ([ContentType_Id])
    REFERENCES [dbo].[ContentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContentTypeFile'
CREATE INDEX [IX_FK_ContentTypeFile]
ON [dbo].[Files]
    ([ContentType_Id]);
GO

-- Creating foreign key on [FileFormat_Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_FileFormatFile]
    FOREIGN KEY ([FileFormat_Id])
    REFERENCES [dbo].[FileFormats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileFormatFile'
CREATE INDEX [IX_FK_FileFormatFile]
ON [dbo].[Files]
    ([FileFormat_Id]);
GO

-- Creating foreign key on [Creator_Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_CreatorFile]
    FOREIGN KEY ([Creator_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatorFile'
CREATE INDEX [IX_FK_CreatorFile]
ON [dbo].[Files]
    ([Creator_Id]);
GO

-- Creating foreign key on [File_Id] in table 'Purchases'
ALTER TABLE [dbo].[Purchases]
ADD CONSTRAINT [FK_FilePurchase]
    FOREIGN KEY ([File_Id])
    REFERENCES [dbo].[Files]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FilePurchase'
CREATE INDEX [IX_FK_FilePurchase]
ON [dbo].[Purchases]
    ([File_Id]);
GO

-- Creating foreign key on [Purchaser_Id] in table 'Purchases'
ALTER TABLE [dbo].[Purchases]
ADD CONSTRAINT [FK_UserPurchase]
    FOREIGN KEY ([Purchaser_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPurchase'
CREATE INDEX [IX_FK_UserPurchase]
ON [dbo].[Purchases]
    ([Purchaser_Id]);
GO

-- Creating foreign key on [PdfFile_Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [FK_FileFile]
    FOREIGN KEY ([PdfFile_Id])
    REFERENCES [dbo].[Files]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FileFile'
CREATE INDEX [IX_FK_FileFile]
ON [dbo].[Files]
    ([PdfFile_Id]);
GO

-- Creating foreign key on [Owners_Id] in table 'OwnerFile'
ALTER TABLE [dbo].[OwnerFile]
ADD CONSTRAINT [FK_OwnerFile_Owner]
    FOREIGN KEY ([Owners_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [OwnedFiles_Id] in table 'OwnerFile'
ALTER TABLE [dbo].[OwnerFile]
ADD CONSTRAINT [FK_OwnerFile_File]
    FOREIGN KEY ([OwnedFiles_Id])
    REFERENCES [dbo].[Files]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OwnerFile_File'
CREATE INDEX [IX_FK_OwnerFile_File]
ON [dbo].[OwnerFile]
    ([OwnedFiles_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------