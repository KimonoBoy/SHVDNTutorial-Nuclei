using LemonUI.Menus;

namespace Nuclei.UI.Menus.Base.ItemFactory;

public static class NativeListItemExtensions
{
    /// <summary>
    ///     Ensures a <see cref="NativeListItem{T}" /> is within the bounds of the <see cref="NativeListItem{T}.Items" />
    ///     array.
    /// </summary>
    /// <typeparam name="T">The type of list item</typeparam>
    /// <param name="listItem">The item this method extends.</param>
    /// <param name="index">The index to be set.</param>
    public static void SetSelectedIndexSafe<T>(this NativeListItem<T> listItem, int index)
    {
        if (listItem.Items.Count == 0)
        {
            listItem.SelectedIndex = -1;
            return;
        }

        if (index >= 0 && index < listItem.Items.Count)
            listItem.SelectedIndex = index;
        else if (index == -1)
            listItem.SelectedIndex = listItem.Items.Count - 1;
        else
            listItem.SelectedIndex = 0;
    }
}