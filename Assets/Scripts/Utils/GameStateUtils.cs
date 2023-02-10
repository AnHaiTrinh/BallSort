using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateUtils : MonoBehaviour
{
    // private static HashSet<string> visitedStates = new HashSet<string>();
    private static Dictionary<string, Move> visitedStates = new Dictionary<string, Move>();
    private static Stack<Move> moves = new Stack<Move>();
    private static int cotCount;
    private const char specialChar = '.';
    private static Queue<string> queue = new Queue<string>();
    public class Move
    {
        private int from, to;
        public Move(int from, int to)
        {
            this.from = from;
            this.to = to;
        }

        public int getTo()
        {
            return to;
        }

        public int getFrom()
        {
            return from;
        }

        public string toString()
        {
            return "From: " + from + "   To: " + to;
        }

        public List<int> getMove()
        {
            return new List<int> { from, to };
        }
    }

    public static string createFromGameInfo(GameInfo gameInfo)
    {
        //visitedStates = new HashSet<string>();
        visitedStates = new Dictionary<string, Move>();
        moves = new Stack<Move>();
        cotCount = gameInfo.cotCount;
        string gameState = "";
        for (int i = 0; i < gameInfo.cotCount; i++)
        {
            int[] hoops = gameInfo.cots[i].hoops;
            int count = hoops.Length;
            for (int j = 0; j < 4 - count; j++)
            {
                gameState += specialChar;
            }
            foreach (int colorIndex in hoops)
            {
                gameState += colorIndex;
            }
        }
        return gameState;

    }
    //public static bool solveGame(string gameState)
    //{
        //if (isCompleted(gameState))
        //{
        //    return true;
        //}
        //foreach (Move move in getPossibleMoves(gameState))
        //{
        //    moves.Push(move);
        //    string newGameState = makeMove(gameState, move);
        //    if (!visitedStates.Contains(newGameState))
        //    {
        //        visitedStates.Add(newGameState);
        //        if (solveGame(newGameState))
        //        {
        //            return true;
        //        }
        //    }
        //    moves.Pop();
        //}
        //return false;
        //queue.Enqueue(gameState);
        //visitedStates[gameState] = null;
        //while (queue.Count > 0)
        //{
        //    string state = queue.Dequeue();
        //    if(isCompleted(state))
        //    {
        //        return true;
        //    }
        //    foreach (Move move in getPossibleMoves(state))
        //    {
        //        string newGameState = makeMove(state, move);
        //        if (!visitedStates.ContainsKey(newGameState))
        //        {
        //            visitedStates[newGameState] = state;
        //            queue.Enqueue(newGameState);
        //        }
        //    }
        //}
        //return false;
    //}

    //public static string generateGameState(int tubeCount, string gameState, int moves)
    //{
    //    cotCount = tubeCount;
    //    Queue<string> queue = new Queue<string>();
    //    queue.Enqueue(gameState);
    //    while (moves > 0 && queue)
    //    {
    //        moves -= 1:

    //    }
    //}

    public static string makeMove(string gameState, Move move)
    {
        int tubeFrom = 4 * move.getFrom();
        while (gameState[tubeFrom] == specialChar)
        {
            tubeFrom += 1;
        }
        char colorIndex = gameState[tubeFrom];
        gameState = gameState.Substring(0, tubeFrom) + specialChar + gameState.Substring(tubeFrom + 1);

        int tubeTo = 4 * move.getTo();
        int i = 0;
        for (;i < 4; i++)
        {
            if (gameState[tubeTo + i] != specialChar)
            {
                break;
            } 
        }
        tubeTo += i - 1;
        gameState = gameState.Substring(0, tubeTo) + colorIndex + gameState.Substring(tubeTo + 1);
        return gameState;
    }

    public static List<Move> getPossibleMoves(string gameState)
    {
        List<Move> res = new List<Move>();
        List<string> tubes = new List<string>();
        for (var i = 0; i < cotCount; i++)
        {
            tubes.Add(gameState.Substring(4 * i, 4));
        }
        for (var i = 0; i < cotCount; i++)
        {
            for (var j = 0; j < cotCount; j++)
            {
                if (j == i)
                {
                    continue;
                }
                if (canPush(tubes[i], tubes[j]) && !isRedundant(tubes[i], tubes[j]))
                {
                    res.Add(new Move(i, j));
                }
            }
        }
        return res;
    }

    public static bool isCompleted(string gameState)
    {
        for (int i = 0; i < cotCount; i++)
        {
            char color = gameState[4 * i];
            for (int j = 1; j < 4; j++)
            {
                if (gameState[4 * i + j] != color)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool isEmpty(string tube)
    {
        foreach (char c in tube)
        {
            if (c != specialChar)
            {
                return false;
            }
        }
        return true;
    }

    public static bool isFull(string tube)
    {
        foreach (char c in tube)
        {
            if (c == specialChar)
            {
                return false;
            }
        }
        return true;
    }

    public static char getTopColor(string tube)
    {
        for (var i = 0; i < 4; i++)
        {
            if (tube[i] != specialChar)
            {
                return tube[i];
            }
        }
        Debug.Log("Tube is empty");
        return specialChar;
    }

    public static bool canPush(string from, string to)
    {
        return !isEmpty(from) && (isEmpty(to) || (getTopColor(from) == getTopColor(to) && !isFull(to)));
    }

    public static int getNumColor(string tube)
    {
        HashSet<char> set = new HashSet<char>(tube);
        set.Remove(specialChar);
        return set.Count;
    }

    public static bool isRedundant(string from, string to)
    {
        //return getNumColor(from) <= 1 && getNumColor(to) != 1;
        int fromCount = getNumColor(from);
        int toCount = getNumColor(to);
        if (fromCount > 1)
        {
            return false;
        }
        else if (toCount != 1)
        {
            return true;
        }
        else
        {
            for (var i = 0; i < 4; i++)
            {
                if (from[i] != specialChar && to[i] == specialChar)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public static Queue<List<int>> displayMoves()
    {
        Stack<Move> ans = new Stack<Move>();
        ans.Push(moves.Pop());
        foreach (Move move in moves)
        {
            if (ans.Peek().getFrom() == move.getTo())
            {
                Move prev = ans.Pop();
                ans.Push(new Move(move.getFrom(), prev.getTo()));
            }
            else
            {
                ans.Push(move);
            }
        }

        Queue<List<int>> results = new Queue<List<int>>();
        while (ans.Count > 0)
        {
            Move move = ans.Pop();
            results.Enqueue(move.getMove());
            Debug.Log(move.toString());
        }
        return results;
    }
}
