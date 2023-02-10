using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public List<Color> listColor = new List<Color>();
    private int colorIndex;

    private SpriteRenderer ballSprite;
    private MoveComponent ballMovement;
    void Awake()
    {
        ballSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        ballMovement = gameObject.GetComponent<MoveComponent>();
    }

    public int getColorIndex()
    {
        return colorIndex;
    }
    public void setColorIndex(int c)
    {
        this.colorIndex = c;
        ballSprite.color = listColor[c];
    }
}
