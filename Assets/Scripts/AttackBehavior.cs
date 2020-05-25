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

        [HorizontalGroup("Damage")]
        [SerializeField]
        private int attackDamage;
        public int AttackDamage
        {
            get { return attackDamage; }
        }
        [HorizontalGroup("Damage")]
        [SerializeField]
        private int comboCost;
        public int ComboCost
        {
            get { return comboCost; }
        }

        [HorizontalGroup("KnockbackValue")]
        [SerializeField]
        float knockbackDurationBonus = 0;
        public float KnockbackDurationBonus
        {
            get { return knockbackDurationBonus; }
        }
        [HorizontalGroup("KnockbackValue")]
        [SerializeField]
        float knockdownValue = 0;
        public float KnockdownValue
        {
            get { return knockdownValue; }
        }

        [Space]
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

        [HorizontalGroup("CancelGround")]
        [SerializeField]
        bool cancelOnGround = false;
        public bool CancelOnGround
        {
            get { return cancelOnGround; }
        }
        [HorizontalGroup("CancelGround")]
        [ShowIf("cancelOnGround")]
        [LabelWidth(100)]
        [SerializeField]
        private AttackController onGroundCombo;
        public AttackController OnGroundCombo
        {
            get { return onGroundCombo; }
        }

        [HorizontalGroup("CancelWall")]
        [SerializeField]
        bool cancelOnWall = false;
        public bool CancelOnWall
        {
            get { return cancelOnWall; }
        }
        [HorizontalGroup("CancelWall")]
        [ShowIf("cancelOnWall")]
        [LabelWidth(100)]
        [SerializeField]
        private AttackController onWallCombo;
        public AttackController OnWallCombo
        {
            get { return onWallCombo; }
        }


        [Title("Attack Parameter")]
        /*[SerializeField]
        bool animationDriven = true;
        public bool AnimationDriven
        {
            get { return animationDriven; }
        }*/

        [SerializeField]
        bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
        }


        /*[HorizontalGroup("AttackParameter2")]
        [SerializeField]
        bool isPiercing = false;
        public bool IsPiercing
        {
            get { return isPiercing; }
        }*/

        [HorizontalGroup("AttackParameter2")]
        [SerializeField]
        bool isMultiHit = false;
        public bool IsMultiHit
        {
            get { return isMultiHit; }
        }

        [HorizontalGroup("AttackParameter2")]
        [SerializeField]
        bool noDirection = false;
        public bool NoDirection
        {
            get { return noDirection; }
        }


        [HorizontalGroup("AttackParameter3")]
        [SerializeField]
        bool linkToCharacter = true;
        public bool LinkToCharacter
        {
            get { return linkToCharacter; }
        }
        [HorizontalGroup("AttackParameter3")]
        [SerializeField]
        bool linkToCharacterAerial = true;
        public bool LinkToCharacterAerial
        {
            get { return linkToCharacterAerial; }
        }



        /*[HorizontalGroup("AttackParameter4")]
        [SerializeField]
        bool cameraFollowAttack = false;
        public bool CameraFollowAttack
        {
            get { return cameraFollowAttack; }
        }*/

        [Space]
        [HorizontalGroup("AttackParameter5")]
        [SerializeField]
        bool guardBreak = false;
        public bool GuardBreak
        {
            get { return guardBreak; }
        }

        [Space]
        [HorizontalGroup("AttackParameter5")]
        [SerializeField]
        bool wakeUpAttack = false;
        public bool WakeUpAttack
        {
            get { return wakeUpAttack; }
        }

        [HorizontalGroup("AttackParameter7")]
        [SerializeField]
        bool throwState = false;
        public bool ThrowState
        {
            get { return throwState; }
        }
        [HorizontalGroup("AttackParameter7")]
        [SerializeField]
        bool attackThrow = false;
        public bool AttackThrow
        {
            get { return attackThrow; }
        }

        [Space]
        [HorizontalGroup("AttackParameter11")]
        [SerializeField]
        bool cancelOnlyOnHit = false;
        public bool CancelOnlyOnHit
        {
            get { return cancelOnlyOnHit; }
        }

        [Space]
        [HorizontalGroup("AttackParameter11")]
        [SerializeField]
        bool keepMomentum = false;
        public bool KeepMomentum
        {
            get { return keepMomentum; }
        }

        [HorizontalGroup("AttackParameter6")]
        [SerializeField]
        bool jumpCancel = false;
        public bool JumpCancel
        {
            get { return jumpCancel; }
        }

        [HorizontalGroup("AttackParameter6")]
        [SerializeField]
        bool dashCancel = false;
        public bool DashCancel
        {
            get { return dashCancel; }
        }

        [HorizontalGroup("AttackParameter10")]
        [SerializeField]
        bool runCancel = false;
        public bool RunCancel
        {
            get { return runCancel; }
        }

        [HorizontalGroup("AttackParameter10")]
        [SerializeField]
        bool specialCancel = false;
        public bool SpecialCancel
        {
            get { return specialCancel; }
        }




        [Space]
        [HorizontalGroup("AttackParameter8")]
        [SerializeField]
        bool resetGravity = false;
        public bool ResetGravity
        {
            get { return resetGravity; }
        }

        /*[Space]
        [HorizontalGroup("AttackParameter8")]
        [SerializeField]
        bool moveToTarget = false;
        public bool MoveToTarget
        {
            get { return moveToTarget; }
        }*/

        [Space]
        [HorizontalGroup("AttackParameter8")]
        [SerializeField]
        bool allyAttack = false;
        public bool AllyAttack
        {
            get { return allyAttack; }
        }
        /*[HorizontalGroup("AttackParameter9")]
        [SerializeField]
        bool cameraFollowAttack = false;
        public bool CameraFollowAttack
        {
            get { return cameraFollowAttack; }
        }*/


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

        [Space]
        [HorizontalGroup("Block")]
        [SerializeField]
        float blockStun = 0.1f;
        public float BlockStun
        {
            get { return blockStun; }
        }
        [Space]
        [HorizontalGroup("Block")]
        [SerializeField]
        float blockKnockback = 0;
        public float BlockKnockback
        {
            get { return blockKnockback; }
        }

        [SerializeField]
        float onHitKnockback = 0;
        public float OnHitKnockback
        {
            get { return onHitKnockback; }
        }


        [Space]
        [HorizontalGroup("FeedbackPuissant")]
        [SerializeField]
        private float hitStop = 0.15f;
        public float HitStop
        {
            get { return hitStop; }
        }


        [Space]
        [HorizontalGroup("FeedbackPuissant")]
        [SerializeField]
        private bool hitStopGlobal;
        public bool HitStopGlobal
        {
            get { return hitStopGlobal; }
        }

        [HorizontalGroup("Zoom")]
        [SerializeField]
        private float targetShakePower;
        public float TargetShakePower
        {
            get { return targetShakePower; }
        }
        [HorizontalGroup("Zoom")]
        [SerializeField]
        private float zoom;
        public float Zoom
        {
            get { return zoom; }
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


        /*[HorizontalGroup("Blink")]
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
        }*/



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