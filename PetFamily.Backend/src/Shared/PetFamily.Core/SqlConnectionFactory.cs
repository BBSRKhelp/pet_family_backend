using System.Data;
using Npgsql;
using PetFamily.Core.Abstractions;

namespace PetFamily.Core;

public class SqlConnectionFactory(string stringConnection) : ISqlConnectionFactory
{
    public IDbConnection GetConnection() =>
        new NpgsqlConnection(stringConnection);
}
//Установил пакет с postgres в Core для этого. +
//в Species.Application GetBreedsByIdSpeciesHandler
//у меня тоже используется Postgres для метода ILike 
//и чтобы туда не ставить поставил только здесь