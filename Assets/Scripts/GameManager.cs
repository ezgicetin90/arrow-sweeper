using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CubeBehavior currentCube;

    private float currentTimeLeft;
    private bool timerRunning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Update()
    {
        if (!timerRunning) return;

        currentTimeLeft -= Time.deltaTime;

        // Optional: Add a UI bar update here

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

        // 🧹 Clean up old cubes
        foreach (var cube in FindObjectsOfType<CubeBehavior>())
        {
            Destroy(cube.gameObject);
        }

        // Reset the camera follower
        CameraFollower follower = Camera.main.GetComponent<CameraFollower>();
        if (follower != null)
        {
            follower.StopFollowing();
        }

        // Load the level
        LevelManager.Instance.LoadLevel(PlayerPrefs.GetInt("CurrentLevel", 0));

        // Restart timer
        LevelData data = LevelManager.Instance.GetCurrentLevelData();
        currentTimeLeft = data.timeLimitSeconds;
        timerRunning = true;

        Debug.Log("⏱️ Timer reset to: " + currentTimeLeft + " seconds.");
    }

    public void SetCurrentCube(CubeBehavior cube)
    {
        currentCube = cube;

        // Move the camera to the top cube's Y
        if (Camera.main != null)
        {
            Vector3 camPos = Camera.main.transform.position;
            camPos.y = currentCube.transform.position.y;
            Camera.main.transform.position = camPos;
        }

        // Smooth follow
        CameraFollower follower = Camera.main.GetComponent<CameraFollower>();
        if (follower != null)
        {
            follower.SetTarget(currentCube.transform);
        }

    }

    public bool IsCubeActive(CubeBehavior cube)
    {
        return cube == currentCube;
    }

    public void AdvanceToNextCube(CubeBehavior nextCube)
    {
        currentCube = nextCube;
    }

    public void CubeCleared(CubeBehavior clearedCube)
    {
        // Stop timer if stack is cleared
        CubeBehavior[] remainingCubes = FindObjectsOfType<CubeBehavior>();

        if (remainingCubes.Length <= 1) // Last cube was just cleared
        {
            Debug.Log("Stack cleared!");
            timerRunning = false;

            Camera.main.GetComponent<CameraFollower>()?.StopFollowing();

            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null)
            {
                ui.ShowLevelComplete();
            }
        }
        else
        {
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
}
