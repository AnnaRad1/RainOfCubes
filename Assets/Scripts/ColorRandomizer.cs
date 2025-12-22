using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    [SerializeField] private Material _baseColor;

    public Color GetRandomColor()
    {
        return Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0.7f, 0.9f), Random.Range(0.8f, 1f));
    }

    public Color GetBaseColor()
    {
        return _baseColor.color;
    }
}
