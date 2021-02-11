using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using multilevel_library;

//навигационная сетка для врага
[System.Serializable]
public class NavigationMesh
{
    private static Queue<Vector3> positions = new Queue<Vector3>(); //очередь посещаемых позиций
    private static Queue<Vector3> backDirs = new Queue<Vector3>(); //очередь задних направлений
    public DirectionSet[,,] dirSets; //массив наборов направлений в ячейках
    private int width;
    private int height;
    private int count;

    public NavigationMesh(MultilevelMaze maze)
    {
        width = maze.width;
        height = maze.height;
        count = maze.count;
        dirSets = new DirectionSet[width, height, count];

        //построить навигационную сетку
        for (int i = 0; i < count; i++)
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Place position = new Place(x * 2 + 1, y * 2 + 1);
                    dirSets[x, y, i] = new DirectionSet(); //присвоение направлений на данную ячейку
                    bool onStairsAngle = Utils.isStairs(maze.floors[i].getItem(position)); //мы на ведущем угле ступенек
                    if (maze.floors[i].canEnemyGo(position, Direction.up, !onStairsAngle)) dirSets[x, y, i].dirs.Add(Vector3.forward);
                    if (maze.floors[i].canEnemyGo(position, Direction.right, !onStairsAngle)) dirSets[x, y, i].dirs.Add(Vector3.right);
                    if (maze.floors[i].canEnemyGo(position, Direction.down, !onStairsAngle)) dirSets[x, y, i].dirs.Add(Vector3.back);
                    if (maze.floors[i].canEnemyGo(position, Direction.left, !onStairsAngle)) dirSets[x, y, i].dirs.Add(Vector3.left);
                    if (onStairsAngle) dirSets[x, y, i].dirs.Add(Vector3.up);
                    if (i > 0 && Utils.isStairs(maze.floors[i - 1].getItem(position))) dirSets[x, y, i].dirs.Add(Vector3.down);
                    dirSets[x, y, i].heightAdjustment = onStairsAngle ? 1.25f : 0; //определить прижатие к потолку на лестницах
                }
    }

    //получить набор направлений по 3D позиции
    public DirectionSet getDirectionSet(Vector3 position)
    {
        return dirSets[Mathf.FloorToInt(position.x), Mathf.FloorToInt(-position.z), Mathf.FloorToInt(position.y)];
    }

    //запустить установку приоритетных направлений
    public void startCallPriority(Vector3 startPosition, int amount)
    {
        positions.Clear();
        backDirs.Clear();
        positions.Enqueue(startPosition);
        backDirs.Enqueue(Vector3.zero);
        setCallPriority(positions, backDirs, amount);
    }

    //установить приоритетные направления
    private void setCallPriority(Queue<Vector3> positions, Queue<Vector3> backDirs, int amount)
    {
        //пока очередь не закончилась
        while (positions.Count > 0 && amount != 0)
        {
            //вынуть из очеред позицию и её ROOT-направление
            Vector3 position = positions.Dequeue();
            Vector3 backDir = backDirs.Dequeue();

            //получить текущий набор направлений
            DirectionSet current = getDirectionSet(position);

            //если требуемое направление не задано
            if (!current.isCallSpecified())
            {
                //для каждого направления
                for (int i = 0; i < current.dirs.Count; i++)
                {
                    if (current.dirs[i] == -backDir) //если направление ведёт назад или является единственным
                        current.setCallDirection(i); //пометить его как направление следования

                    //добавить в очередь данные для обработки
                    positions.Enqueue(position + current.dirs[i]);
                    backDirs.Enqueue(current.dirs[i]);
                }

                if (!current.isCallSpecified()) //если направление так и не удалось задать
                    current.callDone = true; //пометить финишную точку
            }

            amount--;
        }
    }

    //сбросить приоритетные направления
    public void resetPriority()
    {
        for (int i = 0; i < count; i++)
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    dirSets[x, y, i].setCallDirection(-1);
                    dirSets[x, y, i].callDone = false;
                }
    }
}
