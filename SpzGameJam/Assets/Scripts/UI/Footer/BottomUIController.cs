using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.UI
{
    public class ButtonSettings
    {
        public int Index;
        public Sprite ButtonImage;
    }
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

        List<NormalButtonController> m_Buttons = new List<NormalButtonController>();

        [SerializeField]
        Sprite[] buttonImages;
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
                m_Buttons.Add( ctrl );
            }

            return true;
        }
        
        /// <summary>
        /// ボタン選択肢更新
        /// </summary>
        /// <param name="_settings"></param>
        public void ResetButtons()
        {
            Shuffle<NormalButtonController>(ref m_Buttons);
            for (int i = 0; i < m_Buttons.Count; i++)
            {
                var p = new NormalButtonController.InitParam();
                p.ItemIndex = i;
                p.ButtonSprite = buttonImages[i];
                p.OnButtonClicked = OnButtonClicked;
                m_Buttons[i].Init( p );
            }
        }

        void Shuffle<T>( ref List<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + UnityEngine.Random.Range( 0, n - i );
                T temp = list[r];
                list[r] = list[i];
                list[i] = temp;
            }
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
