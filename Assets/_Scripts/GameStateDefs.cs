using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ReplicantPackage
{
    /// <summary>
    /// OWNER: Spencer Martin
    /// Defines an enum for game states - can be added to but please don't fuck with
    /// </summary>
	public class Game
	{
		public enum State { idle, loading, loaded, gameStarting, gameStarted, levelStarting, levelStarted, gamePlaying, levelEnding, levelEnded, gameEnding, gameEnded, gamePausing, gameUnPausing, showingLevelResults, showingGameResults, restartingLevel, restartingGame };
	}
}
