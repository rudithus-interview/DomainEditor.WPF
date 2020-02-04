using System;
using System.Configuration;
using System.Windows;
using InterviewAssessment.Adapters.Persistence.InMemory;
using InterviewAssessment.Adapters.Persistence.SqlLite;
using InterviewAssessment.App.Views;
using InterviewAssessment.Models;
using InterviewAssessment.Ports.Persistence;
using Prism.Ioc;
using Prism.Unity;

namespace InterviewAssessment.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private PersistenceMode _persistenceMode;

        protected override Window CreateShell()
        {
            if (_persistenceMode == PersistenceMode.InMemory)
            {
                AddDemoData();
            }

            return Container.Resolve<MainWindow>();
        }

        private void AddDemoData()
        {
            var repository = Container.Resolve<IEntityRepository>();

            repository.Add("Order", 100, 100, new Models.EntityAttribute[] { });
            repository.Add("OrderLine", 200, 200, new Models.EntityAttribute[] { });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!Enum.TryParse(ConfigurationManager.AppSettings["PersistenceMode"], out _persistenceMode))
            {
                throw new ConfigurationErrorsException("PersistenceMode is missing from appsettings");
            }

            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            switch (_persistenceMode)
            {
                case PersistenceMode.InMemory:
                    containerRegistry.AddInMemoryPersistence();
                    break;
                case PersistenceMode.Sqlite:
                    var connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                    containerRegistry.AddSqlite(dbOptions => dbOptions.ConnectionString = connectionString);
                    break;
            }
        }
    }
}
