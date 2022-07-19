
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : Singleton<CollectableSpawner>
{

    #region PrivateFields

    private CollectableView _tempCollectable;
    private ObjectPool<CollectableView> _pool;
    private List<Transform> _tempTransforms;
    private List<CollectableView> _tempCollectables;
    private int _poolSize;

    #endregion

    public CollectableSpawner()
    {
        InitializeFields();
        Instance = this;
    }


    #region PublicMethods

    public void LoadCollectable(Vector3 position)
    {
        CollectableInit(SpawnCollectable(position));
    }

    public async void LoadCollectablesWithSpray(Vector3 position, int size, float height, float radius)
    {
        List<CollectableView> tempCollectables = GetCollectablesAtPosition(size, position);
        Task task;
        task = CollectableSpray.Instance.SprayCollectables(
            ConvertToTransform(tempCollectables),
            radius,
            height);
        await Task.WhenAll(task);
        CollectablesInit(_tempCollectables);
    }

    public async Task LoadCollectableWithDelay(Transform obj, int delay)
    {
        await Task.Delay(delay);
        CollectableInit(SpawnCollectable(obj.position));
        await Task.Yield();
    }

    public void DestroyCollectables()
    {
        _pool.CleanPool();
    }

    public void InitializePool(List<CollectableView> examples)
    {
        if (examples == null
            || examples.Count == 0
            || _pool == null)
        {
            Debug.Log("Collectable spawner disable, examples missing");
            return;
        }
        _pool.Initialize(examples, _poolSize);
        Debug.Log("Collectable spawner enable");
    }

    #endregion

    #region PrivateMethods

    private void InitializeFields()
    {
        _poolSize = 40;
        _pool = new ObjectPool<CollectableView>();
        _tempCollectables = new List<CollectableView>();
        _tempTransforms = new List<Transform>();
    }

    private CollectableView SpawnCollectable(Vector3 position)
    {
        return _pool.GetObject(position);
    }

    private void CollectableInit(CollectableView collectable)
    {
        if (!collectable)
        {
            return;
        }
        collectable.Initialize();
    }

    private void CollectablesInit(List<CollectableView> collectables)
    {
        Debug.Log("init");
        if (collectables == null)
        {
            return;
        }
        for (int i = 0; i < collectables.Count; i++)
        {
            CollectableInit(collectables[i]);
        }
    }

    private List<Transform> ConvertToTransform(List<CollectableView> list)
    {
        _tempTransforms.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            _tempTransforms.Add(list[i].transform);
        }
        return _tempTransforms;
    }

    private List<CollectableView> GetCollectablesAtPosition(int count, Vector3 basePosition)
    {
        _tempCollectables.Clear();
        for (int _index = 0; _index < count; _index++)
        {
            _tempCollectable = SpawnCollectable(basePosition);
            _tempCollectables.Add(_tempCollectable);
        }
        return _tempCollectables;
    }


    #endregion

}