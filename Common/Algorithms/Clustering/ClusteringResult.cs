namespace Common.Algorithms.Clustering
{
	using System.Collections.Generic;

	public class ClusteringResult<TData>
    {
        public List<ClusteringData<TData>>[] ClusteredData { get; set; }
    }
}
