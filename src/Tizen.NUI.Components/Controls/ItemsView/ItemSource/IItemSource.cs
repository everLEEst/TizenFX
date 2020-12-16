using System;

namespace Tizen.NUI.Components
{
	/// <summary>
    /// Base interface for encapsulated data source in ItemsView.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
	public interface IItemSource : IDisposable
	{
	    /// <summary>
        /// Count of data source.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		int Count { get; }

	    /// <summary>
        /// Position integer value of data object.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		int GetPosition(object item);

	    /// <summary>
        /// Item object in position.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		object GetItem(int position);

	    /// <summary>
        /// Flag of header existence.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		bool HasHeader { get; set; }

	    /// <summary>
        /// Flag of Footer existence.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		bool HasFooter { get; set; }

	    /// <summary>
        /// Boolean checker for position is header or not.
		/// 0 index will be header if header exist.
		/// warning: if header exist, all item index will be increased.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		bool IsHeader(int position);

	    /// <summary>
        /// Boolean checker for position is footer or not.
		/// last index will be footer if footer exist.
		/// warning: footer will be place original data count or data count + 1.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		bool IsFooter(int position);
	}

	/// <summary>
    /// Base interface for encapsulated data source with group structure in CollectionView.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
	public interface IGroupableItemSource : IItemSource
	{
	    /// <summary>
        /// Boolean checker for position is group header or not
		/// </summary>
        /// <since_tizen> 8 </since_tizen>
		bool IsGroupHeader(int position);

	    /// <summary>
        /// Boolean checker for position is group footer or not
        /// </summary>
		/// <since_tizen> 8 </since_tizen>
		bool IsGroupFooter(int position);
	}
}
