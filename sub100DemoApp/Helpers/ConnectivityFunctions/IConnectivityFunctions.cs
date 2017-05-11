using System.Threading.Tasks;

namespace sub100DemoApp
{
	public interface IConnectivityFunctions
	{
		Task<bool> IsConnected();
	}
}