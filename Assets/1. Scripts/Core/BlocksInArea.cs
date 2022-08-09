using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LudoTetris.Core.Utility;

namespace LudoTetris.Core
{
    public class BlocksInArea : MonoBehaviour
    {
        #region Singleton
        public static BlocksInArea singleton;
        void SetSingleton()
        {
            singleton = this;
        }
        #endregion

        #region Fields
        //Const
        Vector2 leftBottomCorner = new Vector2(-4.5f, 0.5f);
        const int WIDTH = 10;
        const int HEIGHT = 22;

        //Setting
        [SerializeField] Transform blocksParent;
        [SerializeField] LudoTetris.UI.Score scoreUI;

        //Runtime
        Block.Block[,] blocks = new Block.Block[WIDTH, HEIGHT];
        #endregion

        #region Unity Message
        void Awake()
        {
            SetSingleton();
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    blocks[i, j] = null;
                }
            }
        }

        void Update()
        {

        }
        #endregion

        #region Public Method
        public (int x, int y) PositionToIndex(Block.Block block)
        {
            return PositionToIndex(block.transform.position);
        }

        public (int x, int y) PositionToIndex(Vector2 position)
        {
            int x = Mathf.RoundToInt(position.x - leftBottomCorner.x);
            int y = Mathf.RoundToInt(position.y - leftBottomCorner.y);
            return (x, y);
        }

        public Vector2 IndexToPosition(int x, int y)
        {
            float vectorX = x + leftBottomCorner.x;
            float vectory = y + leftBottomCorner.y;

            return new Vector2(vectorX, vectory);
        }

        public bool IsAlreadyOccupiedByBlock(int x, int y)
        {
            if (x < 0 || x >= WIDTH || y < 0 || y >= HEIGHT)
            {
                return false;
            }
            return blocks[x, y] != null;
        }
        public bool IsAlreadyOccupiedByBlock(Vector2 positon)
        {
            (int x, int y) = PositionToIndex(positon);
            return IsAlreadyOccupiedByBlock(x, y);
        }

        public Utility.OutOfRange IsOutOfRange(Vector2 position)
        {
            (int x, int y) = PositionToIndex(position);
            return IsOutOfRange(x, y);
        }

        public Utility.OutOfRange IsOutOfRange(int x, int y)
        {
            bool outOfLeft = false;
            bool outOfRight = false;
            bool outOfBottom = false;
            bool outOfTop = false;

            if (x < 0)
            {
                outOfLeft = true;
            }
            if (x >= WIDTH)
            {
                outOfRight = true;
            }
            if (y < 0)
            {
                outOfBottom = true;
            }
            if (x < 0)
            {
                outOfTop = true;
            }

            return new OutOfRange(outOfLeft, outOfRight, outOfBottom, outOfTop);
        }


        public bool IsValidSlot(Vector2 position)
        {
            (int x, int y) = PositionToIndex(position);
            if (x < 0 || x >= WIDTH || y < 0 || y >= HEIGHT)
            {
                return false;
            }
            return !IsAlreadyOccupiedByBlock(x, y);
        }

        public bool IsValidSlot(int x, int y)
        {
            if (x < 0 || x >= WIDTH || y < 0 || y >= HEIGHT)
            {
                return false;
            }
            return !IsAlreadyOccupiedByBlock(x, y);
        }

        public void AddBlocksAndTryClear(Block.Block[] blocks)
        {
            Sound.SoundEffect.singleton.PlaySmallSuccess();
            foreach(var b in blocks)
            {
                AddBlock(b);
            }
            TryClear();
        }
        #endregion

        #region Private Method
        void AddBlock(Block.Block block)
        {
            block.transform.parent = blocksParent;
            (int x, int y) = PositionToIndex(block);

            if (x < 0 || x >= WIDTH || y < 0 || y >= HEIGHT)
            {
                throw new System.Exception("Invalid x,y");
            }
            if (blocks[x, y] != null)
            {
                throw new System.Exception("blocks[x,y] already there.");
            }

            blocks[x, y] = block;
            BlockPositonFit(block);
        }

        void BlockPositonFit(Block.Block block)
        {
            (int x, int y) = PositionToIndex(block.transform.position);
            block.transform.position = IndexToPosition(x, y);
        }

        void BlockPositonFit(int x, int y)
        {
            Block.Block block = blocks[x, y];
            if (block == null)
            {
                throw new System.Exception("Can not find block.");
            }
            BlockPositonFit(block);
        }

        void TryClear()
        {
            List<int> linesToClear = new List<int>();
            for(int h = 0; h < HEIGHT; h++)
            {
                int count = 0;

                for(int w = 0; w < WIDTH; w++)
                {
                    if (blocks[w, h] != null) count++;
                }

                if (count == WIDTH) linesToClear.Add(h);
            }

            //执行消除
            {
                /*
                    先全部消除
                    然后，再对每一个linesToClear操作，把line以上的行下移一个
                */

                //全部消除
                foreach (var lineCount in linesToClear)
                {
                    for (int w = 0; w < WIDTH; w++)
                    {
                        var b = blocks[w, lineCount];
                        Destroy(b.gameObject);
                        blocks[w, lineCount] = null;
                    }
                }
                //下移
                var linesToClearDecending = linesToClear.OrderByDescending(x => x).ToList();
                foreach (var lineCount in linesToClearDecending)
                {
                    for (int h = lineCount+1; h < HEIGHT; h++)
                    {
                        for (int w = 0; w < WIDTH; w++)
                        {
                            var b = blocks[w, h];
                            if (b != null)
                            {
                                blocks[w, h - 1] = blocks[w, h];
                                blocks[w, h] = null;
                                b.transform.position += Vector3.down;
                            }
                        }
                    }
                }
            }

            //算分
            if (linesToClear.Count > 0)
            {
                int score = Setting.singleton.scoreByLineCleared[linesToClear.Count];
                scoreUI.AddScore(score);
                Sound.SoundEffect.singleton.PlayBigSuccess();
            }
            
            
        }
        #endregion
    }
}