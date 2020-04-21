CREATE TABLE [dbo].[Events]
(
	[EventKey] nvarchar(50) NOT NULL PRIMARY KEY,
	[CreationDate] datetime NOT NULL
)

CREATE TABLE [dbo].[EventPropertiesMetaValue]
(
	[PropertyName] nvarchar(50) NOT NULL PRIMARY KEY,
	[EventKey] nvarchar(50) NOT NULL,
	[ValueType] nvarchar(10) NOT NULL,
	[SampleValue] nvarchar(300)
)

INSERT INTO [Events]([EventKey], [CreationDate])
VALUES
	('FirstEvent', '12.12.12'),
	('SecondEvent', '12.12.12')

INSERT INTO [EventPropertiesMetaValue]([PropertyName], [EventKey], [ValueType], [SampleValue])
VALUES
	('FirstProp', 'FirstEvent', 'String', 'This is sample value'),
	('SecondProp', 'SecondEvent', 'String', 'This is sample value')