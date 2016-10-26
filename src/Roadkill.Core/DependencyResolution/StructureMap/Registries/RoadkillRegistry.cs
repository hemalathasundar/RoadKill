using Roadkill.Core.Configuration;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;

namespace Roadkill.Core.DependencyResolution.StructureMap.Registries
{
    public class RoadkillRegistry : Registry
	{
		public ApplicationSettings ApplicationSettings { get; set; }

		public RoadkillRegistry(ConfigReaderWriter configReader)
		{
			ApplicationSettings = configReader.GetApplicationSettings();

			Scan(ScanTypes);
			ConfigureInstances(configReader);

		    IncludeRegistry<RepositoryRegistry>();
		    IncludeRegistry<MvcRegistry>();
            IncludeRegistry<SecurityRegistry>();
            IncludeRegistry(new PluginsRegistry(configReader));
		    IncludeRegistry<CacheRegistry>();
            IncludeRegistry<ServicesRegistry>();
            IncludeRegistry<TextRegistry>();
		    IncludeRegistry<ToolsRegistry>();
		}

		private void ScanTypes(IAssemblyScanner scanner)
		{
			scanner.TheCallingAssembly();
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.Contains("Roadkill"));
			scanner.SingleImplementationsOfInterface();
			scanner.WithDefaultConventions();

			// Config
			scanner.AddAllTypesOf<ApplicationSettings>();
		}

		private void ConfigureInstances(ConfigReaderWriter configReader)
		{
			// Appsettings and reader - these need to go first
			For<ConfigReaderWriter>().HybridHttpOrThreadLocalScoped().Use(configReader);
			For<ApplicationSettings>()
				.HybridHttpOrThreadLocalScoped()
				.Use(x => x.TryGetInstance<ConfigReaderWriter>().GetApplicationSettings());
		}
	}
}