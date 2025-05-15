using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Stomped(Transform player)
    {
        AudioManager.instance.PlayStomp();
        animator.SetTrigger("Hit");
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        Destroy(gameObject, 1f);
        automovement.PauseMovement();
        Dead();
    }
}
