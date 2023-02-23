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
using Terraria.Audio;


using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

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
      
        [Header("Damage")]

        [Label("Enable Super Crits")]
        [Tooltip("Every percentage your weapons crit chance is above 100% is the chance for a super crit that deals x3 damage (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool superCrit { get; set; }

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


        [Label("Prevent your own explosives from harming you")]
        [Tooltip("This will prevent any explosive item you launch/throw from inflicting self damage (Doesn't work with explosive Bullets) (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool NoBoomBoom { get; set; }

        [Label("Falling Stars fall as items instead of projectiles")]
        [Tooltip("This will turn all falling stars into the item as soon as they spawn instead of being a damaging projectile, useful if you want an uninterrupted boss fight")]
        [DefaultValue(false)]
        public bool NoStar4U { get; set; }
    }
    public class StormQoL : Mod
    {
        public override void Load()
        {
           
            if (GetInstance<Configurations>().noDamageSpread)
            {
                //!!All credit goes to Kojo's mod called Rho's Playground!!
                On.Terraria.Main.DamageVar += (orig, damage, luck) => (int)Math.Round(damage * Main.rand.NextFloat(1, 1)); //No damage variance
            }
            base.Load();
        }
        public override void Unload()
        {
            base.Unload();
        }

    }
    public class Projchanges : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (GetInstance<Configurations>().NoBoomBoom)
            {
                if (projectile.friendly)
                {
                    ProjectileID.Sets.RocketsSkipDamageForPlayers[projectile.type] = true;
                }
                if (projectile.type == ProjectileID.ExplosiveBullet)
                {
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                }
            }
            base.SetDefaults(projectile);
        }

        public override void AI(Projectile projectile)
        {
            if (GetInstance<Configurations>().NoStar4U)
            {
                if (projectile.type == ProjectileID.FallingStar)
                {
                    projectile.Kill();
                    //Main.NewText("Lmfao get rekt star lmao", 255, 127, 127);
                }
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
        }
    }
    public class Configplayereffects : ModPlayer //QoL for the world
    {
        public override void PostUpdateEquips() //Updates every frame
        {

        }
    
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (GetInstance<Configurations>().NoBoomBoom)
            {
                if (proj.type == ProjectileID.ExplosiveBullet)
                {
                    return false;
                }
            }
            return base.CanBeHitByProjectile(proj);
        }
    }
    public class VanillaTooltips : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void UpdateInventory(Item item, Player player)
        {

        }
        public override void HoldItem(Item item, Player player)
        {
            base.HoldItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if (GetInstance<Configurations>().superCrit)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (item.CountsAsClass(DamageClass.Generic))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) - 100)))) + "[c/DBBD00:% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Melee) + (int)player.GetCritChance(DamageClass.MeleeNoSpeed) - 100)))) + "[c/DBBD00:% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Ranged) && item.ammo == 0)
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Ranged) - 100)))) + "[c/DBBD00:% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Magic))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Magic) - 100)))) + "[c/DBBD00:% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Summon))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Summon) - 100)))) + "[c/DBBD00:% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Throwing))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Throwing) - 100)))) + "[c/DBBD00:% super crit chance]";
                        }
                    }
                }
            }
        }
    }
    public class ConfigNPCeffects : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {

        }
        int classlesspain;
        int meleepain;
        int rangedpain;
        int magicpain;
        int summonpain;
        int throwingpain;
    
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            var player = Main.player[projectile.owner];
            if (GetInstance<Configurations>().superCrit)
            {
                classlesspain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) - 100));
                meleepain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Melee) + (int)player.GetCritChance(DamageClass.MeleeNoSpeed) - 100));
                rangedpain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Ranged) - 100));
                magicpain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Magic) - 100));
                summonpain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Summon) - 100));
                throwingpain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Throwing) - 100));

                {
                    if (((Main.rand.Next(100) < (classlesspain) && player.HeldItem.CountsAsClass(DamageClass.Generic) && projectile.CountsAsClass(DamageClass.Generic)) ||
                        (Main.rand.Next(100) < (meleepain) && (player.HeldItem.CountsAsClass(DamageClass.Melee) || player.HeldItem.CountsAsClass(DamageClass.MeleeNoSpeed)) && (projectile.CountsAsClass(DamageClass.Melee) || projectile.CountsAsClass(DamageClass.MeleeNoSpeed))) ||
                        (Main.rand.Next(100) < (rangedpain) && player.HeldItem.CountsAsClass(DamageClass.Ranged) && projectile.CountsAsClass(DamageClass.Ranged)) ||
                        (Main.rand.Next(100) < (magicpain) && player.HeldItem.CountsAsClass(DamageClass.Magic) && projectile.CountsAsClass(DamageClass.Magic)) ||
                        (Main.rand.Next(100) < (summonpain) && player.HeldItem.CountsAsClass(DamageClass.Summon) && projectile.CountsAsClass(DamageClass.Summon)) ||
                        (Main.rand.Next(100) < (throwingpain) && player.HeldItem.CountsAsClass(DamageClass.Throwing) && projectile.CountsAsClass(DamageClass.Throwing))) && crit
                        )
                    {
                        if (npc.lifeMax > 5)
                        {
                            damage = (int)(damage * 1.5f);

                            CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 12, 4), Color.Yellow, "Super Crit!", true);
                            CombatText.UpdateCombatText();

                            SoundEngine.PlaySound(SoundID.NPCDeath56 with { Volume = 0.2f, Pitch = 1f }, npc.Center);
                            for (int i = 0; i < 30; i++)
                            {
                                Dust dust;
                                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                Vector2 position = npc.position;
                                dust = Main.dust[Terraria.Dust.NewDust(position, npc.width, npc.height, 169, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                                dust.noGravity = true;
                            }
                        }
                    }
                }
                //Main.NewText("Classless " + (classlesspain) + " Melee " + (meleepain) + " Ranged " + (rangedpain) + " Magic " + (magicpain) + " Summon " + (summonpain) + " Throwing " + (throwingpain), 0, 204, 170);
            }
            base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (GetInstance<Configurations>().noDamageSpread)
            {
                meleepain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Melee) + (int)player.GetCritChance(DamageClass.MeleeNoSpeed) - 100));
                if ((Main.rand.Next(100) < (meleepain) && (player.HeldItem.CountsAsClass(DamageClass.Melee) || player.HeldItem.CountsAsClass(DamageClass.MeleeNoSpeed))) && crit)
                {
                    if (npc.lifeMax > 5 && npc.CanBeChasedBy())
                    {
                        damage = (int)(damage * 1.5f);

                        CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 12, 4), Color.Yellow, "Super Crit!", true);
                        CombatText.UpdateCombatText();

                        SoundEngine.PlaySound(SoundID.NPCDeath56 with { Volume = 0.2f, Pitch = 1f }, npc.Center);
                        for (int i = 0; i < 30; i++)
                        {
                            Dust dust;
                            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                            Vector2 position = npc.position;
                            dust = Main.dust[Terraria.Dust.NewDust(position, npc.width, npc.height, 169, 0f, 0f, 0, new Color(255, 255, 255), 1f)];
                            dust.noGravity = true;
                        }
                    }
                }
                //Main.NewText("Melee " + (meleepain), 0, 204, 170);
            }
            base.ModifyHitByItem(npc, player, item, ref damage, ref knockback, ref crit);
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