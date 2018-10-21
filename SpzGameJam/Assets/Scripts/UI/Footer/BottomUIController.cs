using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.UI
{
    public class BottomUIController : MonoBehaviour
    {
        //-----------------------------------------
        // 定数関連
        //-----------------------------------------
        #region ===== CONSTS =====
        public class InitParam
        {
        }

        const int BUTTON_COUNT = 4;
        #endregion //) ===== CONSTS =====

        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VARIABLES =====
        [SerializeField, Tooltip("ボタンUI")]
        GameObject m_ButtonUIPrefab = null;
        GameObject ButtonUIPrefab{get{return m_ButtonUIPrefab;}}

        [SerializeField, Tooltip("ボタンUIRoot")]
        Transform m_ButtonUIRoot = null;
        Transform ButtonUIRoot{get{return m_ButtonUIRoot;}}


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

            if( !CreateButtons() )
            {
                Debug.LogError("{Failed Init Buttons");
                return false;
            }

            return true;
        }

        private bool CreateButtons()
        {
            Debug.Assert( ButtonUIPrefab != null );
            Debug.Assert( ButtonUIRoot != null );
            if( ButtonUIPrefab == null || ButtonUIRoot == null )
            {
                return false;
            }


            for (int i = 0; i < BUTTON_COUNT; i++)
            {
                var go = Instantiate( ButtonUIPrefab, ButtonUIRoot) as GameObject;
                if( go == null )
                {
                    continue;
                }

                var ctrl = go.GetComponentInChildren<NormalButtonController>();
                if( ctrl == null )
                {
                    continue;
                }

                var p = new NormalButtonController.InitParam();
                p.ItemIndex = i;
                p.OnButtonClicked = OnButtonClicked;

                ctrl.Init( p );
            }

            return true;
        }
        #endregion //) ===== INITIALIZE =====


        //-----------------------------------------
        // BUTTON_CALLBACK
        //-----------------------------------------
        #region ===== BUTTON_CALLBACK =====


        public void OnButtonClicked( int _index )
        {
            GameManager.instance?.SetCurrentIndex( _index );
        }

        #endregion //) ===== BUTTON_CALLBACK =====
    }

}
