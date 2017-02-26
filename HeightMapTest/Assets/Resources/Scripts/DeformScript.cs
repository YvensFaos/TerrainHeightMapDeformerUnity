using UnityEngine;

public class DeformScript : MonoBehaviour
{
    [Header("Deform Terrain Properties")]
    public Terrain TargetTerrain;

    [Range(0.0f, 600.0f)]
    public float BaseTerrainHeight = 1.007142f;

    [Tooltip("Mark this true if you want your terrain to be cleaned to baseTerrainHeight every start.")]
    public bool RestartTerrain = true;

    public float DeformForce = 0.001f;
    private float RealDeformForce;

    [Range(1, 100)]
    public int Spread = 3;

    private int SpreadX;
    private int SpreadY;
    private float x, y;

    [Tooltip("Reference to the game object in which the position will be used to deform the terrain.")]
    public GameObject Deformer;

    [HideInInspector]
    public bool DEBUG = false;

    private Vector3 TerrainBaseCenter;
    private Vector3 TerrainBaseExtents;
    private int HeightmapWidth;
    private int HeightmapHeight;
    private Vector3 center;

    private void Start()
    {
        if (TargetTerrain == null)
        {
            Debug.LogError("No terrain attached to this script. Disable it.");
            enabled = false;
        }
        else
        {
            HeightmapWidth = TargetTerrain.terrainData.heightmapWidth;
            HeightmapHeight = TargetTerrain.terrainData.heightmapHeight;

            Bounds terrainBounds = TargetTerrain.terrainData.bounds;
            TerrainBaseCenter = terrainBounds.center;
            TerrainBaseExtents = terrainBounds.extents;
            if (RestartTerrain)
            {
                float[,] heights = new float[HeightmapWidth, HeightmapHeight];
                float realValue = BaseTerrainHeight / 600.0f;
                for (int i = 0; i < HeightmapWidth; i++)
                {
                    for (int j = 0; j < HeightmapHeight; j++)
                    {
                        heights[i, j] = realValue;
                    }
                }

                TargetTerrain.terrainData.SetHeights(0, 0, heights);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            center = Deformer.transform.position;
            if (DEBUG)
            {
                Debug.DrawRay(center, Vector3.down * 100.0f, Color.red);
            }

            if (center.x >= TerrainBaseCenter.x - TerrainBaseExtents.x &&
                center.x <= TerrainBaseCenter.x + TerrainBaseExtents.x &&
                center.z >= TerrainBaseCenter.z - TerrainBaseExtents.z &&
                center.z <= TerrainBaseCenter.z + TerrainBaseExtents.z)
            {
                x = center.x / (2 * TerrainBaseExtents.x);
                x = (x >= 0) ? x : 0;
                x = (x <= 1) ? x : 1;
                x *= HeightmapWidth;
                y = center.z / (2 * TerrainBaseExtents.z);
                y = (y >= 0) ? y : 0;
                y = (y <= 1) ? y : 1;
                y *= HeightmapHeight;

                SpreadX = (x + Spread >= HeightmapWidth) ? (int)(HeightmapWidth - x) : Spread;
                SpreadY = (y + Spread >= HeightmapHeight) ? (int)(HeightmapHeight - y) : Spread;

                float[,] value = TargetTerrain.terrainData.GetHeights((int)x, (int)y, SpreadX, SpreadY);
                RealDeformForce = (DeformForce / 600.0f);
                for (int i = 0; i < SpreadY; i++)
                {
                    for (int j = 0; j < SpreadX; j++)
                    {
                        if (DEBUG)
                        {
                            Debug.Log(value[i, j]);
                        }
                        value[i, j] += RealDeformForce;
                    }
                }
                TargetTerrain.terrainData.SetHeights((int)x, (int)y, value);
            }
        }
    }
}