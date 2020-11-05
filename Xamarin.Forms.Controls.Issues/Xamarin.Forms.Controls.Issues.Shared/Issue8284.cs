using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 8284, "Cannot add/remove menuitems at runtime to contextActions of a ViewCell", PlatformAffected.Android)]
	public class Issue8284 : TestContentPage
	{
		protected override void Init()
		{
			var listView = new ListView(ListViewCachingStrategy.RecycleElement);
			listView.ItemTemplate = new DataTemplate(typeof(CustomViewCell));

			var source = new ObservableCollection<string>();
			listView.ItemsSource = source;
			Array.ForEach(Enumerable.Range(0,3).Select(i => i.ToString()).ToArray(), item => source.Add(item));

			Content = new StackLayout
			{
				Children = {
					new Label { Text = $"Press 'Add' or 'Remove' and check if the 'ContextActions' are being added." +
					                   $"Verify that by long-pressing an item in the {nameof(ListView)}" +
					                   $"The {nameof(ListView)} uses {nameof(ListViewCachingStrategy.RecycleElement)}" },
					listView
				}
			};
		}

		class CustomViewCell : ViewCell
		{
			public CustomViewCell()
			{
				var view = new StackLayout();
				View = view;
				view.Orientation = StackOrientation.Horizontal;
				view.Children.Add(new Label(){Text = "Add or remove!"});

				var menuItem = new MenuItem() { Text = "Item" };

				var addButton = new Button { Text = "Add" };
				addButton.Clicked += (sender, args) => ContextActions.Add(menuItem);

				var removeButton = new Button { Text = "Remove" };
				removeButton.Clicked += (sender, args) =>
				{
					if(ContextActions.Count != 0)
						ContextActions.RemoveAt(0);
				};

				view.Children.Add(addButton);
				view.Children.Add(removeButton);

				ContextActions.Add(menuItem);
			}
		}

	}
}