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
using Terraria.GameContent.ItemDropRules;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.CodeAnalysis;
using Terraria.WorldBuilding;
using Terraria.GameContent.Bestiary;

namespace StormQoL
{
    public class Configurations : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Celestial Event Shield")]

        [Label("Pillar shield kill count")]
        [Tooltip("How many enemies will have to be defeated for the shield to be destroyed in Classic difficulty")]
        [Range(10, 100)]
        [Slider]
        [DefaultValue(100)]
        //[ReloadRequired]
        public int shieldHealthNormal;

        [Header("Damage")]
        [Label("Set Custom Damage variance")]
        [Tooltip("Allows you to set a custom damage variance percentage for all damage dealt and taken (Requires reload)")]
        [ReloadRequired] //Yes
        [Range(0, 100)]
        [Slider]
        [DefaultValue(15)]
        public int DamageSpread { get; set; }

        [Label("Enable Super Crits")]
        [Tooltip("Every percentage your weapons crit chance is above 100% is the chance for a super crit that deals x3 damage (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool superCrit { get; set; }

        [Header("Player Tweaks")]

        [Label("Prevent your own explosives from harming you")]
        [Tooltip("This will prevent any explosive item you launch/throw from inflicting self-damage (Doesn't work with explosive Bullets) (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool NoBoomBoom { get; set; }

        [Label("Prevent certain generated traps from harming you.")]
        [Tooltip("Prevents traps such as dart traps, boulders, and explosives from harming you, does not affect lihzarhd traps or 'No Traps' worlds")]
        [DefaultValue(false)]
        public bool NoTraps { get; set; }

        [Label("Don't drop a tombstone on death")]
        [Tooltip("Stops you from dropping a tombstone on death, great for preventing unwanted graveyards or just grave clutter.")]
        [DefaultValue(false)]
        public bool FckGraves { get; set; }

        [Label("Prevent being chilled in expert snow biome water")]
        [Tooltip("Prevents you from being inflicted with the chilled debuff while in water in an expert snow biome.")]
        [DefaultValue(false)]
        public bool NoChill { get; set; }

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
        [Header("Shimmer")]

        [Label("Allows boss and event boss drops to be shimmer-able")]
        [Tooltip("This will allow most weapons and armour dropped from bosses/minibosses to be shimmered into another drop from the same boss (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool RIPBossRNG { get; set; }

        [Label("Grant total immunity to sinking in shimmer")]
        [Tooltip("This will prevent you from falling through the floor if you land in shimmer, it even prevents it if you accidentally hold down while wearing the cloak.")]
        [DefaultValue(false)]
        public bool NoSink { get; set; }

        [Header("Misc")]

        [Label("Unlock full bestiary entries with just 1 kill")]
        [Tooltip("This will make it so killing a single enemy unlocks its entire bestiary entry (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool bestUnlocks { get; set; }

        [Label("Prevent prehardmode enemy stats from scaling in expert hardmode")]
        [Tooltip("This will prevent all prehardmode enemies from having their stats scaled in expert mode during hardmode (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool NoStronk { get; set; }

        [Label("Prevent Treasure Bags from dropping dev items")]
        [Tooltip("If you have a lot of bags to open and don't want your inventory cluttered by dev items (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        public bool NoInventoryClutter { get; set; }

        [Label("Falling Stars fall as items instead of projectiles")]
        [Tooltip("This will turn all falling stars into the item as soon as they spawn instead of being a damaging projectile, useful if you want an uninterrupted boss fight")]
        [DefaultValue(false)]
        public bool NoStar4U { get; set; }

        [Label("Make Blazing Wheels and Spike balls killable")]
        [Tooltip("This will allow you to deal damage and kill Blazing Wheels and Dungeon Spike balls, useful if one spawned in an awkward place")]
        [DefaultValue(false)]
        public bool RIPdungeon { get; set; }

    }
    public class StormQoL : Mod
    {
        public override void Load()
        {
            //!!All credit goes to Kojo's mod called Rho's Playground!!
            //On.Terraria.Main.DamageVar += (orig, damage, luck) => (int)Math.Round(damage * Main.rand.NextFloat(1, 1)); //No damage variance
            Main.DefaultDamageVariationPercent = GetInstance<Configurations>().DamageSpread;
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
            if (GetInstance<Configurations>().NoTraps && !Main.noTrapsWorld)
            {
                if (projectile.type is ProjectileID.Boulder or ProjectileID.PoisonDart or ProjectileID.Explosives or ProjectileID.GeyserTrap or ProjectileID.RollingCactus or ProjectileID.RollingCactusSpike)
                {
                    projectile.hostile = false;
                }
            }
            if (GetInstance<Configurations>().FckGraves)
            {
                if (projectile.type is ProjectileID.Tombstone or ProjectileID.GraveMarker or ProjectileID.CrossGraveMarker or ProjectileID.Gravestone or ProjectileID.Headstone or ProjectileID.Obelisk
                    or ProjectileID.RichGravestone1 or ProjectileID.RichGravestone2 or ProjectileID.RichGravestone3 or ProjectileID.RichGravestone4 or ProjectileID.RichGravestone5)
                {
                    projectile.tileCollide = false;
                    projectile.Kill();
                }
            }
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
            //shimmers

            if (GetInstance<Configurations>().RIPBossRNG)
            {
                //The King of the slimes
                if (item.type == ItemID.NinjaHood)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.NinjaShirt;
                if (item.type == ItemID.NinjaShirt)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.NinjaPants;
                if (item.type == ItemID.NinjaPants)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.NinjaHood;
                //The Queen of the bees
                if (item.type == ItemID.BeeKeeper)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BeesKnees;
                if (item.type == ItemID.BeesKnees)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BeeGun;
                if (item.type == ItemID.BeeGun)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.HoneyComb;
                if (item.type == ItemID.HoneyComb)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BeeKeeper;
                //The Clops of the deers
                if (item.type == ItemID.LucyTheAxe)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.PewMaticHorn;
                if (item.type == ItemID.PewMaticHorn)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.WeatherPain;
                if (item.type == ItemID.WeatherPain)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.HoundiusShootius;
                if (item.type == ItemID.HoundiusShootius)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.LucyTheAxe;
                //The flesh made of walls
                if (item.type == ItemID.BreakerBlade)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ClockworkAssaultRifle;
                if (item.type == ItemID.ClockworkAssaultRifle)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.LaserRifle;
                if (item.type == ItemID.LaserRifle)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.FireWhip;
                if (item.type == ItemID.FireWhip)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BreakerBlade;
                //The queen of the slimes
                if (item.type == ItemID.CrystalNinjaHelmet)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.CrystalNinjaChestplate;
                if (item.type == ItemID.CrystalNinjaChestplate)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.CrystalNinjaLeggings;
                if (item.type == ItemID.CrystalNinjaLeggings)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.CrystalNinjaHelmet;
                //The plant of era
                if (item.type == ItemID.Seedler)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.FlowerPow;
                if (item.type == ItemID.FlowerPow)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.GrenadeLauncher;
                if (item.type == ItemID.GrenadeLauncher)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.VenusMagnum;
                if (item.type == ItemID.VenusMagnum)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.NettleBurst;
                if (item.type == ItemID.NettleBurst)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.LeafBlower;
                if (item.type == ItemID.LeafBlower)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.WaspGun;
                if (item.type == ItemID.WaspGun)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Seedler;
                //The go of lem
                if (item.type == ItemID.GolemFist)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.PossessedHatchet;
                if (item.type == ItemID.PossessedHatchet)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Stynger;
                if (item.type == ItemID.Stynger)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.HeatRay;
                if (item.type == ItemID.HeatRay)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.StaffofEarth;
                if (item.type == ItemID.StaffofEarth)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.SunStone;
                if (item.type == ItemID.SunStone)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.EyeoftheGolem;
                if (item.type == ItemID.EyeoftheGolem)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.GolemFist;
                //The Duke of the fish of rons
                if (item.type == ItemID.Flairon)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Tsunami;
                if (item.type == ItemID.Tsunami)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.RazorbladeTyphoon;
                if (item.type == ItemID.RazorbladeTyphoon)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BubbleGun;
                if (item.type == ItemID.BubbleGun)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.TempestStaff;
                if (item.type == ItemID.TempestStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Flairon;
               
                //The light of the empress at night
                if (item.type == ItemID.PiercingStarlight)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.FairyQueenRangedItem;
                if (item.type == ItemID.FairyQueenRangedItem)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.FairyQueenMagicItem;
                if (item.type == ItemID.FairyQueenMagicItem)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.RainbowWhip;
                if (item.type == ItemID.RainbowWhip)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.PiercingStarlight;

                //The lord of the Moon
                if (item.type == ItemID.Terrarian)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Meowmere;
                if (item.type == ItemID.Meowmere)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.StarWrath;
                if (item.type == ItemID.StarWrath)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Celeb2;
                if (item.type == ItemID.Celeb2)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.SDMG;
                if (item.type == ItemID.SDMG)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.LastPrism;
                if (item.type == ItemID.LastPrism)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.LunarFlareBook;
                if (item.type == ItemID.LunarFlareBook)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.RainbowCrystalStaff;
                if (item.type == ItemID.RainbowCrystalStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.MoonlordTurretStaff;
                if (item.type == ItemID.MoonlordTurretStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Terrarian;

                //EVENT bosses
                //The mage of darkness
                ///N/A
                //Get of of my swamp
                if (item.type == ItemID.DD2SquireDemonSword)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.MonkStaffT1;
                if (item.type == ItemID.MonkStaffT1)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.MonkStaffT2;
                if (item.type == ItemID.MonkStaffT2)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.DD2PhoenixBow;
                if (item.type == ItemID.DD2PhoenixBow)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BookStaff;
                if (item.type == ItemID.BookStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.DD2SquireDemonSword;
                //The best of the sy
                if (item.type == ItemID.DD2SquireBetsySword)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.MonkStaffT3;
                if (item.type == ItemID.MonkStaffT3)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.DD2BetsyBow;
                if (item.type == ItemID.DD2BetsyBow)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ApprenticeStaffT3;
                if (item.type == ItemID.ApprenticeStaffT3)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.DD2SquireBetsySword;
                //The wood that mourns
                if (item.type == ItemID.StakeLauncher)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.NecromanticScroll;
                if (item.type == ItemID.NecromanticScroll)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.StakeLauncher;                
                //The King of pumps
                if (item.type == ItemID.TheHorsemansBlade)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.CandyCornRifle;
                if (item.type == ItemID.CandyCornRifle)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.JackOLanternLauncher;
                if (item.type == ItemID.JackOLanternLauncher)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BatScepter;
                if (item.type == ItemID.BatScepter)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.RavenStaff;
                if (item.type == ItemID.RavenStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ScytheWhip;
                if (item.type == ItemID.ScytheWhip)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.TheHorsemansBlade;
                //Tree of chirstmas screams
                if (item.type == ItemID.ChristmasTreeSword)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Razorpine;
                if (item.type == ItemID.Razorpine)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ChristmasTreeSword;
                //The Tank of santas
                if (item.type == ItemID.ElfMelter)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ChainGun;
                if (item.type == ItemID.ChainGun)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ElfMelter;
                //The queen of ice
                if (item.type == ItemID.NorthPole)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.SnowmanCannon;
                if (item.type == ItemID.SnowmanCannon)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BlizzardStaff;
                if (item.type == ItemID.BlizzardStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.NorthPole;
                //The sauce of Mars
                if (item.type == ItemID.InfluxWaver)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.Xenopopper;
                if (item.type == ItemID.Xenopopper)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ElectrosphereLauncher;
                if (item.type == ItemID.ElectrosphereLauncher)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.LaserMachinegun;
                if (item.type == ItemID.LaserMachinegun)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.XenoStaff;
                if (item.type == ItemID.XenoStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.CosmicCarKey;
                if (item.type == ItemID.CosmicCarKey)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.InfluxWaver;
                //Events other
                //phm bloodmoon
                if (item.type == ItemID.BloodRainBow)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.VampireFrogStaff;
                if (item.type == ItemID.VampireFrogStaff)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.BloodRainBow;
                //hm bloodmoon
                if (item.type == ItemID.SharpTears)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.DripplerFlail;
                if (item.type == ItemID.DripplerFlail)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.SharpTears;

                //noblins
                if (item.type == ItemID.ShadowFlameKnife)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ShadowFlameBow;
                if (item.type == ItemID.ShadowFlameBow)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ShadowFlameHexDoll;
                if (item.type == ItemID.ShadowFlameHexDoll)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ShadowFlameKnife;
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
            if (GetInstance<Configurations>().NoSink)
            {
                Player.buffImmune[BuffID.Shimmer] = true;
            }

            if (GetInstance<Configurations>().NoChill)
            {
                if (Player.wet && Player.ZoneSnow && Main.expertMode)
                {
                    Player.buffImmune[BuffID.Chilled] = true;
                }
            }
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
                            line.Text = line.Text + "\n[c/DBBD00:" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) - 100)))) + "% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Melee))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n[c/DBBD00:" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Melee) + (int)player.GetCritChance(DamageClass.MeleeNoSpeed) - 100)))) + "% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Ranged) && item.ammo == 0)
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n[c/DBBD00:" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Ranged) - 100)))) + "% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Magic))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n[c/DBBD00:" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Magic) - 100)))) + "% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Summon))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n[c/DBBD00:" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Summon) - 100)))) + "% super crit chance]";
                        }
                    }
                    if (item.CountsAsClass(DamageClass.Throwing))
                    {
                        if (line.Mod == "Terraria" && line.Name == "CritChance")
                        {
                            line.Text = line.Text + "\n[c/DBBD00:" + (Math.Min(100, Math.Max(0, (item.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Throwing) - 100)))) + "% super crit chance]";
                        }
                    }
                }
            }
        }
    }
    public class ConfigNPCeffects : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults(NPC npc)
        {
            if (GetInstance<Configurations>().NoStronk)
            {
                NPCID.Sets.DontDoHardmodeScaling[npc.type] = true;
            }
        }
        public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            if (GetInstance<Configurations>().bestUnlocks)
            {
                bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[npc.type], quickUnlock: true);
            }
            base.SetBestiary(npc, database, bestiaryEntry);
        }
        public override void AI(NPC npc)
        {
            if (GetInstance<Configurations>().RIPdungeon)
            {
                if (npc.type is NPCID.SpikeBall or NPCID.BlazingWheel)
                {
                    npc.dontTakeDamage = false;
                }
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (GetInstance<Configurations>().RIPdungeon)
            {
                if (npc.type is NPCID.SpikeBall)
                {
                    npcLoot.Add(ItemDropRule.Common(ItemID.Spike, 1, 5, 10));
                }
                if (npc.type is NPCID.BlazingWheel)
                {
                    npcLoot.Add(ItemDropRule.Common(ItemID.LivingFireBlock, 1, 10, 20));
                }
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (GetInstance<Configurations>().RIPdungeon)
            {
                if (npc.life <= 0)
                {
                    if (npc.type is NPCID.SpikeBall or NPCID.BlazingWheel)
                    {
                        int proj = Projectile.NewProjectile(null, new Vector2(npc.Center.X, npc.Center.Y), new Vector2(0, 0), ProjectileID.SolarWhipSwordExplosion, 0, 0, Main.myPlayer);
                        Main.projectile[proj].damage = 0;
                        Main.projectile[proj].tileCollide = false;
                    }
                }
            }
        }
        public override void OnKill(NPC npc)
        {
            
        }
        int classlesspain;
        int meleepain;
        int rangedpain;
        int magicpain;
        int summonpain;
        int throwingpain;

        bool hitindicator;
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            var player = Main.player[projectile.owner];
            if (GetInstance<Configurations>().superCrit)
            {
                //Main.NewText("Pain is " + (player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Ranged) - 100) + " %", 0, 204, 170);
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
                        (Main.rand.Next(100) < (throwingpain) && player.HeldItem.CountsAsClass(DamageClass.Throwing) && projectile.CountsAsClass(DamageClass.Throwing)))
                        )
                    {
                        if (npc.lifeMax > 5)
                        {
                            modifiers.CritDamage *= 1.5f;
                            npc.HideStrikeDamage = true;
                            hitindicator = true;
                            //CombatText.UpdateCombatText();
                        }
                    }
                }
                //Main.NewText("Classless " + (classlesspain) + " Melee " + (meleepain) + " Ranged " + (rangedpain) + " Magic " + (magicpain) + " Summon " + (summonpain) + " Throwing " + (throwingpain), 0, 204, 170);
            }
            base.ModifyHitByProjectile(npc, projectile, ref modifiers);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (hitindicator)
            {
                if (hit.Crit) //have to put it here because of the stupid new methods
                {
                    CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 12, 4), Color.Gold, "" + hit.Damage, true);

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
                else
                {
                    CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 12, 4), Color.Orange, "" + hit.Damage, false);

                }
                npc.HideStrikeDamage = false;
                hitindicator = false;
            }
            base.OnHitByProjectile(npc, projectile, hit, damageDone);
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (GetInstance<Configurations>().superCrit)
            {
                meleepain = Math.Min(100, Math.Max(0, player.HeldItem.crit + (int)player.GetCritChance(DamageClass.Generic) + (int)player.GetCritChance(DamageClass.Melee) + (int)player.GetCritChance(DamageClass.MeleeNoSpeed) - 100));
                if ((Main.rand.Next(100) < (meleepain) && (player.HeldItem.CountsAsClass(DamageClass.Melee) || player.HeldItem.CountsAsClass(DamageClass.MeleeNoSpeed))))
                {
                    if (npc.lifeMax > 5)
                    {
                        modifiers.CritDamage *= 1.5f;
                        npc.HideStrikeDamage = true;
                        hitindicator = true;
                    }
                }
                //Main.NewText("Melee " + (meleepain), 0, 204, 170);
            }
            base.ModifyHitByItem(npc, player, item, ref modifiers);
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (hitindicator)
            {
                if (hit.Crit) //have to put it here because of the stupid new methods
                {
                    CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 12, 4), Color.Gold, "" + hit.Damage, true);

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
                else
                {
                    CombatText.NewText(new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 12, 4), Color.Orange, "" + hit.Damage, false);

                }
                npc.HideStrikeDamage = false;
                hitindicator = false;
            }
            base.OnHitByItem(npc, player, item, hit, damageDone);
        }
    }
    
    public class Configworldeffects : ModSystem //QoL for the world
    {
        public override void PreUpdateWorld()
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