using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridSpawner : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private HexCell _cellPrefab;
}
