using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private Platform _basePlatform;
    [SerializeField] private PositionCalculator _positionCalculator;
    [SerializeField] private ColorRandomizer _colorRandomizer;
    [SerializeField] private int _poolCapacity = 100;
    [SerializeField] private int _maxPoolSize = 100;
    [SerializeField] private float _delay = 0.5f;

    private ObjectPool<Cube> _pool;
    private WaitForSecondsRealtime _waitGetting;

    private void Awake()
    {
        _waitGetting = new WaitForSecondsRealtime(_delay);
        
        _pool = new ObjectPool<Cube>
        (
            createFunc: () => GetNewCube(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => DestroyCube(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _maxPoolSize
        );
    }

    private void Start()
    {
        StartCoroutine(GetCubeFromPool());
    }

    private Cube GetNewCube()
    {
        Cube newCube = Instantiate(_prefab, _parent);
        Color newColor = _colorRandomizer.GetRandomColor();
        newCube.Initialize(GetSpawnPosition());
        newCube.PlatformTouched += ChangeColor;
        newCube.CubeReleasing += ReturnCubeToPool;
        return newCube;
    }

    private void ActionOnGet(Cube cube)
    {
        cube.Renderer.material.color = _colorRandomizer.GetBaseColor();
        cube.transform.position = GetSpawnPosition();
        cube.ResetTouch();
        cube.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private Vector3 GetSpawnPosition()
    {
        return _positionCalculator.GetRandomPosition(_basePlatform.Renderer.bounds);
    }

    private void ReturnCubeToPool(Cube returningCube)
    {
        _pool.Release(returningCube);
    }

    private IEnumerator GetCubeFromPool()
    {
        while (true)
        {
            _pool.Get();
            yield return _waitGetting;
        }
    }

    private void DestroyCube(Cube destroyingCube)
    {
        destroyingCube.PlatformTouched -= ChangeColor;
        destroyingCube.CubeReleasing -= ReturnCubeToPool;
        Destroy(destroyingCube);
    }

    private void ChangeColor(Cube touchedCube)
    {
        touchedCube.Renderer.material.color = _colorRandomizer.GetRandomColor();
    }
}
