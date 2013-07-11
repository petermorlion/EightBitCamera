using System.Windows.Controls;
using EightBitCamera.Data;
using EightBitCamera.Data.Commands;
using EightBitCamera.Data.Queries;
using Microsoft.Phone.Controls;

namespace EightBitCamera
{
    public partial class Settings : PhoneApplicationPage
    {
        private const string CameraRoll = "Camera Roll";
        private const string ApplicationStorage = "Application";
        private const string CameraRollAndApplicationStorage = "Camera Roll and application";

        public Settings()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (pixelationPicker.Items.Count == 0)
            {
                pixelationPicker.Items.Add(3);
                pixelationPicker.Items.Add(4);
                pixelationPicker.Items.Add(5);
                pixelationPicker.Items.Add(6);
                pixelationPicker.Items.Add(7);
                pixelationPicker.Items.Add(8);
                pixelationPicker.Items.Add(9);
                pixelationPicker.Items.Add(10);   
            }

            if (saveLocationPicker.Items.Count == 0)
            {
                saveLocationPicker.Items.Add(CameraRoll);
                saveLocationPicker.Items.Add(ApplicationStorage);
                saveLocationPicker.Items.Add(CameraRollAndApplicationStorage);
            }

            var currentPixelation = new PixelationSizeQuery().Get();
            pixelationPicker.SelectedItem = currentPixelation;

            var currentSaveLocation = new SaveLocationQuery().Get();
            saveLocationPicker.SelectedItem = TranslateSaveLocation(currentSaveLocation);

            pixelationPicker.SelectionChanged += OnPixelationChanged;
            saveLocationPicker.SelectionChanged += OnSaveLocationChanged;
        }

        private void OnPixelationChanged(object sender, SelectionChangedEventArgs e)
        {
            var command = new PixelationSizeCommand();
            command.Set(int.Parse(pixelationPicker.SelectedItem.ToString()));
        }

        private void OnSaveLocationChanged(object sender, SelectionChangedEventArgs e)
        {
            var command = new SaveLocationCommand();
            command.Set(TranslateSaveLocation(saveLocationPicker.SelectedItem.ToString()));
        }

        private string TranslateSaveLocation(SaveLocations saveLocation)
        {
            switch (saveLocation)
            {
                case SaveLocations.CameraRoll:
                    return CameraRoll;
                case SaveLocations.ApplicationStorage:
                    return ApplicationStorage;
                case SaveLocations.CameraRollAndApplicationStorage:
                    return CameraRollAndApplicationStorage;
                default:
                    return CameraRollAndApplicationStorage;
            }
        }

        private SaveLocations TranslateSaveLocation(string saveLocation)
        {
            switch (saveLocation)
            {
                case CameraRoll:
                    return SaveLocations.CameraRoll;
                case ApplicationStorage:
                    return SaveLocations.ApplicationStorage;
                case CameraRollAndApplicationStorage:
                    return SaveLocations.CameraRollAndApplicationStorage;
                default:
                    return SaveLocations.CameraRollAndApplicationStorage;
            }
        }
    }
}