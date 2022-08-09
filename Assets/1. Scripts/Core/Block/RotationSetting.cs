using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoTetris.Core.Block
{
    public class RotationSetting : MonoBehaviour
    {
        public Vector3[] offsets = new Vector3[4];
        [System.NonSerialized] public int rotationTimes = 0;
    }
}