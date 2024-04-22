using OutSystems.ExternalLibraries.SDK;
using psn.PH.Structures;

namespace psn.PH
{
    /// <summary>
    /// Stripe is a technology company that provides companies the software to accept payments, 
    /// send payouts and manage their businesses online. 
    /// In this connector we provide a block and actions that will allow you to make payments, 
    /// create and get customer details, and request for a refund on submitted charges.
    /// </summary>
    [OSInterface(Description = "This is the external logic to Stripe connector, which provide actions that will allow you to make payments, create and get customer details, and request for a refund on submitted charges.", Name = "Stripe_ExternalLogic", IconResourceName = "psn.PH.StripeExtIcon.png")]
    public interface IStripe_Ext
    {
        /// <summary>
        /// Create a customer with card association.
        /// </summary>
        [OSAction(Description = "Create a customer with payment card association.", ReturnName = "CustomerId")]
        public string CreateCustomerWithCardAssociation_Ext(string api_key, string name, string email, string phone, PaymentMethodCardOptions cardOptions);
        /// <summary>
        /// Create a customer.
        /// </summary>
        [OSAction(Description = "Create a customer without payment card association.", ReturnName = "CustomerId")]
        public string CreateCustomer_Ext(string api_key, string name, string email, string phone);
        /// <summary>
        /// Create a payment card association for a customer.
        /// </summary>
        [OSAction(Description = "Create a payment card association for a customer.", ReturnName = "CardId")]
        public string AddCreditCard_Ext(string api_key, string customerId, psn.PH.Structures.PaymentMethodCardOptions cardOptions);
        /// <summary>
        /// Update a customer.
        /// </summary>
        [OSAction(Description = "Update a customer.", ReturnName = "CustomerId")]
        public string UpdateCustomer_Ext(string api_key, string customerId, CustomerUpdateOptions options);
        /// <summary>
        /// Create a payment intent.
        /// </summary>
        [OSAction(Description = "Create a payment intent.", ReturnName = "Intent")]
        public Intent CreatePaymentIntent_Ext(string api_key, int amount, string currency, bool automatic_payment_method, string customer_id);
        /// <summary>
        /// Create a refund.
        /// </summary>
        [OSAction(Description = "Create a refund.", ReturnName = "Refund")]
        public Refund CreateRefund_Ext(string api_key, string charge_id, long amount, string reason);
        /// <summary>
        /// Create charges.
        /// </summary>
        [OSAction(Description = "Create charges.", ReturnName = "ListofCharges")]
        public List<psn.PH.Structures.Charge> GetCharges_Ext(string api_key, string customer_id, int limit);
        /// <summary>
        /// Search for a customer by email address.
        /// </summary>
        [OSAction(Description = "Search for a customer by email address.", ReturnName = "CustomerId")]
        public string SearchCustomer_Ext(string api_key, string email);
        /// <summary>
        /// Create a check out session.
        /// </summary>
        [OSAction(Description = "Create a check out session.", ReturnName = "JSONResponse")]
        public string CreateCheckoutSession_Ext(string api_key, string customer_id, string successful_url, List<psn.PH.Structures.SessionLineItem> sessionLineItems, string mode);
        /// <summary>
        /// Create a subscription.
        /// </summary>
        [OSAction(Description = "Create a subscription.", ReturnName = "SubscriptionJSON")]
        public string CreateSubscription_Ext(string api_key, string customer_id, List<psn.PH.Structures.SubscriptionLineItem> subscriptionLineItems, List<psn.PH.Structures.SubscriptionMetadata> subscriptionMetadataItems);
        /// <summary>
        /// Retrieve subscriptions of a customer.
        /// </summary>
        [OSAction(Description = "Retrieve subscriptions of customer.", ReturnName = "ListofSubscriptionIds")]
        public List<string> RetrieveSubscriptions_Ext(string api_key, string customer_id);
        /// <summary>
        /// Retrieve subscriptions with details of a customer.
        /// </summary>
        [OSAction(Description = "Retrieve subscriptions with details of customer.", ReturnName = "ListofSubscriptionWithDetails")]
        public List<string> RetreiveSubscriptionsWithDetails_Ext(string api_key, string customer_id);
        /// <summary>
        /// Retrieve subscription details of a customer.
        /// </summary>
        [OSAction(Description = "Retrieve the subscription details of a customer.", ReturnName = "SubscriptionDetails")]
        public string RetrieveSubscriptionDetails_Ext(string api_key, string customer_id, string subscriptionId);

        /// <summary>
        /// Retrieve unique build information of this custom library.
        /// </summary>
        [OSAction(Description = "Get unique build information of this custom library.", ReturnName = "buildInfo")]
        public string GetBuildInfo_Ext();

    }
}