using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
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
    public partial class Mappools : Window
    {
        public Mappools()
        {
            InitializeComponent();
            PoolBox.ItemsSource = sqlLib.GetColumnData(sqlLib.mappoolsTab, "mappoolName", string.Empty);
        }

        void Refresh()
        {
            PoolBox.ItemsSource = sqlLib.GetColumnData(sqlLib.mappoolsTab, "mappoolName", string.Empty);
            string query = sqlLib.SearchQueryColumn(sqlLib.mapsTab, "Mappools_mappoolId", sqlLib.GetColumnData(sqlLib.mappoolsTab, "mappoolId", "WHERE mappoolName = " + sqlLib.CheckString(PoolBox.Text))[0]);
            mapsGrid.ItemsSource = sqlLib.FillTable(query).DefaultView;
        }

        private void NewPoolBtn_Click(object sender, RoutedEventArgs e)
        {
            NewPoolWindow window = new NewPoolWindow();
            window.ShowDialog();
            Refresh();
        }

        private void PoolBox_DropDownClosed(object sender, EventArgs e)
        {
            Refresh();
        }

        private void DelPoolBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this pool?", "", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    string[] id = sqlLib.GetColumnData(sqlLib.mappoolsTab, "mappoolId", "WHERE mappoolName = " + sqlLib.CheckString(PoolBox.Text));
                    string query = sqlLib.DeleteRow(sqlLib.mappoolsTab, sqlLib.CheckString(id[0]));
                    sqlLib.RunQuery(query);
                    Refresh();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void AddMapBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMapWindow window = new AddMapWindow();
            window.ShowDialog();
            if (window.isOk)
            {
                int mode = 0;
                switch (window.ModeBox.Text)
                {
                    case "Standard":
                        mode = 0;
                        break;
                    case "Mania":
                        mode = 1;
                        break;
                    case "Taiko":
                        mode = 2;
                        break;
                    case "Catch":
                        mode = 3;
                        break;
                }
                MapInfo info = APIThing.GetMapInfo(window.UrlBox.Text, mode);

                string[] mapValues = new string[sqlLib.TableColumnCount(sqlLib.mapsTab)];
                mapValues[sqlLib.mapId] = string.Empty;
                mapValues[sqlLib.mapIdInPool] = CheckMapNumbersByMod(window.ModBox.Text);
                mapValues[sqlLib.mapMapper] = info.creator;
                mapValues[sqlLib.mapArtist] = info.artist;
                mapValues[sqlLib.mapTitle] = info.title;
                mapValues[sqlLib.mapDifficulty] = info.version;
                mapValues[sqlLib.mapStars] = info.difficultyrating;
                mapValues[sqlLib.mapLenght] = info.total_length;
                mapValues[sqlLib.mapBpm] = info.bpm;
                mapValues[sqlLib.mapMod] = window.ModBox.Text;
                mapValues[sqlLib.mapMappool] = sqlLib.GetColumnData(sqlLib.mappoolsTab, "mappoolId", "WHERE mappoolName = " + sqlLib.CheckString(PoolBox.Text))[0];

                string query = sqlLib.InsertDataQuery(mapValues, sqlLib.mapsTab);
                sqlLib.RunQuery(query);
                Refresh();
            }
        }

        private string CheckMapNumbersByMod(string mod)
        {
            string[] values = new string[1];
            values[0] = sqlLib.GetColumnData(sqlLib.mappoolsTab, "mappoolId", "WHERE mappoolName = " + sqlLib.CheckString(PoolBox.Text))[0];

            string[] columns = new string[1];
            columns[0] = "Mappools_mappoolId";

            string query = sqlLib.MultipleCondSearchQuery(sqlLib.mapsTab, values, columns);
            DataTable dt;
            dt = sqlLib.FillTable(query);
            try
            {
                return (dt.Rows.Count + 1).ToString();
            }
            catch
            {
                return "1";
            }
        }
    }
}
