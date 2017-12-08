using Automatonymous;
using Domain.Shipping.Cargo.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Billing.ProcessManagers
{
    public class CustomerBillingState
    {
        public State CurrentState { get; set; }
    }


    public class CustomerBilling : AutomatonymousStateMachine<CustomerBillingState>
    {


        public CustomerBilling()
        {
            // events declaration
            Event(() => DeliveryStateChanged);
            Event(() => PaymentExecutedEvent);
            Event(() => InvoiceIssuedEvent);
            Event(() => CustomerNotifiedEvent);

            // states declaration
            State(() => CargoDelivered);
            State(() => PaymentSucceeded);
            State(() => PaymentFailed);
            State(() => InvoiceIssued);
            State(() => CustomerNotified);

            Initially(
                When(DeliveryStateChanged, ctx => { return ctx.Data.Delivery.IsUnloadedAtDestination; })
                    .Then(ctx =>
                    {
                        // execute payment 
                    })
                    .TransitionTo(CargoDelivered)
                );

            During(CargoDelivered,
                When(PaymentExecutedEvent, ctx => { return ctx.Data.Succeeded; })
                    .Then(ctx =>
                    {
                        // issue invoice issue command
                        // TODOs
                    })
                    .TransitionTo(PaymentSucceeded)
                ,
                When(PaymentExecutedEvent, ctx => { return !ctx.Data.Succeeded; })
                    .Then(ctx =>
                    {
                        // issue customer notification command
                        // TODOs
                    })
                    .TransitionTo(PaymentFailed)
                );

            During(PaymentSucceeded,
                When(InvoiceIssuedEvent)
                    .Then(ctx =>
                    {
                        // issue customer notification command
                        // TODO
                    })
                    .TransitionTo(InvoiceIssued)
                );

            During(PaymentFailed,
                When(CustomerNotifiedEvent)
                    .Then(ctx =>
                    {
                        // create the billing aggregate
                        // set billing as failed
                        // persist the aggregate
                    })
                    .Finalize()
                );

            During(InvoiceIssued,
                When(CustomerNotifiedEvent)
                    .Then(ctx =>
                    {
                        // create the billing aggregate
                        // set billing as successfull
                        // persist the aggregate
                    })
                    .Finalize()
                );
        }

        public Event<DeliveryStateChanged> DeliveryStateChanged { get; private set; }
        public Event<PaymentExecutedEvent> PaymentExecutedEvent { get; private set; }
        public Event InvoiceIssuedEvent { get; private set; }
        public Event CustomerNotifiedEvent { get; private set; }

        public State CargoDelivered { get; private set; }
        public State PaymentSucceeded { get; private set; }
        public State PaymentFailed { get; private set; }
        public State InvoiceIssued { get; private set; }
        public State CustomerNotified { get; private set; }


    }
}
