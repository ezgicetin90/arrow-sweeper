using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CubeBehavior currentCube;

    private float currentTimeLeft;
    private bool timerRunning = false;
    private bool cameraFrozen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Update()
    {
        if (!timerRunning) return;

        currentTimeLeft -= Time.deltaTime;

        if (currentTimeLeft <= 0)
        {
            timerRunning = false;
            Debug.Log("⏰ Time’s up!");
            UIManager ui = FindObjectOfType<UIManager>();
            ui?.ShowLevelFail();
        }
    }

    public void StartLevel()
    {
        Debug.Log("🌑 Starting level from GameManager...");

        foreach (var cube in FindObjectsOfType<CubeBehavior>())
        {
            Destroy(cube.gameObject);
        }

        // Reset frozen state
        cameraFrozen = false;

        // Load the level
        LevelManager.Instance.LoadLevel(PlayerPrefs.GetInt("CurrentLevel", 0));

        LevelData data = LevelManager.Instance.GetCurrentLevelData();
        currentTimeLeft = data.timeLimitSeconds;
        timerRunning = true;

        Debug.Log("⏱️ Timer reset to: " + currentTimeLeft + " seconds.");
    }

    public void SetCurrentCube(CubeBehavior cube)
    {
        currentCube = cube;

        if (!cameraFrozen && Camera.main != null && currentCube != null)
        {
            CameraFollower follower = Camera.main.GetComponent<CameraFollower>();
            if (follower != null)
            {
                follower.SetTarget(currentCube.transform);
            }
        }
    }

    public bool IsCubeActive(CubeBehavior cube)
    {
        return cube == currentCube;
    }

    public void AdvanceToNextCube(CubeBehavior nextCube)
    {
        currentCube = nextCube;

        if (!cameraFrozen && Camera.main != null && currentCube != null)
        {
            CameraFollower follower = Camera.main.GetComponent<CameraFollower>();
            if (follower != null)
            {
                follower.SetTarget(currentCube.transform);
            }
        }
    }

    public void CubeCleared(CubeBehavior clearedCube)
    {
        CubeBehavior[] remainingCubes = FindObjectsOfType<CubeBehavior>();

        if (remainingCubes.Length <= 1)
        {
            Debug.Log("Stack cleared!");
            timerRunning = false;

            UIManager ui = FindObjectOfType<UIManager>();
            ui?.ShowLevelComplete();
            return;
        }

        // Stop camera follow when 5 or fewer cubes remain
        if (remainingCubes.Length <= 10 && !cameraFrozen)
        {
            CameraFollower follower = Camera.main.GetComponent<CameraFollower>();
            if (follower != null)
            {
                follower.SetTarget(null);
            }

            cameraFrozen = true;
            Debug.Log("🛑 Camera stopped following at 10 cubes.");
        }

        // Find next cube to focus on
        CubeBehavior next = null;
        float closestY = float.MinValue;

        foreach (var cube in remainingCubes)
        {
            if (cube == clearedCube) continue;
            float diff = cube.transform.position.y - clearedCube.transform.position.y;

            if (diff < 0 && diff > closestY)
            {
                closestY = diff;
                next = cube;
            }
        }

        if (next != null)
        {
            AdvanceToNextCube(next);
        }
    }
}
