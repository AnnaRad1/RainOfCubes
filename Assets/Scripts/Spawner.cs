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
            createFunc: () => CreateNewCube(),
            actionOnGet: (obj) => ActivateCube(obj),
            actionOnRelease: (obj) => ReleaseCube(obj),
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

    private Cube CreateNewCube()
    {
        Cube newCube = Instantiate(_prefab, _parent);
        newCube.Initialize(GetSpawnPosition(), _colorRandomizer.GetRandomColor);
        newCube.CubeReleasing += ReturnCubeToPool;
        return newCube;
    }

    private void ActivateCube(Cube cube)
    {
        cube.Renderer.material.color = _colorRandomizer.GetBaseColor();
        cube.transform.position = GetSpawnPosition();
        cube.ResetTouch();
        cube.gameObject.SetActive(true);
    }

    private void ReleaseCube(Cube cube)
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
        while (enabled)
        {
            _pool.Get();
            yield return _waitGetting;
        }
    }

    private void DestroyCube(Cube destroyingCube)
    {
        destroyingCube.CubeReleasing -= ReturnCubeToPool;
        Object.Destroy(destroyingCube);
    }
}
