/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{

    [System.Serializable]
    public class AnimationBehavior
    {
        [HorizontalGroup(Width = 20)]
        [HideLabel]
        public int frame;

        [HorizontalGroup]
        [HideLabel]
        public Sprite sprite;

        [HorizontalGroup]
        [HideLabel]
        public string tag;
    }

    [System.Serializable]
    public class CharacterAnimation
    {
        [HorizontalGroup("Animation", Width = 50)]
        [VerticalGroup("Animation/Left")]
        [SerializeField]
        [HideLabel]
        string animationsID; 

        [SerializeField]
        [VerticalGroup("Animation/Left")]
        [LabelWidth(100)]
        bool canLoop = false;

        [HorizontalGroup("Animation")]
        [VerticalGroup("Animation/Right")]
        [SerializeField]
        List<AnimationBehavior> animationsBehaviour;

    } 

} // #PROJECTNAME# namespace