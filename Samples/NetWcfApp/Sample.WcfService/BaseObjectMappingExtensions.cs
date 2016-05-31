using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Max.Domain.Mapping;

namespace Sample.WcfService
{
    public static class BaseObjectMappingExtensions
    {
        public static void UpdateBaseObject(this Mapper mapper, object source, object target)
        {
            mapper.UpdateSystemObject(source, target);
        }

        public static void UpdateBaseObject(this ReverseMapper mapper, object source, object target)
        {
            mapper.UpdateSystemObject(source, target);
        }
    }
}