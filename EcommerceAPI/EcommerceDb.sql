
-- USERS
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO
-- ROLES
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY,
    Name VARCHAR(50) NOT NULL UNIQUE
);
GO
-- USER ROLES (N:M)
CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,

    CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),

    CONSTRAINT FK_UserRoles_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id),

    CONSTRAINT FK_UserRoles_Roles 
        FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
GO
-- CATEGORIES
CREATE TABLE Categories(
    Id INT PRIMARY KEY IDENTITY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1
);
GO
-- PRODUCTS
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    Name VARCHAR(150) NOT NULL,
    Description VARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    Stock INT NOT NULL CHECK (Stock >= 0),
    CategoryId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Products_Categories 
        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO
-- PRODUCT IMAGES
CREATE TABLE ProductImages (
    Id INT PRIMARY KEY IDENTITY,
    ProductId INT NOT NULL,
    ImageUrl VARCHAR(500) NOT NULL,
    IsMain BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_ProductImages_Products 
        FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO
-- CARTS
CREATE TABLE Carts (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT UQ_Carts_User UNIQUE (UserId),

    CONSTRAINT FK_Carts_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO
-- CART ITEMS
CREATE TABLE CartItems (
    Id INT PRIMARY KEY IDENTITY,
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),

    CONSTRAINT FK_CartItems_Carts 
        FOREIGN KEY (CartId) REFERENCES Carts(Id),

    CONSTRAINT FK_CartItems_Products 
        FOREIGN KEY (ProductId) REFERENCES Products(Id),

    CONSTRAINT UQ_Cart_Product UNIQUE (CartId, ProductId)
);
GO
-- ORDERS
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status VARCHAR(50) NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL CHECK (TotalAmount >= 0),

    CONSTRAINT FK_Orders_Users 
        FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO
-- ORDER ITEMS
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice >= 0),

    CONSTRAINT FK_OrderItems_Orders 
        FOREIGN KEY (OrderId) REFERENCES Orders(Id),

    CONSTRAINT FK_OrderItems_Products 
        FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO
-- PAYMENTS
CREATE TABLE Payments (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL CHECK (Amount >= 0),
    Status VARCHAR(50) NOT NULL,
    TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Payments_Orders 
        FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);
GO