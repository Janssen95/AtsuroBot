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
        public static void Reconnect(string username, string password) //Conectarse a pancho
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

        public static string[] GetMessage() //Leer mensaje y separar en canal, enviador y mensaje en un string (0 es canal, 1 es usuario que envio mensaje, 2 es el mensaje en sí
        {
            if (MainWindow.client.Available > 0 || MainWindow.reader.Peek() > 0)
            {
                string message = MainWindow.reader.ReadLine();
                if (message.Contains("PING"))
                {
                    MainWindow.writer.WriteLine("PONG");
                    MainWindow.writer.Flush();
                    return null;
                }
                else if (message.Contains("QUIT") || message.Contains("JOIN"))
                {
                    return null;
                }
                else if (message.Contains("Created the tournament match"))
                {
                    MainWindow.matches[MainWindow.matches.Count-1].channel = int.Parse(GetCreatedChannel(message));
                    string[] compMessage = new string[2];
                    compMessage[0] = GetSender(message);
                    compMessage[1] = GetMessage(message);
                    return compMessage;
                }
                else if(message.Contains("PRIVMSG #"))
                {
                    string[] compMessage = new string[3];
                    compMessage[0] = GetChannel(message);
                    compMessage[1] = GetSender(message);
                    compMessage[2] = GetMessage(message);
                    return compMessage;
                }
                else if (message.Contains("PRIVMSG"))
                {
                    string[] compMessage = new string[2];
                    compMessage[0] = GetSender(message);
                    compMessage[1] = GetMessage(message);
                    return compMessage;
                }
                else
                {
                    string[] compMessage = new string[1];
                    compMessage[0] = GetMessage(message);
                    return compMessage;
                }
            }
            return null;
        }

        public static void PrintMessage(string[] message, TextBox printOut) //Imprime un mensaje recibido del getmessage al cuadro de texto de la ventana
        {
            if(message == null)
            {
            }
            else if (message.Length == 1)
            {
                printOut.Text += $"\r\n{message[0]}";
            }
            else if (message.Length == 2)
            {
                printOut.Text += $"\r\n{message[0]}: {message[1]}";
                printOut.ScrollToEnd();
            }
            else if (message.Length == 3)
            {
                printOut.Text += $"\r\n{message[1]} at {message[0]}: {message[2]}";
                printOut.ScrollToEnd();
            }
        }

        public static string GetChannel(string message) //Extraer el canal de un mensaje enviado
        {
            string channel = string.Empty;

            int messCount = message.Length;
            char[] messChar = message.ToCharArray();
            for(int i = message.Length - 1; i > 0 && messChar[i] != '#' ; i--)
            {
                channel = messChar[i] + channel;
            }

            messChar = channel.ToCharArray();
            channel = string.Empty;

            for(int i = 0; i < messChar.Length && messChar[i] != ' '; i++)
            {
                channel = channel + messChar[i];
            }

            channel = "#" + channel;
            return channel;
        }

        public static string GetSender(string message) //Conseguir el mensaje que envio un usuario
        {
            string sender = string.Empty;

            char[] sendChar = message.ToCharArray();
            for (int i = 0; i < sendChar.Length && sendChar[i] != '!'; i++)
            {
                sender = sender + sendChar[i];
            }

            sendChar = sender.ToCharArray();
            sender = string.Empty;

            for (int i = sendChar.Length - 1; i > 0 && sendChar[i] != ':'; i--)
            {
                sender = sendChar[i] + sender;
            }

            return sender;
        }

        public static string GetMessage(string message) //Conseguir mensaje enviado lol
        {
            string[] splitMess = message.Split(':');
            string directive = string.Empty;
            
            for(int i = 2; i < splitMess.Length; i++)
            {
                directive = directive + splitMess[i];
            }

            return directive;
        }

        public static string GetCreatedChannel(string message)
        {
            string channel = message.Split('/')[4];
            channel = channel.Split(' ')[0];
            return channel;
        }

        public static void CreateMatch(string matchName) //Envia un mensaje a banchobot para que se abra un cuarto de multi
        {
            if (MainWindow.client.Connected)
            {
                MainWindow.writer.WriteLine(PrivMessageString("!mp make " + matchName, "BanchoBot", false));
                MainWindow.writer.Flush();
            }
        }
    }
}
