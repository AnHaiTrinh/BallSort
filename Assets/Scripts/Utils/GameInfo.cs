using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInfo
{

    public int levelIndex;
    public int colorCount;
    public int cotCount;
    public Cot[] cots;

    [System.Serializable]
    public class Cot
    {
        public int[] hoops;
    }

    public static GameInfo createFromJSON(string json)
    {
        return JsonUtility.FromJson<GameInfo>(json);
    }

    public string saveToJson()
    {
        return JsonUtility.ToJson(this);
    }

}
