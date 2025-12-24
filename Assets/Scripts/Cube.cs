using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private bool _hasTouched = false;
    private Func<Color> _getRandomColor;

    public event Action<Cube> CubeReleasing;

    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }

    public void Initialize(Vector3 position, Func<Color> getRandomColor)
    {
        transform.position = position;
        _getRandomColor = getRandomColor;
    }

    public void ResetTouch()
    {
        _hasTouched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform _platform) && _hasTouched == false)
        {
            _hasTouched = true;
            Renderer.material.color = _getRandomColor();
            StartCoroutine(StartReleaseDelay());
        }
    }

    private IEnumerator StartReleaseDelay()
    {
        yield return GetReleasingTimeDelay();
        CubeReleasing?.Invoke(this);
    }

    private WaitForSecondsRealtime GetReleasingTimeDelay()
    {
        float minLifeSeconds = 2f;
        float maxLifeSeconds = 5f;
        return new WaitForSecondsRealtime(UnityEngine.Random.Range(minLifeSeconds, maxLifeSeconds));
    }
}
