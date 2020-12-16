namespace Tizen.NUI.Components
{
    /// <summary>
    /// [Draft] Selection mode of CollecitonView.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API
	public enum ItemSelectionMode
	{
		/// <summary>
        /// Single selection. select item exculsively so previous selected item will be unselected.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		Single,
		/// <summary>
        /// Multiple selection. select multiple items and previous selected item still remains selected.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		Multiple,
		/// <summary>
        /// None of item can be selected.
        /// </summary>
        /// <since_tizen> 8 </since_tizen>
		None
	}
}
