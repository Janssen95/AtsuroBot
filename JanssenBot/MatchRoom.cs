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
    public class MatchRoom
    {
        StreamReader reader;
        StreamWriter writer;
        TcpClient client;
        DispatcherTimer timer;
        MatchWindow window;

        public MatchRoom(StreamReader reader, StreamWriter writer, TcpClient client)
        {
            this.reader = reader;
            this.writer = writer;
            this.client = client;
            window = new MatchWindow();
            window.Show();
        }

        private void CreateMatch()
        {
            if (client.Connected)
            {
                writer.WriteLine("/msg BanchoBot !mp create To!M");
            }
        }

        private void Timer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.IsEnabled = true;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (client.Connected)
            {
                GetMessage();
            }

            timer.Start();
        }

        private void GetMessage()
        {
            if (client.Available > 0 || reader.Peek() >= 0)
            {
                string message = reader.ReadLine();
                PrintMessage(message, window.ircBox);
            }
        }

        private void PrintMessage(string message, TextBox box)
        {
            if (message.Contains("!cho@ppy.sh QUIT"))
            {
            }
            else
            {
                box.Text += $"\r\n{message}";
                box.ScrollToEnd();
            }
        }
    }
}
