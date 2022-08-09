using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using LudoTetris.Core.Block;

namespace LudoTetris.Editor
{
    public class BlockHelper : MonoBehaviour
    {
        [SerializeField] Color color;

        [Button("SetColor")]
        void SetColor()
        {
            SpriteRenderer[] srs = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sr in srs)
            {
                if (sr.gameObject.name == "Inner")
                {
                    sr.color = color;
                }
            }
        }

        [Button("RenameBlockChildren")]
        void RenameBlockChildren()
        {
            Block[] blocks = transform.GetComponentsInChildren<Block>();
            foreach (var b in blocks)
            {
                b.gameObject.name = "Block";
            }
        }
    }
}