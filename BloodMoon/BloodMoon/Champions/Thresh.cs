using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using FunnySlayerCommon;
using SPredictionMash;

namespace BloodMoon.Champions
{

    public class Thresh
    {
        private static AIHeroClient ME = GameObjects.Player;
        private static Spell Q, W, E, R, Q2;
        private static Menu Config;
        public static FSpred.Prediction.PredictionInput QPred;
        private static FSpred.Prediction.PredictionInput WPred;

        public static void OnGameLoad()
        {
            if (GameObjects.Player.CharacterName != "Thresh")
            {
                return;
            }
            Q = new Spell(SpellSlot.Q, 1050f);
            E = new Spell(SpellSlot.E, 500);

            Q.SetSkillshot(0.5f, 70, 1900, true, SpellType.Line);
            E.SetSkillshot(0.75f, 10f, float.MaxValue, false, SpellType.Line);
            
            QPred = new FSpred.Prediction.PredictionInput
            {
                Aoe = false,
                CollisionYasuoWall = true,
                Collision = true,
                CollisionObjects = new FSpred.Prediction.CollisionableObjects[] { FSpred.Prediction.CollisionableObjects.Minions },
                Delay = 0.50f,
                Radius = 70f,
                Range = 1050f,
                Speed = 1900f,
                Type = FSpred.Prediction.SkillshotType.SkillshotLine,
                From = ME.Position,
                RangeCheckFrom = ME.Position
            };
       
           
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
                var target = Q.GetTarget(); ;
                var Player = GameObjects.Player;
                var input = Q.GetPrediction(target, true);

                if (!target.IsValidTarget())
                    return;

                if (target.HasBuff("threshQ"))
                return;
                        
                if (input.Hitchance >= HitChance.High && target.DistanceToPlayer() > 500)
                {
                    Q.Cast(target);
                }

            }
        }
        
        private static void logicE()
  {
      var target = E.GetTarget(E.Range);
      var Player = GameObjects.Player;

      if (E.IsReady() && target.DistanceToPlayer() < E.Range)
      {
        E.Cast(target.Position.Extend(Player.Position, Vector3.Distance(target.Position, Player.Position) + 500));
      }
    }     
  }  
}
