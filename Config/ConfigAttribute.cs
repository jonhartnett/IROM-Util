namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Marks a field for automatic for loading and saving in a class that uses <see cref="AutoConfig"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class ConfigAttribute : Attribute
	{
		public string Tag;
		public uint FileID;
		
		/// <summary>
		/// Creates a new ConfigAttribute with the given tag and file id.
		/// </summary>
		/// <param name="tag">The unique tag identifier for the member.</param>
		/// <param name="id">The file id, useful for multiple configs in one file.</param>
		public ConfigAttribute(string tag, uint id = 0)
		{
			if(tag.Contains(":") || tag.Contains("\n"))
			{
				throw new Exception("Config tags cannot contain colons or new lines.");
			}
			Tag = tag;
			FileID = id;
		}
	}
}
