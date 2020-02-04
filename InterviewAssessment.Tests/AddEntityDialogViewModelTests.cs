using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using InterviewAssessment.App.Events;
using InterviewAssessment.App.ViewModels;
using Moq;
using Prism.Events;
using Xunit;

namespace InterviewAssessmentTests
{
    public class AddEntityDialogViewModelTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        private readonly Mock<IEventAggregator> _mockEventAggregator;
        private readonly AddEntityDialogViewModel _sut;

        public AddEntityDialogViewModelTests()
        {
            _mockEventAggregator = _fixture.Freeze<Mock<IEventAggregator>>();
            _sut = _fixture.Create<AddEntityDialogViewModel>();
        }

        [Fact]
        public void ConstructorShouldBeGuarded()
        {
            var assertion = new GuardClauseAssertion(_fixture);
            assertion.Verify(typeof(AddEntityDialogViewModel).GetConstructors());
        }

        [Theory, AutoData]
        public void WhenAddingEntity_AddEntityEventShouldBePublished(Mock<AddEntityEvent> mockEvent)
        {
            _mockEventAggregator.Setup(aggregator => aggregator.GetEvent<AddEntityEvent>()).Returns(mockEvent.Object);

            _sut.AddEntityCommand.Execute();

            mockEvent.Verify(e => e.Publish(It.Is<EntityViewModel>(vm => vm.Text == _sut.EntityName)), Times.Once);
        }
    }
}
