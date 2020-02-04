using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using InterviewAssessment.Adapters.Persistence.InMemory;
using InterviewAssessment.Models;
using Xunit;

namespace InterviewAssessment.Tests.Adapters.Persistence.InMemory
{
    public class InMemoryEntityRepositoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly InMemoryEntityRepository _sut;

        public InMemoryEntityRepositoryTests()
        {
            _sut = _fixture.Create<InMemoryEntityRepository>();
        }

        [Fact]
        public void ShouldBeGuarded()
        {
            var assertion = new GuardClauseAssertion(_fixture);
            assertion.Verify(typeof(InMemoryEntityRepository).GetConstructors());
        }

        [Theory, AutoData]
        public void GivenEmptyRepository_FirstEntityIdShouldBe1(string name, int x, int y)
        {
            var entity = _sut.Add(name, x, y, new EntityAttribute[] { });

            entity.Id.Should().Be(1);
        }

        [Theory, AutoData]
        public void GivenEntityIsAdded_WhenGet_EntityShouldBeReturned(string name, int x, int y)
        {
            _sut.Add(name, x, y, new EntityAttribute[] { });

            var entities = _sut.Get();

            entities.Count.Should().Be(1);
            var entity = entities.First();
            entity.Name.Should().Be(name);
            entity.X.Should().Be(x);
            entity.Y.Should().Be(y);
        }
    }
}
