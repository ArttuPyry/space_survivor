using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    public GameObject mapMarker;
    public int gridSizeX;
    public int gridSizeY;
    public float spacingX = 1.0f;
    public float spacingY = 2.0f;

    private Dictionary<int, List<GameObject>> yGroups = new Dictionary<int, List<GameObject>>();

    private void Start()
    {
        GenerateGrid();
        GeneratePath();

        for (int i = 0; i < yGroups.Count; i++)
        {
            Debug.Log(yGroups[i]);
        }
        
        // Log all Y-axis groups and their dot coordinates
        int arrayIndex = 1;
        foreach (var group in yGroups)
        {
            int yValue = group.Key;
            List<GameObject> dotsInGroup = group.Value;

            Debug.Log($"Array {arrayIndex}:");

            foreach (var dot in dotsInGroup)
            {
                Vector3 dotPosition = dot.transform.localPosition;
                Debug.Log($"  Dot at ({dotPosition.x}, {dotPosition.y}, {dotPosition.z})");
            }

            arrayIndex++;
        }
    }

    private void GenerateGrid()
    {
        // Calculate grid center
        float centerX = (gridSizeX - 1) * spacingX * 0.5f;
        float centerY = (gridSizeY - 1) * spacingY * 0.5f;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Random offset
                float offsetX = Random.Range(-0.4f, 0.4f);
                float offsetY = Random.Range(-0.4f, 0.4f);
                // Calculate the position relative to the center of the grid
                float xPos = (x * spacingX + offsetX) - centerX;
                float yPos = (y * spacingY + offsetY) - centerY;

                GameObject mm = Instantiate(mapMarker);
                mm.transform.localPosition = new Vector3(xPos, yPos, 0);

                // Group dots by Y-axis value
                if (!yGroups.ContainsKey(y))
                {
                    yGroups[y] = new List<GameObject>();
                }

                yGroups[y].Add(mm);
            }
        }
    }

    private void GeneratePath()
    {
        List<GameObject> listThing = yGroups.Values.ToList()[1];
        int randomIndex = Random.Range(0, gridSizeX - 1);
        
        for (int i = 0; i < yGroups[0].Count; i++)
        {
            if (i != randomIndex)
            {
                listThing[i].SetActive(false);
            }
        }
    }
}
