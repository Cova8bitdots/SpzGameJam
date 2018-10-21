using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameJam.UI
{
    public class TitleUI : MonoBehaviour
    {


        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====

        [SerializeField, Tooltip("ボタン")]
        NormalButtonController m_titleButton = null;

        private bool m_IsSceneLoaded = false;
        #endregion //) ===== MEMBER_VATRIABLES ===== 

        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====


        void Awake()
        {
            if( m_titleButton != null )
            {
                var p = new NormalButtonController.InitParam();
                p.OnButtonClicked = OnButtonClicked;

                m_titleButton.Init( p );
            }
        }

        #endregion //) ===== MEMBER_VATRIABLES ===== 


        //-----------------------------------------
        // ボタンコールバック
        //-----------------------------------------
        #region ===== BUTTON_CALLBACK =====

        private void OnButtonClicked( int _index )
        {
            if( m_IsSceneLoaded )
            {
                return;
            }

            m_IsSceneLoaded = true;


            SceneManager.LoadScene( "IngameScene", LoadSceneMode.Single );
        }

        #endregion //) ===== BUTTON_CALLBACK ===== 



    }
}
