using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam.UI.Extentions
{
    public static class AnimatorExtentions
    {
        /// <summary>
        /// 定義済みのTriggerをオールクリアする用関数
        /// </summary>
        /// <param name="self"></param>
        public static void ResetAllTriggers(this Animator self)
        {
            for (int i = 0; i < self.parameterCount; i++)
            {
                if( self.parameters[i].type == AnimatorControllerParameterType.Trigger )
                {
                    self.ResetTrigger(self.parameters[i].name);
                }
            }
        }
	    /// <summary>
	    /// 定義済みのBoolを全てfalseにする用関数
	    /// </summary>
	    /// <param name="self"></param>
	    public static void ResetAllBools(this Animator self)
	    {
		    for (int i = 0; i < self.parameterCount; i++)
		    {
			    if( self.parameters[i].type == AnimatorControllerParameterType.Bool )
			    {
				    self.SetBool(self.parameters[i].name, false);
			    }
		    }
	    }
    }
}

