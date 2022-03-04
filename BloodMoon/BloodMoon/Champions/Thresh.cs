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
            Q = new Spell(SpellSlot.Q, 1025);
            E = new Spell(SpellSlot.E, 500);

            Q.SetSkillshot(0.5f, 70f, 1900f, true, SpellType.Line);
            E.SetSkillshot(0.125f, 110f, 2000f, false, SpellType.Line);
                    
            Config = new Menu("Thresh", "BloodMoon", true);
        
            var menuD = new Menu("dsettings", "Drawings");
            menuD.Add(new MenuBool("drawQ", "1075  (Red)", true));
            menuD.Add(new MenuBool("drawW", "900 (Green)", true));

            var menuRR = new Menu("semiR", "Semi Skills");
            menuRR.Add(new MenuKeyBind("farm", "Lane Clear spells", Keys.Select, KeyBindType.Toggle));
            
            Config.Add(menuD);
            Config.Add(menuRR);
            

            Config.Attach();

            GameEvent.OnGameTick += OnGameUpdate;
            Drawing.OnDraw += OnDraw;
        } 

        public static void OnGameUpdate(EventArgs args)
        {
            Dashing();
        
        
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
                var target = Q.GetTarget(1025);
                
                if (!target.IsValidTarget())
                    return;
                
                if (target.DistanceToPlayer() < 500)
                    return;
            
                if (target.HasBuff("threshQ"))
                return;

                var input = Q.GetPrediction(target, true);
;

                if (input.Hitchance >= HitChance.High)
                {
                    Q.Cast(input.CastPosition);
                }

            }
        }
        
        private static void Dashing()
        {
            if (Q.IsReady())
            {
                var target = Q.GetTarget(Q.Range);
            
                if (!target.IsValidTarget())
                    return;
                
                if (target.DistanceToPlayer() < 500)
                    return;
                
                if (target.HasBuff("threshQ"))
                return;

                var input = Q.GetPrediction(target, true);
;

                if (input.Hitchance >= HitChance.Dash)
                {
                    Q.Cast(input.CastPosition);
                }

            }
        }
        
        private static void logicE()
  {
      var target = E.GetTarget(E.Range); ;
      var Player = GameObjects.Player;

      if (E.IsReady() && target.DistanceToPlayer() < E.Range)
      {
        E.Cast(target.Position.Extend(Player.Position, Vector3.Distance(target.Position, Player.Position) + 500));
      }
    }     
  }  
}
