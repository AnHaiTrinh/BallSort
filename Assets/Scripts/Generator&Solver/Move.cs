using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Move
{
    private int from;
    private int to;
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
        return string.Format("{0}-{1}", from, to);
    }

    public List<int> getMove()
    {
        return new List<int> { from, to };
    }
}