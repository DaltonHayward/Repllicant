using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReplicantPackage
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;

        [SerializeField] private GameObject _mainMenuCanvasGO;
        [SerializeField] private GameObject _settingsMenuCanvasGO;
        [SerializeField] private GameObject _controlsMenuCanvasGO;

        [Header("First Selected Options")]
        [SerializeField] private GameObject _mainMenuFirst;
        [SerializeField] private GameObject _settingsMenuFirst;
        [SerializeField] private GameObject _controlsMenuFirst;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }

            _mainMenuCanvasGO.SetActive(false);
            _settingsMenuCanvasGO.SetActive(false);
            _controlsMenuCanvasGO.SetActive(true);
            _controlsMenuCanvasGO.SetActive(false);
        }

        public void OpenMainMenu()
        {
            _mainMenuCanvasGO.SetActive(true);
            _settingsMenuCanvasGO.SetActive(false);
            _controlsMenuCanvasGO.SetActive(false);

            EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
        }

        private void OpenSettingsMenu()
        {
            _settingsMenuCanvasGO.SetActive(true);
            _mainMenuCanvasGO.SetActive(false);
            _controlsMenuCanvasGO.SetActive(false);

            EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
        }

        private void OpenControlsMenu()
        {
            _controlsMenuCanvasGO.SetActive(true);
            _settingsMenuCanvasGO.SetActive(false);
            _mainMenuCanvasGO.SetActive(false);

            EventSystem.current.SetSelectedGameObject(_controlsMenuFirst);
        }

        public void CloseAllMenus()
        {
            _mainMenuCanvasGO.SetActive(false);
            _settingsMenuCanvasGO.SetActive(false);
            _controlsMenuCanvasGO.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);
        }

        #region Main Menu Button Actions

        public void OnSettingsPress()
        {
            OpenSettingsMenu();
        }

        #endregion

        #region Settings Menu Button Actions

        public void OnSettingsBackPress()
        {
            OpenMainMenu();
        }

        public void OnControlsPress()
        {
            OpenControlsMenu();
        }

        #endregion

        #region Controls Menu Button Actions

        public void OnControlsBackPress()
        {
            OpenSettingsMenu();
        }

        public void OnExitPress()
        {
            Application.Quit();
        }

        #endregion

    }

}
