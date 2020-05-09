namespace Common.Abstractions.Entities
{
	// Needed for DDD restriction that states:
	// only aggregate roots can be stored in the database
	public interface IAggregateRoot
	{
	}
}