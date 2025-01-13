using Dapper;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace InventoryManagement.Infra.Data.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly ILogger<ItemRepository> _logger;
    private readonly IDbConnection _dbConnection;

    public ItemRepository(IDbConnection dbConnection, ILogger<ItemRepository> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Item> AddAsync(Item item)
    {
        _logger.LogInformation("Adicionando o item (Name='{0}').", item.Name);
        var query = $"INSERT INTO Item ({GetColumns()}) VALUES ({GetParameters()})";
        await _dbConnection.ExecuteAsync(query, item);
        return item;
    }

    public async Task<Item> DeleteAsync(Item item)
    {
        _logger.LogInformation("Excluindo o item (Id='{0}').", item.Id);
        var query = $"UPDATE Item SET {GetSetColumns()} WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(query, item);
        return item;
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
    {
        _logger.LogInformation("Buscando todos os items.");
        var query = $"SELECT * FROM Item WHERE Deleted = 0";
        return await _dbConnection.QueryAsync<Item>(query);
    }

    public async Task<Item?> GetByIdAndNameAsync(Guid id, string name)
    {
        _logger.LogInformation("Buscando o item por (Id='{0}' e Name=({1}).", id, name);
        var query = $"SELECT * FROM Item WHERE Id = @Id AND Name <> @Name AND Deleted = 0";
        return await _dbConnection.QuerySingleOrDefaultAsync<Item>(query, new { Id = id, Name = name });
    }

    public async Task<Item?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando o item por (Id='{0}').", id);
        var query = $"SELECT * FROM Item WHERE Id = @Id AND Deleted = 0";
        return await _dbConnection.QuerySingleOrDefaultAsync<Item>(query, new { Id = id });
    }

    public async Task<Item?> GetByNameAsync(string name)
    {
        _logger.LogInformation("Buscando o item por (Nome='{0}').", name);
        var query = $"SELECT * FROM Item WHERE Name = @name AND Deleted = 0";
        return await _dbConnection.QuerySingleOrDefaultAsync<Item>(query, new { Name = name });
    }

    public async Task<Item> UpdateAsync(Item item)
    {
        _logger.LogInformation("Atualizando o item de (Id='{0}').", item.Id);
        var query = $"UPDATE Item SET {GetSetColumns()} WHERE Id = @Id AND Deleted = 0";
        await _dbConnection.ExecuteAsync(query, item);
        return item;
    }

    private string GetColumns()
        => string.Join(", ", typeof(Item).GetProperties().Select(p => p.Name));    

    private string GetParameters()
        => string.Join(", ", typeof(Item).GetProperties().Select(p => $"@{p.Name}"));

    private string GetSetColumns()
        => string.Join(", ", typeof(Item).GetProperties().Select(p => $"{p.Name} = @{p.Name}"));
}