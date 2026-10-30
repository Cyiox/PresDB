IF COL_LENGTH(N'dbo.PBV_Matching', N'Awarded_PBV_Total') IS NULL
    ALTER TABLE [dbo].[PBV_Matching] ADD [Awarded_PBV_Total] INT NULL;
GO

IF COL_LENGTH(N'dbo.PBV_Matching', N'Latest_End_Date') IS NULL
    ALTER TABLE [dbo].[PBV_Matching] ADD [Latest_End_Date] DATETIME NULL;
GO
