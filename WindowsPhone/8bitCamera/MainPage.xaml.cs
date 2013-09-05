using System;
using System.Windows;
using EightBitCamera.Data.Commands;
using EightBitCamera.Data.Queries;
using Microsoft.Phone.Controls;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Media.Imaging;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace EightBitCamera
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhotoCamera _photoCamera;
        // TODO: performance penalty when keeping reference to MediaLibrary?
        readonly MediaLibrary _mediaLibrary = new MediaLibrary();
        private Pixelator _pixelator;
        private bool _cameraCaptureInProgress;
        private WriteableBitmap _wb;
        private bool _isCameraInitialized;

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
            _isCameraInitialized = false;

            OnOrientationChanged(this, new OrientationChangedEventArgs(Orientation));
            
            if (PhotoCamera.IsCameraTypeSupported(CameraType.Primary))
            {
                _pixelator = new Pixelator(new PixelationSizeQuery().Get(), true);

                _photoCamera = new PhotoCamera(CameraType.Primary);
                _photoCamera.Initialized += OnCameraInitialized;
                _photoCamera.CaptureImageAvailable += OnCameraCaptureImageAvailable;
                _photoCamera.CaptureCompleted += OnCameraCaptureCompleted;
                _photoCamera.CaptureStarted += OnCameraCaptureStarted;

                ViewfinderBrush.SetSource(_photoCamera);
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

        private void OnCameraInitialized(object sender, CameraOperationCompletedEventArgs e)
        {
            _isCameraInitialized = true;
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
            }
        }

        private void UpdateViewFinder(object state)
        {
            if (_cameraCaptureInProgress || !_isCameraInitialized)
                return;

            var width = (int)_photoCamera.PreviewResolution.Width;
            var height = (int)_photoCamera.PreviewResolution.Height;
            int max = width * height;
            int[] buffer = new int[max];

            //TODO: just before GetPreviewBufferArgb32 to avoid exception, but would be better in beginning of method only. Necesary because of not working with Timer?
            try
            {
                _photoCamera.GetPreviewBufferArgb32(buffer);

                buffer = _pixelator.Pixelate(buffer, width, height);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _wb = new WriteableBitmap(width, height);
                    BitPreview.ImageSource = _wb;
                    buffer.CopyTo(_wb.Pixels, 0);
                    _wb.Invalidate();
                });
            }
            catch (Exception)
            {
                // TODO: catch more specific exception if possible  
            }
        }

        private void OnCameraCaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            var newSaveCounter = new SaveCounterQuery().Get() + 1;
            var fileName = "PixImg_" + newSaveCounter + ".jpg";
            try
            {
                var saveOriginalToCameraRoll = new SaveOriginalToCameraRollQuery().Get();
                var stream = new MemoryStream();
                _wb.SaveJpeg(stream, _wb.PixelWidth, _wb.PixelHeight, 0, 100);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                              {
                                                                  var rotatedStream = RotateStream(stream);

                                                                   if (saveOriginalToCameraRoll)
                                                                  {
                                                                      _mediaLibrary.SavePictureToCameraRoll(fileName, e.ImageStream);
                                                                  }

                                                                  _mediaLibrary.SavePicture(fileName, rotatedStream.ToArray());

                                                                  new ShowSavedMessageCommand().Show();
                                                              });
                
            }
            catch (Exception exception)
            {
                // TODO: do something with exception?
            }

            new SaveCounterCommand().Set(newSaveCounter);
        }

        private MemoryStream RotateStream(MemoryStream stream)
        {
            var angle = 0;
            switch (Orientation)
            {
                case PageOrientation.PortraitUp:
                    angle = 90;
                    break;
                case PageOrientation.LandscapeRight:
                    angle = 180;
                    break;
            }

            if (angle == 0)
                return stream;

            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);

            WriteableBitmap wbTarget = null;
            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            return targetStream;
        }

        private void SettingsButtonClick(object sender, EventArgs eventArgs)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void LibraryButtonClick(object sender, EventArgs eventArgs)
        {
            NavigationService.Navigate(new Uri("/Existing.xaml", UriKind.Relative));
        }

        private void OnBitPreviewTap(object sender, GestureEventArgs e)
        {
            if (_photoCamera == null)
            {
                return;
            }

            _photoCamera.Focus();
            // TODO: wait for focus to be completed?
            _photoCamera.CaptureImage();
        }

        private void OnOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            switch (e.Orientation)
            {
                case PageOrientation.PortraitUp:
                    ViewFinderTransform.Rotation = 90;
                    break;
                case PageOrientation.PortraitDown:
                    ViewFinderTransform.Rotation = 270;
                    break;
                case PageOrientation.LandscapeRight:
                    ViewFinderTransform.Rotation = 180;
                    break;
                default:
                    ViewFinderTransform.Rotation = 0;
                    break;
            }
        }

        private void ShareButtonClick(object sender, EventArgs e)
        {
            var twitterUserQuery = new TwitterUserQuery();
            var twitterUser = twitterUserQuery.Get();
            if (twitterUser == null)
            {
                NavigationService.Navigate(new Uri("/TwitterAuthentication.xaml", UriKind.Relative));
                return;
            }

            NavigationService.Navigate(new Uri("/TweetPhoto.xaml", UriKind.Relative));
        }

        private void AboutMenuItemClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}