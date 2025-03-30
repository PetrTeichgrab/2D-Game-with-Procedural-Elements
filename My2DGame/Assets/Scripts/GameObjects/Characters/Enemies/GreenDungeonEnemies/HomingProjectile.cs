using UnityEngine;

public class HomingProjectile : EnemyShoot
{
    private void Update()
    {
        ShootAndFollowPlayer();
    }

}
