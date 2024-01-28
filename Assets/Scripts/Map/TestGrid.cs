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
    private Dictionary<int, List<GameObject>> paths = new Dictionary<int, List<GameObject>>();
    public List<LineRenderer> lineRenderers;
    public List<Vector2> takenGridGaps;

    public void RandomizeThisthing()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            paths[i].Clear();
        }

        for (int x = 0; x < yGroups.Count; x++)
        {
            for (int d = 0; d < yGroups[x].Count; d++)
            {
                Destroy(yGroups[x][d]);
            }
        }
        yGroups.Clear();
        takenGridGaps.Clear();
        GenerateGrid();
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
                    do
                    {
                        randomIndex = Random.Range(0, yGroups[d].Count);
                        
                    } while (takenGridGaps.Contains((start.transform.position + yGroups[d][randomIndex].transform.position) / 2));

                    if (start.transform.position.x != yGroups[d][randomIndex].transform.position.x)
                    {
                        takenGridGaps.Add((start.transform.position + yGroups[d][randomIndex].transform.position) / 2);
                    } 
                }
                else
                {
                    int min = lastIndex - 1;
                    int max = lastIndex + 1;
                    if (min < 0) { min = 0; }
                    if (max > yGroups[d].Count) { max = yGroups[d].Count; }

                    do
                    {
                        randomIndex = Random.Range(min, max);
                        
                        if (randomIndex == min)
                        {
                            int extraRandi = Random.Range(1, 2);
                            if (extraRandi == 1) randomIndex = Random.Range(min, max);
                        }
                    } while (takenGridGaps.Contains((yGroups[d - 1][lastIndex].transform.position + yGroups[d][randomIndex].transform.position) / 2));
                    
                    if (yGroups[d - 1][lastIndex].transform.position.x != yGroups[d][randomIndex].transform.position.x)
                    {
                        takenGridGaps.Add((yGroups[d - 1][lastIndex].transform.position + yGroups[d][randomIndex].transform.position) / 2);
                    }
                }

                lastIndex = randomIndex;

                paths[i].Add(row[randomIndex]);
            }

            paths[i].Add(boss);
        }
    }

    private void DrawLines()
    {
        for (int x = 0; x < yGroups.Count; x++)
        {
            for (int d = 0; d < yGroups[x].Count; d++)
            {
                // Random offset
                float offsetX = Random.Range(-0.2f, 0.2f);
                float offsetY = Random.Range(-0.2f, 0.2f);
                Vector3 currentPosition = (yGroups[x][d].transform.localPosition);
                Vector3 newPosition = new Vector3(currentPosition.x + offsetX, currentPosition.y + offsetY, currentPosition.z);
                yGroups[x][d].transform.localPosition = newPosition;
            }
        }

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
