
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;
using System.Windows.Navigation;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace EightBitCamera
{
    public partial class MainPage : PhoneApplicationPage
    {
        private int savedCounter = 0;
        PhotoCamera _photoCamera;
        MediaLibrary mediaLibrary = new MediaLibrary();
        private Pixelator _pixelator = new Pixelator(8);

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary))
            {
                _photoCamera = new PhotoCamera(CameraType.Primary);
                _photoCamera.Initialized += OnCameraInitialized;
                _photoCamera.CaptureCompleted += OnCameraCaptureCompleted;
                _photoCamera.CaptureImageAvailable += OnCameraCaptureImageAvailable;
                _photoCamera.CaptureThumbnailAvailable += OnCameraCaptureThumbnailAvailable;
                
                viewfinderBrush.SetSource(_photoCamera);
            }
            else
            {
                // TODO: handle possibility of no camera
            }
        }

        private void OnCameraInitialized(object sender, CameraOperationCompletedEventArgs e)
        {
            // TODO: fix Timer
            int timer = 0;
            while (timer <= 100)
            {
                UpdateViewFinder(null);
                timer++;
            }
            //var timer = new Timer(UpdateViewFinder, null, 0, 1000);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_photoCamera != null)
            {
                _photoCamera.Dispose();

                _photoCamera.CaptureCompleted -= OnCameraCaptureCompleted;
                _photoCamera.CaptureImageAvailable -= OnCameraCaptureImageAvailable;
                _photoCamera.CaptureThumbnailAvailable -= OnCameraCaptureThumbnailAvailable;
            }
        }

        private void ShutterButtonClick(object sender, RoutedEventArgs e)
        {
            if (_photoCamera == null)
                return;

            _photoCamera.CaptureImage();
        }

        private void LoadThumbnails()
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate()
            {
                thumbnailsPanel.Children.Clear();
            });

            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fileNames = isolatedStorageFile.GetFileNames("*_th.jpg");
                foreach (var fileName in fileNames)
                {
                    byte[] data;
                    using (var isolatedStorageFileStream = isolatedStorageFile.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {
                        data = new byte[isolatedStorageFileStream.Length];
                        isolatedStorageFileStream.Read(data, 0, data.Length);
                        isolatedStorageFileStream.Close();
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(delegate()
                    {
                        var memoryStream = new MemoryStream(data);
                        var bitmap = new BitmapImage();
                        bitmap.SetSource(memoryStream);
                        var image = new Image();
                        image.Height = 24;
                        image.Width = 24;
                        image.Source = bitmap;
                        thumbnailsPanel.Children.Add(image);
                    });
                }
            }
        }

        private void UpdateViewFinder(object state)
        {
            var width = (int) _photoCamera.PreviewResolution.Width;
            var height = (int) _photoCamera.PreviewResolution.Height;
            int max = width * height;
            int[] buffer = new int[max];
            _photoCamera.GetPreviewBufferArgb32(buffer);

            buffer = _pixelator.Pixelate(buffer, width, height);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              var wb = new WriteableBitmap(width, height);
                                                              bitPreview.Source = wb;
                                                              buffer.CopyTo(wb.Pixels, 0);
                                                              wb.Invalidate();
                                                          });
        }

        private void OnCameraCaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            var fileName = "8bitImage" + savedCounter + ".jpg";
            try
            {
                mediaLibrary.SavePictureToCameraRoll(fileName, e.ImageStream);
                e.ImageStream.Seek(0, SeekOrigin.Begin);
                using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var targetStream = isolatedStorageFile.OpenFile(fileName, FileMode.Create, FileAccess.Write))
                    {
                        var readBuffer = new byte[4096];
                        var bytesRead = -1;

                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }
            }
            finally
            {
                e.ImageStream.Close();
            }
        }

        private void OnCameraCaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            savedCounter++;
        }

        private void OnCameraCaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            var fileName = "8bitImage" + savedCounter + "_th.jpg";
            try
            {
                using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var targetStream = isolatedStorageFile.OpenFile(fileName, FileMode.Create, FileAccess.Write))
                    {
                        byte[] readBuffer = new byte[4096];
                        int bytesRead = -1;

                        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            targetStream.Write(readBuffer, 0, bytesRead);
                        }
                    }
                }
                LoadThumbnails();
            }
            finally
            {
                e.ImageStream.Close();
            }
        }
    }
}