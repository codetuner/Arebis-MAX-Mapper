using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Max.Tools.DomainGenerator.Model
{
    public class CodeAttributeValuesDictionary : Dictionary<string, string>
    {
        public CodeAttributeValuesDictionary(EnvDTE.CodeAttribute codeAttribute)
        {
            foreach (var valuePair in codeAttribute.Value.Split(','))
            {
                var value = valuePair.Split(new char[] { '=' }, 2);
                if (value.Length == 1)
                {
                    this[this.Keys.Count.ToString()] = value[0].Trim().Replace("\"", "");
                }
                else if (value.Length == 2)
                {
                    this[value[0].Trim()] = value[1].Trim().Replace("\"", "");
                }
            }
        }
    }
}
