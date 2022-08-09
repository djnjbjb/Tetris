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
                �� Emit����
                group�·����м�ĸ��ӣ�����emitterPosition��
                ���û���м�ĸ��ӣ����м�ƫ�ҵĸ��ӷ���emitterPosition��

                �� ϸ��
                ���ԣ�emitterPosition���±߽��0.5f���ɴ˿��Լ���y��
                ����x��
                    �ж�bounds.size.x����������ż��
                    ������������ұ߽���emitterPosition + extents.x,
                    �����ż����emitterPosition + extents.x - 0.5,
                    �ɴ˼���x��
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