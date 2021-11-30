using UnityEngine;

public class PlayerMovingStateModel : BasePlayerStateModel
{
    private Vector3 _movingVector = Vector3.zero;
    private Vector2 _movingVector2D;
    private float _magnitude;
    private Quaternion _rotationTemp;
    private Vector3 _translatePositionTemp;
    private float _vectorSpeedMagnitude;


    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        FindLand(player);

        _movingVector2D = controller.PositionDelta - controller.PositionBegan;

        _magnitude = _movingVector2D.magnitude;

        if (_magnitude > 100)
        {
            _magnitude = 100.0f;
        }

        _vectorSpeedMagnitude = _magnitude * 0.01f;

        _movingVector.x = _movingVector2D.x;
        _movingVector.z = _movingVector2D.y;
        _movingVector.y = 0;

        _rotationTemp = Quaternion.LookRotation(_movingVector, Vector3.up);
        player.transform.rotation = _rotationTemp;

        _translatePositionTemp = Vector3.forward * _vectorSpeedMagnitude * player.MovementSpeed * Time.deltaTime;
        player.transform.Translate(_translatePositionTemp);

        if (player.IsWall)
        {
            player.transform.Translate(_translatePositionTemp * -1.0f);
        }

        player.SetMovingBlend(_vectorSpeedMagnitude);
        //GameEvents.Current.MoveConnectedEnemy(_rotationTemp, _translatePositionTemp, _magnitude);
    }
}