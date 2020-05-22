CREATE TABLE [dbo].[Events]
(
	[EventKey] nvarchar(300) NOT NULL PRIMARY KEY,
	[CreationDate] datetime NOT NULL
)

CREATE TABLE [dbo].[EventPropertiesMetaValue]
(
	[PropertyId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PropertyName] nvarchar(300) NOT NULL,
	[EventKey] nvarchar(300) NOT NULL,
	[ValueType] nvarchar(300) NOT NULL,
	[SampleValue] nvarchar(2000)
)