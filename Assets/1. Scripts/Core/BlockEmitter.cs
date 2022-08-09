using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoTetris.Core
{
    public class BlockEmitter : MonoBehaviour
    {
        [SerializeField] Transform emitterPositionTransform;
        Vector3 emitterPosition;
        [SerializeField] Transform blockGroupParent;
        [SerializeField] GameObject gameOver;

        #region Unity Message
        void Awake()
        {
            emitterPosition = emitterPositionTransform.position;
        }

        void FixedUpdate()
        {
            if (gameOver.activeSelf == true)
            {
                return;
            }
            if (blockGroupParent.childCount == 0)
            {
                EmitAndGameOver();
            }
        }
        #endregion

        #region Private Method
        void EmitAndGameOver()
        {
            var group = Emit();
            if (group.IsOverlapedWithBlocks())
            {
                gameOver.SetActive(true);
                Destroy(group.gameObject);
                Sound.SoundEffect.singleton.PlayFailure();
            }
        }

        Block.BlockGroup Emit()
        {
            var group = Block.BlockGroup.CreateRandom();
            group.transform.parent = blockGroupParent;

            /*
                ● Emit规则
                group下方，中间的格子，放在emitterPosition。
                如果没有中间的格子，则中间偏右的格子放在emitterPosition。

                ● 细化
                所以，emitterPosition比下边界高0.5f，由此可以计算y。
                对于x：
                    判断bounds.size.x是奇数还是偶数
                    如果是奇数，右边界是emitterPosition + extents.x,
                    如果是偶数，emitterPosition + extents.x - 0.5,
                    由此计算x。
            */
            Bounds bounds = group.GetRelativeBounds();
            float y_Pivot;
            {
                float y_FromPivotToLowerBound = bounds.center.y - bounds.extents.y;
                float y_FromLowerBoundToEmitter = 0.5f;
                float y_FromPivotToEmitter = y_FromPivotToLowerBound + y_FromLowerBoundToEmitter;
                y_Pivot = emitterPosition.y - y_FromPivotToEmitter;
            }
            float x_Pivot;
            {
                float x_FromPivotToRightBound = bounds.center.x + bounds.extents.x;

                float x_FromRightBoundToEmitter;
                {
                    if (Mathf.RoundToInt(bounds.size.x) % 2 == 1)
                    {
                        x_FromRightBoundToEmitter = -bounds.extents.x;
                    }
                    else
                    {
                        x_FromRightBoundToEmitter = -bounds.extents.x + 0.5f;
                    }
                }
                float x_FromPivotToEmtter = x_FromPivotToRightBound + x_FromRightBoundToEmitter;
                x_Pivot = emitterPosition.x - x_FromPivotToEmtter;
            }

            group.transform.position = new Vector2(x_Pivot, y_Pivot);
            return group;
        }
        #endregion
    }
}