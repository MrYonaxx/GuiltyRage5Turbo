/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class HealthBarDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Health")]
        [SerializeField]
        Image characterFace;
        [SerializeField]
        TextMeshProUGUI textCharacterName;

        [SerializeField]
        GaugeDrawer healthGauge;

        [SerializeField]
        RectTransform transformBarNumber;
        [SerializeField]
        Image transformBar;

        [Title("Parameter")]
        [SerializeField]
        int healthBarAmount = 1000;

        List<Image> healthBarList = new List<Image>();


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
        public void DrawCharacter(PlayerData characterData, CharacterStatController characterStat)
        {
            textCharacterName.text = characterData.CharacterName;
            if (characterData.CharacterFace == null)
                characterFace.enabled = false;
            else
                characterFace.enabled = true;
            characterFace.sprite = characterData.CharacterFace;

            int healthBarNumber = (characterStat.GetHPMax() / 1000);
            healthBarNumber = Mathf.Max(0, healthBarNumber);
            for(int i = 0; i < healthBarNumber; i++)
            {
                if(healthBarList.Count <= i)
                    healthBarList.Add(Instantiate(transformBar, transformBarNumber));
                healthBarList[i].gameObject.SetActive(true);
            }
            for(int i = healthBarNumber; i < healthBarList.Count; i++)
            {
                healthBarList[i].gameObject.SetActive(false);
            }
            if (healthBarList.Count == 0)
                transformBarNumber.gameObject.SetActive(false);
            DrawHealth(characterStat.GetHP(), characterStat.GetHPMax());
        }

        public void DrawHealth(int hp, int maxHp)
        {
            healthGauge.DrawGauge(hp % healthBarAmount, Mathf.Min(maxHp, healthBarAmount));

            int currentHealth = 0;
            for (int i = healthBarList.Count-1; i >= 0; i--)
            {
                currentHealth += 1000;
                if (hp >= currentHealth)
                    healthBarList[i].enabled = true;
                else
                    healthBarList[i].enabled = false;
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace