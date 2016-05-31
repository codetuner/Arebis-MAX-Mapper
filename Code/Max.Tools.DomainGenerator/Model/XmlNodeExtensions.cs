using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Max.Tools.DomainGenerator.Model
{
    static class XmlNodeExtensions
    {
        public static string ValueOr(this XmlNode subject, string alternative)
        {
            return (subject != null)
                ? subject.Value
                : alternative;
        }
    }
}
