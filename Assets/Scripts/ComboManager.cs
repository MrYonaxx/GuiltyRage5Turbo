/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ComboManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        TextMeshProUGUI textComboCount;
        [SerializeField]
        Animator animatorCombo;
        [SerializeField]
        float comboTime = 1f;

        int comboCount = 0;
        private IEnumerator comboTimeCoroutine;

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
        public void AddCombo()
        {
            comboCount += 1;
            animatorCombo.SetTrigger("Feedback");
            textComboCount.text = comboCount.ToString();
            textComboCount.gameObject.SetActive(true);

            if (comboTimeCoroutine != null)
                StopCoroutine(comboTimeCoroutine);
            comboTimeCoroutine = ComboCoroutine();
            StartCoroutine(comboTimeCoroutine);
        }

        private IEnumerator ComboCoroutine()
        {
            yield return new WaitForSeconds(comboTime);
            comboCount = 0;
            textComboCount.gameObject.SetActive(false);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace