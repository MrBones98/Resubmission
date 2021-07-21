using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject _hexPrefab;
    [SerializeField] private List<Transform> _cells = new List<Transform>();
    [SerializeField] private int _horizontalSpan, _verticalSpan;
    [SerializeField] private float _yOffset = 0.759f;
    [SerializeField] private float _xOffset = 0.87f;
    [SerializeField] private float _timer;
    [SerializeField] private bool _randomValueY;
    [SerializeField] private bool _randomValueX;
    [SerializeField] [Range(1,5)] private int _iterations;

    private void Start()
    {
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        //while (_iterations > 0)
        //{
        for (int x = 0; x < _horizontalSpan; x++)
        {
            for (int y = 0; y < _verticalSpan; y++)
            {
                float xPos = x * _xOffset;
                float yPos = y;
                //Checking if we are on an odd row
                if (y % 2 == 1)
                {
                    xPos += _xOffset / 2;
                }

                GameObject hexGameObject = Instantiate(_hexPrefab, new Vector3(xPos, yPos * _yOffset, 0), Quaternion.identity);
                _cells.Add(hexGameObject.transform);

                //Mirroring the grid created on the opposite axis
                if (CoinFlip())
                {
                    hexGameObject = Instantiate(_hexPrefab, new Vector3(-xPos, -yPos * _yOffset, 0), Quaternion.identity);
                }
                hexGameObject.name = "Hex (" + x + "/" + y + ")";

                hexGameObject.transform.SetParent(this.transform);
                //Trying to save the mirrored one
                if (!_cells.Contains(hexGameObject.transform))
                {
                    _cells.Add(hexGameObject.transform);
                }
            }
                
        }
            //_iterations--;
        //}
    }
    private void Update()
    {
        //DestroyCellsRandomlyOverTime();
    }
    private void RandomHorizontalSpan()
    {
        _horizontalSpan = Random.Range(1, 10);
    }
    private void RandomVerticalSpan()
    {
        _verticalSpan = Random.Range(1, 10);
    }
    private void DestroyCellsRandomlyOverTime()
    {
        StartCoroutine(nameof(DestroyDelay));
    }
    public bool CoinFlip()
    {
        return Random.value > 0.5;
    }
    private IEnumerator DestroyDelay()
    {
        new WaitForSeconds(10);
        _timer = 10f;
        int index = Random.Range(0, _cells.Count - 1);
        while (_timer > 0 && _cells.Count > 0)
        {
            _timer-= Time.deltaTime;
            
            yield return null;
        }
        Destroy(_cells[index].gameObject);
        _cells.RemoveAt(index);
    }
}
