using System;

namespace PlugIn
{
	/// <summary>
	/// Generic plugin interface
	/// </summary>
	public interface IPlugin
	{
		Int32 Add(Int32 a, Int32 b);
		string Name{get;set;}
		IPluginHost Host{get;set;}
		void Show();
	}

	/// <summary>
	/// The host
	/// </summary>
	public interface IPluginHost
	{
		bool Register(IPlugin ipi);
	}
}
