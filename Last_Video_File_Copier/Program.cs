using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

//ref link:https://www.youtube.com/watch?v=9qrwqR5oypw&list=PLAIBPfq19p2EJ6JY0f5DyQfybwBGVglcR&index=54
//personalized console app which copies the last shot video from my camera into my videos directory. 

namespace Last_Video_File_Copier
{
    class Program
    {
        static void Main(string[] args)
        {
            DriveInfo cameraDrive = GetCameraDrive();

            if (cameraDrive == null)
            {
                ShowExitMessage("Could not find camera drive!");
            }

            string videoDir = GetVideoFileDirectory(cameraDrive);

            if (videoDir == null)
            {
                ShowExitMessage("Could not find video file folder!");
            }

            string[] paths = Directory.GetFiles(videoDir);

            if (paths.Length == 0)
            {
                ShowExitMessage("No video files found!");
            }

            string latestVideoFilePath = GetLatestVideoFilePath(paths);

            if (latestVideoFilePath == null)
            {
                ShowExitMessage("Could not recognize present files as videos!");
            }

            try
            {
                string dest = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                dest += "\\" + Path.GetFileName(latestVideoFilePath);

                File.Copy(latestVideoFilePath, dest);
            }
            catch (Exception ex)
            {
                ShowExitMessage(ex.Message);
            }
        }

        private static void ShowExitMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
            Environment.Exit(0);
        }

        private static DriveInfo GetCameraDrive()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.VolumeLabel.ToLower() == "cam_mem")
                {
                    return drive;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the directory for the video files on the specified camera drive
        /// </summary>
        /// <returns>Return null if no directory found</returns>
        private static string GetVideoFileDirectory(DriveInfo cameraDrive)
        {
            string videoFolder = cameraDrive.RootDirectory + @"\AVCHD\BDMV\STREAM"; // Video File Path 

            if (Directory.Exists(videoFolder))
            {
                return videoFolder;
            }
            else
            {
                return null;
            }
        }


        /// <returns>Returns an empy string if no file paths could be parsed</returns>
        private static string GetLatestVideoFilePath(string[] filePaths)
        {
            int highestIndex = -1;
            //string latestVideoFilePath = string.Empty;
            string latestVideoFilePath = null;

            foreach (string path in filePaths)
            {
                int index = 0;
                bool success = int.TryParse(Path.GetFileNameWithoutExtension(path), out index);

                if (success && index > highestIndex)
                {
                    highestIndex = index;
                    latestVideoFilePath = path;
                }
            }

            return latestVideoFilePath;
        }
    }
}
