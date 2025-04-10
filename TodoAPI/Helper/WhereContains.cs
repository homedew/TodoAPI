using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoAPI.Helper
{
    public static class WhereContainsQueryable
    {
        public static IQueryable<T> WhereContains<T>(this IQueryable<T> source, string searchText, params string[] fieldsToSearch)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? predicate = null;

            foreach (var field in fieldsToSearch)
            {
                var property = Expression.Property(parameter, field);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                var search = Expression.Constant(searchText, typeof(string));
                var call = Expression.Call(property, containsMethod, search);

                predicate = predicate == null ? call : Expression.OrElse(predicate, call);
            }

            if (predicate == null) return source;

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
            return source.Where(lambda);
        }

    }
}