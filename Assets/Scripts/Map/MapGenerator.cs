using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mapMarker;
    [SerializeField] private int gridSizeX;
    [SerializeField] private int gridSizeY;
    [SerializeField] private float spacingX = 1.0f;
    [SerializeField] private float spacingY = 2.0f;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject boss;

    private Dictionary<int, List<GameObject>> yGroups = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> paths = new Dictionary<int, List<GameObject>>();
    private List<Vector2[]> takenGridGaps = new List<Vector2[]>();
    [SerializeField] private List<LineRenderer> lineRenderers;

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
        // Linerenderer amount = path amount
        for (int i = 0; i < lineRenderers.Count; i++)
        {
            paths[i] = new List<GameObject>();
            paths[i].Add(start);
            int lastIndex = 0;
            int straightCount = 0;

            // Get path spot from every row
            for (int j = 0; j < yGroups.Count; j++)
            {
                List<GameObject> row = yGroups.Values.ToList()[j];

                int randomIndex;

                // first always random
                if (j == 0)
                {
                    randomIndex = Random.Range(0, yGroups[j].Count);
                }
                else
                {
                    // Get last and make sure it doesn't go outside of list
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

                    // Randomize index
                    randomIndex = Random.Range(min, max);

                    // Add straight count
                    if (yGroups[j - 1][lastIndex].transform.position.x == yGroups[j][randomIndex].transform.position.x)
                    {
                        straightCount++;
                    }

                    // If too many straights in a row make randomidex max (max also prevents paths leaning too much left)
                    if (straightCount == 2)
                    {
                        randomIndex = max;
                        straightCount = 0;
                    }

                    // Check if paths cross and make it go other way but even that crosses then go staright
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
                    // same as last but other way
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

    // This check if Vector2 x2 are in taken grid gaps array ( used to make sure that paths don't cross )
    bool IsVectorPairInList(Vector2 vector1, Vector2 vector2)
    {
        foreach (var vectorArray in takenGridGaps)
        {
            if (vectorArray[0] == vector1 && vectorArray[1] == vector2) { return true; }
        }
        return false;
    }

    private void DrawLines()
    {
        for (int i = 0; i < yGroups.Count; i++)
        {
            for (int j = 0; j < yGroups[i].Count; j++)
            {
                // Random offset for better visuals
                float offsetX = Random.Range(-0.2f, 0.2f);
                float offsetY = Random.Range(-0.2f, 0.2f);
                Vector3 currentPosition = (yGroups[i][j].transform.localPosition);
                Vector3 newPosition = new Vector3(currentPosition.x + offsetX, currentPosition.y + offsetY, currentPosition.z);
                yGroups[i][j].transform.localPosition = newPosition;
            }
        }

        // This draws the lines duh
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
