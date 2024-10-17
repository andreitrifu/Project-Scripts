using UnityEngine;//
public class ScaleAndAnchorController : MonoBehaviour
{
    private static bool isScaled = false;

    public Vector3 initialScale;
    public Vector2 initialAnchorMin;
    public Vector2 initialAnchorMax;
    public Vector2 initialPosition;

    private void Start()
    {
        initialScale = transform.localScale;
        initialAnchorMin = ((RectTransform)transform).anchorMin;
        initialAnchorMax = ((RectTransform)transform).anchorMax;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleScaleAndAnchor();
        }
    }

    private void ToggleScaleAndAnchor()
    {
        if (isScaled)
        {
            transform.localScale = initialScale;
            ((RectTransform)transform).anchorMin = initialAnchorMin;
            ((RectTransform)transform).anchorMax = initialAnchorMax;
            transform.localPosition = initialPosition;
        }
        else
        {
            transform.localScale = new Vector3(5f, 5f, 1f);
            ((RectTransform)transform).anchorMin = new Vector2(0.5f, 0.5f);
            ((RectTransform)transform).anchorMax = new Vector2(0.5f, 0.5f);
            transform.localPosition = Vector3.zero;
        }

        isScaled = !isScaled;
    }
}
