
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/06/2018 14:50:55
-- Generated from EDMX file: C:\Users\jamii\source\repos\WebApplication2\WebApplication2\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [sjassoc_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[EmailLists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailLists];
GO
IF OBJECT_ID(N'[dbo].[Todoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Todoes];
GO
IF OBJECT_ID(N'[dbo].[UserAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Todoes'
CREATE TABLE [dbo].[Todoes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [Task] nvarchar(max)  NULL,
    [Status] nvarchar(50)  NULL,
    [FollowUp] datetime  NULL,
    [Group] nvarchar(50)  NULL
);
GO

-- Creating table 'UserAccounts'
CREATE TABLE [dbo].[UserAccounts] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [ConfirmPassword] nvarchar(max)  NULL
);
GO

-- Creating table 'EmailLists'
CREATE TABLE [dbo].[EmailLists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Group] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Todoes'
ALTER TABLE [dbo].[Todoes]
ADD CONSTRAINT [PK_Todoes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [UserID] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [PK_UserAccounts]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [Id] in table 'EmailLists'
ALTER TABLE [dbo].[EmailLists]
ADD CONSTRAINT [PK_EmailLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------