using Data.DataModels;
using Data.DataModels.Base;
using SwcsAPI.Extensions.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwcsAPI.Extensions
{
    public static class LogicExtensions
    {
        public static List<List<T>> ToClusters<T>(this List<T> incomingList, int n) where T : BaseEntity
        {
            var clusteredList = new List<List<T>>();
            var containerCt = incomingList.Count;
            var lengthOfCluster = (int)(Math.Ceiling((double)containerCt / n));

            if (containerCt % n == 0)
            {
                for (int i = 0; i < n; i++)
                {
                    clusteredList.Add(incomingList.Skip(i * lengthOfCluster).Take(lengthOfCluster).ToList());
                }
            }
            else
            {
                var lastCluster = incomingList.TakeLast(containerCt % n).ToList();
                for (int i = 0; i < n - 1; i++)
                {
                    clusteredList.Add(incomingList.Skip(i * lengthOfCluster).Take(lengthOfCluster).ToList());
                }
                clusteredList.Add(lastCluster);
            }

            return clusteredList;
        }

        public static List<List<Container>> ToKMeansCluster(this List<Container> containers, int n)
        {
            var containerCt = containers.Count;
            var clusteredContainers = new List<List<Container>>();
            List<List<Container>> snapshotOfClusteredContainers;
            var flag = true;
            var clusterCenters = new List<LatLong>();

            for (int i = 0; i < n; i++)
            {
                clusteredContainers.Add(new List<Container>());
                clusterCenters.Add(new LatLong { Latitude = containers[i].Latitude, Longitude = containers[i].Longitude });
            };

            do
            {
                snapshotOfClusteredContainers = clusteredContainers.ToList();
                clusteredContainers = new List<List<Container>>().ResetClusteredListOfContainer(n);

                for (int i = 0; i < containerCt; i++)
                {
                    var distances = new List<double>();
                    foreach (var clusterCenter in clusterCenters)
                    {
                        var distanceToCluster = CalculateDistance(clusterCenter, containers[i]);
                        distances.Add(distanceToCluster);
                    }
                    var indexForFarthestCluster = distances.IndexOf(distances.Min());
                    clusteredContainers[indexForFarthestCluster].Add(containers[i]);
                }

                clusterCenters = clusteredContainers.CalculateCenterOfClusters(n);
                flag = snapshotOfClusteredContainers.IsSame(clusteredContainers, n);
            } while (!flag);

            return clusteredContainers;
        }

        #region K Means Helper Methods
        private static List<LatLong> CalculateCenterOfClusters(this List<List<Container>> clusteredContainers, int n)
        {
            var newCenters = new List<LatLong>();
            for (int i = 0; i < n; i++)
            {
                var latitudeAvg = clusteredContainers[i].Average(x => x.Latitude);
                var longitudeAvg = clusteredContainers[i].Average(x => x.Longitude);
                newCenters.Add(new LatLong { Latitude = latitudeAvg, Longitude = longitudeAvg });
            }
            return newCenters;
        }

        private static double CalculateDistance(LatLong clusterCenter, Container container)
        {
            return Math.Sqrt(Math.Pow((double)(container.Latitude - clusterCenter.Latitude), 2) +
                 Math.Pow((double)(container.Longitude - clusterCenter.Longitude), 2));
        }

        private static List<List<Container>> ResetClusteredListOfContainer(this List<List<Container>> clusteredContainers, int n)
        {
            for (int i = 0; i < n; i++)
            {
                clusteredContainers.Add(new List<Container>());
            }
            return clusteredContainers;
        }

        private static bool IsSame(this List<List<Container>> snapshot, List<List<Container>> current, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (!snapshot[i].SequenceEqual(current[i]))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

    }
}
