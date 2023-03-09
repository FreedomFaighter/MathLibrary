using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace MathLog
{
	public class MathLog : IDisposable
	{
		static string connectionString = "URI=file:MathLog.db";

		static IDbConnection dbcon = (IDbConnection)new SqliteConnection(connectionString);

		public MathLog ()
		{
			if (dbcon.State != ConnectionState.Open) {
				dbcon.Open ();
			}
		}

		public void Write(String Message, String Function)
		{
			string sql = "INSERT INTO MathLog VALUES (@Message, @Time, @Function)";
			IDbDataParameter message = new SqliteParameter ("@Message", Message);
			IDbDataParameter time = new SqliteParameter ("@Time", DateTime.Now);
			IDbDataParameter function = new SqliteParameter ("@Function", Function);

			using (IDbCommand sc = new SqliteCommand(sql, (SqliteConnection)dbcon)) {
				sc.CommandType = CommandType.Text;
				sc.Parameters.Add (message);
				sc.Parameters.Add (time);
				sc.Parameters.Add (function);
				sc.ExecuteNonQuery ();
			}
		}


		#region IDisposable implementation
		public void Dispose ()
		{
			dbcon.Close ();
		}
		#endregion
	}
}

