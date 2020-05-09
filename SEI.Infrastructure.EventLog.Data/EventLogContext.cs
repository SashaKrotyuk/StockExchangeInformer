namespace SEI.Infrastructure.EventLog.Data
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Reflection;

	using Contracts.Model;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization;
	using MongoDB.Driver;

	public class EventLogContext
	{
		public static IMongoDatabase GetDatabase()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["EventLogDB"].ConnectionString;
			var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
			var client = new MongoClient(settings);
			MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
			BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

			////var coreEvents = Assembly.GetAssembly(typeof(AMP.Core.Configuration))
			////		.GetTypes()
			////		.Where(type => typeof(Common.Abstractions.IEvent).IsAssignableFrom(type))
			////		.ToList();
			////var flagEvents = Assembly.GetAssembly(typeof(AMP.Flag.Core.Configuration))
			////		.GetTypes()
			////		.Where(type => typeof(Common.Abstractions.IEvent).IsAssignableFrom(type))
			////		.ToList();
			////var applicationEvents = Assembly.GetAssembly(typeof(AMP.ApplicationEvents.Configuration))
			////		.GetTypes()
			////		.Where(type => typeof(Common.Abstractions.IEvent).IsAssignableFrom(type))
			////		.ToList();

			var events = new List<Type>();
			////events.AddRange(coreEvents);
			////events.AddRange(flagEvents);
			////events.AddRange(applicationEvents);

			foreach (var @event in events)
			{
				BsonClassMap.LookupClassMap(@event);
			}

			BsonClassMap.RegisterClassMap<Contracts.Model.EventInfo>(x =>
			{
				x.AutoMap();
			});

			return client.GetDatabase("EventLogDB");
		}
	}
}