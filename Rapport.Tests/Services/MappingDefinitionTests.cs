using Rapport.Services;
using Xunit;

namespace Rapport.Tests.Services
{
    public class MappingDefinitionTests
    {
        [Fact]
        public void Create()
        {
            var cfg = MappingDefinition.GetConfigurationProvider();
            cfg.AssertConfigurationIsValid();
        }
    }
}
