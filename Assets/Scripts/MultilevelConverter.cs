using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using multilevel_library;
using UnityEditor;

[CustomEditor(typeof(MultilevelConverter))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var ie = (MultilevelConverter)target;
        if (GUILayout.Button("(re)Build Construction"))
        {
            ie.open();
            ie.build();
        }
        if (GUILayout.Button("Delete Construction"))
            ie.clear();
    }
}

public class MultilevelConverter : MonoBehaviour
{
    public MultilevelMaze maze; //многоэтажный лабиринт
    public Material floorMaterial; //материал пола
    public Material wallMaterial; //материал стен
    public GameObject stairs; //ступеньки
    public string fileName; //имя файла
    public Vector3 gridScale; //размер сетки
    public float blockDepth; //толщина блока
    private Vector3 passageSize; //ширина прохода

    public static Vector3 elementwiseMultiply(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    //открыть лабиринт из файла
    public void open()
    {
        using (FileStream fs = new FileStream(fileName, FileMode.Open))
        using (BinaryReader sr = new BinaryReader(fs))
            maze = new MultilevelMaze(sr);
    }

    //построить помещение
    public void build()
    {
        clear();
        passageSize = gridScale * 2 - Vector3.one * blockDepth;

        //построить этажи с перекрытиями
        for (int i = 0; i < maze.count * 2; i++)
        {
            Floor current = i % 2 == 0 ? maze.overlaps[i / 2] : maze.floors[i / 2];

            //предметы на этаже
            for (int y = 0; y <= maze.height * 2; y++)
                for (int x = 0; x <= maze.width * 2; x++)
                {
                    Place position = new Place(x, y);
                    int item = current.getItem(position);
                    if (item == 1)
                        //поставить блок
                        brick(position, current, i);
                    else if (item >= 20 && item <= 23)
                    {
                        //поставить лестницу
                        Direction dir = Direction.fromNumber(item - 20);
                        Place center = position.shiftNew(dir); //центр размещения ступенек
                        GameObject st = Instantiate(stairs, transform);
                        st.transform.localPosition = elementwiseMultiply(new Vector3(center.x, i, -center.y), gridScale);
                        st.transform.rotation = Quaternion.Euler(0, dir.opposite().getNumber() * 90, 0);
                        st.transform.localScale = new Vector3((gridScale.x * 4 - blockDepth) / 11, gridScale.y * 2 / 6, (gridScale.z * 4 - blockDepth) / 11);
                    }
                }
        }
    }

    //поставить стену
    private void setBlock(Vector3 position, Vector3 length)
    {
        Vector3 odd = new Vector3(
            position.x % 2 == 0 ? blockDepth : passageSize.x,
            position.y % 2 == 0 ? blockDepth : passageSize.y,
            position.z % 2 == 0 ? blockDepth : passageSize.z);
        Vector3 scale = new Vector3(
            (length.x - length.x % 2) * gridScale.x + (length.x % 2 == 1 ? odd.x : 0),
            (length.y - length.y % 2) * gridScale.y + (length.y % 2 == 1 ? odd.y : 0),
            (length.z - length.z % 2) * gridScale.z + (length.z % 2 == 1 ? odd.z : 0));
        Vector3 middle = new Vector3(
            position.x * gridScale.x + (scale.x - odd.x) / 2,
            position.y * gridScale.y + (scale.y - odd.y) / 2,
            position.z * gridScale.z - (scale.z - odd.z) / 2);
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.parent = transform;
        wall.transform.localPosition = middle;
        wall.transform.localScale = scale;
        wall.GetComponent<Renderer>().material = position.y % 2 == 0 ? floorMaterial : wallMaterial;
        wall.AddComponent<UVConverter>();
    }

    //поставить блок
    private void brick(Place position, Floor floor, int level)
    {
        //определение конечных границ блока (должны быть нечётными)
        int lengthY = sideLength(position, floor);
        int lengthX = sideSquare(position, floor, lengthY);

        //записать блок на сцену
        setBlock(new Vector3(position.x, level, -position.y), new Vector3(lengthX, 1, lengthY));

        //обозначить блок на карте (чтобы он снова не поставился на это же место)
        for (int y = 0; y < lengthY; y++)
            for (int x = 0; x < lengthX; x++)
                floor.setItem(position.x + x, position.y + y, 0);
    }

    //эти 2 функции используются для расстановки блоков
    private int sideLength(Place position, Floor floor)
    {
        int length = 0;
        while (position.y <= maze.height * 2 && floor.getItem(position) == 1)
        {
            position.shift(Direction.down);
            length++;
        }
        return length;
    }

    private int sideSquare(Place position, Floor floor, int lengthY)
    {
        int length = 0;
        while (position.x <= maze.width * 2 && sideLength(position, floor) >= lengthY)
        {
            position.shift(Direction.right);
            length++;
        }
        return length;
    }

    //очистить помещение
    public void clear()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }
}
