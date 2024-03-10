using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNet_Mvc_Crud_Template.Models.VM
{
    public class IndexVM<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        // Parametresiz yapılandırıcı
        public IndexVM()
        {
            // Items özelliğini burada başlatın
            Items = Enumerable.Empty<T>();
        }

        // Parametreli yapılandırıcı
        public IndexVM(IEnumerable<T>? items, int totalCount, int currentPage, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
