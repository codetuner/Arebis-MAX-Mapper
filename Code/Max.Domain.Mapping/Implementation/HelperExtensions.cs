using System;

namespace Max.Domain.Mapping.Implementation
{
    /// <summary>
    /// Extension methods that can be used in (generated) mapper code.
    /// </summary>
    public static class HelperExtensions
    {
        public static bool IsTrue<T>(this T subject, Predicate<T> predicate)
        {
            return predicate(subject);
        }

        public static bool IsFalse<T>(this T subject, Predicate<T> predicate)
        {
            return !predicate(subject);
        }
    }
}
