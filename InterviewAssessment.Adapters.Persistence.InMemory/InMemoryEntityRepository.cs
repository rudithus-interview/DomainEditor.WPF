using System;
using System.Collections.Generic;
using System.Linq;
using InterviewAssessment.Models;
using InterviewAssessment.Ports.Persistence;

namespace InterviewAssessment.Adapters.Persistence.InMemory
{
    public class InMemoryEntityRepository : IEntityRepository
    {
        private readonly List<Entity> _entities = new List<Entity>();
        private int _nextId = 1;

        public Entity Add(string name, int x, int y, IReadOnlyCollection<EntityAttribute> attributes)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var entity = new Entity(_nextId++, name, x, y, attributes);
            _entities.Add(entity);

            return entity;
        }

        public IReadOnlyCollection<Entity> Get()
        {
            return _entities.AsReadOnly();
        }

        public void Update(Entity entity)
        {
            var oldEntity = _entities.First(e => e.Id == entity.Id);
            _entities.Remove(oldEntity);

            _entities.Add(entity);
        }
    }
}
