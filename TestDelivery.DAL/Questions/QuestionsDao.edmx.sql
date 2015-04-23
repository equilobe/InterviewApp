
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/01/2015 10:22:18
-- Generated from EDMX file: D:\Code\Testing\TestDelivery.DAL\Questions\QuestionsDao.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TestDelivery];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Question]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Question];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Questions'
CREATE TABLE [dbo].[Questions] (
    [Id] uniqueidentifier  NOT NULL,
    [Summary] nvarchar(max)  NOT NULL,
    [Tags] nvarchar(max)  NULL,
    [TimeLimit] int  NOT NULL,
    [Score] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [ExtendedPropertiesXml] nvarchar(max)  NULL
);
GO

-- Creating table 'Questions_Problem'
CREATE TABLE [dbo].[Questions_Problem] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Questions_FreeTextQuestion'
CREATE TABLE [dbo].[Questions_FreeTextQuestion] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Questions_MultipleChoiceQuestion'
CREATE TABLE [dbo].[Questions_MultipleChoiceQuestion] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [PK_Questions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Questions_Problem'
ALTER TABLE [dbo].[Questions_Problem]
ADD CONSTRAINT [PK_Questions_Problem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Questions_FreeTextQuestion'
ALTER TABLE [dbo].[Questions_FreeTextQuestion]
ADD CONSTRAINT [PK_Questions_FreeTextQuestion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Questions_MultipleChoiceQuestion'
ALTER TABLE [dbo].[Questions_MultipleChoiceQuestion]
ADD CONSTRAINT [PK_Questions_MultipleChoiceQuestion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id] in table 'Questions_Problem'
ALTER TABLE [dbo].[Questions_Problem]
ADD CONSTRAINT [FK_Problem_inherits_Question]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Questions_FreeTextQuestion'
ALTER TABLE [dbo].[Questions_FreeTextQuestion]
ADD CONSTRAINT [FK_FreeTextQuestion_inherits_Question]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Questions_MultipleChoiceQuestion'
ALTER TABLE [dbo].[Questions_MultipleChoiceQuestion]
ADD CONSTRAINT [FK_MultipleChoiceQuestion_inherits_Question]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------