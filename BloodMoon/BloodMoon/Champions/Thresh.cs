using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
            
            Q.SetSkillshot(0.5f, 70f, 1900, true, SpellType.Line);
            E.SetSkillshot(0.125f, 110f, 2000, false, SpellType.Line);
                    
            Config = new Menu("Thresh", "BloodMoon", true);
        
            var menuD = new Menu("dsettings", "Drawings");
            menuD.Add(new MenuBool("drawQ", "10050 Red)", true));
            menuD.Add(new MenuBool("drawW", "900 (Green)", true));
      

            var menuRR = new Menu("semiR", "Semi Skills");
            menuRR.Add(new MenuKeyBind("farm", "Lane Clear spells", Keys.Select, KeyBindType.Toggle));
            
            Config.Add(menuD);
            Config.Add(menuRR);
 
           
            Config.Attach();
            GameEvent.OnGameTick += OnGameUpdate;
            AntiGapcloser.OnGapcloser += Gapcloser_OnGapcloser;
      
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
        
        private static void Gapcloser_OnGapcloser(AIHeroClient sender, AntiGapcloser.GapcloserArgs args)
        {
            if (sender.IsAlly)
                return;

            if (args.SpellName == "ZedR")
                return;

            if(args.EndPosition.DistanceToPlayer() < args.StartPosition.DistanceToPlayer())
            {
                if(args.EndPosition.DistanceToPlayer() <= 500 && sender.IsValidTarget(1000))
                {
                    if (E.Cast(sender) == CastStates.SuccessfullyCasted)
                        return;
                }
                else
                {
                    return;
                }
            }
        }
                   
        private static void CastQ()
        {
            var target = Q.GetTarget(Q.Range);
            
            if (target == null)
                return;
            
            if (target.HasBuff("threshq"))
                return;
            
            if (ObjectManager.Player.Distance(target.Position) < 500) 
                return;
            
            var prediction = Q.GetPrediction(target);
            
            if (prediction.Hitchance >= (HitChance.High))
                    {
                        Q.Cast(prediction.CastPosition);
                    }
        }
                
        
        private static void AutoCast()
        {
            var target = Q.GetTarget(Q.Range);
            
            if (target == null)
                return;

            
            if (target.HasBuff("threshq"))
                return;
            
            var prediction = Q.GetPrediction(target);
            
            if (prediction.Hitchance == (HitChance.Dash))
                    {
                        Q.Cast(prediction.CastPosition);
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
