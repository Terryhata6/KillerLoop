
using UnityEngine;

public class DancePlayerView : BaseObjectView 
{
    #region PrivateFields

    #region Serialized

    [SerializeField] private Animator _animator;

    #endregion

    private string _boolName;

    #endregion

    private void Awake()
    {
        _boolName = "Dance";
    }

    public void Dance()
    {
        SetAnimatorBool(_boolName + Random.Range(0, _animator.parameterCount), true);
    }

    public void MoneyFountain()
    {
        //TODO 
    }

    private void SetAnimatorBool(string name,bool value)
    {
        if (!_animator)
        {
            return;
        }
        _animator.SetBool(name,value);
    }
}