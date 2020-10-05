using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace Alexr03.Common.Web.Extensions
{
    public static class DataSourceRequestExtensions
    {
        public static List<FilterDescriptor> GetFilters(this DataSourceRequest dataSourceRequest)
        {
            return dataSourceRequest.Filters.Cast<FilterDescriptor>().ToList();
        }
        
        public static List<GroupDescriptor> GetGroups(this DataSourceRequest dataSourceRequest)
        {
            return dataSourceRequest.Groups.ToList();
        }
    }
}