using Microsoft.UI.Xaml.Controls;

namespace imdbdrinks_ratingsmodule.Test.Helpers
{
    /// <summary>
    /// Interface for abstracting the ListView functionality we need for testing
    /// </summary>
    public interface IListViewAdapter
    {
        int SelectedIndex { get; }
    }

    /// <summary>
    /// Simple implementation of IListViewAdapter for testing
    /// </summary>
    public class TestListViewAdapter : IListViewAdapter
    {
        /// <summary>
        /// Creates a new TestListViewAdapter with the specified selected index
        /// </summary>
        public TestListViewAdapter(int selectedIndex)
        {
            SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// The selected index that will be used in tests
        /// </summary>
        public int SelectedIndex { get; set; }
    }
} 