using InterviewAssessment.Models;
using Prism.Mvvm;

namespace InterviewAssessment.App.ViewModels
{
    public class EntityAttributeViewModel : BindableBase
    {
        private int _id;
        private int _entityId;
        private string _name;
        private string _value;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int EntityId
        {
            get { return _entityId; }
            set { SetProperty(ref _entityId, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public EntityAttribute ToDomainModel()
        {
            return new EntityAttribute(Name, Value);
        }
    }
}
