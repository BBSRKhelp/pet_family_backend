using System.Data;

namespace PetFamily.Core.Interfaces.Database;

public interface ISqlConnectionFactory
{
    IDbConnection GetConnection();
}