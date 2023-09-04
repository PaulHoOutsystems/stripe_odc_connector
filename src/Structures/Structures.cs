using OutSystems.ExternalLibraries.SDK;
using Newtonsoft.Json;

namespace psn.PH.Structures
{
    [OSStructure(Description = "Intent")]
    public struct Intent
    {
        [OSStructureField(DataType = OSDataType.Text, Description = "id", IsMandatory = true)]
        public string id;
        [OSStructureField(DataType = OSDataType.Text, Description = "object", IsMandatory = true)]
        [JsonProperty("object")]
        public string obj;
        [OSStructureField(DataType = OSDataType.LongInteger, Description = "amount", IsMandatory = true)]
        public long amount;
        [OSStructureField(DataType = OSDataType.Text, Description = "client secret", IsMandatory = true)]
        public string client_secret;
        [OSStructureField(DataType = OSDataType.Text, Description = "confirmation method", IsMandatory = true)]
        public string confirmation_method;
        [OSStructureField(DataType = OSDataType.DateTime, Description = "created date time", IsMandatory = true)]
        public DateTime created;
        [OSStructureField(DataType = OSDataType.Text, Description = "currency", IsMandatory = true)]
        public string currency;
        [OSStructureField(DataType = OSDataType.Text, Description = "status", IsMandatory = true)]
        public string status;
    }

    [OSStructure(Description = "Refund")]
    public struct Refund
    {
        [OSStructureField(DataType = OSDataType.Text, Description = "id", IsMandatory = true)]
        public string id;
        [OSStructureField(DataType = OSDataType.Text, Description = "object", IsMandatory = true)]
        [JsonProperty("object")]
        public string obj;
        [OSStructureField(DataType = OSDataType.LongInteger, Description = "amount", IsMandatory = true)]
        public long amount;
        [OSStructureField(DataType = OSDataType.Text, Description = "description", IsMandatory = true)]
        public string description;
        [OSStructureField(DataType = OSDataType.Text, Description = "failure reason", IsMandatory = true)]
        public string failure_reason;
        [OSStructureField(DataType = OSDataType.Text, Description = "payment intent id", IsMandatory = true)]
        public string payment_intent_id;
        [OSStructureField(DataType = OSDataType.Text, Description = "reason", IsMandatory = true)]
        public string reason;
        [OSStructureField(DataType = OSDataType.Text, Description = "receipt number", IsMandatory = true)]
        public string receipt_number;
        [OSStructureField(DataType = OSDataType.Text, Description = "status", IsMandatory = true)]
        public string status;
        [OSStructureField(DataType = OSDataType.Text, Description = "charge id", IsMandatory = true)]
        public string charge_id;
    }

    [OSStructure(Description = "Charge")]
    public struct Charge
    {
        [OSStructureField(DataType = OSDataType.Text, Description = "id", IsMandatory = true)]
        public string id;
        [OSStructureField(DataType = OSDataType.Text, Description = "object", IsMandatory = true)]
        [JsonProperty("object")]
        public string obj;
        [OSStructureField(DataType = OSDataType.LongInteger, Description = "amount", IsMandatory = true)]
        public long amount;
        [OSStructureField(DataType = OSDataType.Text, Description = "payment intent", IsMandatory = true)]
        public string payment_intent;
        [OSStructureField(DataType = OSDataType.Text, Description = "payment intent status", IsMandatory = true)]
        public string payment_intent_status;
    }

    [OSStructure(Description = "Session Line Item")]
    public struct SessionLineItem
    {
        [OSStructureField(DataType = OSDataType.Text, Description = "price Id", IsMandatory = true)]
        public string price_id;
        [OSStructureField(DataType = OSDataType.Integer, Description = "quantity", IsMandatory = true)]
        public int quantity;
    }

    [OSStructure(Description = "Subscription Line Item")]
    public struct SubscriptionLineItem
    {
        [OSStructureField(DataType = OSDataType.Text, Description = "subscription Id", IsMandatory = true)]
        public string price_id;
    }

    [OSStructure(Description = "Subscription Metadata")]
    public struct SubscriptionMetadata
    {
        [OSStructureField(DataType = OSDataType.Text, Description = "metadata key", IsMandatory = true)]
        public string key;
        [OSStructureField(DataType = OSDataType.Text, Description = "metadata value", IsMandatory = true)]
        public string value;
    }

    [OSStructure(Description = "Payment method card options")]
    public struct PaymentMethodCardOptions
    {
        public string Cvc;
        public long? ExpMonth;
        public long? ExpYear;
        public string Number;
        public string Token;
    }
}