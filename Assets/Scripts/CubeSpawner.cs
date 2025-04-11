using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Vector3 startPosition = new Vector3(0, 0, 0);
    public float verticalSpacing = 1.1f;

    private string[][] inputOptions = new string[][]
    {
        new string[] { "Up" },
        new string[] { "Down" },
        new string[] { "Left" },
        new string[] { "Right" },
        new string[] { "Tap" },
        new string[] { "Hold" }
    };

    public void SpawnStack(int stackHeight, Color cubeColor, Color spriteColor)
    {
        CubeBehavior lastSpawnedCube = null;

        for (int i = 0; i < stackHeight; i++)
        {
            Vector3 spawnPos = startPosition + Vector3.up * verticalSpacing * i;
            GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);

            // Apply cube color
            Renderer rend = cube.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = cubeColor;
            }

            // Set expected inputs
            CubeBehavior behavior = cube.GetComponent<CubeBehavior>();
            behavior.expectedInputs = GetRandomInput();

            // Generate visuals
            behavior.GenerateVisuals(spriteColor);

            lastSpawnedCube = behavior;
        }

        GameManager.Instance.SetCurrentCube(lastSpawnedCube);
    }

    private string[] GetRandomInput()
    {
        int rand = Random.Range(0, inputOptions.Length);
        return inputOptions[rand];
    }
}