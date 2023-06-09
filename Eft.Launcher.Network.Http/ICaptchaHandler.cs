using System.Net.Http;
using System.Threading.Tasks;

namespace Eft.Launcher.Network.Http;

public interface ICaptchaHandler
{
	Task<bool> Handle(HttpResponseMessage responseWithCaptcha, HttpClientHandler clientHandler);
}
