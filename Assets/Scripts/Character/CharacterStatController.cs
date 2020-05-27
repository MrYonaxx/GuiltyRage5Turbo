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
    // Contient toute la logique lié aux stats
    public class CharacterStatController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        int currentHP = 0;

        [SerializeField]
        [HideLabel]
        PlayerData characterBaseData;
        [BoxGroup]
        [SerializeField]
        CharacterStat characterBonusStat;
        [BoxGroup]
        [SerializeField]
        CharacterStat characterPercentageBonusStat;

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
            return Mathf.RoundToInt((characterBaseData.CharacterStat.HpMax + characterBonusStat.HpMax) * characterPercentageBonusStat.HpMax);
        }


        public float GetKnockdownResistance()
        {
            return (characterBaseData.CharacterStat.KnockdownResistance + characterBonusStat.KnockdownResistance) * characterPercentageBonusStat.KnockdownResistance;
        }

        public float GetKnockbackTime()
        {
            return (characterBaseData.CharacterStat.KnockbackTime + characterBonusStat.KnockbackTime) * characterPercentageBonusStat.KnockbackTime;
        }



        public float GetMass()
        {
            return (characterBaseData.CharacterStat.Mass + characterBonusStat.Mass) * characterPercentageBonusStat.Mass;
        }

        public float GetSpeed()
        {
            return (characterBaseData.CharacterStat.Speed + characterBonusStat.Speed) * characterPercentageBonusStat.Speed;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public CharacterStatController(PlayerData characterData)
        {
            characterBaseData = characterData;
            characterBonusStat = new CharacterStat();
            characterPercentageBonusStat = new CharacterStat(1);
            currentHP = Mathf.RoundToInt((characterBaseData.CharacterStat.Hp + characterBonusStat.Hp) * characterPercentageBonusStat.Hp);
        }

        public void CreateStatController(PlayerData characterData)
        {
            characterBaseData = characterData;
            characterBonusStat = new CharacterStat();
            characterPercentageBonusStat = new CharacterStat(1);
        }


        public int TakeDamage(AttackBehavior attack)
        {
            int finalDamage = 0;
            int rawDamage = 0;




            finalDamage = attack.AttackDamage;
            currentHP -= finalDamage;
            Mathf.Clamp(currentHP, 0, GetHPMax());
            return finalDamage;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace