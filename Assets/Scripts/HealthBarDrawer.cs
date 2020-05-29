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
    public class HealthBarDrawer: MonoBehaviour, ICharacterInfoDrawer
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Health")]
        [SerializeField]
        Image characterFace;
        [SerializeField]
        Image characterFaceOutline;
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

        [Title("TargetHealth")]
        [SerializeField]
        RectTransform targetTransform;
        [SerializeField]
        GaugeDrawer targetHealthGauge;
        [SerializeField]
        TextMeshProUGUI textTargetName;

        [Title("TargetParameter")]
        [SerializeField]
        int targetHealthBarAmount = 1000;
        [SerializeField]
        float timeTargetDisappearNormal = 3;
        [SerializeField]
        float timeTargetDisappearDead = 1;

        private IEnumerator targetCoroutine;
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
        public void DrawCharacter(CharacterData characterData)
        {
            textCharacterName.text = characterData.CharacterName;
            if (characterData.CharacterFace == null)
                characterFace.enabled = false;
            else
                characterFace.enabled = true;
            characterFace.sprite = characterData.CharacterFace;
            characterFaceOutline.sprite = characterData.CharacterFace;

            int healthBarNumber = ((int)characterData.CharacterStat.HpMax / healthBarAmount);
            healthBarNumber = Mathf.Max(0, healthBarNumber);
            for (int i = 0; i < healthBarNumber; i++)
            {
                if (healthBarList.Count <= i)
                    healthBarList.Add(Instantiate(transformBar, transformBarNumber));
                healthBarList[i].gameObject.SetActive(true);
            }
            for (int i = healthBarNumber; i < healthBarList.Count; i++)
            {
                healthBarList[i].gameObject.SetActive(false);
            }
            if (healthBarList.Count == 0)
                transformBarNumber.gameObject.SetActive(false);
            DrawHealth((int)characterData.CharacterStat.Hp, (int)characterData.CharacterStat.HpMax);
        }

        /*public void DrawCharacter(Character characterData)
        {
            textCharacterName.text = characterData.CharacterName;
            if (characterData.CharacterFace == null)
                characterFace.enabled = false;
            else
                characterFace.enabled = true;
            characterFace.sprite = characterData.CharacterFace;
            characterFaceOutline.sprite = characterData.CharacterFace;

            int healthBarNumber = ((int)characterData.CharacterStat.HpMax / 1000);
            healthBarNumber = Mathf.Max(0, healthBarNumber);
            for (int i = 0; i < healthBarNumber; i++)
            {
                if (healthBarList.Count <= i)
                    healthBarList.Add(Instantiate(transformBar, transformBarNumber));
                healthBarList[i].gameObject.SetActive(true);
            }
            for (int i = healthBarNumber; i < healthBarList.Count; i++)
            {
                healthBarList[i].gameObject.SetActive(false);
            }
            if (healthBarList.Count == 0)
                transformBarNumber.gameObject.SetActive(false);
            DrawHealth((int)characterData.CharacterStat.Hp, (int)characterData.CharacterStat.HpMax);
        }*/

        public void DrawCharacter(PlayerData characterData, CharacterStatController characterStat)
        {
            textCharacterName.text = characterData.CharacterName;
            if (characterData.CharacterFace == null)
                characterFace.enabled = false;
            else
                characterFace.enabled = true;
            characterFace.sprite = characterData.CharacterFace;
            characterFaceOutline.sprite = characterData.CharacterFace;

            int healthBarNumber = (characterStat.GetHPMax() / healthBarAmount);
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
                currentHealth += healthBarAmount;
                if (hp >= currentHealth)
                    healthBarList[i].enabled = true;
                else
                    healthBarList[i].enabled = false;
            }
        }





        public void DrawTarget(int hp, int maxHp, string targetName)
        {
            if (targetCoroutine != null)
                StopCoroutine(targetCoroutine);
            targetTransform.gameObject.SetActive(true);
            targetHealthGauge.DrawGauge(hp % targetHealthBarAmount, Mathf.Min(maxHp, targetHealthBarAmount));
            textTargetName.text = targetName;
            if(hp <= 0)
            {
                targetCoroutine = TargetDisappearCoroutine(timeTargetDisappearDead);
            }
            else
            {
                targetCoroutine = TargetDisappearCoroutine(timeTargetDisappearNormal);
            }
            StartCoroutine(targetCoroutine);

        }

        private IEnumerator TargetDisappearCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            targetTransform.gameObject.SetActive(false);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace