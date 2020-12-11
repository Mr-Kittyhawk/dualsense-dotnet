using System;
using HidSharp;
using System.Collections.Generic;

namespace DualSenseDotNet {
    /// <summary> Basic functions of the library. </summary>
    public static class DualSenseDotNet {

        public const int SonyManufacturerID = 1356;           // hex:054c
        public const int DualSenseControllerProductID = 3302; // hex:0CE6

        public static IEnumerable<DualSenseController> GetConnectedControllers() {
            
            var controllers = new List<DualSenseController>();

            foreach (HidDevice dualsense in FilteredDeviceList.Local.GetHidDevices(SonyManufacturerID, DualSenseControllerProductID)) {
                controllers.Add(new DualSenseController(dualsense.Open()));
            }

            return controllers;
        }
    }
}
