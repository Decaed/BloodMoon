using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using SPrediction;


namespace BloodMoon.Champions
{

    public class Thresh
    {
        private static AIHeroClient ME = GameObjects.Player;
        private static Spell Q, W, E, R;
        private static Menu Config;

        public static void OnGameLoad()
        {
            if (GameObjects.Player.CharacterName != "Thresh")
            {
                return;
            }
            Q = new Spell(SpellSlot.Q, 1050);
            E = new Spell(SpellSlot.E, 500);
            
            Q.SetSkillshot(0.5f, 70, 1900, true, SpellType.Line);
            E.SetSkillshot(0.125f, 110f, 2000f, false, SpellType.Line);
                    
            Config = new Menu("Thresh", "BloodMoon", true);
        
            var menuD = new Menu("dsettings", "Drawings");
            menuD.Add(new MenuBool("drawQ", "1075  (Red)", true));
            menuD.Add(new MenuBool("drawW", "900 (Green)", true));
            
            var pred = new Menu("pred", "Spred Config");

            var menuRR = new Menu("semiR", "Semi Skills");
            menuRR.Add(new MenuKeyBind("farm", "Lane Clear spells", Keys.Select, KeyBindType.Toggle));
            
            Config.Add(menuD);
            Config.Add(menuRR);
            SPrediction.Prediction.Initialize(pred);
            

            Config.Attach();
            GameEvent.OnGameTick += OnGameUpdate;
            Drawing.OnDraw += OnDraw;
        } 

        public static void OnGameUpdate(EventArgs args)
        {
           AutoCast();
        
            switch (Orbwalker.ActiveMode)
            {
                case OrbwalkerMode.Combo:
                    logicE();
                    CastQ();
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
        

                   
        private static void CastQ()
        {
            var qhit = HitChance.High;
            var target = Q.GetTarget(Q.Range);
            if (target == null)
                return;

            if (target.HasBuff("threshQ"))
                return;

            Q.SPredictionCast(target, qhit);
        }
        
        private static void AutoCast()
        {

            if (Q.IsReady())
            {
                foreach (
                    var ii in
                        ObjectManager.Get<AIHeroClient>()
                            .Where(x => x.IsValidTarget(Q.Range)))
                {
                if (ii.HasBuff("threshQ"))
                return;
                    {
                        if (ObjectManager.Player.Distance(ii.Position) < 500)
                        return;
                        {
                        Q.SPredictionCast(ii, HitChance.Dash);
                        }
                    }
                }
            }
        }
        
        private static void logicE()
  {
      var target = E.GetTarget(E.Range);
      var Player = GameObjects.Player;

      if (E.IsReady() && ObjectManager.Player.Distance(target.Position) < 500)
      {
        E.Cast(target.Position.Extend(ObjectManager.Player.Position, Vector3.Distance(target.Position, ObjectManager.Player.Position) + 500));
      }
    }     
  }  
}
