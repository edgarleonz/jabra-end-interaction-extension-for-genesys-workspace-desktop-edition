using Genesyslab.Desktop.Infrastructure;
using JabraInteractionExtension.Models;

namespace JabraInteractionExtension.Views
{
  public interface IHeadsetButtonView : IView
  {
    IPresentationModel Model { get; set; }
  }
}
