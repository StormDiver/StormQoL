using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using StormQoL;

using static Terraria.ModLoader.ModContent;

namespace StormQoL
{
    public class Configurations : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Celestial Event Shield")]

        [Label("Pillar shield kill count (Classic Mode)")]
        [Tooltip("How many enemies will have to be defeated for the shield to be destroyed in Classic difficulty")]
        [Range(10, 100)]
        [Slider]
        [DefaultValue(100)]
        //[ReloadRequired]
        public int shieldHealthNormal;

        [Label("Pillar shield kill count (Expert+ Mode)")]
        [Tooltip("How many enemies will have to be defeated for the shield to be destroyed in Expert or Master")]
        [Range(10, 150)]
        [Slider]
        [DefaultValue(150)]
        // [ReloadRequired]
        public int shieldHealthExpert;
      
        [Header("Damage Variance")]

        [Label("Remove damage variance")]
        [Tooltip("This will remove the random spread in all damage dealt and taken (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool noDamageSpread { get; set; }

        [Header("Mining Speed tweaks")]

        [Label("Allow Axes and Hammers to benefit from mining buffs")]
        [Tooltip("Allows Axes and Hammers to mine faster if you have any mining buffs equipped, works with modded tools too (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool FastChop4U { get; set; }

        [Label("Allow mechanical tools to benefit from mining buffs")]
        [Tooltip("Allows mechanical tools to mine faster if you have any mining buffs equipped, works with modded tools too (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool FastDrill4U { get; set; }

        [Header("Misc")]

        [Label("Prevent Treasure Bags from droppping dev items")]
        [Tooltip("If you have a lot of bags to open and don't want your inventory cluttered by dev items (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool NoInventoryClutter { get; set; }     
    }
    public class StormQoL : Mod
    {
        public override void Load()
        {
            base.Load();
            if (GetInstance<Configurations>().noDamageSpread)
            {
                //!!All credit goes to Kojo's mod called Rho's Playground!!
                On.Terraria.Main.DamageVar += (orig, damage, luck) => (int)Math.Round(damage * Main.rand.NextFloat(1, 1)); //No damage variance
            }
        }

    }
    public class Itemchanges : GlobalItem
    {
        public override bool InstancePerEntity => true;

        //Drill speeds
        int basedrillspeed; //base mining speed of drill
        public override void SetDefaults(Item item)
        {
            if (GetInstance<Configurations>().NoInventoryClutter)
            {
                if (ItemID.Sets.BossBag[item.type])
                {
                    ItemID.Sets.PreHardmodeLikeBossBag[item.type] = true;
                }
            }
            //Mech tools
            if (GetInstance<Configurations>().FastDrill4U)
            {
                if ((item.pick >= 1 || item.axe >= 1 || item.hammer >= 1) && item.channel) // Should detect the item as a mech tool like this unless the modder did it wrong
                {
                    basedrillspeed = item.useTime; // set the int once so it doesn't change
                }
            }
            //Axes and Hammers
            if (GetInstance<Configurations>().FastChop4U)
            {
                if ((item.axe >= 1 || item.hammer >= 1) && !item.channel) // Should detect the item as a hammmer/axe like this unless the modder did it wrong
                {
                    basedrillspeed = item.useTime; // set the int once so it doesn't change
                }
            }
            base.SetDefaults(item);
        }
        public override void HoldItem(Item item, Player player)
        {

            float drillspeed = player.pickSpeed * 100; //Get the player's pickaxe speed times 100 (done to prevent it rounding to 0 and pick speed is between 0 and 1)
            int drillspeed2 = (int)drillspeed; //Convert float to int

            if (GetInstance<Configurations>().FastDrill4U)
            {
                if ((item.pick >= 1 || item.axe >= 1 || item.hammer >= 1) && item.channel) // Should detect the item as a mech tool like this unless the modder did it wrong
                {
                    item.useTime = basedrillspeed * drillspeed2 / 100; //Multiple the base use time by the player's pickspeed divided by 100
                }
            }
            if (GetInstance<Configurations>().FastChop4U)
            {
                if ((item.axe >= 1 || item.hammer >= 1) && !item.channel && item.pick == 0) // Should detect the item as a hammmer/axe like this unless the modder did it wrong
                {
                    item.useTime = basedrillspeed * drillspeed2 / 100; //Multiple the base use time by the player's pickspeed divided by 100
                }
            }
            /*if (item.type == ItemID.CobaltDrill || item.type == ItemID.PalladiumDrill)
            {
                item.useTime = 7 * drillspeed2 / 100;
                //Main.NewText("Pick speed = " + 7 * drillspeed2 / 100, 47, 86, 146);
            }
            if (item.type == ItemID.MythrilDrill)
            {
                item.useTime = 6 * drillspeed2 / 100;
            }
            if (item.type == ItemID.OrichalcumDrill)
            {
                item.useTime = 5 * drillspeed2 / 100;
            }
            if (item.type == ItemID.AdamantiteDrill || item.type == ItemID.TitaniumDrill || item.type == ItemID.Drax || item.type == ItemID.ChlorophyteDrill)
            {
                item.useTime = 4 * drillspeed2 / 100;
            }
            if (item.type == ItemID.LaserDrill)
            {
                item.useTime = 6 * drillspeed2 / 100;
            }
            if (item.type == ItemID.SolarFlareDrill || item.type == ItemID.VortexDrill || item.type == ItemID.NebulaDrill || item.type == ItemID.StardustDrill)
            {
                item.useTime = 2 * drillspeed2 / 100;
            }
            //mine
            if (item.type == ModContent.ItemType<Items.Tools.FastDrill>())
            {
                item.useTime = 5 * drillspeed2 / 100;
            }
            if (item.type == ModContent.ItemType<Items.Tools.FastDrill2>())
            {
                item.useTime = 4 * drillspeed2 / 100;
            }
            if (item.type == ModContent.ItemType<Items.Tools.DerplingDrill>() || item.type == ModContent.ItemType<Items.Tools.SpaceRockDrillSaw>())
            {
                item.useTime = 3 * drillspeed2 / 100;
            }
            if (item.type == ModContent.ItemType<Items.Tools.SantankDrill>())
            {
                item.useTime = 2 * drillspeed2 / 100;
            }*/

        }
    }
    public class Configplayereffects : ModPlayer //QoL for the world
    {
        public override void PostUpdateEquips() //Updates every frame
        {


        }
    }
    public class ConfigNPCeffects : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {

        }
        
    }
    public class Configworldeffects : ModSystem //QoL for the world
    {

        public override void PreUpdateWorld()
        {
            if (Main.expertMode)
            {
                if (NPC.ShieldStrengthTowerVortex > GetInstance<Configurations>().shieldHealthExpert)
                {
                    NPC.ShieldStrengthTowerVortex = GetInstance<Configurations>().shieldHealthExpert;
                }
                if (NPC.ShieldStrengthTowerSolar > GetInstance<Configurations>().shieldHealthExpert)
                {
                    NPC.ShieldStrengthTowerSolar = GetInstance<Configurations>().shieldHealthExpert;
                }
                if (NPC.ShieldStrengthTowerNebula > GetInstance<Configurations>().shieldHealthExpert)
                {
                    NPC.ShieldStrengthTowerNebula = GetInstance<Configurations>().shieldHealthExpert;
                }
                if (NPC.ShieldStrengthTowerStardust > GetInstance<Configurations>().shieldHealthExpert)
                {
                    NPC.ShieldStrengthTowerStardust = GetInstance<Configurations>().shieldHealthExpert;
                }
            }
            if (!Main.expertMode)
            {
                if (NPC.ShieldStrengthTowerVortex > GetInstance<Configurations>().shieldHealthNormal)
                {
                    NPC.ShieldStrengthTowerVortex = GetInstance<Configurations>().shieldHealthNormal;
                }
                if (NPC.ShieldStrengthTowerSolar > GetInstance<Configurations>().shieldHealthNormal)
                {
                    NPC.ShieldStrengthTowerSolar = GetInstance<Configurations>().shieldHealthNormal;
                }
                if (NPC.ShieldStrengthTowerNebula > GetInstance<Configurations>().shieldHealthNormal)
                {
                    NPC.ShieldStrengthTowerNebula = GetInstance<Configurations>().shieldHealthNormal;
                }
                if (NPC.ShieldStrengthTowerStardust > GetInstance<Configurations>().shieldHealthNormal)
                {
                    NPC.ShieldStrengthTowerStardust = GetInstance<Configurations>().shieldHealthNormal;
                }

            }
        }
    }
}