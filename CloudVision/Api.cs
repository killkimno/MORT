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

        //TODO : 에러가 떴을 경우 예외처리 필요
        private bool _available;
        public bool Available => _available;
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
                    Console.WriteLine($"Description: {text.Description} // {text.Locations} // {text}");
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

                _available = true;
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
                    GoogleOcrResult data = new GoogleOcrResult();
                    data.Text = text.Description;

                    int x1 = -1;
                    int x2 = -1;

                    int y1 = -1;
                    int y2 = -1;

                    //사이즈 계산
                    foreach (var vert in text.BoundingPoly.Vertices)
                    {
                        Console.WriteLine(vert);
                        
                        if(x1 == -1)
                        {
                            x1 = vert.X;
                        }
                        else if(x1 !=  vert.X)
                        {
                            x2 = vert.X;
                        }

                        if(y1 == -1)
                        {
                            y1 = vert.Y;
                        }
                        else if(y1 != vert.Y)
                        {
                            y2 = vert.Y;
                        }
                    }

                    if(x2 == -1)
                    {
                        x2 = x1;
                    }

                    if(y2 == -1)
                    {
                        y2 = x1;
                    }

                    if(x2 > x1)
                    {
                        data.X = x1;
                        data.SizeX = x2 - x1;
                    }
                    else
                    {
                        data.X = x2;
                        data.SizeX = x1 - x2;
                    }

                    if(y2 > y1)
                    {
                        data.Y = y1;
                        data.SizeY = y2 - y1;
                    }
                    else
                    {
                        data.Y = y2;
                        data.SizeY = y1 - y2;
                    }

                    Console.WriteLine($"Size : {data.X} / {data.Y} / {data.SizeX} / {data.SizeY}");
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public List<GoogleOcrResult> GetText2(byte[] stream)
        {
            List<GoogleOcrResult> list = new List<GoogleOcrResult>();

            bool first = true;
            string result = "";
            try
            {
                Image img = Image.FromBytes(stream);

                Console.WriteLine("----------------------");
                IReadOnlyList<EntityAnnotation> textAnnotations = _client.DetectText(img);

                Console.WriteLine($" line = {textAnnotations.Count}");
                foreach (EntityAnnotation text in textAnnotations)
                {
                    GoogleOcrResult data = new GoogleOcrResult();
                    data.Main = first;

                    first = false;
                    data.Text = text.Description;

                    int x1 = -1;
                    int x2 = -1;

                    int y1 = -1;
                    int y2 = -1;

                    //사이즈 계산
                    foreach (var vert in text.BoundingPoly.Vertices)
                    {
                        Console.WriteLine(vert);

                        if (x1 == -1)
                        {
                            x1 = vert.X;
                        }
                        else if (x1 != vert.X)
                        {
                            x2 = vert.X;
                        }

                        if (y1 == -1)
                        {
                            y1 = vert.Y;
                        }
                        else if (y1 != vert.Y)
                        {
                            y2 = vert.Y;
                        }
                    }

                    if (x2 == -1)
                    {
                        x2 = x1;
                    }

                    if (y2 == -1)
                    {
                        y2 = x1;
                    }

                    if (x2 > x1)
                    {
                        data.X = x1;
                        data.SizeX = x2 - x1;
                    }
                    else
                    {
                        data.X = x2;
                        data.SizeX = x1 - x2;
                    }

                    if (y2 > y1)
                    {
                        data.Y = y1;
                        data.SizeY = y2 - y1;
                    }
                    else
                    {
                        data.Y = y2;
                        data.SizeY = y1 - y2;
                    }

                    list.Add(data);
                    result += text.Description;
                  // Console.WriteLine($"Description: {text.Description} // {text.} // {text}");
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                Console.WriteLine(e.Message);
            }

            return list;
        }
    }
}
