using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IDataRequest
    {
        int Page { get; set; }
        int RowCount { get; set; }
        int TotalRowCount { get; set; }
    }
}