using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction 
{
    North=0,
    NorthEast= 1,
    SouthEast=2,
    South=3,
    SouthWest=4,
    NorthWest=5,
}
public enum DoorExist
{
    Uncertain,
    Yes,
    No,
}
public static class DoorExistExtension
{
    public static bool asBool(this DoorExist doorExists)
    {
        switch (doorExists)
        {
            case DoorExist.Uncertain:
                throw new System.Exception("DoorExists.Uncertain cannot be cast as a bool");
            case DoorExist.Yes:
                return true;
            case DoorExist.No:
                return false;
            default:
                throw new System.NotImplementedException();
        }
    }
}
public class MapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject[] _fourDirectionsPrefabs;
    [SerializeField] private float _verticalOffset = 0.87f;
    [SerializeField] private float _diagonalHorizontalOffset = 0.759f;
    [SerializeField] private float _diagonalVerticalOffset = 0.44f;
    [SerializeField] [Range(10, 50)] private int _iterations;

    public List<GameObject> ListOfRooms = new List<GameObject>();

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        List<Vector2> openCells = new List<Vector2>();
        Dictionary<Vector2, HoneyComb> cellMap = new Dictionary<Vector2, HoneyComb>();


        openCells.Add(new Vector2(0,0));
        while(openCells.Count>0 && cellMap.Count < _iterations)
        {
            Vector2 cell = openCells[0];
            if (cellMap.ContainsKey(cell))
            {
                openCells.Remove(cell);
                continue;
            }
            Vector2 north = RoundVector( cell + new Vector2(0, _prefab.transform.localScale.x * _verticalOffset));
            Vector2 south = RoundVector (cell+ new Vector2(0, _prefab.transform.localScale.x * -_verticalOffset));

            Vector2 northEast = RoundVector(cell + new Vector2(_prefab.transform.localScale.x * _diagonalHorizontalOffset, _prefab.transform.localScale.x * _diagonalVerticalOffset));
            Vector2 northWest = RoundVector(cell + new Vector2(_prefab.transform.localScale.x * -_diagonalHorizontalOffset, _prefab.transform.localScale.x * _diagonalVerticalOffset));
            Vector2 southEast = RoundVector(cell + new Vector2(_prefab.transform.localScale.x * _diagonalHorizontalOffset, _prefab.transform.localScale.x * -_diagonalVerticalOffset));
            Vector2 southWest = RoundVector(cell  + new Vector2(_prefab.transform.localScale.x * -_diagonalHorizontalOffset, _prefab.transform.localScale.x * -_diagonalVerticalOffset));


            var northDoor = DoorExist.Uncertain;
            var southDoor = DoorExist.Uncertain;
            var northEastDoor = DoorExist.Uncertain;
            var northWestDoor = DoorExist.Uncertain;
            var southEastDoor = DoorExist.Uncertain;
            var southWestDoor = DoorExist.Uncertain;
            if (cellMap.ContainsKey(north))
            {
                northDoor = cellMap[north].NeedsHoneyCombFrom(Direction.South);
            }
            if (cellMap.ContainsKey(south))
            {
                southDoor = cellMap[south].NeedsHoneyCombFrom(Direction.North);
            }
            if (cellMap.ContainsKey(northEast))
            {
                northEastDoor = cellMap[northEast].NeedsHoneyCombFrom(Direction.SouthWest);
            }
            if (cellMap.ContainsKey(northWest))
            {
                northWestDoor = cellMap[northWest].NeedsHoneyCombFrom(Direction.SouthEast);
            }
            if (cellMap.ContainsKey(southEast))
            {
                southEastDoor = cellMap[southEast].NeedsHoneyCombFrom(Direction.NorthWest);
            }
            if (cellMap.ContainsKey(southWest))
            {
                southWestDoor = cellMap[southWest].NeedsHoneyCombFrom(Direction.NorthEast);
            }

            if (northDoor == DoorExist.Uncertain)
            {
                if (CoinFlip())
                {
                    northDoor = DoorExist.Yes;
                    openCells.Add(north);
                }
                else
                {
                    northDoor = DoorExist.No;
                }
            }
            if (southDoor == DoorExist.Uncertain)
            {
                if (CoinFlip())
                {
                    southDoor = DoorExist.Yes;
                    openCells.Add(south);
                }
                else
                {
                    southDoor = DoorExist.No;
                }
            }
            if (northEastDoor == DoorExist.Uncertain)
            {
                if (CoinFlip())
                {
                    northEastDoor = DoorExist.Yes;
                    openCells.Add(northEast);
                }
                else
                {
                    northEastDoor = DoorExist.No;
                }
            }
            if (northWestDoor == DoorExist.Uncertain)
            {
                if (CoinFlip())
                {
                    northWestDoor = DoorExist.Yes;
                    openCells.Add(northWest);
                }
                else
                {
                    northWestDoor = DoorExist.No;
                }
            }
            if (southEastDoor == DoorExist.Uncertain)
            {
                if (CoinFlip())
                {
                    southEastDoor = DoorExist.Yes;
                    openCells.Add(southEast);
                }
                else
                {
                    southEastDoor = DoorExist.No;
                }
            }
            if (southWestDoor == DoorExist.Uncertain)
            {
                if (CoinFlip())
                {
                    southWestDoor = DoorExist.Yes;
                    openCells.Add(southWest);                  
                }
                else
                {
                    southWestDoor = DoorExist.No;
                }
            }
            
            HoneyComb spawnedHoneyComb = new HoneyComb(northDoor.asBool(), northEastDoor.asBool(), southEastDoor.asBool(), southDoor.asBool(), southWestDoor.asBool(), northWestDoor.asBool());

            if (cellMap.ContainsKey(cell))
            {
                cellMap.Remove(cell);
            }
            else
            {
                cellMap.Add(cell, spawnedHoneyComb);
            }

            openCells.Remove(cell);
        }
        foreach (var cell in cellMap)
        {

            Vector3 position = new Vector3(cell.Key.x, cell.Key.y, 0);

            GameObject honeyComb = Instantiate(_prefab, position, Quaternion.identity);
            //Change get honeycombinfo name dude, not info, wall bitmask duh
            honeyComb.GetComponent<Cell>().SetWalls(cell.Value.GetHoneyCombInfo());
            honeyComb.transform.SetParent(this.transform);
            honeyComb.name = "Cell (" + cell.Key + ")";
                
            ListOfRooms.Add(honeyComb);
        }
    }
    public Vector2 RoundVector(Vector2 vector)
    {
        return new Vector2((float)Math.Round(vector.x, 2), (float)Math.Round(vector.y, 2));
    }
    public bool CoinFlip()
    {
        return UnityEngine.Random.value > 0.5;
    }
}
[System.Serializable]
public class HoneyComb
{
    public bool[] Doors;
    //  public HoneyComb(bool north, bool northEast, bool south, bool southEast, bool northWest, bool southWest)
    public HoneyComb( bool north,bool northEast,  bool southEast, bool south, bool southWest, bool northWest )
    {
        Doors = new bool[] { north, northEast,  southEast, south, southWest, northWest};
        //{ north, northEast, south, southEast, northWest, southWest };
    }
    public void AddDoorsToHoneyComb(HoneyComb honeycomb)
    {
        for (int i = 0; i < Doors.Length; i++)
        {
            Doors[i] = Doors[i] || honeycomb.Doors[i] ? true : false;
        }
    }
    public int GetHoneyCombInfo()
    {
        int counter = 0;
        for (int i = 0; i < 6; i++)
        {
            counter += Doors[i] ? 1 << i : 0;
        }
        return counter;
    }
    public DoorExist NeedsHoneyCombFrom(Direction direction)
    {
        return Doors[(int)direction] ? DoorExist.Yes : DoorExist.No;
    }
}
