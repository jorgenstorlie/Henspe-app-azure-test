using Newtonsoft.Json;
using Henspe.Core.Storage.SQLite;

namespace Henspe.Core.Model {

	/// Business entity base class. Provides the ID and key properties.
	public abstract class BusinessEntityBase : IBusinessEntity 
	{
		/// Gets or sets the Database ID. This is the internal id in this system.
		[PrimaryKey, AutoIncrement]
		[JsonIgnore]
		public int Id { get; set; }

		/// Gets or sets the key. This should be used for unique id across systems
		[Indexed]
		public abstract string Key { get; set; }
	}
}