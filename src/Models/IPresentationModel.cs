using System;
using Genesyslab.Desktop.Modules.Core.Model.Interactions;

namespace JabraInteractionExtension.Models
{
	public interface IPresentationModel
	{
		/// <summary>
		/// Gets or sets the header to set in the parent view.
		/// </summary>
		/// <value>The header.</value>
		string DeviceName { get; set; }

		/// <summary>
		/// Gets or sets the counter.
		/// </summary>
		/// <value>The counter.</value>
		Version DeviceFw { get; set; }

		/// <summary>
		/// Gets or sets the case.
		/// </summary>
		/// <value>The case.</value>
		ICase Case { get; set; }

    /// <summary>
    /// Is the interaction visible?
    /// </summary>
    bool IsInteractionVisible { get; set; }

    /// <summary>
    /// Custom actions
    /// </summary>
    void CustomAction0();

	  /// <summary>
	  /// Custom actions
	  /// </summary>
    void CustomAction1();

	  /// <summary>
	  /// Custom actions
	  /// </summary>
	  void CustomAction2();
  }
}