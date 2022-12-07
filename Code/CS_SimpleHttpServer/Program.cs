using System.Net;
using System.Net.Sockets;
using System.Text;
/*
資料來源: https://gist.github.com/NikolayIT/91dee5fea4386199ea6171de80eb2be4
*/
namespace SimpleHttpServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            int Port = 8001;
            Console.WriteLine($"Listening on port {Port}...");

            IHttpServer server = new HttpServer(Port);
            server.Start();
        }
    }

    public interface IHttpServer
    {
        void Start();
    }

    public class HttpServer : IHttpServer
    {
        private readonly TcpListener listener;
        private int NO;
        public HttpServer(int port)
        {
            this.listener = new TcpListener(port);
        }

        public void Start()
        {
            this.listener.Start();
            while (true)
            {
                var client = this.listener.AcceptTcpClient();
                var buffer = new byte[10240];
                var stream = client.GetStream();
                var length = stream.Read(buffer, 0, buffer.Length);
                var incomingMessage = String.Format("Client connected with IP {0}", ((IPEndPoint)client.Client.RemoteEndPoint).Address) + "\n" + Encoding.UTF8.GetString(buffer, 0, length);

                var result = String.Format("{{\"NO\":\"{0:0000}\",\"En_Name\":\"jash.liao\",\"CH_Name\":\"小廖\"}}", NO);//@"{""NO"":""001"",""En_Name"":""jash.liao"",""CH_Name"":""小廖""}";
                NO++;
                stream.Write(
                    Encoding.UTF8.GetBytes(
                        "HTTP/1.0 200 OK" + Environment.NewLine
                        + "Content-Length: " + result.Length + Environment.NewLine
                        + "Content-Type: " + "application / json" + Environment.NewLine
                        + Environment.NewLine
                        + result
                        + Environment.NewLine + Environment.NewLine));
                Console.WriteLine("Incoming message: \n{0}", incomingMessage);
                Console.WriteLine("{0}", result);
            }
        }
    }
}