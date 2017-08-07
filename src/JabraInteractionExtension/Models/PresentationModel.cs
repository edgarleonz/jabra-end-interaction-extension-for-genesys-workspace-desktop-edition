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
	  /// Custom action 1
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
			set { if (deviceName != value) { deviceName = value; OnPropertyChanged("DeviceName"); } }
		}

	  Version deviceFw;
	  /// <summary>
	  /// Gets or sets the header to set in the parent view.
	  /// </summary>
	  /// <value>The header.</value>
	  public Version DeviceFw
	  {
	    get { return deviceFw; }
	    set { if (deviceFw != value) { deviceFw = value; OnPropertyChanged("DeviceFw"); } }
	  }

    ICase @case;
		/// <summary>
    /// Gets or sets the case.
    /// </summary>
    /// <value>The case.</value>
    public ICase Case
		{
			get { return @case; }
			set { if (@case != value) { @case = value; OnPropertyChanged("Case"); } }
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

    /*
	  private void TestVoiceRecording()
	  {
	    var container = HeadsetView.Container;

	    IAgent myAgent = container.Resolve<IAgent>();
	    IEnterpriseServiceProvider enterpriseService = myAgent.EntrepriseService;

	    var esp = HeadsetView.Esp;

	    var iv = esp.Resolve<IInteractionVoice>();

      //iv.IsCallRecording
	    iv.IsCallRecording = true;
	  }

    private void Test123()
	  {
	    var container = HeadsetView.Container;

	    IAgent myAgent = container.Resolve<IAgent>();
	    IEnterpriseServiceProvider enterpriseService = myAgent.EntrepriseService;

	    var interactionManager = container.Resolve<IInteractionManager>();

	    foreach (var i in interactionManager.Interactions)
	    {
	      string msg = "Interaction id: " + i.InteractionId.ToString() + " , Agent name: " + i.Agent.UserName + " , Duration: " + i.Duration.ToString();

	      MessageBox.Show(msg);

	      var data = i.ExtractAttachedData();
	      var keys = data.AllKeys;

	      foreach (var key in keys)
	      {
	        MessageBox.Show("Key - " + key + " : " + data[key].ToString());
	      }

        i.SetAttachedData("Jabra", "Headset BIZ2300, FW 1.2.3");
	    }      
    }

	  private void TestXYZ()
	  {
	    var container = MySampleView.Container;

	    IAgent myAgent = container.Resolve<IAgent>();
	    IEnterpriseServiceProvider enterpriseService = myAgent.EntrepriseService;

	    var interactionManager = container.Resolve<IInteractionManager>();

//      interactionManager.

//	    SerializationInfo info;
//	    StreamingContext context;
//	    var request = new RequestFindInteractions(info, context);

	    var request = RequestFindInteractions.Create();

//	    foreach (var r in request.)
//	    {
//	    }
	  }
    */

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
