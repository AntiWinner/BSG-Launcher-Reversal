using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Eft.Launcher;

public class SelectiveContractResolver : DefaultContractResolver
{
	private readonly string[] m__E000;

	public SelectiveContractResolver(params string[] propertiesToInclude)
	{
		this.m__E000 = propertiesToInclude;
		base.IgnoreSerializableInterface = true;
		base.IgnoreSerializableAttribute = true;
	}

	protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
	{
		return (from p in base.CreateProperties(type, memberSerialization)
			where this.m__E000.Contains(p.PropertyName)
			select p).ToList();
	}

	[CompilerGenerated]
	private bool _E000(JsonProperty p)
	{
		return this.m__E000.Contains(p.PropertyName);
	}
}
