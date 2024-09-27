using UnityEngine;

public class PlantPlacement : MonoBehaviour
{
    public GridManager gridManager;  // Ссылка на объект сетки
    public GameObject plantPrefab;  // Префаб растения
    public LineRenderer horizontalLine;  // Линия для горизонтали
    public LineRenderer verticalLine;  // Линия для вертикали
    public Material transparentLineMaterial;  // Полупрозрачный материал для линий

    private bool isPlacing = false;  // Флаг, активен ли режим посадки

    void Update()
    {
        // Если включен режим посадки, отслеживаем перемещение мыши
        if (isPlacing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellPosition = gridManager.GetGridCellFromPosition(mousePosition);

            // Обновляем отрисовку линий на экране
            DrawLines(cellPosition);

            // Если отпущена кнопка мыши - выполняем посадку растения
            if (Input.GetMouseButtonUp(0))
            {
                PlacePlant(cellPosition);
            }
        }
    }

    // Активация режима посадки растения
    public void ActivatePlantPlacement()
    {
        isPlacing = true;
        horizontalLine.enabled = true;
        verticalLine.enabled = true;

        // Применяем полупрозрачный материал к линиям
        horizontalLine.material = transparentLineMaterial;
        verticalLine.material = transparentLineMaterial;
    }

    // Посадка растения в выбранную ячейку
    void PlacePlant(Vector3 cellPosition)
    {
        // Создаем растение в выбранной позиции
        Instantiate(plantPrefab, cellPosition, Quaternion.identity);

        // Отключаем линии после посадки
        horizontalLine.enabled = false;
        verticalLine.enabled = false;

        // Завершаем режим посадки
        isPlacing = false;
    }

    // Отрисовка линий, которые пересекаются в выбранной ячейке
    void DrawLines(Vector3 cellPosition)
    {
        // Учитываем положение сетки при отрисовке линий
        Vector3 origin = gridManager.gridOrigin;

        // Горизонтальная линия
        horizontalLine.SetPosition(0, new Vector3(origin.x, cellPosition.y, 0));
        horizontalLine.SetPosition(1, new Vector3(origin.x + gridManager.cols * gridManager.cellWidth, cellPosition.y, 0));

        // Вертикальная линия
        verticalLine.SetPosition(0, new Vector3(cellPosition.x, origin.y, 0));
        verticalLine.SetPosition(1, new Vector3(cellPosition.x, origin.y + gridManager.rows * gridManager.cellHeight, 0));
    }
}
