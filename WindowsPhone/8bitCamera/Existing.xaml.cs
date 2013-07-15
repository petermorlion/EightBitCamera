using System;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
    }
}