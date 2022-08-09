using UnityEngine;

namespace LudoTetris.Core.Block
{
    public class BlockGroup : MonoBehaviour, PlayerController.IPlayerControllerObserver
    {
        #region Factory
        public static BlockGroup CreateRandom()
        {
            BlockGroupTemplates templates =
                Resources.Load<BlockGroupTemplates>("BlockGroupTemplates");
            int r = Random.Range(0, templates.templates.Length);
            //int r = Random.Range(0, 1);r = r == 0? 0:5; 
            BlockGroup t = templates.templates[r];
            var bg = Instantiate(t);
            return bg; 
        }
        #endregion

        Block[] blocks;
        Bounds relativeBounds;
        RotationSetting rotationSetting; //可能为null

        #region Unity Mesage
        void Awake()
        {
            blocks = transform.GetComponentsInChildren<Block>();
            rotationSetting = GetComponent<RotationSetting>();
            CalculateRelativeBounds();
        }

        void Start()
        {
            PlayerController.IPlayerControllerObserver ob = this;
            PlayerController.PlayerController.singleton.Register(ob);
        }

        void FixedUpdate()
        {
            TryMoveDownOrDismiss();
        }

        void OnDestroy()
        {
            PlayerController.PlayerController.singleton.UnRegister(this);
        }
        #endregion

        #region Interface
        public void RespondToAction(PlayerController.Action action)
        {
            Sound.SoundEffect.singleton.PlayAction();
            if (action == PlayerController.Action.Rotate)
            {
                Rotate();
            }

            if (action == PlayerController.Action.Left)
            {
                TryMoveLeft();
            }

            if (action == PlayerController.Action.Right)
            {
                TryMoveRight();
            }

            if (action == PlayerController.Action.Down)
            {
                TryMoveDownOrDismiss();
            }
        }
        #endregion

        #region Public Method
        public Bounds GetBounds()
        {
            Bounds b = new Bounds();
            b.size = relativeBounds.size;
            b.center = relativeBounds.center + transform.position;
            return b;
        }

        public Bounds GetRelativeBounds()
        {
            return relativeBounds;
        }

        public bool IsOverlapedWithBlocks()
        {
            foreach(var b in blocks)
            {
                if (BlocksInArea.singleton.IsAlreadyOccupiedByBlock(b.transform.position))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Private Method
        Vector3 GetNearestDirectionInFour(Vector3 vec)
        {
            Vector3 result;
            {
                Vector3[] fourDirections = new Vector3[]
                {
                    Vector3.up,
                    Vector3.down,
                    Vector3.left,
                    Vector3.right
                };

                float product = Vector3.Dot(vec, fourDirections[0]);
                result = fourDirections[0];
                foreach (var d in fourDirections)
                {
                    if (Vector3.Dot(vec, d) >= product)
                    {
                        product = Vector3.Dot(vec, d);
                        result = d;
                    }
                }
            }
            return result;
        }
        bool CanMove(Vector3 directionInFour)
        {
            Vector3 movement = GetNearestDirectionInFour(directionInFour);
            foreach (var b in blocks)
            {
                if (!BlocksInArea.singleton.IsValidSlot
                        (b.transform.position + movement))
                {
                    return false;
                }
            }
            return true;
        }
        void Move(Vector3 directionInFour)
        {
            Vector3 movement = GetNearestDirectionInFour(directionInFour);
            transform.position += movement;
        }
        void TryMove(Vector3 directionInFour)
        {
            if (CanMove(directionInFour))
            {
                Move(directionInFour);
            }
        }

        void TryMoveDownOrDismiss()
        {
            if (CanMove(Vector3.down))
            {
                Move(Vector3.down);
            }
            else
            {
                Dismiss();
            }
        }

        void TryMoveLeft()
        {
            TryMove(Vector3.left);
        }

        void TryMoveRight()
        {
            TryMove(Vector3.right);
        }

        void Dismiss()
        {
            BlocksInArea.singleton.AddBlocksAndTryClear(blocks);
            Destroy(this.gameObject);
        }

        void CalculateRelativeBounds()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            foreach(var b in blocks)
            {
                if (b.transform.position.x < minX)
                {
                    minX = b.transform.position.x;
                }
                if (b.transform.position.x > maxX)
                {
                    maxX = b.transform.position.x;
                }
                if (b.transform.position.y < minY)
                {
                    minY = b.transform.position.y;
                }
                if (b.transform.position.y > maxY)
                {
                    maxY = b.transform.position.y;
                }
            }


            Bounds bounds = new Bounds();
            bounds.center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
            bounds.size = new Vector3(maxX - minX + 1, maxY - minY + 1, 0);

            relativeBounds = bounds;
            relativeBounds.center -= transform.position;
        }

        void Rotate()
        {
            //两种旋转
            if (rotationSetting == null)
            {
                transform.rotation *= Quaternion.Euler(0,0,90);
            }
            else
            {
                int rotationTimes1 = rotationSetting.rotationTimes;
                int rotationTimes2 = (rotationTimes1 + 1) % 4;
                Vector3 offset1 = rotationSetting.offsets[rotationTimes1];
                Vector3 offset2 = rotationSetting.offsets[rotationTimes2];

                foreach (var b in blocks)
                {
                    b.transform.position -= offset1;
                }

                rotationSetting.rotationTimes = rotationTimes2;
                transform.rotation = Quaternion.Euler(0, 0, 90 * rotationSetting.rotationTimes);
                foreach (var b in blocks)
                {
                    b.transform.position += offset2;
                }
            }
            
            //检测边缘
            while (true)
            {
                Utility.OutOfRange allBlockOutOfRange = 
                    new Utility.OutOfRange(false, false, false, false);
                foreach (var b in blocks)
                {
                    var oneBlockOutofRange =
                        BlocksInArea.singleton.IsOutOfRange(b.transform.position);
                    allBlockOutOfRange = allBlockOutOfRange | oneBlockOutofRange;
                }

                if (allBlockOutOfRange.isAllFalseExceptTop())
                {
                    break;
                }
                else
                {
                    if (allBlockOutOfRange.outOfLeft == true)
                    {
                        transform.position -= Vector3.left;
                    }
                    if (allBlockOutOfRange.outOfRight == true)
                    {
                        transform.position -= Vector3.right;
                    }
                    if (allBlockOutOfRange.outOfBottom == true)
                    {
                        transform.position -= Vector3.down;
                    }
                }
            }

            //检测方块重叠
            while (true)
            {
                bool overlaped = false;
                foreach (var b in blocks)
                {
                    overlaped = overlaped ||
                        BlocksInArea.singleton.IsAlreadyOccupiedByBlock(b.transform.position);
                }

                if (!overlaped) break;
                else
                {
                    transform.position += Vector3.up;
                }
            }
        }
        #endregion

    }
}