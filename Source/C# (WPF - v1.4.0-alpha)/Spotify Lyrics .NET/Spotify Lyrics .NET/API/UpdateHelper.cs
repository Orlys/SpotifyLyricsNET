using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Lyrics.NET.API
{
    class UpdateHelper
    {
        MainWindow mainW;

        struct version
        {
            public int major;
            public int minor;
            public int patch;
            public bool stable;
            public bool alpha;
            public bool beta;

            public version(int maj, int min, int pat, bool s, bool a, bool b)
            {
                major = maj;
                minor = min;
                patch = pat;
                stable = s;
                alpha = a;
                beta = b;
            }
        }

        public UpdateHelper(ref MainWindow mw, string currentVersion)
        {
            mainW = mw;
            checkForUpdates(currentVersion);
        }

        public void checkForUpdates(string currentVersion)
        {
            string searchURL = "https://raw.githubusercontent.com/JakubSteplowski/SpotifyLyricsNET/master/version.md";
            string response = getHTTPSRequest(searchURL);

            if (isFirstNewer(stringToVersion(response), stringToVersion(currentVersion)))
            {
                Dialog diag = new Dialog(response);
            }
        }

        private bool isFirstNewer(version v1, version v2)
        {
            if (v1.major > v2.major) return true;
            if (v1.minor > v2.minor) return true;
            if (v1.patch > v2.patch) return true;
            if (v1.stable && !v2.stable) return true;
            if (v1.beta && v2.alpha) return true;
            return false;
        }

        private version stringToVersion(string version)
        {
            version v = new version(0, 0, 0, true, false, false);

            try
            {
                string[] vArr = version.Split('.');
                if (vArr.Count() == 3)
                {
                    v.alpha = vArr[2].Contains("-alpha");
                    v.beta = vArr[2].Contains("-beta") && !v.alpha;
                    v.stable = !v.alpha && !v.beta;
                    v.major = int.Parse(vArr[0].Replace("v", ""));
                    v.minor = int.Parse(vArr[1]);
                    v.patch = int.Parse(vArr[2].Replace("-alpha", "").Replace("-beta", ""));
                }
            }
            catch (Exception ex) {}

            return v;
        }

        private static string getHTTPSRequest(string strRequest)
        {
            try
            {
                WebRequest ThisRequest = WebRequest.Create(strRequest);
                ThisRequest.ContentType = "application/x-www-form-urlencoded";
                ThisRequest.Method = "GET";

                System.Text.ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
                byte[] BytesToSend = Encoder.GetBytes(strRequest);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebResponse TheirResponse = (HttpWebResponse)ThisRequest.GetResponse();

                StreamReader sr = new StreamReader(TheirResponse.GetResponseStream());
                string strResponse = sr.ReadToEnd();
                sr.Close();

                return strResponse;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
