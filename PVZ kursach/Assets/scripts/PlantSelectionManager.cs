using UnityEngine;
using UnityEngine.UI;

public class PlantSelectionManager : MonoBehaviour
{
    public Button plantButton; // Ссылка на кнопку растения
    public PlantPlacement plantPlacement; // Ссылка на скрипт PlantPlacement

    void Start()
    {
        // Подписываемся на событие нажатия кнопки
        plantButton.onClick.AddListener(OnPlantButtonClicked);
    }

    void OnPlantButtonClicked()
    {
        // Включаем режим посадки в скрипте PlantPlacement
        plantPlacement.enabled = true;
    }
}
