using MapGenerator;

const int minSize = 10;
const int maxSize = 20;
const int startX = 0;
const int startY = 0;
const int endX = 9;
const int endY = 9;
const int monsterCount = 5;
const int merchantCount = 3;
const int minDistance = 5;
const int maxDistance = 10;

var generator = new Generator();
var map = generator.GenerateMap(minSize, maxSize, startX, startY, endX, endY, 
    monsterCount, merchantCount, minDistance, maxDistance);

generator.PrintMap(map);
