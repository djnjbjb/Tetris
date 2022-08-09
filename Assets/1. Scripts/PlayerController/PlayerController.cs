using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        #region Singleton
        public static PlayerController singleton;
        void SetSingleton()
        {
            singleton = this;
        }
        #endregion

        List<IPlayerControllerObserver> observers = new List<IPlayerControllerObserver>();

        #region Unity Message
        void Awake()
        {
            SetSingleton();  
        }

        void Update()
        {
            HandleInput();
        }
        #endregion

        #region Public Method
        public void Register(IPlayerControllerObserver observer)
        {
            observers.Add(observer);
        }

        public void UnRegister(IPlayerControllerObserver observer)
        {
            observers.Remove(observer);
        }
        #endregion

        #region Private Method
        void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                foreach(var ob in observers)
                {
                    ob.RespondToAction(Action.Rotate);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                foreach (var ob in observers)
                {
                    ob.RespondToAction(Action.Left);
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                foreach (var ob in observers)
                {
                    ob.RespondToAction(Action.Right);
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                foreach (var ob in observers)
                {
                    ob.RespondToAction(Action.Down);
                }
            }
        }
        #endregion
    }
}