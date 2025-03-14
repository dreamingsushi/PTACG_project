using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public void MagicAttack()
    {
        PlayerController.Instance.Shoot();
    }

    public void SupportAttack()
    {
        PlayerController.Instance.Shoot();
    }
}
