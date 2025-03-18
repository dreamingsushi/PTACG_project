using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public PlayerController playerController;
    public void MagicAttack()
    {
        playerController.Shoot();
    }

    public void SupportAttack()
    {
        playerController.Shoot();
    }
}
