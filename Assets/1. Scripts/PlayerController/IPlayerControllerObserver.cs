using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public interface IPlayerControllerObserver
    {
        public void RespondToAction(Action action);
    }
}