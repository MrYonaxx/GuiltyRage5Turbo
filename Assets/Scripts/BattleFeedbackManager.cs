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
    public class BattleFeedbackManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        Camera cameraMain;
        [SerializeField]
        CameraBattleController cameraController;
        [SerializeField]
        Shake cameraShake;
        [SerializeField]
        Animator zoom;
        [SerializeField]
        RippleEffect rippleEffect;

        [Title("Animation")]
        [SerializeField]
        GameObject animationHit;
        [SerializeField]
        GameObject animationDeath;
        [SerializeField]
        GameObject animationGuard;


        [Title("CardBreakAnimation")]
        [SerializeField]
        GameObject cardBreakAnimation;
        [SerializeField]
        Animator animatorPixelize;
        [SerializeField]
        Animator animatorTextCardBreak;


        [SerializeField]
        List<Character> battleCharacters;

        private IEnumerator motionSpeedCoroutine;

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

        public void SetBattleCharacters(List<Character> characters)
        {
            battleCharacters = characters;
        }

        public void Guard(Character character)
        {
            Instantiate(animationGuard, character.ParticlePoint.position, Quaternion.identity);
        }

        public void ApplyFeedback(AttackBehavior attack)
        {
            if(attack.HitStop > 0)
            {
                SetBattleMotionSpeed(0, attack.HitStop);
            }
            if (attack.ShakeScreen > 0)
            {
                cameraShake.ShakeEffect(attack.ShakeScreen, (int)attack.ShakeScreenTime);
            }
            if (attack.Zoom > 0)
            {
                zoom.SetTrigger("Zoom");
            }
        }

        public void AnimationDeath(Character character)
        {
            Instantiate(animationDeath, character.ParticlePoint.position, Quaternion.identity);
            cameraShake.ShakeEffect(0.2f, 20);
            zoom.SetTrigger("Zoom");
            SetBattleMotionSpeed(0, 0.6f);
        }

        public void AnimationWallBounce(Character character)
        {
            Instantiate(animationHit, character.ParticlePoint.position, Quaternion.identity);
            cameraShake.ShakeEffect(0.1f, 10);
        }


        public void EndBattleMotionSpeed()
        {
            cameraShake.ShakeEffect(0.2f, 25);
            zoom.SetTrigger("BigZoom");
            for (int i = 0; i < battleCharacters.Count; i++)
            {
                battleCharacters[i].SetCharacterMotionSpeed(0);
            }

            if (motionSpeedCoroutine != null)
                StopCoroutine(motionSpeedCoroutine);
            motionSpeedCoroutine = EndBattleCoroutine(0.6f);
            StartCoroutine(motionSpeedCoroutine);
        }
        private IEnumerator EndBattleCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            SetBattleMotionSpeed(0.2f, 2f);
        }






        public void SetBattleMotionSpeed(float motionSpeed, float time)
        {
            for(int i = 0; i < battleCharacters.Count; i++)
            {
                battleCharacters[i].SetCharacterMotionSpeed(motionSpeed);
            }

            if (motionSpeedCoroutine != null)
                StopCoroutine(motionSpeedCoroutine);
            motionSpeedCoroutine = MotionSpeedCoroutine(time);
            StartCoroutine(motionSpeedCoroutine);
        }

        private IEnumerator MotionSpeedCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            for (int i = 0; i < battleCharacters.Count; i++)
            {
                battleCharacters[i].SetCharacterMotionSpeed(1);
            }
        }


        public void RippleEffect(float x, float y)
        {
            Vector3 pos = cameraMain.WorldToViewportPoint(new Vector3(x, y, y));
            rippleEffect.EmitRipple(pos.x, pos.y);
        }

        public void CameraShake()
        {
            cameraShake.ShakeEffect();
        }



        public void AnimationCardBreak(Character currentCharacter)
        {
            SetBattleMotionSpeed(0, 0.5f);
            CameraShake();
            RippleEffect(currentCharacter.transform.position.x, currentCharacter.transform.position.y);
            animatorTextCardBreak.transform.position = currentCharacter.transform.position;
            animatorTextCardBreak.SetTrigger("Feedback");
            Instantiate(cardBreakAnimation, currentCharacter.ParticlePoint.position, Quaternion.identity);
            animatorPixelize.gameObject.SetActive(true);
            animatorPixelize.SetTrigger("CardBreak");
        }

        #endregion

    } 

} // #PROJECTNAME# namespace