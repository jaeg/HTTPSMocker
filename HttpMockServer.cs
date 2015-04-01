using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Windows;


namespace HTTPSMocker  
{
    class HttpMockServer
    {
        string port;
        public HttpListener listener = new HttpListener();
        public HttpMockServer(string port, List<string> prefixes)
        {

            this.port = port;

            foreach (string prefix in prefixes)
            {
                string url = prefix + ":" + port + "/";
                listener.Prefixes.Add(url);
                MainWindow.that.AddLog("Added URL to listener: " + url);
            }
        }


        public void Start()
        {

            //listener.Prefixes.Add("http://localhost:" + port + "/");
            //listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
 
            listener.Start();

            MainWindow.that.AddLog("Server is running");
            while (true)
            {

                try
                {
                    var context = listener.GetContext(); 
                    MainWindow.that.AddLog("Connection came in!");
                    context.Response.StatusCode = 200;
                    context.Response.SendChunked = true;

                    var request = context.Request;
                    MainWindow.that.AddLog("HTTP METHOD: " + request.HttpMethod.ToString());
                    string requestData = GetHttpRequestData(request);
                    MainWindow.that.AddLog("REQUEST DATA: " + requestData);

                    SendHttpResponse(context.Response);
                    MainWindow.that.AddLog("SENT THE RESPONSE: " + MainWindow.that.GetResponse());
                    
                }
                catch (Exception e)
                {
                    MainWindow.that.AddLog("Exception: "+ e.Message);
                }
            }
        }

        private string GetHttpRequestData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return "";
            }

            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);

            string s = reader.ReadToEnd();
            body.Close();
            reader.Close();

            return s;
        }


        private void SendHttpResponse(HttpListenerResponse response)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(MainWindow.that.GetResponse());
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
