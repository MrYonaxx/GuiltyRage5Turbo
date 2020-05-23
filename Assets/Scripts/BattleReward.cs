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
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class BattleReward: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GameRunData gameRunData;

        [Title("Parameter")]
        [SerializeField]
        int rewardNumber = 3;
        [SerializeField]
        ButtonHoldController[] buttonHoldControllers;
        [SerializeField]
        InputController inputController;

        [Title("Animator")]
        [SerializeField]
        PlayerController playerGetCard;
        [SerializeField]
        Canvas canvasReward;
        [SerializeField]
        Animator animatorBattleWin;
        [SerializeField]
        Animator animatorCardControllers;

        [SerializeField]
        UnityEvent OnEventEnd;


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

        public void InitializeBattleReward()
        {
            for (int i = 0; i < buttonHoldControllers.Length; i++)
            {
                buttonHoldControllers[i].ResetButton();
            }
            canvasReward.gameObject.SetActive(true);
            animatorBattleWin.gameObject.SetActive(true);
            animatorBattleWin.SetBool("Appear", true);
            CreateRewards();
            StartCoroutine(BattleRewardCoroutine());
        }

        private void CreateRewards()
        {

        }

        private IEnumerator BattleRewardCoroutine()
        {
            yield return new WaitForSeconds(1f);
            animatorCardControllers.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            inputController.gameObject.SetActive(true);
        }

        public void HoldButton(int index)
        {
            for(int i = 0; i < buttonHoldControllers.Length; i++)
            {
                if (i == index)
                    buttonHoldControllers[i].HoldButton();
                else
                    buttonHoldControllers[i].ResetButton();
            }         
        }

        public void AddReward(int index)
        {
            inputController.gameObject.SetActive(false);
            if (index > rewardNumber)
            {
                // Skip
            }
            else
            {
                animatorBattleWin.SetTrigger("Feedback");
                animatorCardControllers.SetInteger("Reward", index+1);
            }
        }

        // Appelé à la fin de l'anim de animatorCardControllers
        public void AnimationGetoDaze()
        {
            playerGetCard.PlayGetCardAnimation();
            animatorBattleWin.SetBool("Appear", false);
            animatorCardControllers.SetInteger("Reward", 0);
            animatorCardControllers.gameObject.SetActive(false);
            StartCoroutine(BattleRewardEndCoroutine());
        }

        private IEnumerator BattleRewardEndCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            //playerGetCard.SetCanInput(true);
            animatorBattleWin.gameObject.SetActive(false);
            canvasReward.gameObject.SetActive(false);
            OnEventEnd.Invoke();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace