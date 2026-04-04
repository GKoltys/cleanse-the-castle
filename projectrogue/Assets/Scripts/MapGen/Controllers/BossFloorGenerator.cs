using System.Collections.Generic;
using UnityEngine;

public static class BossFloorGenerator
{
    // generate a width x height room to use as a boss arena
    public static MapData GenerateBossRoom(int w, int h, int pad)
    {
        var map = new MapData(w, h);

        int xMin = pad;
        int yMin = pad;
        int xMax = w - pad - 1;
        int yMax = h - pad - 1;

        // set w x h to floor tile type
        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                map.tiles[x, y] = TileType.Floor;
            }
        }

        // set tiles adjacent to wall tile type
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (map.tiles[x, y] != TileType.Floor) continue;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;

                        if (!map.InBounds(nx, ny)) continue;

                        if (map.tiles[nx, ny] == TileType.Empty)
                        {
                            map.tiles[nx, ny] = TileType.Wall;
                        }
                    }
                }
            }
        }

        // ensure horizontal and vertical edges are wall tile type
        for (int x = 0; x < w; x++)
        {
            if (map.tiles[x, 0] == TileType.Empty) map.tiles[x, 0] = TileType.Wall;
            if (map.tiles[x, h - 1] == TileType.Empty) map.tiles[x, h - 1] = TileType.Wall;
        }

        for (int y = 0; y < h; y++)
        {
            if (map.tiles[0, y] == TileType.Empty) map.tiles[0, y] = TileType.Wall;
            if (map.tiles[w - 1, y] == TileType.Empty) map.tiles[w - 1, y] = TileType.Wall;
        }

        return map;
    }

    // set boss spawn to center of floor
    public static Vector2Int GetBossSpawnTile(MapData map)
    {
        return new Vector2Int(map.width / 2, map.height / 2);
    }

    // set player spawn to below center
    public static Vector2Int GetPlayerSpawnTile(MapData map, int pad)
    {
        return new Vector2Int(map.width / 2, pad + 3);
    }

    // pick tile near player and max distance for stair spawn
    public static Vector2Int PickTileNear(
        List<Vector2Int> floors,
        Vector2 centerPos,
        float maxDist,
        int attempts = 30)
    {
        if (floors == null || floors.Count == 0)
            return Vector2Int.zero;

        // default if no position found
        Vector2Int chosen = floors[Random.Range(0, floors.Count)];

        // try attempts within allowed distance
        for (int i = 0; i < attempts; i++)
        {
            Vector2Int candidate = floors[Random.Range(0, floors.Count)];

            float d = Vector2.Distance(
                new Vector2(candidate.x + 0.5f, candidate.y + 0.5f),
                centerPos
            );

            if (d <= maxDist)
                return candidate;
        }

        return chosen;
    }

    // determine which boss spawns given floor count based on every 10 floors e.g floor 10 -> boss[0], floor 20 -> boss[1]
    public static GameObject GetBossForFloor(GameObject[] bossPrefabs, int floorNumber, int bossEveryXFloors)
    {
        if (bossPrefabs == null || bossPrefabs.Length == 0)
            return null;

        if (bossEveryXFloors <= 0)
            return null;

        if (floorNumber % bossEveryXFloors != 0)
            return null;

        int bossIndex = (floorNumber / bossEveryXFloors) - 1;

        if (bossIndex < 0 || bossIndex >= bossPrefabs.Length)
            return null;

        return bossPrefabs[bossIndex];
    }
}