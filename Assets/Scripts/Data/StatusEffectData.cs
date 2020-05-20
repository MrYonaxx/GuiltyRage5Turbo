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
    [CreateAssetMenu(fileName = "StatusData", menuName = "Status", order = 1)]
    public class StatusEffectData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [HorizontalGroup("StatusBasicInfo", Width = 96)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        private Sprite statusIconSprite;
        public Sprite StatusIconSprite
        {
            get { return statusIconSprite; }
        }


        [HorizontalGroup("StatusBasicInfo")]
        [VerticalGroup("StatusBasicInfo/Right")]
        [SerializeField]
        string statusName;
        public string StatusName
        {
            get { return statusName; }
        }

        [HorizontalGroup("StatusBasicInfo")]
        [VerticalGroup("StatusBasicInfo/Right")]
        [SerializeField]
        string statusDescription;
        public string StatusDescription
        {
            get { return statusDescription; }
        }



        [Title("Status Parameter")]
        [SerializeField]
        Vector2 statusTime;
        public Vector2 StatusTime
        {
            get { return statusTime; }
        }

        [SerializeField]
        float tickDamage;
        public float TickDamage
        {
            get { return tickDamage; }
        }
        [SerializeField]
        bool stun;
        public bool Stun
        {
            get { return stun; }
        }

        [SerializeField]
        CharacterStat characterStatModifier;
        public CharacterStat CharacterStatModifier
        {
            get { return characterStatModifier; }
        }

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


        #endregion

    } 

} // #PROJECTNAME# namespace