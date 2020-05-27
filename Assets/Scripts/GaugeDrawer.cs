/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class GaugeDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Image gaugeImage;
        [SerializeField]
        RectTransform gaugeTransform;
        [SerializeField]
        TextMeshProUGUI textGaugeValue;

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
        public void DrawGauge(float amount, float maxAmount)
        {
            if(textGaugeValue != null)
                textGaugeValue.text = amount.ToString();
            gaugeTransform.localScale = new Vector3(amount / maxAmount, gaugeTransform.localScale.y, gaugeTransform.localScale.z);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace