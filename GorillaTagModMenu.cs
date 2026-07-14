using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// A legal Gorilla Tag mod menu DLL with a disconnect button
/// This mod provides basic UI functionality for managing game options
/// and includes a disconnect feature for leaving matches
/// 
/// To compile this as a DLL:
/// 1. Create a new C# Class Library project in Visual Studio
/// 2. Add references to:
///    - UnityEngine.dll
///    - Photon.Pun.dll
///    - Photon.Common.dll
/// 3. Copy this file into the project
/// 4. Build in Release mode to generate the .dll file
/// </summary>
namespace GorillaTagMods
{
    public class GorillaTagModMenu : MonoBehaviour
    {
        private bool menuOpen = false;
        private bool showMenu = false;
        private Vector2 scrollPosition = Vector2.zero;
        private Rect menuRect = new Rect(20, 20, 300, 400);
        
        private List<MenuOption> menuOptions = new List<MenuOption>();
        
        private void Start()
        {
            InitializeMenuOptions();
            Debug.Log("[GorillaTagModMenu] Mod menu initialized successfully");
        }
        
        private void InitializeMenuOptions()
        {
            // Initialize menu options - only legal, non-cheat options
            menuOptions.Add(new MenuOption("Disconnect", DisconnectFromGame));
            menuOptions.Add(new MenuOption("Toggle Menu", ToggleMenu));
            menuOptions.Add(new MenuOption("Settings", OpenSettings));
        }
        
        private void Update()
        {
            // Toggle menu with a specific key combination (pressing 'M')
            if (Input.GetKeyDown(KeyCode.M))
            {
                showMenu = !showMenu;
            }
        }
        
        private void OnGUI()
        {
            if (!showMenu)
                return;
            
            try
            {
                // Draw the menu window
                menuRect = GUI.Window(0, menuRect, DrawMenuWindow, "Gorilla Tag Mod Menu");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GorillaTagModMenu] Error rendering menu: {ex.Message}");
            }
        }
        
        private void DrawMenuWindow(int windowID)
        {
            GUILayout.BeginVertical();
            
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(350));
            
            // Display all menu options
            foreach (MenuOption option in menuOptions)
            {
                if (GUILayout.Button(option.Name, GUILayout.Height(40)))
                {
                    try
                    {
                        option.Action?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[GorillaTagModMenu] Error executing option '{option.Name}': {ex.Message}");
                    }
                }
            }
            
            GUILayout.EndScrollView();
            
            // Close button
            if (GUILayout.Button("Close Menu", GUILayout.Height(30)))
            {
                showMenu = false;
            }
            
            GUILayout.EndVertical();
            
            // Make window draggable
            GUI.DragWindow(new Rect(0, 0, 300, 20));
        }
        
        /// <summary>
        /// Disconnect from the current game session
        /// This is a legal feature that allows players to leave matches
        /// </summary>
        private void DisconnectFromGame()
        {
            try
            {
                // Disconnect from Photon network
                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.Disconnect();
                    Debug.Log("[GorillaTagModMenu] Successfully disconnected from game session");
                }
                else
                {
                    Debug.Log("[GorillaTagModMenu] Not connected to a game session");
                }
                
                showMenu = false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GorillaTagModMenu] Error disconnecting: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Toggle the menu visibility
        /// </summary>
        private void ToggleMenu()
        {
            menuOpen = !menuOpen;
            Debug.Log($"[GorillaTagModMenu] Menu toggled: {(menuOpen ? "Open" : "Closed")}");
        }
        
        /// <summary>
        /// Open game settings (placeholder for future expansion)
        /// </summary>
        private void OpenSettings()
        {
            Debug.Log("[GorillaTagModMenu] Settings opened");
            // Settings can be expanded here
        }
        
        /// <summary>
        /// Helper class to represent a menu option
        /// </summary>
        private class MenuOption
        {
            public string Name { get; set; }
            public Action Action { get; set; }
            
            public MenuOption(string name, Action action)
            {
                Name = name;
                Action = action;
            }
        }
    }
}
