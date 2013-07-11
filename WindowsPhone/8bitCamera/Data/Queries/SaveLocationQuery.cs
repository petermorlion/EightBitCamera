﻿using System;

namespace EightBitCamera.Data.Queries
{
    public class SaveLocationQuery : IsolatedStorageFileQuery<SaveLocations>
    {
        public override SaveLocations Get()
        {
            var settingString = Get(SettingsKeys.SaveLocation);
            try
            {
                return (SaveLocations)Enum.Parse(typeof (SaveLocations), settingString, true);
            }
            catch (ArgumentException)
            {
                return SaveLocations.CameraRollAndApplicationStorage;
            }
        }
    }
}