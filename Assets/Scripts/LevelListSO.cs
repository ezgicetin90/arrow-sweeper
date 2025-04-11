using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "ArrowSwipe/Level List")]
public class LevelListSO : ScriptableObject
{
    public LevelData[] levels;
}