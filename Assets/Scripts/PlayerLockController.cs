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
    public class PlayerLockController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SpriteRenderer spriteDirection;
        [SerializeField]
        Vector3 lockOnSize;
        [SerializeField]
        Vector3 defaultSize;

        [SerializeField]
        Transform lockMarker;

        [SerializeField]
        UnityEventCharacterBattle OnEventTarget;

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
            if(collision.tag == "Enemy")
            {
                transformsLocked.Add(collision.GetComponent<Character>());
                if (targetInList == false)
                {
                    this.transform.localScale = lockOnSize;
                    targetInList = true;
                    StartCoroutine(CheckShortestCoroutine());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Enemy")
            {
                transformsLocked.Remove(collision.GetComponent<Character>());
                if (transformsLocked.Count == 0)
                {
                    targetInList = false;
                    this.transform.localScale = defaultSize;
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
                OnEventTarget.Invoke(transformsLocked[bestIndex]);
                lockMarker.gameObject.SetActive(true);
                lockMarker.transform.SetParent(transformsLocked[bestIndex].transform);
                lockMarker.transform.localPosition =new Vector3(0,0.48f,0);
                lockMarker.transform.localScale = Vector3.one;
                yield return null;
            }
            OnEventTarget.Invoke(null);
            lockMarker.gameObject.SetActive(false);
            lockMarker.transform.SetParent(this.transform);
        }
        #endregion

    } 

} // #PROJECTNAME# namespace