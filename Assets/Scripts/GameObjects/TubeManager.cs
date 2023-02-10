using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tubePrototype;

    public int level;
    //private int numColors = 5;
    //private int numTubes = 8;

    public GameInfo gameInfo;

    private GameObject[] tubes;

    private GameObject poppedBall;
    private Tube selectedTube;
    private Vector3 screenCenterPosition;
    
    public GameObject[] getTubes()
    {
        return tubes;
    }

    void Awake()
    {
        screenCenterPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2 + 200, 0));
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            level = 1;
            PlayerPrefs.SetInt("Level", 1);
        }
        
        string gameInfoString;
        if (PlayerPrefs.HasKey("GameInfo"))
        {
            gameInfoString = PlayerPrefs.GetString("GameInfo");
        }
        else
        {
            gameInfoString = FileUtils.readFile(string.Format("Assets/Levels/Level{0}.txt", level));
            PlayerPrefs.SetString("GameInfo", gameInfoString);
        }
        gameInfo = GameInfo.createFromJSON(gameInfoString);
        int colorCount = gameInfo.colorCount;
        int cotCount = gameInfo.cotCount;
        tubes = new GameObject[cotCount];

        int cotCountFirstRow = cotCount / 2;
        int cotCountSecondRow = cotCount - cotCountFirstRow;
        float tubeWidth = tubePrototype.GetComponent<BoxCollider2D>().size.x;
        float tubeHeight = tubePrototype.GetComponent<BoxCollider2D>().size.y;
        float colPad = Screen.width * (0.26f - 0.02f * cotCountFirstRow);
        float rowPad = Screen.height * 0.4f;

        float firstRowLength = (tubeWidth + colPad) * cotCountFirstRow - colPad;
        float secondRowLength = (tubeWidth + colPad) * cotCountSecondRow - colPad;
        float colLength = 2 * tubeHeight + rowPad;
        int i = 0;
        for (; i < cotCountFirstRow; i++)
        {
            tubes[i] = Instantiate(tubePrototype);
            Vector3 tubePosition = convertToWorld(new Vector3((tubeWidth - firstRowLength) / 2 + i * (colPad + tubeWidth), (colLength - tubeHeight) / 2)
                + new Vector3(Screen.width / 2, Screen.height / 2));
            tubes[i].GetComponent<Transform>().position = tubePosition;
            tubes[i].GetComponent<Tube>().initializeBalls(gameInfo.cots[i].hoops);
        }

        for(; i < cotCount; i++)
        {
            tubes[i] = Instantiate(tubePrototype);
            Vector3 tubePosition = convertToWorld(new Vector3((tubeWidth - secondRowLength) / 2 + (i - cotCountFirstRow) * (colPad + tubeWidth), (tubeHeight - colLength) / 2)
                + new Vector3(Screen.width / 2, Screen.height / 2));
            tubes[i].GetComponent<Transform>().position = tubePosition;
            tubes[i].GetComponent<Tube>().initializeBalls(gameInfo.cots[i].hoops);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTouchDown(GetMouseWorldPosition());
            saveGameInfo();
        }
        
    }

    private Vector3 convertToWorld(Vector3 gamePosition)
    {
        return Camera.main.ScreenToWorldPoint(gamePosition) - screenCenterPosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return mouseWorldPos;
    }

    private void OnTouchDown(Vector3 position)
    {
        Vector2 position2D = new Vector2(position.x, position.y);
        RaycastHit2D hit = Physics2D.Raycast(position2D, Vector2.zero);
        if (hit.collider != null)
        {
            if (poppedBall)
            {
                Tube newlySelectedTube = hit.collider.gameObject.GetComponent<Tube>();

                if (newlySelectedTube.canPushBall(poppedBall.GetComponent<Ball>()))
                {
                    newlySelectedTube.GetComponent<Tube>().addball(poppedBall);
                    selectedTube = newlySelectedTube;
                }
                else
                {
                    selectedTube.GetComponent<Tube>().addball(poppedBall);
                }
                poppedBall = null;
            }
            else
            {
                selectedTube = hit.collider.gameObject.GetComponent<Tube>();
                if (! selectedTube.isEmpty())
                {
                    poppedBall = selectedTube.removeBall();
                }
            }
        }
    }

    private void saveGameInfo()
    {
        PlayerPrefs.SetInt("Level", level);
        for(int i = 0; i < gameInfo.cotCount; i++)
        {
            Tube t = tubes[i].GetComponent<Tube>();

            gameInfo.cots[i].hoops =  t.getBallsColors();  
        }
        PlayerPrefs.SetString("GameInfo", gameInfo.saveToJson());
    } 
}
