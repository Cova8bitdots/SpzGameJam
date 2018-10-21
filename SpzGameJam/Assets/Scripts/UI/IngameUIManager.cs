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


        public static readonly System.Type INIT_STATE                       = typeof(Ingame_Init);
        public static readonly System.Type DEFAULT_STATE                    = typeof(Ingame_Default);
        public static readonly System.Type RESULT_STATE                     = typeof(Ingame_Result);
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


        [SerializeField, Tooltip("リザルトUI")]
        GameObject m_ResultUIPrefab = null;
        GameObject ResultUIPrefab{get{return m_ResultUIPrefab;}}

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
            TopUICtrl?.Initialize( new TopUIController.InitParam() );
            BotUICtrl?.Initialize( new BottomUIController.InitParam() );
        }
   

        #endregion //) ===== INITIALIZE ===== 

        //-----------------------------------------
        // StateMachine 管理
        //-----------------------------------------
        #region ===== STATE_MACHINE_CONTROLL =====

        /// <summary>
        /// StateMachine 初期化メソッド
        /// </summary>
        protected override void InitMachine()
        {
            base.InitMachine();
            SMachine.Initialize(this);
            SMachine.Start(INIT_STATE);
            PushNextState(INIT_STATE);

        }
       #endregion //) ===== STATE_MACHINE_CONTROLL =====
 
    }
}
