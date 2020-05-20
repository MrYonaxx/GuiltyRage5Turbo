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
    // Data de la run actuelle
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 1)]
    public class GameRunData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        private PlayerData playerCharacterData;
        public PlayerData PlayerCharacterData
        {
            get { return playerCharacterData; }
        }


        [SerializeField]
        CharacterStatController playerStats;
        public CharacterStatController PlayerStats
        {
            get { return playerStats; }
        }

        [SerializeField]
        int floor;
        public int Floor
        {
            get { return floor; }
        }






        [SerializeField]
        int money;

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
        public void CreateRunData(PlayerData playerInitialData)
        {
            playerCharacterData = playerInitialData;
            playerStats.CreateStatController(playerInitialData);

            floor = 0;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace