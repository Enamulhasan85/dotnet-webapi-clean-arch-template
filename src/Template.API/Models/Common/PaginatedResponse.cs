using System.Collections.Generic;

namespace Template.API.Models.Common
{
    /// <summary>
    /// Response model for paginated data
    /// </summary>
    /// <typeparam name="T">The type of data being paginated</typeparam>
    public class PaginatedResponse<T>
    {
        /// <summary>
        /// List of items for the current page
        /// </summary>
        public IEnumerable<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of items across all pages
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Whether there is a previous page
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Whether there is a next page
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PaginatedResponse()
        {
        }

        /// <summary>
        /// Constructor with all required properties
        /// </summary>
        /// <param name="data">The paginated data</param>
        /// <param name="page">Current page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="totalCount">Total items count</param>
        public PaginatedResponse(IEnumerable<T> data, int page, int pageSize, int totalCount)
        {
            Data = data;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);
        }
    }
}
