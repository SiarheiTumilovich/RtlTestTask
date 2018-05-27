using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Providers.ShowProvider;
using DataLayer.Storages;
using Moq;
using NUnit.Framework;

namespace BusinessLayer.Tests.Providers
{
    [TestFixture]
    class ShowProviderTests
    {
        private Mock<IShowStorage> _showStorageMock;
        private IShowProvider _showProvider;

        [SetUp]
        public void Setup()
        {
            _showStorageMock = new Mock<IShowStorage>();
            _showProvider = new ShowProvider(_showStorageMock.Object);
        }

        [Test]
        public async Task ShouldReturnTotalCountInSearchResults()
        {
            // Arrange
            _showStorageMock
                .Setup(x => x.GetShowsCount())
                .ReturnsAsync(123);

            // Act
            var searchResults = await _showProvider.GetShows(0, 1);

            // Assert
            Assert.That(searchResults.TotalCount, Is.EqualTo(123));
        }

        [Test]
        public async Task ShouldReturnItemsInSearchResults()
        {
            // Arrange
            _showStorageMock
                .Setup(x => x.GetShows(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new []
                {
                    new DataLayer.Domains.Show
                    {
                        ShowId = 1,
                        Name = "Show"
                    }
                });

            // Act
            var searchResults = await _showProvider.GetShows(0, 1);

            // Assert
            Assert.NotNull(searchResults.Items);
            Assert.That(searchResults.Items.Count, Is.EqualTo(1));
            Assert.That(searchResults.Items.First().Name, Is.EqualTo("Show"));
        }

        [Test]
        public async Task ShouldSaveShowToStorage()
        {
            // Arrange

            // Act
            await _showProvider.SaveShows(new []
            {
                new BusinessLayer.Providers.ShowProvider.Entities.Show
                {
                    Name = "Name",
                    People = new BusinessLayer.Providers.ShowProvider.Entities.Person[0]
                }
            });

            // Assert
            _showStorageMock
                .Verify(x => x.AddShows(It.IsAny<ICollection<DataLayer.Domains.Show>>()), Times.Once);
        }
    }
}
