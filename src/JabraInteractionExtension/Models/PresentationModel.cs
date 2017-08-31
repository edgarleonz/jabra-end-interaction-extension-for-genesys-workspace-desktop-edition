using System;
using System.ComponentModel;
using Genesyslab.Desktop.Modules.Core.Model.Interactions;
using HeadsetView = JabraInteractionExtension.Views.HeadsetView;

namespace JabraInteractionExtension.Models
{
  /// <summary>
  /// The behabior of the PresentationModel class
  /// </summary>
  public class PresentationModel : IPresentationModel, INotifyPropertyChanged
  {
    // Field variables
    private DeviceService deviceService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PresentationModel"/> class.
    /// </summary>
    public PresentationModel()
    {
      DeviceInit();
    }

    private void DeviceInit()
    {
      deviceService = DeviceService.Instance;
      deviceService.DeviceAttachedOrDetacted += DeviceService_DeviceAttachedOrDetacted;
      deviceService.DeviceAction += DeviceService_DeviceAction;
      deviceService.GetConnectedDevices();
    }

    private void DeviceService_DeviceAttachedOrDetacted(object sender, DeviceAttachedOrDetactedEventArgs e)
    {
      if (e.Attached)
      {
        DeviceName = e.DeviceName;
        DeviceFw = e.DeviceFw;
      }
      else
      {
        DeviceName = "-";
        DeviceFw = null;
      }
    }

    private void DeviceService_DeviceAction(object sender, DeviceActionEventArgs e)
    {
      switch (e.Button)
      {
        case TelephonyDevice.ButtonId.CustomButton1:
          CustomAction0();
          break;

        case TelephonyDevice.ButtonId.CustomButton2:
          CustomAction1();
          break;

        case TelephonyDevice.ButtonId.CustomButton3:
          CustomAction2();
          break;
      }
    }

    /// <summary>
    /// Custom action 0
    /// </summary>
    public void CustomAction0()
    {
      CloseInteractionIfPossible();
    }

    /// <summary>
    /// Custom action 1
    /// </summary>
    public void CustomAction1()
    {
      CloseInteractionIfPossible();
    }

    /// <summary>
    /// Custom action 2
    /// </summary>
    public void CustomAction2()
    {
      CloseInteractionIfPossible();
    }


    #region IMySamplePresentationModel Members

    string deviceName = "-";

    /// <summary>
    /// Gets or sets the header to set in the parent view.
    /// </summary>
    /// <value>The header.</value>
    public string DeviceName
    {
      get { return deviceName; }
      set
      {
        if (deviceName != value)
        {
          deviceName = value;
          OnPropertyChanged("DeviceName");
        }
      }
    }

    Version deviceFw;

    /// <summary>
    /// Gets or sets the header to set in the parent view.
    /// </summary>
    /// <value>The header.</value>
    public Version DeviceFw
    {
      get { return deviceFw; }
      set
      {
        if (deviceFw != value)
        {
          deviceFw = value;
          OnPropertyChanged("DeviceFw");
        }
      }
    }

    ICase @case;

    /// <summary>
    /// Gets or sets the case.
    /// </summary>
    /// <value>The case.</value>
    public ICase Case
    {
      get { return @case; }
      set
      {
        if (@case != value)
        {
          @case = value;
          OnPropertyChanged("Case");
        }
      }
    }

    public bool IsInteractionVisible { get; set; }

    private void CloseInteractionIfPossible()
    {
      var container = HeadsetView.Container;

      var interactionManager = container.Resolve<IInteractionManager>();

      string id = string.Empty;
      foreach (var interaction in interactionManager.Interactions)
      {
        if (interaction.IsItPossibleToMarkDone && IsInteractionVisible == true && interaction.CaseId == @case.CaseId)
        {
          id = interaction.InteractionId;
        }
      }
      if (!string.IsNullOrEmpty(id))
      {
        interactionManager.CloseInteraction(id);
      }
    }

    #endregion

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}