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
    public class CharacterReflection: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SpriteRenderer spriteReflection;
        [SerializeField]
        SpriteRenderer spriteToReflect;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public void Update()
        {
            spriteReflection.sprite = spriteToReflect.sprite;
            spriteReflection.flipX = spriteToReflect.flipX;
            spriteReflection.transform.localPosition = new Vector3(spriteToReflect.transform.localPosition.x, spriteToReflect.transform.localPosition.y * -0.5f, spriteToReflect.transform.localPosition.z);
        }
        #endregion

    } 

} // #PROJECTNAME# namespace