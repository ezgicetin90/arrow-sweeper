using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    public string[] expectedInputs;
    public GameObject visualsContainer; // assign this in the Inspector
    private int currentInputIndex = 0;
    private bool isCompleted = false;

    private Camera cam;

    private void OnEnable()
    {
        InputManager.OnInputDetected += CheckInput;
        cam = Camera.main;
    }

    private void OnDisable()
    {
        InputManager.OnInputDetected -= CheckInput;
    }

    void CheckInput(string input)
    {
        if (isCompleted) return;
        if (!GameManager.Instance.IsCubeActive(this)) return;
        if (!IsInputOverThisCube()) return;

        if (input == expectedInputs[currentInputIndex])
        {
            currentInputIndex++;

            if (currentInputIndex >= expectedInputs.Length)
            {
                CompleteCube();
            }
        }
        else
        {
            Debug.Log($"Wrong input for {gameObject.name}: expected {expectedInputs[currentInputIndex]}, got {input}");
        }
    }

    bool IsInputOverThisCube()
    {
        Vector3 inputPos = Input.mousePosition;
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            inputPos = Input.GetTouch(0).position;
#endif

        Ray ray = cam.ScreenPointToRay(inputPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject == this.gameObject;
        }

        return false;
    }

    void CompleteCube()
    {
        isCompleted = true;
        Debug.Log($"{gameObject.name} completed!");
        Destroy(gameObject);

        GameManager.Instance.CubeCleared(this); // Let GameManager decide what happens next
    }

    public void GenerateVisuals(Color spriteColor)
    {
        foreach (Transform child in visualsContainer.transform)
        {
            Destroy(child.gameObject); // Clear old icons
        }

        foreach (string input in expectedInputs)
        {
            GameObject icon = new GameObject("InputIcon");
            icon.transform.SetParent(visualsContainer.transform);
            icon.transform.localPosition = new Vector3(0, 0.56f, -0.1f); // bring forward
            icon.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            icon.transform.localEulerAngles = new Vector3(90, 0, 0);

            SpriteRenderer sr = icon.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("Sprites/" + GetSpriteName(input));
            sr.sortingLayerName = "Overlay";
            sr.sortingOrder = 500;
            spriteColor.a = 1f; // force visible
            sr.color = spriteColor;

            if (sr.sprite == null)
                Debug.LogWarning("Sprite failed to load for input: " + input);
        }
    }

    string GetSpriteName(string input)
    {
        switch (input)
        {
            case "Up": return "arrow_up";
            case "Down": return "arrow_down";
            case "Left": return "arrow_left";
            case "Right": return "arrow_right";
            case "Tap": return "tap_filled";
            case "Hold": return "tap_hold";
            default: return "";
        }
    }
}
