using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudVision
{
    public class Api
    {
        private ImageAnnotatorClient _client;
        public void Test()
        {
            try
            {
                Image img = Image.FromFile("./test.png");
                string cardi = File.ReadAllText("./test.json");

                ImageAnnotatorClient client = new ImageAnnotatorClientBuilder
                {
                    JsonCredentials = cardi
                }.Build();


                Console.WriteLine("----------------------");
                IReadOnlyList<EntityAnnotation> textAnnotations = client.DetectText(img);

                Console.WriteLine($" line = {textAnnotations.Count}");
                foreach (EntityAnnotation text in textAnnotations)
                {
                    Console.WriteLine($"Description: {text.Description}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void InitClient(string path)
        {
            try
            {
                string cardi = File.ReadAllText(path);

                _client = new ImageAnnotatorClientBuilder
                {
                    JsonCredentials = cardi
                }.Build();
            }
            catch
            {

            }
        
        }

        public string GetText(byte[] stream)
        {
            string result = "";
            try
            {
                Image img = Image.FromBytes(stream);

                Console.WriteLine("----------------------");
                IReadOnlyList<EntityAnnotation> textAnnotations = _client.DetectText(img);

                Console.WriteLine($" line = {textAnnotations.Count}");
                foreach (EntityAnnotation text in textAnnotations)
                {
                    result += text.Description;
                    Console.WriteLine($"Description: {text.Description}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
}
