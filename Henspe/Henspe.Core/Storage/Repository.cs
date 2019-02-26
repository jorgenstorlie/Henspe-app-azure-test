using System;
using System.Collections.Generic;
using System.IO;
using Henspe.Core.Model;
using System.Linq;
using SQLite;

namespace SNLA.Core.Storage  
{
	public class Repository : RepositoryBase
	{
		public Repository(SQLiteConnection conn) : base(conn)
		{
			//_database = conn;

			CreateOrUpdateTables();
		}

		private void CreateOrUpdateTables()
		{
			// create the tables
            //_database.CreateTable<Hjertestarter>();
     }

		public void DeleteAllTables()
		{
			// create the tables
           	//DeleteAllItems<Hjertestarter>();
      	}
	}
}