using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "levelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelNumber;
    public int levelOpenPlant;
    public int[] zombieQuantity;
    public int[] wavesPoints;
    public float LevelTime;
}
