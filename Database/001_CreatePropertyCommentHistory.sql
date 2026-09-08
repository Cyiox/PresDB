IF OBJECT_ID(N'dbo.PropertyCommentHistory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PropertyCommentHistory
    (
        PropertyCommentHistoryID int IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_PropertyCommentHistory PRIMARY KEY,
        PropertyID int NOT NULL,
        CommentText nvarchar(max) NOT NULL,
        EditedAt datetime2(7) NOT NULL
            CONSTRAINT DF_PropertyCommentHistory_EditedAt DEFAULT SYSUTCDATETIME(),
        EditedBy nvarchar(255) NOT NULL
    );

    CREATE INDEX IX_PropertyCommentHistory_PropertyID_EditedAt
        ON dbo.PropertyCommentHistory (PropertyID, EditedAt DESC);

    ALTER TABLE dbo.PropertyCommentHistory
        ADD CONSTRAINT FK_PropertyCommentHistory_Properties
        FOREIGN KEY (PropertyID) REFERENCES dbo.Properties (PropertyID);
END;