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

        [SerializeField]
        [HideLabel]
        CharacterStat characterBaseStat;
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
            return Mathf.RoundToInt((characterBaseStat.Hp + characterBonusStat.Hp) * characterPercentageBonusStat.Hp);
        }

        public int GetHPMax()
        {
            return Mathf.RoundToInt((characterBaseStat.HpMax + characterBonusStat.HpMax) * characterPercentageBonusStat.HpMax);
        }

        public float GetKnockbackTime()
        {
            return (characterBaseStat.KnockbackTime + characterBonusStat.KnockbackTime) * characterPercentageBonusStat.KnockbackTime;
        }


        public float GetReload()
        {
            return (characterBaseStat.Reload + characterBonusStat.Reload) * characterPercentageBonusStat.Reload;
        }
        public float GetRecoveryKnockback()
        {
            return (characterBaseStat.RecoveryKnockback + characterBonusStat.RecoveryKnockback) * characterPercentageBonusStat.RecoveryKnockback;
        }

        public float GetMass()
        {
            return (characterBaseStat.Mass + characterBonusStat.Mass) * characterPercentageBonusStat.Mass;
        }

        public float GetSpeed()
        {
            return (characterBaseStat.Speed + characterBonusStat.Speed) * characterPercentageBonusStat.Speed;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public CharacterStatController(PlayerData characterData)
        {
            characterBaseStat = new CharacterStat(characterData.InitialCharacterStat);
            characterBonusStat = new CharacterStat();
            characterPercentageBonusStat = new CharacterStat(1);
        }

        public void CreateStatController(PlayerData characterData)
        {
            characterBaseStat = new CharacterStat(characterData.InitialCharacterStat);
            characterBonusStat = new CharacterStat();
            characterPercentageBonusStat = new CharacterStat(1);
        }


        public int TakeDamage(AttackData attack)
        {
            int finalDamage = 0;
            int rawDamage = 0;// attack.CardData.CardDamage[attack.GetCardValue()];




            finalDamage = rawDamage;

            characterBaseStat.Hp -= finalDamage;
            return finalDamage;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace