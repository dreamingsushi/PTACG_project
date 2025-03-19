using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public PlayerControllerPlus playerController;
    public void MagicAttack()
    {
        playerController.Shoot();
    }

    public void SupportAttack()
    {
        playerController.Shoot();
    }
}
