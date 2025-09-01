using System.Collections.Generic;

namespace SaveLoad
{
	public interface IKeysProvider
	{
		string Provide<TType>();
		IEnumerable<string> ProvideAll();
	}
}