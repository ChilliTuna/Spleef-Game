using System.Collections.Generic;

public static class ToolFuncs
{
    public static void Shuffle<T>(this List<T> list, System.Random rand = null)
    {
        if (rand != null)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        else
        {
            System.Random targetOrderRand = new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = targetOrderRand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}