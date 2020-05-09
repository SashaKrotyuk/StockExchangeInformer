namespace Common.Algorithms.Clustering
{
	using System;

	public class ClusteringData<TData> : IComparable<ClusteringData<TData>>
	{
		private const double Epsilon = 0.0000000000001;

		public ClusteringData(TData data, double value)
		{
			this.Data = data;
			this.Value = value;
			this.Id = Guid.NewGuid();
		}

		public Guid Id { get; }

		public TData Data { get; protected set; }

		public double Value { get; }

		public override bool Equals(object obj)
		{
			return obj != null && this.Equals(obj as ClusteringData<TData>);
		}

		public bool Equals(ClusteringData<TData> data)
		{
			return data != null
				&& (ReferenceEquals(data, this)
					|| (Math.Abs(this.Value - data.Value) < Epsilon));
		}

		public override int GetHashCode()
		{
			var hash = 2357 ^ this.Value.GetHashCode();
			hash = (2357 * hash) ^ this.Id.GetHashCode();

			return hash;
		}

		public int CompareTo(ClusteringData<TData> other)
		{
			return this.Value > other.Value
				? 1
				: this.Value < other.Value ? -1 : 0;
		}
	}
}