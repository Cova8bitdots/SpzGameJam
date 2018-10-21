using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam.UI
{
    public class NormalButtonController : ButtonUIController
    {
    	//-----------------------------------------
    	// 定数関連
    	//-----------------------------------------
    	#region ===== CONSTS =====

    	/// <summary>
    	/// 初期化用パラメータ
    	/// </summary>
    	public class InitParam : InitParameter
    	{
    		public Sprite ButtonSprite = null;
    	}

    	#endregion //) ===== CONSTS ===== 

    	//-----------------------------------------
    	// メンバ変数
    	//-----------------------------------------
    	#region ===== MEMBER_VARIABLES =====
    	// UV操作時のキャッシュ
    	Vector2 m_imageScale = Vector2.one;

    	[SerializeField, Tooltip("表示用ボタン画像")]
    	Image m_buttonImage = null;

    	[SerializeField, Tooltip("Disable時表示用ボタン画像")]
    	Image m_disabledButtonImage = null;

    	#endregion //) ===== MEMBER_VARIABLES ===== 

    	//-----------------------------------------
    	// 初期化
    	//-----------------------------------------
    	#region ===== INITIALIZE =====

    	/// <summary>
    	/// 初期化メソッド
    	/// </summary>
    	/// <param name="_param"></param>
    	public virtual void Init(InitParam _param)
    	{
    		base.Init(_param);
    		if( _param == null)
    		{
    			return;
    		}
    	}

    	public void SetHorizontalFlipImage()
    	{
    		m_imageScale.x *= -1f;
    		m_buttonImage?.material?.SetTextureScale( "_MainTex", m_imageScale );
    		m_disabledButtonImage?.material?.SetTextureScale("_MainTex", m_imageScale);

    	}
    	public void SetVerticalFlipImage()
    	{
    		m_imageScale.y *= -1f;
    		m_buttonImage?.material?.SetTextureScale( "_MainTex", m_imageScale );
    		m_disabledButtonImage?.material?.SetTextureScale("_MainTex", m_imageScale);
    	}
    	#endregion //) ===== INITIALIZE ===== 
    }
}