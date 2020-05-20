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
    [CreateAssetMenu(fileName = "CharacterData", menuName = "PlayableCharacterData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [HorizontalGroup("CharacterBasic", Width = 96, PaddingLeft = 10)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        Sprite characterFace;
        public Sprite CharacterFace
        {
            get { return characterFace; }
        }

        [HorizontalGroup("CharacterBasic", PaddingLeft = 10)]
        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        string characterName;
        public string CharacterName
        {
            get { return characterName; }
        }

        [SerializeField]
        [HideLabel]
        CharacterStat initialCharacterStat;
        public CharacterStat InitialCharacterStat
        {
            get { return initialCharacterStat; }
        }

        [Title("Hitbox")]
        [Space]
        [SerializeField]
        private Vector2 hitbox;
        public Vector2 Hitbox 
        {
            get { return hitbox; } 
        }

        [SerializeField]
        private Vector2 hitboxOffset;
        public Vector2 HitboxOffset
        {
            get { return hitboxOffset; }
        }

        [Space]
        [Space]
        [Space]
        [Title("Card Parameter")]
        [HideLabel]
        [SerializeField]
        string salut;




        [TabGroup("CardDatabase")]
        [SerializeField]
        [ReadOnly]
        private int maxProbability;

        [HideInInspector]
        [TabGroup("Animations")]
        [SerializeField]
        [HideLabel]
        AnimationDatabase animationDatabase;

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