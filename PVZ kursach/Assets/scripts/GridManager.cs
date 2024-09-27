using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 5; // Количество строк
    public int cols = 9; // Количество колонок
    public float cellSize = 1f; // Размер каждой ячейки
    public Vector3 gridOrigin = Vector3.zero; // Позиция, с которой начинается сетка


    private Vector3[,] gridPositions; // Массив для хранения позиций ячеек

    void Start()
    {
        CreateGrid(); // Создаем сетку при старте игры
    }

    void CreateGrid()
    {
        gridPositions = new Vector3[rows, cols]; // Инициализируем массив

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Вычисляем позицию каждой ячейки с учетом начальной позиции gridOrigin
                gridPositions[row, col] = gridOrigin + new Vector3(col * cellSize, row * cellSize, 0);

                // Для отладки отображаем линии сетки (если нужно)
                Debug.DrawLine(gridOrigin + new Vector3(col * cellSize, 0, 0), gridOrigin + new Vector3(col * cellSize, rows * cellSize, 0), Color.white, 100f);
                Debug.DrawLine(gridOrigin + new Vector3(0, row * cellSize, 0), gridOrigin + new Vector3(cols * cellSize, row * cellSize, 0), Color.white, 100f);
            }
        }
    }


    // Метод для получения позиции ячейки
    public Vector3 GetCellPosition(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            return gridPositions[row, col];
        }
        return Vector3.zero;
    }

    // Метод для нахождения ближайшей ячейки по позиции мыши
    public Vector2Int GetGridCellFromPosition(Vector3 position)
    {
        int row = Mathf.Clamp(Mathf.FloorToInt(position.y / cellSize), 0, rows - 1);
        int col = Mathf.Clamp(Mathf.FloorToInt(position.x / cellSize), 0, cols - 1);
        return new Vector2Int(row, col);
    }
}
