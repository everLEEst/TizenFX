using System;

namespace Tizen.NUI.Components
{
	public interface IItemSource : IDisposable
	{
		int Count { get; }

		int GetPosition(object item);
		object GetItem(int position);

		bool HasHeader { get; set; }
		bool HasFooter { get; set; }

		bool IsHeader(int position);
		bool IsFooter(int position);
	}

	public interface IGroupableItemSource : IItemSource
	{
		bool IsGroupHeader(int position);
		bool IsGroupFooter(int position);
	}
}
