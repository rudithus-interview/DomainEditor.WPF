using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using InterviewAssessment.App.Events;
using InterviewAssessment.Ports.Persistence;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace InterviewAssessment.App.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly List<EntityViewModel> _newEntities = new List<EntityViewModel>();
        private readonly HashSet<EntityViewModel> _changedEntities = new HashSet<EntityViewModel>();

        private ObservableCollection<EntityViewModel> _entityStore = new ObservableCollection<EntityViewModel>();

        public MainWindowViewModel(IEntityRepository entityRepository, IEventAggregator eventAggregator)
        {
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            _entityStore.CollectionChanged += EntityStoreChanged;

            SaveCommand = new DelegateCommand(Save);

            InitializeEntityStore();
            SubscribeToAddEntityEvent();
        }

        public DelegateCommand SaveCommand { get; private set; }

        public ObservableCollection<EntityViewModel> EntityStore
        {
            get { return _entityStore; }
            set { SetProperty(ref _entityStore, value); }
        }

        private void EntityStoreChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (EntityViewModel newItem in e.NewItems)
                    {
                        newItem.PropertyChanged += EntityChanged;
                    }
                    break;
                default:
                    break;
            }
        }

        private void InitializeEntityStore()
        {
            var entities = _entityRepository.Get().Select(EntityViewModel.FromDomain);
            _entityStore.AddRange(entities);
        }

        private void SubscribeToAddEntityEvent()
        {
            var addEntityEvent = _eventAggregator.GetEvent<AddEntityEvent>();
            addEntityEvent.Subscribe(AddNewEntity);
        }

        private void EntityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is EntityViewModel entity && !_newEntities.Contains(entity))
            {
                _changedEntities.Add(sender as EntityViewModel);
            }
        }

        private void AddNewEntity(EntityViewModel entityViewModel)
        {
            if (entityViewModel == null)
            {
                throw new ArgumentNullException(nameof(entityViewModel));
            }

            _newEntities.Add(entityViewModel);
            _entityStore.Add(entityViewModel);
        }

        private void Save()
        {
            SaveNewEntities();
            SaveChangedEntities();
        }

        private void SaveChangedEntities()
        {
            foreach (var changedEntity in _changedEntities.Select(e => e.ToDomain()))
            {
                _entityRepository.Update(changedEntity);
            }
            _changedEntities.Clear();
        }

        private void SaveNewEntities()
        {
            foreach (var newEntity in _newEntities)
            {
                var entityAttributes = newEntity.Attributes.Select(vm => vm.ToDomainModel()).ToArray();
                var entity = _entityRepository.Add(newEntity.Text, newEntity.X, newEntity.Y, entityAttributes);
                newEntity.Id = entity.Id;
            }
            _newEntities.Clear();
        }
    }
}
