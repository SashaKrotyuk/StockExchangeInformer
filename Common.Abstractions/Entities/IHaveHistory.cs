namespace Common.Abstractions.Entities
{
	using System;

	public interface IHaveHistory : IHaveCreationDate
	{
		DateTime LastModificationDate { get; }
	}
}