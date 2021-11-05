using System;
using System.Reflection;
using System.Collections.Generic;
using BepInEx;
using EnigmaticThunder;
using EntityStates;
using EnigmaticThunder.Modules;
using EnigmaticThunder.Util;
using EntityStates.ZotStates;
using RoR2;
using RoR2.Skills;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using KinematicCharacterController;
using EntityStates.Merc;
using EntityStates.GolemMonster;
using EntityStates.BrotherMonster;
using EntityStates.ParentMonster;
using EntityStates.Loader;
using EntityStates.LemurianBruiserMonster;
using EntityStates.QuestVolatileBattery;
using EntityStates.TitanMonster;
using EntityStates.BeetleGuardMonster;
using On.RoR2;
using EntityStates.GrandParentBoss;
using EntityStates.NewtMonster;
using EntityStates.VagrantMonster;
using System.Collections;
using RoR2.Navigation;
using RoR2.UI;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;
using RoR2.Audio;
using Rewired.Data;
using Rewired;
using static LordZot.LordZot;
using JetBrains.Annotations;
using On.RoR2.UI;
using IL.RoR2;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace LordZot
{

    [BepInDependency("com.EnigmaDev.EnigmaticThunder")]

    [BepInPlugin(MODUID, "LordZot", "0.0.1")] // put your own name and version here


    public class LordZot : BaseUnityPlugin
    {
        public const string MODUID = "Jot.LordZot"; // put your own names here
        public static Material commandoMat;
        public static GameObject Gem;

        public static RoR2.CharacterMaster GemMaster { get; private set; }
        public static GameObject GemMaster2 { get; private set; }

        public static GameObject zotPrefab; // the survivor body prefab
        public GameObject characterDisplay; // the prefab used for character select
        public GameObject doppelganger; // umbra shit
        public static float Charged;
        public static GameObject arrowProjectile; // prefab for our survivor's primary attack projectile
        private static readonly Color characterColor = new Color(0.55f, 0.15f, 0.55f); // color used for the survivor
        public static float timeRemaining;
        public static float timeRemainingstun;
        public static float jumpcooldown;
        public static bool fallstun;
        public static Animator animator;
        private KeyCode jump;
        private bool jumpInputReceived;
        private RoR2.CharacterModel modela;
        public static RoR2.PlayerCharacterMasterController master;
        public static RoR2.CharacterBody model;
        public static bool inair = false;
        public static bool Busy;
        public static bool wellhefell;
        public static float fallspeed;
        public static float fastestfallspeed;
        public static bool holdtime = false;
        public static float ChargedLaser;
        public static bool ugh;
        public static bool flight = false;
        public static Shader distortion = Resources.Load<Shader>("shaders/fx/hgdistortion");
        public static Shader cloudremap = Resources.Load<Shader>("shaders/fx/cloudremap");
        public static Vector3 prevaim;
        public static Shader cloudintersectionremap = Resources.Load<Shader>("shaders/fx/cloudintersectionremap");
        internal static List<RoR2.BuffDef> buffDefs = new List<RoR2.BuffDef>();
        public static Shader opaquecloudremap = Resources.Load<Shader>("shaders/fx/opaquecloudremap");
        public static float Gemdrain = 0f;
        public static Shader hopoo = Resources.Load<Shader>("shaders/deferred/hgstandard");
        public static float flightcooldown = 0.1f;
        public static bool dontmove = false;
        public static KeyCode Floatbutton = UnityEngine.KeyCode.V;
        public static KeyCode Gempowercheat1 = UnityEngine.KeyCode.L;
        public static bool slammingair;
        public static KeyCode Gempowercheat2 = UnityEngine.KeyCode.LeftControl;
        public static float ticker = 0f;
        private static RoR2.NetworkUser zotuser;
        public GameObject RedOrb { get; private set; }
        public static GameObject RedTrail { get; private set; }

        public static Vector3 Leftfootpos;
        public static Vector3 Righthandpos;
        public static Vector3 Lefthandpos;
        public static Vector3 Rightfootpos;
        public static GameObject originalag = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFXLightning");
        public static GameObject originalug = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXQuick");
        public static float ticker2 = 0f;
        public static RoR2.CharacterMotor motor;

        public RoR2.CharacterModel actualmodel { get; private set; }

        private bool zotisingame = false;
        internal static List<GameObject> projectilePrefabs = new List<GameObject>();
        public static ChildLocator component { get; private set; }
        public float gauntletsfloat { get; private set; }

        public static UnityEngine.Transform modeltransform;
        private bool zotcheck = true;
        public static GameObject trail1;
        public static RoR2.Tracer trailtrace1;
        public static GameObject trail2;
        public static RoR2.Tracer trailtrace2;
        public static RoR2.EffectData lefttraileffect;
        public static GameObject original111 = Resources.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerMageLightningLaser");
        public static GameObject original222 = Resources.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerMageFireLaser");
        public static RoR2.EffectData righttraileffect;

        public static Transform rightshield;
        public static Transform leftshield;

        public static Transform leftfoot;
        public static Transform rightfoot;
        public static Transform righthand;
        public static Transform lefthand;
        public static GameObject rightpunch = Resources.Load<GameObject>("prefabs/effects/tracers/TracerGolem");
        public static int zotlevel;
        public static float zotlevels;
        public static RoR2.SkillLocator zotskills;
        public static GameObject leftpunch = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactLightning");
        public static float GemPowerVal = 20f;
        public static string GemPowerValString;
        public static float MaxGemPowerVal = 39f;
        public RoR2.UI.HGTextMeshProUGUI textMesh;
        public static float ChargedSlam { get; internal set; }
        public float GemStack { get; private set; }
        public static List<RoR2.CharacterBody> ZotWhacked;
        public static RoR2.CharacterBody ZotWhackmaster;
        public float ZotWhackDuration;
        public static Material PleaseWork;
        public float Inevitable = 0f;
        public GameObject GemBarUIReal;
        public GameObject GemBarUI;
        public GameObject GemBar;
        public GameObject GemBarImage;
        public GameObject GemBarOutline;
        private RoR2.UI.HUD hud = null;
        public static Material emiss;
        public GameObject GameObjectReference2 { get; private set; }
        public RectTransform rectTransformback { get; private set; }
        public float percent { get; private set; }
        public static bool LastSwingArm { get; internal set; }
        public RoR2.UI.HUD hud2 { get; private set; }
        public bool Zotchosensurvivor;
        public static Material Emission { get; private set; }

        public static float floatpower;
        public static Material Thematerial { get; private set; }
        public float GempowerValpercent { get; private set; }
        public float ticker22 { get; private set; }
        public object hudclone { get; private set; }
        internal static RoR2.BuffDef ZotWhack { get; private set; }
        internal static RoR2.BuffDef ZotJumping { get; private set; }
        internal static RoR2.BuffDef ZotFlight { get; private set; }
        public RoR2.Tracer RedTracer { get; private set; }
        public float timeinair { get; private set; }
        public float gemtick { get; private set; }
        public static RoR2.PlayerCharacterMasterController playerCharacterMaster;

        public Material mat;
        public bool Superjump;
        public float superjumptogglecooldown;
        public static bool Chargingjump = false;

        public static float Followfloatpower;

        private bool hudinit = false;

        public static bool Charging = false;
        public GameObject GameObjectReference;
        public GameObject GemBarText;
        public static string GemPowerDisplayValue;

        public Slider GempowerSlider { get; private set; }

        public RoR2.UI.HGTextMeshProUGUI GemValText;

        public RectTransform GempowerSlider2 { get; private set; }

        private List<RoR2.CharacterBody> instancesList;
        private List<RoR2.CharacterBody> list;
        private RoR2.CharacterBody characterBody2;
        public RectTransform rectTransformfill;
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        private Func<RoR2.ShakeEmitter> Stopshaking;
        private RoR2.EffectComponent EffectSettings3;
        private RoR2.EffectComponent EffectSettings4;
        private RoR2.EffectComponent EffectSettings5;
        public static GameObject RedTrailPrefab;
        public static Vector3 nextgem;
        private int gemtickbacklog;

        public Vector3 footposition { get; private set; }
        public static RoR2.SpawnCard GemCard;
        public GameObject Gemmasterprefab { get; private set; }
        public RoR2.CharacterMaster gemcomponent { get; private set; }
        public static GameObject Gembody;
        public RoR2.DirectorPlacementRule GemRule { get; private set; }
        public TeamIndex? teamIndexOverride { get; private set; }
        public float GemsActive { get; private set; }
        public float GemsCollected { get; private set; }
        public static TrailRenderer LeftTrail { get; private set; }
        public static TrailRenderer RightTrail { get; private set; }

        public static float leftlegik;
        public static float rightlegik;
        private RaycastHit raycastHitleft;
        private RaycastHit raycastHitright;
        private float leftikfollow = 0;
        private float rightikfollow = 0;

        //  private static float highestRecordedFallSpeed;

        private void Awake()
        {
           

            Assets.PopulateAssets(); // first we load the assets from our assetbundle 

            

            CreateDisplayPrefab(); // then we create our character's body prefab

            CreatePrefab(); // then we create our character's body prefab

            RegisterStates(); // register our skill entitystates for networking 
           // RegisterProjectiles();
            // RegisterBuffs();

            RegisterCharacter(); // and finally put our new survivor in the game
          
            //  Debug.Log("Character Reggied");

            CreateDoppelganger(); // not really mandatory, but it's simple and not having an umbra is just kinda lame

            
            // Debug.Log("Dopple Reggied");
        //    On.PhysicsImpactSpeedModifier.OnCollisionEnter += ZotWallSmash2;
            On.RoR2.Stage.Start += zotcheck2;
            On.RoR2.CharacterBody.OnSprintStart += ZotNoSprint;
            On.RoR2.FootstepHandler.Footstep_AnimationEvent += FootstepBlast;

            On.RoR2.HealthComponent.Suicide += DoYouKnowWhoIAm;

         //   On.RoR2.CharacterBody.OnTakeDamageServer += ZotWallSmashInflict;
            On.RoR2.CharacterMotor.OnMovementHit += Knockbackdamage2;
         //   On.RoR2.GenericPickupController.GrantItem += ZotNoItems;
           
            On.RoR2.CharacterMotor.OnHitGroundServer += Landing2;
            //   On.RoR2.Stage.BeginAdvanceStage += Timetozotcheckagain;
            On.RoR2.UI.HUD.Awake += GemHud;
            
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += ResetZot;
          
         //   On.RoR2.DirectorSpawnRequest.ctor += GemNode;


        }
      /*  private void CreateConfig(ConfigFile config)
        {
            LordZot.JumpHeightMult = config.ActiveBind("Zot Jump Height Multiplier", 1f, "Does not affect base jump height; This increases the rate that the final vertical velocity scales with charge time.");

        }*/


        private void Landing2(On.RoR2.CharacterMotor.orig_OnHitGroundServer orig, RoR2.CharacterMotor self, RoR2.CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            timeinair = 0f;
            var characterBody = self.GetComponentInParent<RoR2.CharacterBody>();
            var magnitude = hitGroundInfo.velocity.magnitude;
            string magdebug = magnitude.ToString();
         //   Debug.Log("magnitude");
          //  Debug.Log(magdebug);
            {
                orig(self, hitGroundInfo);
            }
            if (characterBody.baseNameToken is "ZOT_NAME")
            {
                LordZot.flight = false;




                var position = characterBody.footPosition;


                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                LandingBlast.attacker = characterBody.gameObject;
                LandingBlast.inflictor = characterBody.gameObject;
                LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                LandingBlast.baseDamage = 0.5f + ((40 * 0.1f) * magnitude);
                LandingBlast.baseForce = 1000f + (magnitude * 20f);
                LandingBlast.procCoefficient = 0f;
                LandingBlast.bonusForce = new Vector3(0f, 1000f + (magnitude * 50f), 0f);
                LandingBlast.position = position;
                LandingBlast.radius = 7f + (magnitude * 0.1f);
                LandingBlast.crit = characterBody.RollCrit();
                LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                LandingBlast.Fire();


                timeRemainingstun = 1.6f;

                characterBody.characterMotor.velocity = Vector3.zero;


                GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                GameObject original25 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original66 = EntityStates.TitanMonster.DeathState.initialEffect;
                GameObject original666 = Resources.Load<GameObject>("prefabs/effects/impacteffects/LemurianBruiserDeathImpact");
                //EffectSettings2 = original.GetComponent<RoR2.EffectComponent>();
                // EffectSettings2.applyScale = true;
                // EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                //EffectSettings.applyScale = true;
                //  EffectSettings3 = original666.GetComponent<RoR2.EffectComponent>();
                // EffectSettings3.applyScale = true;
                // EffectSettings4 = original66.GetComponent<RoR2.EffectComponent>();
                //EffectSettings4.applyScale = true;
                // EffectSettings5 = original25.GetComponent<RoR2.EffectComponent>();
                // EffectSettings5.applyScale = true;

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = position;
                effectData.scale = 2f + (0.25f * magnitude);

                RoR2.EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, effectData, true);
                RoR2.EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, effectData, true);
                RoR2.EffectManager.SpawnEffect(original, effectData, true);
                RoR2.EffectManager.SpawnEffect(original25, effectData, true);
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);

                RoR2.EffectData effectData2 = new RoR2.EffectData();

                effectData2.origin = position;
                effectData2.scale = 1f + (0.05f * magnitude);
                RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                if (magnitude > 25)
                {
                    RoR2.Util.PlaySound(ExitSkyLeap.soundString, characterBody.gameObject);
                    RoR2.EffectManager.SpawnEffect(original66, effectData2, true);
                    RoR2.EffectManager.SpawnEffect(original666, effectData, true);
                }
                RoR2.EffectManager.SpawnEffect(original25, effectData2, true);
              
                RoR2.ShakeEmitter shakeEmitter;
                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                shakeEmitter.wave = new Wave
                {
                    amplitude = 0.5f,
                    frequency = 180f,
                    cycleOffset = 0f
                };
                shakeEmitter.duration = 1.5f;
                shakeEmitter.radius = 50f;
                shakeEmitter.amplitudeTimeDecay = true;

         
                // RoR2.EffectManager.SpawnEffect(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, effectData, false);
                // RoR2.Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
                RoR2.Util.PlaySound(ExitSkyLeap.soundString, base.gameObject);// RoR2.Util.PlaySound(FireMegaNova.novaSoundString, characterBody.gameObject);

            };


        }

        private void ZotNoSprint(On.RoR2.CharacterBody.orig_OnSprintStart orig, RoR2.CharacterBody self)
        {
            if (self.baseNameToken is "ZOT_NAME")
            {
                self.isSprinting = false;
                if (timeRemaining <= 0.1f)
                {
                   
                    floatpower += 8f;
                    GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");

                    //   EffectSettings.parentToReferencedTransform = true;
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = lefthand.position;
                    effectData.scale = 1f;
                    RoR2.EffectManager.SpawnEffect(original, effectData, true);

                    RoR2.EffectData effectData2 = new RoR2.EffectData();
                    effectData.origin = righthand.position;
                    effectData.scale = 1f;
                    RoR2.EffectManager.SpawnEffect(original2, effectData, true);




                    ticker2 = 2f;


                    animator.SetFloat("ShieldsIn", 0.12f);


                }
                if (timeRemaining >= 0)
                {

                    timeRemaining = 6f;
                };

               
            }
            else
            {
                orig(self);
            }
            
        }

      

    /*    internal static void AddProjectile(GameObject projectileToAdd)
        {
            projectilePrefabs.Add(projectileToAdd);
        }*/
/*
        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            return Prefabs.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName, true, "", "", 146);
        }*/
      /*  private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.explosionSoundString = "";
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeExpiredSoundString = "";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;
            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }
        private void RegisterProjectiles()
        {


             static void RedTrail2()
            {
                RedTrailPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "RedTrail");
                ProjectileImpactExplosion component = RedTrailPrefab.GetComponent<ProjectileImpactExplosion>();
                InitializeImpactExplosion(component);
                component.blastRadius = 0f;
                component.destroyOnEnemy = false;
                component.lifetime = 2;
                component.impactEffect = null;
                component.timerAfterImpact = false;
                component.lifetimeAfterImpact = 0.1f;
                ProjectileDamage component2 = RedTrailPrefab.GetComponent<ProjectileDamage>();
                component2.damageType = DamageType.Silent;
                component2.damage = 0f;
                ProjectileController component3 = RedTrailPrefab.GetComponent<ProjectileController>();
              //  ProjectileGhostController component4 = RedTrailPrefab.GetComponent<ProjectileGhostController>();
                component3.ghostTransformAnchor = model.modelLocator.modelTransform.GetComponent<ChildLocator>().FindChild("RightHand");
                component3.ghostPrefab = Resources.Load<GameObject>("prefabs/projectileghosts/RedAffixMissileGhost");
                component3.startSound = "";
                Projectiles.RegisterProjectile(RedTrailPrefab);
                

                RedTrailPrefab.GetComponent<Rigidbody>().useGravity = false;
                RoR2.Tracer RedTracer = RedTrailPrefab.AddComponent<RoR2.Tracer>();
                
            }
        }*/
/*
        private void ZotWallSmashInflict(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, RoR2.CharacterBody self, RoR2.DamageReport damageReport)
        {
            orig(self, damageReport);
            damageReport.victimBody.AddTimedBuff(ZotWhack, 6f);

        }*/

        private void GemHud(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
    {
            
            orig.Invoke(self);
            hud = self;
            this.SetupHUD(self.transform);
        
            
        }

        public static Transform FindTransformRecursive([NotNull] Transform startingTransform, string targetName)
        {
            Transform transform = null;
            if (startingTransform != null)
            {
                transform = startingTransform.Find(targetName);
                if (transform == null && startingTransform.childCount > 0)
                {
                    for (int i = 0; i < startingTransform.childCount; i++)
                    {
                        Transform child = startingTransform.GetChild(i);
                        if (child != null)
                        {
                            transform = LordZot.FindTransformRecursive(child, targetName);
                            if (transform != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return transform;
        }

        private void SetupHUD([NotNull]Transform selftransform)
        {
          
            

                if (transform)
                {
                 //   Debug.Log("Generating UI");

                Transform god = hud.mainContainer.transform;
                    
      
                
                    GemBarText = new GameObject("GemBarText");
               // GemBarUIReal = new GameObject("GemPowerBar");
                //GemBarUIReal.transform.SetParent(god, false);
             //   GemBarUIReal.SetActive(true);
              //  Instantiate<GameObject>(GemBar, god, false);
         //       GempowerSlider = GemBarUIReal.GetComponentInChildren<Slider>();
              //  GempowerSlider.transform.SetParent(god, false);
                   // RectTransform textrectTransform = GemBarText.AddComponent<RectTransform>();
                    GemValText = GemBarText.AddComponent<RoR2.UI.HGTextMeshProUGUI>();
              //  GempowerSlider2 = GameObjectReference2.GetComponent<RectTransform>();
             //   GempowerSlider2.anchorMin = new Vector2(0.35f, 0.09f);
             //   GempowerSlider2.anchorMax = new Vector2(0.60f, 0.09f);
             //   GempowerSlider2.anchoredPosition = new Vector2(30f, 0f);

                GemBarText.transform.SetParent(god, false);
                    GemValText.color = new Color(255f, 255f, 255f, 255f);
                    GemValText.fontSize = 44f;
                    GemValText.outlineColor = new Color(0f, 0f, 0f);
                    GemValText.outlineWidth = 0.5f;
                    GemValText.text = "";
                    GemValText.faceColor = Color.white;
                  //  textrectTransform.anchoredPosition = new Vector2(25f, 0f);
                 //   textrectTransform.anchorMin = new Vector2(0.33f, 0.1f);
                 //   textrectTransform.anchorMax = new Vector2(0.33f, 0.1f);
                 //   textrectTransform.sizeDelta = new Vector2(40f, 40f);
                    GemValText.enableWordWrapping = false;
/*               GameObjectReference2 = new GameObject("GemBarback");
                GameObjectReference2.transform.SetParent(GemBarText.transform);
                rectTransformback = GameObjectReference2.AddComponent<RectTransform>();
                rectTransformback.anchorMin = new Vector2(0.35f, 0.09f);
                rectTransformback.anchorMax = new Vector2(0.60f, 0.09f);
                rectTransformback.sizeDelta = new Vector2(200f, 55f);
                rectTransformback.anchoredPosition = new Vector2(30f, 0f);
                GameObjectReference2.AddComponent<Image>();
                GameObjectReference2.GetComponent<Image>().sprite = Assets.gempower;

                GameObjectReference2.GetComponent<Image>().color = new Color(0, 0, 0);

                GameObjectReference = new GameObject("GemBar");
                GameObjectReference.transform.SetParent(GemBarText.transform);
*/

                /*rectTransformfill = GameObjectReference.AddComponent<RectTransform>();
                rectTransformfill.anchorMin = new Vector2(0.35f, 0.09f);
                rectTransformfill.anchorMax = new Vector2(0.6f, 0.09f);
                rectTransformfill.sizeDelta = new Vector2(200f, 55f);
                rectTransformfill.anchoredPosition = new Vector2(30f, 0f);
                GameObjectReference.AddComponent<Image>();
                GameObjectReference.GetComponent<Image>().sprite = Assets.gempower;*/
                hudinit = true;
                  //  Debug.Log("Gem Power UI Initialized");
                }
            
        }

        private void Knockbackdamage2(On.RoR2.CharacterMotor.orig_OnMovementHit orig, RoR2.CharacterMotor self, Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            orig(self, hitCollider, hitNormal, hitPoint, ref hitStabilityReport);

            if (zotisingame)
            {

               
                if (((Math.Abs(self.velocity.magnitude) > 45f) | (Math.Abs(self.velocity.magnitude) > 25f && self.mass > 100)) && self.GetComponent<RoR2.CharacterBody>().teamComponent.teamIndex is TeamIndex.Monster && !self.isGrounded)
                {
                    RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                    LandingBlast.attacker = model.gameObject;
                    LandingBlast.inflictor = model.gameObject;
                    LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(model.gameObject);
                    LandingBlast.baseDamage = (self.mass * (self.velocity.magnitude * 0.3f) * 0.025f) + ((self.mass * (self.velocity.magnitude * 0.3f) * 0.025f) * (40 * 0.005f)) * (1 + self.GetComponentInParent<RoR2.CharacterBody>().level * 0.2f);
                    LandingBlast.baseForce = 15 * self.velocity.magnitude + self.mass;
                    LandingBlast.procCoefficient = 0f;

                    LandingBlast.position = hitPoint;
                    LandingBlast.radius = 1 + self.velocity.magnitude * 0.2f + (self.mass * 0.001f);
                    LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                    LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                    LandingBlast.Fire();

                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                    GameObject original25 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                    GameObject original66 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                    GameObject original2737 = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniImpactVFXLarge");
                    GameObject original27337 = EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab;
                    RoR2.EffectData blast = new RoR2.EffectData();
                    blast.scale = self.velocity.magnitude * 0.1f + (self.mass * 0.00005f);
                    blast.origin = hitPoint;
                    blast.rotation = Quaternion.FromToRotation(hitPoint, hitNormal);
                    RoR2.EffectData blast2 = new RoR2.EffectData();
                    blast2.scale = self.velocity.magnitude * 0.1f + (self.mass * 0.00005f); ;
                    blast2.origin = hitPoint;

                    RoR2.EffectManager.SpawnEffect(original66, blast2, true);
                    RoR2.EffectManager.SpawnEffect(original2, blast, true);
                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                    RoR2.EffectManager.SpawnEffect(original25, blast, true);
                    if (Math.Abs(self.velocity.magnitude) > 180f | self.mass > 600)
                    {
                        RoR2.Util.PlaySound(EntityStates.BrotherMonster.ExitSkyLeap.soundString, self.gameObject);
                        RoR2.EffectManager.SpawnEffect(original27337, blast, true);
                        RoR2.EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, blast, true);
                    }
                    self.velocity = self.velocity * 0.05f;
                }

            }
        }

     

        private void ResetZot(On.RoR2.UI.MainMenu.BaseMainMenuScreen.orig_OnEnter orig, RoR2.UI.MainMenu.BaseMainMenuScreen self, RoR2.UI.MainMenu.MainMenuController mainMenuController)
        {

            GemsActive = 0f;
            GemStack = 0f;
            hudinit = false;
            Superjump = false;
            zotcheck = false;
            On.RoR2.ExperienceManager.AwardExperience -= ZotNoEXP;
            orig(self, mainMenuController);
            On.RoR2.CharacterBody.OnKilledOtherServer -= GemCollection;
            GemsCollected = 0f;
        }

        private void GemCollection(On.RoR2.CharacterBody.orig_OnKilledOtherServer orig, RoR2.CharacterBody self, RoR2.DamageReport damageReport)
        {
            if (damageReport.victimBody.baseNameToken is "TIMECRYSTAL_BODY_NAME" && damageReport.attackerBody.baseNameToken is "ZOT_NAME")
            { GemStack += 4f;
                model.baseMaxHealth = 400 + ((GemStack * 100 * GemStack * 0.0025f)) + (GemStack * 20);
                damageReport.attackerBody.healthComponent.Heal(damageReport.attackerBody.healthComponent.combinedHealth * 0.04f, default(RoR2.ProcChainMask), true);
              floatpower = 1000f;
                MaxGemPowerVal = 90f + (GemStack * 1f) + (GemStack * GemStack * 0.04f);
                GemPowerVal += MaxGemPowerVal * 0.06f;
                GemsActive -= 1f;
                GemsCollected += 0.5f;
            };
            if (damageReport.victimBody.baseNameToken is "TIMECRYSTAL_BODY_NAME" && damageReport.attackerBody.baseNameToken != "ZOT_NAME")
            { damageReport.victimBody.master.Respawn(damageReport.victimBody.corePosition, Quaternion.identity); };
       //     orig(self, damageReport);
       
        }


        /*        private void Timetozotcheckagain(On.RoR2.Stage.orig_BeginAdvanceStage orig, RoR2.Stage self, RoR2.SceneDef destinationStage)
                {
                    zotcheck = false;
                    hudinit = false;
                    orig(self, destinationStage);
                }*/



        private void DoYouKnowWhoIAm(On.RoR2.HealthComponent.orig_Suicide orig, RoR2.HealthComponent self, GameObject killerOverride, GameObject inflictorOverride, DamageType damageType)
        {
            if (self.body.baseNameToken is "ZOT_NAME")
            {
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                GameObject original27 = Resources.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                GameObject original277 = Resources.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");

                GameObject original2738 = Resources.Load<GameObject>("prefabs/effects/VagrantNovaExplosion");
                GameObject original2737 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                GameObject original2777 = Resources.Load<GameObject>("prefabs/effects/GrandparentDeathEffectLightShafts");
                RoR2.EffectData blast = new RoR2.EffectData();
                blast.scale = 10f;
                blast.origin = self.body.corePosition;
                RoR2.EffectManager.SpawnEffect(original27, blast, true);
                RoR2.EffectManager.SpawnEffect(original2738, blast, true);
                RoR2.EffectManager.SpawnEffect(original2, blast, true);

                RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, blast, true);
          
                    RoR2.EffectManager.SpawnEffect(original277, blast, true);
                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                    RoR2.EffectManager.SpawnEffect(original2777, blast, true);





                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                LandingBlast.attacker = self.body.gameObject;
                LandingBlast.inflictor = self.body.gameObject;
                LandingBlast.teamIndex = TeamIndex.Monster;
                LandingBlast.baseDamage = 1000;
                LandingBlast.baseForce = 300f;
                LandingBlast.position = self.body.corePosition;
                LandingBlast.radius = 50f;

                LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.None;
                LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                LandingBlast.Fire();
                RoR2.BlastAttack LandingBlast2 = new RoR2.BlastAttack();
                LandingBlast2.attacker = self.body.gameObject;
                LandingBlast2.inflictor = self.body.gameObject;
                LandingBlast2.teamIndex = TeamIndex.Player;
                LandingBlast2.baseDamage = 1000;
                LandingBlast2.baseForce = 111500f;
                LandingBlast2.position = self.body.corePosition;
                LandingBlast2.radius = 50f;

                LandingBlast2.falloffModel = RoR2.BlastAttack.FalloffModel.None;
                LandingBlast2.attackerFiltering = AttackerFiltering.NeverHit;
                LandingBlast2.Fire();


            }
            else
            orig(self, killerOverride, inflictorOverride, damageType);
        }

        private void zotcheck2(On.RoR2.Stage.orig_Start orig, RoR2.Stage self)
        {
            zotcheck = false;
            if (GemPowerVal < MaxGemPowerVal / 3)
            { GemPowerVal = MaxGemPowerVal / 3; };

            orig(self);
        }

        void FixedUpdate()
        {

            if (!zotcheck)
            {
               // Debug.Log("Zot checking.");
                var instances = RoR2.LocalUserManager.GetFirstLocalUser();
                master = instances.cachedMasterController;
                if (master)
                {
                    
                    if (master.master.GetBody() != null)
                    {
                        if (master.master.GetBody().baseNameToken is "ZOT_NAME")
                        {
                            Debug.Log("We got a Zot");
                            model = master.master.GetBody();
                            modeltransform = model.modelLocator.modelTransform;
                            motor = model.characterMotor;
                            actualmodel = modeltransform.GetComponent<RoR2.CharacterModel>();


                            zotisingame = true;
                           // model.inventory.enabled = true;

                            zotcheck = true;
                            On.RoR2.CharacterBody.OnKilledOtherServer += GemCollection;
                            On.RoR2.ExperienceManager.AwardExperience += ZotNoEXP;
                            


                            component = model.modelLocator.modelTransform.GetComponent<ChildLocator>();
                            model.baseMaxHealth = 400 + ((GemStack * 100 * GemStack * 0.0025f)) + (GemStack * 20);
                            if (component)
                            {
                                rightshield = component.FindChild("RightShield");
                                leftshield = component.FindChild("LeftShield");
                                leftfoot = component.FindChild("LeftFoot");
                                rightfoot = component.FindChild("RightFoot");
                                righthand = component.FindChild("RightHand");
                                lefthand = component.FindChild("LeftHand");

                                if (leftshield)
                                { LeftTrail = leftshield.GetComponent<TrailRenderer>();
                                    LeftTrail.emitting = false;
                                
                                }
                                if (rightshield)
                                { RightTrail = rightshield.GetComponent<TrailRenderer>();
                                    RightTrail.emitting = false;
                                }
                            }
                       
                        }

                        else
                        {
                            Debug.Log("There is no Zot. How could this happen?");
                            zotisingame = false;
                            zotcheck = true;
                        };

                        
                       Debug.Log("Done checking for Zots");

                    }
                }
            }

            if (zotisingame)
            {
              
                if (GempowerSlider)
                { GempowerSlider.maxValue = MaxGemPowerVal;
                    GempowerSlider.value = GemPowerVal;
                }

                if (superjumptogglecooldown > 0f)
                { superjumptogglecooldown -= Time.fixedDeltaTime; };

                
                if (model.baseNameToken is "ZOT_NAME" )
                {
                   
                    if (Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.LeftControl) && superjumptogglecooldown <= 0)
                    {
                        GemStack += 10;
                        GemPowerVal = MaxGemPowerVal;
                        floatpower = 1000f;
                        superjumptogglecooldown = 0.2f;

                    }
                    if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.LeftControl) && superjumptogglecooldown <= 0)
                    {
                      //  On.RoR2.GenericPickupController.GrantItem -= ZotNoItems;
                        On.RoR2.ExperienceManager.AwardExperience -= ZotNoEXP;

                    }

                 


                    if (Input.GetKey(KeyCode.L) && superjumptogglecooldown <= 0 && !Superjump && Input.GetKey(KeyCode.LeftControl))
                    {
                        Superjump = true;
                        superjumptogglecooldown = 1f;
                        GameObject original2777 = Resources.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                        RoR2.EffectData blast = new RoR2.EffectData();
                        blast.scale = 10f;
                        blast.origin = model.corePosition;
                        RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                    };

                    if (Input.GetKey(KeyCode.L) && superjumptogglecooldown <= 0 && Superjump && Input.GetKey(KeyCode.LeftControl))
                    {
                        Superjump = false;
                        superjumptogglecooldown = 1f;
                        GameObject original2777 = Resources.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                        RoR2.EffectData blast = new RoR2.EffectData();
                        blast.scale = 10f;
                        blast.origin = model.corePosition;
                        RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                    };


                    if (model.inputBank.sprint.down && superjumptogglecooldown <= 0)
                    {
                        model.isSprinting = false;
                        if (timeRemaining <= 0.1f)
                        {
                            floatpower += 8f;
                            GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                        
                            //   EffectSettings.parentToReferencedTransform = true;
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = lefthand.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original, effectData, true);

                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                            effectData.origin = righthand.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original2, effectData, true);




                            ticker2 = 2f;


                            animator.SetFloat("ShieldsIn", 0.12f);


                        }
                        if (timeRemaining >= 0)
                        {

                            timeRemaining = 6f;
                        };




                    };

                    if (GemPowerVal > MaxGemPowerVal)
                    { GemPowerVal = MaxGemPowerVal; };
                    if (GemPowerVal < 0)
                    { GemPowerVal = 0; };
                    MaxGemPowerVal = 90f + (GemStack * 1f) + (GemStack * GemStack * 0.04f);
                    if (GemPowerVal >= 1)
                    {
                        model.skillLocator.ResetSkills();
                        Gemdrain = 0f;
                    };

                    if (GemPowerVal < MaxGemPowerVal)
                    { GemPowerVal += MaxGemPowerVal * 0.000045f; };
                    if (Superjump)
                    {if (Chargingjump)
                        {
                            Charged += GemPowerVal * 0.0027f;
                            GemPowerVal -= Time.fixedDeltaTime * 2;
                            GemPowerVal -= GemPowerVal * 0.0027f;
                            Charged += Time.fixedDeltaTime * 2;
                        }
                    };
                    if (Chargingjump)
                    {
                      
                        GemPowerVal -= Time.fixedDeltaTime * 2.8f;
                        Charged += Time.fixedDeltaTime * 2.8f;
                    }
                    if (Charging)
                    {
                        GemPowerVal -= GemPowerVal * 0.0027f;
                        ChargedSlam += GemPowerVal * 0.0027f;
                        ChargedLaser += GemPowerVal * 0.00276f;


                    };


                };



              



                if (model.isSprinting)
                { 
                    model.isSprinting = false;
                };


                if (flight)
                {

                    GemPowerVal -= Time.fixedDeltaTime * 1.5f;
                    RoR2.ShakeEmitter shakeEmitter;
                    shakeEmitter = model.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    shakeEmitter.wave = new Wave
                    {
                        amplitude = 0.05f,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    shakeEmitter.duration = 0.1f;
                    shakeEmitter.radius = 50f;
                    shakeEmitter.amplitudeTimeDecay = false;



                    model.characterMotor.moveDirection += model.inputBank.moveVector;
                    if (motor.velocity.magnitude <= 50)
                    {
                        model.characterMotor.velocity += model.inputBank.moveVector * 1.5f;
                    }

                };

               
                
                if (flight && model.inputBank.jump.down )
                {




                    model.characterMotor.velocity += new Vector3 (0, 0.5f, 0);



                };
                if (flight && model.inputBank.sprint.down)
                {




                    model.characterMotor.velocity += new Vector3(0, -0.5f, 0);



                };





                if (Gemdrain > 0f)
                {
                    Gemdrain -= Time.fixedDeltaTime;

                };


                



                if (ticker22 <= 0f)
                {
                    if (leftikfollow < leftlegik && (leftlegik - leftikfollow) > 0.05)
                    {
                        leftikfollow += leftlegik * 0.01f;
                    };

                    if (leftikfollow > leftlegik && leftikfollow > 0.1f && (leftikfollow - leftlegik) > 0.02)
                    {
                        
                        leftikfollow -= leftikfollow * 0.01f; ;
                    };

                    if (rightikfollow < rightlegik && (rightlegik - rightikfollow) > 0.05)
                    {
                        rightikfollow += rightlegik * 0.01f;
                    };

                    if (rightikfollow > rightlegik && rightikfollow > 0.1f && (rightikfollow - rightlegik) > 0.02)
                    {
                        rightikfollow -= rightikfollow * 0.01f;
                    };
                    if (flight)
                    {
                        ChildLocator component = model.modelLocator.modelTransform.GetComponent<ChildLocator>();

                        if (component)
                        {


                            Transform transform = component.FindChild("LeftFoot");
                            Transform transform2 = component.FindChild("RightFoot");
                            if (transform)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = leftfoot.position;

                                effectData.scale = 5f;


                                //    RoR2.EffectManager.SpawnEffect(originalag, effectData, false);
                                RoR2.EffectManager.SpawnEffect(leftpunch, effectData, true);
                            };
                            if (transform2)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = rightfoot.position;
                                effectData.scale = 5f;


                                //   RoR2.EffectManager.SpawnEffect(originalag, effectData, false);
                                RoR2.EffectManager.SpawnEffect(leftpunch, effectData, true);
                            };
                        };

                        ticker22 = 0.01f;

                   
                    }


                };
                gemtick -= Time.fixedDeltaTime;
                ticker22 -= Time.fixedDeltaTime;

                if (Inevitable >= 10f)
                {
          
                    
                    Inevitable = 0f;
                    GemsActive += 1f;
                    GemCard = Resources.Load<RoR2.SpawnCard>("SpawnCards/CharacterSpawnCards/cscLunarGolem");
                    GemCard = UnityEngine.Object.Instantiate<RoR2.SpawnCard>(GemCard);
                    Gemmasterprefab = GemCard.prefab;
                    Gemmasterprefab = Prefabs.InstantiateClone(Gemmasterprefab, Gemmasterprefab.name, true);
   
                    RoR2.CharacterMaster component = Gemmasterprefab.GetComponent<RoR2.CharacterMaster>();
                    component.GetComponent<RoR2.CharacterAI.BaseAI>().enabled = false;
                    Gembody = Resources.Load<GameObject>("prefabs/characterbodies/TimeCrystalBody");
                    Gembody = Prefabs.InstantiateClone(Gembody, Gembody.name, true);
                    component.bodyPrefab = Gembody;
                    
                    GemCard.prefab = Gemmasterprefab;



                        
                        EnigmaticThunder.Modules.Masters.RegisterMaster(Gemmasterprefab);
                    Debug.Log("masterfab");
                    EnigmaticThunder.Modules.Bodies.RegisterBody(Gembody);
                        GemCard.directorCreditCost = 0;

                    Debug.Log("1");
                    NodeGraph groundNodes = RoR2.SceneInfo.instance.groundNodes;
                    Debug.Log("2");
                    NodeGraph.NodeIndex nodeIndex = groundNodes.FindClosestNode(model.corePosition + new Vector3(UnityEngine.Random.Range(15, 1000), UnityEngine.Random.Range(15, 10000), UnityEngine.Random.Range(15, 10000)), model.hullClassification, float.PositiveInfinity);
                    groundNodes.GetNodePosition(nodeIndex, out nextgem);
                    Debug.Log("3");
                    //RoR2.EffectManager.SimpleEffect(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, nextgem, Quaternion.identity, true);
                    Debug.Log("4");
                    string nextgemoutput = nextgem.ToString();
                    //  var rand = new System.Random();
                 //   var node = nodeIndex[rand.Next(1)];
                    Debug.Log(nextgemoutput);
                    gemtick = UnityEngine.Random.Range((15), (25));
                  
                   
           
            
                    

                    if (NetworkServer.active)
                    {

                       
                        RoR2.DirectorPlacementRule placementRule = new RoR2.DirectorPlacementRule
                        {
                            placementMode = RoR2.DirectorPlacementRule.PlacementMode.Random,
                            minDistance = 15f,
                            maxDistance = 5555f,
                            position = model.transform.position
                        };
                        RoR2.DirectorSpawnRequest GemSpawn = new RoR2.DirectorSpawnRequest(GemCard, placementRule, RoR2.RoR2Application.rng);
                        GemSpawn.teamIndexOverride = new RoR2.TeamIndex?(RoR2.TeamIndex.Neutral);

                        RoR2.DirectorCore.instance.TrySpawnObject(GemSpawn);
                        

                    }
         
                    

                };




                if (ticker <= 0)
                {
                    model.baseMaxHealth = 400 + ((GemStack * 100 * GemStack * 0.0025f)) + (GemStack * 20);
                    ticker = 1f;
                    /*
                                        RedOrb = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/FlurryArrowOrbEffect");
                                        RedOrb.GetComponent<RoR2.Orbs.OrbEffect>().parentObjectTransform = component.FindChild("RightHand");

                                        RoR2.EffectManager.SpawnEffect(RedOrb, righttraileffect, true);
                    */


                    //ChildLocator componentyy = model.modelLocator.modelTransform.GetComponent<ChildLocator>();

                   // ProjectileManager.instance.FireProjectile(RedTrailPrefab, componentyy.FindChild("RightHand").position + Vector3.up * 2, Quaternion.identity, model.gameObject, 0, 0, false, DamageColorIndex.Default, component.FindChild("RightHand").gameObject, 1f);




                };
                ticker -= Time.fixedDeltaTime;
                model.healthComponent.Heal(model.healthComponent.missingCombinedHealth * 0.00003f, default(RoR2.ProcChainMask), false);
              
                
                

             
              

                if (model.characterMotor.isGrounded)
                {
                    timeinair = 0f;
                    flight = false;
                    model.RemoveBuff(ZotFlight);
                    RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                    flightparameters.channeledFlightGranterCount = -2;
                    model.characterMotor.flightParameters = flightparameters;
                    RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                    gravparams.channeledAntiGravityGranterCount = -2;
                    model.characterMotor.gravityParameters = gravparams;



                    /*                      string layer10 = leftikfollow.ToString();
                                         string layer11 = rightikfollow.ToString();
                                        string layer14 = animator.GetFloat("rightdist").ToString();
                                        string layer15 = animator.GetFloat("rightdist").ToString();
                                        string layer12 = leftlegik.ToString();
                                        string layer13 = rightlegik.ToString();
                                        Debug.Log(layer11);
                                        Debug.Log(layer10);
                                        Debug.Log(layer13);
                                        Debug.Log(layer12);
                                        Debug.Log(layer14);
                                        Debug.Log(layer15);*/

                }

                footposition = motor.transform.position;

                leftlegik = (Physics.Raycast(new Ray(leftfoot.position + (model.characterDirection.forward * 3), model.transform.up * -1), out raycastHitleft, 1f, RoR2.LayerIndex.world.mask, QueryTriggerInteraction.Ignore) ? Mathf.Clamp01(1f - (raycastHitleft.distance) / 3f) : 0f);

                rightlegik = (Physics.Raycast(new Ray(rightfoot.position + (model.characterDirection.forward * 3),  model.transform.up * -1), out raycastHitright, 1f, RoR2.LayerIndex.world.mask, QueryTriggerInteraction.Ignore) ? Mathf.Clamp01(1f - (raycastHitright.distance) / 3f) : 0f);
                animator = model.modelLocator.modelTransform.GetComponent<Animator>();



                animator.SetFloat("rightdist", rightikfollow);
                animator.SetFloat("leftdist", leftikfollow);
       /*         if (model.inputBank.moveVector.magnitude > 0 && (leftikfollow > 0.28 | rightikfollow > 0.28))
                {
                    motor.velocity.x += model.inputBank.moveVector.x * 0.1f;
                    motor.velocity.z += model.inputBank.moveVector.z * 0.1f;
                };*/
                if (jumpcooldown > 0)
                { jumpcooldown -= Time.fixedDeltaTime; };

                if (flightcooldown > 0)
                { flightcooldown -= Time.fixedDeltaTime; };
                if (model.inputBank.jump.down && jumpcooldown <= 0 && model.baseNameToken is "ZOT_NAME" && model.characterMotor.isGrounded && !Busy)
                { 
                    {

                        ZotJump zotJump = new ZotJump();
                        RoR2.EntityStateMachine entityStateMachine = model.GetComponent<RoR2.EntityStateMachine>();
                        if (entityStateMachine.networkIdentity.localPlayerAuthority)
                        {
                            entityStateMachine.SetNextState(zotJump);
                        }
                    };
                };

                if (Input.GetKey(LordZot.Floatbutton) && model.baseNameToken is "ZOT_NAME" && !model.characterMotor.isGrounded && !flight && flightcooldown <= 0f)
                {
                    
                    model.AddBuff(ZotFlight);
                    RoR2.ShakeEmitter shakeEmitter;
                    shakeEmitter = model.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    shakeEmitter.wave = new Wave
                    {
                        amplitude = 0.3f,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    shakeEmitter.duration = 0.5f;
                    shakeEmitter.radius = 50f;
                    shakeEmitter.amplitudeTimeDecay = true;


                    RoR2.TemporaryOverlay temporaryOverlay = modeltransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                    temporaryOverlay.duration = 0.6f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = Resources.Load<Material>("materials/matVagrantEnergized");
                    temporaryOverlay.AddToCharacerModel(modeltransform.GetComponent<RoR2.CharacterModel>());
                    GameObject original = Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect");
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = model.corePosition;
                    effectData.scale = 15f;
                    RoR2.EffectManager.SpawnEffect(original, effectData, true);
                    

                    RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                    flightparameters.channeledFlightGranterCount = 2;
                    model.characterMotor.flightParameters = flightparameters;
                    RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                    gravparams.channeledAntiGravityGranterCount = 2;
                    model.characterMotor.gravityParameters = gravparams;

                    model.characterMotor.velocity = Vector3.zero;
                    LordZot.flight = true;
                    flightcooldown = 0.5f;

                };
                if ((Input.GetKey(LordZot.Floatbutton) | GemPowerVal <= 0.1f ) && model.baseNameToken is "ZOT_NAME" && !model.characterMotor.isGrounded && flight && flightcooldown <= 0f)
                {

                    model.RemoveBuff(ZotFlight);
                    RoR2.TemporaryOverlay temporaryOverlay = modeltransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                    temporaryOverlay.duration = 0.5f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = Resources.Load<Material>("materials/matVagrantEnergized");
                    temporaryOverlay.AddToCharacerModel(modeltransform.GetComponent<RoR2.CharacterModel>());
                    GameObject original = Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect");
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = model.corePosition;
                    effectData.scale = 8f;
                    RoR2.EffectManager.SpawnEffect(original, effectData, true);
 

                    RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                    flightparameters.channeledFlightGranterCount = -2;
                    model.characterMotor.flightParameters = flightparameters;
                    RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                    gravparams.channeledAntiGravityGranterCount = -2;
                    model.characterMotor.gravityParameters = gravparams;

                    flightcooldown = 0.5f;
                    LordZot.flight = false;
                };

                if (Busy && motor.velocity.y < 0)
                { motor.velocity.y += 0.09f; };
                if (motor.velocity.y > 500)
                { motor.velocity.y -= motor.velocity.y * 0.04f; };
                
                if (Charged > 0f && !motor.isGrounded )
                {   Charged -= Charged * 0.003f;
                    Debug.Log(Charged.ToString());
                 





                    string debug2 = fallspeed.ToString();
                    //    Debug.Log("speed");
                    //    Debug.Log(debug2);
                    bool isfaster = fallspeed >= fastestfallspeed;
                    if (isfaster)
                    {
                        fastestfallspeed = motor.velocity.y;
                        string debug = fastestfallspeed.ToString();
                        // Debug.Log("fastest");
                        //Debug.Log(debug);
                    }

                    /*if (Busy)
                    { motor.velocity.y = motor.velocity.y * 0.99f; };*/



                };
                if (!Busy && !motor.isGrounded && !flight)
                {
                    fallspeed = motor.velocity.y;
                    motor.velocity.y -= 0.0055f + timeinair;
                    timeinair += 0.0335f;
                }

         
                if (!motor.isGrounded && !flight && Charged > 1)
                {
                   

                };



                if (timeRemainingstun > 0 && motor.isGrounded)
                {
                //    model.characterMotor.Motor.enabled = true;
                    timeinair = 0f;
                    fastestfallspeed = 0f;
                    timeRemainingstun -= Time.fixedDeltaTime;
                    motor.velocity = Vector3.zero;
                    motor.moveDirection = Vector3.zero;


                }
                else

                {

                    string debug = fastestfallspeed.ToString();

                };

                if (Followfloatpower < floatpower)
                { 
                    Followfloatpower += floatpower * 0.01f;
                    Followfloatpower += Time.fixedDeltaTime * 2;
                }
                else
                { Followfloatpower -= Followfloatpower * 0.01f;
                    Followfloatpower -= Time.fixedDeltaTime * 1;
                };
                if (floatpower < 0)
                { floatpower = 0; };
                if (floatpower > 0f)
                {
                    floatpower -= Time.fixedDeltaTime * 3;


                };



                if (floatpower > 10f)
                    { floatpower -= floatpower * 0.015f;
                        Followfloatpower -= Followfloatpower * 0.015f;
                    };
                    if (mat != null)
                    {
                        mat.SetFloat("_EmPower", Followfloatpower);
                    }
                    if (model.modelLocator.modelTransform.GetComponent<RoR2.CharacterModel>())
                    {
                        foreach (RoR2.CharacterModel.RendererInfo renderInfo in model.modelLocator.modelTransform.GetComponent<RoR2.CharacterModel>().baseRendererInfos)
                        {

                            {
                                 mat = renderInfo.defaultMaterial;
                                
                            }
                        }
                    }
                
                if ((floatpower < ChargedSlam | floatpower < ChargedLaser) && Busy)
                { floatpower += Time.fixedDeltaTime * 55;
                  Followfloatpower += Time.fixedDeltaTime * 55;
                };
                if (Followfloatpower < 0f)
                { Followfloatpower = 0f; };
          

            }
        }
        private void ZotNoEXP(On.RoR2.ExperienceManager.orig_AwardExperience orig, RoR2.ExperienceManager self, Vector3 origin, RoR2.CharacterBody body, ulong amount)
        {

            if (body.baseNameToken is "ZOT_NAME")
            {
                amount = 0;
                
            }
            else
            {
                orig(self, origin, body, amount);
            }
        }

       /* private void ZotNoItems(On.RoR2.GenericPickupController.orig_GrantItem orig, RoR2.GenericPickupController self, RoR2.CharacterBody body, RoR2.Inventory inventory)
        {
            if (body = model)
            { inventory.enabled = false; };
            
               orig(self, body, inventory);
            
        }
*/
        void Update()
        {
            if (zotisingame)
            {
            /*    if (Inevitable >= 10)
                {
                    Inevitable = 0f;




                }*/
                //  GempowerValpercent = Math.Clamp((MaxGemPowerVal / GemPowerVal), 0.35f, 0.6f);
                //   rectTransformfill.anchorMax = new Vector2(GempowerValpercent, 0.09f);
                if (!RoR2.PauseManager.isPaused)
                {
                    Inevitable += Time.fixedDeltaTime * 0.1f;
                };
                GemPowerDisplayValue = (GemPowerVal.ToString("0.0") + "/" + MaxGemPowerVal.ToString("0") + "|Gems Collected:" + GemsCollected.ToString("0"));
            GemValText.text = GemPowerDisplayValue;

                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                   
                }
                else
                {
                    var player = RoR2.LocalUserManager.GetFirstLocalUser();
                    
                    var model = player.cachedBody.modelLocator.modelTransform;
                    var animator = model.GetComponent<Animator>();

                    animator.SetFloat("ShieldsIn", 0f);

                    ChildLocator component = model.GetComponent<ChildLocator>();


                    if (component)
                    {

                        GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                     //   EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                     //   EffectSettings.applyScale = true;
                     //   EffectSettings.soundName.IsNullOrWhiteSpace();
                     //   EffectSettings2 = original.GetComponent<RoR2.EffectComponent>();
                     //   EffectSettings2.applyScale = true;
                     //   EffectSettings2.soundName.IsNullOrWhiteSpace();
                        Transform transform = component.FindChild("LeftHand");
                        Transform transform2 = component.FindChild("RightHand");
                        if (transform)
                        {
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = transform.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original, effectData, true);
                        }
                        if (transform2)
                        {
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = transform2.position;
                            effectData.scale = 1f;
                            RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                        }
                    }
                    timeRemaining = 1000f;
                    //   RoR2.EntityStateMachine entityStateMachine = new RoR2.EntityStateMachine();
                    //  entityStateMachine.SetState(zotunSummonShield);

                    //   Debug.Log("statemachine");

                    //  Debug.Log("setstate");

                }

               
            }

        }





            private static GameObject CreateModel(GameObject main, int index)
        {
            Destroy(main.transform.Find("ModelBase").gameObject);
            Destroy(main.transform.Find("CameraPivot").gameObject);
            Destroy(main.transform.Find("AimOrigin").gameObject);
            GameObject result = null;
            bool flag = index == 0;
            if (flag)
            {
                result = Assets.MainAssetBundle.LoadAsset<GameObject>("LordZot");
            }
            else
            {
                bool flag2 = index == 1;
                if (flag2)
                {
                    result = Assets.MainAssetBundle.LoadAsset<GameObject>("ZotDisplayPrefab");
                }
            }
            return result;

        }
    
      
        //  private static void FixedUpdate()
        // {
        //   FixedUpdate();
        //    Debug.Log("ITS RUNNING!!!");
        //    if (summonduration.Equals(true))
        //      Debug.Log("it did run tho1");

        //  {

        //    Debug.Log("it did run tho2");
        //    new WaitForSecondsRealtime(5f);
        //  {
        //      Debug.Log("then it waited");
        //      ZotUnSummonShield zotunSummonShield = new ZotUnSummonShield();

        //        Debug.Log("and set the state");
        //          summonduration.Equals(false);
        //            Debug.Log("and killed the flag");
        //       }

        //       };


        //    }



        internal static void CreatePrefab()
        {
          
       
            
            // first clone the commando prefab so we can turn that into our own survivor
            LordZot.zotPrefab = Prefabs.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "LordZot", true);
           // Debug.Log("cloned");
            
            LordZot.zotPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            // create the model here, we're gonna replace commando's model with our own
            GameObject model = CreateModel(LordZot.zotPrefab, 0);
          //  Debug.Log("model created");

            GameObject gameObject2 = new GameObject("ModelBase");
            gameObject2.transform.parent = LordZot.zotPrefab.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 0.75f, 0f);
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
           // Debug.Log("modelbase");
            GameObject gameObject3 = new GameObject("CameraPivot");
            gameObject3.transform.parent = gameObject2.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 0f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = Vector3.one;
          //  Debug.Log("campiv");
            GameObject gameObject4 = new GameObject("AimOrigin");
            gameObject4.transform.parent = gameObject2.transform;
            gameObject4.transform.localPosition = new Vector3(0f, 2f, 0f);
            gameObject4.transform.localRotation = Quaternion.identity;
            gameObject4.transform.localScale = Vector3.one;
           // Debug.Log("aim");
            Transform transform = model.transform;
            transform.parent = gameObject2.transform;
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.localRotation = Quaternion.identity;

         //   Debug.Log("direction");

            RoR2.CharacterDirection characterDirection = LordZot.zotPrefab.GetComponent<RoR2.CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = gameObject2.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 150f;
            
           // Debug.Log("model set up");

            // set up the character body here
            RoR2.CharacterBody bodyComponent = LordZot.zotPrefab.GetComponent<RoR2.CharacterBody>();
            bodyComponent.bodyIndex = RoR2.BodyIndex.None;
            bodyComponent.baseNameToken = "ZOT_NAME"; // name token
            bodyComponent.subtitleNameToken = "ZOT_SUBTITLE"; // subtitle token- used for umbras
            bodyComponent.bodyFlags = RoR2.CharacterBody.BodyFlags.ImmuneToExecutes | RoR2.CharacterBody.BodyFlags.IgnoreFallDamage | RoR2.CharacterBody.BodyFlags.ImmuneToGoo | RoR2.CharacterBody.BodyFlags.SprintAnyDirection;
            bodyComponent.rootMotionInMainState = false;
            
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = 400;
            bodyComponent.levelMaxHealth = 0;
            bodyComponent.baseRegen = 0.0f;
            bodyComponent.levelRegen = 0.0f;
            bodyComponent.baseMaxShield = 0;                
            bodyComponent.levelMaxShield = 0;
            bodyComponent.baseMoveSpeed = 3.5f;
            bodyComponent.levelMoveSpeed = 0;           
            bodyComponent.baseAcceleration = 7f;
            bodyComponent.baseJumpPower = 0f;
            bodyComponent.levelJumpPower = 0;
            bodyComponent.baseDamage = 0;
            bodyComponent.levelDamage = 0f;
            bodyComponent.baseAttackSpeed = 0.8f;
            bodyComponent.levelAttackSpeed = 0;
            bodyComponent.baseCrit = 0;
            bodyComponent.levelCrit = 0;
            bodyComponent.baseArmor = 100;
            bodyComponent.levelArmor = 0;
            bodyComponent.baseJumpCount = 0;
            bodyComponent.sprintingSpeedMultiplier = 1.0f;
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            bodyComponent.aimOriginTransform = gameObject4.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            bodyComponent.portraitIcon = Assets.icon1portrait;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.preferredInitialStateType = Resources.Load<GameObject>("Prefabs/CharacterBodies/GolemBody").GetComponent<RoR2.CharacterBody>().preferredInitialStateType;
            bodyComponent.preferredPodPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<RoR2.CharacterBody>().preferredPodPrefab;
            bodyComponent.skinIndex = 0U;
         //   Debug.Log("body set up");
            // the charactermotor controls the survivor's movement and stuff
            RoR2.CharacterMotor characterMotor = LordZot.zotPrefab.GetComponent<RoR2.CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 525000f;
            characterMotor.airControl = 1f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;
            
          //  characterMotor.stepOffset = 35f;
            //characterMotor.useGravity = true;
            //characterMotor.isFlying = false;
            //   Debug.Log("movement set up");

            RoR2.InputBankTest inputBankTest = LordZot.zotPrefab.GetComponent<RoR2.InputBankTest>();
            inputBankTest.moveVector = Vector3.zero;

            RoR2.CameraTargetParams cameraTargetParams = LordZot.zotPrefab.GetComponent<RoR2.CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/GolemBody").GetComponent<RoR2.CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = new Vector3 (0f, 0f, 0f);
            cameraTargetParams.dontRaycastToPivot = false;
            

            // this component is used to locate the character model(duh), important to set this up here
            RoR2.ModelLocator modelLocator = LordZot.zotPrefab.GetComponent<RoR2.ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject2.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = false; // set true if you want your character to rotate on terrain like acrid does
            modelLocator.preserveModel = false;
            
            //    Debug.Log("model locator set up");

            // childlocator is something that must be set up in the unity project, it's used to find any child objects for things like footsteps or muzzle flashes
            // also important to set up if you want quality

            // this component is used to handle all overlays and whatever on your character, without setting this up you won't get any cool effects like burning or freeze on the character
            // it goes on the model object of course

            ChildLocator childLocator = model.GetComponent<ChildLocator>();


            RoR2.CharacterModel characterModel = model.AddComponent<RoR2.CharacterModel>();
            characterModel.body = bodyComponent;
            characterModel.baseRendererInfos = new RoR2.CharacterModel.RendererInfo[]
            {
                // set up multiple rendererinfos if needed, but for this example there's only the one
                new RoR2.CharacterModel.RendererInfo
                {
                    defaultMaterial = model.GetComponentInChildren<SkinnedMeshRenderer>().material,
                    renderer = model.GetComponentInChildren<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };

            characterModel.GetComponentInChildren<SkinnedMeshRenderer>().material.shader = Resources.Load<Shader>("shaders/deferred/hgstandard");
           

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<RoR2.TemporaryOverlay>();
           
            RoR2.TeamComponent teamComponent = null;
            if (LordZot.zotPrefab.GetComponent<RoR2.TeamComponent>() != null) teamComponent = LordZot.zotPrefab.GetComponent<RoR2.TeamComponent>();
            else teamComponent = LordZot.zotPrefab.GetComponent<RoR2.TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            RoR2.HealthComponent healthComponent = LordZot.zotPrefab.GetComponent<RoR2.HealthComponent>();
            healthComponent.health = 400;
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;


            LordZot.zotPrefab.GetComponent<RoR2.Interactor>().maxInteractionDistance = 12f;
            LordZot.zotPrefab.GetComponent<RoR2.InteractionDriver>().highlightInteractor = true;
           // Debug.Log("stats set up");
            // this disables ragdoll since the character's not set up for it, and instead plays a death animation
            RoR2.CharacterDeathBehavior characterDeathBehavior = LordZot.zotPrefab.GetComponent<RoR2.CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = LordZot.zotPrefab.GetComponent<RoR2.EntityStateMachine>();
            characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));
            

            RoR2.SetStateOnHurt setStateOnHurt = LordZot.zotPrefab.GetComponent<RoR2.SetStateOnHurt>();
            setStateOnHurt.canBeFrozen = false;
            setStateOnHurt.canBeStunned = false;
            setStateOnHurt.canBeHitStunned = false;
           

            // edit the sfxlocator if you want different sounds
            RoR2.SfxLocator sfxLocator = LordZot.zotPrefab.GetComponent<RoR2.SfxLocator>();
            sfxLocator.deathSound = "Play_ui_player_death";
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_golem_step";
            sfxLocator.fallDamageSound = "Play_golem_step";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";



        //    Debug.Log("sounds set up");

            Rigidbody rigidbody = LordZot.zotPrefab.GetComponent<Rigidbody>();
            rigidbody.mass = 525000f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
         
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.constraints = RigidbodyConstraints.None;
           

            CapsuleCollider capsuleCollider = LordZot.zotPrefab.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 6f;
            capsuleCollider.height = 1.7f;
            capsuleCollider.direction = 1;
            

            KinematicCharacterMotor kinematicCharacterMotor = LordZot.zotPrefab.GetComponent<KinematicCharacterMotor>();
            kinematicCharacterMotor.CharacterController = characterMotor;
            kinematicCharacterMotor.Capsule = capsuleCollider;
         
            kinematicCharacterMotor.Rigidbody = rigidbody;
       

            kinematicCharacterMotor.DetectDiscreteCollisions = true;
            
            kinematicCharacterMotor.GroundDetectionExtraDistance = 35.5f;
            kinematicCharacterMotor.MaxStepHeight = 7000f;
            kinematicCharacterMotor.MinRequiredStepDepth = 250f;
            kinematicCharacterMotor.MaxStableSlopeAngle = 555f;
            kinematicCharacterMotor.MaxStableDistanceFromLedge = 4.9f;
            kinematicCharacterMotor.PreventSnappingOnLedges = false;
            kinematicCharacterMotor.MaxStableDenivelationAngle = 555f;
            kinematicCharacterMotor.RigidbodyInteractionType = RigidbodyInteractionType.Kinematic;
            kinematicCharacterMotor.PreserveAttachedRigidbodyMomentum = true;
            kinematicCharacterMotor.HasPlanarConstraint = false;
            kinematicCharacterMotor.PlanarConstraintAxis = Vector3.up;
            kinematicCharacterMotor.StepHandling = StepHandlingMethod.Extra;
            kinematicCharacterMotor.LedgeHandling = true;
            kinematicCharacterMotor.InteractiveRigidbodyHandling = true;
            kinematicCharacterMotor.SafeMovement = false;
            kinematicCharacterMotor.SetCapsuleDimensions(8, 8, 3.3f);
            kinematicCharacterMotor.Capsule.contactOffset = 1f;
       

            //    Debug.Log("physics set up");

            // this sets up the character's hurtbox, kinda confusing, but should be fine as long as it's set up in unity right
            RoR2.HurtBoxGroup zotBoxGroup = model.AddComponent<RoR2.HurtBoxGroup>();

            RoR2.HurtBox componentInChildren = model.GetComponentInChildren<CapsuleCollider>().gameObject.AddComponent<RoR2.HurtBox>();
            componentInChildren.gameObject.layer = RoR2.LayerIndex.entityPrecise.intVal;
            componentInChildren.healthComponent = healthComponent;
            componentInChildren.isBullseye = true;
            componentInChildren.damageModifier = RoR2.HurtBox.DamageModifier.Normal;
            componentInChildren.hurtBoxGroup = zotBoxGroup;
            componentInChildren.indexInGroup = 0;

            zotBoxGroup.hurtBoxes = new RoR2.HurtBox[]
            {
                componentInChildren
            };

            zotBoxGroup.mainHurtBox = componentInChildren;
            zotBoxGroup.bullseyeCount = 1;

            //  Debug.Log("hurtbox set up");



            RoR2.HitBoxGroup hitBoxGroup = model.AddComponent<RoR2.HitBoxGroup>();

            GameObject gameObject5 = new GameObject("FuryHitbox");
            gameObject5.transform.parent = LordZot.zotPrefab.transform;
            gameObject5.transform.localPosition = new Vector3(0f, 0f, -7f);
            gameObject5.transform.localRotation = Quaternion.identity;
            gameObject5.transform.localScale = new Vector3(35f, 35f, 35f);
            RoR2.HitBox hitBox = gameObject5.AddComponent<RoR2.HitBox>();
            gameObject5.layer = RoR2.LayerIndex.projectile.intVal;
            hitBoxGroup.hitBoxes = new RoR2.HitBox[]
            {
                hitBox
            };
            hitBoxGroup.groupName = "FuryHitGroup";



            // this is for handling footsteps, not needed but polish is always good
            RoR2.FootstepHandler ZotStep = model.AddComponent<RoR2.FootstepHandler>();
            ZotStep.baseFootstepString = "Play_golem_step";
            ZotStep.sprintFootstepOverrideString = "";
            ZotStep.enableFootstepDust = true;
            ZotStep.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericHugeFootstepDust");

        
            // ragdoll controller is a pain to set up so we won't be doing that here..
            RoR2.RagdollController ragdollController = model.AddComponent<RoR2.RagdollController>();
            ragdollController.bones = null;
            ragdollController.componentsToDisableOnRagdoll = null;

            // this handles the pitch and yaw animations, but honestly they are nasty and a huge pain to set up so i didn't bother
            RoR2.AimAnimator aimAnimator = model.AddComponent<RoR2.AimAnimator>();
            aimAnimator.inputBank = inputBankTest;
            aimAnimator.directionComponent = characterDirection;
            aimAnimator.pitchRangeMax = 120f;
            aimAnimator.pitchRangeMin = -120f;
            aimAnimator.yawRangeMin = -120f;
            aimAnimator.yawRangeMax = 120f;
            aimAnimator.pitchGiveupRange = 180f;
            aimAnimator.yawGiveupRange = 200f;
            aimAnimator.giveupDuration = 0.9f;
                
           // Debug.Log("misc set up");

           // void FixedUpdate()
          //  {LordZot.highestRecordedFallSpeed = Mathf.Max(LordZot.highestRecordedFallSpeed, characterMotor ? (-characterMotor.velocity.y) : 0f); };

        }


        internal static RoR2.BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            RoR2.BuffDef buffDef = ScriptableObject.CreateInstance<RoR2.BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            buffDefs.Add(buffDef);
            
            return buffDef;
        }

        internal void CreateDisplayPrefab()
        {

            float model_scale = 0.12f;

            GameObject gameObject = Prefabs.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "ZotDisplayPrefab");

            GameObject gameObject2 = CreateModel(gameObject, 1);

            GameObject gameObject3 = new GameObject("ModelBase");
            gameObject3.transform.parent = gameObject.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 0f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject gameObject4 = new GameObject("CameraPivot");
            gameObject4.transform.parent = gameObject.transform;
            gameObject4.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            gameObject4.transform.localRotation = Quaternion.identity;
            gameObject4.transform.localScale = Vector3.one;

            GameObject gameObject5 = new GameObject("AimOrigin");
            gameObject5.transform.parent = gameObject.transform;
            gameObject5.transform.localPosition = new Vector3(0f, 1.8f, 0f);
            gameObject5.transform.localRotation = Quaternion.identity;
            gameObject5.transform.localScale = Vector3.one;

            Transform transform = gameObject2.transform;
            transform.parent = gameObject3.transform;
            transform.localPosition = new Vector3(-0.04f, 0.45f, -0.5f);
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);  //originally 0.01

            RoR2.ModelLocator modelLocator = gameObject.GetComponent<RoR2.ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject3.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = false; // set true if you want your character to rotate on terrain like acrid does
            modelLocator.preserveModel = false;

            ChildLocator childLocator = gameObject.GetComponent<ChildLocator>();
           
            RoR2.CharacterModel characterModel = gameObject2.AddComponent<RoR2.CharacterModel>();
            characterModel.body = null;

            characterModel.baseRendererInfos = new RoR2.CharacterModel.RendererInfo[]

            {
                new RoR2.CharacterModel.RendererInfo
                {


                    defaultMaterial =  gameObject2.GetComponentInChildren<SkinnedMeshRenderer>().material,
                    renderer =  gameObject2.GetComponentInChildren<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false,
                }

            };
            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<RoR2.TemporaryOverlay>();

            characterModel.SetFieldValue("mainSkinnedMeshRenderer", characterModel.baseRendererInfos[0].renderer.gameObject.GetComponent<SkinnedMeshRenderer>());


            characterDisplay = Prefabs.InstantiateClone(gameObject.GetComponent<RoR2.ModelLocator>().modelBaseTransform.gameObject, "ZotDisplayPrefab", true);
            characterDisplay.AddComponent<NetworkIdentity>();



        }

       
    

        public class Materialy
        {
            public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
            {
                bool flag = !LordZot.commandoMat;
                if (flag)
                {
                    LordZot.commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RoR2.CharacterModel>().baseRendererInfos[0].defaultMaterial;
                }
                Material material = UnityEngine.Object.Instantiate<Material>(LordZot.commandoMat);
                Material material2 = Assets.MainAssetBundle.LoadAsset<Material>(materialName);
                bool flag2 = !material2;
                Material result;
                if (flag2)
                {
                    Debug.LogError(materialName + "doesn't exist");
                    result = LordZot.commandoMat;
                }
                else
                {
                    material.name = materialName;
                    material.SetColor("_Color", material2.GetColor("_Color"));
                    material.SetTexture("_MainTex", material2.GetTexture("_MainTex"));
                    material.SetColor("_EmColor", emissionColor);
                    material.SetFloat("_EmPower", emission);
                    material.SetTexture("_EmTex", material2.GetTexture("_EmissionMap"));
                    material.SetFloat("_NormalStrength", normalStrength);
                    result = material;
                }
                return result;
            }

            // Token: 0x060000C1 RID: 193 RVA: 0x00006DA8 File Offset: 0x00004FA8
            public static Material CreateMaterial(string materialName)
            {
                return Materialy.CreateMaterial(materialName, 0f);
            }

            // Token: 0x060000C2 RID: 194 RVA: 0x00006DC8 File Offset: 0x00004FC8
            public static Material CreateMaterial(string materialName, float emission)
            {
                return Materialy.CreateMaterial(materialName, emission, Color.black);
            }

            // Token: 0x060000C3 RID: 195 RVA: 0x00006DE8 File Offset: 0x00004FE8
            public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
            {
                return Materialy.CreateMaterial(materialName, emission, emissionColor, 0f);
            }

        }

        private void RegisterCharacter()


        {



            







                // write a clean survivor description here!
                string desc = "A stranger from another reality, Lord Zot has come to this planet seeking ever more power.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > An exceedingly heavy survivor who gains power from natural resources and gems. -Unimplemented-" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - Completely unable to sprint, relying on ability usage for mobility." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - Cannot be knocked back, and causes collateral damage to foes when landing, and even when taking mere footsteps." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - Abilities (And Jump!) can be held down to infuse them with Gem Power, increasing their magnitude with no upper limit." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - The rules of this world do not apply to Lord Zot. His attacks do not utilize Base Damage, nor do they interact with On-Hit Effects." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - Zot's Gem Power regeneration increases the higher his Gem Power Maximum is." + Environment.NewLine + Environment.NewLine;

            // add the language tokens
            Languages.Add("ZOT_NAME", "Lord Zot");
            Languages.Add("ZOT_DESCRIPTION", desc);
            Languages.Add("ZOT_SUBTITLE", "Mystic Titan");

            // add our new survivor to the game~

            RoR2.SurvivorDef survivorDef = ScriptableObject.CreateInstance<RoR2.SurvivorDef>();

            survivorDef.cachedName = "ZOT_NAME";
            survivorDef.unlockableName = "";
            survivorDef.descriptionToken = "ZOT_DESCRIPTION";
            survivorDef.primaryColor = characterColor;
            survivorDef.bodyPrefab = zotPrefab;
            survivorDef.displayPrefab = characterDisplay;
            


            Loadouts.RegisterSurvivorDef(survivorDef);
            // set up the survivor's skills here
            SkillSetup();


            EnigmaticThunder.Modules.Bodies.RegisterBody(zotPrefab);
   
        }

        void SkillSetup()
        {
            // get rid of the original skills first, otherwise we'll have commando's loadout and we don't want that
            foreach (RoR2.GenericSkill obj in zotPrefab.GetComponentsInChildren<RoR2.GenericSkill>())
            {
                BaseUnityPlugin.DestroyImmediate(obj);
            }

            PassiveSetup();
            PrimarySetup();
            ZotjumpSetup();
           ZotSecondarySetup();
            ZotUtilitySetup();
            ZotSpecialSetup();
        }

        void RegisterStates()
        {


            // register the entitystates for networking reasons
            Loadouts.RegisterEntityState(typeof(EldritchFury));
            Loadouts.RegisterEntityState(typeof(ZotBlink));
            Loadouts.RegisterEntityState(typeof(ZotBulwark));
            Loadouts.RegisterEntityState(typeof(ZotJump));
            Loadouts.RegisterEntityState(typeof(EldritchSlam));
        }

        void PassiveSetup()
        {
            // set up the passive skill here if you want
            RoR2.SkillLocator component = zotPrefab.GetComponent<RoR2.SkillLocator>();

            Languages.Add("ZOT_PASSIVE_NAME", "Mystic Titan");
            Languages.Add("ZOT_PASSIVE_DESCRIPTION", "<style=cIsDamage>Zot is unable to sprint.</style> <style=cIsUtility>Each footstep deals mild damage and knockback in an area." +
                " Hold jump to begin storing power for a mighty leap. There is no maximum charge." +
                " Zot deals heavy damage in a radius when landing, with radius and damage increasing based on velocity.</style> " +
                " Press V mid-air to toggle float. (activation may be a bit finicky at the moment, so press multiple times until it works lol)" +
                " (Debug cheats: LCTRL+U = Gem Power Boost, LCTRL+L = Charge Jump based on a percentage of Gem Power instead of a flat value)" +
                " Every 120 seconds, Zot gains a small power boost (Since the gem system isn't finished)");

            component.passiveSkill.enabled = true;
            component.passiveSkill.skillNameToken = "ZOT_PASSIVE_NAME";
            component.passiveSkill.skillDescriptionToken = "ZOT_PASSIVE_DESCRIPTION";
            component.passiveSkill.icon = Assets.iconP;
        }

        void PrimarySetup()
        {
            RoR2.SkillLocator component = zotPrefab.GetComponent<RoR2.SkillLocator>();

            Languages.Add("ZOT_PRIMARY_NAME", "Eldritch Fury");
            Languages.Add("ZOT_PRIMARY_DESCRIPTION", "Zot winds up and throws a mighty punch with one of his shielded gauntlets, alternating arms with each swing. Deals <style=cIsDamage>900% damage</style>." +
                "<style=cIsUtility>Damage dealt falls off based on distance.</style>");

            // set up your primary skill def here!

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(EldritchFury));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0.5f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon1;
            mySkillDef.skillDescriptionToken = "ZOT_PRIMARY_DESCRIPTION";
            mySkillDef.skillName = "ZOT_PRIMARY_NAME";
            mySkillDef.skillNameToken = "ZOT_PRIMARY_NAME";

            Loadouts.RegisterSkillDef(mySkillDef);

            component.primary = zotPrefab.AddComponent<RoR2.GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            Loadouts.RegisterSkillFamily(newFamily);
            component.primary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = component.primary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new RoR2.ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };


            // add this code after defining a new skilldef if you're adding an alternate skill

            /*Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = newSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(newSkillDef.skillNameToken, false, null)
            };*/
        }
        void ZotSpecialSetup()
        {
            RoR2.SkillLocator component = zotPrefab.GetComponent<RoR2.SkillLocator>();

            Languages.Add("ZOT_SPECIAL_NAME", "Eldritch Slam");
            Languages.Add("ZOT_SPECIAL_DESCRIPTION", "Zot holds a fist in the air, and then hurls an immense blow with high knockback, dealing <style=cIsDamage>900%</style> damage. (If aimed towards the ground, half damage is dealt in an AoE, and enemies are knocked upwards.) " +
                "<style=cIsUtility>Damage dealt falls off based on distance.</style>");

            // set up your primary skill def here!

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(EldritchSlam));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 1f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon1;
            mySkillDef.skillDescriptionToken = "ZOT_SPECIAL_DESCRIPTION";
            mySkillDef.skillName = "ZOT_SPECIAL_NAME";
            mySkillDef.skillNameToken = "ZOT_SPECIAL_NAME";

            Loadouts.RegisterSkillDef(mySkillDef);

            component.special = zotPrefab.AddComponent<RoR2.GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            Loadouts.RegisterSkillFamily(newFamily);
            component.special.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = component.special.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new RoR2.ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }


        private void ZotUtilitySetup()
        {
            RoR2.SkillLocator component = LordZot.zotPrefab.GetComponent<RoR2.SkillLocator>();
            Languages.Add("ZOT_UTILITY_BLINK_NAME", "Titan's Stride");
            Languages.Add("ZOT_UTILITY_BLINK_DESCRIPTION", "Tap quickly to <style=cIsUtility>dash incredibly fast</style> in the direction of your movement." +
                "When grounded, this ability behaves differently; it will <style=cIsUtility>navigate terrain and avoid entering the air.</style>");
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(ZotBlink));
            skillDef.activationStateMachineName = "Body";
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 1f;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.Any;

            skillDef.isCombatSkill = false;
            skillDef.mustKeyPress = false;
           // skillDef.noSprint = false;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
           // skillDef.shootDelay = 0f;
            skillDef.stockToConsume = 1;
            skillDef.icon = Assets.KayleStride;
            skillDef.skillDescriptionToken = "ZOT_UTILITY_BLINK_DESCRIPTION";
            skillDef.skillName = "ZOT_UTILITY_BLINK_NAME";
            skillDef.skillNameToken = "ZOT_UTILITY_BLINK_NAME";
            Loadouts.RegisterSkillDef(skillDef);
            component.utility = LordZot.zotPrefab.AddComponent<RoR2.GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];
            Loadouts.RegisterSkillFamily(skillFamily);
            Reflection.SetFieldValue<SkillFamily>(component.utility, "_skillFamily", skillFamily);
            SkillFamily skillFamily2 = component.utility.skillFamily;
            skillFamily2.variants[0] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                unlockableName = "",
                viewableNode = new RoR2.ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        private void ZotSecondarySetup()
        {
            RoR2.SkillLocator component = LordZot.zotPrefab.GetComponent<RoR2.SkillLocator>();
            Languages.Add("ZOT_SECONDARY_NAME", "Gem Bulwark");
            Languages.Add("ZOT_SECONDARY_DESCRIPTION", " <style=cIsUtility>Deflect nearby enemies and attacks</style> with a backhand, as you begin charging a gem-powered beam that, once released, deals heavy damage in an area." +
                "<style=cIsUtility>Can be charged indefinitely, increasing blast radius and damage.</style>");
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(ZotBulwark));
            skillDef.activationStateMachineName = "Weapon";
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0.5f;
            skillDef.beginSkillCooldownOnSkillEnd = true;
            skillDef.canceledFromSprinting = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.PrioritySkill;
           // skillDef.isBullets = false;
            skillDef.isCombatSkill = false;
            skillDef.mustKeyPress = false;
          //  skillDef.noSprint = false;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
          //  skillDef.shootDelay = 0f;
            skillDef.stockToConsume = 1;
            skillDef.icon = Assets.icon2;
            skillDef.skillDescriptionToken = "ZOT_SECONDARY_DESCRIPTION";
            skillDef.skillName = "ZOT_SECONDARY_NAME";
            skillDef.skillNameToken = "ZOT_SECONDARY_NAME";
            Loadouts.RegisterSkillDef(skillDef);
            component.secondary = LordZot.zotPrefab.AddComponent<RoR2.GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];
            Loadouts.RegisterSkillFamily(skillFamily);
            Reflection.SetFieldValue<SkillFamily>(component.secondary, "_skillFamily", skillFamily);
            SkillFamily skillFamily2 = component.secondary.skillFamily;
            skillFamily2.variants[0] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                unlockableName = "",
                viewableNode = new RoR2.ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }
     

            private void ZotjumpSetup()
        {
            RoR2.SkillLocator component = LordZot.zotPrefab.GetComponent<RoR2.SkillLocator>();
            Languages.Add("ZOT_SECONDARY_NAME", "Gem Bulwark");
            Languages.Add("ZOT_SECONDARY_DESCRIPTION", " <style=cIsUtility>Deflect nearby enemies</style> with a backhand, and begin charging a gem-powered beam that, once released, deals heavy damage. <style=cIsUtility>C</style>");
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(BaseLeap));
            skillDef.activationStateMachineName = "Body";
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0f;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.PrioritySkill;
           // skillDef.isBullets = false;
            skillDef.isCombatSkill = false;
            skillDef.mustKeyPress = false;
          //  skillDef.noSprint = false;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
           // skillDef.shootDelay = 0f;
            skillDef.stockToConsume = 1;
            skillDef.icon = Assets.icon2;
            skillDef.skillDescriptionToken = "ZOT_SECONDARY_DESCRIPTION";
            skillDef.skillName = "ZOT_SECONDARY_NAME";
            skillDef.skillNameToken = "ZOT_SECONDARY_NAME";
            Loadouts.RegisterSkillDef(skillDef);
            
        }
        private void FootstepBlast(On.RoR2.FootstepHandler.orig_Footstep_AnimationEvent orig, RoR2.FootstepHandler self, AnimationEvent animationEvent)
        {
            var player = playerCharacterMaster;

            var bod = model;
            var modely = bod.modelLocator.modelTransform;
            
            var animaty = modely.GetComponent<Animator>();

            bool flag3 = RoR2.LocalUserManager.GetFirstLocalUser().cachedBody.baseNameToken is "ZOT_NAME";
            bool flag = animationEvent.stringParameter is "LeftFoot"; 
            bool flag2 = animationEvent.stringParameter is "RightFoot";
            if (flag && flag3)
            {

                var bod2 = model;
                var model2 = bod2.modelLocator.modelTransform;

                ChildLocator component = model2.GetComponent<ChildLocator>();
                if (component)
                {
                    Transform transform = component.FindChild("LeftFoot");

                    if (transform)
                    {

                        var position = transform.position + new Vector3(0f, 0f, 0f);

                        new RoR2.BlastAttack
                        {
                            procCoefficient = 0f,
                            attacker = model.gameObject,
                            inflictor = model.gameObject,
                           teamIndex = RoR2.TeamComponent.GetObjectTeam(model.gameObject),
                            baseDamage = 200,
                            baseForce = 1,
                            position = position,
                            radius = 2f,
                            falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                            crit = false
                        }.Fire();
                        RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                            LandingBlast.attacker = bod2.gameObject;
                            LandingBlast.inflictor = bod2.gameObject;
                            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                            LandingBlast.baseDamage = 40 * 0.5f;
                            LandingBlast.baseForce = 15f;
                            LandingBlast.bonusForce = new Vector3(0f, 640f, 0f);
                            LandingBlast.position = position;
                            LandingBlast.radius = 17f;
                            LandingBlast.procCoefficient = 0f;
                        LandingBlast.crit = false;
                            LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                            LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                            LandingBlast.Fire();
                        
                        
                      
                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                        

                        RoR2.EffectData effectData2 = new RoR2.EffectData();

                        effectData2.origin = position;
                        effectData2.scale = 1.5f; 
                        GameObject original66 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                        RoR2.EffectData effectData3 = new RoR2.EffectData();

                        effectData3.origin = position + new Vector3 (0f, -1f, 0f);
                        effectData3.scale = 5.5f;
                        RoR2.EffectManager.SpawnEffect(original66, effectData3, true);
                        RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                      //  RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                    }
                }


            };

            if (flag2 && flag3)
            {


                var bod2 = model;
                var model2 = bod2.modelLocator.modelTransform;

                ChildLocator component = model2.GetComponent<ChildLocator>();
                if (component)
                {
                    Transform transform = component.FindChild("RightFoot");

                    if (transform)
                    {

                        var position = transform.position + new Vector3(0f, 0f, 0f);
                        new RoR2.BlastAttack
                        {
                            procCoefficient = 0f,
                            attacker = model.gameObject,
                            inflictor = model.gameObject,
                            teamIndex = RoR2.TeamComponent.GetObjectTeam(model.gameObject),
                            baseDamage = 200,
                            baseForce = 1,
                            position = position,
                            radius = 2f,
                            falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                            crit = false
                        }.Fire();

                        RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                            LandingBlast.attacker = bod2.gameObject;
                            LandingBlast.inflictor = bod2.gameObject;
                            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                            LandingBlast.baseDamage = 40 * 0.5f;
                            LandingBlast.baseForce = 15f;
                            LandingBlast.bonusForce = new Vector3(0f, 640f, 0f);
                            LandingBlast.position = position;
                            LandingBlast.procCoefficient = 0f;
                            LandingBlast.radius = 17f;
                            LandingBlast.crit = false;
                        LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                            LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                            LandingBlast.Fire();
                        
                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                        RoR2.EffectData effectData2 = new RoR2.EffectData();

                        effectData2.origin = position;
                        effectData2.scale = 1.5f;
                        GameObject original66 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                        RoR2.EffectData effectData3 = new RoR2.EffectData();

                        effectData3.origin = position + new Vector3(0f, -1f, 0f);
                        effectData3.scale = 7.5f;
                        RoR2.EffectManager.SpawnEffect(original66, effectData3, true);
                        RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                      //  RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                    }
                }


            };




            {
                orig(self, animationEvent);
            }


        }


      



        private void CreateDoppelganger()
        {
            // set up the doppelganger for artifact of vengeance here
            // quite simple, gets a bit more complex if you're adding your own ai, but commando ai will do

            doppelganger = Prefabs.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "ExampleSurvivorMonsterMaster", true, "C:\\Users\\test\\Documents\\ror2mods\\ExampleSurvivor\\ExampleSurvivor\\ExampleSurvivor\\LordZot.cs", "CreateDoppelganger", 159);

            Masters.RegisterMaster(this.doppelganger);
            RoR2.CharacterMaster component = doppelganger.GetComponent<RoR2.CharacterMaster>();
            component.bodyPrefab = zotPrefab;







        }

    }
   

    // get the assets from your assetbundle here
    // if it's returning null, check and make sure you have the build action set to "Embedded Resource" and the file names are right because it's not gonna work otherwise
    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;
        //public static AssetBundleResourcesProvider Provider;
        public static RoR2.CharacterModel ZotDisplayPrefab;
        public static RoR2.CharacterModel LordZot;
        public static GameObject GemPowerBar;
        public static Texture emissioncolor;
        public static Texture charPortrait;
        public static Texture icon1portrait;
        public static Sprite iconP;
        public static Sprite fill;
        public static Sprite bar;
           public static Sprite icon1;
         public static Sprite icon2;
         public static Sprite icon3;
        public static Sprite gameboypunch;
        public static Sprite KayleStride;
        public static Sprite gempower;
        public static Sprite gempower2;
        public static Texture gempowa;
        public static Material Zotbodmat;
        public static Material Material015;
        // public static Sprite icon4;

        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LordZot.ZotAssetBundle"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    //Provider = new AssetBundleResourcesProvider("@LordZot", MainAssetBundle);
                   // EnigmaticThunder.AddProvider(Provider);
                    var materials = MainAssetBundle.LoadAllAssets<Material>();

                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].shader.name == "Standard")
                        {
                            PleaseWork = Materialy.CreateMaterial("ZotMat", 0.5f, new Color(1f, 0.3f, 1f), 1f);

                            materials[i].shader = Resources.Load<Shader>("shaders/deferred/hgstandard");
                            materials[i].CopyPropertiesFromMaterial(PleaseWork);
                               
                        }
                    }
                }
            }

            // include this if you're using a custom soundbank
            /*using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("ExampleSurvivor.ExampleSurvivor.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }*/

            // and now we gather the assets
            charPortrait = MainAssetBundle.LoadAsset<Sprite>("ZotMysticTitan_Passive").texture;

            iconP = MainAssetBundle.LoadAsset<Sprite>("ZotMysticTitan_Passive");
            icon1 = MainAssetBundle.LoadAsset<Sprite>("ZotEldritchFury");
            icon2 = MainAssetBundle.LoadAsset<Sprite>("ZotBlast");
            icon3 = MainAssetBundle.LoadAsset<Sprite>("ZotMysticTitan_Passive");
            gameboypunch = MainAssetBundle.LoadAsset<Sprite>("Untitled");
            KayleStride = MainAssetBundle.LoadAsset<Sprite>("bad_stride");
            icon1portrait = MainAssetBundle.LoadAsset<Sprite>("ZotMysticTitan_Passive").texture;
            gempower = MainAssetBundle.LoadAsset<Sprite>("gempower");
            gempower2 = MainAssetBundle.LoadAsset<Sprite>("gempower");
            gempowa = MainAssetBundle.LoadAsset<Sprite>("gempower").texture;
            Zotbodmat = MainAssetBundle.LoadAsset<Material>("Material.015");
            emissioncolor = MainAssetBundle.LoadAsset<Texture>("emission2");


        }
    }
}



 
// the entitystates namespace is used to make the skills, i'm not gonna go into detail here but it's easy to learn through trial and error
namespace EntityStates.ZotStates
{


   
   




    public class ZotBulwark : BaseState
        {
            // Token: 0x06003C55 RID: 15445 RVA: 0x000FB0B8 File Offset: 0x000F92B8
            public override void OnEnter()
            {
            LordZot.LordZot.ChargedLaser = 0f;
                base.OnEnter();
            prevaim = base.characterBody.aimOriginTransform.localPosition;
            backhandhappened = false;
           // characterBody.aimOriginTransform.position = characterBody.aimOriginTransform.position + characterBody.aimOriginTransform.forward * 1.5f + characterBody.aimOriginTransform.up * 4.5f; ;
            characterBody.characterDirection.turnSpeed = 400f;
            base.GetModelAnimator();
            Transform modelTransform = base.GetModelTransform();
            Vector3 prevcampos()
            {  return base.cameraTargetParams.cameraParams.standardLocalCameraPos;
            
            
            }

            LeftTrail.emitting = true;
            RightTrail.emitting = true;


            if (isAuthority)
            {
                originalcampos = prevcampos();
            }
            if (!characterMotor.isGrounded && LordZot.LordZot.ugh)
            {
                PlayAnimation("Body", "AscendDescend");
            };
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
           

          
          
         //   EffectSettings = sonicboom.GetComponent<RoR2.EffectComponent>();
           // EffectSettings.applyScale = true;

            if (base.isAuthority)
            {
               
                LordZot.LordZot.Busy = true;
                LordZot.LordZot.holdtime = false;
                LordZot.LordZot.dontmove = false;
               

                base.cameraTargetParams.cameraParams.standardLocalCameraPos.x = -4f;
              //  base.cameraTargetParams.cameraParams.standardLocalCameraPos.y = -3f;
             //   base.cameraTargetParams.cameraParams.standardLocalCameraPos.z = -22f;
            }

            
            this.stopwatch = 0.7f;
            this.stopwatch1 = 0f;
            this.stopwatch21 = 0f;
            this.stupidtime2 = 0f;
            
            stupidtime = 0f;
            bool flag = base.skillLocator;
            if (flag)
            {
                base.skillLocator.secondary.skillDef.activationStateMachineName = "Weapon";
            }

            
            

            this.laserOn = true;

                floatpower += 8f;



                timeRemaining = 6.0f;
                ticker2 = 2f;
                Animator modelAnimator = base.GetModelAnimator();
                modelAnimator.SetFloat("ShieldsIn", 0.12f);
           

            this.duration = ZotBulwark.baseDuration;
            
                this.chargePlayID = RoR2.Util.PlayAttackSpeedSound(ChargeLaser.attackSoundString, base.gameObject, (1f / this.duration));
                if (modelTransform)
            {

             
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                GameObject laser = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                GameObject laserfab = (EntityStates.GolemMonster.ChargeLaser.laserPrefab);
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                if(isAuthority)
                { 
                    if (component)
                    {
                        Transform transform = component.FindChild("RightHand");
                    Transform transform2 = component.FindChild("LeftHand");
                        if (transform)
                        {
                            if (ZotBulwark.effectPrefab)
                            {
                                this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(EntityStates.GolemMonster.ChargeLaser.effectPrefab, transform.position, transform.rotation);
                                this.chargeEffect.transform.parent = transform;
                                RoR2.ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                if (component2)
                                {
                                    component2.newDuration = this.duration;
                                }
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = transform.position;
                                effectData.scale = 1f;
                                if (timeRemaining <= 0f)
                                { RoR2.EffectManager.SpawnEffect(original, effectData, true); }
                            }
                            if (ZotBulwark.laserPrefab)
                            {
                                this.laserEffect = UnityEngine.Object.Instantiate<GameObject>(EntityStates.GolemMonster.ChargeLaser.laserPrefab, transform.position, transform.rotation);
                                this.laserEffect.transform.parent = transform;
                                this.laserLineComponent = this.laserEffect.GetComponent<LineRenderer>();
                            }
                            if (transform2)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = transform2.position;
                                effectData.scale = 1f;
                                if (timeRemaining <= 0f)
                                { RoR2.EffectManager.SpawnEffect(original2, effectData, true); }
                               
                            }

                        }
                    }
                }
                }
                if (base.characterBody)
                {
                    base.characterBody.SetAimTimer(this.duration);
                    StartAimMode(this.duration);
                }


        }

            // Token: 0x06003C56 RID: 15446 RVA: 0x000FB208 File Offset: 0x000F9408
            public override void OnExit()
        {
            characterBody.characterDirection.turnSpeed = 150;

            LeftTrail.emitting = false;
            RightTrail.emitting = false;
            base.characterBody.SetAimTimer(0f);
            StartAimMode(0f);
            base.GetModelAnimator().SetBool("GodStop", true);
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            AkSoundEngine.StopPlayingID(this.chargePlayID);
               
            LordZot.LordZot.ugh = false;
            LordZot.LordZot.holdtime = false;
            LordZot.LordZot.dontmove = false;
            if (isAuthority)
            {
                base.cameraTargetParams.cameraParams.standardLocalCameraPos = originalcampos;
            }
            LordZot.LordZot.Busy = false;
            LordZot.LordZot.Charging = false;
           // Destroy(da);
            if (this.chargeEffect)
                {
                    EntityState.Destroy(this.chargeEffect);
             
            }
                if (this.laserEffect)
                {
                    EntityState.Destroy(this.laserEffect);
                }
            PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
            base.OnExit();


        }


        // Token: 0x06003C57 RID: 15447 RVA: 0x000FB258 File Offset: 0x000F9458
        public override void Update()
        {
            base.Update();
            if (this.stopwatch21 > 0.41f)
            {
                    Transform modelTransform = base.GetModelTransform();
                //    GameObject original = EntityStates.BrotherMonster.FistSlam.chargeEffectPrefab; 
                    ChildLocator component = modelTransform.GetComponent<ChildLocator>();

                    if (component)
                    {
                        Transform transform = component.FindChild("RightHand");
                        if (transform)
                        {
                            
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = transform.position;
                            effectData.scale = 2f + (LordZot.LordZot.ChargedLaser * 0.4f); 
                          //  RoR2.EffectManager.SpawnEffect(original, effectData, true);
                            
                        }
                    };
              
              


                if (laserEffect && laserLineComponent)
                {
                    if (LordZot.LordZot.ChargedLaser < 20f)
                    {
                        float num = 1000000f;

                        Ray aimRay = GetAimRay();
                       // aimRay.origin += characterBody.aimOriginTransform.forward * 1.5f + characterBody.aimOriginTransform.up * 4.5f;
                        Vector3 position = laserEffect.transform.parent.position;
                        Vector3 point = aimRay.GetPoint(num);
                        this.laserDirection = point - position;
                      
                        RaycastHit raycastHit;
                        if (Physics.Raycast(aimRay, out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.entityPrecise.mask))
                        {
                            point = raycastHit.point;
                        }
                        laserLineComponent.SetPosition(0, position);
                        laserLineComponent.SetPosition(1, point);

                        laserLineComponent.startWidth = 1f + (LordZot.LordZot.ChargedLaser * 0.21f);
                        laserLineComponent.endWidth = 1f + (LordZot.LordZot.ChargedLaser * 0.21f);
                        GameObject end = Resources.Load<GameObject>("prefabs/effects/ChargeOrbitalLaser");
                        GameObject end2 = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                       // EffectSettings2 = end2.GetComponent<RoR2.EffectComponent>();
                     //   EffectSettings2.applyScale = true;
                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                        effectData2.origin = point;
                        effectData2.scale = 1f + (0.5f * LordZot.LordZot.ChargedLaser );
                        RoR2.EffectManager.SpawnEffect(end2, effectData2, true);

                        /*        {
                                    flicker.transform.position = point;
                                }*/
                    }
                    else
                    {
                        float num = 1000000f;
                        Ray aimRay = GetAimRay();

                        Vector3 position = laserEffect.transform.parent.position;
                        Vector3 point = aimRay.GetPoint(num);
                        this.laserDirection = point - position;
                        
                        if (Physics.Raycast(aimRay, out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.entityPrecise.mask))
                        {
                            point = raycastHit.point;
                        }
                        laserLineComponent.SetPosition(0, position);
                        laserLineComponent.SetPosition(1, point);

                        laserLineComponent.startWidth = 3.7f;
                        laserLineComponent.endWidth = 3.7f;
        GameObject end = Resources.Load<GameObject>("prefabs/effects/ChargeOrbitalLaser");
        GameObject end2 = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                       // EffectSettings2 = end2.GetComponent<RoR2.EffectComponent>();
                    //    EffectSettings2.applyScale = true;
                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                        effectData2.origin = point;
                        effectData2.scale = 1f + (0.5f * LordZot.LordZot.ChargedLaser);
                        RoR2.EffectManager.SpawnEffect(end2, effectData2, true);
                    };


                }
            }
        }
        // Token: 0x06003C58 RID: 15448 RVA: 0x000FB3B4 File Offset: 0x000F95B4
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch21 += Time.fixedDeltaTime;
            this.updatewatch += Time.fixedDeltaTime;
            bool flag26 = base.inputBank.skill2.down;
            bool flag266 = base.inputBank.skill2.justReleased;
            bool flag27 = this.stopwatch1 >= this.duration;
            bool flag29 = this.updatewatch >= 1f;
            bool outofpower = LordZot.LordZot.GemPowerVal <= 0f;
            this.duration = 100 + Time.fixedDeltaTime;
            stupidtime2 -= Time.fixedDeltaTime;
            LordZot.LordZot.timeRemaining = 5f;

            if (this.stopwatch21 < 0.15f && !backhandhappened && !flag26)
            {
                Ray backhand = GetAimRay();
                RoR2.Util.CleanseBody(base.characterBody, false, false, false, false, true);
                Animator animator = GetModelAnimator();
                base.PlayCrossfade("Gesture, Additive", "GemBulwark", "FireArrow.playbackRate", 1f, 0.05f);

                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();

                LandingBlast.attacker = base.gameObject;
                LandingBlast.inflictor = base.gameObject;
                LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                LandingBlast.baseDamage = (40 * 0.5f);
                LandingBlast.procCoefficient = 0f;
                LandingBlast.baseForce = 2500f + (LordZot.LordZot.MaxGemPowerVal * 15);
                LandingBlast.bonusForce = (LandingBlast.baseForce * (backhand.direction) + (Vector3.up * LandingBlast.baseForce * 0.7f));
                LandingBlast.position = characterBody.corePosition + characterBody.characterDirection.forward * 1.5f;
                LandingBlast.radius = 20f;
                LandingBlast.crit = false;
                LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.Linear;
                LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                LandingBlast.Fire();
                new RoR2.BlastAttack
                {
                    procCoefficient = 0f,
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                    baseDamage = 11,
                    baseForce = 250f + (LordZot.LordZot.MaxGemPowerVal * 4),
                    position = characterBody.corePosition + characterBody.characterDirection.forward * 1.5f,
                    radius = 55f + 0.03f * LordZot.LordZot.MaxGemPowerVal,
                    falloffModel = RoR2.BlastAttack.FalloffModel.Linear,
                    crit = false
                }.Fire();
                new RoR2.BlastAttack
                {
                    procCoefficient = 0f,
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                    baseDamage = 4,
                    baseForce = 250f + (LordZot.LordZot.MaxGemPowerVal * 1),
                    position = characterBody.corePosition + characterBody.characterDirection.forward * 1.5f,
                    radius = 100f + 0.09f * LordZot.LordZot.MaxGemPowerVal,
                    falloffModel = RoR2.BlastAttack.FalloffModel.Linear,
                    crit = false
                }.Fire();
                characterBody.characterDirection.turnSpeed = 250;
                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                float randomx = (UnityEngine.Random.Range(-7f, 7f));
                var direc = Quaternion.LookRotation(base.GetAimRay().direction, base.characterBody.transform.up);
                GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                Transform modelTransform = base.GetModelTransform();
                sonicboomeffectdata.scale = 5f;
                sonicboomeffectdata.rotation = direc * randomSpin;
                sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -1f + modelTransform.up * 2f + (modeltransform.right * randomx);
                RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();

                sonicboomeffectdata2.scale = 5f;
                sonicboomeffectdata2.rotation = direc * randomSpin;
                sonicboomeffectdata2.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f;

                RoR2.EffectData sonicboomeffectdata3 = new RoR2.EffectData();

                sonicboomeffectdata3.scale = 5f;
                sonicboomeffectdata3.rotation = direc * randomSpin;
                sonicboomeffectdata3.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f + model.transform.right * 4f;

                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                backhandhappened = true;
            }
            if (this.stopwatch21 < 0.15f && backhandhappened)
            {
                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                float randomx = (UnityEngine.Random.Range(-7f, 7f));
                var direc = Quaternion.LookRotation(base.GetAimRay().direction, base.characterBody.transform.up);
                GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                Transform modelTransform = base.GetModelTransform();
                sonicboomeffectdata.scale = 5f;
                sonicboomeffectdata.rotation = direc * randomSpin;
                sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -1f + modelTransform.up * 2f + (modeltransform.right * randomx);
                RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();

                sonicboomeffectdata2.scale = 5f;
                sonicboomeffectdata2.rotation = direc * randomSpin;
                sonicboomeffectdata2.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f;

                RoR2.EffectData sonicboomeffectdata3 = new RoR2.EffectData();

                sonicboomeffectdata3.scale = 5f;
                sonicboomeffectdata3.rotation = direc * randomSpin;
                sonicboomeffectdata3.origin = modelTransform.position + modelTransform.forward * -3f + modelTransform.up * 2f + model.transform.right * 4f;

                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            }

                if (this.stopwatch21 > 0.41f && flag26)
            {
               if (!Charging)
                {
                    if (GemPowerVal > 3f)
                    {
                        LordZot.LordZot.GemPowerVal -= 3f;
                        ChargedLaser += 3f;
                    }
                    else
                    {
                        ChargedLaser += GemPowerVal;
                        LordZot.LordZot.GemPowerVal -= 3f; 
                    
                    }
                    LordZot.LordZot.Charging = true;
                    GameObject original2 = EntityStates.GolemMonster.ChargeLaser.effectPrefab;
                    Transform modelTransform = base.GetModelTransform();
                    ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                    Transform transform2 = component.FindChild("RightHand");
                    this.chargePlayID = RoR2.Util.PlayAttackSpeedSound(ChargeLaser.attackSoundString, base.gameObject, 1f);
                    GameObject laser = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = transform2.position;
                    effectData.scale = 1f + (1f * LordZot.LordZot.ChargedLaser);
                    if (transform2 && isAuthority)
                    {

                        RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                        RoR2.EffectManager.SpawnEffect(laser, effectData, true);
                    }
                };


                if (Charging)
                {
                    if (updatewatch >= 0.5f)
                    {
                        if (LordZot.LordZot.ChargedLaser > 10f)
                        {
                            Transform modelTransform = base.GetModelTransform();
                            ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                            Transform transform2 = component.FindChild("RightHand");
                            if (transform2)
                            {
                                GameObject laser2 = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                                RoR2.EffectData effectData = new RoR2.EffectData();
                                effectData.origin = transform2.position;
                                effectData.scale = 1f + (1f * LordZot.LordZot.ChargedLaser);
                                if (isAuthority)
                                { RoR2.EffectManager.SpawnEffect(laser2, effectData, true); };
                            }
                        };
                        if (LordZot.LordZot.ChargedLaser > 30f)
                        {
                            Transform modelTransform = base.GetModelTransform();
                            ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                            Transform transform2 = component.FindChild("RightHand");
                           
                            {
                                GameObject laser2 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalPrespawnEffect");
                                GameObject laser3 = Resources.Load<GameObject>("prefabs/effects/TitanSpawnEffect");

                                if (effectPrefab)
                                {
                                    this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(laser2, LordZot.LordZot.righthand.position, LordZot.LordZot.righthand.rotation);
                                    this.chargeEffect.transform.parent = transform;
                                    RoR2.ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                    if (component2)
                                    {
                                        component2.newDuration = 1f;
                                    }
                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                    effectData.origin = transform.position;
                                    effectData.scale = 0.5f;
                                    RoR2.EffectManager.SpawnEffect(chargeEffect, effectData, true);
                                    RoR2.EffectData effectData2 = new RoR2.EffectData();
                                    effectData.origin = characterBody.footPosition;
                                    effectData.scale = 1f + (0.2f * LordZot.LordZot.ChargedLaser);
                                    RoR2.EffectManager.SpawnEffect(laser3, effectData2, true);
                                }


                        
                                if (transform2)
                                {
                                    //   EffectSettings3 = laser2.GetComponent<RoR2.EffectComponent>();
                                    //  EffectSettings3.applyScale = true;
                                    //  EffectSettings3.parentToReferencedTransform = true;
                                    laser2.AddComponent<RoR2.DestroyOnTimer>().duration = 1.5f;

                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                    effectData.origin = LordZot.LordZot.righthand.position;
                                    effectData.scale = 1f + (1f * LordZot.LordZot.ChargedLaser);
                                    RoR2.EffectManager.SpawnEffect(laser2, effectData, true);

                                }

                            };


                            if (LordZot.LordZot.ChargedLaser > 15f)
                            {
                                RoR2.ShakeEmitter shakeEmitter;
                                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                shakeEmitter.wave = new Wave
                                {
                                    amplitude = 0.1f,
                                    frequency = 180f,
                                    cycleOffset = 0f
                                };
                                shakeEmitter.duration = 1f;
                                shakeEmitter.radius = 50f;
                                shakeEmitter.amplitudeTimeDecay = false;
                                LordZot.LordZot.timeRemaining = 6f;

                            }
                            else
                            {
                                RoR2.ShakeEmitter shakeEmitter;
                                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                shakeEmitter.wave = new Wave
                                {
                                    amplitude = 0.01f * LordZot.LordZot.ChargedLaser,
                                    frequency = 180f,
                                    cycleOffset = 0f
                                };
                                shakeEmitter.duration = 1f;
                                shakeEmitter.radius = 50f;
                                shakeEmitter.amplitudeTimeDecay = false;
                                LordZot.LordZot.timeRemaining = 6f;

                            };
                            if (LordZot.LordZot.ChargedLaser > 15f)
                            {


                                GameObject original3 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                              //  EffectSettings4 = original3.GetComponent<RoR2.EffectComponent>();
                              //  EffectSettings4.applyScale = true;
                                // EffectSettings4.parentToReferencedTransform = true;

                                RoR2.EffectData effectData2 = new RoR2.EffectData();
                                effectData2.origin = LordZot.LordZot.righthand.position;
                                effectData2.scale = 0.2f;
                                RoR2.EffectManager.SpawnEffect(original3, effectData2, true);
                            }
                            if (LordZot.LordZot.ChargedLaser > 40f)
                            {
                                GameObject laser2 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalPrespawnEffect");
                                GameObject laser3 = Resources.Load<GameObject>("prefabs/effects/TitanSpawnEffect");

                                if (effectPrefab)
                                {
                                    this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(laser2, LordZot.LordZot.righthand.position, LordZot.LordZot.righthand.rotation);
                                    this.chargeEffect.transform.parent = transform;
                                    RoR2.ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                    if (component2)
                                    {
                                        component2.newDuration = 1f;
                                    }
                                    RoR2.ScaleParticleSystemDuration component1 = laser3.GetComponent<RoR2.ScaleParticleSystemDuration>();
                                    if (component1)
                                    {
                                        component1.newDuration = 1f;
                                    }
                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                    effectData.origin = transform.position;
                                    effectData.scale = 0.5f;
                                    RoR2.EffectManager.SpawnEffect(chargeEffect, effectData, true);
                                    RoR2.EffectData effectData2 = new RoR2.EffectData();
                                    effectData.origin = characterBody.footPosition;
                                    effectData.scale = 1f + (0.2f * LordZot.LordZot.ChargedLaser);
                                    RoR2.EffectManager.SpawnEffect(laser3, effectData2, true);
                                }
                            }
                        }
                            if (this.stopwatch21 > 1.1f && this.stopwatch21 < 1.15f)
                        {


                            GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                            RoR2.EffectData effectData = new RoR2.EffectData();

                            effectData.origin = characterBody.footPosition;
                            effectData.scale = 5f;
                            RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                        };

                    }


                    this.updatewatch = 0f;
                    };
  



                if (stupidtime >= 0)
                {
                    stupidtime -= Time.fixedDeltaTime;
                };
                if (stupidtime <= 0)
                {
                    LordZot.LordZot.holdtime = false;
                };

             
               

                base.characterDirection.moveVector += base.inputBank.aimDirection;
            }
            if ((outofpower | (!flag26) && this.stopwatch21 >= 0.41f) && base.isAuthority)
            {
                LordZot.LordZot.Gemdrain = 1f;
                skillLocator.secondary.DeductStock(1);
                ZotLaser zotLaser = new ZotLaser();
                zotLaser.laserDirection = laserDirection;
                this.outer.SetNextState(zotLaser);
                PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
                return;
            };

            if (flag26 && this.stopwatch21 > 0.41f && !LordZot.LordZot.holdtime)
            {
                LordZot.LordZot.Charging = true;
                PlayCrossfade("Bulwark", "HoldBulwark", null, 1, 0.4f);
                this.modelanimator = GetModelAnimator();
                modelanimator.SetBool("GodStop", false);
                modelanimator.SetLayerWeight(4, 1);
                LordZot.LordZot.holdtime = true;
                stupidtime = 10f;
                {

                }
            };
            if (stopwatch21 >= 0.4f && !flag26 && base.isAuthority)
            {
                base.GetModelAnimator().SetBool("GodStop", true);
                PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
                characterBody.aimOriginTransform.localPosition = prevaim;
                PlayCrossfade("Body", "Idle", null, 1f, 0.4f);
                skillLocator.secondary.AddOneStock();
                this.outer.SetNextStateToMain();
            };
        }

            // Token: 0x06003C59 RID: 15449 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.Death;
            }

            // Token: 0x0400370E RID: 14094
            public static float baseDuration = 2f;

            // Token: 0x0400370F RID: 14095
       
     
        // Token: 0x04003710 RID: 14096
        public static GameObject effectPrefab = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);

        // Token: 0x04003711 RID: 14097
        public static GameObject laserPrefab = (EntityStates.GolemMonster.ChargeLaser.laserPrefab);
        private Vector3 originalcampos;
            // Token: 0x04003712 RID: 14098
            public static string attackSoundString;
        private float stopwatch;

        // Token: 0x04003713 RID: 14099
        private float duration;

            // Token: 0x04003714 RID: 14100
            private uint chargePlayID;

            // Token: 0x04003715 RID: 14101
            private GameObject chargeEffect;

        GameObject original2737 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalPrespawnEffect");
        // Token: 0x04003716 RID: 14102
        private GameObject laserEffect;
        private RaycastHit raycastHit;
        // Token: 0x04003717 RID: 14103
        private LineRenderer laserLineComponent;

            // Token: 0x04003718 RID: 14104
            private Vector3 laserDirection;

            // Token: 0x04003719 RID: 14105
            private Vector3 visualEndPosition;

            // Token: 0x0400371A RID: 14106
            private float flashTimer;

            // Token: 0x0400371B RID: 14107
            private bool laserOn;
        private float updatewatch;
        private float stopwatch1;
        private float stupidtime;
        private float stopwatch21;
        private Animator modelanimator;
        private float stupidtime2;
       // private GameObject da;
        private RoR2.EffectComponent EffectSettings3;
        
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings4;
        private bool backhandhappened;

        //  private Light flicker;

        public float ChargeDuration { get; private set; }
    }


    public class ZotLaser : BaseState
    {
        // Token: 0x06003C64 RID: 15460 RVA: 0x000FB714 File Offset: 0x000F9914
        public override void OnEnter()
        {
            base.OnEnter();
            PlayCrossfade("Bulwark", "NoBulwark", null, 1, 0.4f);
            LordZot.LordZot.floatpower += ChargedLaser;
            LordZot.LordZot.ChargedLaser += 0.1f;
            LordZot.LordZot.Busy = true;
            this.duration = ZotLaser.baseDuration / this.attackSpeedStat;
            this.modifiedAimRay = base.GetAimRay();
            
            this.modifiedAimRay.direction = this.laserDirection;
            LordZot.LordZot.Charging = false;
            base.GetModelAnimator().SetBool("GodStop", true);
            base.GetModelAnimator().SetLayerWeight(4, 1);
            LordZot.LordZot.timeRemaining = 6f;
            Transform modelTransform = base.GetModelTransform();
            RoR2.Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);
            string text = "RightHand";
            if (base.characterBody)
            {
              //  base.characterBody.SetAimTimer(1f);
            }
            if (effectPrefab)
            {
                RoR2.EffectManager.SimpleMuzzleFlash(EntityStates.GolemMonster.FireLaser.effectPrefab, base.gameObject, text, true);
            }
            PlayAnimation("Gesture, Additive", "FireBulwark", "FireArrow.playbackRate", 1f);
            if (LordZot.LordZot.ChargedLaser > 40)
            {
                RoR2.Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            }
                if (base.isAuthority)
            {
               
                float num = 1000000f;
                Vector3 vector = this.modifiedAimRay.origin + this.modifiedAimRay.direction * num;
                
                RaycastHit raycastHit;
                if (Physics.Raycast(this.modifiedAimRay, out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.defaultLayer.mask | RoR2.LayerIndex.entityPrecise.mask))
                {
                    vector = raycastHit.point;
                }
                new RoR2.BlastAttack
                { procCoefficient = 0f,
                attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                    baseDamage = 40 * 2.5f * LordZot.LordZot.ChargedLaser,
                    baseForce = 1000 + 555f * LordZot.LordZot.ChargedLaser,
                    position = vector,
                    radius = 6f + 0.9f * LordZot.LordZot.ChargedLaser,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    crit = false,
                bonusForce = ZotLaser.force * this.modifiedAimRay.direction
                }.Fire();
                Vector3 origin = this.modifiedAimRay.origin;
                if (modelTransform)
                {
                    ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                    if (component)
                    {
                        Vector3 vector2 = characterBody.corePosition + characterDirection.forward * 1f;
                        TeamIndex teamIndex = characterBody ? characterBody.teamComponent.teamIndex : TeamIndex.None;
                        int childIndex = component.FindChildIndex(text);
                        if (tracerEffectPrefab)
                        {
                            RoR2.EffectData effectData = new RoR2.EffectData
                            {   scale = 0.5f + (1.9f * LordZot.LordZot.ChargedLaser),
                                origin = vector,
                                start = this.modifiedAimRay.origin
                            };
                            effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                            GameObject original27 = Resources.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                            GameObject original277 = Resources.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");
                            GameObject original27767 = Resources.Load<GameObject>("prefabs/effects/MaulingRockImpact");
                            GameObject original27773 = Resources.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                            GameObject original277671 = Resources.Load<GameObject>("prefabs/effects/SmokescreenEffect");
                            GameObject original277672 = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                            GameObject original277673 = Resources.Load<GameObject>("prefabs/effects/TitanDeathEffect");
                            GameObject original277674 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                            GameObject original277675 = Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                            GameObject original277676 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                            /*  EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                              EffectSettings.applyScale = true;
                              EffectSettings.parentToReferencedTransform = true;
                              EffectSettings2 = original27.GetComponent<RoR2.EffectComponent>();
                              EffectSettings2.applyScale = true;
                              EffectSettings2.parentToReferencedTransform = true;
                              EffectSettings4 = original277.GetComponent<RoR2.EffectComponent>();
                              EffectSettings4.applyScale = true;
                              EffectSettings4.parentToReferencedTransform = true;*/
                            GameObject original2737 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                            GameObject original2777 = Resources.Load<GameObject>("prefabs/effects/LaserTurbineBombExplosion");
                            /* EffectSettings5 = original2737.GetComponent<RoR2.EffectComponent>();
                             EffectSettings5.applyScale = true;
                             EffectSettings5.parentToReferencedTransform = true;

                             EffectSettings3 = original2777.GetComponent<RoR2.EffectComponent>();
                             EffectSettings3.applyScale = false;
                             EffectSettings3.parentToReferencedTransform = true;*/
                            RoR2.EffectData impact = new RoR2.EffectData();
                            impact.scale = 0.5f + (1f * LordZot.LordZot.ChargedLaser);
                            impact.origin = raycastHit.point;
                            impact.rotation = Quaternion.FromToRotation(raycastHit.point, (raycastHit.normal * 2));
                            RoR2.EffectData blast = new RoR2.EffectData();
                            blast.scale = 0.5f;
                            blast.origin = raycastHit.point;
                            RoR2.EffectData blast2 = new RoR2.EffectData();

                            ChildLocator component2 = modelTransform.GetComponent<ChildLocator>();
                                Transform transform = component2.FindChild("RightHand");

                            blast2.scale = 1f + (0.8f * LordZot.LordZot.ChargedLaser);
                            blast2.origin = transform.position;
                            RoR2.EffectManager.SpawnEffect(original27767, impact, true);
                            RoR2.EffectManager.SpawnEffect(original277671, impact, true);
                            RoR2.EffectManager.SpawnEffect(original277672, impact, true);

                           


                            RoR2.EffectManager.SpawnEffect(original277674, impact, true);
                            RoR2.EffectManager.SpawnEffect(original277675, impact, true);
                            RoR2.EffectManager.SpawnEffect(original277676, impact, true);
                            RoR2.EffectManager.SpawnEffect(original27, blast, true);
                            RoR2.EffectManager.SpawnEffect(original2, blast, true);
                            RoR2.EffectManager.SpawnEffect(original2, blast2, true);
                            RoR2.EffectManager.SpawnEffect(ZotLaser.tracerEffectPrefab, effectData, true);
                            RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, effectData, true);
                            if (LordZot.LordZot.ChargedLaser > 40)
                            {
                               
                                RoR2.EffectManager.SpawnEffect(original277, blast, true);

                                RoR2.EffectManager.SpawnEffect(original277673, impact, true);
                                RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                                RoR2.EffectManager.SpawnEffect(original27773, impact, true);
                                LordZot.LordZot.timeRemaining = 6f;
                            };


                                if (LordZot.LordZot.ChargedLaser < 240)
                            {
                                RoR2.ShakeEmitter shakeEmitter;
                                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                shakeEmitter.wave = new Wave
                                {
                                    amplitude = 0.005f * LordZot.LordZot.ChargedLaser,
                                    frequency = 180f,
                                    cycleOffset = 0f
                                };
                                shakeEmitter.duration = 0.02f * LordZot.LordZot.ChargedLaser;
                                shakeEmitter.radius = 50f;
                                shakeEmitter.amplitudeTimeDecay = true;
                                LordZot.LordZot.timeRemaining = 6f;
                            }
                            else
                            {
                                RoR2.ShakeEmitter shakeEmitter;
                                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                shakeEmitter.wave = new Wave
                                {
                                    amplitude = 1f,
                                    frequency = 180f,
                                    cycleOffset = 0f
                                };
                                shakeEmitter.duration = 2.5f;
                                shakeEmitter.radius = 50f;
                                shakeEmitter.amplitudeTimeDecay = true;
                                LordZot.LordZot.timeRemaining = 6f;
                            };



                        }
                    }
                }
            }
        }

        // Token: 0x06003C65 RID: 15461 RVA: 0x00032FA7 File Offset: 0x000311A7
        public override void OnExit()
        {
            base.OnExit();
            characterBody.aimOriginTransform.localPosition = prevaim;
            LordZot.LordZot.ChargedLaser = 0f;
            skillLocator.secondary.DeductStock(1);
            LordZot.LordZot.Busy = false;
            PlayAnimation("Gesture, Additive", "BufferEmpty", "FireArrow.playbackRate", 1f);
        }

        // Token: 0x06003C66 RID: 15462 RVA: 0x000FB969 File Offset: 0x000F9B69
        public override void FixedUpdate()
        {
            base.FixedUpdate();



            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06003C67 RID: 15463 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }

        // Token: 0x04003728 RID: 14120
        public static GameObject effectPrefab = GolemMonster.FireLaser.effectPrefab;

        // Token: 0x04003729 RID: 14121
        public static GameObject hitEffectPrefab = GolemMonster.FireLaser.hitEffectPrefab;

        // Token: 0x0400372A RID: 14122
        public static GameObject tracerEffectPrefab = GolemMonster.FireLaser.tracerEffectPrefab;

        // Token: 0x0400372B RID: 14123
        public static float damageCoefficient;

        // Token: 0x0400372C RID: 14124
        public static float blastRadius;

        // Token: 0x0400372D RID: 14125
        public static float force;

        // Token: 0x0400372E RID: 14126
        public static float minSpread;

        // Token: 0x0400372F RID: 14127
        public static float maxSpread;

        // Token: 0x04003730 RID: 14128
        public static int bulletCount;

        // Token: 0x04003731 RID: 14129
        public static float baseDuration = 0.2f;

        // Token: 0x04003732 RID: 14130
        public static string attackSoundString;

        // Token: 0x04003733 RID: 14131
        public Vector3 laserDirection;

        // Token: 0x04003734 RID: 14132
        private float duration;

        // Token: 0x04003735 RID: 14133
        private Ray modifiedAimRay;
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings4;
        private RoR2.EffectComponent EffectSettings3;
        private RoR2.EffectComponent EffectSettings5;
        private bool hitwall;
    }

    public class ZotBlink : BaseState
    {

        public override void OnEnter()
        {
            base.OnEnter();

            if (LordZot.LordZot.GemPowerVal < 2f && base.isAuthority)
            {
               
                didntstartgemdrained = false;
                LordZot.LordZot.Gemdrain = 1f;
                this.outer.SetNextStateToMain(); }
            else
            {
                
                
                if (characterMotor.isGrounded)
                {
                    nottoofar = true;
                    castfromground = true;
                }
                else
                if
                (GemPowerVal < 2f && base.isAuthority)
                {
                    didntstartgemdrained = false;
                    LordZot.LordZot.Gemdrain = 1f;
                    this.outer.SetNextStateToMain();
                }
                else
                { castfromground = false;
                  LordZot.LordZot.GemPowerVal -= 2f;
                };
                didntstartgemdrained = true;

                RoR2.Util.PlaySound(BaseSlideState.soundString, base.gameObject);
                RoR2.Util.PlaySound(ExitSkyLeap.soundString, zotPrefab);
                this.modelTransform = base.GetModelTransform();
                LordZot.LordZot.GemPowerVal -= 2f;


                Vector3 b = base.inputBank.moveVector * this.blinkDistance;
                characterBody.characterDirection.turnSpeed = 200000f;
                this.blinkDestination = base.transform.position;
                this.blinkStart = base.transform.position;
                NodeGraph groundNodes = RoR2.SceneInfo.instance.groundNodes;
                NodeGraph.NodeIndex nodeIndex = groundNodes.FindClosestNode(base.transform.position + b, base.characterBody.hullClassification);

                
                groundNodes.GetNodePosition(nodeIndex, out this.blinkDestination);
                
                this.blinkDestination += base.transform.position - base.characterBody.footPosition + new Vector3(0f, 0.1f, 0f);
                if (blinkDestination.magnitude > 150)
                { nottoofar = false; };
                if (blinkDestination.magnitude < 150)
                { nottoofar = true; };

                this.CreateBlinkEffect(RoR2.Util.GetCorePosition(base.gameObject));
                

                if (this.modelTransform)
                {
                    this.characterModel = this.modelTransform.GetComponent<RoR2.CharacterModel>();
                }
                if (this.characterModel)
                { 


                    foreach (RoR2.CharacterModel.RendererInfo renderInfo in characterModel.baseRendererInfos)
                    {

                        {
                            Material mat = renderInfo.defaultMaterial;
                            mat.shader = LordZot.LordZot.distortion;
                            mat.shader.SetPropertyValue("_DistortionAmount", 255f);
                        }
                    }
                }

                if (this.hurtboxGroup)
                {
                    RoR2.HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter = 1;
                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                }


                if (NetworkServer.active)
                {
                    RoR2.Util.CleanseBody(base.characterBody, true, false, false, false, false);
                }
                if (BaseSlideState.slideEffectPrefab && base.characterBody)
                {
                    Vector3 position = base.characterBody.corePosition;
                    Quaternion rotation = Quaternion.identity;
                    RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                    sonicboomeffectdata.scale = 5f;
                    sonicboomeffectdata.rotation = rotation;
                    sonicboomeffectdata.origin = position;

                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);

                    Transform transform = base.FindModelChild(BaseSlideState.slideEffectMuzzlestring);
                    
                    if (transform)
                    {  
                        position = transform.position;
                    }
                    if (base.characterDirection)
                    {
                        rotation = RoR2.Util.QuaternionSafeLookRotation(this.slideRotation * base.characterDirection.forward, Vector3.up);
                    }
                    RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                    LandingBlast.attacker = characterBody.gameObject;
                    LandingBlast.inflictor = characterBody.gameObject;
                    LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                    LandingBlast.baseDamage = 40 * 0.5f;
                    LandingBlast.baseForce = 4000f;

                    LandingBlast.position = position;
                    LandingBlast.procCoefficient = 0f;
                    LandingBlast.radius = 35f;
                    LandingBlast.crit = characterBody.RollCrit();
                    LandingBlast.Fire();

                    RoR2.EffectData sonicboomeffectdata3 = new RoR2.EffectData();
                    sonicboomeffectdata3.scale = 5f;
                    sonicboomeffectdata3.rotation = rotation;
                    sonicboomeffectdata3.origin = position;

                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata3, true);
                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                    RoR2.EffectManager.SpawnEffect(original2, sonicboomeffectdata3, true);
                    RoR2.EffectManager.SimpleEffect(BaseSlideState.slideEffectPrefab, position, rotation, true);
                    this.shakeEmitter = zotPrefab.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    this.shakeEmitter.wave = new Wave
                    {
                        amplitude = 0.5f,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    this.shakeEmitter.duration = 0.6f;
                    this.shakeEmitter.radius = 50f;
                    this.shakeEmitter.amplitudeTimeDecay = true;

                    if (!characterMotor.isGrounded)
                    {
                        castfromground = false;
                        RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
                        RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
                        RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
                    };



                }
            };
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            RoR2.EffectData effectData = new RoR2.EffectData();
            effectData.rotation = RoR2.Util.QuaternionSafeLookRotation(this.blinkDestination - this.blinkStart);
            effectData.origin = origin;
            RoR2.EffectData effectDat2a = new RoR2.EffectData();
            effectDat2a.rotation = RoR2.Util.QuaternionSafeLookRotation(this.blinkStart - this.blinkDestination);
            effectDat2a.origin = this.blinkDestination;
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, effectDat2a, true);
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, effectData, true);
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, effectData, true);
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, effectData, true);
        }
        private void SetPosition(Vector3 newPosition)
        {
            if (base.characterMotor)
            {
                base.characterMotor.Motor.SetPositionAndRotation(newPosition, Quaternion.identity, true);
            }
        }
        // Token: 0x06003FBF RID: 16319 RVA: 0x0010AE40 File Offset: 0x00109040
        public override void FixedUpdate()
        {
            base.FixedUpdate();

           if (didntstartgemdrained)
            {
                if (base.isAuthority)
                {
                    stopwatch += Time.fixedDeltaTime;
                    base.FixedUpdate();
                    if (characterMotor.isGrounded && castfromground)

                    {

                        if (base.characterMotor && base.characterDirection)
                        {
                            base.characterMotor.velocity = Vector3.zero;
                        }
                        if (!nottoofar)
                        {
                            this.SetPosition(Vector3.Lerp(this.blinkStart, this.blinkDestination, stopwatch / 0.5f));
                        }
                        else
                        {
                            Vector3 a = Vector3.zero;
                            Vector3 c = Vector3.zero;
                            if (base.inputBank && base.characterDirection)
                            {
                                a = GetAimRay().direction;
                                
                            }
                            if (base.characterMotor)
                            {
                                Vector3 position = base.characterBody.corePosition;
                                Quaternion rotation = Quaternion.identity;
                                RoR2.EffectData sonicboomeffectdata2 = new RoR2.EffectData();
                                sonicboomeffectdata2.scale = 5f;
                                sonicboomeffectdata2.rotation = rotation;
                                sonicboomeffectdata2.origin = position;

                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata2, true);
                                float num = BaseSlideState.speedCoefficientCurve.Evaluate(base.fixedAge / ZotBlink.duration);
                                    base.characterMotor.rootMotion += this.slideRotation * (num * (35f) * (a + base.characterMotor.moveDirection) * Time.fixedDeltaTime);
                                

                            }
                        }
                    }
                    else
                    {
                        if (characterMotor.isGrounded && !castfromground)

                        {

                            if (base.characterMotor && base.characterDirection)
                            {
                          
                                base.characterMotor.velocity = Vector3.zero;
                            }

                        }
                        else
                        {
                            
                            {
                                Vector3 a = Vector3.zero;
                                Vector3 c = Vector3.zero;
                                if (base.inputBank && base.characterDirection)
                                {
                                    a = GetAimRay().direction;
                                }
                                if (base.characterMotor)
                                {
                                    if (!castfromground && !characterMotor.isGrounded)
                                    {
                                        float num = BaseSlideState.speedCoefficientCurve.Evaluate(base.fixedAge / ZotBlink.duration);
                                        base.characterMotor.rootMotion += this.slideRotation * (num * (35f) * a * Time.fixedDeltaTime);
                                    }

                                }

                            }
                   
                        }
                    }




                    if (base.fixedAge >= ZotBlink.duration && base.isAuthority)
                    {
                        this.outer.SetNextStateToMain();
                    }
                    if (!inputBank.skill3.down && isGrounded && base.isAuthority)
                    {
                        this.outer.SetNextStateToMain();
                    }
                }
            }
        }

        // Token: 0x06003FC0 RID: 16320 RVA: 0x0010AEFF File Offset: 0x001090FF
        public override void OnExit()
        {
            this.modelTransform = base.GetModelTransform();
            characterBody.characterDirection.turnSpeed = 150f;
            if (LordZot.LordZot.Gemdrain <= 0)
            { skillLocator.utility.Reset();

                if (this.modelTransform)
                {
                    LordZot.LordZot.floatpower += 19f;
                   /* RoR2.TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                    temporaryOverlay.duration = 1f;
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = Resources.Load<Material>("materials/matEcho");
                    temporaryOverlay.inspectorCharacterModel = this.modelTransform.gameObject.GetComponent<RoR2.CharacterModel>();
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.animateShaderAlpha = true;*/
                };


            };


            if (this.characterModel)
            {
                foreach (RoR2.CharacterModel.RendererInfo renderInfo in characterModel.baseRendererInfos)
                {

                    {
                        Material mat = renderInfo.defaultMaterial;
                        mat.shader = LordZot.LordZot.hopoo;
                    }
                }
            }
            if (this.hurtboxGroup)
            {
                RoR2.HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter = 0;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }

            base.OnExit();
            


        }

        // Token: 0x06003FC2 RID: 16322 RVA: 0x0000D472 File Offset: 0x0000B672
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }





        // Token: 0x04003B7E RID: 15230
        public static float duration = 0.4f;
        private Transform modelTransform;
        // Token: 0x04003B7F RID: 15231
        public static AnimationCurve speedCoefficientCurve;

        // Token: 0x04003B80 RID: 15232
        public static AnimationCurve jumpforwardSpeedCoefficientCurve;
        private RoR2.ShakeEmitter shakeEmitter;
        // Token: 0x04003B81 RID: 15233
        public static string soundString;

        // Token: 0x04003B82 RID: 15234
        public static GameObject slideEffectPrefab;

        // Token: 0x04003B83 RID: 15235
        public static string slideEffectMuzzlestring;

        // Token: 0x04003B84 RID: 15236
        protected Vector3 slideVector;

        // Token: 0x04003B85 RID: 15237
        protected Quaternion slideRotation;
        private RoR2.CharacterModel characterModel;
        public static float destealthDuration = 0.7f;
        private RoR2.HurtBoxGroup hurtboxGroup;
        public static GameObject zotPrefab;
        public static GameObject slamImpactEffect;
        public static GameObject slamEffectPrefab;
        public GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
        private Vector3 blinkDestination = Vector3.zero;

        private Vector3 blinkStart = Vector3.zero;
        private float blinkDistance = 28f;
        private static float stopwatch;
        private bool castfromground;

        public bool nottoofar { get; private set; }

        private bool didntstartgemdrained;
    }


    public class ZotJump : BaseState
    {
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
        public override void OnEnter()
        {
            base.OnEnter();

            LordZot.LordZot.Charged = 0;
            LordZot.LordZot.Chargingjump = true;
            this.stopwatch = 0f;
            this.Duration = this.stopwatch + 1f;
            LordZot.LordZot.jumpcooldown = 5f;
            var position = characterBody.footPosition;
            LordZot.LordZot.timeRemainingstun = 0f;
        
            RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
            LandingBlast.attacker = base.gameObject;
            LandingBlast.inflictor = base.gameObject;
            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
            LandingBlast.baseDamage = 40 * 1f;
            LandingBlast.baseForce = 1000f;
            LandingBlast.bonusForce = new Vector3(0f, 2500f, 0f);
            LandingBlast.position = position;
            LandingBlast.procCoefficient = 0f;
            LandingBlast.radius = 25f;
            LandingBlast.crit = base.RollCrit();
            LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
            LandingBlast.Fire();

            GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
            RoR2.EffectData effectData2 = new RoR2.EffectData();

            effectData2.origin = position;
            effectData2.scale = 25f;
            RoR2.EffectManager.SpawnEffect(original6, effectData2, true);

            this.modelAnimator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();
            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
            RoR2.EffectManager.SimpleEffect(original2, characterBody.footPosition, Quaternion.Euler (0f, 0f, 0f), true);
          /*  if (base.isAuthority);
            { }*/
            PlayAnimation("Body", "ChargeJump", "JumpChargeDuration", this.Duration);
        }
        
        public override void OnExit()
        {
            PlayAnimation("Body", "Idle");
            LordZot.LordZot.Chargingjump = false;
            base.OnExit();
           // characterMotor.enabled = true;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            
            this.stopwatch += Time.fixedDeltaTime;
            this.updatewatch += Time.fixedDeltaTime;
            bool outofpower = LordZot.LordZot.GemPowerVal <= 0;
            bool flag26 = base.inputBank.jump.down;
            bool flag266 = base.inputBank.jump.justReleased;
            bool flag27 = this.stopwatch >= this.Duration;
            bool flag29 = this.updatewatch >= 0.3f;
            this.Duration = this.stopwatch + 100;
            if (LordZot.LordZot.floatpower < LordZot.LordZot.Charged)
            { LordZot.LordZot.floatpower += LordZot.LordZot.Charged * 0.02f; };
            LordZot.LordZot.floatpower += Time.fixedDeltaTime;
            LordZot.LordZot.jumpcooldown = this.Duration;
            if (flag29)
            {
                RoR2.ShakeEmitter shakeEmitter;
                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                shakeEmitter.wave = new Wave
                {
                    amplitude = 0.009f * Charged,
                    frequency = 180f,
                    cycleOffset = 0f
                };
                shakeEmitter.duration = 0.3f;
                shakeEmitter.radius = 50f;
                shakeEmitter.amplitudeTimeDecay = false;
                this.updatewatch = 0f;
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.3f * this.stopwatch + 1f;
                
                
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                
            }

            if(characterMotor.isGrounded)
            {
              //  characterMotor.enabled = false;
                characterMotor.velocity = Vector3.zero;};    

                if (LordZot.LordZot.Charged > 5f && flag29)
            {
                
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                new RoR2.BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = 1 * 0.008f * LordZot.LordZot.Charged,
                    baseForce = 15 * 3f * LordZot.LordZot.Charged,
                    bonusForce = new Vector3(0f, 150f * LordZot.LordZot.Charged, 0f),
                    crit = false,
                    damageType = DamageType.Generic,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    procCoefficient = 0,
                    radius = 13f + LordZot.LordZot.Charged * 0.09f,
                    position = model.corePosition,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    teamIndex = base.teamComponent.teamIndex
                }.Fire();
                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.9f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);

            }

            if (LordZot.LordZot.Charged > 10f && flag29)
            {
                new RoR2.BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = 1 * 0.88f * LordZot.LordZot.Charged,
                    baseForce = 250,
                    bonusForce = new Vector3(0f, 150f, 0f),
                    crit = false,
                    damageType = DamageType.Generic,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    procCoefficient = 0,
                    radius = 13f + LordZot.LordZot.Charged * 0.09f,
                    position = model.corePosition,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    teamIndex = base.teamComponent.teamIndex
                }.Fire();
                GameObject original7 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/TitanSpawnEffect");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.15f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original7, effectData, true);
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);

            }

            if (outofpower && flag26 && base.isAuthority)
            {
                LordZot.LordZot.Gemdrain = 1f;

                LordZot.LordZot.jumpcooldown = 0.4f;
                BaseLeap zotLeap = new BaseLeap();
                { LordZot.LordZot.Charged += this.stopwatch * 1; }
                this.outer.SetNextState(zotLeap);

                return;
            }
            if (flag27)
            {
                PlayCrossfade("Body", "Idle", null, 1f, 0.2f);
                if (base.isAuthority)
                {
                    LordZot.LordZot.jumpcooldown = 0f;
                   
                    BaseLeap zotLeap = new BaseLeap();
                    { LordZot.LordZot.Charged += this.stopwatch * 1; }
                    this.outer.SetNextState(zotLeap);

                    return;
                }
            }
            if (this.stopwatch > 1f && this.stopwatch < 1.1f)
            {

             
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 5f;
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);
            }
            if (this.stopwatch > 5f && this.stopwatch < 5.1f)
            {
                new RoR2.BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = 15 * 1f * LordZot.LordZot.Charged,
                    baseForce = 880 + (30f * LordZot.LordZot.Charged),
                    bonusForce = new Vector3(0f, 150f * LordZot.LordZot.Charged, 0f),
                    crit = false,
                    damageType = DamageType.Generic,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    procCoefficient = 0,
                    radius = 13f + LordZot.LordZot.Charged * 0.09f,
                    position = model.corePosition,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    teamIndex = base.teamComponent.teamIndex
                }.Fire();
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 1f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                RoR2.EffectManager.SpawnEffect(original6, effectData, true);

            }
            if (flag266 && this.stopwatch >= 1f && isAuthority)
            {
                
                LordZot.LordZot.timeRemainingstun = 0f;
                BaseLeap zotLeap = new BaseLeap();
                { LordZot.LordZot.Charged += this.stopwatch * 1; }
                this.outer.SetNextState(zotLeap);
            }
           
            if (flag266 && this.stopwatch < 1f && isAuthority)
            {
               
                LordZot.LordZot.jumpcooldown = 0f;

                BaseLeap zotLeap = new BaseLeap();
                { LordZot.LordZot.Charged += this.stopwatch * 1;
                }
                this.outer.SetNextState(zotLeap);

                return;
            }


     
    }

      
    






        private float stopwatch;
        private Animator modelAnimator;
        private float Duration;
        private Transform modelTransform;
        private float updatewatch;

        public float Charged { get; private set; }
    }

    public class BaseLeap : BaseState
    {
        // Token: 0x06003DF2 RID: 15858 RVA: 0x0000C351 File Offset: 0x0000A551



        // Token: 0x06003DF3 RID: 15859 RVA: 0x001021D0 File Offset: 0x001003D0

        public override void OnEnter()
        {
            
            
            base.OnEnter();
            base.characterDirection.moveVector += base.inputBank.moveVector;
            base.characterMotor.moveDirection += base.inputBank.moveVector;
            applyx = false;
            LordZot.LordZot.Chargingjump = false;
            LordZot.LordZot.floatpower += Charged;
            LordZot.LordZot.flightcooldown = 0.1f;
            LordZot.LordZot.timeRemainingstun = 0f;
            
            this.stopwatch = 0f;
            this.counte = 0f;

            this.previousaccel = 700;
            LordZot.LordZot.ticker2 = 2f;

            BaseLeap.previous = 3f;

            Vector3 direction = base.GetAimRay().direction;
            if (base.isAuthority)
            {




                direction.y = Mathf.Max(direction.y, BaseLeap.minimumY);
                Vector3 b = Vector3.up * LordZot.LordZot.Charged * 3f;
                Vector3 b2 = Vector3.up * LordZot.LordZot.Charged * 4.5f;

                Vector3 b3 = new Vector3(base.characterDirection.moveVector.x, 0f, base.characterDirection.moveVector.z) * (Charged * 10);

                base.characterMotor.Motor.ForceUnground();

                base.characterMotor.velocity +=  b + b2 + (Vector3.up * 22f);
                
            }
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            RoR2.Util.PlaySound(BaseLeap.leapSoundString, base.gameObject);

            if (!slammingair)
            {
                PlayAnimation("Body", "TitanJump", "forwardSpeed", 1f);
            }
            RoR2.Util.PlaySound(BaseLeap.soundLoopStartEvent, base.gameObject);
            Vector3 footPosition = base.characterBody.footPosition;
            new RoR2.BlastAttack
            {
                attacker = base.gameObject,
                baseDamage = 40 * 0.08f * LordZot.LordZot.Charged,
                baseForce = 0,
                bonusForce = new Vector3(0f, 150f * LordZot.LordZot.Charged, 0f),
                crit = this.isCritAuthority,
                damageType = DamageType.Generic,
                falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                procCoefficient = 1,
                radius = 13f + LordZot.LordZot.Charged * 0.09f,
                position = footPosition,
                attackerFiltering = AttackerFiltering.NeverHit,
                teamIndex = base.teamComponent.teamIndex
            }.Fire();
            var position = this.characterBody.footPosition;
            RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

            sonicboomeffectdata.scale = 3f + LordZot.LordZot.Charged;
            sonicboomeffectdata.rotation = Quaternion.LookRotation(motor.Motor.CharacterUp);
            sonicboomeffectdata.origin = position;
            GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(LoomingPresence.blinkPrefab, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);


            GameObject original22 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
            GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
            GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
          //  EffectSettings1 = original22.GetComponent<RoR2.EffectComponent>();
          //  EffectSettings1.applyScale = true;
          //  EffectSettings2 = original2.GetComponent<RoR2.EffectComponent>();
           // EffectSettings2.applyScale = true;

          //  EffectSettings3 = original6.GetComponent<RoR2.EffectComponent>();
          //  EffectSettings3.applyScale = true;
         //  EffectSettings4 = original.GetComponent<RoR2.EffectComponent>();
          //  EffectSettings4.applyScale = true;
            RoR2.EffectData effectData = new RoR2.EffectData();

            effectData.origin = position;
            effectData.scale = 0.5f * LordZot.LordZot.Charged;
            RoR2.EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, effectData, true);

            RoR2.EffectManager.SpawnEffect(original2, effectData, true);
            RoR2.EffectManager.SpawnEffect(original6, effectData, true);
            RoR2.EffectManager.SpawnEffect(original22, effectData, true);

            if (LordZot.LordZot.Charged > 4.5f)
            {
                RoR2.EffectManager.SpawnEffect(EntityStates.TitanMonster.DeathState.initialEffect, effectData, true);
                RoR2.EffectManager.SpawnEffect(original, effectData, true);
            }

        }


        // Token: 0x06003DF6 RID: 15862 RVA: 0x0010242C File Offset: 0x0010062C
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.characterMotor)
            {
                counte += Time.fixedDeltaTime;

                bool utilityleap = inputBank.skill3.down;
                if (utilityleap)
                {
                    LordZot.LordZot.jumpcooldown = 0f;
                    
                    ZotBlink zotblinkleap = new ZotBlink();
                    this.outer.SetState(zotblinkleap);

                };



                if (this.stopwatch >= 0)
                { this.stopwatch -= Time.fixedDeltaTime; };

                if (/*LordZot.LordZot.timeRemainingstun <= 0 &&*/ !LordZot.LordZot.wellhefell)
                {
                
                    bool primaryleap = inputBank.skill1.down;
                    bool secondaryleap = inputBank.skill2.down;
                    bool specialleap = inputBank.skill4.down;
                    if (secondaryleap)
                    {
                        LordZot.LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.LordZot.jumpcooldown = 0f;
                      
                        LordZot.LordZot.ugh = true;
                        ZotBulwark bulwarkleap = new ZotBulwark();
                        this.outer.SetState(bulwarkleap);


                    };

                    if (specialleap && !Busy)
                    {
                        LordZot.LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.LordZot.jumpcooldown = 0f;
                     
                        EldritchSlam eldritchslamleap = new EldritchSlam();
                        this.outer.SetState(eldritchslamleap);



                    };

                    if (!specialleap && slammingair)
                    {
                        base.PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", 0.4f, 0.1f);
                        if(isAuthority)
                        {
                            LordZot.LordZot.timeRemaining = 6f;
                            Animator modelAnimator = base.GetModelAnimator();
                            modelAnimator.SetFloat("ShieldsIn", 0.12f);
                            this.modelTransform = base.GetModelTransform();
                            ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                            LordZot.LordZot.jumpcooldown = 0f;

                            ZotSlam slamleap = new ZotSlam();
                            this.outer.SetState(slamleap);
                        }





                    };

                    if (primaryleap)
                    {
                        LordZot.LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.LordZot.jumpcooldown = 0f;
                     
                        EldritchFury eldritchfuryleap = new EldritchFury();
                        this.outer.SetState(eldritchfuryleap);



                    };
                 
                }
                if (!characterMotor.isGrounded)
                {
                    if (counte > 1f)
                    {
                        if (Charged > 10)
                        {
                            base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            characterMotor.velocity += base.inputBank.moveVector * 0.5f;
                        }
                        else
                        {

                            base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            
                        }

                    }
                    else
                    {
                        if (Charged > 10)
                        {
                            base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            characterMotor.velocity += base.inputBank.moveVector * 3f;
                        }
                        else
                        {

                            base.characterMotor.moveDirection += base.inputBank.moveVector * 1f;
                            characterMotor.velocity += base.inputBank.moveVector * Charged * 0.25f;
                        }
                    };




                };
              /*  if (characterBody.baseAcceleration > 35)

                { base.characterBody.baseAcceleration = base.characterBody.baseAcceleration * 0.97f; };
                if (characterBody.baseAcceleration < 100 && !LordZot.LordZot.flight && !characterMotor.isGrounded)
                { characterMotor.velocity.y -= 1.2f; };*/
                if (LordZot.LordZot.flight && base.isAuthority)
                {
                   
                    LordZot.LordZot.Charged = 0f;
                    this.outer.SetNextStateToMain();
                };

                if (base.fixedAge >= BaseLeap.minimumDuration && ((base.characterMotor.Motor.GroundingStatus.IsStableOnGround && !base.characterMotor.Motor.LastGroundingStatus.IsStableOnGround)))
                {
                   

                    if (!LordZot.LordZot.wellhefell)
                    {
                        if (!Busy)
                        { 
                        PlayAnimation("Body", "AscendDescend 2", null, 1f);
                             };
                        LordZot.LordZot.wellhefell = true;
                        this.stopwatch = 1.6f;
                        base.characterMotor.velocity = Vector3.zero;
                    //    base.characterMotor.Motor.enabled = false;
                    //    characterMotor.enabled = false;
                    };
                    LordZot.LordZot.jumpcooldown = 0f;
                   
                    LordZot.LordZot.Charged = 0f;





                }
                if ((this.stopwatch <= 0 && LordZot.LordZot.wellhefell) | base.inputBank.jump.down && LordZot.LordZot.wellhefell && base.isAuthority)
                {
                    //  Debug.Log("he fell");
                 //   base.characterMotor.Motor.enabled = true;
                    LordZot.LordZot.wellhefell = false;
                    //    LordZot.LordZot.fallstun = false;
                   
                    LordZot.LordZot.jumpcooldown = 0f;
                  
                    base.characterMotor.velocity = Vector3.zero;
            
                    LordZot.LordZot.Charged = 0f;
                    //   characterMotor.enabled = true;
                    if (slammingair)
                    {
                       
                            LordZot.LordZot.Charging = false;

                            ZotSlam zotSlam = new ZotSlam();

                            zotSlam.laserDirection = base.GetAimRay().direction;
                            this.outer.SetNextState(zotSlam);

                            return;
                        
                    }
                    else
                    {
                        this.outer.SetNextStateToMain();
                    }
                };


            }
        }




        public override void OnExit()
        {
            RoR2.Util.PlaySound(BaseLeap.soundLoopStopEvent, base.gameObject);
            if (base.isAuthority)
            {
               
                LordZot.LordZot.wellhefell = false;
                //  LordZot.LordZot.fallstun = false;
            }

            LordZot.LordZot.wellhefell = false;
            //   LordZot.LordZot.fallstun = false;
          

            LordZot.LordZot.jumpcooldown = 0f;
            applyx = false;
            LordZot.LordZot.Charged = 0f;
            base.OnExit();
        }

        // Token: 0x06003DFB RID: 15867 RVA: 0x0000D472 File Offset: 0x0000B672
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        // Token: 0x040038F8 RID: 14584
        public static float minimumDuration = 0.0f;
        private float previousaccel = 700;
        public static float previous = 3f;
        // Token: 0x040038F9 RID: 14585
        public static float blastRadius;

        // Token: 0x040038FA RID: 14586
        public static float blastProcCoefficient;

        // Token: 0x040038FB RID: 14587
        [SerializeField]
        public float blastDamageCoefficient;

        // Token: 0x040038FC RID: 14588
        [SerializeField]
        public float blastForce;

        // Token: 0x040038FD RID: 14589
        public static string leapSoundString;

        // Token: 0x040038FE RID: 14590
        public static GameObject projectilePrefab;

        // Token: 0x040038FF RID: 14591
        [SerializeField]
        public Vector3 blastBonusForce;

        // Token: 0x04003900 RID: 14592
        [SerializeField]
        public GameObject blastImpactEffectPrefab;

        // Token: 0x04003901 RID: 14593
        [SerializeField]
        public GameObject blastEffectPrefab;

        // Token: 0x04003902 RID: 14594
        public static float airControl;

        // Token: 0x04003903 RID: 14595
        public static float aimVelocity;

        // Token: 0x04003904 RID: 14596
        public static float upwardVelocity;

        // Token: 0x04003905 RID: 14597
        public static float forwardVelocity;

        // Token: 0x04003906 RID: 14598
        public static float minimumY;

        // Token: 0x04003907 RID: 14599
        public static float minYVelocityForAnim;

        // Token: 0x04003908 RID: 14600
        public static float maxYVelocityForAnim;

        // Token: 0x04003909 RID: 14601
        public static float knockbackForce;

        // Token: 0x0400390A RID: 14602
        [SerializeField]
        public GameObject fistEffectPrefab;

        // Token: 0x0400390B RID: 14603
        public static string soundLoopStartEvent;

        // Token: 0x0400390C RID: 14604
        public static string soundLoopStopEvent;

        // Token: 0x0400390D RID: 14605
        public static RoR2.NetworkSoundEventDef landingSound;
        private float stopwatch;
        private float counte;

        // Token: 0x0400390E RID: 14606
        private float previousAirControl = 1f;

        // Token: 0x0400390F RID: 14607
        private GameObject leftFistEffectInstance;

        // Token: 0x04003910 RID: 14608
        private GameObject rightFistEffectInstance;

        // Token: 0x04003911 RID: 14609
        protected bool isCritAuthority;


        // Token: 0x04003913 RID: 14611
        private bool detonateNextFrame;
        private Transform modelTransform;
        private RoR2.EffectComponent EffectSettings1;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings3;
        private RoR2.EffectComponent EffectSettings4;
        private Vector3 m2;
        private bool applyx;
    }

    public class ZotLand : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
           
               
            base.PlayAnimation("Body", "AscendDescend2");

        }
        public override void OnExit()
        {
            
                base.OnExit();
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Time.fixedDeltaTime >= 0.9 && base.isAuthority)
            {
                this.outer.SetNextStateToMain();


            }
           


            

        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
    public class EldritchFury : BaseState
        {
            // Token: 0x0600006B RID: 107 RVA: 0x00005D28 File Offset: 0x00003F28


            public override void OnEnter()
            {
                base.OnEnter();
            FuryTapPower = 0f;
            if (LordZot.LordZot.GemPowerVal <= 1f && base.isAuthority)
            {
                LordZot.LordZot.Gemdrain = 0.1f;
                this.outer.SetNextStateToMain();
            }
            else
            {

                characterDirection.turnSpeed = 200;
                bool lastswing = LordZot.LordZot.LastSwingArm is true;
                if (lastswing)
                {
                    this.comboState = this.comboState + 1;
                }

                if (LordZot.LordZot.Gemdrain <= 0)
                { didntstartgemdrained = true; }
                else
                { didntstartgemdrained = false; }
                timeRemaining = 6.0f;
                LordZot.LordZot.Busy = true;
                //  if (LordZot.LordZot.Busy)
                //  { Debug.Log("Made Busy"); }
                this.stopwatch = 0f;
                if (GemPowerVal * 0.02f > 1)
                {
                    FuryTapPower += GemPowerVal * 0.02f;
                    LordZot.LordZot.GemPowerVal -= GemPowerVal * 0.02f;
                   
                }
                else
                { LordZot.LordZot.GemPowerVal -= 1f;
                    FuryTapPower = 1f;
                }
    
                // LordZot.LordZot.ticker2 = 0f;
                this.earlyExitDuration = EldritchFury.baseEarlyExitDuration / this.attackSpeedStat;
                this.animator = LordZot.LordZot.animator;
                this.hasSwung = false;
                this.hasHopped = false;
                this.modelAnimator = LordZot.LordZot.animator;
                this.modelTransform = modeltransform;
                ChildLocator component = LordZot.LordZot.component;



                {

                }
                bool flag = base.skillLocator;
                if (flag)
                {
                    base.skillLocator.primary.skillDef.activationStateMachineName = "Weapon";
                }
                bool @bool = this.animator.GetBool("isMoving");
                bool bool2 = this.animator.GetBool("isGrounded"); ;
                switch (this.comboState)
                {
                    case EldritchFury.ComboState.Punch1:
                        {
                            RightTrail.emitting = true;
                            this.attackDuration = EldritchFury.baseComboAttackDuration * 1f / this.attackSpeedStat;
                            if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                                RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat * 1f);
                            else
                                RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, 3f);
                            bool flag3 = @bool || !bool2;
                            if (flag3)
                            {
                                base.PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", this.attackDuration * 2.2f, 0.2f / this.attackSpeedStat);

                            }
                            else
                            {
                                base.PlayCrossfade("Gesture, Override", "FireArrow", "FireArrow.playbackRate", this.attackDuration * 2.2f, 0.2f / this.attackSpeedStat);
                            }
                            break;
                        }
                    case EldritchFury.ComboState.Punch2:
                        {
                            LeftTrail.emitting = true;
                            this.attackDuration = EldritchFury.baseComboAttackDuration / this.attackSpeedStat * 1f;
                            if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                                RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat * 1f);
                            else
                                RoR2.Util.PlayAttackSpeedSound(ClapState.attackSoundString, base.gameObject, 3f);
                            bool flag4 = @bool || !bool2;
                            if (flag4)
                            {
                                base.PlayCrossfade("Gesture, Additive", "FireArrow2", "FireArrow.playbackRate", this.attackDuration * 1.4f, 0.2f / this.attackSpeedStat);
                            }
                            else
                            {
                                base.PlayCrossfade("Gesture, Override", "FireArrow2", "FireArrow.playbackRate", this.attackDuration * 1f, 0.2f / this.attackSpeedStat);
                            }
                            this.hitEffectPrefab = LoaderMeleeAttack.overchargeImpactEffectPrefab;
                            break;
                        }
                }
                //    base.characterBody.SetAimTimer(this.attackDuration + 1f);
                this.attackDuration = EldritchFury.baseComboAttackDuration / this.attackSpeedStat;
            }
  

        }

            // Token: 0x0600006E RID: 110 RVA: 0x00006314 File Offset: 0x00004514
            public override void OnExit()
            {
            LordZot.LordZot.Busy = false;
            characterDirection.turnSpeed = 150;
            LeftTrail.emitting = false;
            RightTrail.emitting = false;
            base.OnExit();
            }

            // Token: 0x0600006F RID: 111 RVA: 0x00006358 File Offset: 0x00004558
            /// <summary>
            /// 
            /// </summary>
            public override void FixedUpdate()
            {
                base.FixedUpdate();

         

                this.stopwatch += Time.fixedDeltaTime;
            

            if (didntstartgemdrained)
            {
                base.characterMotor.velocity.y -= base.characterMotor.velocity.y * 0.02f;
               
                bool isAuthority = base.isAuthority;
                if (isAuthority)
                {
                    { 
                    switch (this.comboState)
                    {
                        case EldritchFury.ComboState.Punch1:
                            {
                                LordZot.LordZot.LastSwingArm = true;
                                bool flageffect1 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
                                if (flageffect1)
                                {
                          
                                    ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                                    if (component)
                                    {


                                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                        GameObject original3 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                                        GameObject original35 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                        GameObject original4 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                                        Transform transform2 = component.FindChild("RightHand");

                                        if (transform2)
                                        {
                                            /*  Color32 color = new Color32();
                                              color.a = 255;
                                              color.r = 255;
                                              color.g = 0;
                                              color.b = 0;*/

                                            var position2 = transform2.position;
                                            RoR2.EffectData effectData = new RoR2.EffectData();
                                            effectData.origin = position2;
                                            effectData.scale = 18f + (FuryTapPower * 0.3f);
                                            //   effectData.color = color;

                                            RoR2.EffectData effectData4 = new RoR2.EffectData();
                                            effectData4.origin = position2;
                                            effectData4.scale = 4f + (FuryTapPower * 0.6f);
                                            //    effectData4.color = color;
                                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                                            effectData2.origin = position2;
                                            effectData2.scale = 7f + (FuryTapPower * 0.3f);
                                            //     effectData2.color = color;
                                            RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                                            RoR2.EffectManager.SpawnEffect(original3, effectData4, true);

                                            RoR2.EffectManager.SpawnEffect(original35, effectData2, true);
                                            var position3 = base.characterBody.footPosition;
                                            RoR2.EffectData effectData3 = new RoR2.EffectData();
                                            effectData3.origin = position3;
                                            effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                            RoR2.EffectManager.SpawnEffect(original4, effectData3, true);


                                        }
                                    };
                                };

                                break;
                            }
                        case EldritchFury.ComboState.Punch2:
                            {
                                LordZot.LordZot.LastSwingArm = false;
                                bool flageffect1 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
                                if (flageffect1)
                                {
                                 
                                    ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                                    if (component)
                                    {

                                        GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                        GameObject original4 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                                        Transform transform = component.FindChild("LeftHand");

                                        if (transform)
                                        {
                                            var position = transform.position;
                                            RoR2.EffectData effectData = new RoR2.EffectData();
                                            effectData.origin = position;
                                            effectData.scale = 6f + (FuryTapPower * 0.3f);
                                            RoR2.EffectManager.SpawnEffect(original, effectData, true);

                                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                                            effectData2.origin = position;
                                            effectData2.scale = 18f + (FuryTapPower * 0.3f);

                                            RoR2.EffectManager.SpawnEffect(original2, effectData2, true);
                                            var position2 = base.characterBody.footPosition;
                                            RoR2.EffectData effectData3 = new RoR2.EffectData();
                                            effectData3.origin = position2;
                                            effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                            RoR2.EffectManager.SpawnEffect(original4, effectData3, true);


                                        }
                                    };
                                };
                                break;
                            }
                    }


                }
                
                  
                   
                
               

               
                bool flag23 = base.isAuthority && this.stopwatch >= this.attackDuration * 0.55f;
                if (flag23)
                {
                    bool flag24 = !this.hasSwung;
                        if (flag24)
                        {



                            //RoR2.EffectData effectData = new RoR2.EffectData();
                            var position = base.characterBody.corePosition + (base.characterDirection.forward * 6f); //Two units in front of player
                                                                                                                     //  effectData.origin = position;
                                                                                                                     //  effectData.scale = 11f;
                                                                                                                     //   RoR2.EffectManager.SpawnEffect(this.explodePrefab, effectData, false);
                            RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                            LandingBlast.attacker = characterBody.gameObject;
                            LandingBlast.inflictor = characterBody.gameObject;
                            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                            LandingBlast.baseDamage = 40 * 8f * FuryTapPower;
                            LandingBlast.baseForce = 3500f + FuryTapPower * 15;
                            LandingBlast.position = position;
                            LandingBlast.radius = 21f + FuryTapPower * 0.3f;
                            LandingBlast.crit = characterBody.RollCrit();
                            LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                            LandingBlast.Fire();
                            this.shakeEmitter = transform.gameObject.AddComponent<RoR2.ShakeEmitter>();
                            this.shakeEmitter.wave = new Wave
                            {
                                amplitude = 0.4f,
                                frequency = 180f,
                                cycleOffset = 0f
                            };
                            this.shakeEmitter.duration = 0.4f;
                            this.shakeEmitter.radius = 50f;
                            this.shakeEmitter.amplitudeTimeDecay = true;
                            GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                            RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();



                            Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-100f, 100f), Vector3.forward);
                            var direc = modelTransform.rotation;
                            sonicboomeffectdata.scale = 3f;
                            sonicboomeffectdata.rotation = direc * randomSpin;
                            sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -2f + modelTransform.up * 0f;
                            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);


                            hasSwung = true;

                        }
                    }
                   
                }

                if (hasSwung && stopwatch <= this.attackDuration * 0.7f)
                {
                    GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                    RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();



                    Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-100f, 100f), Vector3.forward);
                    var direc = modelTransform.rotation;
                    sonicboomeffectdata.scale = 3f;
                    sonicboomeffectdata.rotation = direc * randomSpin;
                    sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -2f + modelTransform.up * 0f;
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                    randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-100f, 100f), Vector3.forward);
                    RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                }

                bool flag26 = base.inputBank.skill1.down && this.comboState != (EldritchFury.ComboState.Punch2) && this.stopwatch >= this.attackDuration * 0.65f;
                if (flag26 && base.isAuthority)
                {

                    if (LordZot.LordZot.Gemdrain <= 0f)
                    {
                        skillLocator.primary.Reset();
                    }
                    this.outer.SetNextStateToMain();
                    LordZot.LordZot.timeRemaining = 6.0f;

                }
                bool flag2555 = base.inputBank.skill1.down && this.comboState != (EldritchFury.ComboState.Punch1) && this.stopwatch >= this.attackDuration * 0.61f;
                if (flag2555 && base.isAuthority)
                {
                    LordZot.LordZot.LastSwingArm = false;
                    if (LordZot.LordZot.Gemdrain <= 0f)
                    {
                        skillLocator.primary.Reset();
                    }

                    this.outer.SetNextStateToMain();
                    LordZot.LordZot.timeRemaining = 6.0f;

                }
                bool flag27 = this.stopwatch >= this.attackDuration;
                if (flag27 && base.isAuthority)
                {
                    LordZot.LordZot.Busy = false;

                    this.outer.SetNextStateToMain();

                }
                if (this.stopwatch >= this.attackDuration * 0.3f && modelAnimator.GetFloat("ShieldsIn") < 0.1f)
                { 
                floatpower += 8f;
                GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                EffectSettings7 = original.GetComponent<RoR2.EffectComponent>();
                EffectSettings7.applyScale = true;
                EffectSettings7.soundName.IsNullOrWhiteSpace();
               EffectSettings7.parentToReferencedTransform = true;
               EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                EffectSettings.applyScale = true;
                EffectSettings.soundName.IsNullOrWhiteSpace();
                EffectSettings.parentToReferencedTransform = true;
                RoR2.EffectData effectData = new RoR2.EffectData();
                effectData.origin = lefthand.position;
                effectData.scale = 1f;
                RoR2.EffectManager.SpawnEffect(original, effectData, true);

                RoR2.EffectData effectData2 = new RoR2.EffectData();
                effectData.origin = righthand.position;
                effectData.scale = 1f;
                RoR2.EffectManager.SpawnEffect(original2, effectData2, true);



              
                ticker2 = 2f;
                    LordZot.LordZot.timeRemaining = 6.0f;

                    modelAnimator.SetFloat("ShieldsIn", 0.12f);


             
            }
                if (this.stopwatch >= this.attackDuration * 0.3f && !this.hasSwung && base.isAuthority)
                {


                    if (this.comboState == ComboState.Punch1)
                    {
                        RoR2.EffectData effectData22 = new RoR2.EffectData
                        {
                            scale = 0.3f + (FuryTapPower * 0.01f),
                            origin = LordZot.LordZot.righthand.position,
               
                        };

                        GameObject gameObject5 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                         EffectSettings2 = gameObject5.GetComponent<RoR2.EffectComponent>();
                         EffectSettings3 = gameObject5.GetComponent<RoR2.ShakeEmitter>();

                       
                         EffectSettings3.enabled = false;
                         EffectSettings2.applyScale = true;
                         EffectSettings2.parentToReferencedTransform = false;
                        RoR2.EffectManager.SpawnEffect(gameObject5, effectData22, true);
                        

                    }
                    else
                    {

                        RoR2.EffectData effectData3 = new RoR2.EffectData
                        {
                            scale = 0.5f + (FuryTapPower * 0.01f),

                            origin = LordZot.LordZot.lefthand.position
                        };
                    //    leftpunch.GetComponent<RoR2.EffectComponent>().applyScale = true;
                    //    leftpunch.GetComponent<RoR2.ShakeEmitter>().enabled = false;
                        RoR2.EffectManager.SpawnEffect(LordZot.LordZot.leftpunch, effectData3, true);
                    };
                }

                bool inaitrue = LordZot.LordZot.inair is true;
                bool airq = characterMotor.isGrounded;
                if (airq && inaitrue)
                {
                  
                    if (this.stopwatch >= this.attackDuration * 0.1f)
                    {
                        if (isAuthority)
                        {
                            switch (this.comboState)
                            {
                                case EldritchFury.ComboState.Punch1:
                                    {
                                        bool flageffect1 = this.stopwatch >= this.attackDuration * 0.1f && !this.hasSwung;
                                        if (flageffect1)
                                        {
                                            hasSwung = true;
                                            ChildLocator component = LordZot.LordZot.component;


                                            if (component)
                                            {


                                                GameObject original25 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                                GameObject original35 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                                GameObject original45 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                                                Transform transform2 = component.FindChild("RightHand");

                                                if (transform2)
                                                {
                                                /*    Color32 color = new Color32();
                                                    color.a = 255;
                                                    color.r = 255;
                                                    color.g = 0;
                                                    color.b = 0;*/

                                                    var position3 = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                                    effectData.origin = transform2.position;
                                                    effectData.scale = 18f + (FuryTapPower * 0.3f);
                                                  //  effectData.color = color;

                                                    RoR2.EffectData effectData2 = new RoR2.EffectData();
                                                    effectData2.origin = transform2.position;
                                                    effectData2.scale = 3f + (FuryTapPower * 0.3f);
                                                   // effectData2.color = color;
                                                    RoR2.EffectManager.SpawnEffect(original25, effectData, true);
                                                    RoR2.EffectManager.SpawnEffect(original35, effectData2, true);
                                                    var position2 = base.characterBody.footPosition;
                                                    RoR2.EffectData effectData3 = new RoR2.EffectData();
                                                    effectData3.origin = position2;
                                                    effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                                    RoR2.EffectManager.SpawnEffect(original45, effectData3, true);
                                                }
                                            };
                                        };

                                        break;
                                    }
                                case EldritchFury.ComboState.Punch2:
                                    {
                                        bool flageffect1 = this.stopwatch >= this.attackDuration * 0.1f && !this.hasSwung;
                                        if (flageffect1)
                                        {

                                            ChildLocator component = LordZot.LordZot.component;

                                            hasSwung = true;
                                            if (component)
                                            {

                                                GameObject originaly = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                                                GameObject original22 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                                GameObject original44 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                                                Transform transform = component.FindChild("LeftHand");

                                                if (transform)
                                                {
                                                    var position3 = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                                    effectData.origin = transform.position;
                                                    effectData.scale = 6f + (FuryTapPower * 0.3f);
                                                    RoR2.EffectManager.SpawnEffect(originaly, effectData, true);

                                                    RoR2.EffectData effectData2 = new RoR2.EffectData();
                                                    effectData2.origin = transform.position;
                                                    effectData2.scale = 18f + (FuryTapPower * 0.3f);

                                                    RoR2.EffectManager.SpawnEffect(original22, effectData2, true);
                                                    var position2 = base.characterBody.footPosition;
                                                    RoR2.EffectData effectData3 = new RoR2.EffectData();
                                                    effectData3.origin = position2;
                                                    effectData3.scale = 6f + (FuryTapPower * 0.3f);
                                                    RoR2.EffectManager.SpawnEffect(original44, effectData3, true);
                                                }
                                            };
                                        };
                                        break;
                                    }
                            }
                        };
                        var position = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                        RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                        LandingBlast.attacker = characterBody.gameObject;
                        LandingBlast.inflictor = characterBody.gameObject;
                        LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                        LandingBlast.baseDamage = 40 * 8f * FuryTapPower;
                        LandingBlast.baseForce = 3500f + FuryTapPower * 15;
                        LandingBlast.position = position;
                        LandingBlast.radius = 21f + (FuryTapPower * 0.3f);
                        LandingBlast.crit = characterBody.RollCrit();
                        LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                        LandingBlast.Fire();
                        this.shakeEmitter = transform.gameObject.AddComponent<RoR2.ShakeEmitter>();
                        this.shakeEmitter.wave = new Wave
                        {
                            amplitude = 0.4f,
                            frequency = 180f,
                            cycleOffset = 0f
                        };
                        this.shakeEmitter.duration = 0.4f;
                        this.shakeEmitter.radius = 50f;
                        this.shakeEmitter.amplitudeTimeDecay = true;
                    };
                    if (isAuthority)
                    {
                        LordZot.LordZot.inair = false;
                        this.outer.SetNextStateToMain();
                        return;
                    }
                }
                if (!airq && base.isAuthority)
                { LordZot.LordZot.inair = true; };
            }
            bool flag17 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
            if (flag17)
            {
                this.hasSwung = true;
                if (LordZot.LordZot.Gemdrain <= 0f)
                {
                    skillLocator.primary.Reset();
                }
               
            }
        }

       

            // Token: 0x06000070 RID: 112 RVA: 0x000069F0 File Offset: 0x00004BF0
            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.Death;
            }

            // Token: 0x06000071 RID: 113 RVA: 0x00006A03 File Offset: 0x00004C03
            public override void OnSerialize(NetworkWriter writer)
            {
                base.OnSerialize(writer);
                writer.Write((byte)this.comboState);
            }

            // Token: 0x06000072 RID: 114 RVA: 0x00006A1C File Offset: 0x00004C1C
            public override void OnDeserialize(NetworkReader reader)
            {
                base.OnDeserialize(reader);
                this.comboState = (EldritchFury.ComboState)reader.ReadByte();
            }


            // Token: 0x04000093 RID: 147
            public static float comboDamageCoefficient = 6f;

            // Token: 0x04000096 RID: 150
            public static float hitHopVelocity = 5f;

        public static float hitPauseDuration = 0.15f;
        public static float forceMagnitude = 16000f;
        public static float attackspeedaltsoundthreshold = 3f;
     
        // Token: 0x04000099 RID: 153
        private float stopwatch;

        // Token: 0x0400009A RID: 154
        private float attackDuration = 1f;

            // Token: 0x0400009B RID: 155
            private float earlyExitDuration;

            // Token: 0x0400009C RID: 156
            private Animator animator;
        public static float recoilAmplitude = 7f;
            // Token: 0x0400009D RID: 157
            private RoR2.OverlapAttack overlapAttack;

            // Token: 0x0400009E RID: 158
            private float hitPauseTimer;
        public GameObject explodePrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
        // Token: 0x0400009F RID: 159
        private bool isInHitPause;
        private RoR2.ShakeEmitter shakeEmitter;
        // Token: 0x040000A0 RID: 160
        private bool hasSwung;
        private RoR2.Tracer tracer;
            // Token: 0x040000A1 RID: 161
            private bool hasHit;
        private RoR2.EffectData rightponch;
            // Token: 0x040000A2 RID: 162
            private bool hasHopped;
        private Animator modelAnimator;
        private Transform modelTransform;

        // Token: 0x040000A3 RID: 163
        public EldritchFury.ComboState comboState;
        public static GameObject tracerEffectPrefab = GolemMonster.FireLaser.tracerEffectPrefab;
        // Token: 0x040000A5 RID: 165
        private BaseState.HitStopCachedState hitStopCachedState;

            // Token: 0x040000A6 RID: 166
            private GameObject swingEffectPrefab;

            // Token: 0x040000A7 RID: 167
            private GameObject hitEffectPrefab;

            // Token: 0x040000A8 RID: 168
            private string attackSoundString;


            // Token: 0x02000025 RID: 37
            public enum ComboState
            {
                // Token: 0x040001C1 RID: 449
                Punch1,
                // Token: 0x040001C2 RID: 450
                Punch2,
            }

            // Token: 0x02000026 RID: 38
            private struct ComboStateInfo
            {
                // Token: 0x040001C4 RID: 452
                private string mecanimStateName;

                // Token: 0x040001C5 RID: 453
                private string mecanimPlaybackRateName;
            }
        		public static float baseComboAttackDuration = 1f;

		// Token: 0x0400317B RID: 12667
		public static float baseEarlyExitDuration = 0.1f;
        private RoR2.Tracer trac;
        private TracerBehavior that;
        private bool didntstartgemdrained;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.ShakeEmitter EffectSettings3;
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings7;

        public float FuryTapPower { get; private set; }
    }


    public class EldritchSlam : BaseState
    {
        // Token: 0x06003C55 RID: 15445 RVA: 0x000FB0B8 File Offset: 0x000F92B8
        public override void OnEnter()
        {
            base.OnEnter();
            {
               
                RightTrail.emitting = true;

                LordZot.LordZot.ChargedSlam = 0f;

                if (GemPowerVal > 3f)
                {
                    LordZot.LordZot.GemPowerVal -= 3f;
                    ChargedSlam += 3f;
                }
                else
                {
                    ChargedSlam += GemPowerVal;
                    LordZot.LordZot.GemPowerVal -= 3f;

                };

                base.PlayCrossfade("Gesture, Additive", "SlamCharge", "FireArrow.playbackRate", 2.2f, 0.2f);
                 
                    LordZot.LordZot.Busy = true;
                    LordZot.LordZot.holdtime = false;
                    LordZot.LordZot.dontmove = false;
                    base.GetModelAnimator();
                    Transform modelTransform = base.GetModelTransform();
                    Vector3 prevcampos()
                    { return base.cameraTargetParams.cameraParams.standardLocalCameraPos; }
                    if (base.isAuthority)
                    {
                        originalcampos = prevcampos();

                    }
                    if (!characterMotor.isGrounded && LordZot.LordZot.ugh)
                    {
                        PlayAnimation("Body", "AscendDescend", null, 2);
                    };
                    base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
                    Ray backhand = GetAimRay();
                    GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                    RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();



                    this.stopwatch = 0.7f;
                    this.stopwatch1 = 0f;
                    this.stopwatch21 = 0f;
                    this.stupidtime2 = 0f;

                    stupidtime = 0f;
                    bool flag = base.skillLocator;
                    if (flag)
                    {
                        base.skillLocator.secondary.skillDef.activationStateMachineName = "Weapon";
                    }

                    this.duration = EldritchSlam.baseDuration;

                    if (isAuthority)
                    {
                        if (modelTransform)
                        {
                            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                            GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");

                            ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                            if (component)
                            {
                                Transform transform = component.FindChild("RightHand");
                                Transform transform2 = component.FindChild("LeftHand");
                                if (transform)
                                {

                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                    effectData.origin = transform.position;
                                    effectData.scale = 1f;
                                    RoR2.EffectManager.SpawnEffect(original, effectData, true);
                                }

                                if (transform2)
                                {
                                    RoR2.EffectData effectData = new RoR2.EffectData();
                                    effectData.origin = transform2.position;
                                    effectData.scale = 1f;
                                    RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                                }


                                GameObject laser = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                            }
                        }
                    }


                    Animator animator = GetModelAnimator();
                    

                    if (GetModelAnimator().GetFloat("ShieldsIn") < 0.1f)
                    {
                        floatpower += 8f;
                        GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = lefthand.position;
                        effectData.scale = 1f;
                        RoR2.EffectManager.SpawnEffect(original, effectData, true);

                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                        effectData.origin = righthand.position;
                        effectData.scale = 1f;
                        RoR2.EffectManager.SpawnEffect(original2, effectData, true);




                        ticker2 = 2f;


                        animator.SetFloat("ShieldsIn", 0.12f);
                    }
                    timeRemaining = 6.0f;
                
            }
        }

        // Token: 0x06003C56 RID: 15446 RVA: 0x000FB208 File Offset: 0x000F9408
        public override void OnExit()
        {
          
            RightTrail.emitting = false;
            LordZot.LordZot.Charging = false;
            animator.SetFloat("FireArrow.playbackRate", 1f);
            
            LordZot.LordZot.Busy = false;
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            base.OnExit();

        }

        // Token: 0x06003C57 RID: 15447 RVA: 0x000FB258 File Offset: 0x000F9458
        public override void Update()
        {
            base.Update();

            if (this.stopwatch21 > 1.4f)
            {
                animator = GetModelAnimator();
                animator.SetFloat("FireArrow.playbackRate", 0.8f);
            }
            if (this.stopwatch21 > 2f)
            {
                animator = GetModelAnimator();
                animator.SetFloat("FireArrow.playbackRate", 0.4f);
            }
            if (this.stopwatch21 > 2.2f)
            {
                animator = GetModelAnimator();
                animator.SetFloat("FireArrow.playbackRate", 0f);


            /*    if (this.stopwatch21 > 5f)
                {
                    Transform modelTransform = base.GetModelTransform();
                    GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                    ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                    Transform transform2 = component.FindChild("RightHand");

                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = transform2.position;
                    effectData.scale = 0.2f + (LordZot.LordZot.ChargedSlam * 0.01f);
                    RoR2.EffectManager.SpawnEffect(original, effectData, true);

                }
*/


            }

                
            
        }
        // Token: 0x06003C58 RID: 15448 RVA: 0x000FB3B4 File Offset: 0x000F95B4
        public override void FixedUpdate()
        {

      
                base.FixedUpdate();
                this.stopwatch21 += Time.fixedDeltaTime;
                bool outofpower = LordZot.LordZot.GemPowerVal <= 0;
                this.updatewatch += Time.fixedDeltaTime;
                bool flag26 = base.inputBank.skill4.down;
                bool flag266 = base.inputBank.skill4.justReleased;
                bool flag27 = this.stopwatch1 >= this.duration;
                bool flag29 = this.updatewatch >= 0.1f;
                stupidtime2 -= Time.fixedDeltaTime;
            if (this.stopwatch21 > 0.8f)
            {
                LordZot.LordZot.Charging = true;

            };
            if (this.stopwatch21 > 3f)
                            {

                                RoR2.EffectData effectData22 = new RoR2.EffectData
                                {
                                    scale = 0.5f,
                                    origin = LordZot.LordZot.righthand.position,
                                  //  start = LordZot.LordZot.righthand.position + LordZot.LordZot.righthand.up * 0.2f
                                };
                                RoR2.EffectManager.SpawnEffect(effectPrefab, effectData22, false);

                            }
                if (LordZot.LordZot.Charged > 5f && flag29)
                {

                    GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                    RoR2.EffectData effectData = new RoR2.EffectData();

                    effectData.origin = characterBody.footPosition;
                    effectData.scale = 0.9f * this.stopwatch;
                    RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                    RoR2.EffectManager.SpawnEffect(original6, effectData, true);

                }
        
            if (LordZot.LordZot.Charged > 10f && flag29)
                {

                    GameObject original7 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                    GameObject original6 = Resources.Load<GameObject>("prefabs/effects/TitanSpawnEffect");
                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                    RoR2.EffectData effectData = new RoR2.EffectData();

                    effectData.origin = characterBody.footPosition;
                    effectData.scale = 0.15f * this.stopwatch;
                    RoR2.EffectManager.SpawnEffect(original7, effectData, true);
                    RoR2.EffectManager.SpawnEffect(original2, effectData, true);
                    RoR2.EffectManager.SpawnEffect(original6, effectData, true);

                }

                if (motor.isGrounded && model.inputBank.jump.down && !Chargingjump)
                {

                    Chargingjump = true;
                    jumpcooldown = 5f;

                }
                if (Chargingjump && !model.inputBank.jump.down)
                {

                    Chargingjump = false;
                    jumpcooldown = 0f;
                    PlayAnimation("Body", "TitanJump", "forwardSpeed", 1f);
               //    animator.SetFloat("FireArrow.playbackRate", 0f);
                    EntityStates.ZotStates.BaseLeap zotJump = new EntityStates.ZotStates.BaseLeap();
                    slammingair = true;
                    this.outer.SetState(zotJump);
                    return;

                }
            /*            if (!motor.isGrounded)
                        { if (Charged > 5)
                            { motor.velocity.y -= motor.velocity.y * 0.0005f; }

                            else
                            { motor.velocity -= motor.velocity * 0.0035f;} ;
                        }*/
            if (this.stopwatch21 > 0.2f)
            {

                LordZot.LordZot.Charging = true;

            };
            if (this.stopwatch21 > 1.6f)
            {

           
                GameObject laser2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                EffectSettings2 = laser2.GetComponent<RoR2.EffectComponent>();
                EffectSettings2.applyScale = true;
                EffectSettings2.parentToReferencedTransform = true;
                RoR2.EffectData effectData = new RoR2.EffectData();
                effectData.origin = LordZot.LordZot.righthand.position;
                effectData.scale = 0.3f;
                RoR2.EffectManager.SpawnEffect(laser2, effectData, true);
            };

            if (flag29 && flag26)
                {


                   

                    if (LordZot.LordZot.ChargedSlam > 15f)
                    {


                        RoR2.ShakeEmitter shakeEmitter;
                        shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                        shakeEmitter.wave = new Wave
                        {
                            amplitude = 0.1f,
                            frequency = 180f,
                            cycleOffset = 0f
                        };
                        shakeEmitter.duration = 1f;
                        shakeEmitter.radius = 50f;
                        shakeEmitter.amplitudeTimeDecay = false;
                        LordZot.LordZot.timeRemaining = 6f;
                        this.updatewatch = 0f;
                    }
                    else
                    {
                        RoR2.ShakeEmitter shakeEmitter;
                        shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                        shakeEmitter.wave = new Wave
                        {
                            amplitude = 0.008f * this.stopwatch1,
                            frequency = 180f,
                            cycleOffset = 0f
                        };
                        shakeEmitter.duration = 1f;
                        shakeEmitter.radius = 50f;
                        shakeEmitter.amplitudeTimeDecay = false;
                        LordZot.LordZot.timeRemaining = 6f;
                        
                    };
                    if (LordZot.LordZot.ChargedSlam > 40f)
                    {
                        GameObject laser2 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalPrespawnEffect");
                        GameObject laser3 = Resources.Load<GameObject>("prefabs/effects/TitanSpawnEffect");

                        if (effectPrefab)
                        {
                            this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(laser2, LordZot.LordZot.righthand.position, LordZot.LordZot.righthand.rotation);
                            this.chargeEffect.transform.parent = transform;
                            RoR2.ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<RoR2.ScaleParticleSystemDuration>();
                            if (component2)
                            {
                                component2.newDuration = 1f;
                            }
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = transform.position;
                            effectData.scale = 0.5f;
                            RoR2.EffectManager.SpawnEffect(chargeEffect, effectData, true);
                            RoR2.EffectData effectData2 = new RoR2.EffectData();
                            effectData.origin = characterBody.footPosition;
                            effectData.scale = 1f + (0.2f * LordZot.LordZot.ChargedLaser);
                            RoR2.EffectManager.SpawnEffect(laser3, effectData2, true);
                        }


                        Transform modelTransform = base.GetModelTransform();
                        ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                        Transform transform2 = component.FindChild("RightHand");
                        if (transform2)
                        {
                         //   EffectSettings3 = laser2.GetComponent<RoR2.EffectComponent>();
                          //  EffectSettings3.applyScale = true;
                          //  EffectSettings3.parentToReferencedTransform = true;
                            laser2.AddComponent<RoR2.DestroyOnTimer>().duration = 1.5f;

                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = LordZot.LordZot.righthand.position;
                            effectData.scale = 1f + (1f * LordZot.LordZot.ChargedSlam);
                            RoR2.EffectManager.SpawnEffect(laser2, effectData, true);

                        }

                    };


                    this.updatewatch = 0f;

                };
               
                if (outofpower && flag26 && base.isAuthority)
            {
              //  base.PlayCrossfade("Gesture, Additive", "BufferEmpty", "FireArrow.playbackRate", 0.1f, 0f);
                LordZot.LordZot.Gemdrain = 1f;
                    skillLocator.special.DeductStock(1);
                    LordZot.LordZot.Charging = false;
                    ZotSlam zotSlam = new ZotSlam();
                    animator.SetFloat("FireArrow.playbackRate", 1f);
                    zotSlam.laserDirection = laserDirection;
                    this.outer.SetNextState(zotSlam);


                    return;
                };
            if (!flag26 && this.stopwatch21 > 0.2f)
            {
             //   base.PlayCrossfade("Gesture, Additive", "BufferEmpty", "FireArrow.playbackRate", 0.1f, 0f);
                animator.SetFloat("FireArrow.playbackRate", 1f);
                    if (base.isAuthority)
                    {
                        LordZot.LordZot.Charging = false;

                        ZotSlam zotSlam = new ZotSlam();

                        zotSlam.laserDirection = laserDirection;
                        this.outer.SetNextState(zotSlam);

                        return;
                    }
                };
                if (this.stopwatch21 > 1.5f && this.stopwatch21 < 1.6f)
            { 


                    GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                    RoR2.EffectData effectData = new RoR2.EffectData();

                    effectData.origin = characterBody.footPosition;
                    effectData.scale = 5f;
                    RoR2.EffectManager.SpawnEffect(original6, effectData, true);
                };




                if (stupidtime >= 0)
                {
                    stupidtime -= Time.fixedDeltaTime;
                };
                if (stupidtime <= 0)
                {
                    LordZot.LordZot.holdtime = false;
                };

                if (this.stopwatch21 >= 0.2f && !LordZot.LordZot.holdtime)
                {

                    LordZot.LordZot.holdtime = true;
                    stupidtime = 10f;
                    /*  {
                          flicker.type = LightType.Point;
                          flicker.enabled = true;
                          flicker.intensity = 15f;
                          flicker.color = new UnityEngine.Color(255f, 0f, 0f);
                          flicker.range = 4f;
                      }*/
                };

                base.characterDirection.moveVector += base.inputBank.aimDirection;


                if (!flag26 && this.stopwatch21 > 0.2f)
            {
                // base.PlayCrossfade("Gesture, Additive", "BufferEmpty", "FireArrow.playbackRate", 0.1f, 0f);
                animator.SetFloat("FireArrow.playbackRate", 1f);
                    if (base.isAuthority)
                    {
                        EntityStates.ZotStates.ZotSlam zotSlam = new EntityStates.ZotStates.ZotSlam();
                    zotSlam.laserDirection = laserDirection;
                    this.outer.SetState(zotSlam);
                        return;
                    }
                };
            }
        

        // Token: 0x06003C59 RID: 15449 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        // Token: 0x0400370E RID: 14094
        public static float baseDuration = 5f;

        // Token: 0x0400370F RID: 14095


        // Token: 0x04003710 RID: 14096
        public static GameObject effectPrefab = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);

        // Token: 0x04003711 RID: 14097
        public static GameObject laserPrefab = (EntityStates.GolemMonster.ChargeLaser.laserPrefab);
        private Vector3 originalcampos;
        // Token: 0x04003712 RID: 14098
        public static string attackSoundString;
        private float stopwatch;

        // Token: 0x04003713 RID: 14099
        private float duration;

        // Token: 0x04003714 RID: 14100
        private uint chargePlayID;

        // Token: 0x04003715 RID: 14101
        private GameObject chargeEffect;

        // Token: 0x04003716 RID: 14102
        private GameObject laserEffect;

        // Token: 0x04003717 RID: 14103
        private LineRenderer laserLineComponent;

        // Token: 0x04003718 RID: 14104
        private Vector3 laserDirection;

        // Token: 0x04003719 RID: 14105
        private Vector3 visualEndPosition;

        // Token: 0x0400371A RID: 14106
        private float flashTimer;

        // Token: 0x0400371B RID: 14107
        private bool laserOn;
        private float updatewatch;
        private float stopwatch1;
        private float stupidtime;
        private float stopwatch21;
        private Animator modelanimator;
        private float stupidtime2;
        private Animator animator;
        private RoR2.EffectComponent EffectSettings3;
        private RoR2.EffectComponent EffectSettings2;
        private bool didntstartgemdrained;

        //  private Light flicker;

        public float ChargeDuration { get; private set; }
    }
    public class ZotSlam : BaseState
    {
        // Token: 0x06003C64 RID: 15460 RVA: 0x000FB714 File Offset: 0x000F9914
        public override void OnEnter()
        {
            base.OnEnter();

            RightTrail.emitting = true;
            facingdown = false;
            hasswung = false;
            stopwatch21 = 0f;
            LordZot.LordZot.Busy = true;
            LordZot.LordZot.floatpower += ChargedSlam;
            this.modifiedAimRay = base.GetAimRay();
            string aimraydirection = modifiedAimRay.direction.y.ToString();
            this.modifiedAimRay.direction = this.laserDirection;
            LordZot.LordZot.Charging = false;
            slammingair = false;
            // Debug.Log("aimray direction y is" + aimraydirection);
            RoR2.Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);

            //  PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", 1f, 0.02f);
            base.GetModelAnimator().SetBool("GodStop", true);
            base.GetModelAnimator().SetLayerWeight(4, 1);
            if (base.GetAimRay().direction.y < -0.7f)
            {
                facingdown = true;
            };
            if (base.GetAimRay().direction.y > -0.7)
            {
                facingdown = false;
            };
        
                base.PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", 1, 0.02f);
            

            string facedownornot = facingdown.ToString();
         //   Debug.Log("facing down is " + facedownornot);
        }

        // Token: 0x06003C65 RID: 15461 RVA: 0x00032FA7 File Offset: 0x000311A7
        public override void OnExit()
        {
            base.OnExit();
      
            RightTrail.emitting = false;
            LordZot.LordZot.ChargedSlam = 0f;
            LordZot.LordZot.Busy = false;
        }

        // Token: 0x06003C66 RID: 15462 RVA: 0x000FB969 File Offset: 0x000F9B69
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            string facedownornot = facingdown.ToString();
          //  Debug.Log("facing down is " + facedownornot);
            if (base.GetAimRay().direction.y < -0.7f) 
            {
                facingdown = true;

            };

      
            if (!facingdown | hasswung)
            { this.stopwatch21 += Time.fixedDeltaTime; }
             if (facingdown & !motor.isGrounded)
            {
                GetModelAnimator().SetFloat("FireArrow.playbackRate", 0.03f);
                motor.velocity.y -= 1; };

            if (stopwatch21 < 0.31f && !hasswung)
            {
                GameObject laser2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
               /* EffectSettings2 = laser2.GetComponent<RoR2.EffectComponent>();
                EffectSettings2.applyScale = true;
                EffectSettings2.parentToReferencedTransform = true;*/
                RoR2.EffectData effectData = new RoR2.EffectData();
                effectData.origin = LordZot.LordZot.righthand.position;
                effectData.scale = 1f;
                RoR2.EffectManager.SpawnEffect(laser2, effectData, true);
            }


                if (stopwatch21 >= 0.31f && !hasswung && !facingdown)
            {
             
                hasswung = true;
                RoR2.Util.PlayAttackSpeedSound(EntityStates.VagrantMonster.FireTrackingBomb.fireBombSoundString, base.gameObject, 0.87f);
                Transform modelTransform = base.GetModelTransform();
                RoR2.Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);
                string text = "RightHand";
                if (base.characterBody)
                {
                    //  base.characterBody.SetAimTimer(1f);
                }
                if (effectPrefab)
                {
                    RoR2.EffectManager.SimpleMuzzleFlash(EntityStates.GolemMonster.FireLaser.effectPrefab, base.gameObject, text, true);
                }
                if (base.isAuthority)
                {
                    new RoR2.BlastAttack
                    {
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                        baseDamage = 40 * 3.5f + (40 * 3.8f * LordZot.LordZot.ChargedSlam),
                        baseForce = 11000f,
                        position = characterBody.corePosition + characterDirection.forward * 4f + modelTransform.up * 2f,
                        radius = 9f + 0.4f * LordZot.LordZot.ChargedSlam,
                        falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                        crit = characterBody.RollCrit(),
                        bonusForce = this.modifiedAimRay.direction + characterBody.characterDirection.forward * (6000 + 3555f * LordZot.LordZot.ChargedSlam)
                    }.Fire();
                    Vector3 origin = this.modifiedAimRay.origin;
                    if (modelTransform)
                    {
                        ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                        if (component)
                        {
                            int childIndex = component.FindChildIndex(text);
                            if (tracerEffectPrefab)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData
                                {
                                    scale = 1f + (2.9f * LordZot.LordZot.ChargedSlam),
                                    origin = characterBody.corePosition + characterDirection.forward * 4f,
                                    start = this.modifiedAimRay.origin
                                };
                                effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                                GameObject laser3 = Resources.Load<GameObject>("prefabs/effects/VagrantNovaExplosion");
                                GameObject laser2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                GameObject original27 = Resources.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                                GameObject original277 = Resources.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");
                                GameObject original2737 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                                GameObject original2777 = Resources.Load<GameObject>("prefabs/effects/BrotherSunderWaveEnergizedExplosion");
                                GameObject original66 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                GameObject original26 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                                GameObject original25 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");



                                /*   EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings.applyScale = true;
                                   EffectSettings2 = original27.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings2.applyScale = true;
                                   EffectSettings4 = original277.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings4.applyScale = true;
                                   EffectSettings5 = original2737.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings5.applyScale = true;
                                   EffectSettings3 = original2777.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings3.applyScale = true;
   */
                                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                                var direc = Quaternion.LookRotation(this.modifiedAimRay.direction);
                                GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

                                sonicboomeffectdata.scale = 3f;
                                sonicboomeffectdata.rotation = direc * randomSpin;
                                sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * 2f + modelTransform.up * 2f;


                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectData blast = new RoR2.EffectData();
                                blast.scale = 5f + (0.5f * LordZot.LordZot.ChargedSlam);
                                blast.rotation = direc;
                                blast.origin = characterBody.corePosition + characterDirection.forward * 5f + modelTransform.up * 4f;
                                RoR2.EffectData blast3 = new RoR2.EffectData();
                                blast3.scale = 10f + (0.5f * LordZot.LordZot.ChargedSlam);
                                blast3.rotation = direc;
                                blast3.origin = characterBody.footPosition;
                                RoR2.EffectData blast2 = new RoR2.EffectData();

                                ChildLocator component2 = modelTransform.GetComponent<ChildLocator>();
                                Transform transform = component2.FindChild("RightHand");

                                blast2.scale = 5 + (0.3f * LordZot.LordZot.ChargedSlam);
                                blast2.origin = characterBody.corePosition + characterDirection.forward * 5f + modelTransform.up * 4f;
                                blast2.rotation = direc;
                                
                                RoR2.EffectManager.SpawnEffect(laser2, blast, true);
                                RoR2.EffectManager.SpawnEffect(original66, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast2, true);
                                RoR2.EffectManager.SpawnEffect(original27, blast, true);
                                RoR2.EffectManager.SpawnEffect(original25, blast3, true);
                                RoR2.EffectManager.SpawnEffect(original26, blast3, true);
                                if (LordZot.LordZot.ChargedSlam > 4)
                                {
                                    RoR2.EffectManager.SpawnEffect(ZotSlam.tracerEffectPrefab, effectData, true);
                                    RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, effectData, true);

                                };


                                if (LordZot.LordZot.ChargedSlam > 40)
                                {
                                    RoR2.Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
                                    RoR2.EffectManager.SpawnEffect(laser3, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original277, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                                };


                                if (LordZot.LordZot.ChargedSlam < 150)
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 1f + 0.003f * LordZot.LordZot.ChargedSlam,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 1f + 0.009f * LordZot.LordZot.ChargedSlam;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.LordZot.timeRemaining = 6f;
                                }
                                else
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 4f,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 3.5f;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.LordZot.timeRemaining = 6f;
                                }

                            }
                        }
                    }
                }
            }
                    if (!hasswung && facingdown)
                    {
                      
                       
            

                      
                        if (!isGrounded)
                        {
                            motor.velocity += GetAimRay().direction * 15;

                            GetModelAnimator().SetFloat("FireArrow.playbackRate", 0f);
                            PlayAnimation("Body", "AscendDescend", null, 1f); ;

                        };

                    if (isGrounded)
                    {
                    base.PlayCrossfade("Gesture, Additive", "BufferEmpty", "FireArrow.playbackRate", 1, 0.02f);
                    GetModelAnimator().SetFloat("FireArrow.playbackRate", 1f);
                        PlayAnimation("Body", "AscendDescend 2", null, 1f);
                        hasswung = true;
                        Transform transform1 = base.GetModelTransform();

                        RoR2.Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);
                        const string V = "RightHand";
                    RoR2.Util.PlayAttackSpeedSound(EntityStates.VagrantMonster.FireTrackingBomb.fireBombSoundString, base.gameObject, 0.87f);
                    if (transform1)
                    {
                        ChildLocator component = transform1.GetComponent<ChildLocator>();
                        if (component)
                        {
                            int childIndex = component.FindChildIndex(V);
                            if (tracerEffectPrefab)
                            {
                                RoR2.EffectData effectData = new RoR2.EffectData
                                {
                                    scale = 1f + (2.9f * LordZot.LordZot.ChargedSlam),
                                    origin = characterBody.corePosition,
                                    start = this.modifiedAimRay.origin
                                };
                                effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                GameObject original27 = Resources.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");
                                GameObject original277 = Resources.Load<GameObject>("prefabs/effects/ArtifactShellExplosion");
                                GameObject original2737 = Resources.Load<GameObject>("prefabs/effects/ArtifactworldPortalSpawnEffect");
                                GameObject original2777 = Resources.Load<GameObject>("prefabs/effects/GrandparentDeathEffectLightShafts");
                                GameObject original66 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                GameObject original26 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                                GameObject original25 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                                var direc = Quaternion.LookRotation(this.modifiedAimRay.direction);
                                GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

                                sonicboomeffectdata.scale = 3f;
                                sonicboomeffectdata.rotation = Quaternion.FromToRotation(characterBody.corePosition, (characterBody.corePosition + Vector3.up * 5)) * randomSpin;
                                sonicboomeffectdata.origin = transform1.position + transform1.forward * -1f + transform1.up * 0f;
                                /*   EffectSettings = original2.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings.applyScale = true;
                                   EffectSettings2 = original27.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings2.applyScale = true;
                                   EffectSettings4 = original277.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings4.applyScale = true;
                                   EffectSettings5 = original2737.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings5.applyScale = true;
                                   EffectSettings3 = original2777.GetComponent<RoR2.EffectComponent>();
                                   EffectSettings3.applyScale = true;
   */

                                GameObject original27767 = Resources.Load<GameObject>("prefabs/effects/MaulingRockImpact");

                             //   GameObject original277671 = Resources.Load<GameObject>("prefabs/effects/SmokescreenEffect");
                                GameObject original277672 = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                                GameObject original277673 = Resources.Load<GameObject>("prefabs/effects/TitanDeathEffect");
                                GameObject original277674 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGolem");
                                GameObject original277675 = Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                                GameObject original277676 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");

                                RoR2.EffectData impact = new RoR2.EffectData();
                                impact.scale = 2f + (1.8f * LordZot.LordZot.ChargedSlam);
                                impact.origin = model.corePosition + Vector3.up * 2;
                                impact.rotation = Quaternion.FromToRotation(model.corePosition, model.corePosition + Vector3.up);
                                RoR2.EffectManager.SpawnEffect(original27767, impact, true);
                               // RoR2.EffectManager.SpawnEffect(original277671, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277672, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277673, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277674, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277675, impact, true);
                                RoR2.EffectManager.SpawnEffect(original277676, impact, true);
                            
                                
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
                                RoR2.EffectData blast = new RoR2.EffectData();
                                blast.scale = 2f + (2.8f * LordZot.LordZot.ChargedSlam);
                                blast.origin = characterBody.corePosition;
                                RoR2.EffectData blast2 = new RoR2.EffectData();

                                ChildLocator component2 = transform1.GetComponent<ChildLocator>();
                                Transform transform = component2.FindChild("RightHand");

                                blast2.scale = 1f + (0.9f * LordZot.LordZot.ChargedSlam);
                                blast2.origin = characterBody.corePosition;
                                blast2.rotation = direc;

                                RoR2.EffectManager.SpawnEffect(original66, blast, true);
                                RoR2.EffectManager.SpawnEffect(original2, sonicboomeffectdata, true);
                                RoR2.EffectManager.SpawnEffect(original2, blast2, true);
                                RoR2.EffectManager.SpawnEffect(original27, blast, true);
                                RoR2.EffectManager.SpawnEffect(original25, blast, true);
                                RoR2.EffectManager.SpawnEffect(original26, blast, true);
                                if (LordZot.LordZot.ChargedSlam > 4)
                                {
                                    RoR2.EffectManager.SpawnEffect(ZotSlam.tracerEffectPrefab, effectData, true);
                                    RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, effectData, true);

                                };
                                if (LordZot.LordZot.ChargedSlam > 40)
                                {
                                    RoR2.Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
                                    RoR2.EffectManager.SpawnEffect(original277, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2737, blast, true);
                                    RoR2.EffectManager.SpawnEffect(original2777, blast, true);
                                };


                                if (LordZot.LordZot.ChargedSlam < 150)
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 1f + 0.003f * LordZot.LordZot.ChargedSlam,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 1f + 0.009f * LordZot.LordZot.ChargedSlam;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.LordZot.timeRemaining = 6f;
                                }
                                else
                                {
                                    RoR2.ShakeEmitter shakeEmitter;
                                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                    shakeEmitter.wave = new Wave
                                    {
                                        amplitude = 3f,
                                        frequency = 180f,
                                        cycleOffset = 0f
                                    };
                                    shakeEmitter.duration = 3.5f;
                                    shakeEmitter.radius = 50f;
                                    shakeEmitter.amplitudeTimeDecay = true;
                                    LordZot.LordZot.timeRemaining = 6f;
                                }

                                if (base.isAuthority)
                    {
                        new RoR2.BlastAttack
                            {
                                attacker = base.gameObject,
                                inflictor = base.gameObject,
                                teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                                baseDamage = 40 * 1.5f + (40 * 2.3f * LordZot.LordZot.ChargedSlam),
                                baseForce = 15000f,
                                position = characterBody.corePosition,
                                radius = 25f + 2f * LordZot.LordZot.ChargedSlam,
                                falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                                crit = characterBody.RollCrit(),
                                bonusForce = Vector3.up * (9000 + 4555f * LordZot.LordZot.ChargedSlam)
                            }.Fire();
                                Vector3 origin1 = this.modifiedAimRay.origin;
                    
                        
                                    hasswung = true;

                                            
                                        }
                                    }
                                }
                            }
                   



                }

                };
            if (hasswung && base.isAuthority)
            {
                GetModelAnimator().SetFloat("FireArrow.playbackRate", 0.3f);
                Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
                var direc = Quaternion.LookRotation(this.modifiedAimRay.direction);
                GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

                sonicboomeffectdata.scale = 3f;
                sonicboomeffectdata.rotation = direc * randomSpin;
                sonicboomeffectdata.origin = base.GetModelTransform().position + base.GetModelTransform().forward * 2f + base.GetModelTransform().up * 2f;


                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, true);
            }

            if (stopwatch21 >= 0.61f && base.isAuthority)
            {
                GetModelAnimator().SetFloat("FireArrow.playbackRate", 1f);
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06003C67 RID: 15463 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        // Token: 0x04003728 RID: 14120
        public static GameObject effectPrefab = GolemMonster.FireLaser.effectPrefab;

        // Token: 0x04003729 RID: 14121
        public static GameObject hitEffectPrefab = GolemMonster.FireLaser.hitEffectPrefab;

        // Token: 0x0400372A RID: 14122
        public static GameObject tracerEffectPrefab = GolemMonster.FireLaser.tracerEffectPrefab;

        // Token: 0x0400372B RID: 14123
        public static float damageCoefficient;

        // Token: 0x0400372C RID: 14124
        public static float blastRadius;

        // Token: 0x0400372D RID: 14125
        public static float force;

        // Token: 0x0400372E RID: 14126
        public static float minSpread;

        // Token: 0x0400372F RID: 14127
        public static float maxSpread;

        // Token: 0x04003730 RID: 14128
        public static int bulletCount;

        // Token: 0x04003731 RID: 14129
        public static float baseDuration = 1f;

        // Token: 0x04003732 RID: 14130
        public static string attackSoundString;

        // Token: 0x04003733 RID: 14131
        public Vector3 laserDirection;

        // Token: 0x04003734 RID: 14132
        private float duration;

        // Token: 0x04003735 RID: 14133
        private Ray modifiedAimRay;
        private RoR2.EffectComponent EffectSettings;
        private RoR2.EffectComponent EffectSettings2;
        private RoR2.EffectComponent EffectSettings4;
        private RoR2.EffectComponent EffectSettings5;
        private RoR2.EffectComponent EffectSettings3;
        private bool hasswung;
        private float stopwatch21;
        private bool facingdown;

        
    }
}











   