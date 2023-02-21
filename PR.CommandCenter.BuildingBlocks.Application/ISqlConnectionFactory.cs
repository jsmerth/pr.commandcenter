using System.Data;

namespace PR.CommandCenter.BuildingBlocks.Application
{
  public interface ISqlConnectionFactory
  {
    IDbConnection GetOpenConnection();

    IDbConnection CreateNewConnection();

    string GetConnectionString();
  }
}