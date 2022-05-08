using Google.Cloud.Vision.V1;
using Google.Protobuf.Collections;
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
                _available = false;
            }
        
        }

        private void ToRect(RepeatedField<Vertex> vertices , out double x, out double y, out double sizeX, out double sizeY)
        {
            int x1 = -1;
            int x2 = -1;

            int y1 = -1;
            int y2 = -1;

            //사이즈 계산
            foreach (var vert in vertices)
            {
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
                x = x1;
                sizeX = x2 - x1;
            }
            else
            {
                x = x2;
                sizeX = x1 - x2;
            }

            if (y2 > y1)
            {
                y = y1;
                sizeY = y2 - y1;
            }
            else
            {
                y = y2;
                sizeY = y1 - y2;
            }

        }

        public GoogleOcrResult GetText(byte[] stream)
        {            
            GoogleOcrResult googleResult = new GoogleOcrResult();

            try
            {
                Image img = Image.FromBytes(stream);

                TextAnnotation text = _client.DetectDocumentText(img);
              

                int index = 0;
                int wordIndex = 0;
                int lineWordCount = 0;
                string resultText = text.Text.Replace(" ", "");
                googleResult.MainText = text.Text;

                int totalWordCount = 0;
                foreach (var page in text.Pages)
                {
                    foreach (var block in page.Blocks)
                    {
                        foreach (var paragraph in block.Paragraphs)
                        {
                            totalWordCount += paragraph.Words.Count;
                        }
                    }
                }

                List<int> wordCountList = new List<int>();
                googleResult.lineCount = 0;
                googleResult.words = new string[totalWordCount];
                googleResult.x = new double[totalWordCount];
                googleResult.y = new double[totalWordCount];
                googleResult.sizeX = new double[totalWordCount];
                googleResult.sizeY = new double[totalWordCount];
                googleResult.wordsIndex = 1;

                foreach (var page in text.Pages)
                {
                    foreach (var block in page.Blocks)
                    {
                        
                        string box = string.Join(" - ", block.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                    

                        foreach (var paragraph in block.Paragraphs)
                        {
                            box = string.Join(" - ", paragraph.BoundingBox.Vertices.Select(v => $"({v.X}, {v.Y})"));
                     

                            lineWordCount = 0;
                            foreach (var word in paragraph.Words)
                            {
                                lineWordCount++;
                                string wordText = string.Join("", word.Symbols.Select(s => s.Text));

                                index += wordText.Length;

                                int positionOfNewLine = resultText.IndexOf("\n", index);

                                if(positionOfNewLine == index)
                                {
                                    googleResult.lineCount++;
                                    index++;

                                    wordCountList.Add(lineWordCount);
                                    lineWordCount = 0;
                                }

                                googleResult.words[wordIndex] = wordText;
                                double x,y , sizeX, sizeY;
                                ToRect(word.BoundingBox.Vertices, out x, out y, out sizeX, out sizeY);
                                googleResult.x[wordIndex] = x;
                                googleResult.y[wordIndex] = y;
                                googleResult.sizeX[wordIndex] = sizeX; 
                                googleResult.sizeY[wordIndex] = sizeY;

                                wordIndex++;


                            }
                        }
                    }
                }

                googleResult.wordCounts = wordCountList.ToArray();
            }
            catch (Exception e)
            {
                googleResult.isEmpty = true;
                googleResult.MainText = e.Message;
                Console.WriteLine(e.Message);
            }

            return googleResult;
        }
      
    }
}
