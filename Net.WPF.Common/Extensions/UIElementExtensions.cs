using System.Windows;

namespace Wpf.UI.Extensions
{
    /// <summary>
    /// UIElementExtensions
    /// </summary>
    public static class UiElementExtensions
    {
        /// <summary>
        /// Clears binding.
        /// </summary>
        public static void ResetBinding (this UIElement element, DependencyProperty property)
        {
            element.SetValue(property, DependencyProperty.UnsetValue);
        }
    }
}
