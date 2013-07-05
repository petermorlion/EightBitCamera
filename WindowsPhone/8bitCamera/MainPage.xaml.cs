
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
        private int _savedCounter = 0;
        PhotoCamera _photoCamera;
        readonly MediaLibrary mediaLibrary = new MediaLibrary();
        private readonly Pixelator _pixelator = new Pixelator(6);
        private bool _cameraCaptureInProgress;

        public MainPage()
        {
            InitializeComponent();
            CameraButtons.ShutterKeyHalfPressed += OnCameraButtonShutterKeyHalfPressed;
            CameraButtons.ShutterKeyPressed += OnCameraButtonShutterKeyPressed;
        }

        private void OnCameraButtonShutterKeyPressed(object sender, EventArgs e)
        {
            if (_photoCamera == null)
                return;

            _photoCamera.CaptureImage();
        }

        private void OnCameraButtonShutterKeyHalfPressed(object sender, EventArgs e)
        {
            if (_photoCamera == null)
                return;

            try
            {
                _photoCamera.Focus();
            }
            catch (Exception)
            {
                //TODO: catch correct exception (if it exists)
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary))
            {
                _photoCamera = new PhotoCamera(CameraType.Primary);
                _photoCamera.Initialized += OnCameraInitialized;
                _photoCamera.CaptureImageAvailable += OnCameraCaptureImageAvailable;
                _photoCamera.CaptureThumbnailAvailable += OnCameraCaptureThumbnailAvailable;
                _photoCamera.CaptureCompleted += OnCameraCaptureCompleted;
                _photoCamera.CaptureStarted += OnCameraCaptureStarted;

                viewfinderBrush.SetSource(_photoCamera);
            }
            else
            {
                // TODO: handle possibility of no camera
            }
        }

        private void OnCameraCaptureStarted(object sender, EventArgs e)
        {
            _cameraCaptureInProgress = true;
        }

        private void OnCameraCaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            _cameraCaptureInProgress = false;
        }

        private void ShutterButtonClick(object sender, RoutedEventArgs e)
        {
            if (_photoCamera == null)
                return;

            _photoCamera.CaptureImage();
        }


        private void OnCameraInitialized(object sender, CameraOperationCompletedEventArgs e)
        {
            // TODO: fix Timer
            int timer = 0;
            while (true)
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

                _photoCamera.CaptureImageAvailable -= OnCameraCaptureImageAvailable;
                _photoCamera.CaptureThumbnailAvailable -= OnCameraCaptureThumbnailAvailable;
            }
        }

        private void UpdateViewFinder(object state)
        {
            if (_cameraCaptureInProgress)
                return;

            var width = (int)_photoCamera.PreviewResolution.Width;
            var height = (int)_photoCamera.PreviewResolution.Height;
            int max = width * height;
            int[] buffer = new int[max];

            //TODO: just before GetPreviewBufferArgb32 to avoid exception, but would be better in beginning of method only. Necesary because of not working with Timer?
            if (_cameraCaptureInProgress)
                return;

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
            var fileName = "8bitImage" + _savedCounter + ".jpg";
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

        private void OnCameraCaptureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            var fileName = "8bitImage" + _savedCounter + "_th.jpg";
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
            }
            finally
            {
                e.ImageStream.Close();
            }
        }

        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }
    }
}