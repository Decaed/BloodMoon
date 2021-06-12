using System;
using System.Linq;

namespace OneKeyToWin_AIO_Sebby.Champions
{
    using EnsoulSharp;
    using EnsoulSharp.SDK;
    using EnsoulSharp.SDK.MenuUI;
    using SebbyLib;
    using SharpDX;

    class Thresh : Base
    {
        private readonly MenuBool onlyRdy = new MenuBool("onlyRdy", "Draw only ready spells");
        private readonly MenuBool qRange = new MenuBool("qRange", "Q range", false);
        private readonly MenuBool eRange = new MenuBool("eRange", "E range", false);
        private readonly MenuBool rRange = new MenuBool("rRange", "R range", false);
        private readonly MenuBool eDamageEnemies = new MenuBool("eDamageEnemies", "E damage % on enemies", false);
        private readonly MenuBool eDamageJungles = new MenuBool("eDamageJungles", "E damage % on jungles", false);

        private readonly MenuSlider qMana = new MenuSlider("qMana", "Q harass mana %", 50);
        private readonly MenuList qMode = new MenuList("qMode", "Q combo mode", new[] { "Always", "OKTW logic" }, 1);

        private readonly MenuBool dragonW = new MenuBool("dragonW", "Auto W Dragon, Baron, Blue, Red");

        private readonly MenuSlider countE = new MenuSlider("countE", "Auto E if x stacks", 10, 0, 30);
        private readonly MenuSlider dmgE = new MenuSlider("dmgE", "E % dmg adjust", 100, 50, 150);
        private readonly MenuBool deadE = new MenuBool("deadE", "Cast E before Kalista dead");
        private readonly MenuBool killminE = new MenuBool("killminE", "Cast E minion kill + harass target");

        private readonly MenuBool autoR = new MenuBool("autoR", "Auto R");

        //private readonly MenuBool balista = new MenuBool("balista", "Balista R");
        //private readonly MenuSlider rangeBalista = new MenuSlider("rangeBalista", "Balista min range", 500, 0, 1000);

        private readonly MenuBool minionAA = new MenuBool("minionAA", "AA minions if no enemies in range when combo mode");

        private readonly MenuBool farmQ = new MenuBool("farmQ", "Lane clear Q");
        private readonly MenuBool farmE = new MenuBool("farmE", "Lane clear E");
        private readonly MenuSlider farmQcount = new MenuSlider("farmQcount", "Lane clear Q if x minions", 2, 1, 10);
        private readonly MenuSlider farmEcount = new MenuSlider("farmEcount", "Auto E if x minions", 2, 1, 10);
        private readonly MenuBool minionE = new MenuBool("minionE", "Auto E big minion");
        private readonly MenuBool jungleE = new MenuBool("jungleE", "Jungle ks E");

        private float lastCastE = 0;
        //private float grabTime = 0;
        private int countW = 0;
        private AIHeroClient AllyR;

        public Thresh()
        {
            Q = new Spell(SpellSlot.Q, 1100f);
            Q1 = new Spell(SpellSlot.Q, 1100f);
            W = new Spell(SpellSlot.W, 5000f);
            E = new Spell(SpellSlot.E, 500f);
            R = new Spell(SpellSlot.R, 1100f);

            Q.SetSkillshot(0.50f, 70f, 1900f, true, SpellType.Line);
            Q1.SetSkillshot(0.25f, 40f, 2400f, false, SpellType.Line);

            Local.Add(new Menu("draw", "Draw")
            {
                onlyRdy,
                qRange,
                eRange,
                rRange,
                eDamageEnemies,
                eDamageJungles
            });

            Local.Add(new Menu("qConfig", "Q Config")
            {
                qMana,
                qMode
            });

            Local.Add(new Menu("wConfig", "W Config")
            {
                dragonW
            });

            Local.Add(new Menu("eConfig", "E Config")
            {
                countE,
                dmgE,
                deadE,
                killminE
            });

            Local.Add(new Menu("rConfig", "R Config")
            {
                autoR
            });

            //Local.Add(new Menu("balistaConfig", "Balista Config")
            //{
            //    balista,
            //    rangeBalista
            //});

            Local.Add(minionAA);

            FarmMenu.Add(farmQ);
            FarmMenu.Add(farmE);
            FarmMenu.Add(farmQcount);
            FarmMenu.Add(farmEcount);
            FarmMenu.Add(minionE);
            FarmMenu.Add(jungleE);

            
            AntiGapcloser.OnGapcloser += AntiGapcloser_OnGapcloser;
            Game.OnUpdate += Game_OnUpdate;

           
        }
        
        private void AntiGapcloser_OnGapcloser(AIHeroClient sender, AntiGapcloser.GapcloserArgs args)
        {
            if (E.IsReady())
            {
                if (sender.IsValidTarget(500))
                {
                    E.Cast(sender);
                }
            }
        }
        
                private void Game_OnUpdate(EventArgs args)
        {
            if (E.IsReady())
            {
                CastE();
            }

            if (Q.IsReady())
            {
                CastQ();
            }
        }

        private void CastQ()
        {
            var t = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (t.IsValidTarget())
            {
                var poutput = Q.GetPrediction(t);
                var col = poutput.CollisionObjects;
                }
                if (poutput && col == nil);
                Q.Cast(poutput(t));
                }
                }
                }

        private void CastE()
        {
            if (Game.Time - lastCastE < 0.4)
            {
                return;
            }

            E.Cast();
        }
                       
        private void drawText(string msg, AIBaseClient target, System.Drawing.Color color)
        {
            var wts = target.HPBarPosition;
            Drawing.DrawText(wts[0] - msg.Length * 5, wts[1] - 80, color, msg);
        }
    
}
}
}
}
