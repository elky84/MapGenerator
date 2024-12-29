using MapGenerator;

const int rows = 10;
const int cols = 10;
const int minPathLength = 5; // 최소 경로 길이
const int maxPathLength = 15; // 최대 경로 길이
const int minBranches = 2; // 최소 갈림길 수
const int maxBranches = 5; // 최대 갈림길 수

var map = Generator.GenerateMap(rows, cols, minPathLength, maxPathLength, minBranches, maxBranches);

// 맵 출력
for (var r = 0; r < rows; r++)
{
    for (var c = 0; c < cols; c++)
    {
        Console.Write(map[r, c].ToString("D3") + " ");
    }
    Console.WriteLine();
}
