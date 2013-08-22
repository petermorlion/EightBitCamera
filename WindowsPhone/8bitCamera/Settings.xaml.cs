using System.Windows;
using System.Windows.Controls;
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

            var currentPixelation = new PixelationSizeQuery().Get();
            pixelationPicker.SelectedItem = currentPixelation;

            var saveToCameraRoll = new SaveOriginalToCameraRollQuery().Get();
            saveToCameraRollCheckBox.IsChecked = saveToCameraRoll;

            pixelationPicker.SelectionChanged += OnPixelationChanged;
            saveToCameraRollCheckBox.Checked += OnSaveToCameraRollCheckBoxChecked;
        }

        private void OnSaveToCameraRollCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var command = new SaveOriginalToCameraRollCommand();
            command.Set(saveToCameraRollCheckBox.IsChecked.HasValue && saveToCameraRollCheckBox.IsChecked.Value);
        }

        private void OnPixelationChanged(object sender, SelectionChangedEventArgs e)
        {
            var command = new PixelationSizeCommand();
            command.Set(int.Parse(pixelationPicker.SelectedItem.ToString()));
        }
    }
}