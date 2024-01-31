using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    public GameObject mapMarker;
    public int gridSizeX;
    public int gridSizeY;
    public float spacingX = 1.0f;
    public float spacingY = 2.0f;

    public GameObject start;
    public GameObject boss;

    private Dictionary<int, List<GameObject>> yGroups = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> paths = new Dictionary<int, List<GameObject>>();
    public List<Vector2[]> takenGridGaps = new List<Vector2[]>();
    public List<LineRenderer> lineRenderers;

    public void RandomizeThisthing()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            paths[i].Clear();
        }
        paths.Clear();

        for (int x = 0; x < yGroups.Count; x++)
        {
            for (int d = 0; d < yGroups[x].Count; d++)
            {
                Destroy(yGroups[x][d]);
                
            }
            yGroups[x].Clear();
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
            int straightCount = 0;

            for (int j = 0; j < yGroups.Count; j++)
            {
                List<GameObject> row = yGroups.Values.ToList()[j];

                int randomIndex;

                if (j == 0)
                {
                    randomIndex = Random.Range(0, yGroups[j].Count);
                }
                else
                {
                    int min = lastIndex - 1;
                    int max = lastIndex + 1;
                    if (min < 0)
                    {
                        min = 0;
                    }


                    if (max >= yGroups[j].Count)
                    {
                        max = yGroups[j].Count - 1;
                    }

                    randomIndex = Random.Range(min, max);

                    if (yGroups[j - 1][lastIndex].transform.position.x == yGroups[j][randomIndex].transform.position.x)
                    {
                        straightCount++;
                    } 

                    if (straightCount == 2)
                    {
                        randomIndex = max;
                        straightCount = 0;
                    }

                    if (lastIndex != min && randomIndex != max)
                    {
                        if (IsVectorPairInList(yGroups[j - 1][lastIndex - 1].transform.position, yGroups[j][randomIndex + 1].transform.position))
                        {
                            randomIndex = max;
                            if (lastIndex != max && randomIndex != min)
                            {
                                if (IsVectorPairInList(yGroups[j - 1][lastIndex + 1].transform.position, yGroups[j][randomIndex - 1].transform.position))
                                {
                                    randomIndex = lastIndex;
                                }
                            }
                        }
                    }

                    if (lastIndex != max && randomIndex != min)
                    {
                        if (IsVectorPairInList(yGroups[j - 1][lastIndex + 1].transform.position, yGroups[j][randomIndex - 1].transform.position))
                        {
                            randomIndex = min;
                            if (lastIndex != min && randomIndex != max)
                            {
                                if (IsVectorPairInList(yGroups[j - 1][lastIndex - 1].transform.position, yGroups[j][randomIndex + 1].transform.position))
                                {
                                    randomIndex = lastIndex;
                                }
                            }
                        }
                    }

                    // If line doesn't go staright up add it to taken grid gaps list
                    if (yGroups[j - 1][lastIndex].transform.position.x != yGroups[j][randomIndex].transform.position.x)
                    {
                        Vector2[] tmpArray = new Vector2[]
                        {
                            yGroups[j - 1][lastIndex].transform.position,
                            yGroups[j][randomIndex].transform.position
                        };
                        takenGridGaps.Add(tmpArray);
                    }
                }

                lastIndex = 0;
                lastIndex = randomIndex;

                paths[i].Add(row[randomIndex]);
            }
            straightCount = 0;
            paths[i].Add(boss);
        }
    }

    bool IsVectorPairInList(Vector2 vector1, Vector2 vector2)
    {
        foreach (var vectorArray in takenGridGaps)
        {
            if (vectorArray[0] == vector1 && vectorArray[1] == vector2) {  return true; }
        }
        return false;
    }

    private void DrawLines()
    {
        for (int i = 0; i < yGroups.Count; i++)
        {
            for (int j = 0; j < yGroups[i].Count; j++)
            {
                // Random offset
                float offsetX = Random.Range(-0.2f, 0.2f);
                float offsetY = Random.Range(-0.2f, 0.2f);
                Vector3 currentPosition = (yGroups[i][j].transform.localPosition);
                Vector3 newPosition = new Vector3(currentPosition.x + offsetX, currentPosition.y + offsetY, currentPosition.z);
                yGroups[i][j].transform.localPosition = newPosition;
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
            for (int j = 0; j < path.Count; j++)
            {
                lineRenderers[i].SetPosition(j, path[j].transform.position);
            }
        }
    }
}
