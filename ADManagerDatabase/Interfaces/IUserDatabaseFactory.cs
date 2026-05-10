

namespace ADManager.Database.Interfaces
{
    /// <summary>
    /// The primary database factory for ADManager.
    /// Creates <see cref="IDatabaseContext"/> types 
    /// that can have a number of database type backings
    /// </summary>
    public interface IUserDatabaseFactory : IAppDatabaseFactory
    {
    }
}