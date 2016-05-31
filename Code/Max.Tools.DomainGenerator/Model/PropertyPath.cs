using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Max.Tools.DomainGenerator.Model
{
    [Serializable]
    public class PropertyPath
    {
        private static Regex PropertyPathParser = new Regex(@"((^|\.)(?<pathItem>\w*?)(\s*\(\s*as\s+(?<cast>\w*?)\s*\)\s*)?(\[(?<condition>(.*?))\])?)+$");

        private List<string> propertyNamePath = new List<string>();
        private List<string> propertyTypePath = new List<string>();
        private List<int> propertyNamePathIndexes = new List<int>();
        private List<string> conditions = new List<string>();
        private List<string> casts = new List<string>();
        private bool isCollection = false;
        private string propertyType;
        private bool canWrite;

        public PropertyPath()
        { }

        public PropertyPath(MapProperty owner, string ownerType, string path)
        {
            this.Owner = owner;

            Match match = PropertyPathParser.Match(path);

            this.propertyNamePath = new List<string>();
            string lastOwner = ownerType;
            foreach (Capture item in match.Groups["pathItem"].Captures)
            {
                string propType = Session.TypeManager.GetPropertyType(lastOwner, item.Value);
                this.propertyNamePath.Add(item.Value);
                this.propertyTypePath.Add(propType);
                this.propertyNamePathIndexes.Add(item.Index);
                this.canWrite = Session.TypeManager.HasSetter(lastOwner, item.Value);
                lastOwner = propType;
                bool iscollection = Session.TypeManager.IsCollectionType(propType);
                if (iscollection)
                    lastOwner = Session.TypeManager.GetCollectionItemType(lastOwner);
                this.isCollection = (this.isCollection || iscollection);
            }
            this.propertyType = lastOwner;

            // Handle conditions:
            this.conditions = new List<string>();
            foreach (var item in this.propertyNamePath) this.conditions.Add(null);
            foreach (Capture item in match.Groups["condition"].Captures)
            {
                int listpos = -1;
                for (int i = 0; i < this.propertyNamePathIndexes.Count; i++)
                    if (this.propertyNamePathIndexes[i] < item.Index) listpos++;

                this.conditions[listpos] = item.Value;
                this.HasConditions = true;
            }

            // Handle casts:
            this.casts = new List<string>();
            foreach (var item in this.propertyNamePath) this.casts.Add(null);
            foreach (Capture item in match.Groups["cast"].Captures)
            {
                int listpos = 0;
                for (int i = 0; i < this.propertyNamePathIndexes.Count; i++)
                    if (item.Index < this.propertyNamePathIndexes[i]) listpos++;

                this.casts[listpos] = item.Value;
            }
        }

        public MapProperty Owner { get; private set; }

        public DomainGeneratorSession Session
        {
            get { return this.Owner.Session; }
        }

        public int PropertyPathSize
        {
            get { return this.propertyNamePath.Count; }
        }

        public PropertyPath WithoutConditions
        {
            get
            {
                PropertyPath result = (PropertyPath)this.MemberwiseClone();
                result.conditions = new List<string>();
                foreach (var item in result.propertyNamePath) result.conditions.Add(null);
                return result;
            }
        }

        public List<String> PropertyNamePath
        {
            get { return this.propertyNamePath; }
        }

        public bool HasConditions { get; private set; }

        public List<string> Conditions 
        {
            get { return this.conditions; }
        }

        public bool IsPathItemCollection(int pathItemIndex)
        {
            return Session.TypeManager.IsCollectionType(this.propertyTypePath[pathItemIndex]);
        }

        public bool IsSimpleProperty
        {
            get { return (this.propertyNamePath.Count == 1); }
        }

        public bool IsCollection
        {
            get { return this.isCollection; }
        }

        public string PropertyType
        {
            get { return this.propertyType; }
        }

        public bool CanWrite
        {
            get { return this.canWrite; }
        }

        private static bool GetIsCollection(PropertyInfo propertyInfo)
        {
            return
                propertyInfo.PropertyType.IsGenericType
                && typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyInfo.PropertyType);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string sep = String.Empty;
            for (int i = 0; i < this.PropertyNamePath.Count; i++)
            {
                sb.Append(sep);
                sb.Append(this.PropertyNamePath[i]);
                if (this.Conditions[i] != null)
                {
                    sb.Append(".Where(");
                    sb.Append(this.Conditions[i]);
                    sb.Append(")");
                }
                sep = ".";
            }

            return sb.ToString();
        }
    }
}
