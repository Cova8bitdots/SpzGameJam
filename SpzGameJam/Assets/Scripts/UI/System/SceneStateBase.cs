using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.UI
{
    /// <summary>
    /// Scene内のState 動作定義クラスの基底クラス
    /// </summary>
    public abstract class SceneStateBase<T> where T : UIManagerBase<T>
    {
        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====

        /// <summary>
        /// このStateを管理する親クラス兼コルーチンの代理実行人
        /// </summary>
        protected T m_manager;

        /// <summary>
        /// 初期化実行終了チェックフラグ
        /// </summary>
        private bool m_isInitializeEnd = false;
        public bool IsInitializeEnd { get { return m_isInitializeEnd; } }
        protected void BeginInitialize() { m_isInitializeEnd = false; }
        protected void EndInitialize() { m_isInitializeEnd = true; }


        #endregion //) ===== MEMBER_VATRIABLES ===== 

        //-----------------------------------------
        // State操作
        //-----------------------------------------
        #region ===== STATE_METHOD =====

        /// <summary>
        /// State 開始時処理
        /// </summary>
        /// <param name="_manager"></param>
        public virtual void OnEnter(T _manager){ m_manager = _manager; }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="_deltaTime"></param>
        public virtual void OnUpdate(float _deltaTime ){}


        /// <summary>
        /// State 終了処理
        /// </summary>
        public virtual void OnExit() { }


        /// <summary>
        /// 次のStateを取得
        /// Null を返した場合StateMachine の終了
        /// State維持の場合は自信のtypeを返す
        /// </summary>
        /// <returns></returns>
        public virtual System.Type GetNextState() { return this.GetType();  }

        #endregion //) ===== STATE_METHOD ===== 
    }
}