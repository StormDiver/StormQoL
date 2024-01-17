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
using MonoMod.Cil;
using Terraria.Localization;
using Terraria.Social.WeGame;
using static Humanizer.In;

namespace StormQoL
{
    [BackgroundColor(5, 51, 34, 200)]

    public class Configurations : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("CelestialEventShield")]

        //[Label("Enable custom Pillar shield kill count")]
        //[Tooltip("If on then the "Pillar shield kill count" configuration below will be applied")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]

        public bool shieldHealthNormalbool { get; set; }

        //[Label("Pillar shield kill count")]
        //[Tooltip("How many enemies will have to be defeated for the shield to be destroyed")]
        [Range(10, 100)]
        [Slider]
        [DefaultValue(100)]
        [BackgroundColor(27, 130, 114)]
        public int shieldHealthNormal { get; set; }

        [Header("Damage")]
        //[Label("Enable custom Damage variance")]
        //[Tooltip("If on then the "Set custom damage variance" configuration below will be applied (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool DamageSpreadbool { get; set; }

        //[Label(" -Set Custom Damage variance")]
        //[Tooltip("Allows you to set a custom damage variance percentage for all damage dealt and taken (Requires reload)")]
        [ReloadRequired] //Yes
        [Range(0, 100)]
        [Slider]
        [DefaultValue(15)]
        [BackgroundColor(27, 130, 114)]
        public int DamageSpread { get; set; }

        //[Label("Enable Super Crits")]
        //[Tooltip("Every percentage your weapons crit chance is above 100% is the chance for a super crit that deals x3 damage (Requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool superCrit { get; set; }

        //[Label("Display exact use speed and knockback values")]
        //[Tooltip("Shows the exact usetime and knockback values of all items next to their respective tooltip lines")]
        //[ReloadRequired] //No
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool moreInfo { get; set; }

        [Header("PlayerTweaks")]

        //[Label("Enable custom respawn timer")]
        //[Tooltip("If on then the "Respawn timer" configuration below will be applied")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool Respwnedbool { get; set; }

        //[Label(" -Respawn timer (seconds)")]
        //[Tooltip("Allows you to choose how long the cooldown for respawning is")]
        [Range(0, 30)]
        [Slider]
        [DefaultValue(15)]
        [BackgroundColor(27, 130, 114)]
        public int Respwned { get; set; }

        //[Label("Respawn with full health")]
        //[Tooltip("Makes you respawn with a full bar of health")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool TheHealth { get; set; }

        //[Label("Prevent your own explosives from harming you")]
        //[Tooltip("This will prevent any explosive item you launch/throw from inflicting self-damage (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoBoomBoom { get; set; }

        //[Label("Prevent certain generated traps from harming you.")]
        //[Tooltip("Prevents traps such as dart traps, boulders, and explosives from harming you, does not affect lihzarhd traps or 'No Traps' worlds")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoTraps { get; set; }

        //[Label("Don't drop a tombstone on death")]
        //[Tooltip("Stops you from dropping a tombstone on death, great for preventing unwanted graveyards or just grave clutter.")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool FckGraves { get; set; }

        //[Label("Prevent being chilled in expert snow biome water")]
        //[Tooltip("Prevents you from being inflicted with the chilled debuff while in water in an expert snow biome.")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoChill { get; set; }

        [Header("MiningSpeedTweaks")]

        //[Label("Allow Axes and Hammers to benefit from mining buffs")]
        //[Tooltip("Allows Axes and Hammers to mine faster if you have any mining buffs equipped, works with modded tools too (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool FastChop4U { get; set; }

        //[Label("Allow mechanical tools to benefit from mining buffs")]
        //[Tooltip("Allows mechanical tools to mine faster if you have any mining buffs equipped, works with modded tools too (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool FastDrill4U { get; set; }

        //[Label("Enable 3x3 mining for drills")]
        //[Tooltip("Allows drills to mine in a 3x3 area when holding right click while mining, also applies for walls with Jackhammers)]
        //[ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool BigBoiDrill { get; set; }

        //[Label("Allows unsafe walls to be broken from the inside")]
        //[Tooltip("Allows hammers to break unsafe walls even if they're surrounded by unsafe walls")]
        //[ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool HammarTime { get; set; }

        //[Label("Choose block placement speed")]
        //[Tooltip("Allows you to make block placing faster, with 2 speeds (Requires reload)")]
        [ReloadRequired] //Yes
        [DrawTicks]
        [OptionStrings(new string[] { "Default", "Fast", "Insanely Fast" })]
        [DefaultValue("Default")]
        [BackgroundColor(27, 130, 114)]
        public string SpeedPlace { get; set; }

        [Header("Shimmer")]

        //[Label("Allows boss and event boss drops to be shimmer-able")]
        //[Tooltip("This will allow most weapons and armour dropped from bosses/minibosses to be shimmered into another drop from the same boss (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool RIPBossRNG { get; set; }

        //[Label("Grant total immunity to sinking in shimmer")]
        //[Tooltip("This will prevent you from falling through the floor if you land in shimmer, it even prevents it if you accidentally hold down while wearing the cloak.")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoSink { get; set; }

        [Header("MiscTweaks")]

        //[Label("Toggle seasonal events")]
        //[Tooltip("Switch between having the Halloween or Christmas effects active\nEvents will clear after a new day if the option is set back to "none"")]
        //[ReloadRequired] //Yes
        [DrawTicks]
        [OptionStrings(new string[] { "None", "Halloween", "Christmas", "Both" })]
        [DefaultValue("None")]
        [BackgroundColor(27, 130, 114)]
        public string tistheseason { get; set; }

        //[Label("Remove NPC happiness")]
        //[Tooltip("This will completely remove the happiness system from all NPCs, also makes NPCs always sell the pylon for their favourite biome (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool noHappy4U { get; set; }

        //[Label("Unlock full bestiary entries with just 1 kill")]
        //[Tooltip("This will make it so killing a single enemy unlocks its entire bestiary entry (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool bestUnlocks { get; set; }

        //[Label("Prevent prehardmode enemy stats from scaling in expert hardmode")]
        //[Tooltip("This will prevent all prehardmode enemies from having their stats scaled in expert mode during hardmode (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoStronk { get; set; }

        //[Label("Lucky Horseshoe display exact luck values")]
        //[Tooltip("This will make the lucky horseshoe (and its tinkers) display the exact values for total luck, coin luck, and torch")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool LuckyU { get; set; }

        //[Label("Prevent Treasure Bags from dropping dev items")]
        //[Tooltip("If you have a lot of bags to open and don't want your inventory cluttered by dev items (requires reload)")]
        [ReloadRequired] //Yes
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoInventoryClutter { get; set; }

        //[Label("Falling Stars fall as items instead of projectiles")]
        //[Tooltip("This will turn all falling stars into the item as soon as they spawn instead of being a damaging projectile, useful if you want an uninterrupted boss fight")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool NoStar4U { get; set; }

        //[Label("Make Blazing Wheels and Spike balls killable")]
        //[Tooltip("This will allow you to deal damage and kill Blazing Wheels and Dungeon Spike balls, useful if one spawned in an awkward place")]
        [DefaultValue(false)]
        [BackgroundColor(27, 130, 114)]
        public bool RIPdungeon { get; set; }

    }
    public class StormQoL : Mod
    {
        public override void Load()
        {
            if (GetInstance<Configurations>().DamageSpreadbool)
            {
                Main.DefaultDamageVariationPercent = GetInstance<Configurations>().DamageSpread;
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
                    if (projectile.friendly && (projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.Ranged || projectile.DamageType == DamageClass.Magic || projectile.DamageType == DamageClass.Summon || projectile.DamageType == DamageClass.Generic || projectile.DamageType == DamageClass.Throwing)
                    || projectile.type is ProjectileID.Bomb or ProjectileID.StickyBomb or ProjectileID.BouncyBomb or ProjectileID.DryBomb or 
                    ProjectileID.WetBomb or ProjectileID.HoneyBomb or ProjectileID.LavaBomb or ProjectileID.BombFish or ProjectileID.ScarabBomb or ProjectileID.DirtBomb or ProjectileID.DirtStickyBomb
                     or ProjectileID.Dynamite or ProjectileID.StickyDynamite or ProjectileID.BouncyDynamite)
                    {
                        ProjectileID.Sets.RocketsSkipDamageForPlayers[projectile.type] = true;
                        projectile.hostile = false;
                    }
                    if (projectile.type == ProjectileID.ExplosiveBullet)
                    {
                        ProjectileID.Sets.RocketsSkipDamageForPlayers[projectile.type] = true;
                        projectile.hostile = false;
                        projectile.usesLocalNPCImmunity = true;
                        projectile.localNPCHitCooldown = 10;
                    }
                
            }
            base.SetDefaults(projectile);
            if (GetInstance<Configurations>().NoTraps && !Main.noTrapsWorld)
            {
                if (projectile.type is ProjectileID.Boulder or ProjectileID.PoisonDart or ProjectileID.Explosives or ProjectileID.GeyserTrap or
                    ProjectileID.RollingCactus or ProjectileID.RollingCactusSpike or ProjectileID.GasTrap or ProjectileID.TNTBarrel or ProjectileID.LifeCrystalBoulder)
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
            if (GetInstance<Configurations>().NoBoomBoom)
            {
                /*var player = Main.player[projectile.owner];
                if (player == Main.LocalPlayer)
                {
                    if (projectile.friendly)
                    {
                        projectile.hostile = false;
                    }
                }*/
            }
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
                //Get out of my swamp
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

                //Biome Chests
                if (item.type == ItemID.VampireKnives)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.ScourgeoftheCorruptor;
                if (item.type == ItemID.ScourgeoftheCorruptor)
                    ItemID.Sets.ShimmerTransformToItem[item.type] = ItemID.VampireKnives;
            }

            base.SetDefaults(item);
        }
        int minetime;
        int walltime;
        public override void HoldItem(Item item, Player player)
        {
            if (GetInstance<Configurations>().BigBoiDrill || GetInstance<Configurations>().HammarTime)
            {
                int xtilepos = (int)(Main.MouseWorld.X) / 16;
                int ytilepos = (int)(Main.MouseWorld.Y) / 16;


                Tile woodcenter = Main.tile[xtilepos, ytilepos];
                Tile woodtopleft = Main.tile[xtilepos - 1, ytilepos - 1];
                Tile woodtopmid = Main.tile[xtilepos - 0, ytilepos - 1];
                Tile woodtopright = Main.tile[xtilepos + 1, ytilepos - 1];
                Tile woodmidleft = Main.tile[xtilepos - 1, ytilepos - 0];
                Tile woodmidright = Main.tile[xtilepos + 1, ytilepos - 0];
                Tile woodbottomleft = Main.tile[xtilepos - 1, ytilepos + 1];
                Tile woodbottommid = Main.tile[xtilepos + 0, ytilepos + 1];
                Tile woodbottomright = Main.tile[xtilepos + 1, ytilepos + 1];

                if (GetInstance<Configurations>().HammarTime)
                {
                    if (player == Main.LocalPlayer)
                    {
                        walltime++;
                        if (player.HeldItem.hammer >= 1 && player.controlUseItem)
                        {
                            if (walltime >= player.HeldItem.useTime)
                            {
                                if (!Main.wallHouse[woodcenter.WallType])
                                {
                                    player.PickWall((int)(Main.MouseWorld.X / 16) - 0, (int)(Main.MouseWorld.Y / 16) - 0, player.HeldItem.hammer / 2);
                                    walltime = 0;
                                }
                            }
                        }
                    }
                }
                List<int> woods = new List<int>() { TileID.Trees, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                if (GetInstance<Configurations>().BigBoiDrill)
                {
                    if (player == Main.LocalPlayer)
                    {
                        minetime++;

                        if ((player.HeldItem.pick >= 1 || player.HeldItem.hammer >= 1) && player.controlUseItem && player.controlUseTile && player.HeldItem.channel == true)
                        {

                            if (minetime >= player.HeldItem.useTime + 1)
                            {
                                if (!Main.SmartCursorIsUsed) //regular mining
                                {
                                    //if (Vector2.Distance(player.Center, Main.MouseWorld) <= (6 * 16) + (player.HeldItem.tileBoost * 16)) //6 tiles plus bonus reach
                                    if (player.Center.X <= Main.MouseWorld.X + (8 * 16) + (player.HeldItem.tileBoost * 16) + (player.blockRange * 16) &&
                                        player.Center.X >= Main.MouseWorld.X - (8 * 16) - (player.HeldItem.tileBoost * 16) - (player.blockRange * 16) &&
                                        player.Center.Y <= Main.MouseWorld.Y + (6 * 16) + (player.HeldItem.tileBoost * 16) + (player.blockRange * 16) &&
                                        player.Center.Y >= Main.MouseWorld.Y - (6 * 16) - (player.HeldItem.tileBoost * 16) - (player.blockRange * 16))
                                    {
                                        // pickaxe, make sure not wood
                                        if (player.HeldItem.pick >= 1 && !Main.tileAxe[woodcenter.TileType] && Main.tile[xtilepos, ytilepos].HasTile)
                                        {
                                            //prevent trees being mined
                                            if (!Main.tileAxe[woodtopleft.TileType])
                                                player.PickTile((int)(xtilepos) - 1, (int)(ytilepos) - 1, player.HeldItem.pick);

                                            if (!Main.tileAxe[woodtopmid.TileType])
                                                player.PickTile((int)(xtilepos) - 0, (int)(ytilepos) - 1, player.HeldItem.pick);

                                            if (!Main.tileAxe[woodtopright.TileType])
                                                player.PickTile((int)(xtilepos) + 1, (int)(ytilepos) - 1, player.HeldItem.pick);

                                            if (!Main.tileAxe[woodmidleft.TileType])
                                                player.PickTile((int)(xtilepos) - 1, (int)(ytilepos) - 0, player.HeldItem.pick);

                                            if (!Main.tileAxe[woodmidright.TileType])
                                                player.PickTile((int)(xtilepos) + 1, (int)(ytilepos) - 0, player.HeldItem.pick);

                                            if (!Main.tileAxe[woodbottomleft.TileType])
                                                player.PickTile((int)(xtilepos) - 1, (int)(ytilepos) + 1, player.HeldItem.pick);

                                            if (!Main.tileAxe[woodbottommid.TileType])
                                                player.PickTile((int)(xtilepos) - 0, (int)(ytilepos) + 1, player.HeldItem.pick);

                                            if (!Main.tileAxe[woodbottomright.TileType])
                                                player.PickTile((int)(xtilepos) + 1, (int)(ytilepos) + 1, player.HeldItem.pick);
                                        }
                                        //hammer
                                        if (player.HeldItem.hammer >= 1)
                                        {
                                            player.PickWall((int)(xtilepos) - 1, (int)(ytilepos) - 1, player.HeldItem.hammer);
                                            player.PickWall((int)(xtilepos) - 0, (int)(ytilepos) - 1, player.HeldItem.hammer);
                                            player.PickWall((int)(xtilepos) + 1, (int)(ytilepos) - 1, player.HeldItem.hammer);
                                            player.PickWall((int)(xtilepos) - 1, (int)(ytilepos) - 0, player.HeldItem.hammer);
                                            player.PickWall((int)(xtilepos) + 1, (int)(ytilepos) - 0, player.HeldItem.hammer);
                                            player.PickWall((int)(xtilepos) - 1, (int)(ytilepos) + 1, player.HeldItem.hammer);
                                            player.PickWall((int)(xtilepos) - 0, (int)(ytilepos) + 1, player.HeldItem.hammer);
                                            player.PickWall((int)(xtilepos) + 1, (int)(ytilepos) + 1, player.HeldItem.hammer);
                                        }
                                    }
                                }

                                else //smart cursor mining
                                {

                                    Tile woodcentersmrt = Main.tile[Main.SmartCursorX, Main.SmartCursorY];
                                    Tile woodtopleftsmrt = Main.tile[Main.SmartCursorX - 1, Main.SmartCursorY - 1];
                                    Tile woodtopmidsmrt = Main.tile[Main.SmartCursorX - 0, Main.SmartCursorY - 1];
                                    Tile woodtoprightsmrt = Main.tile[Main.SmartCursorX + 1, Main.SmartCursorY - 1];
                                    Tile woodmidleftsmrt = Main.tile[Main.SmartCursorX - 1, Main.SmartCursorY - 0];
                                    Tile woodmidrightsmrt = Main.tile[Main.SmartCursorX + 1, Main.SmartCursorY - 0];
                                    Tile woodbottomleftsmrt = Main.tile[Main.SmartCursorX - 1, Main.SmartCursorY + 1];
                                    Tile woodbottommidsmrt = Main.tile[Main.SmartCursorX + 0, Main.SmartCursorY + 1];
                                    Tile woodbottomrightsmrt = Main.tile[Main.SmartCursorX + 1, Main.SmartCursorY + 1];
                                    //pickaxe, make sure not wood
                                    if (player.HeldItem.pick >= 1 && !Main.tileAxe[woodcentersmrt.TileType] && (Main.tile[xtilepos, ytilepos].HasTile))

                                    {
                                        //stop wood being mined
                                        if (!Main.tileAxe[woodtopleftsmrt.TileType])
                                            player.PickTile((int)(Main.SmartCursorX) - 1, (int)(Main.SmartCursorY) - 1, player.HeldItem.pick);

                                        if (!Main.tileAxe[woodtopmidsmrt.TileType])
                                            player.PickTile((int)(Main.SmartCursorX) - 0, (int)(Main.SmartCursorY) - 1, player.HeldItem.pick);

                                        if (!Main.tileAxe[woodtoprightsmrt.TileType])
                                            player.PickTile((int)(Main.SmartCursorX) + 1, (int)(Main.SmartCursorY) - 1, player.HeldItem.pick);

                                        if (!Main.tileAxe[woodmidleftsmrt.TileType])

                                            player.PickTile((int)(Main.SmartCursorX) - 1, (int)(Main.SmartCursorY) - 0, player.HeldItem.pick);

                                        if (!Main.tileAxe[woodmidrightsmrt.TileType])

                                            player.PickTile((int)(Main.SmartCursorX) + 1, (int)(Main.SmartCursorY) - 0, player.HeldItem.pick);

                                        if (!Main.tileAxe[woodbottomleftsmrt.TileType])

                                            player.PickTile((int)(Main.SmartCursorX) - 1, (int)(Main.SmartCursorY) + 1, player.HeldItem.pick);

                                        if (!Main.tileAxe[woodbottommidsmrt.TileType])

                                            player.PickTile((int)(Main.SmartCursorX) - 0, (int)(Main.SmartCursorY) + 1, player.HeldItem.pick);

                                        if (!Main.tileAxe[woodbottomrightsmrt.TileType])

                                            player.PickTile((int)(Main.SmartCursorX) + 1, (int)(Main.SmartCursorY) + 1, player.HeldItem.pick);
                                    }
                                    //hammer
                                    if (player.HeldItem.hammer >= 1)
                                    {
                                        player.PickWall((int)(Main.SmartCursorX) - 1, (int)(Main.SmartCursorY) - 1, player.HeldItem.hammer);
                                        player.PickWall((int)(Main.SmartCursorX) - 0, (int)(Main.SmartCursorY) - 1, player.HeldItem.hammer);
                                        player.PickWall((int)(Main.SmartCursorX) + 1, (int)(Main.SmartCursorY) - 1, player.HeldItem.hammer);
                                        player.PickWall((int)(Main.SmartCursorX) - 1, (int)(Main.SmartCursorY) - 0, player.HeldItem.hammer);
                                        player.PickWall((int)(Main.SmartCursorX) + 1, (int)(Main.SmartCursorY) - 0, player.HeldItem.hammer);
                                        player.PickWall((int)(Main.SmartCursorX) - 1, (int)(Main.SmartCursorY) + 1, player.HeldItem.hammer);
                                        player.PickWall((int)(Main.SmartCursorX) - 0, (int)(Main.SmartCursorY) + 1, player.HeldItem.hammer);
                                        player.PickWall((int)(Main.SmartCursorX) + 1, (int)(Main.SmartCursorY) + 1, player.HeldItem.hammer);
                                    }
                                }
                                minetime = 0;
                            }
                        }
                    }
                }
            }
    
        
            float drillspeed = player.pickSpeed * 100; //Get the player's pickaxe speed times 100 (done to prevent it rounding to 0 and pick speed is between 0 and 1)
            int drillspeed2 = (int)drillspeed; //Convert float to int

            if (GetInstance<Configurations>().FastDrill4U)
            {
                if (player == Main.LocalPlayer)
                {
                    if ((item.pick >= 1 || item.axe >= 1 || item.hammer >= 1) && item.channel) // Should detect the item as a mech tool like this unless the modder did it wrong
                    {
                        item.useTime = basedrillspeed * drillspeed2 / 100; //Multiple the base use time by the player's pickspeed divided by 100

                        if (item.useTime == 0) //sadly
                            item.useTime = 1;

                    }
                }
            }
            if (GetInstance<Configurations>().FastChop4U)
            {
                if (player == Main.LocalPlayer)
                {
                    if ((item.axe >= 1 || item.hammer >= 1) && !item.channel && item.pick == 0) // Should detect the item as a hammmer/axe like this unless the modder did it wrong
                    {
                        item.useTime = basedrillspeed * drillspeed2 / 100; //Multiple the base use time by the player's pickspeed divided by 100

                        if (item.useTime == 0)
                            item.useTime = 1;
                    }
                }
            }

            if (GetInstance<Configurations>().SpeedPlace == "Insanely Fast") //Insanely Fast placement
            {
                if (player == Main.LocalPlayer)
                {
                    if (item.consumable == true && item.useStyle == ItemUseStyleID.Swing && item.damage <= 0)
                    {
                        item.useTime = 1;
                        item.useAnimation = 1;
                    }
                }
            }
        }
    }
    public class Configplayereffects : ModPlayer //QoL for the world
    {
        
        public override void PostUpdateEquips() //Updates every frame
        {
            if (GetInstance<Configurations>().SpeedPlace == "Fast") //Fast placement
            {
                Player.tileSpeed = 10f;
            }
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
            //Main.NewText("Pain is: " + Player.torchLuck / 5, 0, 204, 170);

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
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (GetInstance<Configurations>().Respwnedbool)
            {
                Player.respawnTimer = (GetInstance<Configurations>().Respwned * 60); //Change respawn timer
            }
            base.Kill(damage, hitDirection, pvp, damageSource);
        }
        public override void OnRespawn()
        {
            if (GetInstance<Configurations>().TheHealth)
            {
                Player.statLife = Player.statLifeMax2;
            }
            base.OnRespawn();
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

            foreach (TooltipLine line in tooltips)
            {
                if (GetInstance<Configurations>().moreInfo)
                {
                    if (line.Mod == "Terraria" && line.Name == "Speed")
                    {
                        line.Text = line.Text + " (" + Math.Round((item.useAnimation / player.GetWeaponAttackSpeed(item))) + " usetime)"; //show use time affeced by speed
                    }
                    if (line.Mod == "Terraria" && line.Name == "Knockback")
                    {
                        line.Text = line.Text + " (" + Math.Round(item.knockBack, 2) + " knockback)"; //round to 1 decimal place, auto updates
                    }
                }
                if (GetInstance<Configurations>().superCrit)
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
                if (GetInstance<Configurations>().BigBoiDrill)
                {
                    if (item.pick >= 1 && item.channel)
                    {
                        if (line.Mod == "Terraria" && line.Name == "PickPower")
                        {
                            line.Text = line.Text + "\n[c/00ffeb:- Hold right click while mining to mine in a 3x3 area]";
                        }
                    }
                    if (item.hammer >= 1 && item.channel)
                    {
                        if (line.Mod == "Terraria" && line.Name == "HammerPower")
                        {
                            line.Text = line.Text + "\n[c/00ffeb:- Hold right click while breaking walls to break walls in a 3x3 area]";
                        }
                    }
                }
                if (GetInstance<Configurations>().LuckyU)
                {
                    if (item.type is ItemID.LuckyHorseshoe or ItemID.ObsidianHorseshoe or ItemID.BalloonHorseshoeHoney or ItemID.BalloonHorseshoeFart or ItemID.BalloonHorseshoeSharkron or ItemID.BlueHorseshoeBalloon or ItemID.WhiteHorseshoeBalloon
                        or ItemID.YellowHorseshoeBalloon or ItemID.HorseshoeBundle)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = line.Text + "\n[c/d2ba5a:Your current luck value is " + Math.Round(Main.LocalPlayer.luck, 2) + "!]" +
                            "\n[c/d2ba5a:Your current coin luck value is " + Math.Round(Main.LocalPlayer.coinLuck) + "!]" +
                            "\n[c/d2ba5a:Your current torch luck value is " + Math.Round(Main.LocalPlayer.torchLuck, 2) + "!]";
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
            if (GetInstance<Configurations>().noHappy4U)
            {
                NPCID.Sets.NoTownNPCHappiness[npc.type] = true;
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

        public override void ModifyShop(NPCShop shop)
        {
            base.ModifyShop(shop);
            if (GetInstance<Configurations>().noHappy4U)
            {
                if (shop.NpcType == NPCID.BestiaryGirl || shop.NpcType == NPCID.Golfer || shop.NpcType == NPCID.Merchant) //Forest
                {
                    shop.Add(ItemID.TeleportationPylonPurity);
                }
                if (shop.NpcType == NPCID.Mechanic || shop.NpcType == NPCID.Cyborg || shop.NpcType == NPCID.SantaClaus) //Snow
                {
                    shop.Add(ItemID.TeleportationPylonSnow);
                }
                if (shop.NpcType == NPCID.ArmsDealer || shop.NpcType == NPCID.Steampunker || shop.NpcType == NPCID.DyeTrader) //Desert
                {
                    shop.Add(ItemID.TeleportationPylonDesert);
                }
                if (shop.NpcType == NPCID.GoblinTinkerer || shop.NpcType == NPCID.Clothier || shop.NpcType == NPCID.Demolitionist) //Cavern
                {
                    shop.Add(ItemID.TeleportationPylonUnderground);
                }
                if (shop.NpcType == NPCID.Stylist || shop.NpcType == NPCID.Pirate) //Ocean
                {
                    shop.Add(ItemID.TeleportationPylonOcean);
                }
                if (shop.NpcType == NPCID.Dryad || (shop.NpcType == NPCID.Painter && shop.Name != "Decor") || shop.NpcType == NPCID.WitchDoctor) //Jungle
                {
                    shop.Add(ItemID.TeleportationPylonJungle);
                }
                if (shop.NpcType == NPCID.PartyGirl || shop.NpcType == NPCID.Wizard || shop.NpcType == NPCID.Princess) //Hallow
                {
                    shop.Add(ItemID.TeleportationPylonHallow, Condition.Hardmode);
                }
                if (shop.NpcType == NPCID.Truffle) //Mushroom
                {
                    shop.Add(ItemID.TeleportationPylonMushroom);
                }
            }
        }
        public override void GetChat(NPC npc, ref string chat)
        {
            /*if (GetInstance<Configurations>().LuckyU)
                if (npc.type == NPCID.Wizard)
                {
                    chat = chat + "\n\n[c/A45EE5:Your current luck value is " + Math.Round(Main.LocalPlayer.luck, 2) + "!]" +
                        "\n[c/A45EE5:Your current coin luck value is " + Math.Round(Main.LocalPlayer.coinLuck) + "!]" +
                        "\n[c/A45EE5:Your current torch luck value is " + Math.Round(Main.LocalPlayer.torchLuck / 5, 2) + "!]";
                }*/
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
        public static bool warningmessage;

        public override void PreUpdateWorld()
        {
            if (GetInstance<Configurations>().shieldHealthNormalbool)
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
            if (GetInstance<Configurations>().tistheseason == "None")
            {
                //clears after a day
            }
            else if (GetInstance<Configurations>().tistheseason == "Halloween")
            {
                Main.xMas = false;
                Main.halloween = true;
            }
            else if (GetInstance<Configurations>().tistheseason == "Christmas")
            {
                Main.xMas = true;
                Main.halloween = false;
            }
            else if (GetInstance<Configurations>().tistheseason == "Both")
            {
                Main.halloween = true;
                Main.xMas = true;
            }


            //if all tweaks are disabled
            if (GetInstance<Configurations>().shieldHealthNormalbool == false && GetInstance<Configurations>().shieldHealthNormal == 100 && GetInstance<Configurations>().DamageSpreadbool == false && GetInstance<Configurations>().DamageSpread == 15
                && GetInstance<Configurations>().superCrit == false && GetInstance<Configurations>().moreInfo == false && GetInstance<Configurations>().Respwnedbool == false && GetInstance<Configurations>().Respwned == 15
                && GetInstance<Configurations>().TheHealth == false && GetInstance<Configurations>().NoBoomBoom == false && GetInstance<Configurations>().NoTraps == false && GetInstance<Configurations>().FckGraves == false
                && GetInstance<Configurations>().NoChill == false && GetInstance<Configurations>().FastChop4U == false && GetInstance<Configurations>().FastDrill4U == false && GetInstance<Configurations>().BigBoiDrill == false
                    && GetInstance<Configurations>().HammarTime == false && GetInstance<Configurations>().SpeedPlace == "Default" && GetInstance<Configurations>().RIPBossRNG == false && GetInstance<Configurations>().NoSink == false
                    && GetInstance<Configurations>().tistheseason == "None" && GetInstance<Configurations>().noHappy4U == false && GetInstance<Configurations>().bestUnlocks == false && GetInstance<Configurations>().NoStronk == false
                    && GetInstance<Configurations>().LuckyU == false && GetInstance<Configurations>().NoInventoryClutter == false && GetInstance<Configurations>().NoStar4U == false && GetInstance<Configurations>().RIPdungeon == false)
            {

                if (!warningmessage)
                {
                    if (Main.netMode == 2) // Server
                    {
                        Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Storm's Tweaks Mod: It seems you have all of the tweaks disabled\nMake sure to check the config menu and enable the changes you want"), new Color(34, 221, 151));
                    }
                    else if (Main.netMode == 0) // Single Player
                    {

                        Main.NewText("Storm's Tweaks Mod: It seems you have all of the tweaks disabled\nMake sure to check the config menu and enable the changes you want", 34, 221, 151);
                    }
                    warningmessage = true;
                }
            }
        }
        public override void OnWorldUnload()
        {
            warningmessage = false;
        }
    }
}