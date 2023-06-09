using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Eft.Launcher.Base.Services.SettingsService;

internal class TaggedJsonContractResolver : DefaultContractResolver
{
	public string[] TagsToSerialize { get; }

	public TaggedJsonContractResolver(params string[] tagsToSerialize)
	{
		TagsToSerialize = tagsToSerialize;
	}

	protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
	{
		IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
		foreach (JsonProperty item in list)
		{
			if (!(item.AttributeProvider.GetAttributes(typeof(TagAttribute), inherit: false).FirstOrDefault() is TagAttribute tagAttribute) || tagAttribute.Tags == null || !tagAttribute.Tags.Intersect(TagsToSerialize).Any())
			{
				item.Ignored = true;
			}
		}
		return list;
	}
}
