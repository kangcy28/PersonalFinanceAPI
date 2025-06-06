-- 建立資料庫
CREATE DATABASE PersonalFinanceDB;
GO

USE PersonalFinanceDB;
GO

-- 1. Users 表
CREATE TABLE Users (
    Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Email nvarchar(100) NOT NULL UNIQUE,
    PasswordHash nvarchar(255) NOT NULL,
    Name nvarchar(50) NOT NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive bit NOT NULL DEFAULT 1
);

-- 2. Categories 表
CREATE TABLE Categories (
    Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name nvarchar(50) NOT NULL,
    Description nvarchar(200) NULL,
    Type nvarchar(20) NOT NULL CHECK (Type IN ('Income', 'Expense')),
    Color nvarchar(7) NULL DEFAULT '#6B7280',
    IsActive bit NOT NULL DEFAULT 1,
    UserId int NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
);

-- 3. Transactions 表
CREATE TABLE Transactions (
    Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Amount decimal(18,2) NOT NULL CHECK (Amount > 0),
    Description nvarchar(200) NULL,
    TransactionDate datetime2 NOT NULL,
    Type nvarchar(20) NOT NULL CHECK (Type IN ('Income', 'Expense')),
    CategoryId int NOT NULL FOREIGN KEY REFERENCES Categories(Id),
    UserId int NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    IsDeleted bit NOT NULL DEFAULT 0
);

-- 建立索引
CREATE INDEX IX_Categories_UserId_Type ON Categories(UserId, Type);
CREATE INDEX IX_Categories_IsActive ON Categories(IsActive);
CREATE INDEX IX_Transactions_UserId_Date ON Transactions(UserId, TransactionDate DESC);
CREATE INDEX IX_Transactions_UserId_Type ON Transactions(UserId, Type);
CREATE INDEX IX_Transactions_CategoryId ON Transactions(CategoryId);
CREATE INDEX IX_Transactions_IsDeleted ON Transactions(IsDeleted);

-- 插入系統用戶
INSERT INTO Users (Email, PasswordHash, Name, CreatedAt, UpdatedAt, IsActive) 
VALUES ('system@system.com', 'SYSTEM_HASH', 'System', GETUTCDATE(), GETUTCDATE(), 0);

-- 取得系統用戶的 ID 並插入分類
DECLARE @SystemUserId int = (SELECT Id FROM Users WHERE Email = 'system@system.com');

INSERT INTO Categories (Name, Type, Color, UserId, CreatedAt, UpdatedAt) VALUES
('薪水', 'Income', '#10B981', @SystemUserId, GETUTCDATE(), GETUTCDATE()),
('獎金', 'Income', '#F59E0B', @SystemUserId, GETUTCDATE(), GETUTCDATE()),
('投資收益', 'Income', '#3B82F6', @SystemUserId, GETUTCDATE(), GETUTCDATE()),
('餐飲', 'Expense', '#EF4444', @SystemUserId, GETUTCDATE(), GETUTCDATE()),
('交通', 'Expense', '#F97316', @SystemUserId, GETUTCDATE(), GETUTCDATE()),
('購物', 'Expense', '#EAB308', @SystemUserId, GETUTCDATE(), GETUTCDATE()),
('娛樂', 'Expense', '#22C55E', @SystemUserId, GETUTCDATE(), GETUTCDATE()),
('醫療', 'Expense', '#06B6D4', @SystemUserId, GETUTCDATE(), GETUTCDATE());