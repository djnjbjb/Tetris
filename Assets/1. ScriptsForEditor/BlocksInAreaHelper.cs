using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using LudoTetris.Core;
using LudoTetris.Core.Block;

public class BlocksInAreaHelper : MonoBehaviour
{
    [System.Serializable]
    public struct XY
    {
        public int x;
        public int y;
    }


    [SerializeField] XY xyTest;
    [Button("AddBlock")]
    void AddBlock()
    {
        Block blockTemplate = Resources.Load<Block>("Block");
        Block b = Instantiate(blockTemplate);
        b.transform.position = BlocksInArea.singleton.IndexToPosition(xyTest.x, xyTest.y);

        Block[] blocks = new Block[1] { b };

        BlocksInArea.singleton.AddBlocksAndTryClear(blocks);
    }

    [SerializeField] XY[] xys;
    [Button("AddBlocks")]
    void AddBlocks()
    {
        Block[] blocks = new Block[xys.Length];

        int index = 0;
        foreach (var xy in xys)
        {
            Block blockTemplate = Resources.Load<Block>("Block");
            Block b =  Instantiate(blockTemplate);
            b.transform.position = BlocksInArea.singleton.IndexToPosition(xy.x, xy.y);
            blocks[index] = b;
            index++;
        }

        BlocksInArea.singleton.AddBlocksAndTryClear(blocks);
    }


}
