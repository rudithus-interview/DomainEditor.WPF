using System;
using InterviewAssessment.Ports.Persistence;
using Prism.Ioc;

namespace InterviewAssessment.Adapters.Persistence.SqlLite
{
    public static class Bootstrapper
    {
        /// <summary>
        /// Add Sqlite persistence layer
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// <param name="configureOptions">Configure Options</param>
        public static void AddSqlite(this IContainerRegistry containerRegistry, Action<DbOptions> configureOptions)
        {
            var dbOptions = new DbOptions();
            configureOptions(dbOptions);

            containerRegistry.RegisterInstance(dbOptions);
            containerRegistry.Register<IEntityRepository, SqliteEntityRepository>();
        }
    }
}
