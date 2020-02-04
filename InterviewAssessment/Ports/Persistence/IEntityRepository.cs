using System.Collections.Generic;
using InterviewAssessment.Models;
using InterviewAssessment.Ports.Persistence.Exceptions;

namespace InterviewAssessment.Ports.Persistence
{
    /// <summary>
    /// Repository for retrieving and adding <see cref="Entity"/> 
    /// </summary>
    public interface IEntityRepository
    {
        /// <summary>
        /// Use for persisting an <see cref="Entity"/> If persist is successful, returned Entity will contain <see cref="Entity.Id"/>
        /// </summary>
        /// <param name="name">Name of <see cref="Entity"/></param>
        /// <param name="x">X coordinate of <see cref="Entity"/></param>
        /// <param name="y">Y coordinate of <see cref="Entity"/></param>
        /// <returns>Persisted Entity</returns>
        /// <exception cref="EntityAddException"></exception>
        Entity Add(string name, int x, int y, IReadOnlyCollection<EntityAttribute> attributes);

        /// <summary>
        /// Use for updating an <see cref="Entity"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="EntityUpdateException"></exception>
        void Update(Entity entity);

        /// <summary>
        /// Use to get all entities
        /// </summary>
        /// <returns>Entity Collection</returns>
        /// <exception cref="EntityGetException"></exception>
        IReadOnlyCollection<Entity> Get();
    }
}
