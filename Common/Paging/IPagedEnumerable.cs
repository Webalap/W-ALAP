using System;
using System.Collections.Generic;


namespace Common.Paging
{

    public interface IPagedEnumerable : IEquatable<IPagedEnumerable> {

        int             PageSize            { get; }
        int             PageNumber          { get; }
        int             RecordCount         { get; }
        int             TotalRecordCount    { get; }

        PagingResult    PagingResult        { get; }

    }




    public interface IPagedEnumerable<out TRecord> : IPagedEnumerable, IEnumerable<TRecord> {}

}
