using Domain.Monitoring.TempRangeMonitor;

using Domain_Monitor = Domain.Monitoring.TempRangeMonitor.Monitor;
using Data_Monitor = Infra.MongoDB.Monitoring.TempRangeMonitor.Monitor;
using MongoDB.Driver;
using Domain.Monitoring;
using AutoMapper;
using System.Threading.Tasks;

namespace Infra.MongoDB.Monitoring.TempRangeMonitor
{
    public class Repo : IRepo
    {
        private readonly IMongoCollection<Data_Monitor> _coll;

        public Repo(IMongoDatabase database)
        {
            _coll = database.GetCollection<Data_Monitor>("TempRangeMonitors");
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
