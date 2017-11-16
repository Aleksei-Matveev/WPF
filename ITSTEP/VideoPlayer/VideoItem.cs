using System;
using System.IO;

namespace VideoPlayer
{
    public class VideoItem
    {
        public VideoItem(string filePath)
        {
            NameFile = Path.GetFileName(filePath);
            FilePath = filePath;           
        }
        public string FilePath { get; set; }

        public override string ToString()
        {
            return NameFile;
        }
        public string NameFile { get; set; }

        public DateTime TotalTime { get; set; }

        public string ShowTime
        {
            get
            {
                return String.Format("{0:HH:mm:ss}", TotalTime);
            }
        }
    }
}
