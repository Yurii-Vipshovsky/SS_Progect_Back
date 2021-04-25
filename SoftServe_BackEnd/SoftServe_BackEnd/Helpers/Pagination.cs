using System;
using System.Collections.Generic;
using SoftServe_BackEnd.Services;

namespace SoftServe_BackEnd.Helpers
{
    public class Pagination
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData,
            PaginationFilter filter, int totalRecords)
        {
            var response = new PagedResponse<List<T>>(pagedData, filter.PageNumber, filter.PageSize);
            var totalPages = totalRecords / (double) filter.PageSize;
            var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.TotalPages = roundedTotalPages;
            response.PageSize = filter.PageSize == new PaginationFilter().PageSize ? totalRecords : filter.PageSize;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}