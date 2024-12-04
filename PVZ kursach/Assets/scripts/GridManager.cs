using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; } // Singleton Instance

    public int rows = 5; // Количество строк
    public int cols = 9; // Количество колонок
    public Vector2 cellSize = new Vector2(1, 1); // Размер каждой ячейки
    public Vector3 gridOrigin = Vector3.zero; // Позиция, с которой начинается сетка

    public List<Zombie> zombies = new List<Zombie>();
    public List<Plant> plants = new List<Plant>();

    

    
    private void Awake()
    {
        // Singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Уничтожаем объект, если он уже существует
        }
        else
        {
            Instance = this; // Назначаем текущий объект в качестве Instance
        }

        CreateGrid();
    }

    void CreateGrid()
    {

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Вычисляем позицию каждой ячейки с учетом начальной позиции gridOrigin
                

                // Для отладки отображаем линии сетки (если нужно)
                Debug.DrawLine(gridOrigin + new Vector3(col * cellSize.x, 0, 0), gridOrigin + new Vector3(col * cellSize.x, rows * cellSize.x, 0), Color.white, 100f);
                Debug.DrawLine(gridOrigin + new Vector3(0, row * cellSize.y, 0), gridOrigin + new Vector3(cols * cellSize.y, row * cellSize.y, 0), Color.white, 100f);
            }
        }
    }

    public Vector3 GetPositionFromGridCell(Vector2Int gridCell)
    {
        return gridOrigin + new Vector3(gridCell.x * cellSize.x, gridCell.y * cellSize.y, 0);
    }
    // Метод для нахождения ближайшей ячейки по позиции мыши
    public Vector2Int GetGridCellFromPosition(Vector2 position)
    {
        position = position - (Vector2)gridOrigin + (cellSize / 2);
        if (position.x < 0 || position.y < 0 || position.x > cols * cellSize.x || position.y > rows * cellSize.y)
        {
            return new Vector2Int(-1, -1);
        }
        int row = Mathf.Clamp(Mathf.FloorToInt(position.y / cellSize.y), 0, rows);
        int col = Mathf.Clamp(Mathf.FloorToInt(position.x / cellSize.x), 0, cols);
        return new Vector2Int(col, row);
    }
}
