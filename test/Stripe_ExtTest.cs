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

    [Fact]
    public void CreateCustomer_Ext_test1()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        var cust_id = se.CreateCustomer_Ext(api_key, name, email, phone);
        output.WriteLine("Customer ID = " + cust_id.ToString());
        Assert.True(cust_id.ToString().Length > 0);
    }

    [Fact]
    public void CreatePaymentIntent_Ext_test1()
    {
        var se = new Stripe_Ext();
        string name = "John Doe";
        string email = "john.doe@nowhere.com";
        string phone = "+1982659123";
        var cust_id = se.CreateCustomer_Ext(api_key, name, email, phone);
        Intent intent = se.CreatePaymentIntent_Ext(api_key, 100, "sgd", true, cust_id);
        output.WriteLine("client_secret = " + intent.client_secret);

        Assert.True(intent.client_secret.Length > 0);
    }

    [Fact]
    public void CreateCheckoutSession_Ext_test1()
    {
        var se = new Stripe_Ext();
        var lineItems = new List<psn.PH.Structures.SessionLineItem>
            {
                new psn.PH.Structures.SessionLineItem
                {
                price_id = "price_1NbbgLCrPSPWXnSu7l0PCoGt",
                quantity = 2,
                },
            };
        var session = se.CreateCheckoutSession_Ext(api_key, "http://www.nowhere.com/main", lineItems, "subscription");
        output.WriteLine(session);
    }

    [Fact]
    public void CreateCheckoutSession_Ext_test2()
    {
        var se = new Stripe_Ext();
        var lineItems = new List<psn.PH.Structures.SessionLineItem>
            {
                new psn.PH.Structures.SessionLineItem
                {
                price_id = "price_1NbbkuCrPSPWXnSudFVjHd80",
                quantity = 5,
                },
            };
        var session = se.CreateCheckoutSession_Ext(api_key, "http://www.nowhere.com/main", lineItems, "payment");
        output.WriteLine(session);
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