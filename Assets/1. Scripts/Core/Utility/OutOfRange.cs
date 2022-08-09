using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoTetris.Core.Utility
{
    public struct OutOfRange
    {
        public bool outOfLeft;
        public bool outOfRight;
        public bool outOfBottom;
        public bool outOfTop;

        public OutOfRange(bool outOfLeft = false, bool outOfRight = false, bool outOfBottom = false, bool outOfTop = false)
        {
            this.outOfLeft = outOfLeft;
            this.outOfRight = outOfRight;
            this.outOfBottom = outOfBottom;
            this.outOfTop = outOfTop;
        }

        public static OutOfRange operator |(OutOfRange a, OutOfRange b)
        {
            OutOfRange result = new OutOfRange();
            result.outOfLeft = a.outOfLeft || b.outOfLeft;
            result.outOfRight = a.outOfRight || b.outOfRight;
            result.outOfBottom = a.outOfBottom || b.outOfBottom;
            result.outOfTop = a.outOfTop || b.outOfTop;
            return result;
        }

        public static OutOfRange operator &(OutOfRange a, OutOfRange b)
        {
            OutOfRange result = new OutOfRange();
            result.outOfLeft = a.outOfLeft && b.outOfLeft;
            result.outOfRight = a.outOfRight && b.outOfRight;
            result.outOfBottom = a.outOfBottom && b.outOfBottom;
            result.outOfTop = a.outOfTop && b.outOfTop;
            return result;
        }

        public bool isAllFalse()
        {
            return (!outOfLeft && !outOfRight && !outOfBottom && !outOfTop);
        }

        public bool isAllFalseExceptTop()
        {
            return (!outOfLeft && !outOfRight && !outOfBottom);
        }
    }
}