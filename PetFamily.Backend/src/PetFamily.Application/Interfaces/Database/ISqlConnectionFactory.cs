using System.Data;

namespace PetFamily.Application.Interfaces.Database;

public interface ISqlConnectionFactory
{
    IDbConnection GetConnection();
}