CREATE TABLE [dbo].[DepartmentEmployee]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [FirstName] NCHAR(256) NOT NULL, 
    [LastName] NCHAR(256) NULL, 
    [JobTitle] NCHAR(256) NULL, 
    [HireDate] DATETIME NULL
)
