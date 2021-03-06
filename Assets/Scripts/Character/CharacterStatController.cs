﻿/*****************************************************************
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
    // Contient toute la logique lié aux stats
    public class CharacterStatController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        int currentHP = 0;

        [SerializeField]
        private CharacterData characterData;

        public CharacterData CharacterData
        {
            get { return characterData; }
        }

        [BoxGroup]
        [SerializeField]
        CharacterStat characterBonusStat;
        [BoxGroup]
        [SerializeField]
        CharacterStat characterPercentageBonusStat;

         // A optimiser en sauvegardant les résultats dans un 3e character stat et en le mettant à jour à chaque changement de stat

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        public int GetHP()
        {
            return Mathf.RoundToInt((currentHP + characterBonusStat.Hp) * characterPercentageBonusStat.Hp);
        }

        public int GetHPMax()
        {
            return Mathf.RoundToInt((characterData.CharacterStat.HpMax + characterBonusStat.HpMax) * characterPercentageBonusStat.HpMax);
        }

        public float GetMotionSpeed()
        {
            return (characterData.CharacterStat.MotionSpeed + characterBonusStat.MotionSpeed) * characterPercentageBonusStat.MotionSpeed;
        }




        public float GetSpeed()
        {
            return (characterData.CharacterStat.Speed + characterBonusStat.Speed) * characterPercentageBonusStat.Speed;
        }

        public float GetJumpImpulsion()
        {
            return (characterData.CharacterStat.JumpImpulsion + characterBonusStat.JumpImpulsion) * characterPercentageBonusStat.JumpImpulsion;
        }

        public float GetGravity()
        {
            return (characterData.CharacterStat.Gravity + characterBonusStat.Gravity) * characterPercentageBonusStat.Gravity;
        }

        public float GetGravityMax()
        {
            return (characterData.CharacterStat.GravityMax + characterBonusStat.GravityMax) * characterPercentageBonusStat.GravityMax;
        }

        public float GetMass()
        {
            return (characterData.CharacterStat.Mass + characterBonusStat.Mass) * characterPercentageBonusStat.Mass;
        }




        public float GetKnockdownResistance()
        {
            return (characterData.CharacterStat.KnockdownResistance + characterBonusStat.KnockdownResistance) * characterPercentageBonusStat.KnockdownResistance;
        }

        public float GetKnockbackTime()
        {
            return (characterData.CharacterStat.KnockbackTime + characterBonusStat.KnockbackTime) * characterPercentageBonusStat.KnockbackTime;
        }

        public float GetWakeUpRecovery()
        {
            return (characterData.CharacterStat.WakeUpRecovery + characterBonusStat.WakeUpRecovery) * characterPercentageBonusStat.WakeUpRecovery;
        }






        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public CharacterStatController(CharacterData data)
        {
            characterData = data;
            characterBonusStat = new CharacterStat();
            characterPercentageBonusStat = new CharacterStat(1);
            currentHP = Mathf.RoundToInt((characterData.CharacterStat.Hp + characterBonusStat.Hp) * characterPercentageBonusStat.Hp);
        }

        public void CreateStatController()
        {
            characterBonusStat = new CharacterStat();
            characterPercentageBonusStat = new CharacterStat(1);
            currentHP = Mathf.RoundToInt((characterData.CharacterStat.Hp + characterBonusStat.Hp) * characterPercentageBonusStat.Hp);
        }


        public int TakeDamage(AttackBehavior attack)
        {
            int finalDamage = 0;
            int rawDamage = 0;




            finalDamage = attack.AttackDamage;
            currentHP -= finalDamage;
            currentHP = Mathf.Clamp(currentHP, 0, GetHPMax());
            return finalDamage;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace