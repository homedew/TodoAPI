using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
namespace TodoApp.Application.Helper
{

    public static class IQueryableExtensions
    {
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
            this IQueryable<T> query, int page, int pageSize, string? sortBy = null, string? sortOrder = "asc", IEnumerable<string>? allowedSortFields = null, CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortFields = sortBy.Split(',');
                var sortDirections = (sortOrder ?? "").Split(',');
                var orderClauses = sortFields.Select((field, index) =>
                {
                    var direction = index < sortDirections.Length && sortDirections[index].ToLower() == "desc" ? "descending" : "ascending";

                    if (allowedSortFields != null && !allowedSortFields.Contains(field, StringComparer.OrdinalIgnoreCase))
                        throw new ArgumentException($"Sorting by '{field}' is not allowed.");

                    return $"{field} {direction}";
                });

                var finalOrder = string.Join(", ", orderClauses);
                query = query.OrderBy(finalOrder);
            }
            var data = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync(cancellationToken);


            return new PagedResponse<T>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = data
            };
        }

    }
}