using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;

namespace Nishtown.Utilities
{
    class Database
    {

        private string connectionString { get; set; }
        private MySqlConnection sqlconn;
        public bool isOpen { get; set; }

        public Database(string connstring)
        {
            this.connectionString = connstring;
            sqlconn = new MySqlConnection(connectionString);
        }

        public Database(string location, string database, string username, string password)
        {
            this.connectionString = "Server=" + location + ";Database=" + database + ";Uid=" + username + ";Pwd=" + password + ";";
            sqlconn = new MySqlConnection(connectionString);
        }

        public void Open()
        {
            try
            {
                sqlconn.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.isOpen = true;
            }
        }

        public void Close()
        {
            try
            {
                if (this.isOpen)
                {
                    sqlconn.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.isOpen = false;
            }
        }

        //used for anything that returns data
        public DataTable Query(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = sqlconn.CreateCommand();

                cmd.CommandText = query;
                MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
                ad.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }

            return dt;
        }

        public void BulkInsert(string csvfile, string table, string fieldterminator, string lineterminator, int linestoskip)
        {
            try
            {
                MySqlBulkLoader bl = new MySqlBulkLoader(sqlconn);
                bl.TableName = table;
                bl.FieldTerminator = fieldterminator;
                bl.LineTerminator = lineterminator;
                bl.FileName = csvfile;
                bl.NumberOfLinesToSkip = linestoskip;
                bl.Load();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //used for update,insert & delete
        public void NonQuery(string query)
        {
            try
            {
                MySqlCommand cmd = sqlconn.CreateCommand();
                cmd.CommandText = query;

                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ExportToCsv(DataTable dt, string csvpath, bool headings = false)
        {
            using(StreamWriter sr = new StreamWriter(csvpath, false))
            {
                if (headings)
                {
                    IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                    sr.WriteLine(string.Join(",",columnNames));
                }
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sr.WriteLine(string.Join(",",fields));
                }
            }

        }

    }
}
