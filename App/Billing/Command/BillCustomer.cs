using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Billing.Command
{
    public class BillCustomer
    {
        public Guid TrackingId { get; set; }

        public Guid CustomerId { get; set; }
    }
}
