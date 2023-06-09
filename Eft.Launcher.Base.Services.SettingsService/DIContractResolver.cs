using System;
using Newtonsoft.Json.Serialization;

namespace Eft.Launcher.Base.Services.SettingsService;

public class DIContractResolver : DefaultContractResolver
{
	private readonly IServiceProvider _sp;

	public DIContractResolver(IServiceProvider sp)
	{
		_sp = sp;
	}

	protected override JsonObjectContract CreateObjectContract(Type objectType)
	{
		_ = objectType.Name;
		object s = _sp.GetService(objectType);
		JsonObjectContract jsonObjectContract;
		if (s != null)
		{
			jsonObjectContract = base.CreateObjectContract(s.GetType());
			jsonObjectContract.DefaultCreator = delegate
			{
				object result = s ?? _sp.GetService(objectType);
				s = null;
				return result;
			};
		}
		else
		{
			jsonObjectContract = base.CreateObjectContract(objectType);
		}
		return jsonObjectContract;
	}
}
