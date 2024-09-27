using UnityEngine;

public class PlantPlacement : MonoBehaviour
{
    public GridManager gridManager; // Ссылка на менеджер сетки
    public GameObject plantPrefab;  // Префаб растения
    public LineRenderer horizontalLine;  // Линия для горизонтальной полосы
    public LineRenderer verticalLine;    // Линия для вертикальной полосы

    private bool isPlacing = false;  // Флаг, активирован ли режим посадки

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Нажатие левой кнопки мыши
        {
            isPlacing = true; // Активируем режим посадки
        }

        if (Input.GetMouseButtonUp(0)) // Отпускание левой кнопки мыши
        {
            PlacePlant(); // Размещаем растение
            isPlacing = false; // Деактивируем режим посадки
        }

        if (isPlacing) // Если режим посадки активен, рисуем линии
        {
            DrawLines();
        }
    }

    void DrawLines()
    {
        // Получаем координаты мыши в мировых координатах
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Находим ближайшую ячейку по позиции мыши
        Vector2Int gridCell = gridManager.GetGridCellFromPosition(mousePosition);
        Vector3 cellPosition = gridManager.GetCellPosition(gridCell.x, gridCell.y);

        // Рисуем горизонтальную линию
        horizontalLine.SetPosition(0, new Vector3(0, cellPosition.y, 0));
        horizontalLine.SetPosition(1, new Vector3(gridManager.cols * gridManager.cellSize, cellPosition.y, 0));

        // Рисуем вертикальную линию
        verticalLine.SetPosition(0, new Vector3(cellPosition.x, 0, 0));
        verticalLine.SetPosition(1, new Vector3(cellPosition.x, gridManager.rows * gridManager.cellSize, 0));
    }

    void PlacePlant()
    {
        // Получаем координаты мыши в мировых координатах
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Находим ближайшую ячейку
        Vector2Int gridCell = gridManager.GetGridCellFromPosition(mousePosition);
        Vector3 cellPosition = gridManager.GetCellPosition(gridCell.x, gridCell.y);

        // Размещаем растение в этой ячейке
        Instantiate(plantPrefab, cellPosition, Quaternion.identity);
    }
}
