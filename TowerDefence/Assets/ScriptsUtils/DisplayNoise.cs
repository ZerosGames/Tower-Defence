using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNoise : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    public float persistance;
    public float lacanaity;

    public int seed;
    public Vector2 Offset;

    public Renderer textureRender;

    [SerializeField]
    private MapGenerator mapGen;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        float[,] noiseMap2 = PerlinNoise.GenerateNoiseMap(mapGen.grid.GetLength(0), mapGen.grid.GetLength(1), seed, noiseScale, octaves, persistance, lacanaity, Offset);

        float[,] noiseMap = new float[mapGen.grid.GetLength(0), mapGen.grid.GetLength(1)];

        int maxNoiseHeight = int.MinValue;

        for (int x = 0; x < mapGen.grid.GetLength(0); x++)
        {
            for (int y = 0; y < mapGen.grid.GetLength(1); y++)
            {
                if (mapGen.grid[x, y].pathDistance > maxNoiseHeight)
                {
                    maxNoiseHeight = mapGen.grid[x, y].pathDistance;
                }
            }
        }

        for (int x = 0; x < mapGen.grid.GetLength(0); x++)
        {
            for (int y = 0; y < mapGen.grid.GetLength(1); y++)
            {
                noiseMap[x, y] = (float)mapGen.grid[x, y].pathDistance / (float)maxNoiseHeight;
                //noiseMap[x, y] = Mathf.Pow(noiseMap[x, y], 1.25f);
                noiseMap[x, y] *= noiseMap2[x, y];
            }
        }

        DrawNoiseMap(noiseMap);
    }

    public void DrawNoiseMap(float[,] _noiseMap)
    {
        int width = _noiseMap.GetLength(0);
        int height = _noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colormap = new Color[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                colormap[y * width + x] = Color.Lerp(Color.black, Color.white, _noiseMap[x, y]);
            }
        }

        texture.SetPixels(colormap);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        textureRender.sharedMaterial.mainTexture = texture;
        transform.localScale = new Vector3(width, 1, height);
    }
}
