CREATE TABLE [dbo].[ShowPersonAssoc]
(
    [ShowId] INT NOT NULL,
    [PersonId] INT NOT NULL, 
    CONSTRAINT PK_ShowPersonAssoc PRIMARY KEY ([ShowId], [PersonId]),
    CONSTRAINT FK_ShowPersonAssoc_ShowId FOREIGN KEY (ShowId) REFERENCES dbo.Show(ShowId),
    CONSTRAINT FK_ShowPersonAssoc_PersonId FOREIGN KEY (PersonId) REFERENCES dbo.Person(PersonId)
)
