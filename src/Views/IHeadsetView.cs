using Genesyslab.Desktop.Infrastructure;
using Genesyslab.Desktop.Modules.Windows.Common.DimSize;
using JabraInteractionExtension.Models;

namespace JabraInteractionExtension.Views
{
	/// <summary>
	/// Interface matching the MySampleView view
	/// </summary>
	public interface IHeadsetView : IView,IMin
	{
		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		IPresentationModel Model { get; set; }
	}
}
