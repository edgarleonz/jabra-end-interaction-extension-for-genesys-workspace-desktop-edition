using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GnProtocol;
using UsbDeviceInfo;
using UsbHidDeviceInterface;

namespace JabraInteractionExtension.Models
{
  internal class TelephonyDevice : IDisposable
  {
    private readonly byte targetGnpAddress = 1;
    protected IUsbHidDevice usbDevice;
    protected IGnProtocol gnProtocolHandler;
    protected ITopLevelCollection topLevelCollection;
    protected ITopLevelCollection buttonTopLevelCollection;
    private object deviceLockObject;
    private object eventLockObject;

    private static readonly List<int> validPiDs = new List<int>()
    {
      0x2301, // BIZ 2300 mono
      0x2302, // BIZ 2300 stereo
      0x2303, // BIZ 2300 MS mono
      0x2304, // BIZ 2300 MS stereo
      0x2319, // LINK 260 (Santana Advanced)
      0x2311, // LINK 265
      0x2321, // BIZ 2400 II CC mono
      0x2322, // BIZ 2400 II CC stereo
      0x2323, // BIZ 2400 II CC MS mono
      0x2324 // BIZ 2400 II CC MS stereo
    };

    private static bool ContainsValidPid(int productId)
    {
      return validPiDs.Contains(productId);
    }

    public string Name => usbDevice.Name;
    public Version Fw => usbDevice.FirmwareVersion;

    private const ushort USAGE_BUTTON_1 = 0x01;
    private const ushort USAGE_BUTTON_2 = 0x02;
    private const ushort USAGE_BUTTON_3 = 0x03;
    private const ushort USAGE_PAGE_BUTTON = 0x09;
    private const ushort USAGE_PAGE_TELEPHONY = 0x0B;
    private const ushort USAGE_PAGE_CONSUMER = 0x0C;
    private const ushort USAGE_PAGE_GN_TELEPHONY = 0xFF30;
    public event EventHandler<DeviceActionEventArgs> ButtonInput;

    public enum ButtonId
    {
      /// <summary>
      /// Custom button 1
      /// </summary>
      CustomButton1 = 101,

      /// <summary>
      /// Custom button 2
      /// </summary>
      CustomButton2 = 102,

      /// <summary>
      /// Custom button 3
      /// </summary>
      CustomButton3 = 103
    }

    internal TelephonyDevice(IUsbHidDevice usbDevice)
    {
      this.usbDevice = usbDevice;
      deviceLockObject = new object();
      eventLockObject = new object();
      try
      {
        lock (deviceLockObject)
        {
          /*
           */
          // Select the top-level collection to use
          topLevelCollection =
            usbDevice.TopLevelCollections.FirstOrDefault(x => x.MainUsagePage == UsagePages.USAGE_PAGE_GN_PROTOCOL);
          if (topLevelCollection == null)
          {
            throw new ArgumentException("The HID device has no telephony support");
          }

          buttonTopLevelCollection =
            usbDevice.TopLevelCollections.FirstOrDefault(x => x.MainUsagePage == UsagePages.USAGE_PAGE_BUTTON);
          if (buttonTopLevelCollection == null)
          {
            throw new ArgumentException("The HID device has no Button support");
          }
          /*
          */

          gnProtocolHandler = new GnProtocolOverUsbHid.GnProtocolOverUsbHid(usbDevice);

          buttonTopLevelCollection.Open();

          // Add device event listeners
          topLevelCollection.ButtonInputReceived += OnButtonInputReceived;
          buttonTopLevelCollection.ButtonInputReceived += OnButtonInputReceived;

          if (usbDevice.HasGnProtocolSupport)
          {
            // Send a GNP_DEVICE_ATTACH_EVENT GN protocol event to the device in order to subscribe to 
            // GN protocol events from the device.
            SendAttachEvent();
          }

          #region Define custom buttons 1 and 2 - not 0   GNP_CONFIG_BUTTON_FUNCTION   0x27 

//          if (ContainsValidPid(usbDevice.ProductId))
          {
            var gnpPacket = new GnpPacket
            {
              DestinationAddress = targetGnpAddress,
              Type = GnpPacket.PacketType.Write,
              Command = GnpCommands.CONFIG,
              Data = new byte[] { 0x27, 0x01, 0x00, 0x00 }
            };
            gnProtocolHandler.SendGnProtocolPacket(gnpPacket);

            gnpPacket = new GnpPacket
            {
              DestinationAddress = targetGnpAddress,
              Type = GnpPacket.PacketType.Write,
              Command = GnpCommands.CONFIG,
              Data = new byte[] { 0x27, 0x01, 0x01, 0x00 }
            };
            gnProtocolHandler.SendGnProtocolPacket(gnpPacket);

            gnpPacket = new GnpPacket
            {
              DestinationAddress = targetGnpAddress,
              Type = GnpPacket.PacketType.Write,
              Command = GnpCommands.CONFIG,
              Data = new byte[] { 0x27, 0x01, 0x02, 0x0D }
            };
            gnProtocolHandler.SendGnProtocolPacket(gnpPacket);


            /*
            var gnpPacket = new GnpPacket
            {
              DestinationAddress = targetGnpAddress,
              Type = GnpPacket.PacketType.Write,
              Command = GnpCommands.CONFIG,
              Data = new byte[] { 0x27, 0x02, 0x01, 0x00 }
            };
            gnProtocolHandler.SendGnProtocolPacket(gnpPacket);

            gnpPacket = new GnpPacket
            {
              DestinationAddress = targetGnpAddress,
              Type = GnpPacket.PacketType.Write,
              Command = GnpCommands.CONFIG,
              Data = new byte[] { 0x27, 0x02, 0x00, 0x00 }
            };
            gnProtocolHandler.SendGnProtocolPacket(gnpPacket);

            gnpPacket = new GnpPacket
            {
              DestinationAddress = targetGnpAddress,
              Type = GnpPacket.PacketType.Write,
              Command = GnpCommands.CONFIG,
              Data = new byte[] { 0x27, 0x02, 0x02, 0x0E }
            };
            gnProtocolHandler.SendGnProtocolPacket(gnpPacket);
            */

          }

          #endregion
        }
      }
      catch (Exception)
      {
        // Ignore exceptions

        //Logger.Write($"Error on the Device init: {e.Message} {e.StackTrace}");
//        this.Dispose();
      }
    }


    private void SendAttachEvent()
    {
      try
      {
        GnpPacket packet = GnProtocol.GnpCommands.GnpDeviceAttachEvent;
        packet.DestinationAddress = targetGnpAddress;
        gnProtocolHandler.SendGnProtocolPacket(packet);
      }
      catch (Exception)
      {
        //Logger.Write($"SendAttachEvent Error: {ex.Message} {ex.StackTrace}");
      }
    }

    private void OnButtonInputReceived(object sender, HidButtonInputEventArgs hidButtonInputEventArgs)
    {
      //    Logger.Write($"TelephonyDevice: Button input from {Name}: MainUsagePage: 0x{hidButtonInputEventArgs.MainUsagePage:X2}, UsagePage: 0x{hidButtonInputEventArgs.UsageInfo.UsagePage:X2}, Usage: 0x{hidButtonInputEventArgs.UsageInfo.Usage:X2}, value: {hidButtonInputEventArgs.Value}");
      if (usbDevice == null)
        return;

      lock (eventLockObject)
      {
        try
        {
          switch (hidButtonInputEventArgs.UsageInfo.UsagePage)
          {
            case USAGE_PAGE_TELEPHONY:
            case USAGE_PAGE_GN_TELEPHONY:
            case USAGE_PAGE_BUTTON:
              switch (hidButtonInputEventArgs.UsageInfo.Usage)
              {
                // Button events
                case USAGE_BUTTON_1:
                  RaiseButtonEvent(ButtonId.CustomButton1, hidButtonInputEventArgs.Value, Name);
                  break;
                case USAGE_BUTTON_2:
                  RaiseButtonEvent(ButtonId.CustomButton2, hidButtonInputEventArgs.Value, Name);
                  break;
                case USAGE_BUTTON_3:
                  RaiseButtonEvent(ButtonId.CustomButton3, hidButtonInputEventArgs.Value, Name);
                  break;
              }
              break;
          }
        }
        catch (Exception ex)
        {
          // Eat exception
          Trace.WriteLine("TelephonyDevice.OnInputButtonData exception: " + ex.Message);
        }
      }
    }

    protected virtual void RaiseButtonEvent(ButtonId buttonId, bool? value, string deviceName = "")
    {
      if (ButtonInput != null)
        ButtonInput(this, new DeviceActionEventArgs { Button = buttonId, Value = value, DeviceName = deviceName });
    }

    public void Dispose()
    {
      lock (deviceLockObject)
      {
        if (usbDevice == null)
          return;

        topLevelCollection.ButtonInputReceived -= OnButtonInputReceived;
        buttonTopLevelCollection.ButtonInputReceived -= OnButtonInputReceived;
        buttonTopLevelCollection.Close();
        topLevelCollection.Close();
        this.usbDevice.Dispose();
      }
    }
  }
}
