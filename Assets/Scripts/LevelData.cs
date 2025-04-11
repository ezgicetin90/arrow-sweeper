using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ArrowSwipe/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public Color skyboxColor;
    public Color cubeColor;
    public Color spriteColor;
    public int stackHeight;
    public Difficulty difficulty;
    public float timeLimitSeconds = 10f;

}

public enum Difficulty
{
    SuperEasy,
    Easy,
    Medium,
    Hard,
    ExtraHard
}