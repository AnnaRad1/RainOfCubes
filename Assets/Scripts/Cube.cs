using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private string _platformTag = "Platform";

    private bool _hasTouched = false;

    public event Action<Cube> CubeReleasing;
    public event Action<Cube> PlatformTouched;

    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;
    }

    public void ResetTouch()
    {
        _hasTouched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_platformTag) && _hasTouched == false)
        {
            _hasTouched = true;
            PlatformTouched?.Invoke(this);
            StartCoroutine(ReportActionWithDelay());
        }
    }

    private IEnumerator ReportActionWithDelay()
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
