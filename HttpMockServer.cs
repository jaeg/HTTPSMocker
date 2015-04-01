using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;


namespace HTTPSMocker  
{
    class HttpMockServer
    {
        string port;
        string response;
        public HttpListener listener = new HttpListener();
        public HttpMockServer(string port, string response)
        {

            this.port = port;
            this.response = response;
        }


        public void Start()
        {

            listener.Prefixes.Add("http://localhost:" + port + "/");
            listener.Prefixes.Add("http://127.0.0.1:" + port + "/");

            listener.Start();

            MessageBox.Show("Server is running");
            while (true)
            {

                try
                {
                    var context = listener.GetContext(); //Block until a connection comes in
                    MessageBox.Show("Connection came in!");
                    context.Response.StatusCode = 200;
                    context.Response.SendChunked = true;

                    HttpListenerResponse myResponse = context.Response;

                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(response);
                    myResponse.ContentLength64 = buffer.Length;
                    System.IO.Stream output = myResponse.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }


    }
}
