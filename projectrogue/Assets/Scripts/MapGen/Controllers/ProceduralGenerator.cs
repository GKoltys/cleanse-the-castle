using System.Collections.Generic;
using UnityEngine;

// procedural generation with BSP algorithm
// https://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation
public static class ProceduralGenerator
{
    public static MapData GenerateFloor(int w, int h, int pad,
        int bspMaxDepth, int minLeafSize, int minRoomSize, int maxRoomSize, int corridorWidth, out Vector2Int playerPos)
    {
        // create a room with tiles set to wall and the root partition
        Init(w, h, pad, out MapData map, out BSPNode root);

        // split recursively until each sub-dungeon is approx size of a room
        Split(root, 0, bspMaxDepth, minLeafSize);

        // create random-size room in each sub-dungeon
        Carve(map, root, minRoomSize, maxRoomSize);

        // connect each room with corridors
        Connect(map, root, corridorWidth);

        // choose player position in map
        playerPos = PickPlayerStart(map, root);

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

    // connect nodes
    private static void Connect(MapData map, BSPNode root, int corridorWidth)
    {
        ConnectNode(map, root, corridorWidth);
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
                {
                    map.tiles[x, y] = TileType.Floor;
                }
            }
        }
    }

    // recursively connects nodes with corridors
    private static void ConnectNode(MapData map, BSPNode node, int corridorWidth)
    {
        if (node == null) return;

        // for a leaf node, choose a point and set it as the connector if it contains a room
        if (node.IsLeaf)
        {
            if (node.room.width > 0 && node.room.height > 0)
            {
                node.connector = PickPointInRoom(node.room);
                node.hasConnector = true;
            }
            else
            {
                node.hasConnector = false;
            }
            return;
        }

        // recurse so each child establishes their connector
        ConnectNode(map, node.left, corridorWidth);
        ConnectNode(map, node.right, corridorWidth);

        // if either child is missing or does not have a connector, they cannot be connected
        if (node.left == null || node.right == null)
        {
            node.hasConnector = false;
            return;
        }
        if (!node.left.hasConnector || !node.right.hasConnector)
        {
            node.hasConnector = false;
            return;
        }

        Vector2Int a = node.left.connector;
        Vector2Int b = node.right.connector;

        // create a corridor between the two subtrees
        CreateCorridor(map, a, b, corridorWidth);

        // set connector for current subtree
        node.connector = (Random.value < 0.5f) ? a : b;
        node.hasConnector = true;
    }

    // pick a random point for the corridor connection point
    private static Vector2Int PickPointInRoom(RectInt room)
    {
        int x = Random.Range(room.xMin, room.xMax);
        int y = Random.Range(room.yMin, room.yMax);
        return new Vector2Int(x, y);
    }

    // creates a corridor between two points a and b
    private static void CreateCorridor(MapData map, Vector2Int a, Vector2Int b, int corridorWidth)
    {
        // if both points share x or y value, then create a vertical/horizontal corridor
        if (a.x == b.x)
        {
            CarveVertical(map, a.x, a.y, b.y, corridorWidth);
            return;
        }

        if (a.y == b.y)
        {
            CarveHorizontal(map, a.y, a.x, b.x, corridorWidth);
            return;
        }

        // if points don't share value, must create a bend in the corridor
        // decide if to make bend around an x or y
        bool useMidX = Random.value < 0.5f;

        if (useMidX)
        {
            // choose an x coordinate between the two points a and b
            int midX = Random.Range(Mathf.Min(a.x, b.x), Mathf.Max(a.x, b.x) + 1);

            // carve horizontally from point a to midX 
            CarveHorizontal(map, a.y, a.x, midX, corridorWidth);
            // then carve vertically along the midX until reaching b's y 
            CarveVertical(map, midX, a.y, b.y, corridorWidth);
            // carve horizontally until reaching point b
            CarveHorizontal(map, b.y, midX, b.x, corridorWidth);
        }
        else
        {
            int midY = Random.Range(Mathf.Min(a.y, b.y), Mathf.Max(a.y, b.y) + 1);

            // carve vertically from point a to midY
            CarveVertical(map, a.x, a.y, midY, corridorWidth);
            // carve horizontally along the midY until reaching b's x
            CarveHorizontal(map, midY, a.x, b.x, corridorWidth);
            // carve vertically until reaching point b
            CarveVertical(map, b.x, midY, b.y, corridorWidth);
        }
    }

    // carve a horizontal corridor between two x points along y
    private static void CarveHorizontal(MapData map, int y, int x0, int x1, int corridorWidth)
    {
        int min = Mathf.Min(x0, x1);
        int max = Mathf.Max(x0, x1);
        for (int x = min; x <= max; x++)
        {
            CarveFloor(map, x, y, corridorWidth);
        }
    }

    // carve a vertical corridor between two y points along x
    private static void CarveVertical(MapData map, int x, int y0, int y1, int corridorWidth)
    {
        int min = Mathf.Min(y0, y1);
        int max = Mathf.Max(y0, y1);
        for (int y = min; y <= max; y++)
        {
            CarveFloor(map, x, y, corridorWidth);
        }
    }

    // set tiles to floor with a specified width
    private static void CarveFloor(MapData map, int x, int y, int corridorWidth)
    {
        // calculate the corridor area
        int half = Mathf.Max(0, corridorWidth / 2);

        for (int dx = -half; dx <= half; dx++)
        {
            for (int dy = -half; dy <= half; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (map.InBounds(nx, ny))
                {
                    map.tiles[nx, ny] = TileType.Floor;
                }

            }

        }

    }

    // choose the starting position for the player
    private static Vector2Int PickPlayerStart(MapData map, BSPNode root)
    {
        // collect a list of leaves then rooms
        var leaves = new List<BSPNode>();
        CollectLeaves(root, leaves);

        var rooms = new List<RectInt>();
        foreach (var leaf in leaves)
        {
            if (leaf.room.width > 0 && leaf.room.height > 0)
            {
                rooms.Add(leaf.room);
            }
  
        }

        // pick a position within a room
        if (rooms.Count > 0)
        {
            RectInt room = rooms[Random.Range(0, rooms.Count)];
            int x = Random.Range(room.xMin, room.xMax);
            int y = Random.Range(room.yMin, room.yMax);
            return new Vector2Int(x, y);
        }

        // fallback with any floor tile
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                if (map.tiles[x, y] == TileType.Floor)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(0, 0);
    }

}

