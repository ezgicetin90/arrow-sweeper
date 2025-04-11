using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Data")]
    public LevelListSO levelList;  // Drag your MasterLevelList.asset here

    private LevelData currentLevelData;
    private int currentLevelIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
    }

    public void LoadLevel(int index)
    {
        if (levelList == null || levelList.levels == null)
        {
            Debug.LogError("⚠️ Level list is not assigned!");
            return;
        }

        if (index < 0 || index >= levelList.levels.Length)
        {
            Debug.LogWarning("❌ Invalid level index: " + index);
            return;
        }

        currentLevelData = levelList.levels[index];
        currentLevelIndex = index;

        Debug.Log("✅ Loaded level: " + currentLevelData.levelName);
        ApplyLevelData(currentLevelData);
    }

    void ApplyLevelData(LevelData data)
    {
        // Set background
        Camera.main.backgroundColor = data.skyboxColor;

        // Spawn stack
        CubeSpawner spawner = FindObjectOfType<CubeSpawner>();
        if (spawner != null)
        {
            spawner.SpawnStack(data.stackHeight, data.cubeColor, data.spriteColor);
        }

        Debug.Log($"🎯 Stack Height: {data.stackHeight}");
        Debug.Log($"🎯 Cube Color: {ColorUtility.ToHtmlStringRGB(data.cubeColor)}");
        Debug.Log($"🎯 Sprite Color: {ColorUtility.ToHtmlStringRGB(data.spriteColor)}");
        Debug.Log($"🎯 Skybox Color: {ColorUtility.ToHtmlStringRGB(data.skyboxColor)}");
        Debug.Log($"🎯 Difficulty: {data.difficulty}");
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }

    public LevelData GetCurrentLevelData() => currentLevelData;
    public int GetCurrentLevelIndex() => currentLevelIndex;
}
