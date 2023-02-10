using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndManager : MonoBehaviour
{
    private const int MAX_LEVEL = 6;
    void FixedUpdate()
    {
        if (checkLevelEnded())
        {
            PlayerPrefs.DeleteKey("GameInfo");
            if (PlayerPrefs.GetInt("Level") == MAX_LEVEL)
            {
                SceneManager.LoadScene("VictoryScene");
            }
            else
            {
                SceneManager.LoadSceneAsync("LevelEndScene");
            }
        }
    }

    private bool checkLevelEnded()
    {
        foreach(GameObject tube in gameObject.GetComponent<TubeManager>().getTubes())
        {
            Tube curTube = tube.GetComponent<Tube>();
            if (!(curTube.isEmpty() || curTube.finished()))
            {
                return false;
            }
        }

        return true;
    }
}
