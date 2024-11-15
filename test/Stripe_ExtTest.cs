using Xunit;
using Xunit.Abstractions;

using psn.PH.Structures;
namespace psn.PH;

public class Stripe_ExtTests
{
    private string api_key = Environment.GetEnvironmentVariable("STRIPE_API_KEY");
    private readonly ITestOutputHelper output;

    public Stripe_ExtTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    private string CreateSimpleCustomerWithCreditCardAssociation(string name, string email, string phone)
    {
        var se = new Stripe_Ext();
        var cust_id = se.CreateCustomerWithCardAssociation_Ext(api_key, name, email, phone, new PaymentMethodCardOptions
        {
            Number = "4242424242424242", // see https://www.memberstack.com/blog/stripe-test-cards
            ExpMonth = 12,
            ExpYear = 2034,
            Cvc = "314",
            Token = "tok_us", // see https://www.memberstack.com/blog/stripe-test-cards
        });
        return cust_id;
    }

    [Fact]
    public void CreateCustomer_Ext_test1()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        // testing with card association
        var cust_id = se.CreateCustomer_Ext(api_key, name, email, phone);
        output.WriteLine("Customer ID = " + cust_id.ToString());
        Assert.True(cust_id.ToString().Length > 0);
        Assert.StartsWith("cus_", cust_id.ToString());
    }

    [Fact]
    public void CreateCustomer_Ext_test2()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        // testing with card association
        var cust_id = se.CreateCustomerWithCardAssociation_Ext(api_key, name, email, phone, new PaymentMethodCardOptions
        {
            Number = "4242424242424242",
            ExpMonth = 12,
            ExpYear = 2034,
            Cvc = "314",
            Token = "tok_us",
        });
        output.WriteLine("Customer ID = " + cust_id.ToString());
        Assert.True(cust_id.ToString().Length > 0);
        Assert.StartsWith("cus_", cust_id.ToString());
    }

    [Fact]
    public void AddCreditCard_Ext_test1()
    {
        var se = new Stripe_Ext();

        string name = "John Doe AddCreditCard_Ext_test1";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        var cust_id = CreateSimpleCustomerWithCreditCardAssociation(name, email, phone);
        var card_id = se.AddCreditCard_Ext(api_key, cust_id, new PaymentMethodCardOptions
        {
            Number = "4242424242424242",
            ExpMonth = 12,
            ExpYear = 2034,
            Cvc = "314",
            Token = "tok_us",
        });
        output.WriteLine("Card ID = " + card_id.ToString());
        Assert.True(card_id.ToString().Length > 0);
        Assert.StartsWith("card_", card_id.ToString());
    }

    [Fact]
    public void UpdateCustomer_Ext_test1()
    {
        var se = new Stripe_Ext();

        string name = "John Doe UpdateCustomer_Ext_test1";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        var cust_id = CreateSimpleCustomerWithCreditCardAssociation(name, email, phone);

        AddressOptions testAddress = new AddressOptions
        {
            City = "Singapore",
            Country = "Singapore",
            Line1 = "1 nowhere",
            Line2 = "OffTheRoad 1234",
            State = "Singapore",
            PostalCode = "1234",
        };

        ShippingOptions shippingOptions = new ShippingOptions
        {
            Address = testAddress,
            Name = "Mary Sim",
            Phone = "+192496224",
        };

        CustomerUpdateOptions cuo = new CustomerUpdateOptions
        {
            Name = "Doe John UpdateCustomer_Ext_test1",
            Email = "doe.john@nowhere.com",
            Phone = "+1982659123",
            Address = testAddress,
            Shipping = shippingOptions,
        };
        output.WriteLine("Update customerID = " + cust_id);
        var cust_id_updated = se.UpdateCustomer_Ext(api_key, cust_id, cuo);
        Assert.StartsWith("cus_", cust_id_updated.ToString());
        Assert.True(cust_id_updated.ToString().Length > 0 && cust_id_updated == cust_id.ToString().Trim());
    }

    [Fact]
    public void CreatePaymentIntent_Ext_test1()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        var cust_id = CreateSimpleCustomerWithCreditCardAssociation(name, email, phone);
        Intent intent = se.CreatePaymentIntent_Ext(api_key, 100, "sgd", true, false, cust_id);
        output.WriteLine("client_secret = " + intent.client_secret);

        Assert.True(intent.client_secret.Length > 0);
    }

    [Fact]
    public void CreateCheckoutSession_Ext_test1()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe_ccos_test1@nowhere.com";
        string phone = "+1982659123";
        var cust_id = CreateSimpleCustomerWithCreditCardAssociation(name, email, phone);

        var lineItems = new List<psn.PH.Structures.SessionLineItem>
            {
                new psn.PH.Structures.SessionLineItem
                {
                price_id = "price_1NbbgLCrPSPWXnSu7l0PCoGt",
                quantity = 2,
                },
                new psn.PH.Structures.SessionLineItem
                {
                price_id = "price_1Ne7VvCrPSPWXnSuRH0Yrkld",
                quantity = 2,
                },
            };
        var session = se.CreateCheckoutSession_Ext(api_key, cust_id, "http://www.nowhere.com/main", lineItems, "subscription");
        output.WriteLine(session);
    }

    [Fact]
    public void CreateCheckoutSession_Ext_test2()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe_ccos_test2@nowhere.com";
        string phone = "+1982659123";
        var cust_id = CreateSimpleCustomerWithCreditCardAssociation(name, email, phone);
        var lineItems = new List<psn.PH.Structures.SessionLineItem>
            {
                new psn.PH.Structures.SessionLineItem
                {
                price_id = "price_1NbbkuCrPSPWXnSudFVjHd80",
                quantity = 5,
                },
                new psn.PH.Structures.SessionLineItem
                {
                price_id = "price_1NbbkuCrPSPWXnSudFVjHd80", // price_1Ne7WxCrPSPWXnSuMb4OYG18
                quantity = 2,
                },
            };
        var session = se.CreateCheckoutSession_Ext(api_key, cust_id, "http://www.nowhere.com/main", lineItems, "payment");
        output.WriteLine(session);
    }

    [Fact]
    public void CreateSubscription_Ext_test1()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        var cust_id = CreateSimpleCustomerWithCreditCardAssociation(name, email, phone);
        List<psn.PH.Structures.SubscriptionLineItem> subscriptionLineItems = new List<PH.Structures.SubscriptionLineItem> {
            new SubscriptionLineItem {
                price_id = "price_1Ne7VvCrPSPWXnSuRH0Yrkld",
            }
        };
        List<psn.PH.Structures.SubscriptionMetadata> subscriptionMetadataItems = new List<PH.Structures.SubscriptionMetadata> {
            new SubscriptionMetadata {
                key = "color",
                value = "red"
            },
            new SubscriptionMetadata {
                key = "origin",
                value = "Europe"
            }
        };
        var subscription = se.CreateSubscription_Ext(api_key, cust_id, "", subscriptionLineItems, subscriptionMetadataItems);
        Assert.True(subscription.Length > 0 && subscription.IndexOf("\"id\": \"sub_") > 0 && subscription.IndexOf("\"object\": \"subscription\"") > 0);
    }

    [Fact]
    public void SearchCustomer_Ext_test1()
    {
        var se = new Stripe_Ext();
        Assert.True(se.SearchCustomer_Ext(api_key, "notfound@nowhere.com") == string.Empty);
    }

    [Fact]
    public void SearchCustomer_Ext_test2()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe.searchtest2@nowhere.com";
        string phone = "+1982659123";
        // testing with card association
        var cust_id = se.CreateCustomer_Ext(api_key, name, email, phone);
        output.WriteLine("SearchTest2 Cust_id = " + cust_id);
        Assert.True(se.SearchCustomer_Ext(api_key, "john.doe.searchtest2@nowhere.com") != string.Empty);
    }

    [Fact]
    public void GetBuildInfo_Ext_test1()
    {
        var se = new Stripe_Ext();
        string buildInfo = se.GetBuildInfo_Ext();
        output.WriteLine(buildInfo);
        Assert.True(buildInfo.Length > 0);
    }
}