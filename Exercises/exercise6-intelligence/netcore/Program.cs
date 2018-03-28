using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmotionApi
{
    class Program
    {
        static async Task Run()
        {
            var emotionService = new FaceServiceClient("fd9e1c352a6a42eaa06e1babc9ce64b1", "https://northeurope.api.cognitive.microsoft.com/face/v1.0");
            var emotions = await emotionService.DetectAsync(
                File.OpenRead(@"C:\Code\GitHub\msft-intelligent-app-workshop\Exercises\exercise6-intelligence\images\rainer.jpg"),
                returnFaceAttributes: new [] { FaceAttributeType.Smile });
            if (emotions.FirstOrDefault().FaceAttributes.Smile > 0.99)
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
