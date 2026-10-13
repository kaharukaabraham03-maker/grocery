CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    UserRole NVARCHAR(20) NOT NULL
);

INSERT INTO Users (Username, Password, UserRole) VALUES ('buyer', '123', 'buyer');
INSERT INTO Users (Username, Password, UserRole) VALUES ('admin', '123', 'admin');

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL
);

INSERT INTO Products (ProductName, UnitPrice, StockQuantity) VALUES ('Fresh Milk (1L)', 2.50, 50);
INSERT INTO Products (ProductName, UnitPrice, StockQuantity) VALUES ('Whole Wheat Bread', 3.00, 30);
INSERT INTO Products (ProductName, UnitPrice, StockQuantity) VALUES ('Organic Eggs (Dozen)', 4.20, 20);