namespace MapGenerator
{
    public class Generator
    {
        public int[,] GenerateMap(int minSize, int maxSize, int startX, int startY, int endX, int endY, 
                                  int monsterCount, int merchantCount, int minDistance, int maxDistance)
        {
            var rand = new Random();
            var mapSize = rand.Next(minSize, maxSize + 1);  // 맵 크기 결정
            var map = new int[mapSize, mapSize];

            // 맵을 기본적으로 빈 공간(0)으로 초기화
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = 0;
                }
            }

            // 시작점(1000)과 종료점(2000) 설정
            map[startX, startY] = 1000;
            map[endX, endY] = 2000;

            // 경로 연결 및 맵 경로 생성 (시작점과 끝점은 반드시 이어지게 하기)
            CreatePath(map, startX, startY, endX, endY);

            // 몬스터 배치(100~199) 및 상인 배치(200~299) 
            PlaceEventsAlongPath(map, rand, monsterCount, 100, 199, startX, startY, endX, endY);
            PlaceEventsAlongPath(map, rand, merchantCount, 200, 299, startX, startY, endX, endY);

            return map;
        }

        // 경로를 따라 이벤트 배치 (경로상에만)
        private void PlaceEventsAlongPath(int[,] map, Random rand, int count, int minValue, int maxValue,
                                          int startX, int startY, int endX, int endY)
        {
            var path = GetPath(map, startX, startY, endX, endY);

            for (var i = 0; i < count; i++)
            {
                if (path.Count == 0)
                    break;

                var (x, y) = path[rand.Next(path.Count)];
                if (map[x, y] == 0)  // 빈 공간에만 이벤트 배치
                {
                    map[x, y] = rand.Next(minValue, maxValue + 1);
                }
            }
        }

        // 경로를 따라 (시작점에서 종료점까지) 경로를 추적하는 메서드
        private List<(int, int)> GetPath(int[,] map, int startX, int startY, int endX, int endY)
        {
            var path = new List<(int, int)>();
            var mapSize = map.GetLength(0);
            var rand = new Random();
            var currentX = startX;
            var currentY = startY;

            while (currentX != endX || currentY != endY)
            {
                path.Add((currentX, currentY));
                
                if(map[currentX, currentY] == 0)
                   map[currentX, currentY] = rand.Next(1, 100);  // 경로 값 설정

                // 상하좌우 4방향 중 랜덤으로 하나를 선택
                var direction = rand.Next(4);
                switch (direction)
                {
                    case 0: // 위
                        if (currentX > 0) currentX--;
                        break;
                    case 1: // 아래
                        if (currentX < mapSize - 1) currentX++;
                        break;
                    case 2: // 왼쪽
                        if (currentY > 0) currentY--;
                        break;
                    case 3: // 오른쪽
                        if (currentY < mapSize - 1) currentY++;
                        break;
                }
            }

            // 종료점도 경로에 포함
            path.Add((endX, endY));
            return path;
        }

        // 이벤트 배치 메서드 (경로를 따라 이벤트 배치)
        private void PlaceEvents(int[,] map, Random rand, int count, int minValue, int maxValue)
        {
            var mapSize = map.GetLength(0);
            for (var i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(mapSize);
                    y = rand.Next(mapSize);
                } while (map[x, y] != 0); // 이미 자리가 차있는 곳은 제외

                map[x, y] = rand.Next(minValue, maxValue + 1);
            }
        }

        // 경로 연결 메서드 (시작점과 종료점을 잇는 경로를 생성)
        private void CreatePath(int[,] map, int startX, int startY, int endX, int endY)
        {
            var mapSize = map.GetLength(0);
            var rand = new Random();
            var currentX = startX;
            var currentY = startY;

            // 현재 위치에서 종료점까지 길을 생성
            while (currentX != endX || currentY != endY)
            {
                if (map[currentX, currentY] == 0)
                    map[currentX, currentY] = rand.Next(1, 100); // 경로 값 설정

                // 상하좌우 4방향 중 랜덤으로 하나를 선택
                var direction = rand.Next(4);
                switch (direction)
                {
                    case 0: // 위
                        if (currentX > 0) currentX--;
                        break;
                    case 1: // 아래
                        if (currentX < mapSize - 1) currentX++;
                        break;
                    case 2: // 왼쪽
                        if (currentY > 0) currentY--;
                        break;
                    case 3: // 오른쪽
                        if (currentY < mapSize - 1) currentY++;
                        break;
                }
            }
        }

        public void PrintMap(int[,] map)
        {
            var mapSize = map.GetLength(0);
            for (var i = 0; i < mapSize; i++)
            {
                for (var j = 0; j < mapSize; j++)
                {
                    Console.Write(map[i, j].ToString().PadLeft(5));
                }
                Console.WriteLine();
            }
        }
    }
}
