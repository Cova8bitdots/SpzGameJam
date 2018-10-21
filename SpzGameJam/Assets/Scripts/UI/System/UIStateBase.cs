using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.UI
{
    /// <summary>
    /// UISceneState の基本クラス
    /// </summary>
    public abstract class UISceneState : SceneStateBase<IngameUIManager>
    {
        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====

        /// <summary>
        /// 制御対象以外のUIが選択されたかどうか
        /// </summary>
        private bool m_isSelectedOtherControlledUI = false;
        public bool IsSelectedOtherControlledUI { get { return m_isSelectedOtherControlledUI; } }


        #endregion //) ===== MEMBER_VATRIABLES ===== 

        //-----------------------------------------
        // 初期化
        //-----------------------------------------
        #region ===== INITIALIZE =====

        /// <summary>
        /// 初期化メソッド
        /// OnEnter から呼ぶ
        /// </summary>
        protected virtual void Initialize()
        {
            EndInitialize();
        }

        /// <summary>
        /// 初期化コルーチン
        /// </summary>
        /// <returns></returns>
    	protected virtual IEnumerator InitCoroutine()
    	{
    		yield return null;
    		// 初期化終了
    		EndInitialize();
    	}

        #endregion //) ===== INITIALIZE =====

        //-----------------------------------------
        // State操作
        //-----------------------------------------
        #region ===== STATE_METHOD =====


        /// <summary>
        /// State 開始時処理
        /// </summary>
        /// <param name="_manager"></param>
        public override void OnEnter(IngameUIManager _manager)
        {
            base.OnEnter(_manager);

            Initialize();
        }


        /// <summary>
        /// 次のStateを取得
        /// Null を返した場合StateMachine の終了
        /// State維持の場合は自信のtypeを返す
        /// </summary>
        /// <returns></returns>
        public override System.Type GetNextState()
        {
            if( m_manager == null || !IsInitializeEnd )
            {
                return base.GetNextState();
            }
            return m_manager.GetTopStateType();
        }


        #endregion //) ===== STATE_METHOD ===== 

        //-----------------------------------------
        // イベント定義
        //-----------------------------------------
        #region ===== EVENTS =====

        /// <summary>
        /// 他のUIが選択された
        /// </summary>
        public void OnSelectOtherUI()
        {
            m_isSelectedOtherControlledUI = true;
        }
        #endregion //) ===== EVENTS ===== 
    }
}