using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoTetris
{
    public class Setting : MonoBehaviour
    {
        #region Singleton
        public static Setting singleton;
        void SetSingleton()
        {
            singleton = this;
        }
        #endregion

        [SerializeField] float timeStep;
        public Dictionary<int, int> scoreByLineCleared = new Dictionary<int, int>
        {
            { 1, 100},
            { 2, 200},
            { 3, 400},
            { 4, 800},
        };
        private void Awake()
        {
            SetSingleton();
            Time.fixedDeltaTime = timeStep;
        }
    }
}