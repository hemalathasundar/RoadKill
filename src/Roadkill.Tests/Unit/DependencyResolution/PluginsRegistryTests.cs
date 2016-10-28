using NUnit.Framework;
using Roadkill.Core.DependencyResolution.StructureMap.Registries;
using Roadkill.Core.Plugins;

namespace Roadkill.Tests.Unit.DependencyResolution
{
	[TestFixture]
	[Category("Unit")]
	public class PluginsRegistryTests : RegistryTestsBase
    {
		[SetUp]
		public void Setup()
		{
			Container = CreateContainer(new PluginsRegistry(ConfigReaderWriterStub));
		}

		[Test]
		public void should_register_default_pluginfactory()
		{
			// Arrange + Act + Assert
			AssertDefaultType<IPluginFactory, PluginFactory>();
		}
	}
}
