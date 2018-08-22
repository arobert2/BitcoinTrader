using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BTCTrader.DataScraper;
using System.Data;

namespace BTCTrader.DataScraperService
{
    public class DatabaseWriter
    {
        public string ConnectionString { get; set; }
        private SqlConnection _connection { get; set; }
        private string Market { get; set; }

        public DatabaseWriter(string contString, string MarketString)
        {
            ConnectionString = contString;
            _connection = new SqlConnection(ConnectionString);
            Market = MarketString.Replace('-','_');
        }

        public bool CheckIfTableExists(MarketTimes mt)
        {
            lock (_connection)
            {
                _connection.Open();
                bool result;
                using (SqlCommand command = new SqlCommand("SELECT count(*) as IsExists FROM dbo.sysobjects where id = object_id('[" + Market + mt.ToString() + "]')", _connection))
                    result = Convert.ToBoolean(command.ExecuteScalar());
                _connection.Close();
            
            return result;
            }
        }

        public void CreateTable(MarketTimes mt)
        {
            lock (_connection)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("CREATE TABLE ");
                sb.Append(Market + mt.ToString());
                sb.Append(" ( ID int PRIMARY KEY IDENTITY, TimeStamp DATETIME NOT NULL,");
                sb.Append(" Frame int NOT NULL, Average float NOT NULL, High float NOT NULL,");
                sb.Append(" LastHigh float NOT NULL, LastLow float NOT NULL, Low float NOT NULL )");

                _connection.Open();
                using (SqlCommand command = new SqlCommand(sb.ToString(), _connection))
                    command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void Create(CoinIntervalModel model, MarketTimes mt)
        {
            if (!CheckIfTableExists(mt))
                CreateTable(mt);

            lock (_connection)
            {
                _connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO " + Market + mt.ToString() + " VALUES (@TimeStamp, @Interval, @Average, @High, @LastHigh, @LastLow, @Low)", _connection))
                {
                    command.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = model.IntervalStamp;
                    command.Parameters.Add("@Interval", SqlDbType.Int).Value = model.Interval;
                    command.Parameters.Add("@Average", SqlDbType.Decimal).Value = model.Average;
                    command.Parameters.Add("@High", SqlDbType.Decimal).Value = model.High;
                    command.Parameters.Add("@LastHigh", SqlDbType.Decimal).Value = model.LastHigh;
                    command.Parameters.Add("@LastLow", SqlDbType.Decimal).Value = model.LastLow;
                    command.Parameters.Add("@Low", SqlDbType.Decimal).Value = model.Low;

                    command.ExecuteNonQuery();
                }
                _connection.Close();
            }
        }

        ~DatabaseWriter()
        {
            _connection.Dispose();
        }
    }
}
