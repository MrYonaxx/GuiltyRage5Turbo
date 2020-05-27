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
    public class GameRunManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GameRunData runData;

        [Title("Player")]
        [SerializeField]
        List<Character> players;



        [Title("HealthBar")]
        [SerializeField]
        HealthBarDrawer healthBarPrefab;
        [SerializeField]
        Transform healthParent;

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
            CreatePlayer();
            CreateHealthBar();
        }

        public void CreatePlayer()
        {
            //runData.CreateRunData(playerData);
            //playerController.SetCharacter(runData.PlayerCharacterData, runData.PlayerDeck, runData.PlayerStats);
        }

        public void CreateHealthBar()
        {
            for(int i = 0; i < players.Count; i++)
            {
                var healthbar = Instantiate(healthBarPrefab, healthParent);
                players[i].SetHealthBar(healthbar);
            }
            healthBarPrefab.gameObject.SetActive(false);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace