﻿/******************************************************************************************
 * Name: ActionButton.cs
 * Created by: Jeremy Voss
 * Created on: 02/27/2019
 * Last Modified: 02/27/2019
 * Description:
 * This button is responsible for handling actions within the game such as mining, farming,
 * etc.  The action button is automatically generated by UI Manager and assigned an 
 * associated action.
 ******************************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PixelsoftGames.PixelUI
{
    [RequireComponent(typeof(Button))]
    public class ActionButton : MonoBehaviour
    {
        #region Constants

        static readonly List<Actions> unlockedActions = new List<Actions>() { };

        #endregion

        #region Fields & Properties

        [SerializeField]
        [Tooltip("The button text for this button.")]
        Text buttonText = null;

        /// <summary>
        /// The action associated with this button.
        /// </summary>
        Actions action;
        /// <summary>
        /// The button instance associated with this button.
        /// </summary>
        Button button = null;

        #endregion

        #region Monobehavior Callbacks

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Called when the button is clicked which will cause it to transmit to Game Manager that an action is being performed
        /// and what action that is.
        /// </summary>
        public void OnClick()
        {
            GameManager.Instance.PerformAction(action);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called by the UI Manager when creating the action buttons on demand.  An action from the list of available actions
        /// for the associated location is automatically assigned to this button.
        /// </summary>
        /// <param name="action">The action assigned to this button.</param>
        public void SetData(Actions action)
        {
            this.action = action;
            buttonText.text = action.ToString().Replace('_', ' ');

            if (unlockedActions == null || unlockedActions.Count == 0 || !unlockedActions.Contains(action))
                button.interactable = false;
            else
                button.interactable = true;
        }

        #endregion
    }
}