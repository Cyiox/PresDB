IF OBJECT_ID(N'dbo.PropertyCommentHistory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PropertyCommentHistory (
        PropertyCommentHistoryID INT            IDENTITY (1, 1) NOT NULL CONSTRAINT PK_PropertyCommentHistory PRIMARY KEY,
        PropertyID               INT            NOT NULL,
        CommentText              NVARCHAR (MAX) NOT NULL,
        EditedAt                 DATETIME2 (7)  CONSTRAINT DF_PropertyCommentHistory_EditedAt DEFAULT SYSUTCDATETIME() NOT NULL,
        EditedBy                 NVARCHAR (255) NOT NULL
    );
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.PropertyCommentHistory')
      AND name = N'IX_PropertyCommentHistory_PropertyID_EditedAt'
)
BEGIN
    CREATE INDEX IX_PropertyCommentHistory_PropertyID_EditedAt
        ON dbo.PropertyCommentHistory(PropertyID, EditedAt DESC);
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.key_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.Properties')
      AND unique_index_id IN (
          SELECT index_id
          FROM sys.indexes
          WHERE object_id = OBJECT_ID(N'dbo.Properties')
            AND has_filter = 0
            AND is_unique = 1
      )
)
AND NOT EXISTS (
    SELECT PropertyID
    FROM dbo.Properties
    GROUP BY PropertyID
    HAVING COUNT(*) > 1
)
BEGIN
    CREATE UNIQUE INDEX UX_Properties_PropertyID ON dbo.Properties(PropertyID);
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_PropertyCommentHistory_Properties'
      AND parent_object_id = OBJECT_ID(N'dbo.PropertyCommentHistory')
)
BEGIN
    ALTER TABLE dbo.PropertyCommentHistory
        ADD CONSTRAINT FK_PropertyCommentHistory_Properties
        FOREIGN KEY (PropertyID) REFERENCES dbo.Properties(PropertyID);
END;