using System;
using UnityEngine;

public class GridFloor : MonoBehaviour
{
    MeshFilter _meshFilter;
    [SerializeField] Vector3 minBound;
    [SerializeField] Vector3 maxBound;
    
    [SerializeField] private float padding = 1f;

    
    Grid[][] _grid;
    [SerializeField] private int gridSize = 7;

    public float distanceBetweenGrids = 0;
    public GameObject box;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        minBound = _meshFilter.mesh.bounds.min + new Vector3(padding, 0, padding);
        maxBound = _meshFilter.mesh.bounds.max - new Vector3(padding, 0, padding);
        Debug.Log(this.maxBound);
        
        distanceBetweenGrids = maxBound.x / (float)((int)gridSize/2);
        GenerateGrid();
        
    }

    private void GenerateGrid()
    {
        _grid = new Grid[gridSize][];
        for(int i = 0; i < gridSize; i++)
        {
            _grid[i] = new Grid[gridSize];
            for(int j = 0; j < gridSize; j++)
            {
                _grid[i][j] = new Grid();
                _grid[i][j].center = new Vector3(
                    minBound.x + distanceBetweenGrids * i,
                    0,
                    minBound.z + distanceBetweenGrids * j);
                _grid[i][j].row = i;
                _grid[i][j].col = j;
                
                Debug.Log($"{i} {j} {_grid[i][j].center}");
                
                if(doDebugBox)Instantiate(box, _grid[i][j].center + Vector3.up, Quaternion.identity);
            }
        }
    }

    public bool doDebugBox = false;
    public Grid GetRandomGrid()
    {
        int row = UnityEngine.Random.Range(0, gridSize);
        int col = UnityEngine.Random.Range(0, gridSize);
        return _grid[row][col];
    }
}

public class Grid
{
    public Vector3 center;
    public int row;
    public int col;
}
