using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using EightBitCamera.Data.Commands;
using EightBitCamera.Data.Queries;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;

namespace EightBitCamera
{
    public partial class Existing : PhoneApplicationPage
    {
        private Pixelator _pixelator;
        private WriteableBitmap _writableBitmap;
        private int[] _originalPixels;

        public Existing()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (pixelatedImage.Source == null && e.IsNavigationInitiator)
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

            var pixelateSize = new PixelationSizeQuery().Get();
            pixelationListBox.SelectedItem = pixelateSize; // TODO: don't work
            pixelationListBox.SelectionChanged += OnPixelationListBoxSelectionChanged;
            _pixelator = new Pixelator(pixelateSize, false);
        }

        private void OnPhotoChooserTaskCompleted(object sender, PhotoResult e)
        {
            var stream = e.ChosenPhoto;
            if (stream == null)
            {
                // If connected to Zune and debugging (WP7), the PhotoChooserTask won't open so we just pick the first image available.
#if DEBUG
                var mediaLibrary = new MediaLibrary();
                if (mediaLibrary.SavedPictures.Count > 0)
                {
                    var firstPicture = mediaLibrary.SavedPictures[0];
                    stream = firstPicture.GetImage();
                }
#else
                return;
#endif
            }

            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);

            //TODO: close stream?
            
            _writableBitmap = new WriteableBitmap(bitmapImage);
            pixelatedImage.Source = _writableBitmap;
            _originalPixels = new int[_writableBitmap.Pixels.Length];
            _writableBitmap.Pixels.CopyTo(_originalPixels, 0);
            
            PixelateWriteableBitmap();
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
            if (e.AddedItems.Count == 0 || _writableBitmap == null)
            {
                return;
            }

            var pixelateSize = int.Parse(((ListBoxItem)e.AddedItems[0]).Content.ToString());
            _pixelator.PixelateSize = pixelateSize;
            PixelateWriteableBitmap();
        }

        private void PixelateWriteableBitmap()
        {
            if (_originalPixels == null || _writableBitmap == null)
            {
                return;
            }

            var pixelated = _pixelator.Pixelate(_originalPixels, _writableBitmap.PixelWidth, _writableBitmap.PixelHeight);
            pixelated.CopyTo(_writableBitmap.Pixels, 0);
            _writableBitmap.Invalidate();
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            if (_writableBitmap == null)
            {
                MessageBox.Show("Please select an image from your library.", "No image selected", MessageBoxButton.OK);
                return;
            }

            var newSaveCounter = new SaveCounterQuery().Get() + 1;
            var fileName = "PixImg_" + newSaveCounter + ".jpg";

            var stream = new MemoryStream();
            _writableBitmap.SaveJpeg(stream, _writableBitmap.PixelWidth, _writableBitmap.PixelHeight, 0, 100);

            var mediaLibrary = new MediaLibrary();
            mediaLibrary.SavePicture(fileName, stream.ToArray());

            new ShowSavedMessageCommand().Show();
        }
    }
}