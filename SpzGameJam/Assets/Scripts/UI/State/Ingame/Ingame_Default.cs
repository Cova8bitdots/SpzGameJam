using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.UI;

public class Ingame_Default : UISceneState
{
        //-----------------------------------------
        // 初期化
        //-----------------------------------------
        #region ===== INITIALIZE =====

        /// <summary>
        /// 初期化メソッド
        /// OnEnter から呼ぶ
        /// </summary>
        protected override void Initialize()
        {
            m_manager.InvokeCoroutine( InitCoroutine() );
            
        }

        /// <summary>
        /// 初期化コルーチン
        /// </summary>
        /// <returns></returns>
    	protected override IEnumerator InitCoroutine()
    	{
    		yield return null;
    		// 初期化終了
    		EndInitialize();
    	}

        #endregion //) ===== INITIALIZE =====
}
