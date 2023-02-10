using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListUtils
{
    private const int LOOPS = 100;
    public static List<int> Shuffle(int n, int duplicate)
    {
        List<int> list = new List<int>();
        for (int j = 0; j < duplicate; j++)
        {
            for (int i = 0; i < n; i++)
            {
                list.Add(i);
            }
        }
        for (int i = 0; i < LOOPS; i++)
        {
            int num = n;
            while (num > 1)
            {
                num--;
                int k = Random.Range(0, num + 1);
                int tmp = list[k];
                list[k] = list[num];
                list[num] = tmp;
            }
            if (isValid(list, duplicate))
            {
                break;
            }
        }
        return list;
    }

    private static bool isValid(List<int> list, int duplicate)
    {
        for (int i = 0; i < list.Count / duplicate; i+= duplicate)
        {
            int cur = list[i];
            int j = 1;
            for(; j < duplicate; j++)
            {
                if (list[i+j] != cur)
                {
                    break;
                }
            }
            if (j == duplicate)
            {
                return false;
            }
        }
        return true;
    }
}
