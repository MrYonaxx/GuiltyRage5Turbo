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
    public class CustomStat
    {
        [HorizontalGroup("attribut", LabelWidth = 150)]
        public string CustomStatName;
        [HorizontalGroup("attribut", LabelWidth = 150)]
        public float CustomStatValue;
    }


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
        public float AttackMultiplier = 1;

        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float DefenseMultiplier = 1;

        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float MotionSpeed = 1;

        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Physics")]
        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float Speed;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float JumpImpulsion;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float Gravity;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float GravityMax;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float Mass = 1;



        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Knockback")]
        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float KnockbackTime = 1;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float KnockdownResistance = 1;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float WakeUpRecovery = 1;

        [Space]
        [Space]
        [SerializeField]
        public CustomStat[] customStats;

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
            
            AttackMultiplier = 0;
            DefenseMultiplier = 0;
            MotionSpeed = 0;

            Speed = 0;
            JumpImpulsion = 0;
            Gravity = 0;
            GravityMax = 0;
            Mass = 0;

            KnockbackTime = 0;
            KnockdownResistance = 0;
            WakeUpRecovery = 0;
        }

        // A utiliser si CharacterStat est utilisé comme pourcentage
        public CharacterStat(int i)
        {
            Hp = i;
            HpMax = i;

            AttackMultiplier = i;
            DefenseMultiplier = i;
            MotionSpeed = i;

            Speed = i;
            JumpImpulsion = i;
            Gravity = i;
            GravityMax = i;
            Mass = i;

            KnockbackTime = i;
            KnockdownResistance = i;
            WakeUpRecovery = i;
        }


        public CharacterStat(CharacterStat characterStat)
        {
            Hp = characterStat.Hp;
            HpMax = characterStat.HpMax;

            AttackMultiplier = characterStat.AttackMultiplier;
            DefenseMultiplier = characterStat.DefenseMultiplier;
            MotionSpeed = characterStat.MotionSpeed;

            Speed = characterStat.Speed;
            JumpImpulsion = characterStat.JumpImpulsion;
            Gravity = characterStat.Gravity;
            GravityMax = characterStat.GravityMax;
            Mass = characterStat.Mass;

            KnockbackTime = characterStat.KnockbackTime;
            KnockdownResistance = characterStat.KnockdownResistance;
            WakeUpRecovery = characterStat.WakeUpRecovery;


        }

        #endregion

    } 

} // #PROJECTNAME# namespace