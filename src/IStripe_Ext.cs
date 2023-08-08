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
    [OSInterface(Description = "Stripe is a technology company that provides companies the software to accept payments, send payouts and manage their businesses online. In this connector we provide a block and actions that will allow you to make payments, create and get customer details, and request for a refund on submitted charges.", Name = "Stripe_Ext", IconResourceName = "psn.PH.StripeExtIcon.png")]
    public interface IStripe_Ext
    {
        /// <summary>
        /// Create a customer.
        /// </summary>
        [OSAction(Description = "Create a customer.", ReturnName = "CustomerId")]
        public string CreateCustomer_Ext(string api_key, string name, string email, string phone);
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
        public string CreateCheckoutSession_Ext(string api_key, string successful_url, List<psn.PH.Structures.SessionLineItem> sessionLineItems, string mode);

        [OSAction(Description = "Create a subscription.", ReturnName = "SubscriptionId")]
        public string CreateSubscription_Ext(string api_key, string customer_id, List<psn.PH.Structures.SubscriptionLineItem> subscriptionLineItems, List<psn.PH.Structures.SubscriptionMetadata> subscriptionMetadataItems);

        [OSAction(Description = "Retrieve subscriptions of customer.", ReturnName = "ListofSubscriptionIds")]
        public List<string> RetrieveSubscriptions_Ext(string api_key, string customer_id);
        [OSAction(Description = "Get unique build information for this custom library.", ReturnName = "buildInfo")]
        public string GetBuildInfo_Ext();

    }
}