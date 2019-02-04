using System;
using System.Collections.Generic;
using System.IO;
using Henspe.Core.Model;
using System.Linq;
using SQLite;
using SNLA.Core.Storage; 

namespace Henspe.Core.Storage  
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
           // DeleteAllItems<Hjertestarter>();
      }

        /*public List<T> Query<T>(string sqlQuery) where T : IBusinessEntity, new()
        {
            return _database.Query<T>(sqlQuery).ToList();
        }

        public List<T> QueryParams<T>(string sqlQuery, object[] paramArray) where T : IBusinessEntity, new()
        {
            return _database.Query<T>(sqlQuery, paramArray).ToList();
        }

        public IEnumerable<T> GetItemList<T>() where T : IBusinessEntity, new()
		{
			return (from i in _database.Table<T>() select i);
		}

		public T GetItem<T>(int idInput) where T : IBusinessEntity, new()
		{
			return _database.Table<T>().FirstOrDefault(x => x.Id == idInput);
		}

		public T GetItem<T>(string keyInput) where T : IBusinessEntity, new()
		{
			return _database.Table<T>().FirstOrDefault(x => keyInput.Equals(x.Key));
		}

        public int SaveItem<T>(T item) where T : IBusinessEntity
        {
            lock (locker)
            {
                return item.Id != 0 ? _database.Update(item) : _database.Insert(item);
            }
        }

        public int SaveItems(IEnumerable<IBusinessEntity> items)
        {
            lock (locker)
            {
                var c = 0;
                _database.RunInTransaction(delegate
                {
                    foreach (var item in items)
                    {
                        c += item.Id != 0 ? _database.Update(item) : _database.Insert(item);
                    }
                });
                return c;
            }
        }

		public int DeleteItem(IBusinessEntity item)
		{
			lock (locker)
			{
				return _database.Delete(item);
			}
		}

		public int DeleteItems(IEnumerable<IBusinessEntity> items)
		{
			lock (locker)
			{
				var c = 0;
				_database.RunInTransaction(delegate
				{
					foreach (var item in items)
					{
						c += _database.Delete(item);
					}
				});
				return c;
			}
		}

		public int DeleteAllItems<T>() where T : IBusinessEntity, new()
		{
			lock (locker)
			{
				var c = 0;
				_database.RunInTransaction(delegate
				{
					foreach (var item in (from i in _database.Table<T>() select i))
					{
						c += _database.Delete(item);
					}
				});
				return c;
			}
		}*/
	}
}