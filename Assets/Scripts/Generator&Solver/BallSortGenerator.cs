using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSortGenerator
{
    //public GState generateLevels(int colorCount, int cotCount, int moves)
    //{

    //}
    public void exportToJsonFile(int levelIndex, GState gameState)
    {
        FileUtils.writeFile(string.Format("Assets/Levels/Level{0}.txt", levelIndex), gameState.encodeToJSON(levelIndex));
    }
}
