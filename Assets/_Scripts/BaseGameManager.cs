using UnityEngine;
using UnityEngine.Events;

namespace ReplicantPackage
{
	/// <summary>
    /// OWNER: Spencer Martin
    /// Tracks game states, including core states i.e.: game running, game paused, loading, game ended etc.
    /// </summary>

	public class BaseGameManager : MonoBehaviour
	{
		public Game.State currentGameState;
		public Game.State targetGameState;
		public Game.State lastGameState;

		private bool paused;

		public void SetTargetState(Game.State aState)
		{
			targetGameState = aState;

			if (targetGameState != currentGameState)
				UpdateTargetState();
		}

		public Game.State GetCurrentState()
		{
			return currentGameState;
		}

		[Header("Game Events")]

		public UnityEvent OnLoaded;

		public UnityEvent OnGameStarting;
		public UnityEvent OnGameStarted;

		public UnityEvent OnGameEnding;
		public UnityEvent OnGameEnded;
		public UnityEvent OnGamePause;
		public UnityEvent OnGameUnPause;
		public UnityEvent OnRestartGame;

		public virtual void Loaded() { OnLoaded.Invoke(); }
		public virtual void GameStarting() { OnGameStarting.Invoke(); }
		public virtual void GameStarted() { OnGameStarted.Invoke(); }
		public virtual void GameEnding() { OnGameEnding.Invoke(); }
		public virtual void GameEnded() { OnGameEnded.Invoke(); }
		public virtual void GamePause() { OnGamePause.Invoke(); }
		public virtual void GameUnPause() { OnGameUnPause.Invoke(); }
		public virtual void RestartGame() { OnRestartGame.Invoke(); }

		public virtual void UpdateTargetState()
		{
			// we will never need to run target state functions if we're already in this state, so we check for that and drop out if needed
			if (targetGameState == currentGameState)
				return;

			switch (targetGameState)
			{
				case Game.State.idle:
					break;

				case Game.State.loading:
					break;

				case Game.State.loaded:
					Loaded();
					break;

				case Game.State.gameStarting:
					GameStarting();
					break;

				case Game.State.gameStarted:
					GameStarted();
					break;

				case Game.State.gamePlaying:
					break;


				case Game.State.gameEnding:
					GameEnding();
					break;

				case Game.State.gameEnded:
					GameEnded();
					break;

				case Game.State.gamePausing:
					GamePause();
					break;

				case Game.State.gameUnPausing:
					GameUnPause();
					break;


				case Game.State.restartingGame:
					RestartGame();
					break;
			}

			// now update the current state to reflect the change
			currentGameState = targetGameState;
		}

		public virtual void UpdateCurrentState()
		{
			switch (currentGameState)
			{
				case Game.State.idle:
					break;

				case Game.State.loading:
					break;

				case Game.State.loaded:
					break;

				case Game.State.gameStarting:
					break;

				case Game.State.gameStarted:
					break;

				case Game.State.gamePlaying:
					break;


				case Game.State.gameEnding:
					break;

				case Game.State.gameEnded:
					break;

				case Game.State.gamePausing:
					break;

				case Game.State.gameUnPausing:
					break;

				case Game.State.restartingGame:
					break;

			}
		}

		public virtual void GamePaused()
		{
			OnGamePause.Invoke();
			Paused = true;
		}

		public virtual void GameUnPaused()
		{
			OnGameUnPause.Invoke();
			Paused = false;
		}

		public bool Paused
		{
			get
			{
				// get paused
				return paused;
			}
			set
			{
				// set paused 
				paused = value;

				if (paused)
				{
					// pause time
					Time.timeScale = 0f;
				}
				else
				{
					// unpause Unity
					Time.timeScale = 1f;
				}
			}
		}

	}
}
