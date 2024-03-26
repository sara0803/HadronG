using System;
using System.Collections;
using GeneralsMiniGames;
using MiniGameTest.Challenges;
using UnityEngine;

namespace MiniGameTest.Minigame
{
    public class LoopMiniGame01 : LoopMiniGame
    {
        protected override IEnumerator CouLoopMiniGame()
        {
            var tmpChallengeSelected = SelectChallenge.Instance.RefChallengeSetupSelected as Challenge01;

            yield return new WaitUntil(
                () => tmpChallengeSelected
                    ? tmpChallengeSelected.GallinasQuantity == tmpChallengeSelected.CurrentChallange.gallinasAmount
                    : throw new Exception("No Challenge")
                );
            
            MiniGameFinished();
        }
    }
}