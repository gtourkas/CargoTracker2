using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface ICommandIssuer
    {
        Task IssueAsync<T>(T message) where T : class, ICommand;

    }
}
