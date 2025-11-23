using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 10;
    [SerializeField] private int depth = 10;
    [SerializeField] private float spacing = 0f; 

    [Header("References")]
    [SerializeField] private GameObject cubePrefab;

    private void Start()
    {
        GenerateGrid();
    }

    [ContextMenu("Generate Grid")] 
    public void GenerateGrid()
    {
        if (Application.isEditor)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xPos = x * (1f + spacing);
                float zPos = z * (1f + spacing);

                Vector3 spawnPosition = new Vector3(xPos, 0, zPos) + transform.position;

     
                GameObject spawnedCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
                spawnedCube.name = $"Cube_{x}_{z}";
                spawnedCube.transform.parent = this.transform;
            }
        }
    }
}