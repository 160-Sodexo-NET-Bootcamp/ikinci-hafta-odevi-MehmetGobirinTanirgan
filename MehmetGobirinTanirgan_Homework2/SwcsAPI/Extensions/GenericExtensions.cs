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
            //Bu metot ile ilgili item listesini eşit kümelere listelere bölerek, bu list içine atacağız.
            var clusteredList = new List<List<T>>();

            //List'in uzunluğu.
            var incomingListLength = incomingList.Count;

            //Bir kümenin kaç adet item'dan oluşması gerektiğini bu şekilde hesapladım.
            var lengthOfCluster = (int)(Math.Round((decimal)incomingListLength / n, MidpointRounding.AwayFromZero)); 
           

            //Eğer ki bulduğum küme uzunluğu ile küme sayısının çarpımı, liste uzunluğumdan büyük veya eşit ise 
            // direkt olarak listeyi eşit kümelere ayırabilirim, son kümeyi eksik elemanlı dahi olsa Linq ile ayırabilmiş oluyorum.
            //Aksi takdirde, küme uzunluğu ile küme adedinin çarpımı, liste uzunluğumdan küçük ise bu noktada son kümeyi fazlalık
            //itemleri taşıyacak şekilde ayarladım.
            if (lengthOfCluster * n >= incomingListLength)
            {
                clusteredList = incomingList.CreateEqualClusters(n, lengthOfCluster);
            }
            else
            {
                //Burada tek fark son kümeyi TakeLast metotu ile kendim yakalayıp, for döngüsünde
                //son kümeyle ilgilenmemesini sağladım.
                var extraItems = incomingList.TakeLast(incomingListLength % n).ToList();// Fazlalık itemi aldım listeden.
                clusteredList = incomingList.CreateEqualClusters(n, lengthOfCluster);// Listeyi eşit parçalara böldüm.

                foreach (var item in extraItems) //Burda da fazlalık item/itemler son kümeye eklendi.
                {
                    clusteredList[clusteredList.Count - 1].Add(item);
                }
            }
            return clusteredList;
        }


        private static List<List<T>> CreateEqualClusters<T>(this List<T> incomingList, int n, int lengthOfCluster) where T : BaseEntity
        {
            var clusteredList = new List<List<T>>();
            for (int i = 0; i < n; i++)//Küme sayısı kadar dön.
            {
                //Küme boyutu kadar atla, küme boyutu kadar al mantığı uygulayarak listeyi parçaladım.
                clusteredList.Add(incomingList.Skip(i * lengthOfCluster).Take(lengthOfCluster).ToList());
            }
            return clusteredList;
        }
    }
}
