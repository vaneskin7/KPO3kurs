using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace KPOserver
{
    public class Server
    {
        private Socket listener;
        private bool isRunning;
        private static int size = 2048;
        private const string ipAddres = "0.0.0.0";
        private static string userQuery;
        private static bool isCorrect;
        public Server()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            isRunning = false;
        }

        public void Start(int port) // запуск сервера
        {
            isRunning = true;
            listener.Bind(new IPEndPoint(IPAddress.Parse(ipAddres), port));
            listener.Listen(10);
            Console.WriteLine("Сервер запущен. Порт: " + port);

            while (isRunning)
            {
                Socket clientSocket = listener.Accept();
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(clientSocket);
            }
        }

        public void Stop() // остановка сервера
        {
            isRunning = false;
            listener.Close();
            Console.WriteLine("Сервер остановлен.");
        }

        private void HandleClient(object clientObj) // функция общения с клиентом
        {
            Socket clientSocket = (Socket)clientObj;

            Console.WriteLine("Получен запрос клиента на установленное соединение \n" +
                        "Дескриптор {0};\n IP-адрес клиентского сокета {1};\n Порт {2}", 
                        clientSocket.Handle, ((IPEndPoint)clientSocket.RemoteEndPoint).Address, 
                        ((IPEndPoint)clientSocket.RemoteEndPoint).Port);

            while (true) // общение с подключившимся клиентом
            {
                string dataRec = null;
                byte[] byteRec = new byte[size];
                int lenBytesRecieved;
                try
                {
                    lenBytesRecieved = clientSocket.Receive(byteRec);
                }
                catch (Exception)
                {
                    Console.WriteLine("Клиент отключился");
                    break;
                }

                // декодируем все байты из указанного массива байтов в строку
                dataRec += Encoding.UTF8.GetString(byteRec, 0, lenBytesRecieved);
                dataRec = dataRec.Substring(0, dataRec.Length);
                Console.WriteLine("Получено сообщение от клиента {0}", dataRec);
                userQuery = dataRec;
                byte[] byteSend;
                int lenBytesSend; 
                
                if (userQuery[0..8] == "connect ") // авторизация пользователя
                {
                    userQuery = userQuery[8..];
                    Authorization(clientSocket);
                    /*string dataToSend = "";
                    if (isCorrect == true)
                    {
                        dataToSend = "correct";
                        SendDataToClient(dataToSend, clientSocket);
                    }
                    else
                    {
                        dataToSend = "invalid";
                        SendDataToClient(dataToSend, clientSocket);
                        //clientSocket.Close();
                    }
                    isCorrect = false;*/
                    continue;

                }
                if (userQuery[0..6] == "UPDATE" || userQuery[0..6] == "INSERT" || userQuery[0..6] == "DELETE")
                {
                    UpdateData(userQuery, clientSocket);
                }
                else
                {
                    SendDataToClient(userQuery, clientSocket);
                }
            }
            clientSocket.Close();
            Console.WriteLine("Клиент отключился.");
        }

        
    }

}