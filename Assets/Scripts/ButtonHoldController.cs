/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ButtonHoldController : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Image imageAmount;

        [SerializeField]
        float maxAmount = 0.6f;
        [SerializeField]
        float amountReduction = 0.1f;

        [SerializeField]
        UnityEvent eventButton;

        bool active = true;
        float currentAmount = 0;

        private IEnumerator holdAmountCoroutine = null;

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

        public void ResetButton()
        {
            active = false;
            currentAmount = 0;
            imageAmount.fillAmount = currentAmount / maxAmount;
        }

        public void HoldButton()
        {
            active = true;
            currentAmount += Time.deltaTime;
            imageAmount.fillAmount = currentAmount / maxAmount;
            if (currentAmount >= maxAmount)
            {
                active = false;
                eventButton.Invoke();
                if (holdAmountCoroutine != null)
                    StopCoroutine(holdAmountCoroutine);
            }
            else
            {
                if (holdAmountCoroutine != null)
                    StopCoroutine(holdAmountCoroutine);
                holdAmountCoroutine = ReduceHoldAmountCoroutine();
                StartCoroutine(holdAmountCoroutine);
            }
        }

        private IEnumerator ReduceHoldAmountCoroutine()
        {
            while(currentAmount > 0)
            {
                if (active == true)
                {
                    active = false;
                    yield return null;
                    yield return null;
                }
                else
                {
                    currentAmount -= amountReduction;
                    imageAmount.fillAmount = currentAmount / maxAmount;
                }
                yield return null;
            }
            currentAmount = 0;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace