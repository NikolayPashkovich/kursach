using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 5;  // Количество рядов
    public int cols = 9;  // Количество столбцов
    public float cellWidth = 1f;  // Ширина ячеек
    public float cellHeight = 0.75f;  // Высота ячеек (делаем их прямоугольными)
    public Vector3 gridOrigin = Vector3.zero;  // Начальная позиция сетки

    public Vector3[,] gridPositions;  // Массив для хранения позиций ячеек

    void Start()
    {
        CreateGrid();  // Создание сетки при старте игры
    }

    // Создание сетки
    void CreateGrid()
    {
        gridPositions = new Vector3[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Позиции ячеек с учетом их ширины, высоты и начального положения сетки
                gridPositions[row, col] = gridOrigin + new Vector3(col * cellWidth, row * cellHeight, 0);
            }
        }
    }

    // Возвращает ближайшую ячейку по позиции мыши
    public Vector3 GetGridCellFromPosition(Vector3 position)
    {
        Vector3 localPosition = position - gridOrigin;  // Считаем локальную позицию относительно начала сетки

        // Вычисляем ближайшие индексы ячейки (учитывая размеры ячеек)
        int col = Mathf.Clamp(Mathf.FloorToInt(localPosition.x / cellWidth), 0, cols - 1);
        int row = Mathf.Clamp(Mathf.FloorToInt(localPosition.y / cellHeight), 0, rows - 1);

        return gridPositions[row, col];  // Возвращаем позицию центра ячейки
    }
}
