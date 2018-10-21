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

            SetText( 0 );

            return true;
        }

        #endregion //) ===== INITIALIZE =====


        //-----------------------------------------
        // public メソッド
        //-----------------------------------------
        #region ===== PUBLIC_APIS =====


        public void SetText( int _value )
        {
            if( _value < 0 )
            {
                return;
            }

            ScoreText.text = _value.ToString();
        }
        #endregion //) ===== PUBLIC_APIS =====
    }

}
