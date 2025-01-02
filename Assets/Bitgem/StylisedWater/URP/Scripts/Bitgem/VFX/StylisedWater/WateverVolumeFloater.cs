#region Using statements

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Bitgem.VFX.StylisedWater
{
    public class WateverVolumeFloater : MonoBehaviour
    {
        #region Public fields

        public WaterVolumeHelper WaterVolumeHelper = null;
        public float waterLevelYoffset;



        #endregion

        #region MonoBehaviour events

        void LateUpdate()   
        {
            var instance = WaterVolumeHelper ? WaterVolumeHelper : WaterVolumeHelper.Instance;
            if (!instance)
            {
                return;
            }
            transform.position = new Vector3(transform.position.x, (instance.GetHeight(transform.position) ?? transform.position.y) + waterLevelYoffset, transform.position.z);
        }

        #endregion
    }
}