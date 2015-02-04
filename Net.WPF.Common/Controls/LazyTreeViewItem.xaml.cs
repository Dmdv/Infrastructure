using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.UI.Controls
{
	/// <summary>
	/// LazyTreeViewItem.
	/// </summary>
	public class LazyTreeViewItem : TreeViewItem
	{
		/// <summary>
		/// The <see cref="HasChildren" /> dependency property's name.
		/// </summary>
		public const string HasChildrenPropertyName = "HasChildren";

		/// <summary>
		/// Identifies the <see cref="HasChildren" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty HasChildrenProperty =
			DependencyProperty.Register(
				HasChildrenPropertyName,
				typeof (bool),
				typeof (LazyTreeViewItem),
				new PropertyMetadata(OnHasChildrenChanged));

		public event EventHandler ItemClicked;

		public event EventHandler ItemExpanded;

		/// <summary>
		/// Gets or sets the value of the <see cref="HasChildren" />
		/// </summary>
		public bool HasChildren
		{
			get { return (bool) GetValue(HasChildrenProperty); }
			set { SetValue(HasChildrenProperty, value); }
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			var item = new LazyTreeViewItem();

			// Expanded
			item.ItemExpanded += (s, e) => RaiseEvent(ItemExpanded, s);

			// Clicked
			item.ItemClicked += (s, e) => RaiseEvent(ItemClicked, s);

			return item;
		}

		protected override void OnExpanded(RoutedEventArgs e)
		{
			RaiseEvent(ItemExpanded, this);
			base.OnExpanded(e);
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			RaiseEvent(ItemClicked, this);
			e.Handled = true;
			base.OnMouseLeftButtonUp(e);
		}

		private static void OnHasChildrenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private static void RaiseEvent(EventHandler handler, object sender)
		{
			if (handler != null)
			{
				handler(sender, EventArgs.Empty);
			}
		}
	}
}