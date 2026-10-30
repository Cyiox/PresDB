IF OBJECT_ID(N'[dbo].[PBV_Matching]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PBV_Matching] (
        [PropertyID] INT NOT NULL,
        [Project] NVARCHAR(255) NULL,
        [PBV_Address] NVARCHAR(255) NULL,
        [PBV_CityTown] NVARCHAR(255) NULL,
        [PBV_TotalUnits] INT NULL,
        [PBV_Units] INT NULL,
        [Awarded_PBV_Total] INT NULL,
        [Latest_End_Date] DATETIME NULL,
        [PBV_ContractNumber] NVARCHAR(255) NULL,
        [PBV_Status] NVARCHAR(255) NULL,
        [Match_Method] NVARCHAR(255) NULL,
        [Match_Confidence] INT NOT NULL,
        [Base_Confidence] INT NULL,
        [Flags] NVARCHAR(255) NULL,
        [Unit_Note] NVARCHAR(255) NULL,
        CONSTRAINT [PK_PBV_Matching] PRIMARY KEY ([PropertyID])
    );
END
GO
