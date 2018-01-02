using Microsoft.ProjectOxford.Emotion;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmotionApi
{
    class Program
    {
        static async Task Run()
        {
            var emotionService = new EmotionServiceClient("524b2093a8d84cddae93714a9d8edd96");
            var emotions = await emotionService.RecognizeAsync(File.OpenRead(@"C:\Code\GitHub\msft-intelligent-app-workshop\Exercises\exercise6-intelligence\images\rainer.jpg"));
            if (emotions.FirstOrDefault().Scores.Happiness > 0.99)
            {
                Console.WriteLine(":-)");
            }
        }

        static void Main(string[] args)
        {
            Run().Wait();
        }
    }
}
