using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpManagement.Models
{
    public class CriteriaModel
    {
        public int PageSize { get; set; }

        public int CurrPage { get; set; }

        public string SortField { get; set; }

        public string SortDirection { get; set; }

        public string SearchString { get; set; }
    }
}