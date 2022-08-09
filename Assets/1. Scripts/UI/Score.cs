using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LudoTetris.UI
{
    public class Score : MonoBehaviour
    {
        TMP_Text tmpText;
        int score = 0;

        #region Unity Message
        private void Awake()
        {
            tmpText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            tmpText.text = score.ToString();
        }
        #endregion

        #region Public Method
        public void AddScore(int score)
        {
            this.score += score;
        }
        #endregion
    }
}