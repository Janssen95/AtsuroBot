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
using System.Windows.Shapes;

namespace JanssenBot.Windows
{
    /// <summary>
    /// Interaction logic for AddMapWindow.xaml
    /// </summary>
    public partial class AddMapWindow : Window
    {
        public bool isOk = false;

        public AddMapWindow()
        {
            InitializeComponent();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            isOk = true;
            Close();
        }
    }
}
