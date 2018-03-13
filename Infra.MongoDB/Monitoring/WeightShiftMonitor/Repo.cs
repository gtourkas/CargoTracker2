using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Monitoring;
using Domain.Monitoring.WeightShiftMonitor;
using MongoDB.Driver;
using Domain_Monitor = Domain.Monitoring.WeightShiftMonitor.Monitor;
using Data_Monitor = Infra.MongoDB.Monitoring.WeightShiftMonitor.Monitor;
using AutoMapper;

namespace Infra.MongoDB.Monitoring.WeightShiftMonitor
{
    public class Repo : IRepo
    {
        private readonly IMongoCollection<Data_Monitor> _coll;

        public Repo(IMongoDatabase database)
        {
            _coll = database.GetCollection<Data_Monitor>("WeighShiftMonitors");
        }

        public async Task<Domain_Monitor> GetAsync(ContainerId id)
        {
            var d = await _coll
                .FindSync(_ => _.ContainerId == id.Value)
                .FirstOrDefaultAsync();

            return Mapper.Map<Data_Monitor, Domain_Monitor>(d);
        }

        public async Task SaveAsync(Domain_Monitor monitor)
        {
            var d = Mapper.Map<Domain_Monitor, Data_Monitor>(monitor);

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
