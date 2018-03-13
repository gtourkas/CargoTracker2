using System;
using AutoMapper;
using Domain.Monitoring;
using Domain.Monitoring.TempRangeMonitor;
using Domain.Monitoring.WeightShiftMonitor;
using Infra.MongoDB.Monitoring.TempRangeMonitor;
using Mongo2Go;
using Xunit;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infra.MongoDB.Tests
{
    public class TempRangeMonitorRepoTest : IDisposable
    {
        private MongoDbRunner _runner;

        private IMongoDatabase _database;

        public TempRangeMonitorRepoTest()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => {
                cfg.AddProfile<TypeMappings>();
            });

            _runner = MongoDbRunner.Start();

            _database = new MongoClient(_runner.ConnectionString).GetDatabase($"{nameof(TempRangeMonitorRepoTest)}DB");
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public void TypeMappingsConfigurationIsValid()
        {
            Mapper.Configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Save()
        {
            var repo = new Repo(_database);
            repo.SaveAsync(
                new Domain.Monitoring.TempRangeMonitor.Monitor(
                    new ContainerId("ABCDE")
                    , new Domain.Monitoring.TempRangeMonitor.Specification(
                        new Range(new Temp(-4), new Temp(4))
                        , new Duration(TimeSpan.FromSeconds(300))
                    )
                )
            ).Wait();

            var monitor = repo.GetAsync(new ContainerId("ABCDE")).Result;
        }

    }
}

