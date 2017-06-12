using System;

namespace JabraInteractionExtension.Models
{
  class DeviceAttachedOrDetactedEventArgs : EventArgs
  {
    /// <summary>
    /// Is device attached? Or detached
    /// </summary>
    public bool Attached { get; set; }

    /// <summary>
    /// Device name
    /// </summary>
    public string DeviceName { get; set; }

    /// <summary>
    /// Device name
    /// </summary>
    public Version DeviceFw { get; set; }

    /// <summary>
    /// No of devices attached
    /// </summary>
    public int NoOfDevices { get; set; }

    public override string ToString()
    {
      if (Attached)
      {
        return $"DeviceAttached: {DeviceName} is attached - No of compatibility device: {NoOfDevices}";
      }
      return $"DeviceAttached: {DeviceName} is detached - No of compatibility device: {NoOfDevices}";
    }
  }
}
