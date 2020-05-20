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
    public class AttackBehavior
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Title("Attack Parameter")]
        [SerializeField]
        private string attackName;

        public string AttackName
        {
            get { return attackName; }
        }

        [SerializeField]
        private int attackDamage;
        public int AttackDamage
        {
            get { return attackDamage; }
        }

        [SerializeField]
        private string attackAnimation;
        public string AttackAnimation
        {
            get { return attackAnimation; }
        }


        [SerializeField]
        private float lifetime;
        public float Lifetime
        {
            get { return lifetime; }
        }


        [SerializeField]
        private AttackController comboTo;
        public AttackController ComboTo
        {
            get { return comboTo; }
        }

        [SerializeField]
        private AttackController targetCombo;
        public AttackController TargetCombo
        {
            get { return targetCombo; }
        }

        [SerializeField]
        private AttackController onHitCombo;
        public AttackController OnHitCombo
        {
            get { return onHitCombo; }
        }


        [Title("Attack Parameter")]
        [SerializeField]
        bool animationDriven = true;
        public bool AnimationDriven
        {
            get { return animationDriven; }
        }

        [HorizontalGroup("AttackParameter1")]
        [SerializeField]
        bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
        }

        [HorizontalGroup("AttackParameter1")]
        [SerializeField]
        bool allyAttack = false;
        public bool AllyAttack
        {
            get { return allyAttack; }
        }

        [HorizontalGroup("AttackParameter2")]
        [SerializeField]
        bool isPiercing = false;
        public bool IsPiercing
        {
            get { return isPiercing; }
        }

        [HorizontalGroup("AttackParameter2")]
        [SerializeField]
        bool isMultiHit = false;
        public bool IsMultiHit
        {
            get { return isMultiHit; }
        }


        [HorizontalGroup("AttackParameter3")]
        [SerializeField]
        bool linkToCharacter = false;
        public bool LinkToCharacter
        {
            get { return linkToCharacter; }
        }

        [HorizontalGroup("AttackParameter3")]
        [SerializeField]
        bool noDirection = false;
        public bool NoDirection
        {
            get { return noDirection; }
        }


        [HorizontalGroup("AttackParameter4")]
        [SerializeField]
        bool moveToTarget = false;
        public bool MoveToTarget
        {
            get { return moveToTarget; }
        }

        [HorizontalGroup("AttackParameter4")]
        [SerializeField]
        bool cameraFollowAttack = false;
        public bool CameraFollowAttack
        {
            get { return cameraFollowAttack; }
        }

        [SerializeField]
        bool guardBreak = false;
        public bool GuardBreak
        {
            get { return guardBreak; }
        }

        [SerializeField]
        bool wakeUpAttack = false;
        public bool WakeUpAttack
        {
            get { return wakeUpAttack; }
        }

        [SerializeField]
        bool jumpCancel = false;
        public bool JumpCancel
        {
            get { return jumpCancel; }
        }

        [SerializeField]
        bool dashCancel = false;
        public bool DashCancel
        {
            get { return dashCancel; }
        }

        [SerializeField]
        bool throwState = false;
        public bool ThrowState
        {
            get { return throwState; }
        }

        [SerializeField]
        bool attackThrow = false;
        public bool AttackThrow
        {
            get { return attackThrow; }
        }

        [SerializeField]
        bool resetGravity = false;
        public bool ResetGravity
        {
            get { return resetGravity; }
        }


        [Title("Feedback")]
        [SerializeField]
        private GameObject onHitAnimation;
        public GameObject OnHitAnimation
        {
            get { return onHitAnimation; }
        }


        [HorizontalGroup("Knockback")]
        [SerializeField]
        float knockbackPowerX = 1;
        public float KnockbackPowerX
        {
            get { return knockbackPowerX; }
        }
        [HorizontalGroup("Knockback")]
        [SerializeField]
        float knockbackPowerZ = 0;
        public float KnockbackPowerZ
        {
            get { return knockbackPowerZ; }
        }

        [HorizontalGroup("KnockbackAerial")]
        [SerializeField]
        float knockbackAerialPowerX = 1;
        public float KnockbackAerialPowerX
        {
            get { return knockbackAerialPowerX; }
        }
        [HorizontalGroup("KnockbackAerial")]
        [SerializeField]
        float knockbackAerialPowerZ = 0;
        public float KnockbackAerialPowerZ
        {
            get { return knockbackAerialPowerZ; }
        }

        [HorizontalGroup("Knockback2")]
        [SerializeField]
        float knockbackDurationMultiplier = 1;
        public float KnockbackDurationMultiplier
        {
            get { return knockbackDurationMultiplier; }
        }


        [HorizontalGroup("FeedbackPuissant")]
        [SerializeField]
        private float hitStop = 0.15f;
        public float HitStop
        {
            get { return hitStop; }
        }
        [HorizontalGroup("FeedbackPuissant")]
        [SerializeField]
        private float zoom;
        public float Zoom
        {
            get { return zoom; }
        }

        [HorizontalGroup("TargetShake")]
        [SerializeField]
        private float targetShakePower;
        public float TargetShakePower
        {
            get { return targetShakePower; }
        }

        [HorizontalGroup("Shake")]
        [SerializeField]
        private float shakeScreen;
        public float ShakeScreen
        {
            get { return shakeScreen; }
        }
        [HorizontalGroup("Shake")]
        [SerializeField]
        private float shakeScreenTime;
        public float ShakeScreenTime
        {
            get { return shakeScreenTime; }
        }


        [HorizontalGroup("Blink")]
        [SerializeField]
        private float blinkTarget;
        public float BlinkTarget
        {
            get { return blinkTarget; }
        }
        [HorizontalGroup("Blink")]
        [SerializeField]
        [HideLabel]
        private Color blinkTargetColor;
        public Color BlinkTargetColor
        {
            get { return blinkTargetColor; }
        }



        [Title("Attack Behavior")]
        [SerializeField]
        AttackBehaviorData[] attackBehaviorDatas;
        public AttackBehaviorData[] AttackBehaviorDatas
        {
            get { return attackBehaviorDatas; }
        }

        /*[SerializeField]
        AttackController complexBehavior;
        public AttackController ComplexBehavior
        {
            get { return complexBehavior; }
        }*/


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


        #endregion

    } 

    [System.Serializable]
    public class AttackBehaviorData
    {
        [HorizontalGroup("AttackData", LabelWidth = 50)]
        [SerializeField]
        public int frame;

        [HorizontalGroup("AttackData")]
        [SerializeField]
        [HideLabel]
        public AttackBehaviorAction action;

        [HorizontalGroup("AttackData")]
        [SerializeField]
        [HideLabel]
        public string argument;

        [HorizontalGroup("AttackData")]
        [SerializeField]
        [HideLabel]
        public Vector3 argument1;

        [HorizontalGroup("AttackData")]
        [SerializeField]
        [HideLabel]
        public GameObject anim;

        [HorizontalGroup("AttackData")]
        [SerializeField]
        [HideLabel]
        public AttackController move;


    }

    public enum AttackBehaviorAction
    {
        None,
        Active,
        MoveForward,
        MoveTo,
        PlayAnimation
    }

} // #PROJECTNAME# namespace