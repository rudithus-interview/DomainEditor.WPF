using System.Collections.Generic;
using System.Linq;

namespace InterviewAssessment.Models
{
    public class Entity
    {
        public Entity(
            int id,
            string name,
            int x,
            int y,
            IEnumerable<EntityAttribute> attributes)
        {
            Id = id;
            Name = name;
            X = x;
            Y = y;
            Attributes = attributes.ToList();
        }

        public int Id { get; }

        public string Name { get; }

        public int X { get; }

        public int Y { get; }

        public List<EntityAttribute> Attributes { get; }
    }
}
