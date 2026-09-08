IF OBJECT_ID(N'dbo.PropertyCommentHistory', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.PropertyCommentHistory (
            PropertyCommentHistoryID INT            IDENTITY (1, 1) NOT NULL CONSTRAINT PK_PropertyCommentHistory PRIMARY KEY,
            PropertyID               INT            NOT NULL,
            CommentText              NVARCHAR (MAX) NOT NULL,
            EditedAt                 DATETIME2 (7)  CONSTRAINT DF_PropertyCommentHistory_EditedAt DEFAULT SYSUTCDATETIME() NOT NULL,
            EditedBy                 NVARCHAR (255) NOT NULL
        );
        CREATE INDEX IX_PropertyCommentHistory_PropertyID_EditedAt
            ON dbo.PropertyCommentHistory(PropertyID, EditedAt DESC);
        ALTER TABLE dbo.PropertyCommentHistory
            ADD CONSTRAINT FK_PropertyCommentHistory_Properties FOREIGN KEY (PropertyID) REFERENCES dbo.Properties (PropertyID);
    END