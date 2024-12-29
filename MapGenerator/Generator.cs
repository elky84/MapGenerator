namespace MapGenerator;

public static class Generator
{
    private static readonly Random Random = new Random();

    public static int[,] GenerateMap(
        int rows,
        int cols,
        int minPathLength,
        int maxPathLength,
        int minBranches,
        int maxBranches)
    {
        // 맵 초기화
        var map = new int[rows, cols];
        for (var r = 0; r < rows; r++)
            for (var c = 0; c < cols; c++)
                map[r, c] = 0;

        // 시작점과 끝점 설정
        var (startRow, startCol) = (Random.Next(rows), Random.Next(cols));
        var (endRow, endCol) = (Random.Next(rows), Random.Next(cols));

        // 끝점이 올바른 최소/최대 경로 길이를 가지도록 설정
        while (Math.Abs(startRow - endRow) + Math.Abs(startCol - endCol) < minPathLength ||
               Math.Abs(startRow - endRow) + Math.Abs(startCol - endCol) > maxPathLength)
        {
            endRow = Random.Next(rows);
            endCol = Random.Next(cols);
        }

        map[startRow, startCol] = 1; // 입구
        map[endRow, endCol] = 2;    // 출구

        // 주 경로 생성
        CreateMainPath(map, startRow, startCol, endRow, endCol);

        // 랜덤 갈래길 추가
        AddBranches(map, minBranches, maxBranches);

        // 지형 및 오브젝트 배치
        PlaceRandomTerrain(map, 10, 19);
        PlaceRandomObjects(map, 100, 199);

        return map;
    }

    private static void CreateMainPath(int[,] map, int startRow, int startCol, int endRow, int endCol)
    {
        var directions = new (int dr, int dc)[]
        {
            (-1, 0), (1, 0), (0, -1), (0, 1)
        };

        var visited = new bool[map.GetLength(0), map.GetLength(1)];
        var queue = new Queue<(int row, int col, List<(int row, int col)> path)>();
        queue.Enqueue((startRow, startCol, new List<(int, int)> { (startRow, startCol) }));

        visited[startRow, startCol] = true;

        while (queue.Count > 0)
        {
            var (currentRow, currentCol, path) = queue.Dequeue();

            if (currentRow == endRow && currentCol == endCol)
            {
                // 경로를 실제로 맵에 반영
                foreach (var (r, c) in path)
                {
                    if (map[r, c] != 1 && map[r, c] != 2) // 입구와 출구는 유지
                        map[r, c] = 3; // 경로
                }
                return;
            }

            Shuffle(directions);

            foreach (var (dr, dc) in directions)
            {
                var newRow = currentRow + dr;
                var newCol = currentCol + dc;

                if (!IsInBounds(map, newRow, newCol) || visited[newRow, newCol])
                    continue;
                
                visited[newRow, newCol] = true;
                var newPath = new List<(int, int)>(path) { (newRow, newCol) };
                queue.Enqueue((newRow, newCol, newPath));
            }
        }
    }

    private static void AddBranches(int[,] map, int minBranches, int maxBranches)
    {
        var directions = new (int dr, int dc)[]
        {
            (-1, 0), (1, 0), (0, -1), (0, 1)
        };

        // 갈림길 개수 결정
        var branches = Random.Next(minBranches, maxBranches + 1);

        for (var i = 0; i < branches; i++)
        {
            // 경로에서 랜덤 위치 찾기
            var pathCells = new List<(int row, int col)>();
            for (var r = 0; r < map.GetLength(0); r++)
                for (var c = 0; c < map.GetLength(1); c++)
                    if (map[r, c] == 3) pathCells.Add((r, c));

            if (pathCells.Count == 0) break;

            var (branchRow, branchCol) = pathCells[Random.Next(pathCells.Count)];

            // 무작위 방향 섞기
            Shuffle(directions);

            foreach (var (dr, dc) in directions)
            {
                var newRow = branchRow + dr;
                var newCol = branchCol + dc;

                if (!IsInBounds(map, newRow, newCol) || map[newRow, newCol] != 0)
                    continue;
                
                map[newRow, newCol] = 3; // 갈래길 생성
                break;
            }
        }
    }

    private static void PlaceRandomTerrain(int[,] map, int minTerrain, int maxTerrain)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);
        var numTerrains = Random.Next(rows * cols / 10, rows * cols / 5);

        for (var i = 0; i < numTerrains; i++)
        {
            var row = Random.Next(rows);
            var col = Random.Next(cols);

            if (map[row, col] == 0)
            {
                map[row, col] = Random.Next(minTerrain, maxTerrain + 1);
            }
        }
    }

    private static void PlaceRandomObjects(int[,] map, int minObject, int maxObject)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);
        var numObjects = Random.Next(rows * cols / 15, rows * cols / 10);

        for (var i = 0; i < numObjects; i++)
        {
            var row = Random.Next(rows);
            var col = Random.Next(cols);

            if (map[row, col] == 0)
            {
                map[row, col] = Random.Next(minObject, maxObject + 1);
            }
        }
    }

    private static bool IsInBounds(int[,] map, int row, int col)
    {
        return row >= 0 && row < map.GetLength(0) && col >= 0 && col < map.GetLength(1);
    }

    private static void Shuffle<T>(T[] array)
    {
        for (var i = array.Length - 1; i > 0; i--)
        {
            var j = Random.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
