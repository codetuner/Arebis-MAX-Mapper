using System;
using System.Collections.Generic;

namespace Max.Domain.Mapping
{
    /// <summary>
    /// Mapper to map to contract types.
    /// </summary>
    public class Mapper
    {
        private List<object> _mappedSources;
        private List<object> _mappedTargets;

        public Mapper()
        {
            this._mappedSources = new List<object>();
            this._mappedTargets = new List<object>();
        }

        /// <summary>
        /// Registers a resolved source/target pair.
        /// </summary>
        public void RegisterMapping(object source, object target)
        {
            this._mappedSources.Add(source);
            this._mappedTargets.Add(target);
        }

        /// <summary>
        /// Gets the pre-registered target for a given source.
        /// Returns null if no target was yet registered for the given source.
        /// </summary>
        public object GetMappedTarget(object source)
        {
            for (int i = 0; i < this._mappedSources.Count; i++)
            {
                if (Object.ReferenceEquals(this._mappedSources[i], source))
                    return this._mappedTargets[i];
            }
            return null;
        }

        /// <summary>
        /// Gets the pre-registered target of type TTarget for a given source.
        /// Returns null if no target of that type was yet registered for the given source.
        /// </summary>
        public TTarget GetMappedTarget<TTarget>(object source)
        {
            for (int i = 0; i < this._mappedSources.Count; i++)
            {
                if (Object.ReferenceEquals(this._mappedSources[i], source) && (this._mappedTargets[i] is TTarget))
                    return (TTarget)this._mappedTargets[i];
            }
            return default(TTarget);
        }

        /// <summary>
        /// Base type mapper method.
        /// </summary>
        public virtual void UpdateSystemObject(object source, object target)
        {
        }
    }
}
