using System.Collections.Generic;
using UnityEngine;

// procedural generation with BSP algorithm
// https://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation
public static class ProceduralGenerator
{
    public static MapData GenerateFloor(int w, int h, int pad, int bspMaxDepth, int minLeafSize, int minRoomSize, int maxRoomSize)
    {
        // create a room with tiles set to wall and the root partition
        Init(w, h, pad, out MapData map, out BSPNode root);

        // split recursively until each sub-dungeon is approx size of a room
        Split(root, 0, bspMaxDepth, minLeafSize);

        // create random-size room in each sub-dungeon
        Carve(map, root, minRoomSize, maxRoomSize);

        // connect each room with corridors
        // Connect(map, root, corridorWidth?)

        return map;

    }

    // initialize the map and root partition
    private static void Init(int w, int h, int pad, out MapData map, out BSPNode root)
    {

        map = new MapData(w, h);

        // create a room with all tiles set to wall
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                map.tiles[x, y] = TileType.Wall;
            }
        }

        // root partition
        root = new BSPNode(new RectInt(
            pad,
            pad,
            w - pad * 2,
            h - pad * 2
        ));
    }

    // split the root BSP node into child nodes recursively
    private static void Split(BSPNode node, int depth, int maxDepth, int minLeafSize)
    {
        // stop splitting if maxDepth reached or current partitions is smaller than minLeafSize
        if (depth >= maxDepth) return;

        if (node.bounds.width < minLeafSize * 2 &&
            node.bounds.height < minLeafSize * 2)
            return;

        // randomly decide if to split horizontally or vertically
        bool splitHorizontally =
            node.bounds.height > node.bounds.width ||
            (node.bounds.height == node.bounds.width && Random.value > 0.5f);

        if (splitHorizontally)
        {
            // choose random y so that top and bottom partitions are at least minLeafSize
            int splitY = Random.Range(
                node.bounds.yMin + minLeafSize,
                node.bounds.yMax - minLeafSize
            );

            node.left = new BSPNode(new RectInt(
                node.bounds.xMin,
                node.bounds.yMin,
                node.bounds.width,
                splitY - node.bounds.yMin
            ));

            node.right = new BSPNode(new RectInt(
                node.bounds.xMin,
                splitY,
                node.bounds.width,
                node.bounds.yMax - splitY
            ));
        }
        else
        {
            // choose random x so that left and right partitions are at least minLeafSize
            int splitX = Random.Range(
                node.bounds.xMin + minLeafSize,
                node.bounds.xMax - minLeafSize
            );

            node.left = new BSPNode(new RectInt(
                node.bounds.xMin,
                node.bounds.yMin,
                splitX - node.bounds.xMin,
                node.bounds.height
            ));

            node.right = new BSPNode(new RectInt(
                splitX,
                node.bounds.yMin,
                node.bounds.xMax - splitX,
                node.bounds.height
            ));
        }

        // split the child partitions
        Split(node.left, depth + 1, maxDepth, minLeafSize);
        Split(node.right, depth + 1, maxDepth, minLeafSize);
    }

    // carve a room into each leaf node
    private static void Carve(MapData map, BSPNode root, int minRoomSize, int maxRoomSize)
    {
        var leaves = new List<BSPNode>();
        CollectLeaves(root, leaves);

        foreach (var leaf in leaves)
        {
            leaf.room = CreateRoomInLeaf(
                 leaf.bounds,
                 minRoomSize,
                 maxRoomSize
                 );

            CarveRect(map, leaf.room);
        }

    }

    // collect the leaf nodes that are used for room creation
    private static void CollectLeaves(BSPNode node, List<BSPNode> leaves)
    {
        if (node == null) return;
        if (node.IsLeaf) { leaves.Add(node); return; }
        CollectLeaves(node.left, leaves);
        CollectLeaves(node.right, leaves);
    }

    // create a randomly-sized room within a leaf
    private static RectInt CreateRoomInLeaf(RectInt leaf, int minRoomSize, int maxRoomSize)
    {
        // if the leaf is too small, no room
        if (leaf.width < minRoomSize || leaf.height < minRoomSize)
            return new RectInt(0, 0, 0, 0);

        // choose random width and height for the room
        int roomW = Random.Range(
            minRoomSize,
            Mathf.Min(maxRoomSize, leaf.width) + 1
        );

        int roomH = Random.Range(
            minRoomSize,
            Mathf.Min(maxRoomSize, leaf.height) + 1
        );

        // choose random x and y for the room
        int roomX = Random.Range(
            leaf.xMin,
            leaf.xMax - roomW + 1
        );

        int roomY = Random.Range(
            leaf.yMin,
            leaf.yMax - roomH + 1
        );

        return new RectInt(roomX, roomY, roomW, roomH);
    }

    // carve the CreateRoomInLeaf rectange into the map
    private static void CarveRect(MapData map, RectInt r)
    {
        if (r.width <= 0 || r.height <= 0) return;

        // set the tiles inside the rectange to Floor tiles
        for (int x = r.xMin; x < r.xMax; x++)
        {
            for (int y = r.yMin; y < r.yMax; y++)
            {
                if (map.InBounds(x, y))
                    map.tiles[x, y] = TileType.Floor;
            }
        }
    }

}

