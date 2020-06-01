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
    public class TargetController : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SpriteRenderer spriteDirection;

        [SerializeField]
        string targetsTag = "Player";

        [Title("Targeting Logic")]
        [SerializeField]
        bool getNearest = true;
        [SerializeField]
        bool getRandom = false;
        [SerializeField]
        bool getFurthest = false;
        [SerializeField]
        List<CharacterState> bannedStates = new List<CharacterState>();


        [SerializeField]
        Transform lockMarker;

        /*[SerializeField]
        UnityEventCharacterBattle OnEventTarget;*/

        List<Character> transformsLocked = new List<Character>();

        bool targetInList = false;

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
        private void Update()
        {
            if (spriteDirection.flipX == true)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (spriteDirection.flipX == false)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == targetsTag)
            {
                transformsLocked.Add(collision.GetComponent<Character>());
                if (targetInList == false)
                {
                    //this.transform.localScale = lockOnSize;
                    targetInList = true;
                    //StartCoroutine(CheckShortestCoroutine());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == targetsTag)
            {
                transformsLocked.Remove(collision.GetComponent<Character>());
                if (transformsLocked.Count == 0)
                {
                    targetInList = false;
                    //this.transform.localScale = defaultSize;
                }
            }
        }

        private IEnumerator CheckShortestCoroutine()
        {
            while (targetInList == true)
            {
                float bestLength = 999;
                int bestIndex = 0;
                for (int i = 0; i < transformsLocked.Count; i++)
                {
                    float length = Vector3.Magnitude(transformsLocked[i].transform.position - transform.position);
                    if (bestLength >= length)
                    {
                        bestLength = length;
                        bestIndex = i;
                    }
                }
                //OnEventTarget.Invoke(transformsLocked[bestIndex]);
                lockMarker.gameObject.SetActive(true);
                lockMarker.transform.SetParent(transformsLocked[bestIndex].transform);
                lockMarker.transform.localPosition = new Vector3(0, 0.48f, 0);
                lockMarker.transform.localScale = Vector3.one;
                yield return null;
            }
            //OnEventTarget.Invoke(null);
            lockMarker.gameObject.SetActive(false);
            lockMarker.transform.SetParent(this.transform);
        }




        public Character GetTarget()
        {
            List<Character> targets = new List<Character>();
            for (int i = 0; i < transformsLocked.Count; i++)
            {
                if (CheckBannedStates(transformsLocked[i]) == true)
                {
                    targets.Add(transformsLocked[i]);
                }
            }
            if (targets.Count == 0)
                return GetTarget(transformsLocked);
            return GetTarget(targets);
        }

        public Character GetTarget(List<Character> characters)
        {
            int res = 0;

            if (getRandom == true)
                res = GetRandom(characters);
            if (getNearest == true)
                res = GetNearest(characters);

            Debug.Log(characters[res].name);
            return characters[res];
        }


        public int GetNearest(List<Character> characters)
        {
            int bestIndex = 0;
            float bestLength = 999999;
            for (int i = 0; i < characters.Count; i++)
            {
                float length = Vector3.Magnitude(characters[i].transform.position - transform.position);
                if (bestLength >= length)
                {
                    bestLength = length;
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        public int GetRandom(List<Character> characters)
        {
            return Random.Range(0, characters.Count);
        }

        public bool CheckBannedStates(Character target)
        {
            for(int i = 0; i < bannedStates.Count; i++)
            {
                if (target.State == bannedStates[i])
                    return false;
            }
            return true;
        }

        #endregion

    }

} // #PROJECTNAME# namespace
