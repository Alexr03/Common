using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace Alexr03.Common.Web.Extensions
{
    public static class DataSourceRequestExtensions
    {
        public static List<FilterDescriptor> GetAllFilterDescriptors(this DataSourceRequest request)
        {
            if (request == null)
            {
                return new List<FilterDescriptor>();
            }
            var allFilterDescriptors = new List<FilterDescriptor>();
            RecurseFilterDescriptors(request.Filters, allFilterDescriptors);
            return allFilterDescriptors;
        }

        private static void RecurseFilterDescriptors(IEnumerable<IFilterDescriptor> requestFilters, ICollection<FilterDescriptor> allFilterDescriptors)
        {
            foreach (var filterDescriptor in requestFilters)
            {
                if (filterDescriptor is FilterDescriptor descriptor)
                {
                    allFilterDescriptors.Add(descriptor);
                }
                else if (filterDescriptor is CompositeFilterDescriptor compositeFilterDescriptor)
                {
                    RecurseFilterDescriptors(compositeFilterDescriptor.FilterDescriptors, allFilterDescriptors);
                }
            }
        }
    }
}