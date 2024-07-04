using System;
using System.Linq;
using System.Linq.Expressions;

namespace NMS.Assistant.Persistence.Helper
{
    public static class FilterHelper
    {
        /// <summary>
        /// Conditionally adds a Where expression IF the property has a non-zero value
        /// </summary>
        /// <param name="query">An existing IQueryable<Vehicle> query.</param>
        /// <param name="property">The property to be checked.</param>
        /// <param name="predicate">The predicate to be used within the .Where() method.</param>
        /// <returns></returns>
        public static IQueryable<T> AddFilterIfValue<T>(this IQueryable<T> query, int property, Expression<Func<T, bool>> predicate)
        {
            if (property > 0)
            {
                return query.Where(predicate);
            }

            return query;
        }

        /// <summary>
        /// Conditionally adds a Where expression IF the property has a non-empty value
        /// </summary>
        /// <param name="query">An existing IQueryable<Vehicle> query.</param>
        /// <param name="property">The property to be checked.</param>
        /// <param name="predicate">The predicate to be used within the .Where() method.</param>
        /// <returns></returns>
        public static IQueryable<T> AddFilterIfValue<T>(this IQueryable<T> query, string property, Expression<Func<T, bool>> predicate)
        {
            if (!string.IsNullOrWhiteSpace(property))
            {
                return query.Where(predicate);
            }

            return query;
        }

        /// <summary>
        /// Conditionally adds a Where expression IF the property has a non-empty value
        /// </summary>
        /// <param name="query">An existing IQueryable<Vehicle> query.</param>
        /// <param name="isEnabled">The property to be checked.</param>
        /// <param name="predicate">The predicate to be used within the .Where() method.</param>
        /// <returns></returns>
        public static IQueryable<T> AddFilterIfValue<T>(this IQueryable<T> query, bool isEnabled, Expression<Func<T, bool>> predicate)
        {
            if (isEnabled)
            {
                return query.Where(predicate);
            }

            return query;
        }
    }
}
