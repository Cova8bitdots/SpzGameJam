using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coroutine の代理実行者用インターフェース
/// (MonoBehaviour 非継承クラスがCoroutine を実行したいときの代理実行人)
/// </summary>
public interface ICoroutineExecutor
{
    Coroutine InvokeCoroutine(IEnumerator _enumrator);
}