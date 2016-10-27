using System.Collections.Generic;
using Roadkill.Core.Database.MongoDB;
using Roadkill.Core.Database.Repositories;
using Roadkill.Core.Database.Repositories.Dapper;

namespace Roadkill.Core.Database
{
	public class DapperRepositoryFactory : IRepositoryFactory
	{
		// Hack to make sure the factory doesn't return invalid Repositories, while installing.
		private readonly bool _pendingInstallation;

		public DapperRepositoryFactory()
		{
		}

		public DapperRepositoryFactory(string databaseProviderName, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				_pendingInstallation = true;
			}
		}

        private static IDbConnectionFactory CreateDbConnectionFactory(string databaseProviderName, string connectionString)
        {
            IDbConnectionFactory dbConnectionFactory = new SqlConnectionFactory(connectionString);
            if (databaseProviderName == SupportedDatabases.Postgres)
            {
                dbConnectionFactory = new PostgresConnectionFactory(connectionString);
            }

            return dbConnectionFactory;
        }

		public ISettingsRepository GetSettingsRepository(string databaseProviderName, string connectionString)
		{
			if (_pendingInstallation)
				return null;

			if (databaseProviderName == SupportedDatabases.MongoDB)
			{
				return new MongoDBSettingsRepository(connectionString);
			}
			else
            {
                IDbConnectionFactory dbConnectionFactory = CreateDbConnectionFactory(databaseProviderName, connectionString);
                return new DapperSettingsRepository(dbConnectionFactory);
            }
        }

        public IUserRepository GetUserRepository(string databaseProviderName, string connectionString)
		{
			if (_pendingInstallation)
				return null;

			if (databaseProviderName == SupportedDatabases.MongoDB)
			{
				return new MongoDBUserRepository(connectionString);
			}
			else
			{
                IDbConnectionFactory dbConnectionFactory = CreateDbConnectionFactory(databaseProviderName, connectionString);
                return new DapperUserRepository(dbConnectionFactory);
            }
		}

		public IPageRepository GetPageRepository(string databaseProviderName, string connectionString)
		{
			if (_pendingInstallation)
				return null;

			if (databaseProviderName == SupportedDatabases.MongoDB)
			{
				return new MongoDBPageRepository(connectionString);
			}
			else
			{
                IDbConnectionFactory dbConnectionFactory = CreateDbConnectionFactory(databaseProviderName, connectionString);
                return new DapperPageRepository(dbConnectionFactory);
            }
		}

		public IEnumerable<RepositoryInfo> ListAll()
		{
			return new List<RepositoryInfo>()
			{
				SupportedDatabases.MongoDB,
				SupportedDatabases.MySQL,
				SupportedDatabases.Postgres,
				SupportedDatabases.SqlServer2008
			};
		}
	}
}