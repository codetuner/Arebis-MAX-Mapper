using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace Max.Tools.DomainGenerator
{
    public abstract class GenerationHostTemplateBase<T> : Microsoft.VisualStudio.TextTemplating.TextTransformation
    {
        private GenerationHost<T> host;

        public GenerationHost<T> Host
        {
            get
            {
                if (host == null)
                {
                    host = (GenerationHost<T>)CallContext.GetData("GenerationHost");
                }
                return host;
            }
        }

        public T Model
        {
            get
            {
                return this.Host.Model;
            }
        }

        public string LocalNamespace
        {
            get
            {
                return this.Host.LocalNamespace;
            }
        }
    }
}
