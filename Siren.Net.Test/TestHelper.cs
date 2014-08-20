namespace Siren.Net.Test
{
    using System.Linq;
    using WebApiContrib.Formatting.Siren.Client;
    using Xunit;

    internal static class TestHelper
    {
        public const string SPEC_SAMPLE_JSON =
            @"{
                ""class"": [ ""order"" ],
                ""properties"": { 
                    ""orderNumber"": 42, 
                    ""itemCount"": 3,
                    ""status"": ""pending""
                },
                ""entities"": [
                    { 
                        ""class"": [ ""items"", ""collection"" ], 
                        ""rel"": [ ""http://x.io/rels/order-items"" ], 
                        ""href"": ""http://api.x.io/orders/42/items""
                    },
                    {
                        ""class"": [ ""info"", ""customer"" ],
                        ""rel"": [ ""http://x.io/rels/customer"" ], 
                        ""properties"": { 
                            ""customerId"": ""pj123"",
                            ""name"": ""Peter Joseph""
                        },
                        ""links"": [
                            { ""rel"": [ ""self"" ], ""href"": ""http://api.x.io/customers/pj123"", ""title"": ""CustomerUri"" }
                        ]
                    }
                ],
                ""actions"": [
                    {
                        ""name"": ""add-item"",
                        ""title"": ""Add Item"",
                        ""method"": ""POST"",
                        ""href"": ""http://api.x.io/orders/42/items"",
                        ""type"": ""application/x-www-form-urlencoded"",
                        ""fields"": [
                            { ""name"": ""orderNumber"", ""type"": ""hidden"", ""value"": ""42"" },
                            { ""name"": ""productCode"", ""type"": ""text"" },
                            { ""name"": ""quantity"", ""type"": ""number"" }
                        ]
                    }
                ],
                ""links"": [
                    { ""rel"": [ ""self"" ], ""href"": ""http://api.x.io/orders/42"" },
                    { ""rel"": [ ""previous"" ], ""href"": ""http://api.x.io/orders/41"" },
                    { ""rel"": [ ""next"" ], ""href"": ""http://api.x.io/orders/43"" }
                ]
                }";

        /// <summary>
        /// Validates that the structure of <paramref name="entity"/> matches that of the spec sample JSON.
        /// </summary>
        /// <param name="entity">The Siren Entity to compare to the spec sample.</param>
        public static void AssertIsSpecSample(ISirenEntity entity)
        {
            Assert.Equal("order", entity.Classes.Single());

            var properties = entity.Properties;
            Assert.Equal(3, properties.Count);
            Assert.Equal("42", properties["orderNumber"].ToString());
            Assert.Equal("3", properties["itemCount"].ToString());
            Assert.Equal("pending", properties["status"].ToString());

            Assert.Equal(2, entity.Entities.Count);

            var link = entity.Entities.Single(x => x.IsLink()).AsLink();
            Assert.NotNull(link);
            Assert.Equal(2, link.Classes.Count);
            Assert.Contains("items", link.Classes);
            Assert.Contains("collection", link.Classes);
            Assert.Equal("http://x.io/rels/order-items", link.Rel.Single());
            Assert.Equal("http://api.x.io/orders/42/items", link.Href);

            var rep = entity.Entities.Single(x => x.IsRepresentation()).AsRepresentation();
            Assert.NotNull(rep);
            Assert.Equal(2, rep.Classes.Count);
            Assert.Contains("info", rep.Classes);
            Assert.Contains("customer", rep.Classes);
            Assert.Equal("http://x.io/rels/customer", rep.Rel.Single());
            var repProperties = rep.Properties;
            Assert.Equal(2, repProperties.Count);
            Assert.Equal("pj123", repProperties["customerId"].ToString());
            Assert.Equal("Peter Joseph", repProperties["name"].ToString());
            var repLink = rep.Links.Single();
            Assert.Equal("self", repLink.Rel.Single());
            Assert.Equal("http://api.x.io/customers/pj123", repLink.Href);
            Assert.Equal("CustomerUri", repLink.Title);

            var action = entity.Actions.Single();
            Assert.Equal("add-item", action.Name);
            Assert.Equal("Add Item", action.Title);
            Assert.Equal("POST", action.Method.Method);
            Assert.Equal("http://api.x.io/orders/42/items", action.Href);
            Assert.Equal("application/x-www-form-urlencoded", action.Type.MediaType);

            var fields = action.Fields;
            Assert.Equal(3, fields.Count);
            var orderNumber = fields.Single(x => x.Name == "orderNumber");
            Assert.Equal(FieldType.Hidden, orderNumber.Type);
            Assert.Equal("42", orderNumber.Value);
            var productType = fields.Single(x => x.Name == "productCode");
            Assert.Equal(FieldType.Text, productType.Type);
            var quantity = fields.Single(x => x.Name == "quantity");
            Assert.Equal(FieldType.Number, quantity.Type);

            var links = entity.Links;
            Assert.Equal(3, links.Count);
            var self = links.Single(x => x.Rel.Single() == "self");
            Assert.Equal("http://api.x.io/orders/42", self.Href);
            var prev = links.Single(x => x.Rel.Single() == "previous");
            Assert.Equal("http://api.x.io/orders/41", prev.Href);
            var next = links.Single(x => x.Rel.Single() == "next");
            Assert.Equal("http://api.x.io/orders/43", next.Href);
        }
    }
}
