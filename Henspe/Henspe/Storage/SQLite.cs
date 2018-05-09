using System;
using System.Collections.Generic;

namespace Henspe.Core.Storage.SQLite 
{
	public class PrimaryKeyAttribute : Attribute 
	{
	}

	public class AutoIncrementAttribute : Attribute 
	{
	}

	public class IndexedAttribute : Attribute 
	{
	}

	public class IgnoreAttribute : Attribute 
	{
	}

	public class MaxLengthAttribute : Attribute 
	{
		public int Value { get; private set; }

		public MaxLengthAttribute(int length)
		{
			Value = length;
		}
	}

	public class CollationAttribute : Attribute 
	{
		public string Value { get; private set; }

		public CollationAttribute(string collation)
		{
			Value = collation;
		}
	}
}

namespace Henspe.Core.Storage.SQLiteBase 
{
	public abstract class SQLiteConnection : IDisposable 
	{
		public string DatabasePath { get; protected set; }

		public bool TimeExecution { get; set; }

		public bool Trace { get; set; }

		public SQLiteConnection(string databasePath)
		{
			DatabasePath = databasePath;
		}

		public abstract int CreateTable<T>();

		public abstract SQLiteCommand CreateCommand(string cmdText, params object[] ps);

		public abstract int Execute(string query, params object[] args);

		public abstract List<T> Query<T>(string query, params object[] args) where T : new();

		public abstract IEnumerable<T> DeferredQuery<T>(string query, params object[] args) where T : new();

		public abstract List<object> Query(TableMapping map, string query, params object[] args);

		public abstract IEnumerable<object> DeferredQuery(TableMapping map, string query, params object[] args);

		public abstract TableQuery<T> Table<T>() where T : new();

		public abstract T Get<T>(object pk) where T : new();

		public bool IsInTransaction { get; protected set; }

		public abstract void BeginTransaction();

		public abstract void Rollback();

		public abstract void Commit();

		public abstract void RunInTransaction(Action action);

		public abstract int InsertAll(System.Collections.IEnumerable objects);

		public abstract int Insert(object obj);

		public abstract int Insert(object obj, Type objType);

		public abstract int Insert(object obj, string extra);

		public abstract int Insert(object obj, string extra, Type objType);

		public abstract int Update(object obj);

		public abstract int Update(object obj, Type objType);

		public abstract int Delete<T>(T obj);

		public void Dispose()
		{
			Close();
		}

		public abstract void Close();
	}

	public abstract class TableMapping {}

	public abstract class SQLiteCommand {}

	public abstract class TableQuery<T> : IEnumerable<T> where T : new() 
	{
		public virtual IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}