using AutoFixture.Xunit2;
using FluentAssertions;
using InterviewAssessment.App.ViewModels;
using InterviewAssessment.Models;
using Xunit;

namespace InterviewAssessmentTests
{
    public class EntityViewModelTests
    {
        [Theory, AutoData]
        public void GivenEntity_WhenConvertingToViewModel_ConversionShouldBeCorrect(Entity entity)
        {
            var viewModel = EntityViewModel.FromDomain(entity);

            viewModel.Id.Should().Be(entity.Id);
            viewModel.Text.Should().Be(entity.Name);
            viewModel.X.Should().Be(entity.X);
            viewModel.Y.Should().Be(entity.Y);
        }

        [Theory, AutoData]
        public void WhenTextIsSet_PropertyChangedEventShouldTrigger(EntityViewModel sut, string newText)
        {
            using (var monitoredSubject = sut.Monitor())
            {
                sut.Text = newText;

                monitoredSubject.Should().RaisePropertyChangeFor(subject => subject.Text);
            }
        }

        [Theory, AutoData]
        public void WhenXIsSet_PropertyChangedEventShouldTrigger(EntityViewModel sut, int newX)
        {
            using (var monitoredSubject = sut.Monitor())
            {
                sut.X = newX;

                monitoredSubject.Should().RaisePropertyChangeFor(subject => subject.X);
            }
        }

        [Theory, AutoData]
        public void WhenYIsSet_PropertyChangedEventShouldTrigger(EntityViewModel sut, int newY)
        {
            using (var monitoredSubject = sut.Monitor())
            {
                sut.Y = newY;

                monitoredSubject.Should().RaisePropertyChangeFor(subject => subject.Y);
            }
        }
    }
}
