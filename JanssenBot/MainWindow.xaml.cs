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
        private string username;
        private string password;
        public static string apiKey;
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
                apiKey = win.ApiBox.Text;
            }
            ircLib.Reconnect(username, password);
            Timer();
        }

        private void Timer() //Inicia el timer para que cada cierto tiempo se revise si llegan mensajes
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 25);
            timer.IsEnabled = true;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e) //cada que transcurra el tiempo establecido, se ejecuta esta función
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

        private void NewMatch_Click(object sender, RoutedEventArgs e) //Inicia una instancia de un cuarto de multi
        {
            MatchWindow window = new MatchWindow();
            window.ShowDialog();
            MatchRoom newMatch = new MatchRoom(window.TeamOne.Text, window.TeamTwo.Text, window.RoomName.Text, int.Parse(window.BestOf.Text));
        }

        private void JoinOsu_Click(object sender, RoutedEventArgs e) //Unirse al canal #spanish, esto es para motivos de pruebas
        {
            writer.WriteLine("JOIN #spanish");
            writer.Flush();
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
    }
}
