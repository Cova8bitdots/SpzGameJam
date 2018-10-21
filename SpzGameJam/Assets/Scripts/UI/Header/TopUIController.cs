using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameJam.UI
{
    public class TopUIController : MonoBehaviour
    {
        //-----------------------------------------
        // 定数関連
        //-----------------------------------------
        #region ===== CONSTS =====
        public class InitParam
        {
        }

        #endregion //) ===== CONSTS =====

        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VARIABLES =====
        [SerializeField, Tooltip("ボタンUI")]
        TextMeshProUGUI m_ScoreText = null;
        TextMeshProUGUI ScoreText{get{return m_ScoreText;}}

        int m_currentScore = 0;
        public int CurrentScore{get{return m_currentScore;}}
        #endregion //) ===== MEMBER_VARIABLES =====

        //-----------------------------------------
        // 初期化
        //-----------------------------------------
        #region ===== INITIALIZE =====

        public bool Initialize( InitParam _param )
        {
            Debug.Assert( _param != null );
            if( _param == null )
            {
                return false;
            }

            AddText( 0 );

            return true;
        }

        #endregion //) ===== INITIALIZE =====


        //-----------------------------------------
        // public メソッド
        //-----------------------------------------
        #region ===== PUBLIC_APIS =====


        public void AddText( int _value )
        {
            m_currentScore += _value;

            ScoreText.text = m_currentScore.ToString();
        }
        #endregion //) ===== PUBLIC_APIS =====
    }

}
