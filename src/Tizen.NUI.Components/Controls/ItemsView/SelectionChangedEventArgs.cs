using System;
using System.Collections.Generic;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// Selection changed event. this might be deprecated.
    /// </summary>
    /// <since_tizen> 8 </since_tizen>
	public class SelectionChangedEventArgs : EventArgs
	{

    	/// <summary>
    	/// Previous selecitdon list.
    	/// </summary>
    	/// <since_tizen> 8 </since_tizen>
		public IReadOnlyList<object> PreviousSelection { get; }

    	/// <summary>
  	    /// Current selecitdon list.	
  	 	///  </summary>
  		/// <since_tizen> 8 </since_tizen>
		public IReadOnlyList<object> CurrentSelection { get; }

		static readonly IReadOnlyList<object> s_empty = new List<object>(0);

		internal SelectionChangedEventArgs(object previousSelection, object currentSelection)
		{
			PreviousSelection = previousSelection != null ? new List<object>(1) { previousSelection } : s_empty;
			CurrentSelection = currentSelection != null ? new List<object>(1) { currentSelection } : s_empty;
		}

		internal SelectionChangedEventArgs(IList<object> previousSelection, IList<object> currentSelection)
		{
			PreviousSelection = new List<object>(previousSelection ?? throw new ArgumentNullException(nameof(previousSelection)));
			CurrentSelection = new List<object>(currentSelection ?? throw new ArgumentNullException(nameof(currentSelection)));
		}
	}
}