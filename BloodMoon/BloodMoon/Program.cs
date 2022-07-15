using EnsoulSharp;
using EnsoulSharp.SDK;
using BloodMoon.Champions;
using System;
using System.Net;

namespace BloodMoon {

    public class Program {

        public static void Main(string[] args) {
            GameEvent.OnGameLoad += OnLoadingComplete;
        }

        private static void OnLoadingComplete() {
            if (ObjectManager.Player == null)
                return;
            try {
                switch (GameObjects.Player.CharacterName) {
                       case "Thresh":
                        Thresh.OnGameLoad();
                        Game.Print("<font color='#ff0000' size='25'> [BloodMoon]:  </font>" + ObjectManager.Player.CharacterName + " Loaded");
                        Game.Print("<font color='#ff0000' size='25'> [BloodMoon]:  </font>" + "<font color='#F7FF00' size='25'></font>");
                        Game.Print("<font color='#ff0000' size='25'> [BloodMoon]:  </font>" + "<font color='#F7FF00' size='25'></font>");
                        break;

                    default:
                        Game.Print("<font color='#ff0000' size='25'></font>");
                        Console.WriteLine("[BloodMoon] Does Not Support " + ObjectManager.Player.CharacterName);
                        break;
                }
                string stringg;
                string uri = "https://github.com/Decaed/BloodMoon/blob/main/BloodMoon/version.txt";
                using (WebClient client = new WebClient()) {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    stringg = client.DownloadString(uri);
                }
                string versionas = "2.0\n";
                if (versionas != stringg) {
                    Game.Print("<font color='#ff0000'> [BloodMoon]: </font> <font color='#ffe6ff' size='25'></font>");
                }
                else if (versionas == stringg) {
                    Game.Print("<font color='#ff0000' size='25'> [BloodMoon]: </font> <font color='#ffe6ff' size='25'></font>");
                }
            }
            catch (Exception ex) {
                Game.Print("Error in loading");
            }
        }
    }
}
