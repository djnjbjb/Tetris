using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoTetris.Core.Block
{
    [CreateAssetMenu(fileName = "BlockGroupTemplates", menuName = "ScriptableObjects/BlockGroupTemplates", order = 1)]
    public class BlockGroupTemplates : ScriptableObject
    {
        public BlockGroup[] templates;
    }
}