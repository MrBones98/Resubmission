using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _enemyAmount;
    [SerializeField] private GameObject _enemyPrefab;

    private MapSpawner _mapSpawner;
    private Vector2 _enemyPosition;
    private int _enemySpawnIndex;
    private void Start()
    {
        _mapSpawner = GetComponent<MapSpawner>();
        InstantiateEnemy();
    }
    private void InstantiateEnemy()
    {
        for (int i = 0; i < _enemyAmount; i++)
        {
            _enemySpawnIndex = Random.Range(0, _mapSpawner.ListOfRooms.Count - 1);
            _enemyPosition = _mapSpawner.ListOfRooms[_enemySpawnIndex].transform.position;

            Instantiate(_enemyPrefab, _enemyPosition, Quaternion.identity);
        }
    }
}
