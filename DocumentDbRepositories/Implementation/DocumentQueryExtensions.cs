using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace DocumentDbRepositories.Implementation
{
    public static class DocumentQueryExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IDocumentQuery<T> query)
        {
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<T>();
                results.AddRange(response);
            }
            return results;
        }
        public static async Task<T> FirstOrDefaultAsync<T>(this IDocumentQuery<T> query)
        {
            var response = await query.ExecuteNextAsync<T>();
            return response.FirstOrDefault();
        }
    }
}