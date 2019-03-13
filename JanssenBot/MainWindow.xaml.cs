using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Net.Sockets;

namespace JanssenBot
{
    public partial class MainWindow : Window
    {
        TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        
        private string username;
        private string password;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login win = new Login();
            win.ShowDialog();
            if(win.loggedIn == true)
            {
                username = win.UserBox.Text;
                password = win.PassBox.Text;
            }
            Reconnect();

            Timer();
        }

        private void Reconnect()
        {
            client = new TcpClient("irc.ppy.sh", 6667);
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.WriteLine("PASS " + password + Environment.NewLine
                    + "NICK " + username + Environment.NewLine
                    + "USER " + username + " 8 * :" + username);
            writer.Flush();
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

            if (!client.Connected)
            {
                Reconnect();
            }

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
                PrintMessage(message, ircBox);
            }
        }

        private void PrintMessage(string message, TextBox box)
        {
            if(message.Contains("!cho@ppy.sh QUIT"))
            {
            }
            else
            {
                box.Text += $"\r\n{message}";
                box.ScrollToEnd();
            }
        }

        private void NewTextBox()
        {
            TabItem newTabItem = new TabItem
            {
                Header = "Test",
                Name = "Test"
            };
            Tabs.Items.Add(newTabItem);
        }

        private void NewMatch_Click(object sender, RoutedEventArgs e)
        {
            writer.WriteLine("/QUERY BanchoBot !mp make To!M");
            MatchRoom newMatch = new MatchRoom(reader, writer, client);
        }

        private void JoinOsu_Click(object sender, RoutedEventArgs e)
        {
            writer.WriteLine("JOIN #osu");
            writer.Flush();
        }
    }
}
