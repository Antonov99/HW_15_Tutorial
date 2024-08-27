using System.Collections.Generic;
using Services;
using SqliteModule;
using UnityEngine;

namespace Game.App
{
    [CreateAssetMenu(
        fileName = "Database Service Pack",
        menuName = "App/Database/New Database Service Pack"
    )]
    public sealed class DatabaseServicePack : ServicePackBase
    {
        private SqliteDatabase database;

        private SqliteDatabaseInstaller databaseInstaller;

        private SqliteDatabaseUpdater databaseUpdater;
        
        public override IEnumerable<object> ProvideServices()
        {
            var dbPath = $"URI=file:{DatabaseConfig.DestinationPath}";
            database = new SqliteDatabase(dbPath);

            var adapter = new DatabaseAdapter();
            databaseInstaller = new SqliteDatabaseInstaller(adapter);
            databaseUpdater = new SqliteDatabaseUpdater(database, adapter);

            yield return database;
            yield return databaseInstaller;
            yield return databaseUpdater;
        }
    }
}