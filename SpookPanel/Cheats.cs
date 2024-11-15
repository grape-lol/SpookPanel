using UnityEngine;
using Zorro.Core.CLI;
using Vector2 = UnityEngine.Vector2;

namespace SpookPanel
{
    internal class Cheats : MonoBehaviour
    {
        private Rect windowRect = new Rect(20, 20, 300, 400);
        private int selectedTab = 0;
        private Vector2 scrollPosition = Vector2.zero;
        private string[] tabNames = { "World", "Player", "Misc" };
        private DebugUIHandler handler;

        private Color backgroundColor = new Color(0.12f, 0.12f, 0.12f);
        private Color tabActiveColor = new Color(0.25f, 0.25f, 0.25f);
        private Color textColor = Color.white;
        public bool staminaBool = false;

        public void OnGUI()
        {
            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = textColor;

            windowRect = GUI.Window(0, windowRect, DrawWindow, "Spook Panel");
        }

        public void DrawWindow(int windowID)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < tabNames.Length; i++)
            {
                GUI.backgroundColor = selectedTab == i ? tabActiveColor : backgroundColor;
                if (GUILayout.Toggle(selectedTab == i, tabNames[i], "Button", GUILayout.Height(25)))
                {
                    selectedTab = i;
                }
            }
            GUILayout.EndHorizontal();

            GUI.backgroundColor = backgroundColor;

            GUILayout.Space(10);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            switch (selectedTab)
            {
                case 0:
                    DrawWorldTab();
                    break;
                case 1:
                    DrawPlayerTab();
                    break;
                case 2:
                    DrawMiscTab();
                    break;
            }

            GUILayout.EndScrollView();
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void DrawWorldTab()
        {
            if (GUILayout.Button("Set Morning", GUILayout.Height(30)))
            {
                foreach (EveningToggler et in GameObject.FindObjectsOfType<EveningToggler>())
                {
                    et.DayTimeChanged(TimeOfDay.Morning);
                }
            }

            if (GUILayout.Button("Set Evening", GUILayout.Height(30)))
            {
                foreach (EveningToggler et in GameObject.FindObjectsOfType<EveningToggler>())
                {
                    et.DayTimeChanged(TimeOfDay.Evening);
                }
            }
        }

        private void DrawPlayerTab()
        {
            if (GUILayout.Button("Unlock All Hats", GUILayout.Height(30)))
            {
                MetaProgressionHandler.UnlockAllHats();
            }

            if (GUILayout.Button("Clear All Hats", GUILayout.Height(30)))
            {
                MetaProgressionHandler.ClearAllUnlockedHatsHats();
            }

            staminaBool = GUILayout.Toggle(staminaBool, "Infinite Stamina", GUILayout.Height(30));
            {
                if (staminaBool == true)
                {
                    if (Player.localPlayer != null)
                    {
                        Player.localPlayer.data.remainingOxygen = Player.localPlayer.data.maxOxygen;
                        // Player.localPlayer.data.staminaDepleated = false;
                    }
                }
            }
        }

        private void DrawMiscTab()
        {
            if (GUILayout.Button("Open Console", GUILayout.Height(30)))
            {
                handler = FindObjectOfType<DebugUIHandler>();
                handler.Show();
            }

            if (GUILayout.Button("Close Console", GUILayout.Height(30)))
            {
                Debug.ClearDeveloperConsole();
                handler.Hide();
            }
        }
    }
}
