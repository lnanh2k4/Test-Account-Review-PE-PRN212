USE [master]
GO
/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Test_Account')
BEGIN
	ALTER DATABASE Test_Account SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE Test_Account SET ONLINE;
	DROP DATABASE Test_Account;
END

GO
CREATE DATABASE Test_Account
GO
USE Test_Account
GO
/*******************************************************************************
	Drop tables if exists
*******************************************************************************/
DECLARE @sql nvarchar(MAX) 
SET @sql = N'' 

SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(KCU1.TABLE_SCHEMA) 
    + N'.' + QUOTENAME(KCU1.TABLE_NAME) 
    + N' DROP CONSTRAINT ' -- + QUOTENAME(rc.CONSTRAINT_SCHEMA)  + N'.'  -- not in MS-SQL
    + QUOTENAME(rc.CONSTRAINT_NAME) + N'; ' + CHAR(13) + CHAR(10) 
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 

INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
    ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
    AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
    AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME 

EXECUTE(@sql) 

GO
DECLARE @sql2 NVARCHAR(max)=''

SELECT @sql2 += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.'+ QUOTENAME(TABLE_NAME) + '; '
FROM   INFORMATION_SCHEMA.TABLES
WHERE  TABLE_TYPE = 'BASE TABLE'

Exec Sp_executesql @sql2 
GO 

CREATE TABLE RoleAccount (
roleId int primary key,
roleName nvarchar(255)
) 
GO

CREATE TABLE Account(
accId int identity(1,1) primary key,
fullName nvarchar(255),
password varchar(255),
dob Date,
email varchar(255),
[role] int,
[address] text,
sex int,
accStatus int,
CONSTRAINT FK_Account_Role FOREIGN KEY ([role]) REFERENCES RoleAccount(roleId)
)
GO

INSERT INTO RoleAccount(roleId, roleName) VALUES (0,'Admin') 
INSERT INTO RoleAccount(roleId, roleName) VALUES (1,'Customer') 
INSERT INTO RoleAccount(roleId, roleName) VALUES (2,'Seller') 
INSERT INTO RoleAccount(roleId, roleName) VALUES (3,'Warehouse') 

INSERT INTO Account(fullName, [password], dob, email, [role], [address], sex, accStatus) VALUES ('Le Nhut Anh', '123','2004-01-01','lnanh2k4@gmail.com',0,'Can Tho', 1, 0) 
INSERT INTO Account(fullName, [password], dob, email, [role], [address], sex, accStatus) VALUES ('Tran Minh Quan', '123','2004-01-01','lnanh2k4@gmail.com',1,'Can Tho', 0, 1) 
INSERT INTO Account(fullName, [password], dob, email, [role], [address], sex, accStatus) VALUES ('Dang Cong Khanh', '123','2004-01-01','lnanh2k4@gmail.com',2,'Can Tho', 1, 0) 
INSERT INTO Account(fullName, [password], dob, email, [role], [address], sex, accStatus) VALUES ('Tran Le Gia Huy', '123','2004-01-01','lnanh2k4@gmail.com',3,'Can Tho', 0, 1) 

-- accStatus => 0: Inactive, 1: Active
-- Sex => 0: Female, 1: Male
