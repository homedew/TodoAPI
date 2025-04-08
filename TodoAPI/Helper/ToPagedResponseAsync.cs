using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Helper
{

    public static class IQueryableExtensions
    {
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
            this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await query.CountAsync(cancellationToken);
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