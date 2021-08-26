using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Extensions
{
    public static class Extensions
    {
        // public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage,
        //     int totalItems, int totalPages)
        // {
        //     var paginationHeaders = new PaginationHeaders(currentPage, itemsPerPage, totalItems, totalPages);
        //     var camelCaseForMatter = new JsonSerializerSettings();
        //     camelCaseForMatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //     response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeaders, camelCaseForMatter));
        // }
        // public static void AddApplicationError(this HttpResponse response, string message)
        // {
        //     response.Headers.Add("Application-Error", message);
        //     response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
        //     response.Headers.Add("Access-Control-Allow-Origin", "*");
        // }
        public static void AddPagination(this HttpResponse response,
           int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeaders(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination",
                JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

    }
}