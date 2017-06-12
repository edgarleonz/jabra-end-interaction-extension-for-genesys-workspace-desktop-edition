using System;
using System.Collections.Generic;
using UsbDeviceInfo;
using UsbDeviceScannerInterface;

namespace JabraInteractionExtension.Models
{
  internal class DeviceService : IDisposable
  {
    private IUsbDeviceScanner deviceScanner;
    private Dictionary<UsbHidDeviceInfo, TelephonyDevice> connectedUsbDevices = new Dictionary<UsbHidDeviceInfo, TelephonyDevice>();
    private readonly object eventLockObject = new object();

    private static readonly Lazy<DeviceService> lazy =
      new Lazy<DeviceService>(() => new DeviceService());

    public static DeviceService Instance => lazy.Value;

    private DeviceService()
    {
      deviceScanner = new UsbDeviceScanner.UsbDeviceScanner();
      deviceScanner.JabraUsbHidDeviceAdded += OnJabraHidDeviceAdded;
      deviceScanner.JabraUsbHidDeviceRemoved += OnJabraHidDeviceRemoved;
      deviceScanner.Start();
      var devices = deviceScanner.AttachedJabraHidDevices;
      foreach (var deviceInfo in devices)
      {
        connectedUsbDevices.Add(deviceInfo, new TelephonyDevice(new UsbHidDevice.UsbHidDevice(deviceInfo)));
        connectedUsbDevices[deviceInfo].ButtonInput += DeviceService_ButtonInput;
      }
    }

    #region Events

    public event EventHandler<DeviceActionEventArgs> DeviceAction;
    public event EventHandler<DeviceAttachedOrDetactedEventArgs> DeviceAttachedOrDetacted;

    #endregion

    public void GetConnectedDevices()
    {
      lock (eventLockObject)
      {
        if (connectedUsbDevices.Count == 0)
        {
          NoDevicesChanged("None", null, false);
        }
        else
        {
          foreach (var device in connectedUsbDevices)
          {
            NoDevicesChanged(device.Value.Name, device.Value.Fw, true);
          }
        }
      }
    }

    private void DeviceService_ButtonInput(object sender, DeviceActionEventArgs e)
    {
      if (e.Value == true)
      {
        DeviceAction?.Invoke(this, e);
      }
    }

    private void NoDevicesChanged(string deviceName, Version deviceFw, bool attached)
    {
      DeviceAttachedOrDetacted?.Invoke(this, new DeviceAttachedOrDetactedEventArgs { DeviceName = deviceName, DeviceFw = deviceFw, Attached = attached, NoOfDevices = connectedUsbDevices.Count });
    }

    private void OnJabraHidDeviceRemoved(UsbHidDeviceInfo deviceInfo)
    {
      lock (eventLockObject)
      {
        if (!connectedUsbDevices.ContainsKey(deviceInfo)) return;
        connectedUsbDevices[deviceInfo].ButtonInput -= DeviceService_ButtonInput;
        connectedUsbDevices.Remove(deviceInfo);
      }
      NoDevicesChanged(deviceInfo.Name, deviceInfo.FirmwareVersion, false);
    }

    private void OnJabraHidDeviceAdded(UsbHidDeviceInfo deviceInfo)
    {
      lock (eventLockObject)
      {
        if (connectedUsbDevices.ContainsKey(deviceInfo)) return;
        connectedUsbDevices.Add(deviceInfo, new TelephonyDevice(new UsbHidDevice.UsbHidDevice(deviceInfo)));
        connectedUsbDevices[deviceInfo].ButtonInput += DeviceService_ButtonInput;
      }
      NoDevicesChanged(deviceInfo.Name, deviceInfo.FirmwareVersion, true);
    }

    public void Dispose()
    {
      try
      {
        foreach (var connectedDevice in connectedUsbDevices)
        {
          connectedDevice.Value.Dispose();
        }
        deviceScanner.Dispose();
        deviceScanner = null;
        connectedUsbDevices = null;
      }
      catch (Exception)
      {
      }
    }
  }
}
