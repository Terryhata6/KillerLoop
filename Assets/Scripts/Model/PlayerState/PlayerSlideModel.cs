using UnityEngine;

public class PlayerSlideModel : BasePlayerStateModel
{
    private Vector3 _rotationTarget;
    private Vector3 _tempVector;
    private Quaternion _rotation;
    private int _index;
    private Vector3 _currentPointToMove;
    private float _currentPlayerSpeed;
    private float _tempMagnitude;

    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        
        CheckToStand(player);
        CheckForNewPoint(player);

        player.Rotate(_rotation);
        player.transform.position = Vector3.MoveTowards(player.Position,_currentPointToMove, Time.deltaTime * _currentPlayerSpeed);
    }

    private void CheckForNewPoint(PlayerView player)
    {
        _tempMagnitude = (player.Position - _currentPointToMove).magnitude;

        if (_tempMagnitude <= 0.3f || _index.Equals(0))
        {
            if (_index < player.MovingPoints.Count)
            {
                _currentPointToMove = player.MovingPoints[_index];
                _currentPlayerSpeed = 8f;
                _rotationTarget = _currentPointToMove;
                _rotationTarget.y = 0f;
                _rotation = Quaternion.LookRotation(_rotationTarget);
            }
            _index++;
        }
    }

    private void CheckToStand(PlayerView player)
    {
        if (_index > player.MovingPoints.Count)
        {
            Debug.Log("endslide");
            _index = 0;
            player.EndSlide();
            player.Stand();
        }
    }
}