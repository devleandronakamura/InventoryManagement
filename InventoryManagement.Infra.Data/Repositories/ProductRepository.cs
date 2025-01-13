using Dapper;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace InventoryManagement.Infra.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly IDbConnection _dbConnection;

    public ProductRepository(IDbConnection dbConnection, ILogger<ProductRepository> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Product> AddAsync(Product product)
    {
        _logger.LogInformation("Adicionando o item (PartNumber='{0}').", product.PartNumber);
        var query = $"INSERT INTO Product ({GetColumns()}) VALUES ({GetParameters()})";
        await _dbConnection.ExecuteAsync(query, product);
        return product;
    }

    public async Task<Product> DeleteAsync(Product product)
    {
        _logger.LogInformation("Excluindo o item (Id='{0}').", product.Id);
        var query = $"UPDATE Product SET {GetSetColumns()} WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, product);
        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        _logger.LogInformation("Buscando todos os itens.");
        var query = @"
SELECT 
      P.Id
    , P.CreatedAt
    , P.UpdatedAt
    , P.ItemId AS ItemId
    , P.PartNumber
    , P.Price
    , P.Deleted
    , 'Split'
    , I.Id
    , I.Name
    , I.Description
FROM 
    Product P 
    INNER JOIN Item I ON I.Id = P.ItemId
WHERE 
    P.Deleted = 0
    AND I.Deleted = 0
";

        var products = await _dbConnection.QueryAsync<Product, Item, Product>(
            query,
            (product, item) =>
            {
                product.Item = item;
                return product;
            },
            splitOn: "Split"
        );

        return products;
    }

    public async Task<Product?> GetByIdAndPartNumberAsync(Guid id, string partNumber)
    {
        _logger.LogInformation("Buscando o produto por (Id='{0}' e PartNumber=({1}).", id, partNumber);
        var query = $"SELECT * FROM Product WHERE Id = @Id AND PartNumber <> @PartNumber AND Deleted = 0";
        return await _dbConnection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id, PartNumber = partNumber });
    }

    public async Task<IEnumerable<Product>> GetByIdAndQuantityAsync(Guid id, int quantity)
    {
        _logger.LogInformation("Buscando os produtos Limite {0} onde o produto (Id='{1}').", quantity, id);
        var query = @$"
SELECT
      P.Id
    , P.CreatedAt
    , P.UpdatedAt
    , P.ItemId AS ItemId
    , P.PartNumber
    , P.Price
    , P.Deleted
    , 'Split'
    , I.Id
    , I.Name
    , I.Description
FROM 
    Product P 
    INNER JOIN Item I ON I.Id = P.ItemId
WHERE 
    P.Deleted = 0
    AND I.Deleted = 0
    AND I.Id = @Id
    AND P.Available = 1
ORDER BY
    P.CreatedAt ASC
LIMIT 
    {quantity}
";

        var products = await _dbConnection.QueryAsync<Product, Item, Product>(
            query,
            (product, item) =>
            {
                product.Item = item;
                return product;
            },
            new { Id = id },
            splitOn: "Split"
        );

        return products;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando o produto por (Id='{0}').", id);
        var query = @"
SELECT 
      P.Id
    , P.CreatedAt
    , P.UpdatedAt
    , P.ItemId AS ItemId
    , P.PartNumber
    , P.Price
    , P.Deleted
    , 'Split'
    , I.Id
    , I.Name
    , I.Description
FROM 
    Product P 
    INNER JOIN Item I ON I.Id = P.ItemId
WHERE 
    P.Id = @Id
    AND P.Deleted = 0
    AND I.Deleted = 0
";

        var products = await _dbConnection.QueryAsync<Product, Item, Product>(
            query,
            (product, item) =>
            {
                product.Item = item;
                return product;
            }, 
            new { Id = id },
            splitOn: "Split"
        );

        return products.FirstOrDefault();
    }

    public async Task<Product?> GetByPartNumberAsync(string partNumber)
    {
        _logger.LogInformation("Buscando o produto por (PartNumber='{0}').", partNumber);
        var query = $"SELECT * FROM Product WHERE PartNumber = @PartNumber AND Deleted = 0 AND Available = 1";
        return await _dbConnection.QuerySingleOrDefaultAsync<Product>(query, new { PartNumber = partNumber });
    }

    public async Task RegisterByConsumptionIdAndProductsIdsAsync(Guid consumptionId, List<Guid> productsIds)
    {
        _logger.LogInformation("Consumindo os produtos ('{0}') onde (Id='{1}')", string.Join(", ", productsIds), consumptionId);
        var query = $"UPDATE Product SET Available = 0, ConsumptionId = @ConsumptionId WHERE Id IN @Ids";
        await _dbConnection.ExecuteAsync(query, new { Ids = productsIds, ConsumptionId = consumptionId });
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _logger.LogInformation("Atualizando o produto de (Id='{0}').", product.Id);
        var query = $"UPDATE Product SET {GetSetColumns()} WHERE Id = @Id AND Deleted = 0 AND Available = 1";
        await _dbConnection.ExecuteAsync(query, product);
        return product;
    }

    private string GetColumns()
        => string.Join(", ", typeof(Product).GetProperties().Where(p => p.Name != nameof(Item)).Select(p => p.Name));

    private string GetParameters()
        => string.Join(", ", typeof(Product).GetProperties().Where(p => p.Name != nameof(Item)).Select(p => $"@{p.Name}"));

    private string GetSetColumns()
        => string.Join(", ", typeof(Product).GetProperties().Where(p => p.Name != nameof(Item)).Select(p => $"{p.Name} = @{p.Name}"));       
}