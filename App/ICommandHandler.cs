﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface ICommandHandlerFactory<T> : IHandlerFactory<T> where T: class, ICommand
    { 
    }

    public interface ICommandHandler<T> : IHandler<T> where T : class, ICommand 
    {
    }
}
