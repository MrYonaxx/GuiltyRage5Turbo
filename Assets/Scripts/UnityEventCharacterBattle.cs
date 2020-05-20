/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{

    [System.Serializable]
    public class UnityEventCharacterBattle : UnityEvent<Character>
    {
    }

    [System.Serializable]
    public class UnityEventAttackBehavior : UnityEvent<AttackBehavior>
    {
    }

    /*[System.Serializable]
    public class UnityEventAttackBehavior : UnityEvent<Character, De>
    {
    }*/

} // #PROJECTNAME# namespace