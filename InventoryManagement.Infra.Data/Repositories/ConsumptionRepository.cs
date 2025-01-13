using Dapper;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace InventoryManagement.Infra.Data.Repositories;

public class ConsumptionRepository : IConsumptionRepository
{
    private readonly ILogger<ConsumptionRepository> _logger;
    private readonly IDbConnection _dbConnection;

    public ConsumptionRepository(IDbConnection dbConnection, ILogger<ConsumptionRepository> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<(Guid Id, string Product, decimal AveragePrice, int Quantity)>> GetByDateAsync(DateTime date)
    {
        _logger.LogInformation("Buscando os consumos pela data {0}.", date);
        var query = @"
SELECT 
      T.Id
    , T.Product
    , AVG(T.Price) AS AveragePrice
    , COUNT(T.Product) AS Quantity
FROM (
    SELECT 
          I.Id
        , I.Name AS Product
        , P.Price
        , P.Id AS ProductId
    FROM 
        Consumption C
        INNER JOIN Item I ON C.ItemId = I.Id
        INNER JOIN Product P ON P.ItemId = I.Id
    WHERE 
        DATE(C.CreatedAt) = @Date
        AND C.Deleted = 0
        AND P.Deleted = 0
        AND I.Deleted = 0
        AND P.Available = 0
    GROUP BY
        I.Id, I.Name, P.Id
) AS T
GROUP BY
    T.Id,
    T.Product;
";

        return await _dbConnection.QueryAsync<(Guid Id, string Product, decimal AveragePrice, int Quantity)>(query, new { Date = date });
    }

    public async Task<Consumption> RegisterProductsIdsAsync(Consumption consumption, List<Guid> productsIds)
    {
        if (_dbConnection.State == ConnectionState.Closed)
            _dbConnection.Open();        

        using var transaction = _dbConnection.BeginTransaction();

        try
        {
            _logger.LogInformation("Adicionando o consumo (Id='{0}' e quantidade='{1}').", consumption.Id, consumption.Quantity);
            var query = $"INSERT INTO Consumption ({GetColumns()}) VALUES ({GetParameters()})";
            await _dbConnection.ExecuteAsync(query, consumption);

            _logger.LogInformation("Consumindo os produtos ('{0}') onde (Id='{1}')", string.Join(", ", productsIds), consumption.Id);
            query = $"UPDATE Product SET Available = 0, ConsumptionId = @ConsumptionId WHERE Id IN @Ids";
            await _dbConnection.ExecuteAsync(query, new { Ids = productsIds, ConsumptionId = consumption.Id });
            transaction.Commit();
        }
        catch (Exception ex)
        {
            var errorMessage = $"Erro ao inserir o consumo (Id='{consumption.Id}' e Quantidade='{consumption.Quantity}').";
            _logger.LogError("StatusCode='{0}'. MessageError='{1}'. Exception='{2}'", 500, errorMessage, ex.Message);
            transaction.Rollback();
        }

        return consumption;
    }

    private string GetColumns()
        => string.Join(", ", typeof(Consumption).GetProperties().Where(p => p.Name != nameof(Consumption.Item) && p.Name != nameof(Consumption.Product) && p.Name != nameof(Consumption.ProductId)).Select(p => p.Name));
    
    private string GetParameters()
        => string.Join(", ", typeof(Consumption).GetProperties().Where(p => p.Name != nameof(Consumption.Item) && p.Name != nameof(Consumption.Product) && p.Name != nameof(Consumption.ProductId)).Select(p => $"@{p.Name}"));

    private string GetSetColumns()
        => string.Join(", ", typeof(Consumption).GetProperties().Where(p => p.Name != nameof(Consumption.Item) && p.Name != nameof(Consumption.Product) && p.Name != nameof(Consumption.ProductId)).Select(p => $"{p.Name} = @{p.Name}"));    
}