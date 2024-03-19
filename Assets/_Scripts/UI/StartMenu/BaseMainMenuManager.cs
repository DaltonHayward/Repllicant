using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Owner: Spencer Martin
/// Main Menu manager
/// </summary>
namespace ReplicantPackage
{
	public class BaseMainMenuManager : MonoBehaviour
	{
		#region Variable Declarations
		public CanvasManager _canvasManager;
		//public AudioMixer _theAudioMixer;

		//public Slider _sfxSlider;
		//public Slider _musicSlider;

		public string gameSceneName; 

		#endregion
		void Start()
		{
			Init();
		}

		public virtual void Init()
		{
			//if (_canvasManager == null)
			//	Debug.LogError("You need to add a reference to a _canvasManager Component on your main menu Component!");

			// set the values of the UI (visible) sliders to those of the audio mixers
			//SetupSliders();

			Invoke("ShowMainMenu", 0.5f);
		}

		/* Need to set this up with FMOD
		void SetupSliders()
		{
			// now set the UI sliders to match the values in the mixer
			float value;
			_theAudioMixer.GetFloat("SoundVol", out value);
			_sfxSlider.value = DecibelToLinear(value);

			_theAudioMixer.GetFloat("MusicVol", out value);
			_musicSlider.value = DecibelToLinear(value);
		}

		public virtual void SetSFXVolumeLevel(float aValue)
		{
			float dbVol = Mathf.Log10(aValue) * 20f;
			_theAudioMixer.SetFloat("SoundVol", dbVol);
		}

		public virtual void SetMusicVolumeLevel(float aValue)
		{
			float dbVol = Mathf.Log10(aValue) * 20f;
			_theAudioMixer.SetFloat("MusicVol", dbVol);
		}

		private float DecibelToLinear(float dB) // source https://answers.unity.com/questions/283192/how-to-convert-decibel-number-to-audio-source-volu.html
		{
			float linear = Mathf.Pow(10.0f, dB / 20.0f);
			return linear;
		}
		*/
		void ShowMainMenu() //switches the active canvas from the loading screen to the main menu
		{
			_canvasManager.TimedSwitchCanvas(2, 0);
		}

		public virtual void StartGame() //hides main menu canvas, shows loading screen, and invokes LoadGameScene
		{
			_canvasManager.HideCanvas(0, false);
			_canvasManager.ShowCanvas(2, false);

			Invoke("LoadGameScene", 1f);
		}

		public virtual void LoadGameScene() //Loads game scene
		{
			SceneManager.LoadScene(gameSceneName);
		}

		public virtual void ShowOptions() //switch to Options canvas
		{
			// switch to options menu
			_canvasManager.TimedSwitchCanvas(0, 1);
		}

		public virtual void ShowExitConfirm() // switch to ExitConfirm canvas
		{
			_canvasManager.TimedSwitchCanvas(0, 3);
		}

		public virtual void CloseExitConfirm() // switch from ExitConfirm back to main menu
		{
			_canvasManager.TimedSwitchCanvas(3, 0);
		}

		public virtual void ExitGame() // close game
		{
			// close the app down
			Application.Quit();
		}

		public virtual void OptionsBackToMainMenu() // go back to main menu from Options canvas
		{
			// return to main menu from options
			_canvasManager.LastCanvas();
		}

		public virtual void BackToMainMenu() // go back to previous canvas
		{
			// return to main menu from options
			_canvasManager.LastCanvas();
		}
	}
}
