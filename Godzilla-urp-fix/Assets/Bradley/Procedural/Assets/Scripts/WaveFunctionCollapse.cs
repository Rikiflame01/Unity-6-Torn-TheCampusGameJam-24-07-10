//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//[ExecuteInEditMode]
//public class WaveFunctionCollapse : MonoBehaviour
//{
//    public int dimensions;
//    public Tile[] tileObjects;
//    public List<Cell> gridComponents;
//    public Cell cellObj;

//    public GameObject mapParent;

//    public Tile backupTile;

//    private int iteration;
//    private bool isGenerating;

//    private void Awake()
//    {
//        if (!Application.isPlaying)
//        {
//            gridComponents = new List<Cell>();
//        }
//    }

//    public void StartGeneration()
//    {
//        DeleteGeneratedBuildings();
//        InitializeGrid();
//        EditorApplication.update += EditorUpdate;
//    }

//    void InitializeGrid()
//    {
//        gridComponents.Clear();

//        for (int y = 0; y < dimensions; y++)
//        {
//            for (int x = 0; x < dimensions; x++)
//            {
//                Cell newCell = Instantiate(cellObj, new Vector3(x * 10, 0, y * 10), Quaternion.identity);
//                newCell.CreateCell(false, tileObjects);
//                gridComponents.Add(newCell);
//            }
//        }

//        isGenerating = true;
//        iteration = 0;
//    }

//    void EditorUpdate()
//    {
//        if (isGenerating)
//        {
//            CheckEntropy();
//        }
//    }

//    void CheckEntropy()
//    {
//        List<Cell> tempGrid = new List<Cell>(gridComponents);
//        tempGrid.RemoveAll(c => c.collapsed);
//        tempGrid.Sort((a, b) => a.tileOptions.Length - b.tileOptions.Length);
//        tempGrid.RemoveAll(a => a.tileOptions.Length != tempGrid[0].tileOptions.Length);

//        CollapseCell(tempGrid);

//        if (iteration >= dimensions * dimensions)
//        {
//            isGenerating = false;
//            EditorApplication.update -= EditorUpdate;
//            DeleteAllCells();  // Call the method to delete all cells after generation is complete
//        }
//    }

//    void CollapseCell(List<Cell> tempGrid)
//    {
//        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count);

//        Cell cellToCollapse = tempGrid[randIndex];

//        cellToCollapse.collapsed = true;
//        try
//        {
//            Tile selectedTile = cellToCollapse.tileOptions[UnityEngine.Random.Range(0, cellToCollapse.tileOptions.Length)];
//            cellToCollapse.tileOptions = new Tile[] { selectedTile };
//        }
//        catch
//        {
//            Tile selectedTile = backupTile;
//            cellToCollapse.tileOptions = new Tile[] { selectedTile };
//        }

//        Tile foundTile = cellToCollapse.tileOptions[0];
//        Instantiate(foundTile, cellToCollapse.transform.position, foundTile.transform.rotation, mapParent.transform);

//        UpdateGeneration();
//    }

//    void UpdateGeneration()
//    {
//        List<Cell> newGenerationCell = new List<Cell>(gridComponents);

//        for (int y = 0; y < dimensions; y++)
//        {
//            for (int x = 0; x < dimensions; x++)
//            {
//                var index = x + y * dimensions;

//                if (gridComponents[index].collapsed)
//                {
//                    newGenerationCell[index] = gridComponents[index];
//                }
//                else
//                {
//                    List<Tile> options = new List<Tile>();
//                    foreach (Tile t in tileObjects)
//                    {
//                        options.Add(t);
//                    }

//                    if (y > 0)
//                    {
//                        Cell up = gridComponents[x + (y - 1) * dimensions];
//                        List<Tile> validOptions = new List<Tile>();

//                        foreach (Tile possibleOptions in up.tileOptions)
//                        {
//                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
//                            var valid = tileObjects[validOption].downNeighbours;

//                            validOptions = validOptions.Concat(valid).ToList();
//                        }

//                        CheckValidity(options, validOptions);
//                    }

//                    if (x < dimensions - 1)
//                    {
//                        Cell left = gridComponents[x + 1 + y * dimensions];
//                        List<Tile> validOptions = new List<Tile>();

//                        foreach (Tile possibleOptions in left.tileOptions)
//                        {
//                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
//                            var valid = tileObjects[validOption].rightNeighbours;

//                            validOptions = validOptions.Concat(valid).ToList();
//                        }

//                        CheckValidity(options, validOptions);
//                    }

//                    if (y < dimensions - 1)
//                    {
//                        Cell down = gridComponents[x + (y + 1) * dimensions];
//                        List<Tile> validOptions = new List<Tile>();

//                        foreach (Tile possibleOptions in down.tileOptions)
//                        {
//                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
//                            var valid = tileObjects[validOption].upNeighbours;

//                            validOptions = validOptions.Concat(valid).ToList();
//                        }

//                        CheckValidity(options, validOptions);
//                    }

//                    if (x > 0)
//                    {
//                        Cell right = gridComponents[x - 1 + y * dimensions];
//                        List<Tile> validOptions = new List<Tile>();

//                        foreach (Tile possibleOptions in right.tileOptions)
//                        {
//                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
//                            var valid = tileObjects[validOption].leftNeighbours;

//                            validOptions = validOptions.Concat(valid).ToList();
//                        }

//                        CheckValidity(options, validOptions);
//                    }

//                    Tile[] newTileList = new Tile[options.Count];

//                    for (int i = 0; i < options.Count; i++)
//                    {
//                        newTileList[i] = options[i];
//                    }

//                    newGenerationCell[index].RecreateCell(newTileList);
//                }
//            }
//        }

//        gridComponents = newGenerationCell;
//        iteration++;
//    }

//    void CheckValidity(List<Tile> optionList, List<Tile> validOption)
//    {
//        for (int x = optionList.Count - 1; x >= 0; x--)
//        {
//            var element = optionList[x];
//            if (!validOption.Contains(element))
//            {
//                optionList.RemoveAt(x);
//            }
//        }
//    }

//    void DeleteAllCells()
//    {
//        foreach (var cell in gridComponents)
//        {
//            if (cell != null)
//            {
//                DestroyImmediate(cell.gameObject);
//            }
//        }
//        gridComponents.Clear();  // Clear the list to remove references
//    }

//    public void DeleteGeneratedBuildings()
//    {
//        // Delete all existing cells in gridComponents
//        DeleteAllCells();

//        // Clear any existing buildings under mapParent
//        if (mapParent != null)
//        {
//            while (mapParent.transform.childCount > 0)
//            {
//                foreach (Transform child in mapParent.transform)
//                {
//                    DestroyImmediate(child.gameObject);
//                }
//            }

//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class WaveFunctionCollapse : MonoBehaviour
{
    public int width;
    public int height;
    public Tile[] tileObjects;
    public List<Cell> gridComponents;
    public Cell cellObj;

    public GameObject mapParent;

    public Tile backupTile;

    private int iteration;
    private bool isGenerating;

    private void Awake()
    {
        if (!Application.isPlaying)
        {
            gridComponents = new List<Cell>();
        }
    }

    public void StartGeneration()
    {
        DeleteGeneratedBuildings();
        InitializeGrid();
        //EditorApplication.update += EditorUpdate;
    }

    void InitializeGrid()
    {
        gridComponents.Clear();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell newCell = Instantiate(cellObj, new Vector3(x * 10, 0, y * 10), Quaternion.identity);
                newCell.CreateCell(false, tileObjects);
                gridComponents.Add(newCell);
            }
        }

        isGenerating = true;
        iteration = 0;
    }

    void EditorUpdate()
    {
        if (isGenerating)
        {
            CheckEntropy();
        }
    }

    void CheckEntropy()
    {
        List<Cell> tempGrid = new List<Cell>(gridComponents);
        tempGrid.RemoveAll(c => c.collapsed);
        tempGrid.Sort((a, b) => a.tileOptions.Length - b.tileOptions.Length);
        tempGrid.RemoveAll(a => a.tileOptions.Length != tempGrid[0].tileOptions.Length);

        CollapseCell(tempGrid);

        if (iteration >= width * height)
        {
            isGenerating = false;
            //EditorApplication.update -= EditorUpdate;
            DeleteAllCells();  // Call the method to delete all cells after generation is complete
        }
    }

    void CollapseCell(List<Cell> tempGrid)
    {
        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count);

        Cell cellToCollapse = tempGrid[randIndex];

        cellToCollapse.collapsed = true;
        try
        {
            Tile selectedTile = cellToCollapse.tileOptions[UnityEngine.Random.Range(0, cellToCollapse.tileOptions.Length)];
            cellToCollapse.tileOptions = new Tile[] { selectedTile };
        }
        catch
        {
            Tile selectedTile = backupTile;
            cellToCollapse.tileOptions = new Tile[] { selectedTile };
        }

        Tile foundTile = cellToCollapse.tileOptions[0];
        Instantiate(foundTile, cellToCollapse.transform.position, foundTile.transform.rotation, mapParent.transform);

        UpdateGeneration();
    }

    void UpdateGeneration()
    {
        List<Cell> newGenerationCell = new List<Cell>(gridComponents);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var index = x + y * width;

                if (gridComponents[index].collapsed)
                {
                    newGenerationCell[index] = gridComponents[index];
                }
                else
                {
                    List<Tile> options = new List<Tile>();
                    foreach (Tile t in tileObjects)
                    {
                        options.Add(t);
                    }

                    if (y > 0)
                    {
                        Cell up = gridComponents[x + (y - 1) * width];
                        List<Tile> validOptions = new List<Tile>();

                        foreach (Tile possibleOptions in up.tileOptions)
                        {
                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            var valid = tileObjects[validOption].downNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (x < width - 1)
                    {
                        Cell left = gridComponents[x + 1 + y * width];
                        List<Tile> validOptions = new List<Tile>();

                        foreach (Tile possibleOptions in left.tileOptions)
                        {
                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            var valid = tileObjects[validOption].rightNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (y < height - 1)
                    {
                        Cell down = gridComponents[x + (y + 1) * width];
                        List<Tile> validOptions = new List<Tile>();

                        foreach (Tile possibleOptions in down.tileOptions)
                        {
                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            var valid = tileObjects[validOption].upNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (x > 0)
                    {
                        Cell right = gridComponents[x - 1 + y * width];
                        List<Tile> validOptions = new List<Tile>();

                        foreach (Tile possibleOptions in right.tileOptions)
                        {
                            var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                            var valid = tileObjects[validOption].leftNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    Tile[] newTileList = new Tile[options.Count];

                    for (int i = 0; i < options.Count; i++)
                    {
                        newTileList[i] = options[i];
                    }

                    newGenerationCell[index].RecreateCell(newTileList);
                }
            }
        }

        gridComponents = newGenerationCell;
        iteration++;
    }

    void CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        for (int x = optionList.Count - 1; x >= 0; x--)
        {
            var element = optionList[x];
            if (!validOption.Contains(element))
            {
                optionList.RemoveAt(x);
            }
        }
    }

    void DeleteAllCells()
    {
        foreach (var cell in gridComponents)
        {
            if (cell != null)
            {
                DestroyImmediate(cell.gameObject);
            }
        }
        gridComponents.Clear();  // Clear the list to remove references
    }

    public void DeleteGeneratedBuildings()
    {
        // Delete all existing cells in gridComponents
        DeleteAllCells();

        // Clear any existing buildings under mapParent
        if (mapParent != null)
        {
            while (mapParent.transform.childCount > 0)
            {
                foreach (Transform child in mapParent.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}