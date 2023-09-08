using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridValueType
{
    TakeableBrick = 0,
    UnTakeableBrick,
    PlayerStartPoint,
    WinPos,

}
public class GameManager : MonoBehaviour
{
    static GameManager instance;
    private List<GameObject> brickPrefabs = new List<GameObject>();
    public GameObject myGameObject;
    bool isWin;
    public GameObject GetPrefab(int gridValue)
    {
        int indexInList = gridValue - 1;
        if (indexInList >= 0 && indexInList <= brickPrefabs.Count)
        {
            return brickPrefabs[indexInList];
        }
        return null;
    }
    private void Awake()
    {
        instance = this;
    }

}
