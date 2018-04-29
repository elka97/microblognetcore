
if  (select name from sys.databases where name='MicroBlogPendo') is null 
	CREATE DATABASE [MicroBlogPendo];
GO

USE [MicroBlogPendo]
GO

IF OBJECT_ID (N'dbo.User') IS NULL BEGIN
	CREATE TABLE [dbo].[User](
		[UserId] [int] IDENTITY NOT NULL,	
		[Name] [nvarchar](100) NOT NULL,	
		[Email] [nvarchar](255) NOT NULL,
		[Passsword] [nvarchar](30) NOT NULL,
		CONSTRAINT [PK_User] PRIMARY KEY ([UserId]) 
	);
end

IF OBJECT_ID (N'dbo.Post') IS NULL BEGIN
	CREATE TABLE [dbo].[Post](
		[PostId] [int] NOT NULL IDENTITY,
		[UserId] [int] NOT NULL,
		[Content] nvarchar(max) not null,
		[Title] nvarchar(max) null,  
		[CreationDate] [datetime] NOT NULL,
		CONSTRAINT [PK_Post] PRIMARY KEY ([PostId]),
		CONSTRAINT [FK_Post_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([UserId]) 
	) ;
end

IF OBJECT_ID (N'dbo.PostVote') IS NULL BEGIN
	CREATE TABLE [dbo].[PostVote](
		[PostId] [int] NOT NULL ,
		[UserId] [int] NOT NULL ,
		[VoteUp] [bit]  null,
		[VoteDown] [bit]  null,
		CONSTRAINT [PK_PostVote] PRIMARY KEY ([PostId], [UserId]),
		CONSTRAINT [FK_PostVote_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([UserId]) ,
		CONSTRAINT [FK_PostVote_Post_PostId] FOREIGN KEY ([PostId]) REFERENCES [Post] ([PostId]) 
	) ;
end


IF OBJECT_ID('viewTopPosts') IS NOT NULL
	DROP VIEW viewTopPosts
GO

CREATE VIEW viewTopPosts AS
	select 
		p.PostId, p.Content, p.CreationDate, v.VoteUpNumber  
	from 
		[dbo].[Post] p join 
		(select count(v.[VoteUp]) as VoteUpNumber, v.[PostId] 
		from [dbo].[PostVote] v 
		where v.[VoteUp] is not null
		group by v.[PostId] ) as v on v.PostID = p.PostId;	
GO

--alter table [dbo].[PostVote] add constraint [FK_PostVote_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([UserId]) 