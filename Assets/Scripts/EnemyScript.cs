using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private const int gridSize = 6;
    private Rigidbody rb;

    private Vector3 offset; //относительное смещение помещения относительно центра центральной системы координат
    private NavigationMesh nav; //навигационная сетка
    [SerializeField] private MultilevelConverter mc; //конвертер помещений
    [SerializeField] private GameObject player; //кого преследовать
    [SerializeField] private float speed; //скорость
    [SerializeField] private int maxLag; //максимальное допустимое расстояние погони за игроком

    private bool follow; //преследование активно
    private bool followLast; //последнее значение преследования
    private bool isCalled; //движение на вызов?
    private Vector3 target; //целевая позиция
    private Vector3 direction; //последнее выданное направление
    [SerializeField] private Vector3 targetOffset; //сдвиг цели по-вертикали
    [HideInInspector] public bool canFollow; //преследование возможно
    [SerializeField] private bool canKill; //убийство возможно

    // Start is called before the first frame update
    void Start()
    {
        offset = mc.gameObject.transform.position; //запросить глобальное смещение
        mc.open();
        nav = new NavigationMesh(mc.maze); //запросить навигационную сетку
        canFollow = true; //взвести врага
        isCalled = false; //движение не по вызову
        rb = GetComponent<Rigidbody>();
        target = transform.position; //пока цели нет
        Invoke("ChangeDirection", 0.5f); //ждём, когда установится nav
    }

    // Update is called once per frame
    void Update()
    {
        //получить видимость игрока врагом
        follow = canFollow && !Physics.Linecast(transform.position, player.transform.position + targetOffset);

        //определить погоню врага
        if (follow)
        {
            target = player.transform.position + targetOffset;
            setVelocity();
            CancelInvoke();
        }

        //выправить позицию, если игрок потерялся
        if (!follow && followLast)
        {
            if (player != null) call((player.transform.position - offset) / gridSize);
            Invoke("ChangeDirection", 0.5f); //восстановить нормальный таймер
        }

        //перезаписать последнее значение видимости
        followLast = follow;
    }

    //вызвать на позицию с установленным отставанием
    public void call(Vector3 pos)
    {
        //установить приоритетные направления
        nav.resetPriority();
        nav.startCallPriority(pos, maxLag);

        //включить вызов
        isCalled = true;
    }

    //обновить физические скорости
    private void setVelocity()
    {
        rb.velocity = (target - transform.position).normalized * speed; //установить линейную скорость
    }

    //сменить направление движения
    private void ChangeDirection()
    {
        if (!follow) //оборвать таймер перемещения в случае погони
        {
            Vector3 pos = (transform.position - offset) / gridSize; //текущая позиция
            Vector3Int posLogic = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z)); //текущая логическая позиция
            DirectionSet set = nav.getDirectionSet(pos); //набор направлений в текущей ячейке

            //проверить вызов
            if (isCalled)
            {
                if (!set.isCallSpecified() || set.callDone)
                    isCalled = false;
                else
                    direction = set.getCallDirection();
            }

            //вызов выполнен
            if (!isCalled)
            {
                if (set.dirs.Count == 1) //если тупик, то единственное направление
                    direction = set.dirs[0];
                else //иначе запретить инверс последнего направления
                    direction = set.randomOtherSpecified(-direction);
            }

            //задать целевую позицию
            target = (posLogic + direction) * gridSize + Vector3.one * gridSize / 2 + Vector3.up * nav.getDirectionSet(pos + direction).heightAdjustment + offset;
            setVelocity();
            Invoke("ChangeDirection", (target - transform.position).magnitude / speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //если столкновение с игроком
        if (canFollow && canKill && collision.gameObject.CompareTag("Player"))
        {
            canFollow = false; //перестать преследовать
            Debug.Log("Player killed"); //убить попавшегося игрока
        }
    }
}
