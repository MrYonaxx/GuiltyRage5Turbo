using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing 
{
    public interface ICharacterInfoDrawer
    {
        void DrawCharacter(PlayerData character);

        void DrawHealth(int hp, int maxHp);

        void DrawTarget(int hp, int maxHp, string targetName);
    }
}
