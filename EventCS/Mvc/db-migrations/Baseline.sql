CREATE TABLE [dbo].[Events]
(
	[EventKey] nvarchar(300) NOT NULL PRIMARY KEY,
	[CreationDate] datetime NOT NULL
)

CREATE TABLE [dbo].[EventPropertiesMetaValue]
(
	[PropertyName] nvarchar(300) NOT NULL PRIMARY KEY,
	[EventKey] nvarchar(300) NOT NULL,
	[ValueType] nvarchar(300) NOT NULL,
	[SampleValue] nvarchar(300)
)