using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GardeningExpress.DespatchCloudClient.Tests.Integration
{
    public class TestUtils
    {

        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null || propertyName == null)
            {
                throw new ArgumentNullException(nameof(obj) + " or " + nameof(propertyName) + " or " + nameof(value));
            }

            // Get the type of the object
            Type type = obj.GetType();

            // Find the property by name
            PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{type.Name}'.");
            }

            // Check if the property is writable
            if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException($"Property '{propertyName}' on type '{type.Name}' is read-only.");
            }

            // Set the property value
            propertyInfo.SetValue(obj, value);
        }

    }
}
