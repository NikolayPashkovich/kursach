using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EconomicController : MonoBehaviour
{
    // Start is called before the first frame update
    public static EconomicController instance { get; private set; }
    
    public int Suns { get; private set; }
    

    [SerializeField] float timeToSpawnSun;
    [SerializeField] Sun sunPrefab;
    float sunsTimer =0;

    bool isGameStarted = false;
    [SerializeField] MainCamScript mainCam;
    [SerializeField] ZombieSpawner zombieSpawner;
    [SerializeField] PlantPlacement plantPlacement;
    [SerializeField] UIController uiController;
    [SerializeField] MusicController musicController;
    public int maxPlants;
    bool isGameEnded = false;
    int levelNum;
    int levelOpenPlant;
    public void TrySelectPlant(SelectButton selectButton)
    {
        if (selectButton.plant.GetCost() > Suns) { return; }
        //RemoveSuns(plant.GetCost());
        plantPlacement.SelectPlacingPlant(selectButton);
    }
    
    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); }
        else
        {
            instance = this;
        }
        uiController.UpdateUI();
        Suns = 50;
        zombieSpawner.enabled = false;
        plantPlacement.enabled = false;
       
    }
    public void UpdateProgressBar(float value)
    {
        uiController.UpdateProgressBar(value);
    }
    public void StartLevel(LevelData levelData)
    {
        zombieSpawner.SetData(levelData);
        uiController.PutUpFlags(levelData.wavesPoints);
        levelNum = levelData.levelNumber;
        levelOpenPlant = levelData.levelOpenPlant;
        mainCam.GoToSelect();
        uiController.SetActiveSelect(true);
        zombieSpawner.enabled = true;
        zombieSpawner.SpawnZombiesForStart();
        zombieSpawner.enabled = false;
    }
    private void Start()
    {
        //mainCam.GoToSelect();
        //uiController.SetActiveSelect(true);
        //zombieSpawner.enabled = true;
        //zombieSpawner.SpawnZombiesForStart();
        //zombieSpawner.enabled = false;
    }
    public void GoToGame()
    {
        if (isGameStarted || uiController.selectButtons.Count == 0) { return; }
        StartCoroutine(GoToGameCorutine());
    }
    public IEnumerator GoToGameCorutine()
    {
        isGameStarted = true;
        uiController.GoToGame();
        uiController.UpdateUI();
        yield return StartCoroutine( mainCam.GoToGame());
        zombieSpawner.enabled = true;
        zombieSpawner.GoToGame();
        plantPlacement.enabled = true;
        musicController.GoToGame();
    }
    private void FixedUpdate()
    {
        if (isGameStarted)
        {
            sunsTimer += Time.fixedDeltaTime;
            if (sunsTimer >= timeToSpawnSun)
            {
                sunsTimer = 0;
                SpawnSun();
            }
            if (!isGameEnded && zombieSpawner.isEndSpavned && GridManager.Instance.zombies.Count == 0)
            {
                isGameEnded = true;
                StartCoroutine(EndGameRoutine());
            }
            if (!isGameEnded && zombieSpawner.isPlayerLoose)
            {
                isGameEnded = true;
                StartCoroutine(uiController.YouLooseRoutine());
                musicController.Loose();
            }
        }
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartCoroutine(EndGameRoutine());
        //}
    }
    public void EndSelect()
    {
        zombieSpawner.enabled = true;
        plantPlacement.enabled = true;
        mainCam.GoToGame();
        isGameStarted = true;
    }
    public void AddSuns(int plusSuns)
    {
        Suns += plusSuns;
        uiController.UpdateUI();
    }
    public void RemoveSuns(int minusSuns)
    {
        Suns -= minusSuns;
        uiController.UpdateUI();
    }
    
    void SpawnSun()
    {
        Vector3 target = GridManager.Instance.GetPositionFromGridCell(new Vector2Int(Random.Range(0, GridManager.Instance.cols), Random.Range(0, GridManager.Instance.rows)));
        target.z = -5;
        target.x += Random.Range(-GridManager.Instance.cellSize.x/2, GridManager.Instance.cellSize.x/2);
        sunPrefab.targetPos = target;
        Instantiate(sunPrefab.gameObject, new Vector3(target.x,5, target.z), Quaternion.Euler(0, 0, 0));
    }
    IEnumerator EndGameRoutine()
    {
        int savedLevelNum = FileManager.LoadLevelNumber();
        if (savedLevelNum < levelNum)
        {
            FileManager.SaveLevelNumber(levelNum);
        }
        if (levelOpenPlant != -1)
        {
            List<int> openedPlant = new List<int>( FileManager.LoadOpenPlants());
            if (!openedPlant.Contains(levelOpenPlant))
            {
                openedPlant.Add(levelOpenPlant);
                FileManager.SaveOpenPlants(openedPlant.ToArray());
            }
            StartCoroutine( uiController.endImageRoutine(levelOpenPlant));
        }
        yield return musicController.Win();


        yield return SceneManager.LoadSceneAsync(0);
    }
    public void GoToMenuButtonClick()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
