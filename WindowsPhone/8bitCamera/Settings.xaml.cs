﻿using System.Windows.Controls;
using EightBitCamera.Data.Commands;
using EightBitCamera.Data.Queries;
using Microsoft.Phone.Controls;

namespace EightBitCamera
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            pixelationPicker.Items.Add(3);
            pixelationPicker.Items.Add(4);
            pixelationPicker.Items.Add(5);
            pixelationPicker.Items.Add(6);
            pixelationPicker.Items.Add(7);
            pixelationPicker.Items.Add(8);
            pixelationPicker.Items.Add(9);
            pixelationPicker.Items.Add(10);

            var currentSetting = new PixelationSizeQuery().Get();
            pixelationPicker.SelectedItem = currentSetting;

            pixelationPicker.SelectionChanged += OnPixelationChanged;
        }

        private void OnPixelationChanged(object sender, SelectionChangedEventArgs e)
        {
            var command = new PixelationSizeCommand();
            command.Set(int.Parse(pixelationPicker.SelectedItem.ToString()));
        }
    }
}