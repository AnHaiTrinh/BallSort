using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallSortSolver : MonoBehaviour
{
    private TubeManager tubeManager;
    private Queue<GState> queue;
    private HashSet<string> visitedStates;
    private Stack<Move> moves;
    private GameObject ball;
    private int count = 0;
    public void solveGameBFS()
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        tubeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TubeManager>();
        visitedStates = new HashSet<string>();
        GState gameState = GState.createFromGameInfo(GameInfo.createFromJSON(PlayerPrefs.GetString("GameInfo")));

        queue = new Queue<GState>();
        queue.Enqueue(gameState);
        visitedStates.Add(gameState.boardState);
        count++;


        while (queue.Count > 0)
        {
            GState currentGState = queue.Dequeue();
            if (currentGState.isCompleted())
            {
                moves = displayMovesBFS(currentGState);
                watch.Stop();
                Debug.Log($"Execution time: {watch.ElapsedMilliseconds} ms");
                return;
            }
            foreach (GState childState in currentGState.getChildren())
            {
                if (!visitedStates.Contains(childState.boardState))
                {
                    count++;
                    visitedStates.Add(childState.boardState);
                    queue.Enqueue(childState);
                }
            }
        }
        Debug.Log("Unwinnable game state");
    }

    public GState recursiveSolver(GState gameState)
    {
        count++;
        if (gameState.isCompleted())
        {
            return gameState;
        }
        foreach (GState childState in gameState.getChildren())
        {
            if (!visitedStates.Contains(childState.boardState))
            {
                visitedStates.Add(childState.boardState);
                GState finalState = recursiveSolver(childState);
                if (finalState != null)
                {
                    return finalState;
                }
            }
        }
        return null;
    }
    public void solveGameDFS()
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        tubeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TubeManager>();
        visitedStates = new HashSet<string>();
        GState gameState = GState.createFromGameInfo(GameInfo.createFromJSON(PlayerPrefs.GetString("GameInfo")));

        GState finalState = recursiveSolver(gameState);
        if (finalState != null)
        {
            moves = displayMovesDFS(finalState);
        }
        else
        {
            Debug.Log("Unwinnable game state");
        }
        watch.Stop();
        Debug.Log($"Execution time: {watch.ElapsedMilliseconds} ms");
    }
    public Stack<Move> displayMovesDFS(GState finalGState)
    {
        Debug.Log("Count: " + count);
        Stack<Move> ans = new Stack<Move>();
        GState curState = finalGState;
        while (curState.parent != null)
        {
            if (ans.Count > 0 && ans.Peek().getFrom() == curState.tail.getTo())
            {
                Move lastMove = ans.Pop();
                ans.Push(new Move(curState.tail.getFrom(), lastMove.getTo()));
            }
            else
            {
                ans.Push(curState.tail);
            }
            curState = curState.parent;
        }
        Debug.Log("Moves: " + ans.Count);
        return ans;
    }
    public Stack<Move> displayMovesBFS(GState finalGState)
    {
        Debug.Log("Count: " + count);
        Stack<Move> ans = new Stack<Move>();
        GState curGState = finalGState;
        while (curGState.parent != null)
        {   
            ans.Push(curGState.tail);
            curGState = curGState.parent;
        }
        Debug.Log("Moves: " + ans.Count);
        return ans;
    }

    void Update()
    {
        if (moves != null)
        {
            if (ball != null && ball.GetComponent<MoveComponent>().isMoving())
            {
                return;
            }
            else
            {
                if (moves.Count > 0)
                {
                    Move move = moves.Pop();
                    int from = move.getFrom();
                    int to = move.getTo();
                    ball = tubeManager.getTubes()[from].GetComponent<Tube>().removeBall();
                    tubeManager.getTubes()[to].GetComponent<Tube>().addball(ball);
                }
            }
        }
    }
}