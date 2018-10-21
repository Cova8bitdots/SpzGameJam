using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam.UI
{
    /// <summary>
    /// UIを統合管理するクラスの基底クラス
    /// </summary>
    public abstract class UIManagerBase<T> : MonoBehaviourSingleton<T>, ICoroutineExecutor where T : MonoBehaviourSingleton<T>
    {
        //-----------------------------------------
        // 定数関連
        //-----------------------------------------
        #region ===== CONSTS =====

        /// <summary>
        /// 初期化用パラメータ定義クラス
        /// </summary>
        public class InitParameter
        {
        }

        public delegate void OnChangeStateCallback( System.Type _prev, System.Type _current );
        #endregion //) ===== CONSTS ===== 

        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====

        UISceneStateMachine<UISceneState> m_stateMachine = null;
        protected UISceneStateMachine<UISceneState> SMachine { get { return m_stateMachine; } }

        /* State */
        Stack<System.Type> m_stateStack = new Stack<System.Type>();

        /* ステート切り替えイベント登録用 */
        OnChangeStateCallback m_onChangeStateEvent;

        /* ステート切り替え時のパラメータ受け渡し用 */
        object m_stateInitParam = null;
        public void SetStateInitParam( object _obj ){ m_stateInitParam = _obj;}
        public object GetStateInitParam()
        {
            object param = m_stateInitParam;
            m_stateInitParam = null;
            return param;
        }

        #endregion //) ===== MEMBER_VATRIABLES ===== 


        //-----------------------------------------
        // 初期化
        //-----------------------------------------
        #region ===== INITIALIZE =====

        /// <summary>
        /// 初期化メソッド
        /// 継承先から呼んでもらおう
        /// </summary>
        /// <param name="_param"></param>
        protected void Initialize(InitParameter _param)
        {
            if (_param == null)
            {
                return;
            }

            //StateMachine 初期化
            InitMachine();
        }

        #endregion //) ===== INITIALIZE ===== 


        //-----------------------------------------
        // Update
        //-----------------------------------------
        #region ===== UPDATE =====

        /// <summary>
        /// Update メソッド
        /// 出来ればMonobehaviour のUpdate は叩きたくないです。
        /// 管理している親クラスからUpdate をコールされたい
        /// </summary>
        public virtual void Update()
        {
            float deltaTime = Time.deltaTime;
            if( m_stateMachine != null )
            {
                m_stateMachine.Update(deltaTime);
            }

        }

        #endregion //) ===== UPDATE ===== 


        //-----------------------------------------
        // ICoroutineExecutor
        //-----------------------------------------
        #region ===== ICoroutineExecutor =====

        /// <summary>
        /// コルーチンの代理実行
        /// </summary>
        public Coroutine InvokeCoroutine(IEnumerator _coroutine )
        {
            if( _coroutine == null )
            {
                return null;
            }

            return StartCoroutine(_coroutine);
        }

        #endregion //) ===== ICoroutineExecutor ===== 

        //-----------------------------------------
        // StateMachine 管理
        //-----------------------------------------
        #region ===== STATE_MACHINE_CONTROLL =====

        /// <summary>
        /// StateMachine 初期化メソッド
        /// </summary>
        protected virtual void InitMachine()
        {
            // 初期化済み
            if( SMachine != null )
            {
                return;
            }
            m_stateMachine = new UISceneStateMachine<UISceneState>();
            SMachine.AddStateChangeEvent(OnChangeState);
        }


        /// <summary>
        /// StateMachine から現在のステートタイプを取得
        /// </summary>
        /// <returns></returns>
        public System.Type GetCurrentStateType() { return m_stateMachine != null ? m_stateMachine.CurrentStateType : null; }

        /// <summary>
        /// StateMachine から一つ前のステートタイプを取得
        /// </summary>
        /// <returns></returns>
        public System.Type GetPrevStateType() { return m_stateMachine != null ? m_stateMachine.PrevStateType : null; }

        /// <summary>
        /// State 切り替え時のコールバック
        /// </summary>
        /// <param name="_prevStateType"></param>
        /// <param name="_currentStateType"></param>
        public virtual void OnChangeState(System.Type _prevStateType, System.Type _currentStateType)
        {
            m_onChangeStateEvent?.Invoke( _prevStateType, _currentStateType);
        }
        #endregion //) ===== STATE_MACHINE_CONTROLL ===== 


        //-----------------------------------------
        // State 管理
        //-----------------------------------------
        #region ===== STATE_MANAGEMENT =====
        /// <summary>
        /// StateStack にある現在のStateType を取得
        /// </summary>
        /// <returns></returns>
        public System.Type GetTopStateType() { return m_stateStack.Count < 1 ? null : m_stateStack.Peek(); }

        /// <summary>
        /// 現在Stateの破棄
        /// </summary>
        public void PopState()
        {
            if( m_stateStack.Count < 1)
            {
                return;
            }
            m_stateStack.Pop();
        }

        /// <summary>
        /// 指定のStateを経由している場合、最初に見つかるそのStateまでPopする
        /// </summary>
        /// <param name="_state"></param>
        public bool PopToState( System.Type _state)
        {
            if (!HasState(_state))
            {   // 指定のStateを経由していないなら何もしない
                return false;
            }

            PopState();
            while (m_stateStack.Peek() != _state)
            {
                PopState();
            }

            return true;
        }

        /// <summary>
        /// 次ステートのセット
        /// </summary>
        /// <param name="_nextType"></param>
        public void PushNextState( System.Type _nextType )
        {
            m_stateStack.Push(_nextType);
        }

        /// <summary>
        /// 現在ステートを破棄して新たにステートをセット
        /// ステートが排他的な場合に使用
        /// </summary>
        /// <param name="_nextType"></param>
        public void PushExclusiveState( System.Type _nextType )
        {
            PopState();
            PushNextState(_nextType);
        }

        /// <summary>
        /// 特定のStateにいる or 経由してきたかどうか
        /// </summary>
        /// <param name="_t"></param>
        /// <returns></returns>
        public bool HasState(System.Type _t )
        {
            return m_stateStack.Contains(_t);
        }

        /// <summary>
        /// 現在のStateのStack数を返す
        /// </summary>
        /// <returns></returns>
        public int GetStateStackCount()
        {
            return m_stateStack != null ? m_stateStack.Count : 0;
        }


    #if UNITY_EDITOR
        public string[] GetStateNameArray()
        {
            if( m_stateStack.Count < 1)
            {
                return new string[1] { "Empty" };
            }

            var ary = m_stateStack.ToArray();
            string[] ret = new string[ary.Length];

            for (int i = 0; i < ary.Length; i++)
            {
                ret[i] = ary[i].ToString();
            }
            return ret;
        }
    #endif

        #endregion //) ===== STATE_MANAGEMENT ===== 

        //-----------------------------------------
        // イベント設定
        //-----------------------------------------
        #region ===== EVENT =====

        public void AddListenerOnChangeStateEvent( OnChangeStateCallback _callback){ m_onChangeStateEvent += _callback;}
        public void RemoveListenerOnChangeStateEvent( OnChangeStateCallback _callback){ m_onChangeStateEvent -= _callback;}

        #endregion //) ===== EVENT ===== 

    }
}