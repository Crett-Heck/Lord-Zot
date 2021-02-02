using System;
using System.Reflection;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
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
using EntityStates.BeetleGuardMonster;
using On.RoR2;
using EntityStates.GrandParentBoss;
using EntityStates.NewtMonster;
using EntityStates.VagrantMonster;
using System.Collections;

namespace LordZot
{

    [BepInDependency("com.bepis.r2api")]

    [BepInPlugin(MODUID, "LordZot", "0.0.1")] // put your own name and version here
    [R2APISubmoduleDependency(nameof(LanguageAPI), nameof(ResourcesAPI), nameof(PrefabAPI), nameof(SurvivorAPI), nameof(LoadoutAPI), nameof(ItemAPI), nameof(DifficultyAPI), nameof(BuffAPI), nameof(ItemAPI))] // need these dependencies for the mod to work properly


    public class LordZot : BaseUnityPlugin
    {
        public const string MODUID = "Jot.LordZot"; // put your own names here

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
        public Animator animator;
        private KeyCode jump;
        private bool jumpInputReceived;
        private RoR2.CharacterModel modela;
        public static bool inair = false;
        public static bool Busy;
        public static bool wellhefell;
        public static float fallspeed;
        public static float fastestfallspeed;
        public static bool holdtime = false;
        public static float ChargedLaser;
        public static bool ugh;
        public static bool flight = false;
        public static float flightcooldown = 0f;
        public static bool dontmove = false;
        //  private static float highestRecordedFallSpeed;

        private void Awake()
        {

            Assets.PopulateAssets(); // first we load the assets from our assetbundle 



            CreateDisplayPrefab(); // then we create our character's body prefab

            CreatePrefab(); // then we create our character's body prefab

            RegisterStates(); // register our skill entitystates for networking 



            RegisterCharacter(); // and finally put our new survivor in the game

          //  Debug.Log("Character Reggied");

            CreateDoppelganger(); // not really mandatory, but it's simple and not having an umbra is just kinda lame

            
           // Debug.Log("Dopple Reggied");

            On.RoR2.GlobalEventManager.OnCharacterHitGround += Landing; ;
            On.RoR2.FootstepHandler.Footstep_AnimationEvent += FootstepBlast;
            On.RoR2.GenericSkill.OnExecute += SummonShield;
     
        }



        void FixedUpdate()
        {
            var player = RoR2.PlayerCharacterMasterController.instances[0].master;
            var model = player.GetBody();
            var motor = model.characterMotor;
            

            if (model.characterMotor.isGrounded && model.baseNameToken is "ZOT_NAME")
            { flight = false;
                RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                flightparameters.channeledFlightGranterCount = 0;
                model.characterMotor.flightParameters = flightparameters;
                RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                gravparams.channeledAntiGravityGranterCount = 0;
                model.characterMotor.gravityParameters = gravparams;
            };

            if (jumpcooldown > 0)
            { jumpcooldown -= Time.deltaTime; };

            if (flightcooldown > 0)
            { flightcooldown -= Time.deltaTime; };
            if (model.inputBank.jump.down && jumpcooldown <= 0 && model.baseNameToken is "ZOT_NAME" && model.characterMotor.isGrounded && !Busy)
            {
                {

                    model.baseMoveSpeed = 3f;
                    ZotJump zotJump = new ZotJump();
                    RoR2.EntityStateMachine entityStateMachine = model.GetComponent<RoR2.EntityStateMachine>();
                    entityStateMachine.SetNextState(zotJump);
                    
                };
            };

            if (model.inputBank.sprint.down && model.baseNameToken is "ZOT_NAME" && !model.characterMotor.isGrounded && !flight && flightcooldown <= 0f)
            {
                GameObject original = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");
                RoR2.EffectData effectData = new RoR2.EffectData();
                effectData.origin = model.corePosition;
                effectData.scale = 5f;
                RoR2.EffectManager.SpawnEffect(original, effectData, false);
                model.baseMoveSpeed = 3f;

                RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                flightparameters.channeledFlightGranterCount = 2;
                model.characterMotor.flightParameters = flightparameters;
                RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                gravparams.channeledAntiGravityGranterCount = 2;
                model.characterMotor.gravityParameters = gravparams;

                model.characterMotor.velocity = Vector3.zero;
                LordZot.flight = true;
                flightcooldown = 500000f;
               
            };
            if (model.inputBank.sprint.down && model.baseNameToken is "ZOT_NAME" && !model.characterMotor.isGrounded && flight && flightcooldown <= 0f)
            {
                GameObject original = Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect");
                RoR2.EffectData effectData = new RoR2.EffectData();
                effectData.origin = model.corePosition;
                effectData.scale = 4f;
                RoR2.EffectManager.SpawnEffect(original, effectData, false);
                model.baseMoveSpeed = 3f;

                RoR2.CharacterFlightParameters flightparameters = model.characterMotor.flightParameters;
                flightparameters.channeledFlightGranterCount = -2;
                model.characterMotor.flightParameters = flightparameters;
                RoR2.CharacterGravityParameters gravparams = model.characterMotor.gravityParameters;
                gravparams.channeledAntiGravityGranterCount = -2;
                model.characterMotor.gravityParameters = gravparams;

                flightcooldown = 500000f;
                LordZot.flight = false;
            };

            if (model.inputBank.sprint.justReleased)
            {
                flightcooldown = 0.05f;
            }
  

                if (flight && model.baseNameToken is "ZOT_NAME" && !model.characterMotor.isGrounded )
            {
                model.baseMoveSpeed = 3f;
                model.characterMotor.moveDirection = model.inputBank.moveVector;
                ChildLocator component = model.GetComponent<ChildLocator>();

                if (component)
                {

                    GameObject original = Resources.Load<GameObject>("prefabs/effects/muzzleflashes/MuzzleflashHuntress");
                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/muzzleflashes/MuzzleflashHuntress");
                    Transform transform = component.FindChild("LeftFoot");
                    Transform transform2 = component.FindChild("RightFoot");
                    if (transform)
                    {
                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = transform.position;
                        effectData.scale = 5f;
                        RoR2.EffectManager.SpawnEffect(original, effectData, false);
                    }
                    if (transform2)
                    {
                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = transform2.position;
                        effectData.scale = 5f;
                        RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                   
                    }
                }


            };

            if (!motor.isGrounded && !flight)
            { motor.velocity.y -= Time.deltaTime * 1.5f;

                fallspeed = motor.velocity.y;
                string debug2 = fallspeed.ToString();
            //    Debug.Log("speed");
            //    Debug.Log(debug2);
                bool isfaster = fallspeed <= 0;
                if (isfaster)
                {
                    fastestfallspeed = motor.velocity.y;
                    string debug = fastestfallspeed.ToString();
                   // Debug.Log("fastest");
                    //Debug.Log(debug);
                }
                if (Busy)
                { motor.velocity.y = motor.velocity.y * 0.99f; };

            };
        
         
                
                if (timeRemainingstun > 0 && motor.isGrounded)
            {
              

                fastestfallspeed = 0f;
                    timeRemainingstun -= Time.deltaTime;
                motor.velocity = Vector3.zero;
                motor.moveDirection = Vector3.zero;
                

                }
                else
      
                {
            
                string debug = fastestfallspeed.ToString();

            };
        }
        void Update()
        {
            


            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }

            else
            {
                var player = RoR2.PlayerCharacterMasterController.instances[0].master;
              
                var model = player.GetBody().modelLocator.modelTransform;
                var animator = model.GetComponent<Animator>();
          

                animator.SetFloat("ShieldsIn", 0f);
            
                ChildLocator component = model.GetComponent<ChildLocator>();
            

                if (component)
                {

                    GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                    Transform transform = component.FindChild("LeftHand");
                    Transform transform2 = component.FindChild("RightHand");
                    if (transform)
                    {
                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = transform.position;
                        effectData.scale = 1f;
                        RoR2.EffectManager.SpawnEffect(original, effectData, false);
                    }
                    if (transform2)
                    {
                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = transform2.position;
                        effectData.scale = 1f;
                        RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                    }
                }
                timeRemaining = 1000f;
                //   RoR2.EntityStateMachine entityStateMachine = new RoR2.EntityStateMachine();
                //  entityStateMachine.SetState(zotunSummonShield);

             //   Debug.Log("statemachine");
               
              //  Debug.Log("setstate");



               
            }
        }

        private void SummonShield(On.RoR2.GenericSkill.orig_OnExecute orig, RoR2.GenericSkill self)
        {

            {
                if (self.skillNameToken.Equals("ZOT_PRIMARY_NAME"))
                {
                  
                    timeRemaining = 6.0f;
                    
                 
                    var model = self.characterBody.modelLocator.modelTransform;
                    var animator = model.GetComponent<Animator>();
                   

                    animator.SetFloat("ShieldsIn", 0.12f);
                 
                   


                }
              
            };
            {
                if (self.skillNameToken.Equals("ZOT_SECONDARY_NAME"))
                {
                    timeRemaining = 6;


                }
                orig(self);
            };

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
            LordZot.zotPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "LordZot", true);
           // Debug.Log("cloned");
            
            LordZot.zotPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            // create the model here, we're gonna replace commando's model with our own
            GameObject model = CreateModel(LordZot.zotPrefab, 0);
          //  Debug.Log("model created");

            GameObject gameObject2 = new GameObject("ModelBase");
            gameObject2.transform.parent = LordZot.zotPrefab.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 0.67f, 0f);
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
            characterDirection.turnSpeed = 700f;

           // Debug.Log("model set up");

            // set up the character body here
            RoR2.CharacterBody bodyComponent = LordZot.zotPrefab.GetComponent<RoR2.CharacterBody>();
            bodyComponent.bodyIndex = -1;
            bodyComponent.baseNameToken = "ZOT_NAME"; // name token
            bodyComponent.subtitleNameToken = "ZOT_SUBTITLE"; // subtitle token- used for umbras
            bodyComponent.bodyFlags = RoR2.CharacterBody.BodyFlags.ImmuneToExecutes | RoR2.CharacterBody.BodyFlags.IgnoreFallDamage | RoR2.CharacterBody.BodyFlags.ImmuneToGoo | RoR2.CharacterBody.BodyFlags.SprintAnyDirection;
            bodyComponent.rootMotionInMainState = false;
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = 700;
            bodyComponent.levelMaxHealth = 150;
            bodyComponent.baseRegen = 0.0f;
            bodyComponent.levelRegen = 0.3f;
            bodyComponent.baseMaxShield = 0;
            bodyComponent.levelMaxShield = 0;
            bodyComponent.baseMoveSpeed = 3;
            bodyComponent.levelMoveSpeed = 0;
            bodyComponent.baseAcceleration = 100;
            bodyComponent.baseJumpPower = 0f;
            bodyComponent.levelJumpPower = 0;
            bodyComponent.baseDamage = 40;
            bodyComponent.levelDamage = 8f;
            bodyComponent.baseAttackSpeed = 0.8f;
            bodyComponent.levelAttackSpeed = 0;
            bodyComponent.baseCrit = 0;
            bodyComponent.levelCrit = 0;
            bodyComponent.baseArmor = 250;
            bodyComponent.levelArmor = 10;
            bodyComponent.baseJumpCount = 0;
            bodyComponent.sprintingSpeedMultiplier = 1.0f;
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            bodyComponent.aimOriginTransform = gameObject4.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            bodyComponent.portraitIcon = Assets.icon1portrait;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.preferredPodPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<RoR2.CharacterBody>().preferredPodPrefab;
            bodyComponent.preferredInitialStateType = Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody").GetComponent<RoR2.CharacterBody>().preferredInitialStateType;
            bodyComponent.skinIndex = 0U;
         //   Debug.Log("body set up");
            // the charactermotor controls the survivor's movement and stuff
            RoR2.CharacterMotor characterMotor = LordZot.zotPrefab.GetComponent<RoR2.CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 125000f;
            characterMotor.airControl = 1f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;
            
            //characterMotor.useGravity = true;
            //characterMotor.isFlying = false;
         //   Debug.Log("movement set up");

            RoR2.InputBankTest inputBankTest = LordZot.zotPrefab.GetComponent<RoR2.InputBankTest>();
            inputBankTest.moveVector = Vector3.zero;

            RoR2.CameraTargetParams cameraTargetParams = LordZot.zotPrefab.GetComponent<RoR2.CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/GolemBody").GetComponent<RoR2.CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = RoR2.CameraTargetParams.AimType.Standard;
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

            // this component is used to handle all overlays and whatever on your character, without setting this up you won't get any cool effects like burning or freeze on the character
            // it goes on the model object of course
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
            healthComponent.health = 700f;
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
            rigidbody.mass = 125000f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider capsuleCollider = LordZot.zotPrefab.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 1.6f;
            capsuleCollider.height = 6f;
            capsuleCollider.direction = 1;

            KinematicCharacterMotor kinematicCharacterMotor = LordZot.zotPrefab.GetComponent<KinematicCharacterMotor>();
            kinematicCharacterMotor.CharacterController = characterMotor;
            kinematicCharacterMotor.Capsule = capsuleCollider;
            kinematicCharacterMotor.Rigidbody = rigidbody;

            capsuleCollider.radius = 1.6f;
            capsuleCollider.height = 6f;
            capsuleCollider.center = new Vector3(0, 0, 0);
            capsuleCollider.material = null;

            kinematicCharacterMotor.DetectDiscreteCollisions = false;
            kinematicCharacterMotor.GroundDetectionExtraDistance = 4f;
            kinematicCharacterMotor.MaxStepHeight = 50f;
            kinematicCharacterMotor.MinRequiredStepDepth = 0.1f;
            kinematicCharacterMotor.MaxStableSlopeAngle = 120f;
            kinematicCharacterMotor.MaxStableDistanceFromLedge = 0.5f;
            kinematicCharacterMotor.PreventSnappingOnLedges = false;
            kinematicCharacterMotor.MaxStableDenivelationAngle = 120f;
            kinematicCharacterMotor.RigidbodyInteractionType = RigidbodyInteractionType.Kinematic;
            kinematicCharacterMotor.PreserveAttachedRigidbodyMomentum = true;
            kinematicCharacterMotor.HasPlanarConstraint = false;
            kinematicCharacterMotor.PlanarConstraintAxis = Vector3.up;
            kinematicCharacterMotor.StepHandling = StepHandlingMethod.Standard;
            kinematicCharacterMotor.LedgeHandling = true;
            kinematicCharacterMotor.InteractiveRigidbodyHandling = true;
            kinematicCharacterMotor.SafeMovement = false;

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
            aimAnimator.pitchRangeMax = 55f;
            aimAnimator.pitchRangeMin = -50f;
            aimAnimator.yawRangeMin = -150f;
            aimAnimator.yawRangeMax = 150f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 45f;
            aimAnimator.giveupDuration = 8f;

           // Debug.Log("misc set up");

           // void FixedUpdate()
          //  {LordZot.highestRecordedFallSpeed = Mathf.Max(LordZot.highestRecordedFallSpeed, characterMotor ? (-characterMotor.velocity.y) : 0f); };

        }




        internal void CreateDisplayPrefab()
        {

            float model_scale = 0.12f;

            GameObject gameObject = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "ZotDisplayPrefab");

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
                    ignoreOverlays = false
                }

            };
            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<RoR2.TemporaryOverlay>();

            characterModel.SetFieldValue("mainSkinnedMeshRenderer", characterModel.baseRendererInfos[0].renderer.gameObject.GetComponent<SkinnedMeshRenderer>());


            characterDisplay = PrefabAPI.InstantiateClone(gameObject.GetComponent<RoR2.ModelLocator>().modelBaseTransform.gameObject, "ZotDisplayPrefab", true);
            characterDisplay.AddComponent<NetworkIdentity>();



        }

        

        private void RegisterCharacter()


        {




            // clone rex's syringe projectile prefab here to use as our own projectile
            arrowProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SyringeProjectile"), "Prefabs/Projectiles/ExampleArrowProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\ExampleSurvivor\\ExampleSurvivor\\ExampleSurvivor\\LordZot.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
           arrowProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            arrowProjectile.GetComponent<ProjectileDamage>().damage = 1f;
           arrowProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            //register it for networking
           if (arrowProjectile) PrefabAPI.RegisterNetworkPrefab(arrowProjectile);

            // add it to the projectile catalog or it won't work in multiplayer
            RoR2.ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(arrowProjectile);
            };





            // write a clean survivor description here!
            string desc = "A stranger from another reality, Lord Zot has come to this planet seeking ever more power.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > An exceedingly heavy survivor who gains power from natural resources and gems. -Unimplemented-" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - Completely unable to sprint, relying on ability usage for mobility." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - Cannot be knocked back, and causes collateral damage to foes when landing, and even when taking mere footsteps." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > - Holding Jump slowly increases its magnitude." + Environment.NewLine + Environment.NewLine;

            // add the language tokens
            LanguageAPI.Add("ZOT_NAME", "Lord Zot");
            LanguageAPI.Add("ZOT_DESCRIPTION", desc);
            LanguageAPI.Add("ZOT_SUBTITLE", "Mystic Titan");

            // add our new survivor to the game~
            RoR2.SurvivorDef survivorDef = new RoR2.SurvivorDef
            {
                name = "ZOT_NAME",
                unlockableName = "",
                descriptionToken = "ZOT_DESCRIPTION",
                primaryColor = characterColor,
                bodyPrefab = zotPrefab,
                displayPrefab = characterDisplay
            };


            SurvivorAPI.AddSurvivor(survivorDef);

            // set up the survivor's skills here
            SkillSetup();

            // gotta add it to the body catalog too
            RoR2.BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(zotPrefab);
            };
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
        }

        void RegisterStates()
        {
            

            // register the entitystates for networking reasons
            LoadoutAPI.AddSkill(typeof(EldritchFury));
            LoadoutAPI.AddSkill(typeof(ZotBlink));
            LoadoutAPI.AddSkill(typeof(ZotBulwark));
            LoadoutAPI.AddSkill(typeof(ZotJump));
        }

        void PassiveSetup()
        {
            // set up the passive skill here if you want
            RoR2.SkillLocator component = zotPrefab.GetComponent<RoR2.SkillLocator>();

            LanguageAPI.Add("ZOT_PASSIVE_NAME", "Mystic Titan");
            LanguageAPI.Add("ZOT_PASSIVE_DESCRIPTION", "<style=cIsDamage>Zot is unable to jump or sprint.</style> Collect Gems from the environment to gain power.");

            component.passiveSkill.enabled = true;
            component.passiveSkill.skillNameToken = "ZOT_PASSIVE_NAME";
            component.passiveSkill.skillDescriptionToken = "ZOT_PASSIVE_DESCRIPTION";
            component.passiveSkill.icon = Assets.iconP;
        }

        void PrimarySetup()
        {
            RoR2.SkillLocator component = zotPrefab.GetComponent<RoR2.SkillLocator>();

            LanguageAPI.Add("ZOT_PRIMARY_NAME", "Eldritch Fury");
            LanguageAPI.Add("ZOT_PRIMARY_DESCRIPTION", "Zot winds up and throws a mighty punch with one of his shielded gauntlets, alternating arms with each swing. Deals <style=cIsDamage>600% damage</style>.");

            // set up your primary skill def here!

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(EldritchFury));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon1;
            mySkillDef.skillDescriptionToken = "ZOT_PRIMARY_DESCRIPTION";
            mySkillDef.skillName = "ZOT_PRIMARY_NAME";
            mySkillDef.skillNameToken = "ZOT_PRIMARY_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            component.primary = zotPrefab.AddComponent<RoR2.GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
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


        private void ZotUtilitySetup()
        {
            RoR2.SkillLocator component = LordZot.zotPrefab.GetComponent<RoR2.SkillLocator>();
            LanguageAPI.Add("ZOT_UTILITY_BLINK_NAME", "Titan's Stride");
            LanguageAPI.Add("ZOT_UTILITY_BLINK_DESCRIPTION", "Tap quickly to <style=cIsUtility>dash incredibly fast</style> in the direction of your movement.");
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(ZotBlink));
            skillDef.activationStateMachineName = "Body";
            skillDef.baseMaxStock = 3;
            skillDef.baseRechargeInterval = 3f;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.Any;
            skillDef.isBullets = false;
            skillDef.isCombatSkill = false;
            skillDef.mustKeyPress = false;
            skillDef.noSprint = false;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.shootDelay = 0f;
            skillDef.stockToConsume = 1;
            skillDef.icon = Assets.KayleStride;
            skillDef.skillDescriptionToken = "ZOT_UTILITY_BLINK_DESCRIPTION";
            skillDef.skillName = "ZOT_UTILITY_BLINK_NAME";
            skillDef.skillNameToken = "ZOT_UTILITY_BLINK_NAME";
            LoadoutAPI.AddSkillDef(skillDef);
            component.utility = LordZot.zotPrefab.AddComponent<RoR2.GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(skillFamily);
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
            LanguageAPI.Add("ZOT_SECONDARY_NAME", "Gem Bulwark");
            LanguageAPI.Add("ZOT_SECONDARY_DESCRIPTION", " <style=cIsUtility>Deflect nearby enemies and attacks</style> with a backhand, as you begin charging a gem-powered beam that, once released, deals heavy damage.");
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(ZotBulwark));
            skillDef.activationStateMachineName = "Weapon";
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0f;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.PrioritySkill;
            skillDef.isBullets = false;
            skillDef.isCombatSkill = false;
            skillDef.mustKeyPress = false;
            skillDef.noSprint = false;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.shootDelay = 0f;
            skillDef.stockToConsume = 1;
            skillDef.icon = Assets.icon2;
            skillDef.skillDescriptionToken = "ZOT_SECONDARY_DESCRIPTION";
            skillDef.skillName = "ZOT_SECONDARY_NAME";
            skillDef.skillNameToken = "ZOT_SECONDARY_NAME";
            LoadoutAPI.AddSkillDef(skillDef);
            component.secondary = LordZot.zotPrefab.AddComponent<RoR2.GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(skillFamily);
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
            LanguageAPI.Add("ZOT_SECONDARY_NAME", "Gem Bulwark");
            LanguageAPI.Add("ZOT_SECONDARY_DESCRIPTION", " <style=cIsUtility>Deflect nearby enemies and attacks</style> with a backhand, as you begin charging a gem-powered beam that, once released, deals heavy damage.");
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
            skillDef.activationState = new SerializableEntityStateType(typeof(BaseLeap));
            skillDef.activationStateMachineName = "Body";
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0f;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.PrioritySkill;
            skillDef.isBullets = false;
            skillDef.isCombatSkill = false;
            skillDef.mustKeyPress = false;
            skillDef.noSprint = false;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 1;
            skillDef.shootDelay = 0f;
            skillDef.stockToConsume = 1;
            skillDef.icon = Assets.icon2;
            skillDef.skillDescriptionToken = "ZOT_SECONDARY_DESCRIPTION";
            skillDef.skillName = "ZOT_SECONDARY_NAME";
            skillDef.skillNameToken = "ZOT_SECONDARY_NAME";
            LoadoutAPI.AddSkillDef(skillDef);
            
        }
        private void FootstepBlast(On.RoR2.FootstepHandler.orig_Footstep_AnimationEvent orig, RoR2.FootstepHandler self, AnimationEvent animationEvent)
        {
            var player = RoR2.PlayerCharacterMasterController.instances[0].master;
            
            var bod = player.GetBody();
            var model = player.GetBody().modelLocator.modelTransform;
            var animaty = model.GetComponent<Animator>();


            bool flag = animationEvent.stringParameter is "LeftFoot"; 
            bool flag2 = animationEvent.stringParameter is "RightFoot";
            if (flag)
            {
                var player2 = RoR2.PlayerCharacterMasterController.instances[0].master;

                var bod2 = player.GetBody();
                var model2 = player.GetBody().modelLocator.modelTransform;

                ChildLocator component = model.GetComponent<ChildLocator>();
                if (component)
                {
                    Transform transform = component.FindChild("LeftFoot");

                    if (transform)
                    {

                        var position = transform.position + new Vector3(0f, 0f, 0f);


                        RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                        LandingBlast.attacker = bod2.gameObject;
                        LandingBlast.inflictor = bod2.gameObject;
                        LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                        LandingBlast.baseDamage = bod2.damage * 0.5f;
                        LandingBlast.baseForce = 1500f;
                        LandingBlast.bonusForce = new Vector3(0f, 1100f, 0f);
                        LandingBlast.position = position;
                        LandingBlast.radius = 25f;
                        LandingBlast.crit = bod.RollCrit();
                        LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                        LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                        LandingBlast.Fire();

                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                        

                        RoR2.EffectData effectData2 = new RoR2.EffectData();

                        effectData2.origin = position;
                        effectData2.scale = 1.5f;
                        
                        RoR2.EffectManager.SpawnEffect(original2, effectData2, false);
                      //  RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                    }
                }


            };

            if (flag2)
            {
                var player2 = RoR2.PlayerCharacterMasterController.instances[0].master;

                var bod2 = player.GetBody();
                var model2 = player.GetBody().modelLocator.modelTransform;

                ChildLocator component = model.GetComponent<ChildLocator>();
                if (component)
                {
                    Transform transform = component.FindChild("RightFoot");

                    if (transform)
                    {

                        var position = transform.position + new Vector3(0f, 0f, 0f);


                        RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                        LandingBlast.attacker = bod2.gameObject;
                        LandingBlast.inflictor = bod2.gameObject;
                        LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                        LandingBlast.baseDamage = bod2.damage * 0.5f;
                        LandingBlast.baseForce = 1500f;
                        LandingBlast.bonusForce = new Vector3(0f, 1100f, 0f);
                        LandingBlast.position = position;
                        LandingBlast.radius = 25f;
                        LandingBlast.crit = bod.RollCrit();
                        LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                        LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                        LandingBlast.Fire();

                        GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                        RoR2.EffectData effectData2 = new RoR2.EffectData();

                        effectData2.origin = position;
                        effectData2.scale = 1.5f;
                        RoR2.EffectManager.SpawnEffect(original2, effectData2, false);
                      //  RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                    }
                }


            };




            {
                orig(self, animationEvent);
            }


        }


        private void Landing(On.RoR2.GlobalEventManager.orig_OnCharacterHitGround orig, RoR2.GlobalEventManager self, RoR2.CharacterBody characterBody, Vector3 impactVelocity)
        {
            bool flag = characterBody.baseNameToken is "ZOT_NAME";
            
                if (flag)
            {
                LordZot.flight = false;
                var magnitude = fastestfallspeed + -fastestfallspeed + -fastestfallspeed;
                string magdebug = magnitude.ToString();
             //   Debug.Log("magnitude");
                Debug.Log(magdebug);
                var position = characterBody.footPosition;
                
               
                RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
                LandingBlast.attacker = characterBody.gameObject;
                LandingBlast.inflictor = characterBody.gameObject;
                LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
                LandingBlast.baseDamage = (characterBody.damage * 0.2f) * magnitude;
                LandingBlast.baseForce = magnitude * 50f;
                LandingBlast.bonusForce = new Vector3(0f, magnitude * 250f, 0f);
                LandingBlast.position = position;
                LandingBlast.radius = magnitude * 0.5f;
                LandingBlast.crit = characterBody.RollCrit();
                LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
                LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
                LandingBlast.Fire();

                
                timeRemainingstun = 1.6f;
                var player = RoR2.PlayerCharacterMasterController.instances[0].master;
                var model = player.GetBody();
                var motor = model.characterMotor;
                motor.velocity = Vector3.zero;
              

                GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");
                GameObject original25 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original66 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                GameObject original666 = Resources.Load<GameObject>("prefabs/effects/impacteffects/LemurianBruiserDeathImpact");


                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = position;
                effectData.scale = 0.4f * magnitude;
                RoR2.EffectManager.SpawnEffect(EntityStates.BeetleGuardMonster.GroundSlam.slamEffectPrefab, effectData, false);
                RoR2.EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, effectData, false);
                RoR2.EffectManager.SpawnEffect(original, effectData, false);
                RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                
                RoR2.EffectData effectData2 = new RoR2.EffectData();

                effectData2.origin = position;
                effectData2.scale = 0.8f * magnitude;
                RoR2.EffectManager.SpawnEffect(original2, effectData2, false);
                RoR2.Util.PlaySound(ExitSkyLeap.soundString, characterBody.gameObject);
                RoR2.EffectManager.SpawnEffect(original25, effectData2, false);
                RoR2.EffectManager.SpawnEffect(original66, effectData2, false);
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
                if (magnitude > 120)
                { RoR2.EffectManager.SpawnEffect(original666, effectData, false);
                   // RoR2.EffectManager.SpawnEffect(EntityStates.VagrantMonster.FireMegaNova.novaEffectPrefab, effectData, false);
                       // RoR2.Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
                    RoR2.Util.PlaySound (ExitSkyLeap.soundString, base.gameObject);// RoR2.Util.PlaySound(FireMegaNova.novaSoundString, characterBody.gameObject);
                    ;
                };

            };






            {
                orig(self, characterBody, impactVelocity);
            }

        }



        private void CreateDoppelganger()
        {
            // set up the doppelganger for artifact of vengeance here
            // quite simple, gets a bit more complex if you're adding your own ai, but commando ai will do

            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "ExampleSurvivorMonsterMaster", true, "C:\\Users\\test\\Documents\\ror2mods\\ExampleSurvivor\\ExampleSurvivor\\ExampleSurvivor\\LordZot.cs", "CreateDoppelganger", 159);

            RoR2.MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(doppelganger);
            };

            RoR2.CharacterMaster component = doppelganger.GetComponent<RoR2.CharacterMaster>();
            component.bodyPrefab = zotPrefab;







        }

    }
  

    // get the assets from your assetbundle here
    // if it's returning null, check and make sure you have the build action set to "Embedded Resource" and the file names are right because it's not gonna work otherwise
    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;
        public static AssetBundleResourcesProvider Provider;
        public static RoR2.CharacterModel ZotDisplayPrefab;
        public static RoR2.CharacterModel LordZot;



         public static Texture charPortrait;
        public static Texture icon1portrait;
        public static Sprite iconP;
           public static Sprite icon1;
         public static Sprite icon2;
         public static Sprite icon3;
        public static Sprite gameboypunch;
        public static Sprite KayleStride;

        // public static Sprite icon4;

        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LordZot.ZotAssetBundle"))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    Provider = new AssetBundleResourcesProvider("@LordZot", MainAssetBundle);
                    R2API.ResourcesAPI.AddProvider(Provider);
                    var materials = MainAssetBundle.LoadAllAssets<Material>();

                    for (int i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].shader.name == "Standard")
                        {
                            materials[i].shader = Resources.Load<Shader>("shaders/deferred/hgstandard");
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
            icon1portrait = MainAssetBundle.LoadAsset<Sprite>("ZotEldritchFury").texture;


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
                base.OnEnter();
            characterBody.aimOriginTransform.position = characterBody.aimOriginTransform.position + characterBody.aimOriginTransform.forward * 3;
            LordZot.LordZot.Busy = true;
            LordZot.LordZot.holdtime = false;
            LordZot.LordZot.dontmove = false;
            base.GetModelAnimator();
            Transform modelTransform = base.GetModelTransform();
            Vector3 prevcampos()
            {  return base.cameraTargetParams.cameraParams.standardLocalCameraPos; }
            if (base.isAuthority)
            {
                originalcampos = prevcampos();
            }
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            Ray backhand = GetAimRay();
            GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
            RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
            
          if (!characterMotor.isGrounded && LordZot.LordZot.ugh)
            { PlayAnimation("Body", "AscendDescend");
            };
            Quaternion randomSpin = Quaternion.AngleAxis(UnityEngine.Random.Range(-300f, 300f), Vector3.forward);
            var direc = modelTransform.rotation;
            sonicboomeffectdata.scale = 3f;
            sonicboomeffectdata.rotation = direc * randomSpin;
            sonicboomeffectdata.origin = modelTransform.position + modelTransform.forward * -2f + modelTransform.up * 0f;
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
            LandingBlast.attacker = base.gameObject;
            LandingBlast.inflictor = base.gameObject;
            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
            LandingBlast.baseDamage = characterBody.damage * 0.5f;
            LandingBlast.baseForce = 6500f;
            LandingBlast.bonusForce = LandingBlast.baseForce * backhand.direction;
            LandingBlast.position = characterBody.corePosition + characterBody.characterDirection.forward * 6f;
            LandingBlast.radius = 25f;
            LandingBlast.crit = base.RollCrit();
            LandingBlast.falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot;
            LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
            LandingBlast.Fire();
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


            base.cameraTargetParams.cameraParams.standardLocalCameraPos.x = -7f;
              base.cameraTargetParams.cameraParams.standardLocalCameraPos.y = -3f;
            base.cameraTargetParams.cameraParams.standardLocalCameraPos.z = -22f;

            this.duration = ZotBulwark.baseDuration;
            
                this.chargePlayID = RoR2.Util.PlayScaledSound(ChargeLaser.attackSoundString, base.gameObject, (1f / this.duration));
                if (modelTransform)
            {
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                GameObject laser = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                GameObject laserfab = (EntityStates.GolemMonster.ChargeLaser.laserPrefab);
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
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
                            RoR2.EffectManager.SpawnEffect(original, effectData, false);
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
                            RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                        }
                          
                        
                    }
                }
                }
                if (base.characterBody)
                {
                    base.characterBody.SetAimTimer(this.duration);
                    StartAimMode(this.duration);
                }
               
            Animator animator = GetModelAnimator();
            base.PlayAnimation("Gesture, Additive", "GemBulwark", "FireArrow.playbackRate", 1f);
            LordZot.LordZot.ChargedLaser = 1f;
            
            this.laserOn = true;    
            animator.SetFloat("ShieldsIn", 0.12f);
        }

            // Token: 0x06003C56 RID: 15446 RVA: 0x000FB208 File Offset: 0x000F9408
            public override void OnExit()
            {
            characterBody.aimOriginTransform.position = characterBody.aimOriginTransform.position - characterBody.aimOriginTransform.forward * 3;
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            AkSoundEngine.StopPlayingID(this.chargePlayID);
                base.OnExit();
            LordZot.LordZot.ugh = false;
            LordZot.LordZot.holdtime = false;
            LordZot.LordZot.dontmove = false;
            base.cameraTargetParams.cameraParams.standardLocalCameraPos = originalcampos;
            LordZot.LordZot.Busy = false;
            if (this.chargeEffect)
                {
                    EntityState.Destroy(this.chargeEffect);
                }
                if (this.laserEffect)
                {
                    EntityState.Destroy(this.laserEffect);
                }
            }

        // Token: 0x06003C57 RID: 15447 RVA: 0x000FB258 File Offset: 0x000F9458
        public override void Update()
        {
            base.Update();
            if (this.stopwatch21 > 1f)
            {
                if (this.stopwatch21 < 1.05f )
                {
                    Transform modelTransform = base.GetModelTransform();
                    GameObject original = Resources.Load<GameObject>("prefabs/effects/muzzleflashes/MuzzleflashFire"); 
                    ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                    if (component)
                    {
                        Transform transform = component.FindChild("RightHand");
                        if (transform)
                        {
                            
                            RoR2.EffectData effectData = new RoR2.EffectData();
                            effectData.origin = transform.position;
                            effectData.scale = 2f;
                            RoR2.EffectManager.SpawnEffect(original, effectData, false);
                            
                        }
                    };
                }
              


                if (laserEffect && laserLineComponent)
                {
                    if (this.stopwatch1 < 15f)
                    {
                        float num = 1000f;
                        Ray aimRay = GetAimRay();
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

                        laserLineComponent.startWidth = 1f + (stopwatch1 * 0.25f);
                        laserLineComponent.endWidth = 1f + (stopwatch1 * 0.25f);

                        //GameObject end = Resources.Load<GameObject>("prefabs/effects/muzzleflashes/MuzzleflashHuntressFlurry");
                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = point;
                        effectData.scale = 1f + (0.215f * stopwatch1);
                      //  RoR2.EffectManager.SpawnEffect(end, effectData, false);
                     
                    }
                    else
                    {
                        float num = 1000f;
                        Ray aimRay = GetAimRay();

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

                        laserLineComponent.startWidth = 3.7f;
                        laserLineComponent.endWidth = 3.7f;
                       // GameObject end = Resources.Load<GameObject>("prefabs/effects/muzzleflashes/MuzzleflashHuntressFlurry");
                        RoR2.EffectData effectData = new RoR2.EffectData();
                        effectData.origin = point;
                        effectData.scale = 6f;
                       // RoR2.EffectManager.SpawnEffect(end, effectData, false);
                    };


                }
            }
        }
            // Token: 0x06003C58 RID: 15448 RVA: 0x000FB3B4 File Offset: 0x000F95B4
            public override void FixedUpdate()
            {
                base.FixedUpdate();
            this.stopwatch21 += Time.fixedDeltaTime;
            this.stopwatch1 += Time.fixedDeltaTime + (Time.fixedDeltaTime * (0.1f * characterBody.level));
            this.updatewatch += Time.fixedDeltaTime;
            bool flag26 = base.inputBank.skill2.down;
            bool flag266 = base.inputBank.skill2.justReleased;
            bool flag27 = this.stopwatch1 >= this.duration;
            bool flag29 = this.updatewatch >= 1f;
            this.duration = this.stopwatch1 + 100;
            stupidtime2 -= Time.fixedDeltaTime;
         
            if (LordZot.LordZot.flight = true)
            {
                
            };
            if (characterMotor.isGrounded && LordZot.LordZot.ugh)
            {



            };
            if (this.stopwatch1 >= 0.6f && !flag26)
            {
                   
                    // PlayAnimation("Body", "Idle", null, 1f);

                this.outer.SetNextStateToMain(); };

                if (flag29)
            {
                if (this.stopwatch1 > 15f)
                {
                    RoR2.ShakeEmitter shakeEmitter;
                    shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                    shakeEmitter.wave = new Wave
                    {
                        amplitude = 0.5f,
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
                        amplitude = 0.33f * this.stopwatch1,
                        frequency = 180f,
                        cycleOffset = 0f
                    };
                    shakeEmitter.duration = 1f;
                    shakeEmitter.radius = 50f;
                    shakeEmitter.amplitudeTimeDecay = false;
                    LordZot.LordZot.timeRemaining = 6f;
                    this.updatewatch = 0f;
                };

            };

            if (this.stopwatch21 > 1f && flag29)
            {
                GameObject original2 = EntityStates.GolemMonster.ChargeLaser.effectPrefab;
                Transform modelTransform = base.GetModelTransform();
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                Transform transform2 = component.FindChild("RightHand");
                this.chargePlayID = RoR2.Util.PlayScaledSound(ChargeLaser.attackSoundString, base.gameObject, 1f);
                if (transform2)
                {
                    GameObject laser = (EntityStates.GolemMonster.ChargeLaser.effectPrefab);
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = transform2.position;
                    effectData.scale = 1f + (0.2f * stopwatch1);
                    RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                    RoR2.EffectManager.SpawnEffect(laser, effectData, false);
                }

            };

            if (this.stopwatch21 > 10f && flag29)
            {
                Transform modelTransform = base.GetModelTransform();
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                Transform transform2 = component.FindChild("RightHand");
                if (transform2)
                {
                    GameObject laser2 = Resources.Load<GameObject>("prefabs/GolemClapCharge");
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = transform2.position;
                    effectData.scale = 1f + (0.2f * stopwatch1);
                    RoR2.EffectManager.SpawnEffect(laser2, effectData, false);
                }
            };

            if (flag27 && flag26)
            {

                ZotLaser zotLaser = new ZotLaser();
                { LordZot.LordZot.ChargedLaser = this.stopwatch1; }
                LordZot.LordZot.ChargedLaser = this.stopwatch1;
                zotLaser.laserDirection = laserDirection;
                this.outer.SetNextState(zotLaser);


                return;
            };
            if (flag27 | (!flag26 && this.stopwatch21 >= 1f))
            {

                ZotLaser zotLaser = new ZotLaser();
                { LordZot.LordZot.ChargedLaser = this.stopwatch1; }
                LordZot.LordZot.ChargedLaser = this.stopwatch1;
                zotLaser.laserDirection = laserDirection;
                this.outer.SetNextState(zotLaser);

                return;
            };
            if (this.stopwatch21 > 1f && this.stopwatch21 < 1.1f)
            {


                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 5f;
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);
            };
            if (this.stopwatch21 > 5f && this.stopwatch21 < 5.1f)
            {

                

            };



            if (stupidtime >= 0)
            {
                stupidtime -= Time.fixedDeltaTime;
            };
            if (stupidtime <= 0)
            {
                LordZot.LordZot.holdtime = false;
            };

            if (this.stopwatch21 >= 0.65f && !LordZot.LordZot.holdtime)
            {
                PlayCrossfade("Bulwark", "HoldBulwark", null, 1, 0.1f);
                this.modelanimator = GetModelAnimator();
                modelanimator.SetBool("GodStop", false);
                LordZot.LordZot.holdtime = true;
                    stupidtime = 10f;
            };

            base.characterDirection.moveVector = base.inputBank.aimDirection;


            if (base.fixedAge >= this.duration && base.isAuthority)
                {
                
                EntityStates.ZotStates.ZotLaser zotLaser = new EntityStates.ZotStates.ZotLaser();
                    zotLaser.laserDirection = laserDirection;
                { LordZot.LordZot.ChargedLaser = this.stopwatch1; }
                LordZot.LordZot.ChargedLaser = this.stopwatch1;
                this.outer.SetState(zotLaser);
                    return;
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

        public float ChargeDuration { get; private set; }
    }


    public class ZotLaser : BaseState
    {
        // Token: 0x06003C64 RID: 15460 RVA: 0x000FB714 File Offset: 0x000F9914
        public override void OnEnter()
        {
            base.OnEnter();
            LordZot.LordZot.Busy = true;
            this.duration = ZotLaser.baseDuration / this.attackSpeedStat;
            this.modifiedAimRay = base.GetAimRay();
            this.modifiedAimRay.direction = this.laserDirection;
        
            base.GetModelAnimator().SetBool("GodStop", true);
            base.GetModelAnimator();
            PlayAnimation("Gesture, Additive", "FireBulwark", "FireArrow.playbackRate", 1f);
            Transform modelTransform = base.GetModelTransform();
            RoR2.Util.PlaySound(EntityStates.GolemMonster.FireLaser.attackSoundString, base.gameObject);
            string text = "RightHand";
            if (base.characterBody)
            {
              //  base.characterBody.SetAimTimer(1f);
            }
            if (effectPrefab)
            {
                RoR2.EffectManager.SimpleMuzzleFlash(EntityStates.GolemMonster.FireLaser.effectPrefab, base.gameObject, text, false);
            }
            if (base.isAuthority)
            {
                float num = 100f;
                Vector3 vector = this.modifiedAimRay.origin + this.modifiedAimRay.direction * num;
                RaycastHit raycastHit;
                if (Physics.Raycast(this.modifiedAimRay, out raycastHit, num, RoR2.LayerIndex.world.mask | RoR2.LayerIndex.defaultLayer.mask | RoR2.LayerIndex.entityPrecise.mask))
                {
                    vector = raycastHit.point;
                }
                new RoR2.BlastAttack
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = RoR2.TeamComponent.GetObjectTeam(base.gameObject),
                    baseDamage = this.damageStat * 1.4f * LordZot.LordZot.ChargedLaser,
                    baseForce = 1000 + 555f * LordZot.LordZot.ChargedLaser,
                    position = vector,
                    radius = 13 + 0.3f * LordZot.LordZot.ChargedLaser,
                    falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                    bonusForce = ZotLaser.force * this.modifiedAimRay.direction
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
                            {   scale = 3f + (2.9f * LordZot.LordZot.ChargedLaser),
                                origin = vector,
                                start = this.modifiedAimRay.origin
                            };
                            effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                            GameObject original27 = Resources.Load<GameObject>("prefabs/effects/MagmaWormImpactExplosion");


                            RoR2.EffectData blast = new RoR2.EffectData();
                            blast.scale = 5f + (1.8f * LordZot.LordZot.ChargedLaser);
                            blast.origin = raycastHit.point;
                            RoR2.EffectData blast2 = new RoR2.EffectData();

                            ChildLocator component2 = modelTransform.GetComponent<ChildLocator>();
                                Transform transform = component2.FindChild("RightHand");

                            blast2.scale = 3f + (0.9f * LordZot.LordZot.ChargedLaser);
                            blast2.origin = transform.position;
                            RoR2.EffectManager.SpawnEffect(original27, blast, true);
                            RoR2.EffectManager.SpawnEffect(original2, blast, true);
                            RoR2.EffectManager.SpawnEffect(original2, blast2, true);
                            RoR2.EffectManager.SpawnEffect(ZotLaser.tracerEffectPrefab, effectData, true);
                            RoR2.EffectManager.SpawnEffect(EntityStates.QuestVolatileBattery.CountDown.explosionEffectPrefab, effectData, true);
                            if (LordZot.LordZot.ChargedLaser < 240)
                            {
                                RoR2.ShakeEmitter shakeEmitter;
                                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                                shakeEmitter.wave = new Wave
                                {
                                    amplitude = 0.05f * LordZot.LordZot.ChargedLaser,
                                    frequency = 180f,
                                    cycleOffset = 0f
                                };
                                shakeEmitter.duration = 0.05f * LordZot.LordZot.ChargedLaser;
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
                                    amplitude = 0.3f * LordZot.LordZot.ChargedLaser,
                                    frequency = 180f,
                                    cycleOffset = 0f
                                };
                                shakeEmitter.duration = 9f;
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
            LordZot.LordZot.Busy = false;
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
    }

    public class ZotBlink : BaseState
    {

        public override void OnEnter()
        {
            base.OnEnter();


            RoR2.Util.PlaySound(BaseSlideState.soundString, base.gameObject);
            RoR2.Util.PlaySound(ExitSkyLeap.soundString, zotPrefab);
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<RoR2.CharacterModel>();
            }
            if (this.characterModel)
            {
                 this.characterModel.invisibilityCount++;
            }
            
            if (this.hurtboxGroup)
            {
                RoR2.HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }

            if (base.inputBank)
            {
                _ = base.characterDirection;
            }
            if (NetworkServer.active)
            {
                RoR2.Util.CleanseBody(base.characterBody, true, false, false, false, false);
            }
            if (BaseSlideState.slideEffectPrefab && base.characterBody)
            {
                Vector3 position = base.characterBody.corePosition;
                Quaternion rotation = Quaternion.identity;
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
                LandingBlast.baseDamage = characterBody.damage * 1f;
                LandingBlast.baseForce = 4000f;
                LandingBlast.position = position;
                LandingBlast.radius = 35f;
                LandingBlast.crit = characterBody.RollCrit();
                LandingBlast.Fire();

                RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();
                sonicboomeffectdata.scale = 3f;
                sonicboomeffectdata.rotation = rotation;
                sonicboomeffectdata.origin = position;
                
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
                RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                RoR2.EffectManager.SpawnEffect(original2, sonicboomeffectdata, false);
                RoR2.EffectManager.SimpleEffect(BaseSlideState.slideEffectPrefab, position, rotation, false);
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

            }
        }

        // Token: 0x06003FBF RID: 16319 RVA: 0x0010AE40 File Offset: 0x00109040
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                Vector3 a = Vector3.zero;
                if (base.inputBank && base.characterDirection)
                {
                    a = base.characterDirection.forward;
                }
                if (base.characterMotor)
                {
                    float num = BaseSlideState.speedCoefficientCurve.Evaluate(base.fixedAge / ZotBlink.duration);
                    base.characterMotor.rootMotion += this.slideRotation * (num * (this.moveSpeedStat * 6) * a * Time.fixedDeltaTime);
                }

                if (base.fixedAge >= ZotBlink.duration)
                {
                    this.outer.SetNextStateToMain();
                }
            }

        }

        // Token: 0x06003FC0 RID: 16320 RVA: 0x0010AEFF File Offset: 0x001090FF
        public override void OnExit()
        {

            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                RoR2.TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                temporaryOverlay.duration = 0.2f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<RoR2.CharacterModel>());
                RoR2.TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                temporaryOverlay2.duration = 0.3f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<RoR2.CharacterModel>());
            }
            if (this.characterModel)
            {
                this.characterModel.invisibilityCount--;
            }
            if (this.hurtboxGroup)
            {
                RoR2.HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
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
        public static float duration = 0.2f;
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
        public static float destealthDuration = 0.2f;
        private RoR2.HurtBoxGroup hurtboxGroup;
        public static GameObject zotPrefab;
        public static GameObject slamImpactEffect;
        public static GameObject slamEffectPrefab;
        public GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");



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
            this.stopwatch = 1f;
            this.Duration = this.stopwatch + 1f;
            LordZot.LordZot.jumpcooldown = 5f;
            var position = characterBody.footPosition;
            LordZot.LordZot.timeRemainingstun = 0f;

            RoR2.BlastAttack LandingBlast = new RoR2.BlastAttack();
            LandingBlast.attacker = base.gameObject;
            LandingBlast.inflictor = base.gameObject;
            LandingBlast.teamIndex = RoR2.TeamComponent.GetObjectTeam(LandingBlast.attacker);
            LandingBlast.baseDamage = base.damageStat * 1f;
            LandingBlast.baseForce = 1000f;
            LandingBlast.bonusForce = new Vector3(0f, 2500f, 0f);
            LandingBlast.position = position;
            LandingBlast.radius = 25f;
            LandingBlast.crit = base.RollCrit();
            LandingBlast.attackerFiltering = AttackerFiltering.NeverHit;
            LandingBlast.Fire();

            GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
            RoR2.EffectData effectData2 = new RoR2.EffectData();

            effectData2.origin = position;
            effectData2.scale = 25f;
            RoR2.EffectManager.SpawnEffect(original6, effectData2, false);

            this.modelAnimator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();
            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
            RoR2.EffectManager.SimpleEffect(original2, characterBody.footPosition, Quaternion.Euler (0f, 0f, 0f), false);

            PlayAnimation("Body", "ChargeJump", "JumpChargeDuration", this.Duration);

        }
        
        public override void OnExit()
        {


            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;
            this.updatewatch += Time.fixedDeltaTime;
            bool flag26 = base.inputBank.jump.down;
            bool flag266 = base.inputBank.jump.justReleased;
            bool flag27 = this.stopwatch >= this.Duration;
            bool flag29 = this.updatewatch >= 0.3f;
            this.Duration = this.stopwatch + 100;
            LordZot.LordZot.jumpcooldown = this.Duration;
            if (flag29)
            {
                RoR2.ShakeEmitter shakeEmitter;
                shakeEmitter = characterBody.gameObject.AddComponent<RoR2.ShakeEmitter>();
                shakeEmitter.wave = new Wave
                {
                    amplitude = 0.1f * this.stopwatch,
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
                
                
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);
                
            }

            if(characterMotor.isGrounded)
            {characterMotor.velocity = Vector3.zero;};    

                if (this.stopwatch > 5f && flag29)
            {
                RoR2.TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                temporaryOverlay.duration = 0.3f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(1f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matVagrantEnergized");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<RoR2.CharacterModel>());
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.9f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);

            }

            if (this.stopwatch > 10f && flag29)
            {
                RoR2.TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                temporaryOverlay.duration = 0.9f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(1f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matVagrantEnergized");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<RoR2.CharacterModel>());
                GameObject original7 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/LightningFlash");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 0.15f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original7, effectData, false);
                RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);

            }

            if (flag27 && flag26)
            {
                LordZot.LordZot.jumpcooldown = 0f;
                BaseLeap zotLeap = new BaseLeap();
                { LordZot.LordZot.Charged = this.stopwatch; }
                LordZot.LordZot.Charged = this.stopwatch;
                this.outer.SetNextState(zotLeap);

                return;
            }
            if (flag27)
            {
                LordZot.LordZot.jumpcooldown = 0f;
                PlayCrossfade("Body", "Idle", null, 1f, 0.2f);
                this.outer.SetNextStateToMain();
               
                return;
            }
            if (this.stopwatch > 1f && this.stopwatch < 1.1f)
            {

             
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/CharacterLandImpact");
                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 5f;
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);
            }
            if (this.stopwatch > 5f && this.stopwatch < 5.1f)
            {
                
                GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                RoR2.EffectData effectData = new RoR2.EffectData();

                effectData.origin = characterBody.footPosition;
                effectData.scale = 1f * this.stopwatch;
                RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);
                RoR2.EffectManager.SpawnEffect(original6, effectData, false);

            }
            if (flag266 && this.stopwatch >= 1f)
            {
                LordZot.LordZot.timeRemainingstun = 0f;
                BaseLeap zotLeap = new BaseLeap();
                { LordZot.LordZot.Charged = this.stopwatch; }
                LordZot.LordZot.Charged = this.stopwatch;
                this.outer.SetNextState(zotLeap);
            }

            if (flag266 && this.stopwatch < 1f)
            {
                LordZot.LordZot.jumpcooldown = 0f;

                this.outer.SetNextStateToMain();

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
            LordZot.LordZot.flightcooldown = 0.5f;
            LordZot.LordZot.timeRemainingstun = 0f;
            PlayAnimation("Body", "TitanJump", "forwardSpeed", 1f);
            this.stopwatch = 0f;
            this.previousAirControl = 1f;
            base.characterMotor.airControl = 3f;
            this.previousaccel = 700;
            base.characterBody.baseAcceleration = 80f * LordZot.LordZot.Charged * (base.characterBody.level * 0.2f) + 255;
            BaseLeap.previous = 3f;
            base.characterBody.baseMoveSpeed = (LordZot.LordZot.Charged * base.characterBody.level * 0.5f) + (LordZot.LordZot.Charged * 4f) + 3f;
            Vector3 direction = base.GetAimRay().direction;
            if (base.isAuthority)
            {




                direction.y = Mathf.Max(direction.y, BaseLeap.minimumY);
                Vector3 b = Vector3.up * LordZot.LordZot.Charged * 0.4f * base.characterBody.level;
                Vector3 b2 = Vector3.up * LordZot.LordZot.Charged * 5f;
                base.characterMotor.Motor.ForceUnground();
               
                base.characterMotor.velocity = b + b2 + Vector3.up * 15f;
                this.isCritAuthority = base.RollCrit();
            }
            base.GetModelTransform().GetComponent<RoR2.AimAnimator>().enabled = true;
            RoR2.Util.PlaySound(BaseLeap.leapSoundString, base.gameObject);

            if (base.isAuthority)
            {

            }
            RoR2.Util.PlaySound(BaseLeap.soundLoopStartEvent, base.gameObject);
            Vector3 footPosition = base.characterBody.footPosition;
            new RoR2.BlastAttack
            {
                attacker = base.gameObject,
                baseDamage = this.damageStat * 0.1f * LordZot.LordZot.Charged,
                baseForce = 0,
                bonusForce = new Vector3(0f, 150f * LordZot.LordZot.Charged, 0f),
                crit = this.isCritAuthority,
                damageType = DamageType.Generic,
                falloffModel = RoR2.BlastAttack.FalloffModel.SweetSpot,
                procCoefficient = 1,
                radius = 25f,
                position = footPosition,
                attackerFiltering = AttackerFiltering.NeverHit,
                teamIndex = base.teamComponent.teamIndex
            }.Fire();
            var position = this.characterBody.footPosition;
            RoR2.EffectData sonicboomeffectdata = new RoR2.EffectData();

            sonicboomeffectdata.scale = 3f;
            sonicboomeffectdata.rotation = Quaternion.AngleAxis(90, Vector3.up);
            sonicboomeffectdata.origin = position;
            GameObject sonicboom = Resources.Load<GameObject>("prefabs/effects/SonicBoomEffect");
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);
            GameObject original22 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
            GameObject original6 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
            GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/PodGroundImpact");
            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/ScavSitImpact");

            RoR2.EffectData effectData = new RoR2.EffectData();

            effectData.origin = position;
            effectData.scale = 0.2f * LordZot.LordZot.Charged;
            RoR2.EffectManager.SpawnEffect(EntityStates.LemurianBruiserMonster.SpawnState.spawnEffectPrefab, effectData, false);

            RoR2.EffectManager.SpawnEffect(original2, effectData, false);
            RoR2.EffectManager.SpawnEffect(original6, effectData, false);
            RoR2.EffectManager.SpawnEffect(original22, effectData, false);

            if (LordZot.LordZot.Charged > 4.5f)
            {
                RoR2.EffectManager.SpawnEffect(EntityStates.GravekeeperBoss.SpawnState.spawnEffectPrefab, effectData, false);
                RoR2.EffectManager.SpawnEffect(original, effectData, false);
            }

        }


        // Token: 0x06003DF6 RID: 15862 RVA: 0x0010242C File Offset: 0x0010062C
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.characterMotor)


            {
                if (this.stopwatch >= 0)
                { this.stopwatch -= Time.fixedDeltaTime; };

                if (/*LordZot.LordZot.timeRemainingstun <= 0 &&*/ !LordZot.LordZot.wellhefell)
                {
                    bool utilityleap = inputBank.skill3.down;
                    bool primaryleap = inputBank.skill1.down;
                    bool secondaryleap = inputBank.skill2.down;
                    if (secondaryleap)
                    {
                        LordZot.LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.LordZot.jumpcooldown = 0f;
                        base.characterMotor.airControl = 1f;

                        base.characterBody.baseMoveSpeed = 3f;
                        base.characterBody.baseAcceleration = 700f;
                        LordZot.LordZot.ugh = true;
                        ZotBulwark bulwarkleap = new ZotBulwark();
                        this.outer.SetState(bulwarkleap);


                    };

                    if (primaryleap)
                    {
                        LordZot.LordZot.timeRemaining = 6f;
                        Animator modelAnimator = base.GetModelAnimator();
                        modelAnimator.SetFloat("ShieldsIn", 0.12f);
                        this.modelTransform = base.GetModelTransform();
                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                        LordZot.LordZot.jumpcooldown = 0f;
                        base.characterMotor.airControl = this.previousAirControl;

                        base.characterBody.baseMoveSpeed = 3f;
                        base.characterBody.baseAcceleration = 700f;
                        EldritchFury eldritchfuryleap = new EldritchFury();
                        this.outer.SetState(eldritchfuryleap);



                    };
                    if (utilityleap)
                    {
                        LordZot.LordZot.jumpcooldown = 0f;
                        base.characterMotor.airControl = this.previousAirControl;

                        base.characterBody.baseMoveSpeed = 3f;
                        base.characterBody.baseAcceleration = 700f;

                        ZotBlink zotblinkleap = new ZotBlink();
                        this.outer.SetState(zotblinkleap);

                    };
                }
                if (!characterMotor.isGrounded)
                {
                    base.characterDirection.moveVector = base.inputBank.moveVector;
                    base.characterMotor.moveDirection = base.inputBank.moveVector;
                    base.characterMotor.airControl = base.characterMotor.airControl - base.characterMotor.airControl * 0.01f;
                };
                if (characterBody.baseAcceleration > 35)

                { base.characterBody.baseAcceleration = base.characterBody.baseAcceleration * 0.97f; }
                if (characterBody.baseAcceleration < 100 && !LordZot.LordZot.flight && !characterMotor.isGrounded)
                { characterMotor.velocity.y -= 1f; };
                if (LordZot.LordZot.flight)
                {
                    base.characterMotor.airControl = 1f;

                    base.characterBody.baseMoveSpeed = 3f;
                    base.characterBody.baseAcceleration = 700;
                    LordZot.LordZot.Charged = 0f;
                    this.outer.SetNextStateToMain();
                };

                if (base.fixedAge >= BaseLeap.minimumDuration && ((base.characterMotor.Motor.GroundingStatus.IsStableOnGround && !base.characterMotor.Motor.LastGroundingStatus.IsStableOnGround)))
                {

                    if (!LordZot.LordZot.wellhefell) ;
                    {
                        PlayAnimation("Body", "AscendDescend 2", null, 1f);
                        LordZot.LordZot.wellhefell = true;
                        this.stopwatch = 1.6f;
                        base.characterMotor.velocity = Vector3.zero;
                    };
                    LordZot.LordZot.jumpcooldown = 0f;
                    base.characterMotor.airControl = 1f;

                    base.characterBody.baseMoveSpeed = 3f;
                    base.characterBody.baseAcceleration = 700;
                    LordZot.LordZot.Charged = 0f;





                }
                if ((this.stopwatch <= 0 && LordZot.LordZot.wellhefell) | base.inputBank.jump.down && LordZot.LordZot.wellhefell)
                {
                    //  Debug.Log("he fell");
                    LordZot.LordZot.wellhefell = false;
                    //    LordZot.LordZot.fallstun = false;
                    characterBody.baseMoveSpeed = 3f;
                    LordZot.LordZot.jumpcooldown = 0f;
                    base.characterMotor.airControl = this.previousAirControl;
                    base.characterMotor.velocity = Vector3.zero;
                    base.characterBody.baseMoveSpeed = 3f;
                    base.characterBody.baseAcceleration = 700f;
                    LordZot.LordZot.Charged = 0f;
                    this.outer.SetNextStateToMain();
                };


            }
        }




        public override void OnExit()
        {
            RoR2.Util.PlaySound(BaseLeap.soundLoopStopEvent, base.gameObject);
            if (base.isAuthority)
            {
                characterBody.baseMoveSpeed = 3f;
                LordZot.LordZot.wellhefell = false;
                //  LordZot.LordZot.fallstun = false;
            }
            characterBody.baseMoveSpeed = 3f;
            LordZot.LordZot.wellhefell = false;
            //   LordZot.LordZot.fallstun = false;
            base.characterMotor.airControl = this.previousAirControl;

            LordZot.LordZot.jumpcooldown = 0f;
            base.characterBody.baseMoveSpeed = 3f;
            base.characterBody.baseAcceleration = 700f;
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
            if (Time.fixedDeltaTime >= 0.9)
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
                LordZot.LordZot.Busy = true;
          //  if (LordZot.LordZot.Busy)
          //  { Debug.Log("Made Busy"); }
            this.stopwatch = 0f;
                this.earlyExitDuration = EldritchFury.baseEarlyExitDuration / this.attackSpeedStat;
                this.animator = base.GetModelAnimator();
                this.hasSwung = false;
                this.hasHopped = false;
            this.modelAnimator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();
            ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();

            
            if (component)
            {

                GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                Transform transform = component.FindChild("LeftHand");
                Transform transform2 = component.FindChild("RightHand");
                if (transform)
                {
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = transform.position;
                    effectData.scale = 1f;
                    RoR2.EffectManager.SpawnEffect(original, effectData, false);
                }
                if (transform2)
                {
                    RoR2.EffectData effectData = new RoR2.EffectData();
                    effectData.origin = transform2.position;
                    effectData.scale = 1f;
                    RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                }
            }

            {
              
                 }
                bool flag = base.skillLocator;
                if (flag)
                {
                    base.skillLocator.primary.skillDef.activationStateMachineName = "Weapon";
                }
                bool @bool = this.animator.GetBool("isMoving");
                bool bool2 = this.animator.GetBool("isGrounded");;
                switch (this.comboState)
                {
                    case EldritchFury.ComboState.Punch1:
                        {
                            this.attackDuration = EldritchFury.baseComboAttackDuration * 1f / this.attackSpeedStat;
                        if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                            RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat * 1f);
                        else
                            RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, 3f);
                        bool flag3 = @bool || !bool2;
                            if (flag3)
                            {
                            base.PlayCrossfade("Gesture, Additive", "FireArrow", "FireArrow.playbackRate", this.attackDuration, 0.2f / this.attackSpeedStat);
                            
                        }
                            else
                            {
                            base.PlayCrossfade("Gesture, Override", "FireArrow", "FireArrow.playbackRate", this.attackDuration, 0.2f / this.attackSpeedStat);
                        }
                        break;
                        }
                    case EldritchFury.ComboState.Punch2:
                        {
                            this.attackDuration = EldritchFury.baseComboAttackDuration / this.attackSpeedStat * 1f;
                        if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                            RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat * 1f);
                        else
                            RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, 3f);
                        bool flag4 = @bool || !bool2;
                            if (flag4)
                            {
                                base.PlayCrossfade("Gesture, Additive", "FireArrow2", "FireArrow.playbackRate", this.attackDuration, 0.2f / this.attackSpeedStat);
                            }
                            else
                            {
                            base.PlayCrossfade("Gesture, Override", "FireArrow2", "FireArrow.playbackRate", this.attackDuration, 0.2f / this.attackSpeedStat);
                            }
                        this.hitEffectPrefab = LoaderMeleeAttack.overchargeImpactEffectPrefab;
                        break;
                        }
                }
            //    base.characterBody.SetAimTimer(this.attackDuration + 1f);
            this.attackDuration = EldritchFury.baseComboAttackDuration / this.attackSpeedStat;

  

        }

            // Token: 0x0600006E RID: 110 RVA: 0x00006314 File Offset: 0x00004514
            public override void OnExit()
            {
            LordZot.LordZot.Busy = false;
           

            base.OnExit();
            }

            // Token: 0x0600006F RID: 111 RVA: 0x00006358 File Offset: 0x00004558
            /// <summary>
            /// 
            /// </summary>
            public override void FixedUpdate()
            {
                base.FixedUpdate();
                this.hitPauseTimer -= Time.fixedDeltaTime;
                bool isAuthority = base.isAuthority;
                if (isAuthority)
                {
                switch (this.comboState)
                {
                    case EldritchFury.ComboState.Punch1:
                        {
                            bool flageffect1 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
                            if (flageffect1)
                            {
                                this.modelAnimator = base.GetModelAnimator();
                                this.modelTransform = base.GetModelTransform();
                                ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                                if (component)
                                {


                                    GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                    GameObject original3 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                    GameObject original4 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                                    Transform transform2 = component.FindChild("RightHand");
                                  
                                    if (transform2)
                                    {
                                        Color32 color = new Color32();
                                        color.a = 255;
                                        color.r = 255;
                                        color.g = 0;
                                        color.b = 0;
                                        
                                        var position = transform2.position;
                                        RoR2.EffectData effectData = new RoR2.EffectData();
                                        effectData.origin = position;
                                        effectData.scale = 45f;
                                        effectData.color = color;
                                    
                                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                                        effectData2.origin = position;
                                        effectData2.scale = 10f;
                                        effectData2.color = color;
                                        RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                                        RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                                        var position2 = base.characterBody.footPosition;
                                        RoR2.EffectData effectData3 = new RoR2.EffectData();
                                        effectData3.origin = position2;
                                        effectData3.scale = 6f;
                                        RoR2.EffectManager.SpawnEffect(original4, effectData3, false);
                                    }
                                };
                            };

                            break;
                        }
                    case EldritchFury.ComboState.Punch2:
                        {
                            bool flageffect1 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
                            if (flageffect1)
                            {
                                this.modelAnimator = base.GetModelAnimator();
                                this.modelTransform = base.GetModelTransform();
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
                                        effectData.scale = 6f;
                                        RoR2.EffectManager.SpawnEffect(original, effectData, false);

                                        RoR2.EffectData effectData2 = new RoR2.EffectData();
                                        effectData2.origin = position;
                                        effectData2.scale = 45f;
                                        
                                        RoR2.EffectManager.SpawnEffect(original2, effectData2, false);
                                        var position2 = base.characterBody.footPosition;
                                        RoR2.EffectData effectData3 = new RoR2.EffectData();
                                        effectData3.origin = position2;
                                        effectData3.scale = 6f;
                                        RoR2.EffectManager.SpawnEffect(original4, effectData3, false);
                                    }
                                };
                            };
                            break;
                        }
                }



                bool flag = base.FireMeleeOverlap(this.overlapAttack, this.animator, "FuryHitGroup.active", EldritchFury.forceMagnitude, true);
                this.hasHit = (this.hasHit || flag);
                bool flag2 = flag;
                if (flag2)
                {

                    bool flag9 = this.comboState == EldritchFury.ComboState.Punch2;

                    if (flag9) ;

                    //    {
                    //  if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                    //        RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat);
                    //    else
                    //        RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, 3f);
                    // }
                    //      else
                    //      {
                    //     if (this.attackSpeedStat < attackspeedaltsoundthreshold)
                    //          RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, this.attackSpeedStat);
                    //      else
                    //          RoR2.Util.PlayScaledSound(ClapState.attackSoundString, base.gameObject, 3f);
                    //  }

                    bool flag11 = !this.hasHopped;
                        if (flag11)
                        {
                            bool flag12 = base.characterMotor && !base.characterMotor.isGrounded;
                            if (flag12)
                            {
                                base.SmallHop(base.characterMotor, EldritchFury.hitHopVelocity);
                            }
                            this.hasHopped = true;
                        }
                        bool flag15 = !this.isInHitPause;
                        if (flag15)
                        {
                            this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "FireArrow.playbackRate");
                            this.hitPauseTimer = EldritchFury.hitPauseDuration / this.attackSpeedStat;
                            this.isInHitPause = true;
                        }
                    }
                    bool flag16 = this.hitPauseTimer <= 0f && this.isInHitPause;
                    if (flag16)
                    {
                        base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                        this.isInHitPause = false;
                    }
                }
                bool flag17 = this.stopwatch >= this.attackDuration * 0.55f && !this.hasSwung;
                if (flag17)
                {

                    this.hasSwung = true;


                //     this.animator.SetFloat("Sword.active", 1f);
            }
                else
                {
               //     this.animator.SetFloat("Sword.active", 0f);
                }
                bool flag20 = !this.isInHitPause;
                if (flag20)
                {
                    this.stopwatch += Time.fixedDeltaTime;
                }
                else
                {
                    bool flag21 = base.characterMotor;
                    if (flag21)
                    {
                        base.characterMotor.velocity = Vector3.zero;
                    }
                    bool flag22 = this.animator;
                    if (flag22)
                    {
                        this.animator.SetFloat("FireArrow.playbackRate", 0f);
                    }
                }
                bool flag23 = base.isAuthority && this.stopwatch >= this.attackDuration * 0.55f;
                if (flag23)
                {
                    bool flag24 = !this.hasSwung;
                    if (flag24)
                    {
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
                        LandingBlast.baseDamage = characterBody.damage * 9f;
                        LandingBlast.baseForce = 3500f;
                        LandingBlast.position = position;
                        LandingBlast.radius = 28f;
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
                        RoR2.EffectManager.SpawnEffect(sonicboom, sonicboomeffectdata, false);

                    }
                    }
                    bool flag26 = base.inputBank.skill1.down && this.comboState != EldritchFury.ComboState.Punch2 && this.stopwatch >= this.attackDuration * 0.65f;
                    if (flag26)
                    {
                       EldritchFury eldritchFury = new EldritchFury();
                        eldritchFury.comboState = this.comboState + 1;
                        this.outer.SetNextState(eldritchFury);
                    LordZot.LordZot.timeRemaining = 6.0f;
                    return;
                    }
                bool flag2555 = base.inputBank.skill1.down && this.comboState != EldritchFury.ComboState.Punch1 && this.stopwatch >= this.attackDuration * 0.61f;
                if (flag2555)
                {
                    EldritchFury eldritchFury2 = new EldritchFury();
                    eldritchFury2.comboState = this.comboState - 1;
                    this.outer.SetNextState(eldritchFury2);
                    LordZot.LordZot.timeRemaining = 6.0f;
                    return;
                }
                bool flag27 = this.stopwatch >= this.attackDuration;
                    if (flag27)
                {
                    LordZot.LordZot.Busy = false;
                   // Debug.Log("busy false");
                    this.outer.SetNextStateToMain();
                        return;
                    }
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
                                    
                                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                                        if (component)
                                        {


                                            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                            GameObject original3 = Resources.Load<GameObject>("prefabs/effects/impacteffects/FusionCellExplosion");
                                            GameObject original4 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");
                                            Transform transform2 = component.FindChild("RightHand");

                                            if (transform2)
                                            {
                                                Color32 color = new Color32();
                                                color.a = 255;
                                                color.r = 255;
                                                color.g = 0;
                                                color.b = 0;

                                                var position3 = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                                                RoR2.EffectData effectData = new RoR2.EffectData();
                                                effectData.origin = position3;
                                                effectData.scale = 45f;
                                                effectData.color = color;

                                                RoR2.EffectData effectData2 = new RoR2.EffectData();
                                                effectData2.origin = position3;
                                                effectData2.scale = 10f;
                                                effectData2.color = color;
                                                RoR2.EffectManager.SpawnEffect(original2, effectData, false);
                                                RoR2.EffectManager.SpawnEffect(original3, effectData2, false);
                                                var position2 = base.characterBody.footPosition;
                                                RoR2.EffectData effectData3 = new RoR2.EffectData();
                                                effectData3.origin = position2;
                                                effectData3.scale = 6f;
                                                RoR2.EffectManager.SpawnEffect(original4, effectData3, false);
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
                                      
                                        ChildLocator component = this.modelTransform.GetComponent<ChildLocator>();


                                        if (component)
                                        {

                                            GameObject original = Resources.Load<GameObject>("prefabs/effects/impacteffects/VagrantCannonExplosion");
                                            GameObject original2 = Resources.Load<GameObject>("prefabs/effects/impacteffects/IgniteExplosionVFX");
                                            GameObject original4 = Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact");

                                            Transform transform = component.FindChild("LeftHand");

                                            if (transform)
                                            {
                                                var position3 = base.characterBody.corePosition + (base.characterDirection.forward * 6f);
                                                RoR2.EffectData effectData = new RoR2.EffectData();
                                                effectData.origin = position3;
                                                effectData.scale = 6f;
                                                RoR2.EffectManager.SpawnEffect(original, effectData, false);

                                                RoR2.EffectData effectData2 = new RoR2.EffectData();
                                                effectData2.origin = position3;
                                                effectData2.scale = 45f;

                                                RoR2.EffectManager.SpawnEffect(original2, effectData2, false);
                                                var position2 = base.characterBody.footPosition;
                                                RoR2.EffectData effectData3 = new RoR2.EffectData();
                                                effectData3.origin = position2;
                                                effectData3.scale = 6f;
                                                RoR2.EffectManager.SpawnEffect(original4, effectData3, false);
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
                    LandingBlast.baseDamage = characterBody.damage * 9f;
                    LandingBlast.baseForce = 3500f;
                    LandingBlast.position = position;
                    LandingBlast.radius = 28f;
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

                LordZot.LordZot.inair = false;
                this.outer.SetNextStateToMain();
                return;
            }
            if (!airq)
            { LordZot.LordZot.inair = true; };
            
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
            private float attackDuration;

            // Token: 0x0400009B RID: 155
            private float earlyExitDuration;

            // Token: 0x0400009C RID: 156
            private Animator animator;
        public static float recoilAmplitude = 7f;
            // Token: 0x0400009D RID: 157
            private RoR2.OverlapAttack overlapAttack;

            // Token: 0x0400009E RID: 158
            private float hitPauseTimer;
        public GameObject explodePrefab = Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");
        // Token: 0x0400009F RID: 159
        private bool isInHitPause;
        private RoR2.ShakeEmitter shakeEmitter;
        // Token: 0x040000A0 RID: 160
        private bool hasSwung;

            // Token: 0x040000A1 RID: 161
            private bool hasHit;

            // Token: 0x040000A2 RID: 162
            private bool hasHopped;
        private Animator modelAnimator;
        private Transform modelTransform;

        // Token: 0x040000A3 RID: 163
        public EldritchFury.ComboState comboState;

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
        }




    }











   