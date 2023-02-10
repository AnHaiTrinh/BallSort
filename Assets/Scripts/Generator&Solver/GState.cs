using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GState
{
    public string boardState;
    public int cotCount;
    public int colorCount;
    public Move tail;
    public GState parent;
    private const char specialChar = '.';
    
    public GState(string boardState, int cotCount, int colorCount, Move tail = null, GState parent = null)
    {
        this.cotCount = cotCount;
        this.colorCount = colorCount;
        this.boardState = boardState;
        this.tail = tail;
        this.parent = parent;
    }

    public static GState createFromGameInfo(GameInfo gameInfo)
    {
        //visitedStates = new HashSet<string>();
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
        return new GState(gameState, gameInfo.cotCount, gameInfo.colorCount);

    }
    public List<GState> getChildren()
    {
        List<GState> children = new List<GState>();
        foreach(Move move in getPossibleMoves())
        {
            string childState = makeMove(move);
            children.Add(new GState(childState, cotCount, colorCount, move, this));
        }
        return children;
    }

    public List<Move> getPossibleMoves()
    {
        List<Move> res = new List<Move>();
        List<string> tubes = new List<string>();
        for (var i = 0; i < cotCount; i++)
        {
            tubes.Add(boardState.Substring(4 * i, 4));
        }
        for (var i = 0; i < cotCount; i++)
        {
            bool hasOneColor = false;
            List<Move> tmpResults = new List<Move>();
            for (var j = 0; j < cotCount; j++)
            {
                if (hasOneColor)
                    break;
                if (j == i)
                    continue;
                if (canPush(tubes[i], tubes[j]) && !isRedundant(tubes[i], tubes[j]))
                {
                    if (getNumColor(tubes[j]) == 1)
                    {
                        hasOneColor = true;
                        tmpResults.Clear();
                        tmpResults.Add(new Move(i, j));
                    }
                    else 
                    {
                        tmpResults.Add(new Move(i, j));
                    }
                    
                }
            }
            res.AddRange(tmpResults);
        }
        return res;
    }

    public string makeMove(Move move)
    {
        string newState = boardState;
        int tubeFrom = 4 * move.getFrom();
        while (boardState[tubeFrom] == specialChar)
        {
            tubeFrom += 1;
        }
        char colorIndex = boardState[tubeFrom];
        newState = newState.Substring(0, tubeFrom) + specialChar + newState.Substring(tubeFrom + 1);
        int tubeTo = 4 * move.getTo();
        int i = 0;
        for (; i < 4; i++)
        {
            if (boardState[tubeTo + i] != specialChar)
            {
                break;
            }
        }
        tubeTo += i - 1;
        newState = newState.Substring(0, tubeTo) + colorIndex + newState.Substring(tubeTo + 1);
        return newState;
    }

    public bool isCompleted()
    {
        for (int i = 0; i < cotCount; i++)
        {
            char color = boardState[4 * i];
            for (int j = 1; j < 4; j++)
            {
                if (boardState[4 * i + j] != color)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool isEmpty(string tube)
    {
        return getSize(tube) == 0;
    }

    public bool isFull(string tube)
    {
        return getSize(tube) == 4;
    }

    public char getTopColor(string tube)
    {
        for (var i = 0; i < 4; i++)
        {
            if (tube[i] != specialChar)
            {
                return tube[i];
            }
        }
        //Debug.Log("Tube is empty");
        return specialChar;
    }

    public int getSize(string tube)
    {
        int i = 0;
        while(i < 4 && tube[i] == specialChar)
        {
            i++;
        }
        return 4 - i;
    }

    public HashSet<char> getTopcolors()
    {
        HashSet<char> res = new HashSet<char>();
        for(var i = 0; i < cotCount; i++)
        {
            res.Add(getTopColor(boardState.Substring(4 * i, 4)));
        }
        return res;
    }
    public char getSecondColor(string tube)
    {
        int i = 0;
        while (i < 4 && tube[i] == specialChar)
            i++;
        if (i == 4)
        {
            Debug.Log("Tube empty");
            return specialChar;
        }
        char topColor = tube[i];
        while (i < 4 && tube[i] == topColor)
            i++;
        if (i == 4)
        {
            Debug.Log("Tube has one color");
            return topColor;
        }
        return tube[i];
    }

    public int getConsecutiveTopColor(string tube)
    {
        char topColor = specialChar;
        int i = 0;
        int ans = 0;
        while (i < 4)
        {
            if (tube[i] != specialChar)
            {
                if (topColor == specialChar)
                {
                    topColor = tube[i];
                    ans = 1;
                }
                else
                {
                    if (tube[i] == topColor)
                    {
                        ans++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            i++;
        }
        return ans;
    }

    public bool canPush(string from, string to)
    {
        return !isEmpty(from) && (isEmpty(to) || (getTopColor(from) == getTopColor(to) && !isFull(to)));
    }

    public int getNumColor(string tube)
    {
        HashSet<char> set = new HashSet<char>(tube);
        set.Remove(specialChar);
        return set.Count;
    }

    public bool isRedundant(string from, string to)
    {
        //return getNumColor(from) <= 1 && getNumColor(to) != 1;
        int fromCount = getNumColor(from);
        int toCount = getNumColor(to);
        if (fromCount > 1)
        {
            if (getConsecutiveTopColor(from) + getSize(to) > 4)
            {
                return true;
            }
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

    public string encodeToJSON(int levelIndex)
    {
        string res = string.Format("{\"levelIndex\": {0}, \"colorCount\": {1}, \"cotCount\": {2}, \"cots\": [", levelIndex, colorCount, colorCount);
        for(var i = 0; i < cotCount; i += 1)
        {
            string tube = "{\"hoops\": [";
            foreach(char c in boardState.Substring(i ,i + 4))
            {
                if (c != specialChar)
                {
                    tube += c + ", ";
                }
            }
            tube = tube.Substring(0, tube.Length - 2) + "]}, ";
            res += tube;
        }
        res = res.Substring(0, res.Length - 2) + "]}";
        return res;
    }
}
