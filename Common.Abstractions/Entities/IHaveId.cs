namespace Common.Abstractions.Entities
{
	public interface IHaveId<T>
		where T : struct
	{
		T Id { get; }
	}
}