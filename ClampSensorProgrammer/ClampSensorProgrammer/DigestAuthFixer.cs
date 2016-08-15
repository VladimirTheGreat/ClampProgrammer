using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClampSensorProgrammer
{
    class DigestAuthFixer
    {
        private static string _host;
        private static string _user;
        private static string _password;
        private static string _realm;
        private static string _nonce;
        private static string _opaque;
        private static string _qop;
        private static string _cnonce;
        private static DateTime _cnonceDate;
        private static int _nc;

        public DigestAuthFixer(string host, string user, string password)
        {
            // TODO: Complete member initialization
            _host = host;
            _user = user;
            _password = password;
        }

        private string CalculateMd5Hash(
            string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private string GrabHeaderVar(
            string varName,
            string header)
        {
            var regHeader = new Regex(string.Format(@"{0}=""([^""]*)""", varName));
            var matchHeader = regHeader.Match(header);
            if (matchHeader.Success)
                return matchHeader.Groups[1].Value;
            throw new ApplicationException(string.Format("Header {0} not found", varName));
        }

        private string GetDigestHeader(
            string dir)
        {
            _nc = _nc + 1;

            var ha1 = CalculateMd5Hash(string.Format("{0}:{1}:{2}", _user, _realm, _password));
            var ha2 = CalculateMd5Hash(string.Format("{0}:{1}", "GET", dir));
            var digestResponse =
                CalculateMd5Hash(string.Format("{0}:{1}:{2:00000000}:{3}:{4}:{5}", ha1, _nonce, _nc, _cnonce, _qop, ha2));

            return string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", " +
                "algorithm=MD5, response=\"{4}\", qop={5}, nc={6:00000000}, cnonce=\"{7}\", opaque=\"{8}\"",
                _user, _realm, _nonce, dir, digestResponse, _qop, _nc, _cnonce, _opaque);
        }

        public string GrabResponse(
            string dir)
        {
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var url = _host + dir;
            var uri = new Uri(url);

            var request = (HttpWebRequest)WebRequest.Create(uri);

            // If we've got a recent Auth header, re-use it!
            if (!string.IsNullOrEmpty(_cnonce) &&
                DateTime.Now.Subtract(_cnonceDate).TotalHours < 1.0)
            {
                request.Headers.Add("Authorization", GetDigestHeader(dir));
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                // Try to fix a 401 exception by adding a Authorization header
                if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    throw;

                var wwwAuthenticateHeader = ex.Response.Headers["WWW-Authenticate"];
                _realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
                _nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
                _qop = GrabHeaderVar("qop", wwwAuthenticateHeader);
                _opaque = GrabHeaderVar("opaque", wwwAuthenticateHeader);

                _nc = 0;
                _cnonce = new Random().Next(123400, 9999999).ToString();
                _cnonceDate = DateTime.Now;

                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request2 = (HttpWebRequest)WebRequest.Create(uri);
                request2.Headers.Add("Authorization", GetDigestHeader(dir));
                response = (HttpWebResponse)request2.GetResponse();
            }
            var reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }

        public byte[] GrabBytesResponse(
            string dir)
        {
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var url = _host + dir;
            var uri = new Uri(url);

            var request = (HttpWebRequest)WebRequest.Create(uri);

            // If we've got a recent Auth header, re-use it!
            if (!string.IsNullOrEmpty(_cnonce) &&
                DateTime.Now.Subtract(_cnonceDate).TotalHours < 1.0)
            {
                request.Headers.Add("Authorization", GetDigestHeader(dir));
            }

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                // Try to fix a 401 exception by adding a Authorization header
                if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    throw;

                var wwwAuthenticateHeader = ex.Response.Headers["WWW-Authenticate"];
                _realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
                _nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
                _qop = GrabHeaderVar("qop", wwwAuthenticateHeader);
                _opaque = GrabHeaderVar("opaque", wwwAuthenticateHeader);

                _nc = 0;
                _cnonce = new Random().Next(123400, 9999999).ToString();
                _cnonceDate = DateTime.Now;

                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request2 = (HttpWebRequest)WebRequest.Create(uri);
                request2.Headers.Add("Authorization", GetDigestHeader(dir));

                try
                {
                    response = (HttpWebResponse)request2.GetResponse();
                }
                catch (WebException we)
                {
                    if (we.Response != null && ((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.NotModified)
                    {
                        throw new FileNotFoundException();
                    }

                    throw new WebException();
                }
            }
            var reader = response.GetResponseStream();

            MemoryStream ms = new MemoryStream();
            reader.CopyTo(ms);

            byte[] message = ms.ToArray();

            MD5 chk = MD5.Create();
            byte[] my_md5 = chk.ComputeHash(message);

            byte[] server_md5 = new byte[16];

            for (int i = 0; i < 32; ++i)
            {
                string md5_header = response.GetResponseHeader("MD5");

                server_md5[i / 2] <<= 4;

                if (Convert.ToByte(md5_header[i]) >= Convert.ToByte('a') && Convert.ToByte(md5_header[i]) <= 'f')
                {
                    server_md5[i / 2] += (byte)(md5_header[i] - Convert.ToByte('a') + 10);
                }
                else if (md5_header[i] >= Convert.ToByte('A') && md5_header[i] <= 'F')
                {
                    server_md5[i / 2] += (byte)(md5_header[i] - Convert.ToByte('A') + 10);
                }
                else
                {
                    server_md5[i / 2] += (byte)(md5_header[i] - Convert.ToByte('0'));
                }
            }

            for (int i = 0; i < 16; ++i)
            {
                if (my_md5[i] != server_md5[i])
                {
                    throw new InvalidDataException();
                }
            }

            return message;
        }
    }
}
