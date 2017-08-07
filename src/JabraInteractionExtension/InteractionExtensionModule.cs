using Genesyslab.Desktop.Infrastructure;
using Genesyslab.Desktop.Infrastructure.Commands;
using Genesyslab.Desktop.Infrastructure.DependencyInjection;
using Genesyslab.Desktop.Infrastructure.ViewManager;
using JabraInteractionExtension.Models;
using JabraInteractionExtension.Views;

namespace JabraInteractionExtension
{
	/// <summary>
	/// This class is a sample module which shows several ways of customization
	/// </summary>
	public class InteractionExtensionModule : IModule
	{
		readonly IObjectContainer container;
		readonly IViewManager viewManager;
		readonly ICommandManager commandManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="InteractionExtensionModule"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="viewManager">The view manager.</param>
		/// <param name="commandManager">The command manager.</param>
		public InteractionExtensionModule(IObjectContainer container, IViewManager viewManager, ICommandManager commandManager)
		{
			this.container = container;
			this.viewManager = viewManager;
			this.commandManager = commandManager;
		}

		/// <summary>
		/// Initializes the module.
		/// </summary>
		public void Initialize()
		{
			// Add a view in the right panel in the interaction window

			// Here we register the view (GUI) "IMySampleView" and its behavior counterpart "IMySampleViewModel"
			container.RegisterType<IHeadsetView, HeadsetView>();
			container.RegisterType<IPresentationModel, PresentationModel>();

			// Put the MySample view in the region "InteractionWorksheetRegion"
			viewManager.ViewsByRegionName["InteractionWorksheetRegion"].Add(
				new ViewActivator() { ViewType = typeof(IHeadsetView), ViewName = "HeadsetInteraction", ActivateView = true }
			);

			// Here we register the view (GUI) "IMySampleButtonView"
			container.RegisterType<IHeadsetButtonView, HeadsetButtonView>();

			// Put the MySampleMenuView view in the region "CaseViewSideButtonRegion" (The case toggle button in the interaction windows)
			viewManager.ViewsByRegionName["CaseViewSideButtonRegion"].Add(
				new ViewActivator() { ViewType = typeof(IHeadsetButtonView), ViewName = "HeadsetButtonView", ActivateView = true }
			);
		}
	}
}
