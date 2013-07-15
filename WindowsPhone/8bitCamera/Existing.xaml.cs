using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using EightBitCamera.Data.Queries;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace EightBitCamera
{
    public partial class Existing : PhoneApplicationPage
    {
        public Existing()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // TODO: only show chooser first time. If user doesn't select a picture and goes back, show placeholder
            if (pixelatedImage.Source == null)
            {
                var task = new PhotoChooserTask();
                task.Completed += OnPhotoChooserTaskCompleted;
                task.ShowCamera = true;
                task.Show();
            }

            if (pixelationListBox.Items.Count == 0)
            {
                pixelationListBox.Items.Add(3);
                pixelationListBox.Items.Add(4);
                pixelationListBox.Items.Add(5);
                pixelationListBox.Items.Add(6);
                pixelationListBox.Items.Add(7);
                pixelationListBox.Items.Add(8);
                pixelationListBox.Items.Add(9);
                pixelationListBox.Items.Add(10);
            }

            // TODO: don't work
            pixelationListBox.SelectedItem = new PixelationSizeQuery().Get();
            pixelationListBox.SelectionChanged += OnPixelationListBoxSelectionChanged;
        }

        private void OnPhotoChooserTaskCompleted(object sender, PhotoResult e)
        {
            if (e.ChosenPhoto == null)
            {
                return;
            }

            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(e.ChosenPhoto);
            pixelatedImage.Source = bitmapImage;
        }

        private void LibraryButtonClick(object sender, EventArgs e)
        {
            var task = new PhotoChooserTask();
            task.Completed += OnPhotoChooserTaskCompleted;
            task.ShowCamera = true;
            task.Show();
        }

        private void OnPixelationListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            var pixelation = int.Parse(((ListBoxItem) e.AddedItems[0]).Content.ToString());
            var pixelator = new Pixelator(pixelation);
            // TODO: pixelate it!
        }
    }
}