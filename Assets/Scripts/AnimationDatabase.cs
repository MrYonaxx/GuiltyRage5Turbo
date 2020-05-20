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
    public class AnimationDatabase
    {
        /*[HorizontalGroup]
        [SerializeField]
        private List<string> animationsID; 
        public List<string> AnimationsID
        {
            get { return animationsID; }
        }*/

        [HorizontalGroup]
        [SerializeField]
        private List<CharacterAnimation> animations;
        public List<CharacterAnimation> Animations
        {
            get { return animations; }
        }

    } 

} // #PROJECTNAME# namespace