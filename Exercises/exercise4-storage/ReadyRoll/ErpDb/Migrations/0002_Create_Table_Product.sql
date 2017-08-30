-- <Migration ID="b588b798-ba16-4c3b-b2d6-250b4c2503cd" />
GO

PRINT N'Creating [dbo].[Product]'
GO
CREATE TABLE [dbo].[Product]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[ProductCode] [nvarchar] (20) NOT NULL,
[ProductDescription] [nvarchar] (max) NULL,
[UnitPrice] [decimal] (18, 2) NULL
)
GO
