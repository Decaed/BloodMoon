using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using FSpred.Prediction;
using SPredictionMash;

namespace BloodMoon.Champions
{

    public class Thresh
    {
        private static AIHeroClient ME = GameObjects.Player;
        private static Spell Q, W, E, R, Q2;
        private static Menu Config;
        private static FSpred.Prediction.PredictionInput QPred;
        private static FSpred.Prediction.PredictionInput WPred;

        public static void OnGameLoad()
        {
            if (GameObjects.Player.CharacterName != "Thresh")
            {
                return;
            }
            Q = new Spell(SpellSlot.Q, 1050f);
            E = new Spell(SpellSlot.E, 500);

            Q.SetSkillshot(0.5f, 70f, 1900f, true, SpellType.Line);
            E.SetSkillshot(0.75f, 10f, float.MaxValue, false, SpellType.Line);
            
            QPred = new FSpred.Prediction.PredictionInput
            {
                Aoe = false,
                CollisionYasuoWall = true,
                Collision = true,
                CollisionObjects = new FSpred.Prediction.CollisionableObjects[] { FSpred.Prediction.CollisionableObjects.Minions, FSpred.Prediction.CollisionableObjects.YasuoWall },
                Delay = 0.5f,
                Radius = 70f,
                Range = 1050f,
                Speed = 1900f,
                Type = FSpred.Prediction.SkillshotType.SkillshotLine,
                From = ME.Position,
                RangeCheckFrom = ME.Position
            };
            

            Config = new Menu("Thresh", "BloodMoon", true);

            var menuD = new Menu("dsettings", "DRAWNINGS");
            menuD.Add(new MenuBool("drawQ", "Q Range  (RED)", true));
            menuD.Add(new MenuBool("drawE", "E Range  (BLUE)", true));
            menuD.Add(new MenuBool("drawW", "W Range (GREEN)", true));
            menuD.Add(new MenuBool("drawR", "R Range  (WHITE)", false));

            var MenuC = new Menu("infor", "INFORMATION", false);
            MenuC.Add(new Menu("infotool", " Harass||LastHit =  C, " + "\n LaneClear||JungleFarm = V, " + "\n Combo = SPACEBAR" + " \n Last Hit = X \n" + " Disable Drawings = L " + " \n Fore more FPS Disable EzEvade and LyrdumAIO Drawings + [Awarness] Waypoint and check Jungle->track only jungle \n If you have a bug or suggestion post it on discord server discord.gg/KfQFVhdqtz"));

            var menuM = new Menu("mana", "MANA HARASS");
            menuM.Add(new MenuSlider("manaW", "W mana %", 60, 0, 100));
            menuM.Add(new MenuSlider("manaE", "E mana %", 60, 0, 100));
            menuM.Add(new MenuSlider("manaQ", "Q mana %", 60, 0, 100));

            var MenuS = new Menu("harass", "HARASS SKILLS", false);
            MenuS.Add(new MenuBool("useQ", "Use Q ", true));
            MenuS.Add(new MenuBool("useE", "Use E ", true));
            MenuS.Add(new MenuBool("useW", "Use W ", false));

            var Menuclear = new Menu("laneclear", "LANE CLEAR SKILLS", false);
            Menuclear.Add(new MenuBool("useE", "Use E ", true));

            var MenuJungle = new Menu("jungleskills", "JUNGLE SKILLS", false);
            MenuJungle.Add(new MenuBool("useQ", "Use Q ", true));
            MenuJungle.Add(new MenuBool("useE", "Use E ", true));
            MenuJungle.Add(new MenuBool("useW", "Use W ", false));


            var menuK = new Menu("skinslide", "SKIN CHANGER");
            menuK.Add(new MenuSliderButton("skin", "SkinID", 0, 0, 20, false));

            var menuRR = new Menu("semiR", "SEMI SKILLS");
            menuRR.Add(new MenuKeyBind("farm", "Lane Clear spells", Keys.Select, KeyBindType.Toggle));

            Config.Add(MenuC);
            Config.Add(menuD);
            Config.Add(MenuS);
            Config.Add(Menuclear);
            Config.Add(MenuJungle);
            Config.Add(menuM);
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
                var target = TargetSelector.GetTarget(Q.Range);
                var Player = GameObjects.Player;
                var pred = SebbyLibPorted.Prediction.Prediction.GetPrediction(Q, target);

                if (!target.IsValidTarget())
                    return;

                if (target.HasBuff("threshQ"))
                return;
                        
                if (pred != null && pred.Hitchance >= SebbyLibPorted.Prediction.HitChance.High)
                {
                    Q.Cast(pred.CastPosition);
                }

            }
        }

        private static void logicE()
  {
      var target = E.GetTarget(E.Range);
      var Player = GameObjects.Player;

      if (E.IsReady() && Player.Distance(target.Position) < E.Range)
      {
        E.Cast(target.Position.Extend(Player.Position, Vector3.Distance(target.Position, Player.Position) + 500));
      }
    }     
  }  
}
