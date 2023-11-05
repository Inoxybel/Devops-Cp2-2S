using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Infra.Options;

namespace Infra;

public class DbContext
{
    public IMongoCollection<User> User { get; }
    public IMongoCollection<ActivationCode> ActivationCode { get; }

    protected DbContext() { }

    public DbContext(IOptions<DatabaseInstanceOptions> databaseInstanceOptions)
    {
        var client = new MongoClient(databaseInstanceOptions.Value.ConnectionString);

        var database = client.GetDatabase(databaseInstanceOptions.Value.Name);

        User = database.GetCollection<User>(nameof(User));
        ActivationCode = database.GetCollection<ActivationCode>(nameof(ActivationCode));
    }
}
