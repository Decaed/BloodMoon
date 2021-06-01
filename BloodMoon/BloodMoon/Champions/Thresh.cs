using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloodMoon.Champions
{

    public class Thresh
    {
        private static AIHeroClient ME = GameObjects.Player;
        private static Spell Q, W, E, R, Q2;
        private static Menu Config;

        public static void OnGameLoad()
        {
            if (GameObjects.Player.CharacterName != "Thresh")
            {
                return;
            }
            Q = new Spell(SpellSlot.Q, 1050f);
            E = new Spell(SpellSlot.E, 500);

            Q.SetSkillshot(0.5f, 70, 1900, true, SpellType.Line);
            E.SetSkillshot(0.125f, 110, 2000, false, SpellType.Line);
                    
            Config = new Menu("Thresh", "BloodMoon", true);
            
            var PredMenu = new Menu("Prediction", "Prediction");
            SPredictionMash.ConfigMenu.Initialize(PredMenu, "Get Prediction");

            var menuD = new Menu("dsettings", "Drawings");
            menuD.Add(new MenuBool("drawQ", "Q Range  (Red)", true));
            menuD.Add(new MenuBool("drawE", "E Range  (Blue)", true));
            menuD.Add(new MenuBool("drawW", "W Range (Green)", true));
            menuD.Add(new MenuBool("drawR", "R Range  (White)", false));

            var menuK = new Menu("skinslide", "Skin Changer");
            menuK.Add(new MenuSliderButton("skin", "SkinID", 0, 0, 20, false));

            var menuRR = new Menu("semiR", "Semi Skills");
            menuRR.Add(new MenuKeyBind("farm", "Lane Clear spells", Keys.Select, KeyBindType.Toggle));
            
            Config.Add(menuD);
            Config.Add(menuK);
            Config.Add(menuRR);
            

            Config.Attach();

            GameEvent.OnGameTick += OnGameUpdate;
            Drawing.OnDraw += OnDraw;
        }

        public static void OnGameUpdate(EventArgs args)
        {

            switch (Orbwalker.ActiveMode)
            {
                case OrbwalkerMode.Combo:
                    logicE();
                    logicQ();
                    break;

                case OrbwalkerMode.Harass:
                    break;

                case OrbwalkerMode.LaneClear:
                    break;

                case OrbwalkerMode.LastHit:                   
                    break;

            }

           
            skinch();
        }

        private static void skinch()
        {
            if (Config["skinslide"].GetValue<MenuSliderButton>("skin").Enabled)
            {
                int skinut = Config["skinslide"].GetValue<MenuSliderButton>("skin").Value;

                if (GameObjects.Player.SkinId != skinut)
                    GameObjects.Player.SetSkin(skinut);
            }
        }


        private static void OnDraw(EventArgs args)
        {
            var position = GameObjects.Player.Position;

            if (Config["dsettings"].GetValue<MenuBool>("drawQ").Enabled)
            {
                Render.Circle.DrawCircle(position, Q.Range, System.Drawing.Color.Red);
            }

            if (Config["dsettings"].GetValue<MenuBool>("drawE").Enabled)
            {
                Render.Circle.DrawCircle(position, E.Range, System.Drawing.Color.Blue);
            }

            if (Config["dsettings"].GetValue<MenuBool>("drawW").Enabled)
            {
                Render.Circle.DrawCircle(position, W.Range, System.Drawing.Color.Green);
            }

            if (Config["dsettings"].GetValue<MenuBool>("drawR").Enabled)
            {
                Render.Circle.DrawCircle(position, R.Range, System.Drawing.Color.White);
            }
        }

        private static void logicQ()
        {
            if (Q.IsReady())
            {
                var target = Q.GetTarget();
                var input = Q.GetPrediction(target, true);

                if (!target.IsValidTarget())
                    return;

                if (target.HasBuff("threshQ"))
                return;
                        
                if (input.Hitchance >= HitChance.High && target.DistanceToPlayer() > 500)
                {
                    Q.Cast(input.CastPosition);
                }

            }
        }
        
        private static void logicE()
  {
      var target = E.GetTarget(); ;
      var Player = GameObjects.Player;

      if (E.IsReady() && target.DistanceToPlayer() < E.Range)
      {
        E.Cast(target.Position.Extend(Player.Position, Vector3.Distance(target.Position, Player.Position) + 500));
      }
    }     
  }  
}
