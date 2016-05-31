using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Max.Tools.DomainGenerator.Model
{
    [Serializable]
    public class Namespace
    {
        public Namespace()
        { }

        public Namespace(string name)
            : this(name, null)
        { }

        public Namespace(string name, string alias)
        {
            this.Alias = alias;
            this.Name = name;
        }

        public bool HasAlias 
        { 
            get { return this.Alias != null; } 
        }

        public string Alias { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Namespace;
            if (other == null)
                return false;
            else
                return ((this.Alias ?? String.Empty) + (this.Name ?? String.Empty)).Equals(((other.Alias ?? String.Empty) + (other.Name ?? String.Empty)));
        }

        public override int GetHashCode()
        {
            return ((this.Alias ?? String.Empty).GetHashCode() ^ (this.Name ?? String.Empty).GetHashCode());
        }

        public bool IsPredefined 
        { 
            get { return (new String[] { "Contract", "Mapping" }).Contains(this.Alias); } 
        }
    }
}
