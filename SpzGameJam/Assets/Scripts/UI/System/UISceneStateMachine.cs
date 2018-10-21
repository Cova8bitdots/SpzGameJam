using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.UI
{
    /// <summary>
    /// UI の現在のSceneのState制御用ステートマシン
    /// IngameUIManager と共生関係にあります
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UISceneStateMachine<T> where T : UISceneState
    {
        //-----------------------------------------
        // 定数関連
        //-----------------------------------------
        #region ===== CONSTS =====

        /// <summary>
        /// State 変更イベントのデリゲータ
        /// </summary>
        /// <param name="_prevType">前のState</param>
        /// <param name="_currentType">現在(次)のState</param>
        public delegate void OnChangeStateEvent(System.Type _prevType, System.Type _currentType);


        #endregion //) ===== CONSTS ===== 

        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====

        /// <summary>
        /// このStateMachineの管理元
        /// </summary>
        protected IngameUIManager m_manager = null;

        /// <summary>
        /// 前のState
        /// </summary>
        private T m_prevState = null;
        protected T PrevState { get { return m_prevState; } }

        /// <summary>
        /// 前のState のType
        /// </summary>
        public System.Type PrevStateType { get { return m_prevState != null ? m_prevState.GetType() : null; } }

        /// <summary>
        /// 現在のState
        /// </summary>
        protected T m_currentState = null;
        protected T CurrentState { get { return m_currentState; } }

        /// <summary>
        /// 現在のState のType
        /// </summary>
        public System.Type CurrentStateType { get { return m_currentState != null ? m_currentState.GetType() : null; } }

        /// <summary>
        /// State切り替わり時のイベント管理用
        /// </summary>
        protected OnChangeStateEvent m_onChangeStateEvent = delegate { };

        #endregion //) ===== MEMBER_VATRIABLES ===== 

        //-----------------------------------------
        // State切り替わりイベント管理
        //-----------------------------------------
        #region ===== CHANGE_STATE_EVENT_CONTROLL =====

        /// <summary>
        /// State切り替わりイベントの登録
        /// </summary>
        /// <param name="_e"></param>
        public void AddStateChangeEvent( OnChangeStateEvent _e )
        {
            m_onChangeStateEvent += _e;
        }

        /// <summary>
        /// State切り替わりイベントの削除
        /// </summary>
        /// <param name="_e"></param>
        public void RemoveStateChangeEvent(OnChangeStateEvent _e)
        {
            m_onChangeStateEvent -= _e;
        }

        #endregion //) ===== CHANGE_STATE_EVENT_CONTROLL ===== 

        //-----------------------------------------
        // StateMachine操作
        //-----------------------------------------
        #region ===== STATEMACHINE_METHOD =====

        public void Initialize(IngameUIManager _manager )
        {
            m_manager = _manager;
        }

        /// <summary>
        /// StateMachine 稼働
        /// </summary>
        /// <param name="_currentStateType"></param>
        public void Start( System.Type _currentStateType )
        {
            m_currentState = CreateNextState( _currentStateType );

            if( m_currentState != null )
            {
                m_currentState.OnEnter(m_manager);
            }
        }

        public void Update(float _deltaTime )
        {
            if( !IsRun() )
            {
                return;
            }

            // 現在Stateの更新
            CurrentState.OnUpdate(_deltaTime);

            // 次Staeのチェック
            System.Type nextStateType = CurrentState.GetNextState();

            // 次も同じState
            if( CurrentStateType == nextStateType )
            {
                return;
            }

            /* 次が異なるStateなので現在State の終了準備 */
            // State のBackup
            m_prevState = CurrentState;
            // 終了処理
            CurrentState.OnExit();

            // 次がnull ならStateMachine の終了
            if( nextStateType == null )
            {
                m_currentState = null;
                return;
            }

            // 次Stateの生成
            m_currentState = CreateNextState(nextStateType);
            if (m_currentState != null)
            {
                m_currentState.OnEnter(m_manager);
            }
            // State切り替わりいべんとの通知
            if (m_onChangeStateEvent != null)
            {
                m_onChangeStateEvent(PrevStateType, CurrentStateType);
            }
        }

        /// <summary>
        /// StateMachine 稼働中かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsRun() { return m_currentState != null; }

        #endregion //) ===== STATE_METHOD ===== 


        /// <summary>
        /// 指定State を生成
        /// </summary>
        /// <param name="_type">生成するState</param>
        /// <returns></returns>
        private T CreateNextState( System.Type _type )
        {
            if( _type == null )
            {
                return null;
            }

            return System.Activator.CreateInstance(_type) as T;
        }
    }
}