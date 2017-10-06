using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmotionRecognition
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var camera = new CameraCaptureUI();
            var photo = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            var stream = await photo.OpenAsync(Windows.Storage.FileAccessMode.Read);

            var emotionClient = new EmotionServiceClient("74dde28011f64a94bb0a1a0ecc8b628a");
            var emotions = await emotionClient.RecognizeAsync(stream.AsStream());

            var result = emotions.FirstOrDefault();

            if (result != null && result.Scores.Happiness > 0.7)
            {
                tbSmiley.Text = ":-)";
            }
            else
            {
                tbSmiley.Text = ":-(";
            }
        }
    }
}
