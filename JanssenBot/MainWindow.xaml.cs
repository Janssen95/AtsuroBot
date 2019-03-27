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
        public static TcpClient client;
        public static StreamReader reader;
        public static StreamWriter writer;
        public static string username;
        private string password;
        public static string apiKey;
        public static List<MatchRoom> matches = new List<MatchRoom>();
        DispatcherTimer MessageTimer;
        DispatcherTimer MatchTimer;

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
                apiKey = win.ApiBox.Text;
            }
            ircLib.Reconnect(username, password);
            MessagesTimer();
            MatchesTimer();
        }

        private void MessagesTimer() //Inicia el timer para que cada cierto tiempo se revise si llegan mensajes
        {
            MessageTimer = new DispatcherTimer();
            MessageTimer.Tick += new EventHandler(MessagesTimer_Tick);
            MessageTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);
            MessageTimer.IsEnabled = true;
            MessageTimer.Start();
        }

        private void MessagesTimer_Tick(object sender, EventArgs e) //cada que transcurra el tiempo establecido, se ejecuta esta función
        {
            if (!client.Connected)
            {
                ircLib.Reconnect(username, password);
            }

            if (client.Connected)
            {
                string[] message = ircLib.GetMessage();
                ircLib.PrintMessage(message, ircBox);
            }
        }

        private void MatchesTimer() //Inicia el timer para que cada cierto tiempo se revise el estado de las matches
        {
            MatchTimer = new DispatcherTimer();
            MatchTimer.Tick += new EventHandler(MatchesTimer_Tick);
            MatchTimer.Interval = new TimeSpan(0, 0, 0, 10);
            MatchTimer.IsEnabled = true;
            MatchTimer.Start();
        }

        private void MatchesTimer_Tick(object sender, EventArgs e) //cada que transcurra el tiempo establecido, se ejecuta esta función
        {
            for(int i = 0; i < matches.Count; i++)
            {
                matches[i].Main();
            }
        }
        
        private void NewMatch_Click(object sender, RoutedEventArgs e) //Inicia una instancia de un cuarto de multi
        {
            MatchWindow window = new MatchWindow();
            window.ShowDialog();
            if (window.ok)
            {
                matches.Add(new MatchRoom(matches.Count, window.TeamOne.Text, window.TeamTwo.Text, window.RoomName.Text, window.Phase.Text, int.Parse(window.BestOf.Text)));
            }
        }

        private void SendBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                if (client.Connected)
                {
                    string send = ircLib.PrivMessageString(SendBox.Text, ChannelBox.Text, true);
                    SendBox.Text = string.Empty;
                    writer.WriteLine(send);
                    writer.Flush();
                }
            }
        }

        private void MappoolBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
