using System;
using System.IO;
using System.Threading.Tasks;

namespace _02_SliceFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter video file path (including the filename in it): ");
            string videoPath = Console.ReadLine();

            Console.Write("Enter destination path: ");
            string destinationPath = Console.ReadLine();

            Console.Write("On how many pieces to slice the file: ");
            int parts = int.Parse(Console.ReadLine());

            SliceAsync(videoPath, destinationPath, parts);

            Console.WriteLine("Anything else?");
            while (true) Console.ReadLine();
        }

        private static void SliceAsync(string videoPath, string destinationPath, int parts)
        {
            Task.Run(() => Slice(videoPath, destinationPath, parts));
        }

        static void Slice(string videoPath, string destinationPath, int parts)
        {
            if(!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using (var reader = new FileStream(videoPath, FileMode.Open))
            {
                var fileInfo = new FileInfo(videoPath);

                long partLength = (reader.Length / parts) + 1;
                long currentByte = 0;

                for (int currentPart = 1; currentPart <= parts; currentPart++)
                {
                    string filePath = string.Format("{0}/Part-{1}{2}", destinationPath, currentPart, fileInfo.Extension);

                    using (var slicedFile = new FileStream(filePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[4096];

                        while(currentByte <= partLength * currentPart)
                        {
                            int transferBytes = reader.Read(buffer, 0, buffer.Length);

                            if (transferBytes == 0) break;

                            slicedFile.Write(buffer, 0, transferBytes);

                            currentByte += transferBytes;
                        }
                    }
                }

                Console.WriteLine("Slice complete.");
            }
        }

    }
}
