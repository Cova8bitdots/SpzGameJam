using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.UI
{
    public class IngameUIManager : UIManagerBase<IngameUIManager>
    {
        //-----------------------------------------
        // 定数関連
        //-----------------------------------------
        #region ===== CONSTS =====
        public class InitParam : InitParameter
        {

        }
        #endregion //) ===== CONSTS ===== 

        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====
        [Header("UI")]
        [SerializeField, Tooltip("TopUI")]
        TopUIController m_TopUICtrl = null;
        TopUIController TopUICtrl{get{return m_TopUICtrl;}}

        [SerializeField, Tooltip("BopUI")]
        BottomUIController m_BotUICtrl = null;
        BottomUIController BotUICtrl{get{return m_BotUICtrl;}}

        #endregion //) ===== MEMBER_VATRIABLES ===== 


        //-----------------------------------------
        // 初期化
        //-----------------------------------------
        #region ===== INITIALIZE =====


        /// <summary>
        /// Awake method to associate singleton with instance
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Initialize( new InitParam() );
        }

        public void Initialize( InitParam _param )
        {
            base.Initialize( _param );
            SMachine.Initialize(this);

            TopUICtrl?.Initialize( new TopUIController.InitParam() );
            BotUICtrl?.Initialize( new BottomUIController.InitParam() );
        }

    

        #endregion //) ===== INITIALIZE ===== 

    }
}
