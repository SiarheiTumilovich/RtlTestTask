using NUnit.Framework;

namespace BusinessLayer.Tests
{
    [SetUpFixture]
    class AssemblyFixture
    {
        [OneTimeSetUp]
        public void T()
        {
            AutoMapper.Mapper.Initialize(Bootstrapper.ConfigureMapping);
        }
    }
}
