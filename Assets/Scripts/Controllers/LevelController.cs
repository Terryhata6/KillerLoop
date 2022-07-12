using System.Collections.Generic;
using UnityEngine;
public class LevelController : BaseController, IProgressValuesUpdater, IExecute, IContainServices
{
    private float time;
    private ProgressValue[] _values;
    private List<IServiceConsumer<IService>> _consumers;
    public ProgressValue[] ProgressValues => _values;

    public void AddConsumer(IServiceConsumer<IService> consumer) 
    {
        if (_consumers == null)
        {
            _consumers = new List<IServiceConsumer<IService>>();
        }
        if (!_consumers.Contains(consumer))
        {
            _consumers.Add(consumer);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        _values = new ProgressValue[4];
        ProgressValue value = new ProgressValue();
        value.ValueType = ProgressValueType.EnemyBeaten;
        _values[0] = value;
        ProgressValue valuee = new ProgressValue();
        valuee.ValueType = ProgressValueType.TargetDistance;
        _values[1] = valuee;
    }

    public void Execute()
    {
        time += Time.deltaTime;
        if (time >= 1f)
        {
            time = 0f;
            UpdateValues();
        }
    }
    private void UpdateValues()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            if (_values[i].ValueType == ProgressValueType.TargetDistance)
            {
                _values[i].Value += 0.1f;
            }
            if (_values[i].ValueType == ProgressValueType.EnemyBeaten)
            {
                _values[i].Value += 1f; 
            }
        }
        for (int i = 0; i < _consumers.Count; i++)
        {
            _consumers[i].UseService(this);
        }
    }

    public List<IService> GetServices()
    {
        List<IService> services = new List<IService>();
        services.Add(this);
        return services;
    }
}