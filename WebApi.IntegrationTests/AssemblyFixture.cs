using NUnit.Framework;

namespace WebApi.IntegrationTests
{
    [SetUpFixture]
    class AssemblyFixture
    {
        [OneTimeSetUp]
        public void T()
        {
            AutoMapper.Mapper.Initialize(Startup.ConfigureMapping);
        }
    }
}
