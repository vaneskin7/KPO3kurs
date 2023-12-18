using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace KPO3kurs
{
    public class InternetConnection
    {
        public static int PORT = 0;
        private const int SIZE = 8192;
        //private static string login;
        //private static string password;
        public static bool connection;
        public static string ipAdress = "";
        private static Socket s1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static IPHostEntry ipHost;
        private static IPAddress IPAddress;
        private static IPEndPoint ipEndPoint;

        public InternetConnection(string userLogin, string userPassword)
        {
            //login = userLogin;
            //password = userPassword;
        }

        public InternetConnection() {}

        public static bool ConnectToServer(string login, string password, string adress, int port) // подключение к серверу
        {
            try
            {
                PORT = port;
                ipAdress = adress;
                s1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    ipHost = Dns.Resolve(ipAdress);
                    IPAddress = ipHost.AddressList[1];
                    ipEndPoint = new IPEndPoint(IPAddress, PORT);
                }
                catch (Exception)
                {
                    MessageBox.Show("Отсутствует подключение к серверу", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return false;
                    throw;
                }
                
                s1.Connect(ipEndPoint);
                string textClient = "connect " + login + ";" + password; //Convert.ToChar(3);
                byte[] byteSend = Encoding.UTF8.GetBytes(textClient);
                s1.Send(byteSend);
                byte[] byteRec = new byte[SIZE];//буфер для сообщений сервера
                int len = s1.Receive(byteRec); // получаем от сервера массив байтов 
                string textServer = null;
                textServer = Encoding.UTF8.GetString(byteRec, 0, len);
                connection = true;
                if (textServer == "correct")
                {
                    return true;
                }
                else
                {
                    s1.Close();
                    return false;
                }
            }
            catch (SocketException ex)
            {
                connection = false;
                MessageBox.Show("Отсутствует подключение к серверу", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        public static string GetDataFromServer() // запрос информации от сервера
        {
            try
            {
                string query = "SELECT empID, empName, empRank,empDate, empGender, empBirthDate FROM employeesInfo";
                byte[] byteSend = Encoding.UTF8.GetBytes(query);
                s1.Send(byteSend);
                byte[] byteRec = new byte[SIZE];
                int len = s1.Receive(byteRec);
                string textServer = null;
                textServer = Encoding.UTF8.GetString(byteRec, 0, len);
                connection = true;
                return textServer;
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует подключение к серверу", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                
                connection = false;
                return null;
            }
        }

        public static bool SendDataToServer(string query, out string textServer) // отправка данных на сервер
        {
            try
            {
                    byte[] byteSend = Encoding.UTF8.GetBytes(query);
                    s1.Send(byteSend);
                    byte[] byteRec = new byte[SIZE];
                    int len = s1.Receive(byteRec);
                    textServer = null;
                    textServer = Encoding.UTF8.GetString(byteRec, 0, len);
                    connection = true;
                    return true;
            }
            catch (Exception)
            {
                    connection = false;
                    MessageBox.Show("Отсутствует подключение к серверу", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    textServer = "";
                    return false;
            }
        }
    }
}


