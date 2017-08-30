-- <Migration ID="41e3294c-0449-454d-9db8-d746d3d11d45" />
GO

PRINT N'Creating [dbo].[Order]'
GO
CREATE TABLE [dbo].[Order]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[Amount] [int] NOT NULL,
[Customer] [nvarchar] (max) NULL,
[Product] [nvarchar] (max) NULL,
[TotalAmount] [decimal] (18, 2) NOT NULL,
[UnitPrice] [decimal] (18, 2) NOT NULL
)
GO
PRINT N'Creating primary key [PK_Order] on [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED  ([ID])
GO
