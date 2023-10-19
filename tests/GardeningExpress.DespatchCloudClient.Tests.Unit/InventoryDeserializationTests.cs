using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace GardeningExpress.DespatchCloudClient.Tests.Unit
{
    public class InventoryDeserializationTests
    {
        [Test]
        public void Deserializes_empty_array_custom_fields()
        {
            var json = "{\"custom_fields\": []}";
            var result = JsonConvert.DeserializeObject<DTO.Inventory>(json);

            result.CustomFields.ShouldBeEmpty();
        }

        [Test]
        public void Deserializes_null_custom_fields()
        {
            var json = "{\"custom_fields\": null}";
            var result = JsonConvert.DeserializeObject<DTO.Inventory>(json);

            result.CustomFields.ShouldBeEmpty();
        }

        [Test]
        public void Deserializes_object_custom_fields_with_one_item()
        {
            var json = "{\"custom_fields\": {\"pot-size-3\": \"6 Litre\"}}";
            var result = JsonConvert.DeserializeObject<DTO.Inventory>(json);

            result.CustomFields.Count.ShouldBe(1);
            result.CustomFields.ShouldContainKeyAndValue("pot-size-3", "6 Litre");
        }

        [Test]
        public void Deserializes_object_custom_fields_with_one_item_int_value()
        {
            var json = "{\"custom_fields\": {\"pot-size-3\": 123}}";
            var result = JsonConvert.DeserializeObject<DTO.Inventory>(json);

            result.CustomFields.Count.ShouldBe(1);
            result.CustomFields.ShouldContainKey("pot-size-3");
            var value = result.CustomFields["pot-size-3"];

            // it's a long
            var i = Convert.ToInt32(value);
            i.ShouldBe(123);
        }

        [Test]
        public void Deserializes_array_custom_fields_with_one_item()
        {
            var json = "{\"custom_fields\": [{\"pot-size-3\": \"6 Litre\"}]}";
            var result = JsonConvert.DeserializeObject<DTO.Inventory>(json);

            result.CustomFields.Count.ShouldBe(1);
            result.CustomFields.ShouldContainKeyAndValue("pot-size-3", "6 Litre");
        }

        [Test]
        public void Deserializes_object_custom_fields_with_two_items()
        {
            var json = "{\"custom_fields\":{\"pot-size-3\":\"6 Litre\",\"passport-2\":\"test\"}}";

            var result = JsonConvert.DeserializeObject<DTO.Inventory>(json);

            result.CustomFields.Count.ShouldBe(2);
            result.CustomFields.ShouldContainKeyAndValue("pot-size-3", "6 Litre");
            result.CustomFields.ShouldContainKeyAndValue("passport-2", "test");
        }

        [Test]
        public void Deserializes_array_custom_fields_with_two_items()
        {
            var json = "{\"custom_fields\":[{\"pot-size-3\":\"6 Litre\"},{\"passport-2\":\"test\"}]}";
            var result = JsonConvert.DeserializeObject<DTO.Inventory>(json);

            result.CustomFields.Count.ShouldBe(2);
            result.CustomFields.ShouldContainKeyAndValue("pot-size-3", "6 Litre");
            result.CustomFields.ShouldContainKeyAndValue("passport-2", "test");
        }
    }
}