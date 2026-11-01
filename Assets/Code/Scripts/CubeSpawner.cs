using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Transform context;
    [SerializeField] private CubeElement elementPrefab;

    public Dictionary<Vector3Int, CubeElement> SpawnCube(int size)
    {
        Dictionary<Vector3Int, CubeElement> elements = new();

        var cubeSize = elementPrefab.transform.localScale.x;
        var halfSize = Mathf.FloorToInt(size / 2);
        
        var startPos = new Vector3(
            -cubeSize * halfSize,
            -cubeSize * halfSize,
            -cubeSize * halfSize);

        for(var x = 0; x < size; x++)
        {
            for(var y = 0; y < size; y++)
            {
                for(var z = 0; z < size; z++)
                {
                    var pos = new Vector3(
                        startPos.x + cubeSize * x,
                        startPos.y + cubeSize * y,
                        startPos.z + cubeSize * z);

                    var elem = Instantiate(elementPrefab, pos, Quaternion.identity, context);
                    EnableElementWalls(elem, new Vector3Int(x, y, z), size);
                    elements.Add(new Vector3Int(x, y, z), elem);
                }
            }
        }

        return elements;
    }

    private void EnableElementWalls(CubeElement element, Vector3Int pos, int size)
    {
        element.EnableRightWall = pos.x == size - 1;
        element.EnableLeftWall = pos.x == 0;
        element.EnableTopWall = pos.y == size - 1;
        element.EnableBottomWall = pos.y == 0;
        element.EnableFrontWall = pos.z == size - 1;
        element.EnableBackWall = pos.z == 0;
    }
}
