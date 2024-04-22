using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using psn.PH.Structures;
using Stripe;

namespace psn.PH
{
    public class Stripe_Ext : IStripe_Ext
    {
        private bool isValidateEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
#pragma warning disable CS0168
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
#pragma warning disable CS0168
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    // @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    // Take the regex from https://www.w3resource.com/javascript/form/email-validation.php 
                    @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public string CreateCustomer_Ext(string api_key, string name, string email, string phone)
        {
            if (!isValidateEmailAddress(email))
            {
                throw new ArgumentException("Invalid email format supplied as argument");
            }
            StripeConfiguration.ApiKey = api_key;

            var ccoptions = new CustomerCreateOptions
            {
                Email = email,
                Name = name,
                Phone = phone,
            };
            var cservice = new CustomerService();
            var serviceResult = cservice.Create(ccoptions);
            var customerId = serviceResult.Id;
            return customerId;
        }

        public string CreateCustomerWithCardAssociation_Ext(string api_key, string name, string email, string phone, psn.PH.Structures.PaymentMethodCardOptions cardOptions)
            {
            StripeConfiguration.ApiKey = api_key;
            var customerId = CreateCustomer_Ext(api_key, name, email, phone);
            if (cardOptions.Token != string.Empty)
            {
                var cardTokenOptions = new CardCreateOptions
                {
                    Source = cardOptions.Token,
                };
            // associate the card created to the customer as the source of payment
            var _cardService = new CardService();
            _cardService.Create(customerId, cardTokenOptions);
            }
            return customerId;
        }

        public string AddCreditCard_Ext(string api_key, string customerId, psn.PH.Structures.PaymentMethodCardOptions cardOptions)
        {
            StripeConfiguration.ApiKey = api_key;
            if (cardOptions.Token != string.Empty)
            {
                var cardTokenOptions = new CardCreateOptions
                {
                    Source = cardOptions.Token,
                };
                // associate the card created to the customer as the source of payment
                var _cardService = new CardService();
                var _card = _cardService.Create(customerId, cardTokenOptions);
                return _card.Id;
            }
            return string.Empty;
        }

        public string UpdateCustomer_Ext(string api_key, string customerId, psn.PH.Structures.CustomerUpdateOptions options)
        {
            var cuo = new Stripe.CustomerUpdateOptions
            {
                Name = options.Name, // mandatory field
                Email = options.Email, // mandatory field
                Phone = options.Phone, // mandatory field
            };
            if (options.Address.City != null && options.Address.City.Trim() != string.Empty) // non-mandatory field
            {
                cuo.Address = new Stripe.AddressOptions
                {
                    City = options.Address.City.Trim(),
                    Country = options.Address.Country.Trim(),
                    Line1 = options.Address.Line1.Trim(),
                    Line2 = options.Address.Line2.Trim(),
                    PostalCode = options.Address.PostalCode.Trim(),
                };
            }
            if (options.Shipping.Name != null && options.Shipping.Name.Trim() != string.Empty) // non-mandatory field
            {
                cuo.Shipping = new Stripe.ShippingOptions
                {
                    Name = options.Shipping.Name.Trim(),
                    Phone = options.Shipping.Phone.Trim(),
                    Address = new Stripe.AddressOptions
                    {
                        City = options.Shipping.Address.City.Trim(),
                        Country = options.Shipping.Address.Country.Trim(),
                        Line1 = options.Shipping.Address.Line1.Trim(),
                        Line2 = options.Shipping.Address.Line2.Trim(),
                        PostalCode = options.Shipping.Address.PostalCode.Trim(),
                    },
                };

            }
            var service = new CustomerService();
            var result = service.Update(customerId, cuo);
            return result.Id;
        }

        public string SearchCustomer_Ext(string api_key, string email)
        {
            StripeConfiguration.ApiKey = api_key;

            var options = new CustomerSearchOptions
            {
                Query = "email:'" + email + "'",
            };
            var service = new CustomerService();
            StripeSearchResult<Customer> searchResult = service.Search(options);
            if (searchResult.TotalCount == 0)
            {
                return string.Empty;
            }
            Customer customer = searchResult.First();
            return customer.Id;
        }

        public Intent CreatePaymentIntent_Ext(string api_key, int amount, string currency, bool automatic_payment_method, string customer_id)
        {
            // https://stripe.com/docs/api/payment_intents?lang=dotnet
            StripeConfiguration.ApiKey = api_key;

            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = automatic_payment_method,
                },
                Customer = customer_id,

            };
            var service = new PaymentIntentService();
            var serviceResult = service.Create(options);
            Intent result = new()
            {
                id = serviceResult.Id,
                client_secret = serviceResult.ClientSecret,
                status = serviceResult.Status,
                amount = serviceResult.Amount,
                currency = serviceResult.Currency,
                confirmation_method = serviceResult.ConfirmationMethod,
                created = serviceResult.Created,
                obj = serviceResult.Object,
            };

            return result;
        }
        public psn.PH.Structures.Refund CreateRefund_Ext(string api_key, string charge_id, long amount, string reason)
        {
            StripeConfiguration.ApiKey = api_key;
            var options = new RefundCreateOptions
            {
                Charge = charge_id,
            };
            var service = new RefundService();
            var serviceResult = service.Create(options);
            psn.PH.Structures.Refund result = new()
            {
                id = serviceResult.Id,
                obj = serviceResult.Object,
                amount = serviceResult.Amount,
                description = serviceResult.Description,
                failure_reason = serviceResult.FailureReason,
                payment_intent_id = serviceResult.PaymentIntentId,
                reason = serviceResult.Reason,
                receipt_number = serviceResult.ReceiptNumber,
                status = serviceResult.Status,
                charge_id = serviceResult.ChargeId,
            };

            return result;
        }
        public List<psn.PH.Structures.Charge> GetCharges_Ext(string api_key, string customer_id, int limit)
        {
            StripeConfiguration.ApiKey = api_key;

            var options = new ChargeSearchOptions
            {
                Query = "customer:'" + customer_id + "'",
                Limit = limit,
            };
            var service = new ChargeService();
            StripeSearchResult<Stripe.Charge> resultList = service.Search(options);
            List<psn.PH.Structures.Charge> result = new List<psn.PH.Structures.Charge>();
            foreach (var item in resultList)
            {
                Structures.Charge charge = new()
                {
                    id = item.Id,
                    obj = item.Object,
                    amount = item.Amount,
                    payment_intent = item.PaymentIntent.Id,
                    payment_intent_status = item.PaymentIntent.Status,
                };

                result.Add(charge);
            }
            return result;
        }

        public string CreateCheckoutSession_Ext(string api_key, string customer_id, string successful_url, List<psn.PH.Structures.SessionLineItem> sessionLineItems, string mode)
        {
            StripeConfiguration.ApiKey = api_key;
            var lineItems = new List<Stripe.Checkout.SessionLineItemOptions>();
            Stripe.Checkout.SessionCreateOptions options;

            foreach (var item in sessionLineItems)
            {
                Stripe.Checkout.SessionLineItemOptions lineItem = new Stripe.Checkout.SessionLineItemOptions
                {
                    Price = item.price_id,
                    Quantity = item.quantity,
                };
                lineItems.Add(lineItem);
            }

            options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = successful_url,
                LineItems = lineItems,
                Mode = mode,
            };

            if (customer_id.Trim() != string.Empty)
            {
                // customer Id is an optional field
                options.Customer = customer_id;
            }

            var service = new Stripe.Checkout.SessionService();
            var serviceResult = service.Create(options);
            return JsonSerializer.Serialize(serviceResult);
        }

        public string CreateSubscription_Ext(string api_key, string customer_id, List<psn.PH.Structures.SubscriptionLineItem> subscriptionLineItems, List<psn.PH.Structures.SubscriptionMetadata> subscriptionMetadataItems)
        {
            StripeConfiguration.ApiKey = api_key;
            var lineItems = new List<Stripe.SubscriptionItemOptions>();
            var metadataItems = new Dictionary<string, string>();
            foreach (var item in subscriptionMetadataItems)
            {
                metadataItems.Add(item.key, item.value);
            }
            foreach (var item in subscriptionLineItems)
            {
                Stripe.SubscriptionItemOptions lineItem = new()
                {
                    Price = item.price_id,
                };
                lineItems.Add(lineItem);
            }
            var options = new SubscriptionCreateOptions
            {
                Customer = customer_id,
                Items = lineItems,
                Metadata = metadataItems,
            };
            var service = new SubscriptionService();
            var serviceResult = service.Create(options);
            return serviceResult.ToJson();
        }

        public List<string> RetrieveSubscriptions_Ext(string api_key, string customer_id)
        {
            StripeConfiguration.ApiKey = api_key;
            var options = new SubscriptionListOptions
            {
                Customer = customer_id,
            };
            var service = new SubscriptionService();
            StripeList<Subscription> subscriptions = service.List(
              options);
            List<string> result = new();
            foreach (var subscription in subscriptions)
            {
                result.Add(subscription.Id);
            }
            return result;
        }

        public List<string> RetreiveSubscriptionsWithDetails_Ext(string api_key, string customer_id)
        {
            StripeConfiguration.ApiKey = api_key;
            var options = new SubscriptionListOptions
            {
                Customer = customer_id,
            };
            var service = new SubscriptionService();
            StripeList<Subscription> subscriptions = service.List(
              options);
            List<string> result = new();
            foreach (var subscription in subscriptions)
            {
                result.Add(subscription.ToJson());
            }
            return result;
        }

        public string RetrieveSubscriptionDetails_Ext(string api_key, string customer_id, string subscriptionId)
        {
            StripeConfiguration.ApiKey = api_key;
            var options = new SubscriptionListOptions
            {
                Customer = customer_id,
            };
            var service = new SubscriptionService();
            StripeList<Subscription> subscriptions = service.List(
              options);
            foreach (var subscription in subscriptions)
            {
                if (subscription.Id == subscriptionId)
                {
                    return subscription.ToJson();
                }

            }
            return string.Empty;
        }

        private string ReadResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            if (assembly.GetManifestResourceStream(resourcePath) != null)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath)!)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

        public string GetBuildInfo_Ext()
        {
            return ReadResource("psn.PH.buildinfo.txt");
        }
    }
}
