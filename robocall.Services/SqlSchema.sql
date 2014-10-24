USE [master]
GO
/****** Object:  Database [BallotRobocall]    Script Date: 10/23/2014 8:07:57 PM ******/
CREATE DATABASE [BallotRobocall]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BallotRobocall', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\BallotRobocall.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BallotRobocall_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\BallotRobocall_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BallotRobocall] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BallotRobocall].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BallotRobocall] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BallotRobocall] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BallotRobocall] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BallotRobocall] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BallotRobocall] SET ARITHABORT OFF 
GO
ALTER DATABASE [BallotRobocall] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BallotRobocall] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BallotRobocall] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BallotRobocall] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BallotRobocall] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BallotRobocall] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BallotRobocall] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BallotRobocall] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BallotRobocall] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BallotRobocall] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BallotRobocall] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BallotRobocall] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BallotRobocall] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BallotRobocall] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BallotRobocall] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BallotRobocall] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BallotRobocall] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BallotRobocall] SET RECOVERY FULL 
GO
ALTER DATABASE [BallotRobocall] SET  MULTI_USER 
GO
ALTER DATABASE [BallotRobocall] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BallotRobocall] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BallotRobocall] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BallotRobocall] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [BallotRobocall] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BallotRobocall', N'ON'
GO
USE [BallotRobocall]
GO
/****** Object:  User [ballot]    Script Date: 10/23/2014 8:07:57 PM ******/
CREATE USER [ballot] FOR LOGIN [ballot] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ballot]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ballot]
GO
/****** Object:  Table [dbo].[CallLog]    Script Date: 10/23/2014 8:07:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CallLog](
	[CallLogId] [int] IDENTITY(1,1) NOT NULL,
	[PhoneListId] [int] NOT NULL,
	[Result] [varchar](4000) NULL,
	[AddedAt] [datetimeoffset](0) NOT NULL,
 CONSTRAINT [PK_CallLog] PRIMARY KEY CLUSTERED 
(
	[CallLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Campaigns]    Script Date: 10/23/2014 8:07:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Campaigns](
	[CampaignId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[MaxCallCount] [int] NULL,
	[AudioFileUrl] [varchar](max) NULL,
	[CallerID] [varchar](20) NULL,
	[AnswerOnMedia] [bit] NULL,
	[StartDate] [datetimeoffset](0) NULL,
	[EndDate] [datetimeoffset](0) NULL,
	[DayStartTime] [datetime] NULL,
	[DayEndTime] [datetime] NULL,
	[DaysInWeekActive] [varchar](255) NULL,
	[StartTokenUrl] [varchar](255) NULL,
 CONSTRAINT [PK_Campaigns] PRIMARY KEY CLUSTERED 
(
	[CampaignId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PhoneList]    Script Date: 10/23/2014 8:07:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhoneList](
	[PhoneListId] [int] IDENTITY(1,1) NOT NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Status] [varchar](255) NULL,
	[CallCount] [int] NULL,
	[CalledAt] [datetimeoffset](0) NULL,
	[CampaignId] [int] NULL,
 CONSTRAINT [PK_PhoneList] PRIMARY KEY CLUSTERED 
(
	[PhoneListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CallLog]  WITH CHECK ADD  CONSTRAINT [FK_CallLog_PhoneList] FOREIGN KEY([PhoneListId])
REFERENCES [dbo].[PhoneList] ([PhoneListId])
GO
ALTER TABLE [dbo].[CallLog] CHECK CONSTRAINT [FK_CallLog_PhoneList]
GO
ALTER TABLE [dbo].[PhoneList]  WITH CHECK ADD  CONSTRAINT [FK_PhoneList_Campaigns] FOREIGN KEY([CampaignId])
REFERENCES [dbo].[Campaigns] ([CampaignId])
GO
ALTER TABLE [dbo].[PhoneList] CHECK CONSTRAINT [FK_PhoneList_Campaigns]
GO
USE [master]
GO
ALTER DATABASE [BallotRobocall] SET  READ_WRITE 
GO
