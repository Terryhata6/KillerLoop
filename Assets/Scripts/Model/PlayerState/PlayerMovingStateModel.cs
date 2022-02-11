using UnityEngine;

public class PlayerMovingStateModel : BasePlayerStateModel
{
    private Vector3 _movingVector = Vector3.zero;
    private Vector2 _movingVector2D;
    private float _magnitude;

    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        if (controller.PositionDelta - controller.PositionBegan == Vector2.zero)
        {
            player.Stand();
            return;
        }

        _movingVector2D = controller.PositionDelta - controller.PositionBegan;
        CalculateMovingVector3d(_movingVector2D, out _movingVector, out _magnitude);

        player.Rotate(Quaternion.LookRotation(_movingVector, Vector3.up));
        player.Move(Vector3.forward * _magnitude * Time.deltaTime);
        player.SetMovingBlend(_magnitude);
        
        CheckToJump(player);
    }

    private void CalculateMovingVector3d(Vector2 vector2d,out Vector3 movingVector3d, out float magnitude)
    {
        magnitude = vector2d.magnitude;
        if (magnitude > 100)
        {
            magnitude = 100.0f;
        }
        magnitude *= 0.01f;
        
        movingVector3d.x = vector2d.x;
        movingVector3d.z = vector2d.y;
        movingVector3d.y = 0;
    }
    
    private void CheckToJump(PlayerView player)
    {
        if (!player.RayCastCheck(player.Position + Vector3.up * 1.5f, player.Forward * 2f + Vector3.down * 4f, 3f, (1 << 11)|(1<<12))
        || player.RayCastCheck(player.Position + Vector3.up * 1.5f, player.Forward * 1f, 1.5f , 1 << 11))
        {
            Debug.Log("jump");
            player.StopRun();
            player.Jump();
        }
    }
}