﻿namespace SEI.News.Service.Infrastructure
{
	using System;
	using System.Reflection;

	using Microsoft.AspNet.SignalR.Infrastructure;
	using Newtonsoft.Json.Serialization;

	public class SignalRContractResolver : IContractResolver
	{
		private readonly Assembly assembly;
		private readonly IContractResolver camelCaseContractResolver;
		private readonly IContractResolver defaultContractSerializer;

		public SignalRContractResolver()
		{
			defaultContractSerializer = new DefaultContractResolver();
			camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
			assembly = typeof(Connection).Assembly;
		}

		public JsonContract ResolveContract(Type type)
		{
			if (type.Assembly.Equals(assembly))
			{
				return defaultContractSerializer.ResolveContract(type);
			}

			return camelCaseContractResolver.ResolveContract(type);
		}
	}
}