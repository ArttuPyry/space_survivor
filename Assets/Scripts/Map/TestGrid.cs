using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    public GameObject mapMarker;
    public int gridSizeX;
    public int gridSizeY;
    public float spacingX = 1.0f;
    public float spacingY = 2.0f;

    public int minMarkerDisable;
    public int maxMarkerDisable;

    public GameObject start;
    public GameObject boss;

    private Dictionary<int, List<GameObject>> yGroups = new Dictionary<int, List<GameObject>>();
    public List<LineRenderer> lineRenderers;
    private Dictionary<int, List<GameObject>> paths = new Dictionary<int, List<GameObject>>();

    public void RandomizeThisthing()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            paths[i].Clear();
        }
        MakePaths();
        DrawLines();
    }
    private void Start()
    {
        GenerateGrid();
        MakePaths();
        DrawLines();

        print((yGroups[0][0].transform.position + yGroups[1][1].transform.position) / 2);
        print((yGroups[0][1].transform.position + yGroups[1][0].transform.position) / 2);
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
                float xPos = (x * spacingX) - centerX;
                float yPos = (y * spacingY) - centerY;

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

    private void MakePaths()
    {
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            paths[i] = new List<GameObject>();
            paths[i].Add(start);
            int lastIndex = 0;

            for (int d = 0; d < yGroups.Count; d++)
            {
                List<GameObject> row = yGroups.Values.ToList()[d];

                int randomIndex;

                if (d == 0)
                {
                    randomIndex = Random.Range(0, yGroups[d].Count);
                }
                else
                {
                    print(lastIndex);
                    int min = lastIndex - 2;
                    int max = lastIndex + 2;
                    if (min < 0) { min = 0; }
                    if (max > yGroups[d].Count) { max = yGroups[d].Count; }
                    randomIndex = Random.Range(min, max);
                }

                lastIndex = randomIndex;

                paths[i].Add(row[randomIndex]);
            }

            paths[i].Add(boss);
        }
    }

    private void DrawLines()
    {
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            List<GameObject> path = paths.Values.ToList()[i];
            lineRenderers[i].positionCount = path.Count;
            lineRenderers[i].startColor = Color.yellow;
            lineRenderers[i].endColor = Color.yellow;
            lineRenderers[i].startWidth = 0.1f;
            lineRenderers[i].endWidth = 0.1f;
            for (int b = 0; b < path.Count; b++)
            {
                lineRenderers[i].SetPosition(b, path[b].transform.position);
            }
        }
    }


    private void DisableMapMarkers()
    {
        for (int i = 0; i < yGroups.Count; i++)
        {
            List<GameObject> row = yGroups.Values.ToList()[i];
            int disableAmount = Random.Range(minMarkerDisable, maxMarkerDisable);

            for (int a = 0; a < disableAmount; a++)
            {
                int randomIndex;

                do
                {
                    randomIndex = Random.Range(0, row.Count);
                } while (!row[randomIndex].activeSelf);

                row[randomIndex].SetActive(false);
            }
        }
    }
}
