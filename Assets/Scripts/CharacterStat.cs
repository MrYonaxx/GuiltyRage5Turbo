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
    public class CharacterStat
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [HorizontalGroup("HP", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10, Width = 0.75f)]
        [HideLabel]
        [ProgressBar(0, "HpMax", Height = 20)]
        [Title("HP")]
        [SerializeField]
        private float hp;
        public float Hp
        {
            get { return hp; }
            set { hp = Mathf.Max(0, value); }
        }


        [HorizontalGroup("HP", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [HideLabel]
        [Title("HP Max")]
        [SerializeField]
        public float HpMax;

        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Statistiques")]
        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float Attack;

        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float Magic;

        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float Defense;

        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Card")]
        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float Reload;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float Recovery;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float RecoveryKnockback;


        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Character")]
        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float Speed;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float Mass = 1;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float KnockbackTime = 1;

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
        public CharacterStat()
        {
            Hp = 0;
            HpMax = 0;
            Attack = 0;
            Magic = 0;
            Defense = 0;
            Reload = 0;
            Recovery = 0;
            Speed = 0;
            Mass = 0;
            KnockbackTime = 0;
            RecoveryKnockback = 0;
        }

        // A utiliser si CharacterStat est utilisé comme pourcentage
        public CharacterStat(int i)
        {
            Hp = i;
            HpMax = i;
            Attack = i;
            Magic = i;
            Defense = i;
            Reload = i;
            Recovery = i;
            Speed = i;
            Mass = i;
            KnockbackTime = i;
            RecoveryKnockback = i;
        }


        public CharacterStat(CharacterStat characterStat)
        {
            Hp = characterStat.Hp;
            HpMax = characterStat.HpMax;
            Attack = characterStat.Attack;
            Magic = characterStat.Magic;
            Defense = characterStat.Defense;

            Reload = characterStat.Reload;
            Recovery = characterStat.Recovery;
            RecoveryKnockback = characterStat.RecoveryKnockback;

            Speed = characterStat.Speed;
            Mass = characterStat.Mass;
            KnockbackTime = characterStat.KnockbackTime;

        }

        #endregion

    } 

} // #PROJECTNAME# namespace