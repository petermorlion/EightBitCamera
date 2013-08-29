using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using EightBitCamera.Data;
using EightBitCamera.Data.Queries;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;

namespace EightBitCamera
{
    public partial class TweetPhoto : PhoneApplicationPage
    {
        private readonly RestClient _client;
        private readonly OAuthCredentials _credentials;

        public TweetPhoto()
        {
            InitializeComponent();

            var twitterUser = new TwitterUserQuery().Get();

            _credentials = new OAuthCredentials
            {
                Type = OAuthType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Token = twitterUser.AccessToken,
                TokenSecret = twitterUser.AccessTokenSecret,
                Version = TwitterSettings.OAuthVersion,
            };

            _client = new RestClient
            {
                Authority = "http://api.twitter.com",
                HasElevatedPermissions = true
            };

            var mediaLibrary = new MediaLibrary();
            var latestPicture = mediaLibrary.Pictures.Where(x => x.Name.Contains("PixImg_")).OrderByDescending(x => x.Date).FirstOrDefault();
            if (latestPicture == null)
            {
                // TODO handle case where no pictures have been taken yet
            }

            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(latestPicture.GetImage());
            Image.Source = bitmapImage;
        }

        private void OnTweetButtonClicked(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest
            {
                Credentials = _credentials,
                Path = "/1.1/statuses/update.json",
                Method = WebMethod.Post
            };

            request.AddParameter("status", TweetTextBox.Text);

            _client.BeginRequest(request, new RestCallback(NewTweetCompleted));
        }

        private void NewTweetCompleted(RestRequest request, RestResponse response, object userstate)
        {
            // We want to ensure we are running on our thread UI
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (NavigationService.CanGoBack)
                    {
                        NavigationService.GoBack();
                    }
                    else
                    {
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    }
                }
                else
                {
                    MessageBox.Show("There was an error connecting to Twitter");
                }
            });
        }

        private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }
    }
}