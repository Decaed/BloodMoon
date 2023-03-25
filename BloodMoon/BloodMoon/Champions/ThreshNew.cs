using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using SharpDX;
using System;

namespace BloodMoon.Champions
{
    public class Thresh
    {
        private AIHeroClient ME = ObjectManager.Player;
        private Spell Q, W, E, R;
        private Menu Config;

        public Thresh()
        {
            if (ME.CharacterName != "Thresh")
                return;

            Q = new Spell(SpellSlot.Q, 1050);
            E = new Spell(SpellSlot.E, 500);

            Q.SetSkillshot(0.5f, 70f, 1900, true, SpellType.Line);
            E.SetSkillshot(0.125f, 110f, 2000, false, SpellType.Line);

            Config = new Menu("Thresh", "BloodMoon", true);

            var menuD = new Menu("dsettings", "Drawings");
            menuD.Add(new MenuBool("drawQ", "1050 Red", true));
            menuD.Add(new MenuBool("drawW", "900 Green", true));

            var menuRR = new Menu("semiR", "Semi Skills");
            menuRR.Add(new MenuKeyBind("farm", "Lane Clear spells", Keys.Select, KeyBindType.Toggle));

            Config.Add(menuD);
            Config.Add(menuRR);

            Config.Attach();
            GameEvent.OnGameTick += OnGameUpdate;
            AntiGapcloser.OnGapcloser += Gapcloser_OnGapcloser;
        }
