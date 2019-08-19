using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Very basic data file
// Not optimized, and dumb, but should be fine for its task.

namespace Spotify_Lyrics.NET.API
{
    class FileSystemHelper
    {
        string localResourcesDir;
        string localResourcesData;

        public FileSystemHelper()
        {
            localResourcesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Spotify Lyrics .NET\Data";
            localResourcesData = localResourcesDir + @"\lyrics.dat";

            checkLocalResources();
        }

        private void checkLocalResources()
        {
            if (!Directory.Exists(localResourcesDir))
            {
                Directory.CreateDirectory(localResourcesDir);
            }

            if (!File.Exists(localResourcesData))
            {
                File.CreateText(localResourcesData);
            }
        }

        public List<string> getLyrics(string title)
        {
            try
            {
                title = title.ToLower().Trim();

                string[] lyricsList = File.ReadAllLines(localResourcesData);

                foreach (string ly in lyricsList)
                {
                    if (ly.Contains("="))
                    {
                        string t = ly.Substring(0, ly.IndexOf("=")).ToLower().Trim();
                               

                        if (t == title)
                        {
                            string vals = ly.Substring(ly.IndexOf("=") + 1);
                            List<string> valsArr = vals.Split(',').ToList<string>();
                            // 0 = id, 1 = coverImg, 2 = url
                            return valsArr;
                        }
                    }
                    else break;
                }
            }
            catch (Exception ex) { }
            return new List<String>();
        }

        public bool saveLyrics(string title, string id, string coverImg, string url)
        {
            try
            {
                checkLocalResources();
                File.AppendAllText(localResourcesData, title + "=" + id + "," + coverImg + "," + url + '\n');

                return true;
            } catch (Exception ex) { }
            return false;
        }

        public bool removeLyrics(string title)
        {
            try
            {
                checkLocalResources();
                string[] lyricsList = File.ReadAllLines(localResourcesData);
                string lyricsListTemp = "";
                foreach (string ly in lyricsList)
                {
                    if (ly.Contains("="))
                    {
                        string t = ly.Substring(0, ly.IndexOf("=")).ToLower().Trim();

                        if (t != title) lyricsListTemp += ly;
                    }
                    else break;
                }

                File.WriteAllText(localResourcesData, lyricsListTemp);

                return true;
            }
            catch (Exception ex) { }
            return false;
        }
    }
}
