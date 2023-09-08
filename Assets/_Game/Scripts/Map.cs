using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrickType
{
    wallPrefab = 1,
    TakeableBrick = 2,
    UnTakeabkeBrick = 3,
    PlayerStartPoint = 4,
    targetPointPrefab = 5,
}
public class Map : MonoBehaviour
{
    private static Map instance;
    public static Map Instance { get => instance; }
    public List<GameObject> brickPrefabs = new List<GameObject>();
    [SerializeField] private List<TextAsset> listLevel = new List<TextAsset>();
    private int countMap;
    // Start is called before the first frame update

    private void Awake()
    {
        Map.instance = this;
    }
    void Start()
    {
        CreateLevel();
        countMap = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject GetPrefab(int gridValue)
    {
        int indexInList = gridValue;
        if (indexInList >= 0 && indexInList < brickPrefabs.Count)
        {
            return brickPrefabs[indexInList];
        }
        return null;
    }

    private void CreateLevel()
    {
        string[] mapData = ReadLevelText();

        int mapCol = mapData[0].ToCharArray().Length;
        int mapRow = mapData.Length;

        for (int j = 0; j < mapRow; j++)
        {
            char[] values = mapData[j].ToCharArray();

            for (int i = 0; i < mapCol; i++)
            {
                int value = int.Parse(values[i].ToString());
                GameObject gameObject;

                if (GetPrefab(value) != null)
                {
                    gameObject = Instantiate(GetPrefab(value));
                    gameObject.transform.parent = transform;
                    gameObject.transform.position = Vector3ToGrid(j, i);
                    gameObject.name = i.ToString() + "_" + j.ToString();
                }
            }
        }
    }

    private string[] ReadLevelText()
    {
        string curentMap = listLevel[countMap].name.ToString();
        TextAsset binData = Resources.Load(curentMap) as TextAsset;
        string data = binData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    public Vector3 Vector3ToGrid(int row, int col)
    {
        return new Vector3(col, 0, row);
    }

    //Check tọa độ nhân vật đang đứng trên gird
    public Grid Vector3ToGridPos(Vector3 playPos)
    {
        int row = GetRowOrColNumber(playPos.x);
        int col = GetRowOrColNumber(playPos.z);
        return new Grid(row, col);
    }

    public int GetRowOrColNumber(float x)
    {
        return Mathf.FloorToInt(x + 0.5f);
    }
}