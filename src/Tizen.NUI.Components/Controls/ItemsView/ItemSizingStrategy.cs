using System.ComponentModel;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// [Draft] Size calculation strategy for CollectionView.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
    [EditorBrowsable(EditorBrowsableState.Never)]
	public enum ItemSizingStrategy
	{

        /// <summary>
        /// Measure all items in advanced.
        /// Estimate first item size for all, and when scroll reached position,
        /// measure strictly. Note : This will make scroll bar trembling.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		MeasureAll,
		
        /// <summary>
        /// Measure first item and deligate size for all items.
        /// if template is selector, the size of first item from each template will be deligated.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
        MeasureFirst,
	}
}
