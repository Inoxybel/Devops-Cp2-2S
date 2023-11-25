using MongoDB.Bson.Serialization.Conventions;

namespace DevOps_CP2_4S.Infra;

public static class MongoConfiguration
{
    public static void RegisterConfigurations()
    {
        var pack = new ConventionPack()
        {
            new IgnoreExtraElementsConvention(true),
            new CamelCaseElementNameConvention(),
            new EnumRepresentationConvention(MongoDB.Bson.BsonType.String)
        };

        ConventionRegistry.Register("My Solution Conventions", pack, t => true);
    }
}
