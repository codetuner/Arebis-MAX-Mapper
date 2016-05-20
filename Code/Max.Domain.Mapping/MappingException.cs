using System;
using System.Runtime.Serialization;
using System.Security;

namespace Max.Domain.Mapping
{
    /// <summary>
    /// An exception in mapping objects.
    /// </summary>
    [Serializable]
    public class MappingException : InvalidCastException
    {
        public MappingException()
            : base()
        { }

        public MappingException(string message)
            : base(message)
        { }

        public MappingException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected MappingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
