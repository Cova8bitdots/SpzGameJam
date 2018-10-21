using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using GameJam.UI.Extentions;


namespace GameJam.UI
{
    /// <summary>
    /// ボタンの動作定義用クラス
    /// </summary>
    [RequireComponent( typeof( Button))]
    public class ButtonUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //-----------------------------------------
        // 定数関連
        //-----------------------------------------
        #region ===== CONSTS =====

        //Animator Trigger 定義
        protected const string ANIMATOR_TRIGGER_BUTTON_PRESS_DOWN = "OnPressed";
        protected const string ANIMATOR_TRIGGER_BUTTON_CLICKED = "OnReleaseSucceeded";
        protected const string ANIMATOR_TRIGGER_BUTTON_CANCEL_TAP = "OnReleaseCanceled";

        protected const string ANIMATOR_TRIGGER_BUTTON_ENABLE = "Enabled";
        protected const string ANIMATOR_TRIGGER_BUTTON_DISABLE = "Disabled";

        /// <summary>
        /// 初期化用パラメータ
        /// </summary>
        public class InitParameter
        {
            public int ItemIndex = -1;  // ButtonID
            public System.Action<int> OnButtonClicked = null;               // ボタンクリック時コールバック
            public System.Action<int> OnButtonLongTapped = null;            // ボタン長押し時コールバック
            public System.Action<PointerEventData> OnFlickCallback = null;  // ボタンフリック時コールバック
        }

        [System.Serializable]
        public class TouchSound
        {
            public string OnPress           = "se_sys_operation_ok_push";               // ボタン押下時
            public string OnReleaseSuccess  = "se_sys_operation_ok_release";            // ボタンリリース(通常)
            public string OnReleaseCancel   = "se_sys_cancel";                          // ボタンリリース(キャンセル)
        }
        #endregion //) ===== CONSTS ===== 

        //-----------------------------------------
        // メンバ変数
        //-----------------------------------------
        #region ===== MEMBER_VATRIABLES =====

        [SerializeField, Tooltip("ボタンの動きをAnimator で管理する場合設定")]
        Animator m_animator = null;

        //制御対象
        Button m_button = null;
        protected Button CurrentButton { get { return m_button; } }

        // 初期化パラメータ格納変数群
        // ID
        protected int m_itemIndex = -1;
        // Callbacks
        protected System.Action<int> m_onButtonClicked = null;                // ボタンクリック時コールバック
        protected System.Action<int> m_onButtonLongTapped = null;             // ボタン長押し時コールバック
        protected System.Action<PointerEventData> m_onFlickCallback = null;   // ボタンフリック時コールバック

        /* Flags */
        // Tap判定用
        private bool m_isStartPress = false;
        public bool IsStartPress { get { return m_isStartPress; } }
        //Click 判定用
        private bool isPressed = false;
        public bool IsPressed { get { return isPressed; } }
        // 押下中にBound 外へ指を動かしてキャンセルしたかどうか
        private bool m_isCancelTap = false;
        public bool IsCancelTap { get { return m_isCancelTap; } }
        // ドラッグ操作判定
        private bool m_isStartDrag = false;
        public bool IsStartDrag { get { return m_isStartDrag;  } }


        /* Timer */
        private float m_dragTimer = 0.0f;       // Drag時間
        public  float DragTime { get { return m_dragTimer; } }
        private float m_pressedTime = 0.0f;     // 長押し判定検知用
        public float PressedTime { get { return m_pressedTime; } }

        // /* Sound */
        // [SerializeField]
        // private TouchSound m_touchSound;
        // protected TouchSound Sound{get{return m_touchSound;}}
        #endregion //) ===== MEMBER_VATRIABLES ===== 

        //-----------------------------------------
        // 初期化
        //-----------------------------------------
        #region ===== INITIALIZE =====

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        protected void Init( InitParameter _param )
        {
            if (m_button == null)
            {
                m_button = GetComponent<Button>();
                // 長押し判定もあるため、onClick にはEvent登録を行わない
                // (自前で呼ぶ)
            }

            if ( _param == null )
            {
                return;
            }

            m_itemIndex = _param.ItemIndex;
            m_onButtonClicked = _param.OnButtonClicked;
            m_onButtonLongTapped = _param.OnButtonLongTapped;
            m_onFlickCallback = _param.OnFlickCallback;
        }

        /// <summary>
        /// ボタンタップ開始判定のリセット
        /// 意図的にClickイベントなどが実行されないための処理
        /// </summary>
        protected void ResetButtonPressParameters()
        {
            m_isStartPress = false;
            isPressed = true;
        }
        #endregion //) ===== INITIALIZE ===== 

        //-----------------------------------------
        // Getter
        //-----------------------------------------
        #region ===== GETTER =====

        /// <summary>
        /// 長押しイベントを保持しているか
        /// </summary>
        public bool HasLongTapEvent { get { return m_onButtonLongTapped != null;  } }
        #endregion //) ===== GETTER ===== 

        //-----------------------------------------
        // LifeCycle
        //-----------------------------------------
        #region ===== LIFECYCLE =====

        protected virtual void Update()
        {
            // 長押し検知用
            ObserveLongTapCheck();
        }

        private void ObserveLongTapCheck()
        {
            if( !m_isStartPress )
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            m_pressedTime += deltaTime;

            // 長押し判定
            if (m_pressedTime > UIConsts.LONG_TAP_TIME_THRESHOLD && !m_isCancelTap && HasLongTapEvent)
            {
                OnLongTap();
                //ボタンアニメーションをクリック時と同じもので再生
                PlayButtonAnim(ANIMATOR_TRIGGER_BUTTON_CLICKED);
                return;
            }
        }

        #endregion //) ===== LIFECYCLE ===== 

        //-----------------------------------------
        // アニメーションまわり
        //-----------------------------------------
        #region ===== ANIMATOR_ANIMATION =====

        /// <summary>
        /// Animator 再生関数
        /// </summary>
        /// <param name="_triggerName"></param>
        protected void PlayButtonAnim( string _triggerName )
        {
            if (m_animator == null)
            {
                return;
            }
            m_animator.ResetAllTriggers();
            m_animator.SetTrigger(_triggerName);
        }

        #endregion //) ===== ANIMATOR_ANIMATION ===== 

        //-----------------------------------------
        // Interface実装
        //
        // Unity の処理順番
        // 1.OnPointerDown
        // 2. Drag開始したら OnBeginDrag
        // 3. Drag中なら OnDrag
        // 4. OnPointerUp
        // 5. Dragしてたら OnEndDrag
        // 
        //-----------------------------------------
        #region ===== INTERFACE =====

        public virtual void OnPointerDown(PointerEventData _event)
        {

            // 押下中アニメ再生
            PlayButtonAnim(ANIMATOR_TRIGGER_BUTTON_PRESS_DOWN);
            // // Sound再生
            // SoundManager.Play(SoundType.Se, Sound.OnPress, 1.0f);

            ResetTouchParam();
        }
        protected void ResetTouchParam()
        {
            //Reset Parameter
            m_pressedTime = 0.0f;
            isPressed = false;
            m_isCancelTap = false;
            m_isStartPress = true;
            m_isStartDrag = false;
        }

        public virtual void OnPointerUp(PointerEventData _event)
        {
            if( !m_isStartPress)
            {
                return;
            }
            m_isStartPress = false;
            if ( _event.pointerCurrentRaycast.gameObject == _event.pointerPress )
            {
                // Press とUp の対象が同一のためClick イベントの実行
                // OnPointerClick はOnPounterUp の後に呼ばれるためここで判定

                // クリックアニメ再生
                PlayButtonAnim(ANIMATOR_TRIGGER_BUTTON_CLICKED);
                // // Sound再生
                // SoundManager.Play(SoundType.Se, Sound.OnReleaseSuccess, 1.0f);

                if (m_button != null)
                {
                    //m_button.onClick.Invoke();
                    OnClickButton();
                }

                isPressed = true;
            }
            else
            {
                //選択解除のためキャンセル扱い
                PlayButtonAnim(ANIMATOR_TRIGGER_BUTTON_CANCEL_TAP);
                // // Sound再生
                // SoundManager.Play(SoundType.Se, Sound.OnReleaseCancel, 1.0f);
            }
        }

        public virtual void OnBeginDrag(PointerEventData _event)
        {
            m_isStartDrag = true;
        }
        public virtual void OnDrag(PointerEventData _event)
        {
            // 範囲外に出た場合はタップキャンセル扱い
            if( !m_isCancelTap && _event.pointerPress != _event.pointerCurrentRaycast.gameObject )
            {
                Debug.Log("Cancel Tap");
                m_isCancelTap = true;
            }
        }
        public virtual void OnEndDrag(PointerEventData _event)
        {
            // Click判定を優先する
            if(isPressed)
            {
                return;
            }
            // Drag時間が閾値Over ならFlick判定を行わない
            if(m_pressedTime >= UIConsts.FLICK_TIME_THRESHOULD)
            {
                return;
            }
            OnFlick(_event);
        }

        #endregion //) ===== INTERFACE ===== 

        //-----------------------------------------
        // ボタン操作
        //-----------------------------------------
        #region ===== BUTTON =====

        // ボタンのEnable操作
        public void SetButtonEnable() { SwitchButtonEnable(true);  }
        public void SetButtonDisable() { SwitchButtonEnable(false); }
        private void SwitchButtonEnable( bool isEnable )
        {
            if(m_button == null )
            {
                return;
            }
            m_button.interactable = isEnable;
    		m_button.image.raycastTarget = isEnable;

            PlayButtonAnim(isEnable ? ANIMATOR_TRIGGER_BUTTON_ENABLE : ANIMATOR_TRIGGER_BUTTON_DISABLE);
        }

        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        public virtual void OnClickButton()
        {
    		if( IsCancelTap )
    		{
    			return;
    		}
            if (m_onButtonClicked != null)
            {
                m_onButtonClicked(m_itemIndex);
            }
        }

        /// <summary>
        /// ボタン長押しイベント
        /// </summary>
        public virtual void OnLongTap()
        {
            if( !HasLongTapEvent )
            {
                return;
            }
            // 通常のClick判定をこのあと行わせないため変数のリセット
            ResetButtonPressParameters();
            //Event 実行
            m_onButtonLongTapped(m_itemIndex);
        }

        /// <summary>
        /// ボタンフリックイベント
        /// </summary>
        /// <param name="_event"></param>
        protected virtual void OnFlick(PointerEventData _event)
        {
            if (m_onFlickCallback != null)
            {
                m_onFlickCallback(_event);
            }
        }


    	public void ShowButton(){ SwitchButtonVisible( true ); }
    	public void HideButton(){ SwitchButtonVisible( false ); }
    	private void SwitchButtonVisible( bool _isEnable )
        {
            if(m_animator == null )
            {
                return;
            }
    		m_animator.gameObject.SetActive( _isEnable);
        }

        #endregion //) ===== BUTTON ===== 
    }
}
