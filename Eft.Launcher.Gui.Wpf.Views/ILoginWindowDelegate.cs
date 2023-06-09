using Newtonsoft.Json.Linq;

namespace Eft.Launcher.Gui.Wpf.Views;

public interface ILoginWindowDelegate : IWindowDelegate
{
	void SetCapsLockState(bool isKeyDown);

	void DisplayActivationCodeForm();

	void LockLoginButton(int delay);

	void DisplayCaptcha();

	void DisplayPhoneBindingForm(JToken geoInfo);

	void DisplayPhoneVerificationForm(string codeExpire);
}
