using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Oleg.Kleyman.Sandbox.Javascription.Serialization
{
    public class UtorrentWebRequest
    {
        public UtorrentResponse MakeRequest(string url, string userName, string password, string cookie)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            var authInfo = string.Format("{0}:{1}", userName, password);
            var jar = new CookieContainer();
            
            request.CookieContainer = jar;

            if(cookie != null)
            {
                jar.Add(new Uri(url), new Cookie("GUID", cookie));
            }
            //request.Headers["Authorization"] = string.Format("Basic {0}", Convert.ToBase64String(Encoding.Default.GetBytes(authInfo)));
            request.Credentials = new NetworkCredential(userName, password);
            var response = (HttpWebResponse)request.GetResponse();
            using(var reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                var resp = new UtorrentResponse();
                resp.Text = reader.ReadToEnd();
                resp.Cookie = jar.GetCookies(request.RequestUri)["GUID"].Value;
                return resp;
            }
        }
    }
}
