using System;
using InterviewAssessment.App.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace InterviewAssessment.App.ViewModels
{
    public class AddEntityDialogViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private string _entityName;

        public AddEntityDialogViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            AddEntityCommand = new DelegateCommand(AddEntity);
        }

        private void AddEntity()
        {
            var rnd = new Random();

            var entityX = rnd.Next((int)MaximumX);
            var entityY = rnd.Next((int)MaximumY);

            var entityViewModel = new EntityViewModel { Text = EntityName, X = entityX, Y = entityY };

            _eventAggregator.GetEvent<AddEntityEvent>().Publish(entityViewModel);
        }

        public string EntityName
        {
            get { return _entityName; }
            set { SetProperty(ref _entityName, value); }
        }

        public double MaximumX { get; set; }

        public double MaximumY { get; set; }

        public DelegateCommand AddEntityCommand { get; private set; }
    }
}