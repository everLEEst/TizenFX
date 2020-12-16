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
		MeasureAll, /// Measure all items in advanced way. Estimate first item size for all, and when scroll reached position, measure strictly. Note : This will make scroll bar trembling.
		MeasureFirst /// Measure all items as same as first item. if template is selector, the size of first item from each template will be deligated.
        /// MeasureAllItemsStrict could be necessary
	}
}
