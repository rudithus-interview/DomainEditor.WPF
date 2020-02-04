using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using InterviewAssessment.App.Events;
using InterviewAssessment.App.ViewModels;
using InterviewAssessment.Models;
using InterviewAssessment.Ports.Persistence;
using Moq;
using Prism.Events;
using Xunit;

namespace InterviewAssessmentTests
{
    public class MainWindowViewModelTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        private readonly MainWindowViewModel _sut;
        private readonly AddEntityEvent _addEntityEvent = new AddEntityEvent();
        private readonly Mock<IEntityRepository> _mockRepository;

        public MainWindowViewModelTests()
        {
            var mockEventAggregator = _fixture.Freeze<Mock<IEventAggregator>>();
            mockEventAggregator.Setup(aggregator => aggregator.GetEvent<AddEntityEvent>()).Returns(_addEntityEvent);
            _mockRepository = _fixture.Freeze<Mock<IEntityRepository>>();
            _sut = _fixture.Build<MainWindowViewModel>().OmitAutoProperties().Create();
        }

        [Fact]
        public void ConstructorShouldBeGuarded()
        {
            var assertion = new GuardClauseAssertion(_fixture);
            assertion.Verify(typeof(MainWindowViewModel).GetConstructors());
        }

        [Theory, AutoData]
        public void WhenConstructing_GivenEntityRepositoryReturnsEntities_EntityStoreShouldContainEntities(Entity[] entities)
        {
            var expectedCollection = entities.Select(e => EntityViewModel.FromDomain(e)).ToList();
            _mockRepository.Setup(repository => repository.Get()).Returns(entities);

            var sut = _fixture.Build<MainWindowViewModel>().OmitAutoProperties().Create();

            sut.EntityStore.Should().BeEquivalentTo(expectedCollection);
        }

        [Fact]
        public void WhenConstructing_SaveCommandIsNotNull()
        {
            _sut.SaveCommand.Should().NotBeNull();
        }

        [Fact]
        public void WhenConstructingMainWindowViewModel_EntityStoreShouldHaveDemoData()
        {
            _sut.EntityStore.Should().NotBeEmpty();
        }

        [Fact]
        public void WhenEntityStoreIsSet_PropertyChangedEventShouldTrigger()
        {
            using (var monitoredSubject = _sut.Monitor())
            {
                _sut.EntityStore = _fixture.Create<ObservableCollection<EntityViewModel>>();

                monitoredSubject.Should().RaisePropertyChangeFor(sut => sut.EntityStore);
            }
        }

        [Theory, AutoData]
        public void GivenEntityAddedEvent_WhenAdding_EntityShouldBeAddedToObservableStore(EntityViewModel entityViewModel)
        {
            _addEntityEvent.Publish(entityViewModel);

            _sut.EntityStore.Should().Contain(entityViewModel);
        }

        [Theory, AutoData]
        public void GivenEntityAddedEvent_WhenSaving_EntityShouldBeAddedToRepository(EntityViewModel newEntity)
        {
            var expectedName = string.Empty;
            int expectedX = -1;
            int expectedY = -1;
            var expectedAttributes = new List<EntityAttribute>();

            _mockRepository
                .Setup(repository => repository.Add(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<IReadOnlyCollection<EntityAttribute>>()))
                .Returns(_fixture.Create<Entity>())
                .Callback<string, int, int, IReadOnlyCollection<EntityAttribute>>((name, x, y, attributes) =>
                {
                    expectedName = name;
                    expectedX = x;
                    expectedY = y;
                    expectedAttributes = attributes.ToList();
                });

            _addEntityEvent.Publish(newEntity);

            _sut.SaveCommand.Execute();

            _mockRepository.Verify();
            VerifyEntity(newEntity, expectedName, expectedX, expectedY, expectedAttributes);
        }

        [Theory, AutoData]
        public void GivenEntityIsChanged_WhenSaving_RepositoryShouldBeUpdated(
            int newX)
        {
            var randomEntity = _sut.EntityStore.GetRandomItem();
            randomEntity.X = newX;

            _sut.SaveCommand.Execute();

            _mockRepository.Verify(repository => repository.Update(It.Is<Entity>(e => e.Id == randomEntity.Id)), Times.Once);
        }

        [Fact]
        public void GivenNullEntityViewModel_WhenAdding_ShouldThrowArgumentNullException()
        {
            Action action = () => _addEntityEvent.Publish(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory, AutoData]
        public void GivenNewEntity_WhenEntityIsChangedWithoutSaving_EntityShouldBeAddedToRepository_AndNoUpdateShouldBePerformed(EntityViewModel newEntity, int newX)
        {
            var expectedName = string.Empty;
            int expectedX = -1;
            int expectedY = -1;
            var expectedAttributes = new List<EntityAttribute>();

            _mockRepository
                .Setup(repository => repository.Add(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<IReadOnlyCollection<EntityAttribute>>()))
                .Returns(_fixture.Create<Entity>())
                .Callback<string, int, int, IReadOnlyCollection<EntityAttribute>>((name, x, y, attributes) =>
                {
                    expectedName = name;
                    expectedX = x;
                    expectedY = y;
                    expectedAttributes = attributes.ToList();
                });

            _addEntityEvent.Publish(newEntity);
            newEntity.X = newX;

            _sut.SaveCommand.Execute();

            VerifyEntity(newEntity, expectedName, expectedX, expectedY, expectedAttributes);
            _mockRepository.Verify(repository => repository.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Theory, AutoData]
        public void GivenNewEntity_WhenSaved_AndNoAdditionalChange_SecondSaveShouldNotUpdateRepository(EntityViewModel newEntity)
        {
            var expectedName = string.Empty;
            int expectedX = -1;
            int expectedY = -1;
            var expectedAttributes = new List<EntityAttribute>();

            _mockRepository
                .Setup(repository => repository.Add(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<IReadOnlyCollection<EntityAttribute>>()))
                .Returns(_fixture.Create<Entity>())
                .Callback<string, int, int, IReadOnlyCollection<EntityAttribute>>((name, x, y, attributes) =>
                {
                    expectedName = name;
                    expectedX = x;
                    expectedY = y;
                    expectedAttributes = attributes.ToList();
                });

            _addEntityEvent.Publish(newEntity);

            _sut.SaveCommand.Execute();
            _sut.SaveCommand.Execute();

            VerifyEntity(newEntity,expectedName,expectedX,expectedY,expectedAttributes);
        }

        [Theory, AutoData]
        public void GivenEntityChange_WhenSaved_AndNoAdditionalChange_SecondSaveShouldNotUpdateRepository(int newX)
        {
            var entity = _sut.EntityStore.GetRandomItem();
            entity.X = newX;

            _sut.SaveCommand.Execute();
            _sut.SaveCommand.Execute();

            _mockRepository.Verify(repository => repository.Update(It.Is<Entity>(e => e.Id == entity.Id)), Times.Once);
        }


        private static void VerifyEntity(EntityViewModel newEntity, string expectedName, int expectedX, int expectedY, List<EntityAttribute> expectedAttributes)
        {
            expectedName.Should().Be(newEntity.Text);
            expectedX.Should().Be(newEntity.X);
            expectedY.Should().Be(newEntity.Y);
            expectedAttributes.Should().BeEquivalentTo(newEntity.Attributes.Select(ea => ea.ToDomainModel()));
        }
    }

}
