using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Providers.ShowGrabbing;
using BusinessLayer.Providers.ShowProvider;
using DataLayer.Storages;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace WebApi.IntegrationTests.Controllers
{
    [TestFixture]
    public class ShowControllerTests
    {
        private Mock<IShowGrabber> _showGrabberMock;
        private Mock<IShowStorage> _showStarageMock;
        private ShowController _showController;

        private List<DataLayer.Domains.Show> _shows;

        [SetUp]
        public void SetUp()
        {
            _shows = new List<DataLayer.Domains.Show>();

            _showStarageMock = new Mock<IShowStorage>();
            _showStarageMock
                .Setup(x => x.AddShows(It.IsAny<ICollection<DataLayer.Domains.Show>>()))
                .Returns((ICollection<DataLayer.Domains.Show> shows) =>
                {
                    _shows.AddRange(shows);
                    return Task.CompletedTask;
                });
            _showStarageMock
                .Setup(x => x.GetShows(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int pageNum, int pageSize) => _shows.Skip(pageNum).Take(pageSize).ToList());

            var showProvider = new ShowProvider(_showStarageMock.Object);

            _showGrabberMock = new Mock<IShowGrabber>();
            var showGrabberFactoryMock = new Mock<IShowGrabberFactory>();
            showGrabberFactoryMock
                .Setup(x => x.CreateGrabber(It.IsAny<string>()))
                .Returns(_showGrabberMock.Object);

            _showController = new ShowController(showProvider, showGrabberFactoryMock.Object);
        }

        #region Private setup methods

        private void ConfigureShowGrabberMock()
        {
            _showGrabberMock
                .Setup(g => g.Grab(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new[]
                {
                    new BusinessLayer.Providers.ShowProvider.Entities.Show
                    {
                        Id = 1,
                        Name = "Show #1",
                        People = new[]
                        {
                            new BusinessLayer.Providers.ShowProvider.Entities.Person { Id = 2, Name = "Person #1", Birthday = new DateTime(2000, 8, 1) }
                        }
                    }
                });
        }

        #endregion

        [Test]
        public async Task ShouldFetchShowsToStorageFromExternalResource()
        {
            // Arrange
            ConfigureShowGrabberMock();

            // Act
            await _showController.GrabData("tvMaze");

            // Assert
            var show = _shows.FirstOrDefault();
            Assert.NotNull(show);
            Assert.That(show.ShowId, Is.EqualTo(1));
            Assert.That(show.Name, Is.EqualTo("Show #1"));

            var person = show.People.Select(p => p.Person).FirstOrDefault();
            Assert.NotNull(person);
            Assert.That(person.PersonId, Is.EqualTo(2));
            Assert.That(person.Name, Is.EqualTo("Person #1"));
        }

        [Test]
        public async Task ShouldReturnShowsWithOrderedByDescendingCasts()
        {
            // Arrange
            _shows.Add(
                new DataLayer.Domains.Show
                {
                    People = new DateTime?[]
                        {
                            new DateTime(2000, 1, 1),
                            null,
                            new DateTime(2012, 12, 1),
                            null,
                            new DateTime(1988, 1, 1)
                        }
                        .Select(d => new DataLayer.Domains.ShowPersonAssoc
                        {
                            Person = new DataLayer.Domains.Person
                            {
                                Birthday = d
                            }
                        })
                        .ToList()
                }
            );

            // Act
            var searchResults = await _showController.GetShows();

            // Assert
            CollectionAssert.AreEqual(
                new [] { "2012-12-01", "2000-01-01", "1988-01-01", null, null },
                searchResults.First().Cast.Select(p => p.Birthday)
            );
        }
    }
}
