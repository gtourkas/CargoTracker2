using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface IHandlerFactory<T> where T: class
    {
        IHandler<T> Create();
    }

    public interface IHandler<T> where T : class
    {
        Task HandleAsync(T msg);
    }
}
