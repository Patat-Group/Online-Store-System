using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Extensions
{
    public static class Extensions
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage,
            int totalItems, int totalPages)
        {
            var paginationHeaders = new PaginationHeaders(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseForMatter = new JsonSerializerSettings();
            camelCaseForMatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeaders,camelCaseForMatter));
        }
    }   
}