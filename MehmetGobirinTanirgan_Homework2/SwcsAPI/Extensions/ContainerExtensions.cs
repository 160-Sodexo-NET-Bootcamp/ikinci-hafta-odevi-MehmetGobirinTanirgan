using Data.DataModels;
using SwcsAPI.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwcsAPI.Extensions
{
    public static class ContainerExtensions
    {
       
        public static List<List<Container>> ToKMeansClusters(this List<Container> containers, int n)
        {
            var containerCt = containers.Count; //Container sayısı.
            var clusteredContainers = new List<List<Container>>(); //Result olarak döneceğim liste.
            List<List<Container>> snapshotOfClusteredContainers;// Algoritma içerisinde result listemi bir önceki haliyle
                                                                // kıyaslamak için tanımladığım liste.
            var clusterCenters = new List<LatLong>(); //Kümelerin merkezlerini tutan liste.


            //Başlangıç için n tane boş küme, yani liste ekledim. Bu n tane küme için başlangıç merkezlerini,
            //ilk n tane container'ın koordinatlarıyla eşleştirdim.
            for (int i = 0; i < n; i++)
            {
                // n tane boş liste ekleme.
                clusteredContainers.Add(new List<Container>());
                //Başlangıç merkezlerini ekleme.
                clusterCenters.Add(new LatLong { Latitude = containers[i].Latitude, Longitude = containers[i].Longitude });
            };

            do
            {
                // Burada result listemin algoritmaya girmeden önceki anlık halini kopyalamış oldum.
                snapshotOfClusteredContainers = clusteredContainers.ToList();

                // Burada da her algoritma girişi öncesinde result listemi resetleyerek, boş n tane küme haline getirip,
                // yeni oluşan küme merkezi koordinatlarına göre container'ların ilgili kümeye atılmasını sağladım.
                clusteredContainers = new List<List<Container>>().ResetClusteredListOfContainer(n);

                for (int i = 0; i < containerCt; i++) // İşlemi listedeki tüm containerlar için yapacağım.
                {
                    var distances = new List<double>(); // Container'ın n tane küme merkezine olan uzaklıklarını bu listede tutuyorum.
                    foreach (var clusterCenter in clusterCenters) //Sırasıyla container'ın her küme merkezine olan
                                                                  //uzaklıklarını bulup, distances listesine atıyorum.
                    {
                        var distanceToCluster = CalculateDistance(clusterCenter, containers[i]); //Distance bulma
                        distances.Add(distanceToCluster);
                    }
                    var indexForFarthestCluster = distances.IndexOf(distances.Min());//Burada da en yakın olduğu kümeyi bulmak
                                                                                     //amacıyla, en düşük mesafeye sahip olan
                                                                                     //sonucun indeksini çekiyorum.

                    clusteredContainers[indexForFarthestCluster].Add(containers[i]);//Burda da bulduğum küme index'i ile ilgili
                                                                                    //kümeye, ilgili container'ımı ekliyorum.
                                                                                    
                    
                    //Bu kısımda ise container en yakın olduğu kümeye eklendikten sonra, o kümenin yeni kitle merkezini hesaplayarak,
                    //küme merkezlerini sürekli güncel tutmuş oluyorum.
                    clusterCenters[indexForFarthestCluster] = clusteredContainers[indexForFarthestCluster].CalculateCenterOfCluster(); 
                }

            } while (!snapshotOfClusteredContainers.IsItSame(clusteredContainers, n));// Eğer ki yeni küme listem, bir önceki
                                                                                      // süreçte ki haliyle aynı ise devam etme,
                                                                                      // döngüyü kır. Çünkü artık küme merkezleri
                                                                                      // değişmeyecek ve stabil bir sonuca
                                                                                      // ulaşmışım anlamına gelecek.

            return clusteredContainers; // Algoritma tamamlandı. Result'ı dön.
        }

        #region K Means Helper Methods
        private static LatLong CalculateCenterOfCluster(this List<Container> containersOfCluster)
        {
            // Enlem ve boylam değerlerinin ortalamalarını yeni merkez olarak belirle
            var latitudeAvg = containersOfCluster.Average(x => x.Latitude);
            var longitudeAvg = containersOfCluster.Average(x => x.Longitude);
            return new LatLong { Latitude = latitudeAvg, Longitude = longitudeAvg }; //Yeni merkez koordinatını dön.
        }

        private static double CalculateDistance(LatLong clusterCenter, Container container)
        {
            // İki boyutlu düzlemde, herhangi iki nokta arasındaki mesafeyi veren denklem. 
            return Math.Sqrt(Math.Pow((double)(container.Latitude - clusterCenter.Latitude), 2) +
                 Math.Pow((double)(container.Longitude - clusterCenter.Longitude), 2));
        }

        private static List<List<Container>> ResetClusteredListOfContainer(this List<List<Container>> clusteredContainers, int n)
        {
            //Burası sadece algoritmanın başlangıcında küme listesini boşaltmak amacıyla yapıldı.
            for (int i = 0; i < n; i++)
            {
                clusteredContainers.Add(new List<Container>());
            }
            return clusteredContainers;
        }

        private static bool IsItSame(this List<List<Container>> snapshot, List<List<Container>> current, int n)
        {
            // Result listemdeki her bir küme(liste), bir önceki halinin kümeleriyle aynı ise true dön, değilse false dön.
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
