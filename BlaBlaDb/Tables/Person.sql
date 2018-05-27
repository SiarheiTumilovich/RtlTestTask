CREATE TABLE [dbo].[Person]
(
    [PersonId] INT NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,
    [Birthday] DATETIME2 NULL,
    CONSTRAINT PK_Person PRIMARY KEY (PersonId)
)
