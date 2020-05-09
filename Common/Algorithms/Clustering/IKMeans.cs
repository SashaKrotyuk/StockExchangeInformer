namespace Common.Algorithms.Clustering
{
	using System.Collections.Generic;

	public interface IKMeans<TData>
    {
        ClusteringResult<TData> Cluster(int clustersCount, IEnumerable<ClusteringData<TData>> data);
    }
}