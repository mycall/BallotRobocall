using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace robocall.Services
{
    public class WebClient
    {
        public static Guid Authenticate(string Url, string Username, string Password)
        {
            var result = new Guid();
            try
            {
                var postBody = string.Format("Username={0}&Password={1}", Username, Password);
                var postPage = Request(Url,
                    postBody,
                    "POST",
                    new NameValueCollection { });
                result = GetUserID(StreamToString(postPage.Body));

            }
            catch (Exception e)
            {

            }
            return result;
        }

        public class Response
        {
            public HttpStatusCode StatusCode { get; set; }
            public string StatusDescription { get; set; }
            public string RequestVerificationToken { get; set; }
            public NameValueCollection FedAuth { get; set; }
            public MemoryStream Body { get; set; }

            public Response()
            {
                FedAuth = new NameValueCollection();
            }
        }

        public static Response Request(string Url, string Body, string Method, NameValueCollection Headers = null)
        {
            var result = new Response();
            ServicePointManager.ServerCertificateValidationCallback = (MyCertValidationCb);

            var req = (HttpWebRequest)WebRequest.Create(Url);
            req.Proxy = null;
            req.Method = Method.ToUpper();
            req = AddHeaders(req, Headers);
            if (req.Method == "POST")
            {
                var payload = Body;
                req.ContentLength = payload.Length;
                req.ContentType = "application/x-www-form-urlencoded";

                var output = new StreamWriter(req.GetRequestStream());
                output.Write(payload);
                output.Close();
            }

            var res = req.GetResponse() as HttpWebResponse;
            if (res != null)
            {
                result.StatusCode = res.StatusCode;
                result.StatusDescription = res.StatusDescription;
                if (res.SupportsHeaders)
                    foreach (var name in res.Headers.AllKeys)
                    {
                        var value = res.Headers[name].Trim();
                        if (name == "Set-Cookie")
                        {
                            if (value.StartsWith("__RequestVerificationToken"))
                                result.RequestVerificationToken = value.Substring(0, value.IndexOf(";"));
                            else if (value.StartsWith("FedAuth"))
                                result.FedAuth.Add(value.Substring(0, value.IndexOf("=")), value.Substring(value.IndexOf("=") + 1));
                        }

                    }


                var buffer = new byte[res.ContentLength];
                var stream = res.GetResponseStream();
                if (stream != null)
                    using (var reader = new BinaryReader(stream))
                    {
                        buffer = reader.ReadBytes(buffer.Length);
                    }
                res.Close();
                result.Body = new MemoryStream(buffer);
            }
            return result;
        }

        public static string StreamToString(MemoryStream stream)
        {
            if (stream == null) return null;
            var sr = new StreamReader(stream, Encoding.UTF8);
            return sr.ReadToEnd();
        }

        public static bool MyCertValidationCb(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors)
                return false;
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                var z = Zone.CreateFromUrl(((HttpWebRequest)sender).RequestUri.ToString());
                return z.SecurityZone == System.Security.SecurityZone.Intranet || z.SecurityZone == System.Security.SecurityZone.MyComputer;
            }
            return true;
        }

        public static HttpWebRequest AddHeaders(HttpWebRequest request, NameValueCollection Headers)
        {
            if (Headers == null) return request;
            foreach (var name in Headers.AllKeys)
            {
                var value = Headers[name];
                switch (name.ToLower())
                {
                    case "accept":
                        request.Accept = value;
                        break;
                    case "connection":
                        if (value.ToLower() == "keep-alive")
                            request.KeepAlive = true;
                        else
                            request.Connection = value;
                        break;
                    case "content-length":
                        request.ContentLength = Convert.ToInt32(value);
                        break;
                    case "content-type":
                        request.ContentType = value;
                        break;
                    case "date":
                        request.Date = Convert.ToDateTime(value);
                        break;
                    case "expect":
                        request.Expect = value;
                        break;
                    case "host":
                        request.Host = value;
                        break;
                    case "if-modified-since":
                        request.IfModifiedSince = Convert.ToDateTime(value);
                        break;
                    case "referer":
                        request.Referer = value;
                        break;
                    case "transfer-encoding":
                        request.TransferEncoding = value;
                        break;
                    case "user-agent":
                        request.UserAgent = value;
                        break;

                    default:
                        request.Headers.Add(name, value);
                        break;
                }
            }
            return request;
        }

        public static Guid GetUserID(string Body)
        {
            var re = new Regex(@"{""UserID"":""(.*?)""}");
            return new Guid(re.IsMatch(Body) ? re.Match(Body).Groups[1].Value : null);
        }
    }
}
