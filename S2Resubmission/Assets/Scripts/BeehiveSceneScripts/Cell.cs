using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject[] _walls;
    public void SetWalls(int bitMask)
    {
        for (int i = 0; i < _walls.Length; i++)
        {
            bool wallExist = ((bitMask >> i) & 1 ) == 1;
            _walls[i].SetActive(wallExist);
            //Set Walls in the correct order for the walls prefab
        }
    }
}
