using System.Data;
using Npgsql;
using PetFamily.Core.Abstractions;

namespace PetFamily.Core;

public class SqlConnectionFactory(string stringConnection) : ISqlConnectionFactory
{
    public IDbConnection GetConnection() =>
        new NpgsqlConnection(stringConnection);
}