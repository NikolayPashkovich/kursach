using UnityEngine;
using UnityEngine.UI;

public class PlantSelectionManager : MonoBehaviour
{
    public Button plantButton; // ������ �� ������ ��������
    public PlantPlacement plantPlacement; // ������ �� ������ PlantPlacement

    void Start()
    {
        // ������������� �� ������� ������� ������
        plantButton.onClick.AddListener(OnPlantButtonClicked);
    }

    void OnPlantButtonClicked()
    {
        // �������� ����� ������� � ������� PlantPlacement
        plantPlacement.enabled = true;
    }
}
