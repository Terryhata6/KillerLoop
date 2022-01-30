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
        if (controller.PositionBegan == Vector2.zero)
        {
            player.SetState(PlayerState.Idle);
            return;
        }
        
        //FindLand(player);

        _movingVector2D = controller.PositionDelta - controller.PositionBegan;

        CalculateMovingVector3d(_movingVector2D, out _movingVector, out _magnitude);

        _vectorSpeedMagnitude = _magnitude * 0.01f;

        _rotationTemp = Quaternion.LookRotation(_movingVector, Vector3.up);
        _translatePositionTemp = player.Forward * _vectorSpeedMagnitude * player.MovementSpeed * Time.deltaTime;
        
        player.Rotate(_rotationTemp);
        player.Move(_translatePositionTemp);

        player.SetMovingBlend(_vectorSpeedMagnitude);
        //GameEvents.Current.MoveConnectedEnemy(_rotationTemp, _translatePositionTemp, _magnitude);
        
        CheckToJump(player);
    }

    private void CalculateMovingVector3d(Vector2 vector2d,out Vector3 movingVector3d, out float magnitude)
    {
        magnitude = vector2d.magnitude;

        if (magnitude > 100)
        {
            magnitude = 100.0f;
        }
        movingVector3d.x = vector2d.x;
        movingVector3d.z = vector2d.y;
        movingVector3d.y = 0;
        
    }
    
    private void CheckToJump(PlayerView player)
    {
        if (!player.RayCastCheck(player.Position + Vector3.up * 1.5f, player.Forward * 2f + Vector3.down * 4f, 3f, 1 << 11))
        {
            Debug.Log("jump");
            player.StopRun();
            player.Jump();
        }
        else if (player.RayCastCheck(player.Position + Vector3.up * 1.5f, player.Forward * 1f, 1.5f , 1 << 11))
        {
            player.StopRun();
            player.Jump();
            Debug.Log("jump");
        }
    }
}