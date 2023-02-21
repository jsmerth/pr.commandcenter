﻿CREATE TABLE [dbo].[Bots]
(
  [Id] UNIQUEIDENTIFIER NOT NULL, 
  [TypeId] INT NOT NULL DEFAULT 0,
  [Business] NVARCHAR (50) NOT NULL,
  [City] NVARCHAR (50) NOT NULL,
  [Region] NVARCHAR (50) NOT NULL,
  [Country] NVARCHAR (50) NOT NULL,
  [Lat] DECIMAL(10,8) NOT NULL,
  [Lon] DECIMAL(11,8) NOT NULL,
  [DateModified] DATETIME2 NOT NULL DEFAULT CURRENT_TIMESTAMP,
  [DateCreated] DATETIME2 NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT [PK_bots_bot_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TRIGGER [dbo].[Trigger_Bots_Modified_Date]
  ON [dbo].[Bots]
  FOR INSERT, UPDATE
  AS
  BEGIN
    UPDATE [dbo].[Bots]
    SET DateModified = CURRENT_TIMESTAMP
  END
ALTER TABLE [dbo].[Bots] ENABLE TRIGGER [Trigger_Bots_Modified_Date]
GO

