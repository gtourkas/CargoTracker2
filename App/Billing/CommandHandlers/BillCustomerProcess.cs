using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Billing.CommandHandlers
{
    public class BillCustomerProcess
    {
        public State CurrentState { get; set; }

        public Guid TrackingId { get; set; }
    }
}
