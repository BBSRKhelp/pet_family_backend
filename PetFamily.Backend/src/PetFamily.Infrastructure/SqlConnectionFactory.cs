using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Application.Interfaces.Database;

namespace PetFamily.Infrastructure;

public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    private const string DATABASE = "Database";

    public IDbConnection GetConnection() =>
        new NpgsqlConnection(configuration.GetConnectionString(DATABASE));
}