using InterviewAssessment.Ports.Persistence;
using Prism.Ioc;

namespace InterviewAssessment.Adapters.Persistence.InMemory
{
    public static class Bootstrapper
    {
        public static void AddInMemoryPersistence(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IEntityRepository, InMemoryEntityRepository>();
        }
    }
}
