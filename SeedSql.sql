-- Criação do banco de dados
create database InventoryDb;
 
-- Uso do banco de dados criado
use InventoryDb;

-- Criação da tabela 'Item'
CREATE TABLE Item (
    Id CHAR(36) PRIMARY KEY,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NULL,
    Name VARCHAR(255) NOT NULL,
    Description VARCHAR(500),
    Deleted TINYINT(1)
);

-- Criação da tabela 'Consumption'
CREATE TABLE Consumption (
    Id CHAR(36) PRIMARY KEY,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NULL,
    ItemId CHAR(36) NOT NULL,
    Quantity INT NOT NULL,
    Deleted TINYINT(1),
    FOREIGN KEY (ItemId) REFERENCES Item(Id)
);

-- Criação da tabela 'Product'
CREATE TABLE Product (
    Id CHAR(36) PRIMARY KEY,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NULL,
    ItemId CHAR(36) NOT NULL,
    ConsumptionId CHAR(36) NULL,
    PartNumber VARCHAR(255) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Available TINYINT(1) NOT NULL,	
    Deleted TINYINT(1) NOT NULL,
    FOREIGN KEY (ItemId) REFERENCES Item(Id),
    FOREIGN KEY (ConsumptionId) REFERENCES Consumption(Id)
);