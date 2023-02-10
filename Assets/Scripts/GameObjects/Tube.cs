using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tube : MonoBehaviour
{
    [SerializeField]
    private GameObject ballProtoype;
    private Stack<GameObject> balls = new Stack<GameObject>(4);

    private bool isFinished;

    public void initializeBalls(int[] colorIndices)
    {
        if (colorIndices.Length > 4)
        {
            Debug.Log("There can be at most 4 balls in each tube");
        }
        for (int i = 0; i < colorIndices.Length; i++)
        {
            GameObject newBall = Instantiate(ballProtoype, transform);
            newBall.GetComponent<Ball>().setColorIndex(colorIndices[i]);
            newBall.transform.position = transform.position + new Vector3(0, 0.7f * i - 1.1f, 0);
            balls.Push(newBall);
        }
    }
    public GameObject removeBall()
    {
            GameObject poppedBall = balls.Pop();
            poppedBall.GetComponent<MoveComponent>().thenMoveTo(new Vector3(transform.position.x, transform.position.y + 2.0f, 0f), 0.25f - balls.Count * 0.03f);
            return poppedBall;
    }
    public GameObject addball(GameObject ball)
    {
        MoveComponent move = ball.GetComponent<MoveComponent>();
        move.thenMoveTo(new Vector3(transform.position.x, transform.position.y + 2.0f, 0f), 0.4f)
            .thenMoveTo(new Vector3(transform.position.x, transform.position.y -1.1f + 0.7f * balls.Count, 0f), 0.25f - balls.Count * 0.03f);
        balls.Push(ball);
        return ball;
    }

    public bool isEmpty()
    {
        return balls.Count == 0;
    }

    public bool canPushBall(Ball b)
    {
        return isEmpty() || (b.getColorIndex() == getTopBallColor() && !isFull());
    }

    public bool isFull()
    {
        return balls.Count == 4;
    }

    public int getTopBallColor()
    {
        return balls.Peek().GetComponent<Ball>().getColorIndex();
    }

    public bool finished()
    {
        return isFinished;
    }

    void FixedUpdate()
    {
         if (isFinished)
         {
            //Debug.Log("Finished");
         }
         else
         {
            if (balls.Count < 4)
            {
                return;
            }
            int color = balls.Peek().GetComponent<Ball>().getColorIndex();
            foreach (GameObject ball in balls)
            {
                if (ball.GetComponent<Ball>().getColorIndex() != color || ball.GetComponent<MoveComponent>().isMoving())
                {
                    return;
                }
            }
            isFinished = true;
         }
    }

    public int[] getBallsColors()
    {
        if (balls.Count == 0)
        {
            return Array.Empty<int>();
        }
        else
        {
            List<int> ballsColors = new List<int>();
            foreach(GameObject ball in balls)
            {
                ballsColors.Add(ball.GetComponent<Ball>().getColorIndex());
            }
            return ballsColors.ToArray();
        }

    }
}
