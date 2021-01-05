namespace Tizen.NUI.Components
{
    // Lets observable items sources notify observers about dataset changes
    internal interface ICollectionChangedNotifier
    {
        void NotifyDataSetChanged();
        void NotifyItemChanged(IItemSource source, int startIndex);
        void NotifyItemInserted(IItemSource source, int startIndex);
        void NotifyItemMoved(IItemSource source, int fromPosition, int toPosition);
        void NotifyItemRangeChanged(IItemSource source, int start, int end);
        void NotifyItemRangeInserted(IItemSource source, int startIndex, int count);
        void NotifyItemRangeRemoved(IItemSource source, int startIndex, int count);
        void NotifyItemRemoved(IItemSource source, int startIndex);
    }
}