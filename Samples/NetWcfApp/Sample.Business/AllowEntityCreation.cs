using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sample.Business
{
    public static class AllowEntityCreation
    {
        static AllowEntityCreation() {
            Value = true;
        }

        public static bool Value { get; set; }

        public static void AssertEntityCreationAllowed()
        {
            if (!AllowEntityCreation.Value)
                throw new InvalidOperationException("Entity creation not allowed.");
        }
    }

    partial class Contact
    {
        public Contact()
        {
            AllowEntityCreation.AssertEntityCreationAllowed();
        }
    }

    partial class Employee
    {
        public Employee()
        {
            AllowEntityCreation.AssertEntityCreationAllowed();
        }
    }

    partial class SalesTerritory
    {
        public SalesTerritory()
        {
            AllowEntityCreation.AssertEntityCreationAllowed();
        }
    }
}
