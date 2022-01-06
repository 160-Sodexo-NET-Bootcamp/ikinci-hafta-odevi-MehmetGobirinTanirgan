using Data.DataModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwcsAPI.Extensions
{
    public static class GenericExtensions
    {
        public static List<List<T>> ToClusters<T>(this List<T> incomingList, int n) where T : BaseEntity
        {
            //Bu metot ile ilgili container listesini eşit kümelere bölüyoruz.
            var clusteredList = new List<List<T>>();
            var containerCt = incomingList.Count;
            var lengthOfCluster = (int)(Math.Ceiling((double)containerCt / n)); //Bir kümenin kaç adet container'dan
                                                                                //oluşması gerektiğini bu şekilde hesapladım.

            //Eğer ki container sayısı, küme sayısına tam bölünüyorsa direkt olarak container listesini parçalayabiliriz demektir. Aksi takdirde son küme, diğer kümelerden eksik adette olabilecek şekilde ayarlamak istedim.
            if (containerCt % n == 0)
            {
                for (int i = 0; i < n; i++) //Küme sayısı kadar dön.
                {
                    //Küme boyutu kadar atla, küme boyutu kadar al mantığı uygulayarak listeyi parçaladım.
                    clusteredList.Add(incomingList.Skip(i * lengthOfCluster).Take(lengthOfCluster).ToList());
                }
            }
            else
            {
                //Burada tek fark son kümeyi TakeLast metotu ile kendim yakalayıp, for döngüsünde de n-1 sınırı koyarak
                //son kümeyle ilgilenmemesini sağladım.
                var lastCluster = incomingList.TakeLast(containerCt % n).ToList();
                for (int i = 0; i < n - 1; i++)
                {
                    clusteredList.Add(incomingList.Skip(i * lengthOfCluster).Take(lengthOfCluster).ToList());
                }
                clusteredList.Add(lastCluster);
            }

            return clusteredList;
        }

    }
}
