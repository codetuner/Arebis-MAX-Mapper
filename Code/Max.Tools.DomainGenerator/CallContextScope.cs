using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace Max.Tools.DomainGenerator
{
    public class CallContextScope : IDisposable
    {
        private string name;
        private object previousData;

        public CallContextScope(string name, object data)
        {
            this.name = name;
            this.previousData = CallContext.GetData(name);
            CallContext.SetData(name, data);
        }

        public void Dispose()
        {
            CallContext.SetData(name, previousData);
        }
    }
}
