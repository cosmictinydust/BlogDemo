using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Core.Entities
{
    public class PaginatedList<T>:List<T> where T:class
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        private int _totalItemCount;
        public int TotalItemCount
        {
            get => _totalItemCount;
            set => _totalItemCount = value > 0 ? value : 0;
        }

        public int PageCount => TotalItemCount / PageSize + (TotalItemCount % PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PageIndex > 0;
        public bool HasNext => PageIndex < PageCount - 1;

        public PaginatedList(int pageIndex,int pageSize,int totalItemCount,IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            AddRange(data);
        }
    }
}
