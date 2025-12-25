using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    [Header("Random Color Settings")]
    [SerializeField] private float _minHue = 0f;
    [SerializeField] private float _maxHue = 1f;
    [SerializeField] private float _minSaturation = 0.7f;
    [SerializeField] private float _maxSaturation = 0.9f;
    [SerializeField] private float _minValue = 0.8f;
    [SerializeField] private float _maxValue = 1f;

    [Header("Base Color")]
    [SerializeField] private Material _baseColor;

    public Color GetRandomColor()
    {
        return Color.HSVToRGB(Random.Range(_minHue, _maxHue), Random.Range(_minSaturation, _maxSaturation), Random.Range(_minValue, _maxValue));
    }

    public Color GetBaseColor()
    {
        return _baseColor.color;
    }
}
