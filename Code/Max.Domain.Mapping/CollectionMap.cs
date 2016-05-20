using System;
using System.Collections.Generic;

namespace Max.Domain.Mapping
{
    /// <summary>
    /// Helper class to map collections.
    /// Use it's static Build() method to build a new generic CollectionMap instance.
    /// </summary>
    public abstract class CollectionMap
    {
        /// <summary>
        /// Builds a new generic CollectionMap instance allowing infering of the
        /// source and target item types.
        /// </summary>
        /// <typeparam name="TSourceItem">Type of source collection members.</typeparam>
        /// <typeparam name="TTargetItem">Type of target collection members.</typeparam>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="targetCollection">The target collection.</param>
        /// <param name="comparisonPredicate">The expression to match items from both collections.</param>
        /// <returns>A generic CollectionMap instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static CollectionMap<TSourceItem, TTargetItem> Build<TSourceItem, TTargetItem>(
            IEnumerable<TSourceItem> sourceCollection,
            IEnumerable<TTargetItem> targetCollection,
            Predicate<KeyValuePair<TSourceItem, TTargetItem>> comparisonPredicate)
        {
            return new CollectionMap<TSourceItem, TTargetItem>(
                sourceCollection,
                targetCollection,
                comparisonPredicate);
        }
    }

    /// <summary>
    /// Helper class to map collections.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of source collection members.</typeparam>
    /// <typeparam name="TTargetItem">Type of target collection members.</typeparam>
    public class CollectionMap<TSourceItem, TTargetItem> : CollectionMap
    {
        private Dictionary<TSourceItem, TTargetItem> matchingItems
            = new Dictionary<TSourceItem,TTargetItem>();
        private List<TSourceItem> itemsToAdd = new List<TSourceItem>();
        private List<TTargetItem> itemsToRemove = new List<TTargetItem>();

        /// <summary>
        /// Constructs a new CollectionMap.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="targetCollection">The target collection.</param>
        /// <param name="comparisonPredicate">The expression to match items from both collections.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public CollectionMap(
            IEnumerable<TSourceItem> sourceCollection,
            IEnumerable<TTargetItem> targetCollection,
            Predicate<KeyValuePair<TSourceItem, TTargetItem>> comparisonPredicate)
        {
            foreach (TTargetItem titem in targetCollection)
            {
                bool found = false;
                foreach (TSourceItem sitem in sourceCollection)
                {
                    KeyValuePair<TSourceItem, TTargetItem> pair 
                        = new KeyValuePair<TSourceItem, TTargetItem>(sitem, titem);
                    if (comparisonPredicate(pair))
                    {
                        matchingItems.Add(sitem, titem);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    this.itemsToRemove.Add(titem);
                }
            }
            foreach (TSourceItem sitem in sourceCollection)
            {
                if (!this.matchingItems.ContainsKey(sitem))
                    this.itemsToAdd.Add(sitem);
            }
        }

        /// <summary>
        /// Items to be added on the target collection to match the source collection.
        /// </summary>
        public IEnumerable<TSourceItem> SourcesToAdd
        {
            get { return this.itemsToAdd; }
        }

        /// <summary>
        /// Items to be removed from the target collection to match the source collection.
        /// </summary>
        public IEnumerable<TTargetItem> TargetsToRemove
        {
            get { return this.itemsToRemove; }
        }

        /// <summary>
        /// Source collection items that remain in the target collection.
        /// </summary>
        public IEnumerable<TSourceItem> SourcesToKeep
        {
            get { return this.matchingItems.Keys; }
        }

        /// <summary>
        /// Target collection items that exist in the source collection.
        /// </summary>
        public IEnumerable<TTargetItem> TargetsToKeep
        {
            get { return this.matchingItems.Values; }
        }

        /// <summary>
        /// The target collection item for a given source collection item.
        /// </summary>
        public TTargetItem TargetFor(TSourceItem sourceItem)
        {
            return this.matchingItems[sourceItem];
        }
    }
}
