using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace JanssenBot
{
    class sqlLib
    {
        public static string matchesTab = "Matches", teamsTab = "Teams", teamMatchTab = "Teams_has_Matches", playersTab = "Players", mappoolsTab = "Mappools", mapsTab = "Maps";
        public static int playerId = 0, playerName = 1, playerRank = 2, playerTeam = 3;
        public static int teamId = 0, teamName = 1, teamCaptain = 2;
        public static int matchId = 0, matchDateTime = 1, matchTeamOne = 2, matchTeamTwo = 3, matchMappool = 4;
        public static int mappoolId = 0, mappoolName=1;
        public static int mapId = 0, mapIdInPool = 1, mapMapper = 2, mapArtist = 3, mapTitle = 4, mapDifficulty = 5, mapStars = 6, mapLenght = 7, mapBpm = 8, mapMod = 9, mapMappool = 10;

        public static MySqlConnection Connect() //Conecta a la base de datos
        {
            try
            {
                MySqlConnection conectar = new MySqlConnection("server=187.155.46.153;port=3306;database=tourneydb;Uid=Client;pwd=ultraxion1995;SslMode=None");
                conectar.Open();
                return conectar;
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
                return null;
            }
        }

        public static void RunQuery(string query) //Intenta correr un comando de MySql
        {
            try
            {
                MySqlConnection cn = Connect();
                MySqlCommand cmd = new MySqlCommand(query, cn);
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                cn.Close();
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        public static DataTable FillTable(string query) //Llena el datagrid a partir de un query
        {
            try
            {
                MySqlConnection cn = Connect();
                MySqlDataAdapter ad = new MySqlDataAdapter(query, cn);
                DataTable table = new DataTable();
                table.Clear();
                ad.Fill(table);
                cn.Close();
                return table;
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
                return null;
            }
        }

        public static string SearchQueryColumn(string table, string column, string value) //Devuelve un query para buscar en un solo campo de una tabla
        {
            string searchString = "SELECT * FROM " + table + " WHERE " + column + " LIKE " + "'%" + value + "%'";
            return searchString;
        }
        


        public static string SearchQueryAll(string table, string value) //Devuelve un query para buscar en todos los campos de una tabla
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            DataColumnCollection columns = dt.Columns;
            string searchString = "SELECT * FROM ";
            int count = 0;
            foreach (DataColumn column in columns)
            {
                if (count == 0)
                {
                    searchString = searchString + table + " WHERE " + columns[count].ColumnName + " LIKE " + "'%" + value + "%'";
                }
                else if (count > 0)
                {
                    searchString = searchString + " OR " + columns[count].ColumnName + " LIKE " + "'%" + value + "%'";
                }
                count++;
            }
            return searchString;
        }

        public static string MultipleCondSearchQuery(string table, string[] values, string[] columns)//Busqueda con condiciones multiples
        {
            string searchString = "SELECT * FROM " + table;
            int count = 0;
            foreach (string value in values)
            {
                if (count == 0)
                {
                    searchString = searchString + " WHERE " + columns[count] + " = " + CheckString(value);
                    count++;
                }
                else
                {
                    searchString = searchString + " AND " + columns[count] + " = " + CheckString(value);
                    count++;
                }
            }
            return searchString;
        }

        public static string InsertDataQuery(string[] values, string table) //Arma el query para insertar un arreglo de datos a una tabla
        {
            string insertString = "INSERT INTO " + table;
            DataTable dt = FillTable("SELECT * FROM " + table);
            DataColumnCollection columns = dt.Columns;
            int count = 0;
            foreach (DataColumn column in columns)
            {
                if (count == 0)
                {
                    insertString = insertString + " (" + columns[count].ColumnName;
                }
                else if (count < columns.Count - 1)
                {
                    insertString = insertString + ", " + columns[count].ColumnName;
                }
                else if (count == columns.Count - 1)
                {
                    insertString = insertString + ", " + columns[count].ColumnName + ") ";
                }
                count++;
            }
            for (int i = 0; i <= values.Length - 1; i++)
            {
                if (i == 0)
                {
                    insertString = insertString + "VALUES (" + CheckString(values[i]);
                }

                else if (i < values.Length - 1)
                {
                    insertString = insertString + ", " + CheckString(values[i]);
                }

                else if (i == values.Length - 1)
                {
                    insertString = insertString + ", " + CheckString(values[i]) + ")";
                }
            }
            return insertString;
        }

        public static string EditCellQuery(string[] values, string table) //Query para editar las celdas de una fila
        {
            string query = "UPDATE " + table + " SET ";
            DataTable dt = FillTable("SELECT * FROM " + table);
            var columns = dt.Columns;
            for (int i = 1; i < columns.Count; i++)
            {
                if (i < columns.Count - 1)
                {
                    query = query + columns[i].ColumnName + " = " + CheckStringComilla(values[i]) + ", ";
                }
                else
                {
                    query = query + columns[i].ColumnName + " = " + CheckStringComilla(values[i]) + " ";
                }
            }
            query = query + "WHERE " + columns[0].ColumnName + " = " + values[0];
            return query;
        }

        public static string CheckString(string value) //Para convertir los valores de string a la sintaxis correcta de MySql
        {
            if (value.Contains('\x0027'))
            {
                value = value.Replace('\x0027', '`');
            }
            if (value == string.Empty)
            {
                value = "null";
            }
            else
            {
                value = "'" + value + "'";
            }
            if (value.Contains("'"))
            {
                value.Replace("'", "");
            }
            return value;
        }

        public static string CheckStringComilla(string value)
        {
            if (value == string.Empty)
            {
                value = "null";
            }
            if (value.Contains("'"))
            {
                value.Replace("'", "");
            }
            return value;
        }

        public static string[] GetSelectedCells(DataGrid selectedGrid) //Te regresa los datos de la fila seleccionada en el dataGrid
        {
            if (selectedGrid.SelectedItems.Count > 0)
            {
                var cellInfos = selectedGrid.SelectedCells;

                var list1 = new List<string>();

                int i = 0;
                foreach (DataGridCellInfo cellInfo in cellInfos)
                {
                    if (cellInfo.IsValid)
                    {
                        var content = cellInfo.Column.GetCellContent(cellInfo.Item);
                        var row = (DataRowView)content.DataContext;
                        object[] obj = row.Row.ItemArray;
                        list1.Add(obj[i].ToString());
                    }
                    i++;
                }
                string[] values = list1.ToArray();
                return values;
            }

            else
            {
                MessageBox.Show("Selecciona una fila");
                string[] values = new string[0];
                return values;
            }
        }

        public static string MatchQuery(string[] searchValue, string table) //Devuelve un query que indica si un elemento de la tabla ya contiene algun elemento de los enviados
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            string[] match = new string[dt.Columns.Count];
            string returnQuery = "SELECT idCliente FROM clientes WHERE idCliente = 9999999";
            bool firstPass = true;

            for (int i = 1; i <= dt.Columns.Count - 1; i++)
            {
                string column = dt.Columns[i].ColumnName;
                string searchQuery = "SELECT " + column + " FROM " + table + " WHERE " + column + " = " + CheckString(searchValue[i]);
                DataTable columnTable = FillTable(searchQuery);

                if (columnTable.Rows.Count > 0)
                {
                    for (int j = 0; j <= columnTable.Rows.Count - 1; j++)
                    {
                        if (firstPass)
                        {
                            DataRow row = columnTable.Rows[j];
                            returnQuery = "SELECT * FROM " + table + " WHERE " + column + " = " + CheckString(row[0].ToString());
                            firstPass = false;
                        }
                        else
                        {
                            DataRow row = columnTable.Rows[j];
                            returnQuery = returnQuery + " OR " + column + " = " + CheckString(row[0].ToString());
                        }
                    }
                }
            }
            return returnQuery;
        }

        public static string MatchQuery(string[] searchValue, string table, string index) //Devuelve un query que indica si un elemento de la tabla ya contiene algun elemento de los enviados
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            string[] match = new string[dt.Columns.Count];
            string returnQuery = "SELECT idCliente FROM clientes WHERE idCliente = 9999999";
            bool firstPass = true;

            for (int i = 1; i < dt.Columns.Count - 1; i++)
            {
                string column = dt.Columns[i].ColumnName;
                string searchQuery = "SELECT " + column + " FROM " + table + " WHERE " + column + " = " + CheckString(searchValue[i]);
                DataTable columnTable = FillTable(searchQuery);

                if (columnTable.Rows.Count > 0)
                {
                    for (int j = 0; j <= columnTable.Rows.Count - 1; j++)
                    {
                        if (firstPass)
                        {
                            DataRow row = columnTable.Rows[j];
                            returnQuery = "SELECT * FROM " + table + " WHERE idCliente = " + index + " AND (" + column + " = " + CheckString(row[0].ToString());
                            firstPass = false;
                        }
                        else
                        {
                            DataRow row = columnTable.Rows[j];
                            returnQuery = returnQuery + " OR " + column + " = " + CheckString(row[0].ToString());
                        }
                    }
                }
            }
            if (!firstPass)
            {
                returnQuery = returnQuery + ")";
            }
            return returnQuery;
        }

        public static string LastRowIndex(string table) //Devuelve el id del ultimo elemento de la tabla
        {
            string index;
            DataTable dt = FillTable("SELECT * FROM " + table);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[dt.Rows.Count - 1];
                index = dr[0].ToString();
                return index;
            }
            else
            {
                return index = "1";
            }
        }

        public static string[] GetColumnData(string table, string column, string condition) //Devuelve un arreglo de strings con datos de una columna, puedes proporcionar condicion
        {
            string[] array;
            string query = "SELECT " + column + " FROM " + table + " " + condition;
            DataTable dt = FillTable(query);
            array = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                array[i] = row[0].ToString();
            }
            return array;
        }


        public static string[] GetRowData(string table, string index)
        {
            string[] array;
            string query = "SELECT * FROM " + table;
            DataTable dt = FillTable(query);
            string idColumn = dt.Columns[0].ColumnName;
            query = "SELECT * FROM " + table + " WHERE " + idColumn + " = " + index;
            dt.Clear();
            dt = FillTable(query);
            DataRow row = dt.Rows[0];
            array = new string[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                array[i] = row[i].ToString();
            }

            return array;
        }

        public static string UpdateValueQuery(string table, string column, string newValue, string index)
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            string idColumn = dt.Columns[0].ColumnName;
            string query = "UPDATE " + table + " SET " + column + " = " + CheckString(newValue) + " WHERE " + idColumn + " = " + index;
            return query;
        }

        public static string UpdateRowQuery(string table, string[] newValues)
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            bool firstPass = true;
            string query = "UPDATE " + table + " SET ";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (firstPass)
                {
                    query = query + dt.Columns[i].ColumnName + " = " + CheckString(newValues[i]);
                }
                else
                {
                    query = query + ", " + dt.Columns[i].ColumnName + " = " + CheckString(newValues[i]);
                }
            }
            query = query + " WHERE " + dt.Columns[0].ColumnName + " = " + newValues[0];
            return query;
        }

        public static string DeleteRow(string table, string index)
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            string idColumn = dt.Columns[0].ColumnName;
            string query = "DELETE FROM " + table + " WHERE " + idColumn + " = " + index;
            return query;
        }

        public static string DeleteRowConditionMultiple(string table, string condition)
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            string query = "DELETE FROM " + table + " " + condition;
            return query;
        }

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        public static bool IsDigitsOnly(char c)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }
            return true;
        }

        public static int TableColumnCount(string table)
        {
            DataTable dt = FillTable("SELECT * FROM " + table);
            return dt.Columns.Count;
        }
    }
}
