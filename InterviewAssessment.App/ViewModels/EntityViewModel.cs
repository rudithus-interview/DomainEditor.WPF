using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using InterviewAssessment.Models;
using Prism.Mvvm;

namespace InterviewAssessment.App.ViewModels
{
    public class EntityViewModel : BindableBase
    {
        private int _id;
        private int _x;
        private int _y;
        private string _text;

        public EntityViewModel()
        {
            Attributes = new ObservableCollection<EntityAttributeViewModel>();
            Attributes.CollectionChanged += Attributes_CollectionChanged;
        }

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public int X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        public int Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private void Attributes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (EntityAttributeViewModel attribute in e.NewItems)
                    {
                        attribute.PropertyChanged += Attribute_PropertyChanged;
                    }
                    break;
            }
        }

        private void Attribute_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        public ObservableCollection<EntityAttributeViewModel> Attributes { get; private set; }

        public Entity ToDomain()
        {
            var entityAttributes = Attributes.Select(vm => vm.ToDomainModel());
            return new Entity(Id, Text, X, Y, entityAttributes);
        }

        public static EntityViewModel FromDomain(Entity entity)
        {
            var entityViewModel = new EntityViewModel
            {
                Id = entity.Id,
                X = entity.X,
                Y = entity.Y,
                Text = entity.Name
            };
            entityViewModel.Attributes.AddRange(entity.Attributes.Select(a => new EntityAttributeViewModel { Name = a.Name, Value = a.Value, EntityId = entity.Id }));

            return entityViewModel;
        }
    }
}
