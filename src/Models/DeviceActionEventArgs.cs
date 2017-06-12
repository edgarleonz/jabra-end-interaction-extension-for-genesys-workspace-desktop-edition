using System;

namespace JabraInteractionExtension.Models
{
  class DeviceActionEventArgs : EventArgs
  {
    /// <summary>
    /// Button identifier
    /// </summary>
    public TelephonyDevice.ButtonId Button { get; set; }

    /// <summary>
    /// Button value.
    /// If the button is a relative button the value is undefined (null).
    /// </summary>
    public bool? Value { get; set; }

    /// <summary>
    /// Device name
    /// </summary>
    public string DeviceName { get; set; }

    /// <summary>
    /// Get the Action from a device:  string DeviceAction;{Button};{Value};{DeviceName}
    /// </summary>
    /// <returns>
    /// string DeviceAction;{Button};{Value};{DeviceName}
    /// where {Button} id the "CustomButtonX" (X = {0,1,2}).
    /// where {value} is "True" or "False".
    /// Where {DeviceName} is the device sending the event.
    /// </returns>
    public string DeviceActionCommandString()
    {
      return $"DeviceAction;{Button};{Value};{DeviceName}";
    }

    public override string ToString()
    {
      return $"DeviceAction: Button pressed: {Button} Value: {Value} on Device: {DeviceName}";
    }
  }
}
