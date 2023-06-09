using System;
using System.Collections.Generic;

namespace Eft.Launcher.Base.Services.SettingsService;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
internal class TagAttribute : Attribute
{
	public IReadOnlyCollection<string> Tags { get; set; }

	public TagAttribute(params string[] tags)
	{
		Tags = (IReadOnlyCollection<string>)(object)tags;
	}
}
