using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 9998, "[Bug] [iOS] [Android] ListView bug for CachingStrategy='RecycleElementAndDataTemplate'", PlatformAffected.Android & PlatformAffected.iOS)]
	public class Issue9998 : ContentPage
	{
		public Issue9998()
		{
			Action1GlobalCommand = new Command<ItemModel>(async (model) => await Action1GlobalCommandHandlerAsync(model));
			Action2GlobalCommand = new Command<ItemModel>(async (model) => await Action2GlobalCommandHandlerAsync(model));
			Action3GlobalCommand = new Command<ItemModel>(async (model) => await Action3GlobalCommandHandlerAsync(model));

			var items = new ImprovedObservableCollection<ItemModel>()
			{
				new ItemModel() { Type = ItemModel.ItemModelType.Type1, Action1Command = Action1GlobalCommand,  Action2Command = Action2GlobalCommand,  Action3Command = Action3GlobalCommand },
				new ItemModel() { Type = ItemModel.ItemModelType.Type2, Action1Command = Action1GlobalCommand,  Action2Command = Action2GlobalCommand,  Action3Command = Action3GlobalCommand },
				new ItemModel() { Type = ItemModel.ItemModelType.Type3, Action1Command = Action1GlobalCommand,  Action2Command = Action2GlobalCommand,  Action3Command = Action3GlobalCommand }
			};
			
			var listView = new ListView(ListViewCachingStrategy.RecycleElementAndDataTemplate)
			{
				HasUnevenRows = true,
			};
			listView.ItemsSource = items;
			listView.ItemTemplate = new ItemModelTemplateSelector();

			Content = new StackLayout
			{
				Children = {
					new Label { Text = "Welcome to Xamarin.Forms!" },
					listView
				}
			};

		}

		public ICommand Action1GlobalCommand { get; }
		public ICommand Action2GlobalCommand { get; }
		public ICommand Action3GlobalCommand { get; }

		async Task Action1GlobalCommandHandlerAsync(ItemModel model) => 
			await DisplayAlert("Information", $"Action 1 for {model.TypeName}", "OK");

		async Task Action2GlobalCommandHandlerAsync(ItemModel model) => 
			await DisplayAlert("Information", $"Action 2 for {model.TypeName}", "OK");

		async Task Action3GlobalCommandHandlerAsync(ItemModel model) => 
			await DisplayAlert("Information", $"Action 3 for {model.TypeName}", "OK");

		class ItemModel
		{
			public enum ItemModelType
			{
				Type1,
				Type2,
				Type3
			}

			public string TypeName { get { return this.Type.ToString(); } }

			public ItemModelType Type { get; set; }

			public ICommand Action1Command { get; set; }
			public ICommand Action2Command { get; set; }
			public ICommand Action3Command { get; set; }
		}

		[Preserve(AllMembers = true)]
		class ItemModelTemplateSelector : DataTemplateSelector
		{
			readonly DataTemplate Type1Template = new DataTemplate(typeof(Type1Cell));
			readonly DataTemplate Type2Template = new DataTemplate(typeof(Type2Cell));
			readonly DataTemplate Type3Template = new DataTemplate(typeof(Type3Cell));

			protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
			{
				if(!(item is ItemModel itemModel)) throw new ArgumentException();

				var template = itemModel.Type switch
				{
					ItemModel.ItemModelType.Type1 => Type1Template,
					ItemModel.ItemModelType.Type2 => Type2Template,
					ItemModel.ItemModelType.Type3 => Type3Template,
					_ => throw new ArgumentException()
				};

				return template;

			}
		}

		[Preserve(AllMembers = true)]
		class Type1Cell : ViewCell
		{
			public Type1Cell()
			{
				var menuItem1 = new MenuItem() { Text = "Action1" };
				var menuItem2 = new MenuItem() { Text = "Action2" };

				menuItem1.SetBinding(MenuItem.CommandProperty, $"{nameof(ItemModel.Action1Command)}");
				menuItem1.SetBinding(MenuItem.CommandParameterProperty, $".");

				menuItem2.SetBinding(MenuItem.CommandProperty, $"{nameof(ItemModel.Action2Command)}");
				menuItem2.SetBinding(MenuItem.CommandParameterProperty, $".");

				ContextActions.Add(menuItem1);
				ContextActions.Add(menuItem2);

				var l1 = new Label();
				l1.SetBinding(Label.TextProperty, $"{nameof(ItemModel.TypeName)}");

				View = new StackLayout()
				{
					Orientation = StackOrientation.Vertical,
					MinimumHeightRequest = 60,
					Padding = 20,
					Children =
					{
						new StackLayout()
						{
							Orientation = StackOrientation.Horizontal,
							Children =
							{
								new Label(){Text = "ItemModel Type = "},
								l1
							}
						},
						new Label(){Text="Template1"},
						new Label(){Text="Should have ContextActions 1 and 2"},
					}
				};

			}
		}

		[Preserve(AllMembers = true)]
		class Type2Cell : ViewCell
		{
			public Type2Cell()
			{
				var l1 = new Label();
				l1.SetBinding(Label.TextProperty, $"{nameof(ItemModel.TypeName)}");

				View = new StackLayout()
				{
					Orientation = StackOrientation.Vertical,
					MinimumHeightRequest = 60,
					Padding = 20,
					Children =
					{
						new StackLayout()
						{
							Orientation = StackOrientation.Horizontal,
							Children =
							{
								new Label() { Text = "ItemModel Type = " },
								l1
							}
						},
						new Label() { Text = "Template2" },
						new Label() { Text = "Should have NO ContextActions" },
					}
				};

			}
		}

		[Preserve(AllMembers = true)]
		class Type3Cell : ViewCell
		{
			public Type3Cell()
			{
				var menuItem3 = new MenuItem() { Text = "Action3" };

				menuItem3.SetBinding(MenuItem.CommandProperty, $"{nameof(ItemModel.Action3Command)}");
				menuItem3.SetBinding(MenuItem.CommandParameterProperty, $".");

				ContextActions.Add(menuItem3);

				var l1 = new Label();
				l1.SetBinding(Label.TextProperty, $"{nameof(ItemModel.TypeName)}");

				View = new StackLayout()
				{
					Orientation = StackOrientation.Vertical,
					MinimumHeightRequest = 60,
					Padding = 20,
					Children =
					{
						new StackLayout()
						{
							Orientation = StackOrientation.Horizontal,
							Children =
							{
								new Label() { Text = "ItemModel Type = " },
								l1
							}
						},
						new Label() { Text = "Template3" },
						new Label() { Text = "Should have ContextAction 3" },
					}
				};
			}
		}
	}
}