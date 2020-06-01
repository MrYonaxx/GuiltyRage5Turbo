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
using Sirenix.Serialization;
using UnityEngine.Events;

namespace VoiceActing
{
    public enum Logique
    {
        AND,
        OR
    }

    [System.Serializable]
    public struct EnemyPatternSequence
    {
        [VerticalGroup(PaddingTop = 20)]
        [SerializeField]
        [HideLabel]
        public string SequenceID;

        [VerticalGroup("Pattern", PaddingBottom = 30)]
        [OdinSerialize]
        [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public List<EnemyBehavior> behaviours;
    }

    [System.Serializable]
    public struct EnemyPattern
    {

        [VerticalGroup(PaddingTop = 20)]
        [SerializeField]
        [HideLabel]
        public string PatternID;

        [SerializeField]
        [VerticalGroup("Pattern", PaddingBottom = 30)]
        [HorizontalGroup("Pattern/Right", LabelWidth = 100, Width = 200)]
        [HideLabel]
        //[ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public EnemyCondition conditions;

        [OdinSerialize]
        [HorizontalGroup("Pattern/Right")]
        [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
        public List<EnemyBehavior> behaviours;

    }

    [System.Serializable]
    public struct EnemyCondition
    {

        [SerializeField]
        public int Phase;

        [SerializeField]
        public Vector3 Distance;

        [ListDrawerSettings(Expanded = true)]
        [SerializeField]
        public List<CharacterState> targetStates;

        public bool CheckCondition(Character character, int characterPhase)
        {
            if (Phase > characterPhase)
                return false;

            Vector3 dist = character.transform.position - character.Target.transform.position;
            if(Distance.x != 0)
            {
                if (Mathf.Abs(dist.x) > Distance.x)
                    return false;
            }
            if (Distance.y != 0)
            {
                if (Mathf.Abs(dist.y) > Distance.y)
                    return false;
            }
            if (Distance.z != 0)
            {
                if (character.Target.SpriteRenderer.transform.localPosition.y > Distance.z)
                    return false;
            }

            if(targetStates == null)
            {
                return true;
            }
            for (int j = 0; j < targetStates.Count; j++)
            {
                if (targetStates[j] == character.Target.State)
                {
                    return true;
                }
            }


            return false;
        }

    }

    public struct EnemyHPTrigger
    {
        public float HpTrigger;
        public UnityEvent HPTriggerEvent;
    }


    public class EnemyController : SerializedMonoBehaviour, ICharacterController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Enemy")]
        [SerializeField]
        TargetController targetController;
        [SerializeField]
        float timeEnemyAppear = 0.5f;
        [SerializeField]
        float timeEnemyDisappear = 2f;

        [OdinSerialize]
        [TabGroup("Decision Tree")]
        [System.NonSerialized]
        [ListDrawerSettings(Expanded = true)]
        EnemyPattern[] patterns;

        [OdinSerialize]
        [TabGroup("Decision Tree Wakeup")]
        [System.NonSerialized]
        [ListDrawerSettings(Expanded = true)]
        EnemyPattern[] wakeUpBehaviors;

        [OdinSerialize]
        [TabGroup("Sequence")]
        [System.NonSerialized]
        EnemyPatternSequence[] behaviorSequences;

        [TabGroup("PhaseTrigger")]
        [SerializeField]
        EnemyHPTrigger[] phasesTrigger;

        [OdinSerialize]
        [System.NonSerialized]
        [SerializeField]
        EnemyBehavior startUpPattern;



        List<EnemyBehavior> currentPattern = new List<EnemyBehavior>();

        float enemyActionTime = 0;
        bool isHit = false;
        int phase = 1;

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

        private void Start()
        {
            currentPattern.Add(startUpPattern);
        }

        public void EnemyAppear(bool b)
        {
            if (b == true)
            {
                StartCoroutine(EnemyAppearCoroutine());
            }
            else
            {
                StartCoroutine(EnemyDisappearCoroutine());
            }
        }

        private IEnumerator EnemyAppearCoroutine()
        {
            Vector3 startPosition = new Vector3(1, 0, 1);
            Vector3 finalPosition = new Vector3(1, 1, 1);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / timeEnemyAppear;
                this.transform.localScale = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
        }

        private IEnumerator EnemyDisappearCoroutine()
        {
            Vector3 startPosition = new Vector3(1, 1, 1);
            Vector3 finalPosition = new Vector3(1, 0, 1);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / timeEnemyDisappear;
                this.transform.localScale = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
            Destroy(this.gameObject);
        }





        private bool CheckPhases(Character character)
        {
            if (phase - 1 < phasesTrigger.Length)
            {
                if (character.CharacterStat.GetHP() < phasesTrigger[phase - 1].HpTrigger)
                {
                    character.CancelAct();
                    phasesTrigger[phase - 1].HPTriggerEvent.Invoke();
                    phase += 1;
                    return true;
                }
            }
            return false;
        }


        public void UpdateController(Character character)
        {
            if(CheckPhases(character) == true)
                return;
            EnemyDecision(character);
        }




        public void EnemyDecision(Character character)
        {
            if (character.State == CharacterState.Hit || character.State == CharacterState.Down || character.State == CharacterState.Dead)
                return;
            //patterns
            if (currentPattern.Count == 0)
            {
                SelectTarget(character);
                SelectAction(patterns, character);
            }
            if (character.Target == null)
                character.SetTarget(character);
            if (enemyActionTime <= 0)
            {
                enemyActionTime = currentPattern[0].StartBehavior(this, character);
            }

            if(character.State != CharacterState.Acting)
                enemyActionTime -= Time.deltaTime;

            currentPattern[0].UpdateBehavior(this, character);
            if (enemyActionTime <= 0)
            {
                currentPattern[0].EndBehavior(this, character);
                currentPattern.RemoveAt(0);
            }
        }

        private void SelectAction(EnemyPattern[] enemyPatterns, Character character)
        {
            for(int i = 0; i < enemyPatterns.Length; i++)
            {
                if (enemyPatterns[i].conditions.CheckCondition(character, phase) == true)
                {
                    currentPattern.Add(enemyPatterns[i].behaviours[Random.Range(0, enemyPatterns[i].behaviours.Count)]);
                    return;
                }
            }
        }

        public void StopPattern()
        {
            enemyActionTime = 0;
        }

        public void ForcePattern(EnemyBehavior behavior, Character character)
        {
            currentPattern[0] = behavior;
            enemyActionTime = currentPattern[0].StartBehavior(this, character);

        }

        public void ForceBehavior(EnemyBehavior behavior, Character character)
        {
            currentPattern[0] = behavior;
            enemyActionTime = currentPattern[0].StartBehavior(this, character);
        }

        public void PlaySequence(string sequenceID)
        {
            for(int i = 0; i < behaviorSequences.Length; i++)
            {
                if(behaviorSequences[i].SequenceID == sequenceID)
                {
                    for(int j = 0; j < behaviorSequences[i].behaviours.Count; j++)
                    {
                        currentPattern.Add(behaviorSequences[i].behaviours[j]);
                    }
                    currentPattern.RemoveAt(0);
                    return;
                }
            }
        }




        public void LateUpdateController(Character character)
        {
            CheckInterrupt(character);
        }
        private void CheckInterrupt(Character character)
        {
            if (character.State == CharacterState.Hit)
            {
                currentPattern.Clear();
                isHit = true;
            }
            else if (isHit == true && character.State == CharacterState.Idle)
            {
                if (wakeUpBehaviors != null)
                {
                    SelectAction(wakeUpBehaviors, character);
                    enemyActionTime = currentPattern[0].StartBehavior(this, character);
                }
                isHit = false;
            }
        }



        public void SelectTarget(Character c)
        {
            c.SetTarget(targetController.GetTarget());
            if (c.Target == null)
                c.SetTarget(c);
        }
        #endregion

    } 

} // #PROJECTNAME# namespace