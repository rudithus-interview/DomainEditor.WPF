using InterviewAssessment.App.ViewModels;
using Prism.Events;

namespace InterviewAssessment.App.Events
{
    public class AddEntityEvent : PubSubEvent<EntityViewModel> { }
}
