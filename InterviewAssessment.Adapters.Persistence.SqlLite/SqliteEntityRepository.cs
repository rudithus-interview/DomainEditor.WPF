using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using InterviewAssessment.Adapters.Persistence.SqlLite.Models;
using InterviewAssessment.Models;
using InterviewAssessment.Ports.Persistence;
using InterviewAssessment.Ports.Persistence.Exceptions;

namespace InterviewAssessment.Adapters.Persistence.SqlLite
{
    public class SqliteEntityRepository : IEntityRepository
    {
        private readonly DbOptions _dbOptions;

        public SqliteEntityRepository(DbOptions dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public Entity Add(string name, int x, int y, IReadOnlyCollection<EntityAttribute> attributes)
        {
            const string entityInsertQuery = @"
INSERT INTO entities(name) 
VALUES(@name);
SELECT last_insert_rowid();";

            const string coordinateInsertQuery = @"
INSERT INTO coords(id, x, y) 
VALUES(@id, @x, @y);";

            const string attributeInsertQuery = @"
INSERT INTO attributes(EntityId, Name, Value)
VALUES(@entityId, @name, @value)";

            try
            {
                using (var con = new SQLiteConnection(_dbOptions.ConnectionString))
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        var entityId = con.QuerySingle<int>(entityInsertQuery, new { name });
                        con.Query(coordinateInsertQuery, new { id = entityId, x, y });

                        foreach (var attribute in attributes)
                        {
                            con.Query(attributeInsertQuery, new { entityId, name = attribute.Name, value = attribute.Value });
                        }

                        transaction.Commit();

                        return new Entity(entityId, name, x, y, attributes);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new EntityAddException("Failed to persist Entity", ex);
            }
        }

        public IReadOnlyCollection<Entity> Get()
        {
            const string entityQuery = @"
SELECT * FROM entities e
JOIN coords c ON c.Id = e.Id;";

            const string attributesQuery = @"
SELECT * FROM attributes
WHERE EntityId = @entityId;";

            try
            {
                using (var con = new SQLiteConnection(_dbOptions.ConnectionString))
                {
                    return con.Query<EntityData, CoordinateData, Entity>(entityQuery,
                        (entityData, coordinateData) =>
                    {
                        var attributes = con.Query<AttributeData>(attributesQuery, new { entityId = entityData.Id }).Select(ea => new EntityAttribute(ea.Name, ea.Value));

                        return new Entity(entityData.Id, entityData.Name, coordinateData.X, coordinateData.Y, attributes);
                    }).ToArray();
                }
            }
            catch (System.Exception ex)
            {
                throw new EntityGetException("Failed to get Entities", ex.InnerException);
            }
        }

        public void Update(Entity entity)
        {
            const string coordsUpdateQuery = @"
UPDATE coords SET x=@x, y=@y
WHERE id=@id";

            const string attributesDeleteQuery = @"
DELETE FROM attributes
WHERE EntityId = @entityId;";

            const string attributesInsertQuery = @"
INSERT INTO attributes(EntityId, Name, Value)
VALUES(@entityId, @name, @value);";

            try
            {
                using (var con = new SQLiteConnection(_dbOptions.ConnectionString))
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        con.Query(coordsUpdateQuery, new { id = entity.Id, x = entity.X, y = entity.Y });
                        con.Query(attributesDeleteQuery, new { entityId = entity.Id });
                        foreach (var attribute in entity.Attributes)
                        {
                            con.Query(attributesInsertQuery, new { entityId = entity.Id, name = attribute.Name, value = attribute.Value });
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new EntityUpdateException("Update failed", ex);
            }
        }
    }
}
