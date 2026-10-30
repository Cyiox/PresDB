IF OBJECT_ID(N'[dbo].[ImportReviewDecision]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ImportReviewDecision] (
        [DataSource] NVARCHAR(255) NOT NULL,
        [ImportId] INT NOT NULL,
        [Decision] NVARCHAR(20) NOT NULL,
        [DecisionDate] DATETIME2 NOT NULL,
        [DecisionBy] NVARCHAR(255) NULL,
        CONSTRAINT [PK_ImportReviewDecision] PRIMARY KEY ([DataSource], [ImportId])
    );
END
GO
