using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Monitoring.Container
{
    public interface IRepo
    {
        Task<Container> GetAsync(ContainerId id);

        Task SaveAsync(Container container);

        Task DeleteAsync(ContainerId id);
    }
}
