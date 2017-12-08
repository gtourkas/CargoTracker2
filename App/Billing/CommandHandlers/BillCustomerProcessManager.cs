using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Billing.CommandHandlers
{
    public class BillCustomerProcessManager : AutomatonymousStateMachine<BillCustomerProcess>
    {
        public State CargoDelivered { get; private set; }
        public State PaymentProcessed { get; private set; }
        public State InvoiceIssued { get; private set; }


        public Event ProcessPayment { get; private set; }
        public Event IssueInvoice { get; private set; }
        public Event NotifyCustomer { get; private set; }
        // public Event<Person> Introduce { get; private set; }

    }
}
