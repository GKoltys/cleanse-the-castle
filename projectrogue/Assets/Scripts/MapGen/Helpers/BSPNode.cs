using UnityEngine;
using System.Collections.Generic;

public class BSPNode
{
    public RectInt bounds;
    public BSPNode left;
    public BSPNode right;
    public RectInt room;
    public Vector2Int connector;
    public bool hasConnector;

    public bool IsLeaf => left == null && right == null;

    public BSPNode(RectInt bounds)
    {
        this.bounds = bounds;
    }
}
