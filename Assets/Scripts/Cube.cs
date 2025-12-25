using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer), typeof(ColorRandomizer))]
public class Cube : MonoBehaviour
{
    private ColorRandomizer _colorRandomizer;
    private bool _hasTouched = false;
    private Renderer _renderer;

    public event Action<Cube> CubeReleasing;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _colorRandomizer = GetComponent<ColorRandomizer>();
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;
    }

    public void ResetTouch()
    {
        _hasTouched = false;
        _renderer.material.color = _colorRandomizer.GetBaseColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform _platform) && _hasTouched == false)
        {
            _hasTouched = true;
            _renderer.material.color = _colorRandomizer.GetRandomColor();
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
