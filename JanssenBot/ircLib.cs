using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Net.Sockets;

namespace JanssenBot
{
    class ircLib
    {
        public static void Reconnect(string username, string password)
        {
            MainWindow.client = new TcpClient("irc.ppy.sh", 6667);
            MainWindow.reader = new StreamReader(MainWindow.client.GetStream());
            MainWindow.writer = new StreamWriter(MainWindow.client.GetStream());
            MainWindow.writer.WriteLine("PASS " + password + Environment.NewLine
                    + "NICK " + username + Environment.NewLine
                    + "USER " + username + " 8 * :" + username);
            MainWindow.writer.Flush();
        }

        public static string PrivMessageString(string message, string reciever, bool isChannel) //Construir un string para enviar mensaje privado a alguien o a algun canal
        {
            if (isChannel)
            {
                string result = "PRIVMSG #" + reciever + " :" + message;
                return result;
            }
            else
            {
                string result = "PRIVMSG " + reciever + " :" + message;
                return result;
            }
        }

        public static string GetChannel(string message) //Extraer el canal de un mensaje enviado
        {
            string channel = string.Empty;

            int messCount = message.Length;
            char[] messChar = message.ToCharArray();
            for(int i = message.Length - 1; messChar[i] != '#'; i--)
            {
                channel = messChar[i] + channel;
            }

            messChar = channel.ToCharArray();
            channel = string.Empty;

            for(int i = 0; messChar[i] != ' '; i++)
            {
                channel = channel + messChar[i];
            }

            return channel;
        }

        public static string GetMessage() //Leer mensaje
        {
            if (MainWindow.client.Available > 0 || MainWindow.reader.Peek() >= 0)
            {
                string message = MainWindow.reader.ReadLine();
                if (message.Contains("PING"))
                {
                    MainWindow.writer.WriteLine("PONG cho.ppy.sh");
                    MainWindow.writer.Flush();
                }
                return message;
            }
            return null;
        }

        public static void PrintMessage(string message, TextBox printOut)
        {
            if (message == null || message.Contains("!cho@ppy.sh QUIT") || message.Contains("!cho@ppy.sh JOIN"))
            {
            }
            else
            {
                printOut.Text += $"\r\n{message}";
                printOut.ScrollToEnd();
            }
        }
    }
}
