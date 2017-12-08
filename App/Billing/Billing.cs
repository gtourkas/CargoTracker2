using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Billing
{
    public class Billing
    {
        public Guid TrackingId { get; private set; }
        public Guid CustomerId { get; private set; }
        public double Amount { get; private set; }

        public bool PaymentSucceeded { get; private set; }
        public string InvoiceNumber { get; private set; }

        public Billing(Guid trackingId, Guid customerId, double amount)
        {
            TrackingId = trackingId;
            CustomerId = customerId;
            Amount = amount;
        } 

        public void SetPaymentAsSuccessfull(string invoiceNumber)
        {
            PaymentSucceeded = true;
            InvoiceNumber = invoiceNumber;
        }

        public void SetPaymentAsFailed()
        {
            PaymentSucceeded = false;
        }
    }
}
