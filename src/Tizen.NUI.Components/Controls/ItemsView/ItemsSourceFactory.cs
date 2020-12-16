using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
//using AndroidX.RecyclerView.Widget; ??? need to find whot it needs? adapter?

namespace Tizen.NUI.Components
{
	internal static class ItemsSourceFactory
	{
		public static IItemSource Create(IEnumerable itemsSource, ICollectionChangedNotifier notifier)
		{
			if (itemsSource == null)
			{
				return new EmptySource();
			}

			switch (itemsSource)
			{
				case IList list when itemsSource is INotifyCollectionChanged:
					return new ObservableItemSource(new MarshalingObservableCollection(list), notifier);
				case IEnumerable _ when itemsSource is INotifyCollectionChanged:
					return new ObservableItemSource(itemsSource as IEnumerable, notifier);
				case IEnumerable<object> generic:
					return new ListSource(generic);
			}

			return new ListSource(itemsSource);
		}

		public static IItemSource Create(IEnumerable itemsSource, ItemsLayouter layouter)//RecyclerView.Adapter adapter) Recycler ? 
		{
			return Create(itemsSource,layouter);
		}

		public static IItemSource Create(ItemsView colView, ItemsLayouter layouter)//RecyclerView.Adapter adapter) Recycler ?
		{
			return Create(colView.ItemsSource, layouter);
		}

		public static IGroupableItemSource Create(CollectionView colView, ItemsLayouter layouter)//RecyclerView.Adapter adapter)
		{
			var source = colView.ItemsSource;

			if (colView.IsGrouped && source != null)
			{
				return new ObservableGroupedSource(colView,  layouter);
			}

			return new UngroupedItemSource(Create(colView.ItemsSource, layouter));
		}
	}
}
