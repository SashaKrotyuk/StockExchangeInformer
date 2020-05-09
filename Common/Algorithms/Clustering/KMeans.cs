namespace Common.Algorithms.Clustering
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;

	using log4net;

	public class KMeans<TData> : IKMeans<TData>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		
        public KMeans()
        {
            this.MaxIterationsCount = 20;
        }

		public int MaxIterationsCount { get; set; }

		public ClusteringResult<TData> Cluster(int clustersCount, IEnumerable<ClusteringData<TData>> data)
        {
            var sw = new Stopwatch();
            sw.Start();
            
            var dataIndexes = this.CreateMapDataToIndex(data);
            Log.DebugFormat("KMeans: start clustering {0} data items, number of clusters set to {1}", dataIndexes.Count, clustersCount);
            
            // Number of clusters can not be more than number of data.
            clustersCount = Math.Min(dataIndexes.Count, clustersCount);
            Log.DebugFormat("KMeans: number of clusters changed to {0}", clustersCount);
            var clustering = this.InitiallyDevideDataByClusters(clustersCount, dataIndexes);
            
            // When clustersCount == 1 then do not need to clustering at all, just return given data as one cluster.
            if (clustersCount > 1)
            {
                var means = new double[clustersCount];
                var centroids = new ClusteringData<TData>[clustersCount];

                this.CalculateCentroids(dataIndexes, clustering, means, centroids);
                var interationNum = 0;

                while (this.RecalculateClusters(dataIndexes, clustering, centroids) && interationNum++ < this.MaxIterationsCount)
                {
                    this.CalculateCentroids(dataIndexes, clustering, means, centroids);
                }
            }

            var result = this.BuildClusteredResult(clustersCount, dataIndexes, clustering);
            sw.Stop();
            Log.DebugFormat("KMeans: end clustering {0} data items, took {1} ms", dataIndexes.Count, sw.ElapsedMilliseconds);

            return result;
        }

        private ClusteringResult<TData> BuildClusteredResult(int clustersCount, Dictionary<Guid, ClusteringData<TData>> dataIndexes, Dictionary<Guid, int> clustering)
        {
            var clusteredData = new List<ClusteringData<TData>>[clustersCount];

            foreach (var pair in clustering)
            {
                var clusterData = clusteredData[pair.Value];

                if (clusterData == null)
                {
                    clusterData = new List<ClusteringData<TData>>();
                    clusteredData[pair.Value] = clusterData;
                }

                clusterData.Add(dataIndexes[pair.Key]);
            }

            return new ClusteringResult<TData> { ClusteredData = clusteredData.Where(o => o != null).ToArray() };
        }

        private bool RecalculateClusters(Dictionary<Guid, ClusteringData<TData>> dataIndexes, Dictionary<Guid, int> clustering, ClusteringData<TData>[] centroids)
        {
            var ret = false;
            var clustersCount = centroids.Length;

            // Iterate over all data and check if data closer to the centroid
            // of another cluster then move it there.
            foreach (var pair in dataIndexes)
            {
                var minDiff = double.MaxValue;
                var closestClusterNum = 0;

                // Find shortest distance to the clusters' centroid.
                for (var i = 0; i < clustersCount; i++)
                {
                    // Cluster can be empty.
                    if (centroids[i] == null)
                    {
                        continue;
                    }

                    var diff = Math.Abs(pair.Value.Value - centroids[i].Value);
                        
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        closestClusterNum = i;
                    }
                }

                // If find better dinstance then change cluster for the data.
                if (closestClusterNum != clustering[pair.Key])
                {
                    clustering[pair.Key] = closestClusterNum;
                    ret = true;
                }
            }

            return ret;
        }

        private void CalculateCentroids(Dictionary<Guid, ClusteringData<TData>> dataIndexes, Dictionary<Guid, int> clustering, double[] means, ClusteringData<TData>[] centroids)
        {
            this.CalculateMeans(dataIndexes, clustering, means);

            for (var i = 0; i < centroids.Length; i++)
            {
                var centroid = this.CalculateCentroid(dataIndexes, clustering, i, means);
                centroids[i] = centroid;
            }
        }

        private ClusteringData<TData> CalculateCentroid(Dictionary<Guid, ClusteringData<TData>> dataIndexes, Dictionary<Guid, int> clustering, int clusterNum, double[] means)
        {
            var centroid = default(ClusteringData<TData>);
            var minDist = double.MaxValue;
            var clusterMean = means[clusterNum];

            // Calculate the centroid which is the data that are closest to the given cluster mean.
            foreach (var dataPair in dataIndexes.Where(o => clustering[o.Key] == clusterNum))
            {
                var diff = Math.Abs(dataPair.Value.Value - clusterMean);

                if (diff < minDist)
                {
                    minDist = diff;
                    centroid = dataPair.Value;
                }
            }

            return centroid;
        }

        private void CalculateMeans(Dictionary<Guid, ClusteringData<TData>> dataIndexes, Dictionary<Guid, int> clustering, double[] means)
        {
            var clustersCount = means.Length;
            
            for (var i = 0; i < clustersCount; i++)
            {
                means[i] = 0.0;
            }

            var dataInClusterCount = new int[clustersCount];

            // Sum data values for each cluster and store values count in dataInClusterCount. 
            foreach (var pair in dataIndexes)
            {
                var clusterNumber = clustering[pair.Key];
                ++dataInClusterCount[clusterNumber];

                means[clusterNumber] += pair.Value.Value;
            }

            // Calculate mean value for each cluster.
            for (var i = 0; i < clustersCount; i++)
            {
                if (dataInClusterCount[i] == 0)
                {
                    Log.DebugFormat("KMeans: can't calculate mean for cluster number {0}, it does not have any data", i);
                }
                else
                {
                    means[i] /= dataInClusterCount[i];
                }
            }
        }

        private Dictionary<Guid, int> InitiallyDevideDataByClusters(int clustersCount, Dictionary<Guid, ClusteringData<TData>> dataIndexes)
        {
            var dataLen = dataIndexes.Count;

            if (dataLen < clustersCount)
            {
                Log.DebugFormat("KMeans: number of data items({0}) is less than number of clusters({1})", dataLen, clustersCount);
            }

            var clustering = new Dictionary<Guid, int>(dataLen);
            var pos = 0;
            var dataIndexesEnumerator = dataIndexes.GetEnumerator();

            // Assign first n-datas to first n-clusters (where n = clustersCount).
            while (pos < clustersCount && dataIndexesEnumerator.MoveNext())
            {
                clustering.Add(dataIndexesEnumerator.Current.Key, pos++);
            }

            // Assign each data to a random cluster.
            var random = new Random(0);

            while (dataIndexesEnumerator.MoveNext())
            {
                clustering.Add(dataIndexesEnumerator.Current.Key, random.Next(0, clustersCount));
            }

			dataIndexesEnumerator.Dispose();

            return clustering;
        }

        private Dictionary<Guid, ClusteringData<TData>> CreateMapDataToIndex(IEnumerable<ClusteringData<TData>> data)
        {
            // Generate map which helps to find data by its index.
            return data.ToDictionary(clusteringData => clusteringData.Id);
        }
    }
}
