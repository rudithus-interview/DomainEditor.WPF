using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;

namespace InterviewAssessment.App.ViewModels
{
    public class EntityAttributesPanelViewModel : BindableBase
    {
        private ObservableCollection<EntityAttributeViewModel> _attributes;

        public EntityAttributesPanelViewModel(ObservableCollection<EntityAttributeViewModel> attributes)
        {
            _attributes = attributes;
            AddAttributeCommand = new DelegateCommand(AddAttribute);
        }

        public ObservableCollection<EntityAttributeViewModel> Attributes
        {
            get { return _attributes; }
            set { SetProperty(ref _attributes, value); }
        }

        public DelegateCommand AddAttributeCommand { get; private set; }

        private void AddAttribute()
        {
            Attributes.Add(new EntityAttributeViewModel());
        }
    }
}
