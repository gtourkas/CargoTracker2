using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Monitoring;
using Domain.Monitoring.Container;
using MongoDB.Driver;

using Domain_Container = Domain.Monitoring.Container.Container;
using Data_Container = Infra.MongoDB.Monitoring.Container.Container;
using AutoMapper;

namespace Infra.MongoDB.Monitoring.Container
{
    public class Repo : IRepo
    {
        private readonly IMongoCollection<Data_Container> _coll;

        public Repo(IMongoDatabase database)
        {
            _coll = database.GetCollection<Data_Container>("Containers");
        }

        public async Task<Domain_Container> GetAsync(ContainerId id)
        {
            var d = await _coll
                .FindSync(_ => _.ContainerId == id.Value)
                .FirstOrDefaultAsync();

            return Mapper.Map<Data_Container, Domain_Container>(d);
        }

        public async Task SaveAsync(Domain_Container container)
        {
            var d = Mapper.Map<Domain_Container, Data_Container>(container);

            await _coll.ReplaceOneAsync(
                _ => _.ContainerId == d.ContainerId
                , options: new UpdateOptions() { IsUpsert = true }
                , replacement: d);
        }

        public async Task DeleteAsync(ContainerId id)
        {
            await _coll.DeleteOneAsync(_ => _.ContainerId == id.Value);
        }

    }
}
