using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Monitoring.TempRangeMonitor
{
    public interface IRepo
    {
        Task<Monitor> GetAsync(ContainerId id);

        Task SaveAsync(Monitor container);

        Task DeleteAsync(ContainerId id);
    }
}
