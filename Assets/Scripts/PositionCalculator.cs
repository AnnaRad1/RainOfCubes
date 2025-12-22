using UnityEngine;

public class PositionCalculator : MonoBehaviour
{ 
    [SerializeField] private float _hightChanger;

    public Vector3 GetRandomPosition(Bounds platformBounds)
    {
        return new Vector3
        (
            Random.Range(platformBounds.min.x, platformBounds.max.x),
            platformBounds.max.y + _hightChanger,
            Random.Range(platformBounds.min.z, platformBounds.max.z)
        );
    }
}
