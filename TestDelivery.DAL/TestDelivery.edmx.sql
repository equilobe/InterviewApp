
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/31/2015 20:33:06
-- Generated from EDMX file: D:\Code\Testing\TestDelivery.DAL\TestDelivery.edmx
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

IF OBJECT_ID(N'[dbo].[FK_RespondentTest_Test]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Test] DROP CONSTRAINT [FK_RespondentTest_Test];
GO
IF OBJECT_ID(N'[dbo].[FK_Test_TestTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Test] DROP CONSTRAINT [FK_Test_TestTemplate];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Respondent]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Respondent];
GO
IF OBJECT_ID(N'[dbo].[Test]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Test];
GO
IF OBJECT_ID(N'[dbo].[TestTemplate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestTemplate];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Respondent'
CREATE TABLE [dbo].[Respondent] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Email] nvarchar(500)  NOT NULL,
    [PhoneNumber] nvarchar(50)  NULL,
    [Url] nvarchar(50)  NULL,
    [Notes] nvarchar(max)  NULL,
    [SecretCode] nvarchar(max)  NULL
);
GO

-- Creating table 'Test'
CREATE TABLE [dbo].[Test] (
    [Id] uniqueidentifier  NOT NULL,
    [RespondentId] uniqueidentifier  NOT NULL,
    [TestId] uniqueidentifier  NOT NULL,
    [SecretCode] nvarchar(50)  NOT NULL,
    [DateEmailSend] datetime  NULL,
    [DateStarted] datetime  NULL,
    [DateCompleted] datetime  NULL,
    [DateEvaluated] datetime  NULL,
    [AnswersXML] nvarchar(max)  NULL,
    [ScorePercent] int  NOT NULL
);
GO

-- Creating table 'TestTemplate'
CREATE TABLE [dbo].[TestTemplate] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [StartInstructions] nvarchar(max)  NULL,
    [QuestionsXML] nvarchar(max)  NULL,
    [TimeLimit] int  NOT NULL,
    [IsQuestionTimeLimitEnforced] bit  NOT NULL,
    [IsRandomOrder] bit  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Priority] int  NOT NULL,
    [EndInstructions] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Respondent'
ALTER TABLE [dbo].[Respondent]
ADD CONSTRAINT [PK_Respondent]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Test'
ALTER TABLE [dbo].[Test]
ADD CONSTRAINT [PK_Test]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TestTemplate'
ALTER TABLE [dbo].[TestTemplate]
ADD CONSTRAINT [PK_TestTemplate]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RespondentId] in table 'Test'
ALTER TABLE [dbo].[Test]
ADD CONSTRAINT [FK_RespondentTest_Test]
    FOREIGN KEY ([RespondentId])
    REFERENCES [dbo].[Respondent]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RespondentTest_Test'
CREATE INDEX [IX_FK_RespondentTest_Test]
ON [dbo].[Test]
    ([RespondentId]);
GO

-- Creating foreign key on [TestId] in table 'Test'
ALTER TABLE [dbo].[Test]
ADD CONSTRAINT [FK_Test_TestTemplate]
    FOREIGN KEY ([TestId])
    REFERENCES [dbo].[TestTemplate]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Test_TestTemplate'
CREATE INDEX [IX_FK_Test_TestTemplate]
ON [dbo].[Test]
    ([TestId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------