using System;
using HidSharp;
using System.Collections.Generic;

namespace DualSenseDotNet {
    public static class Basic {

        public const int SonyManufacturerID = 1356;           // hex:054c
        public const int DualSenseControllerProductID = 3302; // hex:0CE6

        public static IEnumerable<Controller> GetConnectedControllers() {
            
            var controllers = new List<Controller>();

            foreach (HidDevice dualsense in FilteredDeviceList.Local.GetHidDevices(SonyManufacturerID, DualSenseControllerProductID)) {
                controllers.Add(new Controller(dualsense.Open()));
            }

            return controllers;
        }
    }
}
