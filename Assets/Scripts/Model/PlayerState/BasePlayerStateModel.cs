using UnityEngine;
using Abstractions;

public abstract class BasePlayerStateModel : IPlayerState
{
    protected RaycastHit _hit;
    private float _footDistance = 1f;
    private Vector3 _raycastOffset = new Vector3(0, 1, 0);
    private float _rayLenght = 2f;
    private float _failVelocity = -10f;
    private float _temp;
    private Vector3 _tempVector;


    public virtual void Execute(PlayerController controller, PlayerView player)
    {
        
    }
    
    
    
    protected void FindLand(PlayerView player)
    {
        if (Physics.Raycast(player.transform.position + _raycastOffset, Vector3.down, out _hit, _rayLenght))
        {
            player.Rigidbody.isKinematic = true;
            player.transform.position += new Vector3(0, (_footDistance - _hit.distance), 0);
            //if (player.Animator.GetBool("InFalling"))
            //{
            //    player.Animator.SetBool("InFalling", false);
            //}
        }
        else
        {
            player.Rigidbody.isKinematic = false;
            //if (player.Animator.GetBool("InFalling"))
            //{
            //    player.Animator.SetBool("InFalling", true);
            //}
            if (player.Rigidbody.velocity.y <= _failVelocity)
            {
                player.LevelFail();
                Time.timeScale = 0;
            }
        }
    }
}