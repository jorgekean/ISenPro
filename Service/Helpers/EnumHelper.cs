using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieves the description attribute of an enum value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The description of the enum value, or the enum name if no description is set.</returns>
        public static string GetDescription<T>(T enumValue) where T : Enum
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? enumValue.ToString();
        }

        /// <summary>
        /// Retrieves a list of all enum values and their descriptions.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>A dictionary with enum values as keys and descriptions as values.</returns>
        public static Dictionary<int, string> GetAllEnumDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .ToDictionary(e => Convert.ToInt32(e), e => GetDescription(e));
        }

        /// <summary>
        /// Finds an enum value by its integer value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The integer value of the enum.</param>
        /// <returns>The corresponding enum value, or null if not found.</returns>
        public static T? GetEnumByValue<T>(int value) where T : struct, Enum
        {
            return Enum.IsDefined(typeof(T), value) ? (T)Enum.ToObject(typeof(T), value) : null;
        }

        /// <summary>
        /// Finds an enum value by its description.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="description">The description of the enum.</param>
        /// <returns>The corresponding enum value, or null if not found.</returns>
        public static T? GetEnumByDescription<T>(string description) where T : struct, Enum
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if (string.Equals(GetDescription(value), description, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves a list of enums with their values and descriptions.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>A list of objects with Value and Description fields.</returns>
        public static List<(int Value, string Description)> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => (Value: Convert.ToInt32(e), Description: GetDescription(e)))
                       .ToList();
        }

    }
}
