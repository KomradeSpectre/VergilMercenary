// Decompiled with JetBrains decompiler
// Type: VergilMerc.VergilMerc
// Assembly: VergilMerc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2237CD1-6B13-451D-BD6A-1B885E247906
// Assembly location: C:\Users\Jonathan Church\Downloads\rob-VergilMercenary-1.2.1\VergilMerc.dll

//using Aatrox;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using EntityStates;
//using IL.RoR2;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using VergilMercenary;
//using VergilMercenary.EntityStates;

namespace VergilMercenary
{
    [BepInDependency("com.rob.Aatrox", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.bepis.r2api")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod)]
    [BepInPlugin("com.rob.VergilMercenary", "VergilMercenary", "1.2.2")]
    [R2APISubmoduleDependency(new string[] { "PrefabAPI", "SurvivorAPI", "LoadoutAPI", "LanguageAPI", "ResourcesAPI" })]
    public class VergilMerc : BaseUnityPlugin
    {
        public const string MODUID = "com.rob.VergilMercenary";
        public AssetBundle mainAssetBundle;
        public static ConfigEntry<bool> vergilSkin;
        public static ConfigEntry<bool> vergilExSkin;
        public static ConfigEntry<bool> vergilCoatlessSkin;
        public static ConfigEntry<bool> vergilCoatlessExSkin;
        public static ConfigEntry<bool> vergilAltSkin;
        public static ConfigEntry<KeyCode> tauntKey;
        public static ConfigEntry<KeyCode> altTauntKey;
        public static int musicActive = 0;
        private bool hasAatrox = false;

        private void Awake()
        {
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VergilMercenary.VergilMerc"))
			{
				mainAssetBundle = AssetBundle.LoadFromStream(manifestResourceStream);
			}

			Init();
        }

        private void Init()
        {
			CreateLang();
			CreateConfig();
			CheckCompats();
			RegisterSkins();
        }

        private void CreateLang()
        {
			LanguageAPI.Add("MERC_VERGIL_SKIN", "Vergil");
			LanguageAPI.Add("MERC_VERGIL_EX_SKIN", "Vergil EX");
			LanguageAPI.Add("MERC_VERGIL_COATLESS_SKIN", "Vergil (Coatless)");
			LanguageAPI.Add("MERC_VERGIL_COATLESSEX_SKIN", "Vergil EX (Coatless)");
			LanguageAPI.Add("MERC_VERGIL_ALT_SKIN", "The Motivator");
		}

		private void CreateConfig()
		{
			VergilMerc.vergilSkin = base.Config.Bind<bool>(new ConfigDefinition("02 - Skins", "Vergil"), true, new ConfigDescription("Enables Vergil skin", null, Array.Empty<object>()));
			VergilMerc.vergilExSkin = base.Config.Bind<bool>(new ConfigDefinition("02 - Skins", "Vergil EX"), true, new ConfigDescription("Enables Vergil EX skin", null, Array.Empty<object>()));
			VergilMerc.vergilCoatlessSkin = base.Config.Bind<bool>(new ConfigDefinition("02 - Skins", "Vergil (Coatless)"), true, new ConfigDescription("Enables Vergil (Coatless) skin", null, Array.Empty<object>()));
			VergilMerc.vergilCoatlessExSkin = base.Config.Bind<bool>(new ConfigDefinition("02 - Skins", "Vergil EX (Coatless)"), false, new ConfigDescription("Enables Vergil EX (Coatless) skin", null, Array.Empty<object>()));
			VergilMerc.vergilAltSkin = base.Config.Bind<bool>(new ConfigDefinition("02 - Skins", "The Motivator"), true, new ConfigDescription("Enables the Motivator skin (concept by Glasus)", null, Array.Empty<object>()));
			VergilMerc.tauntKey = base.Config.Bind<KeyCode>(new ConfigDefinition("01 - Keybinds", "Taunt Keybind"), (KeyCode)122, new ConfigDescription("Key used to taunt and toggle Bury the Light(only usable with Aatrox installed)", null, Array.Empty<object>()));
			VergilMerc.altTauntKey = base.Config.Bind<KeyCode>(new ConfigDefinition("01 - Keybinds", "Alt Taunt Keybind"), (KeyCode)120, new ConfigDescription("Key used to taunt and toggle Devil Trigger(only usable with Aatrox installed)", null, Array.Empty<object>()));
		}

		private void CheckCompats()
        {
			/*if (Chainloader.PluginInfos.ContainsKey("com.rob.Aatrox")) { hasAatrox = true; }

            if (hasAatrox)
            {
				IL.RoR2.MusicController.LateUpdate += delegate (ILContext il)
				{
					ILCursor ilcursor = new ILCursor(il);
					ILCursor ilcursor2 = ilcursor;
					Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
					array[0] = delegate (Instruction i)
					{
						int num;
						return ILPatternMatchingExt.MatchStloc(i, 0);
					};
					ilcursor2.GotoNext(array);
					ilcursor.EmitDelegate<Func<bool, bool>>((bool b) => b || VergilMerc.musicActive != 0);
				};
			}*/
		}

		private void RegisterSkins()
        {
			/*if (this.hasAatrox)
            {
                // ISSUE: method pointer
                On.RoR2.SurvivorCatalog.Init += delegate (On.RoR2.SurvivorCatalog.orig_Init orig)
                {
                    orig.Invoke();
                    this.AddSkins();
                    this.VergilSetup();
                };
            }
            else
            {
				On.RoR2.SurvivorCatalog.Init += delegate (On.RoR2.SurvivorCatalog.orig_Init orig)
                {
                    orig.Invoke();
                    this.AddSkins();
                };
            }*/

			On.RoR2.SurvivorCatalog.Init += delegate (On.RoR2.SurvivorCatalog.orig_Init orig)
			{
				orig.Invoke();
				AddSkins();
			};
		}

        /*private void VergilSetup()
        {
            LoadoutAPI.AddSkill(typeof(MainState));
            LoadoutAPI.AddSkill(typeof(BaseEmote));
            LoadoutAPI.AddSkill(typeof(Taunt));
            LoadoutAPI.AddSkill(typeof(AltTaunt));
            GameObject gameObject = Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody");
            gameObject.GetComponentInChildren<RoR2.EntityStateMachine>().mainStateType = new SerializableEntityStateType(typeof(MainState));
            gameObject.AddComponent<GenericStyleController>();
            gameObject.AddComponent<VergilController>();
        }*/

        private void AddSkins()
        {

            #region Find Commando Body

            RoR2.SurvivorDef survivorDef = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.FindSurvivorIndex("Merc"));
			GameObject bodyPrefab = survivorDef.bodyPrefab;
			Renderer[] componentsInChildren = bodyPrefab.GetComponentsInChildren<Renderer>(true);
            RoR2.ModelSkinController componentInChildren = bodyPrefab.GetComponentInChildren<ModelSkinController>();
			GameObject gameObject = componentInChildren.gameObject;
			Material defaultMaterial = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            #endregion

            #region Set Up Materials

            Material VergilMaterial = UnityEngine.Object.Instantiate<Material>(defaultMaterial);
			VergilMaterial.SetColor("_Color", Color.white);
			VergilMaterial.SetTexture("_MainTex", this.mainAssetBundle.LoadAsset<Material>("matVergil").GetTexture("_MainTex"));
			VergilMaterial.SetColor("_EmColor", Color.white);
			VergilMaterial.SetFloat("_EmPower", 0.2f);
			VergilMaterial.SetTexture("_EmTex", this.mainAssetBundle.LoadAsset<Material>("matVergil").GetTexture("_EmissionMap"));
			VergilMaterial.SetFloat("_NormalStrength", 0f);

			Material VergilEXMaterial = UnityEngine.Object.Instantiate<Material>(defaultMaterial);
			VergilEXMaterial.SetColor("_Color", Color.white);
			VergilEXMaterial.SetTexture("_MainTex", this.mainAssetBundle.LoadAsset<Material>("matVergilEX").GetTexture("_MainTex"));
			VergilEXMaterial.SetColor("_EmColor", Color.white);
			VergilEXMaterial.SetFloat("_EmPower", 0f);
			VergilEXMaterial.SetFloat("_NormalStrength", 0f);

			Material VergilAltMaterial = UnityEngine.Object.Instantiate<Material>(defaultMaterial);
			VergilAltMaterial.SetColor("_Color", Color.white);
			VergilAltMaterial.SetTexture("_MainTex", this.mainAssetBundle.LoadAsset<Material>("matVergilAlt").GetTexture("_MainTex"));
			VergilAltMaterial.SetColor("_EmColor", Color.white);
			VergilAltMaterial.SetFloat("_EmPower", 1f);
			VergilAltMaterial.SetTexture("_EmTex", this.mainAssetBundle.LoadAsset<Material>("matVergilAlt").GetTexture("_EmissionMap"));
			VergilAltMaterial.SetFloat("_NormalStrength", 0f);

			Material YamatoMaterial = UnityEngine.Object.Instantiate<Material>(defaultMaterial);
			YamatoMaterial.SetColor("_Color", Color.white);
			YamatoMaterial.SetTexture("_MainTex", this.mainAssetBundle.LoadAsset<Material>("matYamato").GetTexture("_MainTex"));
			YamatoMaterial.SetColor("_EmColor", Color.white);
			YamatoMaterial.SetFloat("_EmPower", 0f);
			YamatoMaterial.SetFloat("_NormalStrength", 0f);

            #endregion

            #region Create Vergil Skin
            LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
			skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.145f, 0.27f, 0.423f), new Color(0.62f, 0.62f, 0.62f), new Color(0.211f, 0.149f, 0.109f), new Color(0.5333f, 0.415f, 0.215f));
			skinDefInfo.Name = "MercVergil";
			skinDefInfo.NameToken = "MERC_VERGIL_SKIN";
			skinDefInfo.RootObject = gameObject;
			skinDefInfo.UnlockableDef = null;
			skinDefInfo.BaseSkins = new SkinDef[0];
			skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
			CharacterModel.RendererInfo[] array = new CharacterModel.RendererInfo[2];
			int num = 0;
			CharacterModel.RendererInfo rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = VergilMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[3];
			array[num] = rendererInfo;
			int num2 = 1;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = YamatoMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[4];
			array[num2] = rendererInfo;
			skinDefInfo.RendererInfos = array;
			SkinDef.MeshReplacement[] array2 = new SkinDef.MeshReplacement[2];
			int num3 = 0;
			SkinDef.MeshReplacement meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("VergilMesh");
			meshReplacement.renderer = componentsInChildren[3];
			array2[num3] = meshReplacement;
			int num4 = 1;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("YamatoMesh");
			meshReplacement.renderer = componentsInChildren[4];
			array2[num4] = meshReplacement;
			skinDefInfo.MeshReplacements = array2;
			skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
			skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];

            #endregion

            #region Create EX Vergil Skin

            LoadoutAPI.SkinDefInfo skinDefInfo2 = skinDefInfo;
			skinDefInfo = default(LoadoutAPI.SkinDefInfo);
			skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.15f, 0.15f, 0.15f), new Color(0.62f, 0.62f, 0.62f), new Color(0.15f, 0.19f, 0.19f), new Color(0.39f, 0.427f, 0.459f));
			skinDefInfo.Name = "MercVergilEX";
			skinDefInfo.NameToken = "MERC_VERGIL_EX_SKIN";
			skinDefInfo.RootObject = gameObject;
			skinDefInfo.UnlockableDef = null;
			skinDefInfo.BaseSkins = new SkinDef[0];
			skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
			CharacterModel.RendererInfo[] array3 = new CharacterModel.RendererInfo[2];
			int num5 = 0;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = VergilEXMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[3];
			array3[num5] = rendererInfo;
			int num6 = 1;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = YamatoMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[4];
			array3[num6] = rendererInfo;
			skinDefInfo.RendererInfos = array3;
			SkinDef.MeshReplacement[] array4 = new SkinDef.MeshReplacement[2];
			int num7 = 0;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("VergilEXMesh");
			meshReplacement.renderer = componentsInChildren[3];
			array4[num7] = meshReplacement;
			int num8 = 1;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("YamatoMesh");
			meshReplacement.renderer = componentsInChildren[4];
			array4[num8] = meshReplacement;
			skinDefInfo.MeshReplacements = array4;
			skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
			skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];

            #endregion

            #region Create Coatless Vergil Skin

            LoadoutAPI.SkinDefInfo skinDefInfo3 = skinDefInfo;
			skinDefInfo = default(LoadoutAPI.SkinDefInfo);
			skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.09f, 0.09f, 0.156f), new Color(0.62f, 0.62f, 0.62f), new Color(0.211f, 0.149f, 0.109f), new Color(0.89f, 0.713f, 0.56f));
			skinDefInfo.Name = "MercVergilCoatless";
			skinDefInfo.NameToken = "MERC_VERGIL_COATLESS_SKIN";
			skinDefInfo.RootObject = gameObject;
			skinDefInfo.UnlockableDef = null;
			skinDefInfo.BaseSkins = new SkinDef[0];
			skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
			CharacterModel.RendererInfo[] array5 = new CharacterModel.RendererInfo[2];
			int num9 = 0;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = VergilMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[3];
			array5[num9] = rendererInfo;
			int num10 = 1;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = YamatoMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[4];
			array5[num10] = rendererInfo;
			skinDefInfo.RendererInfos = array5;
			SkinDef.MeshReplacement[] array6 = new SkinDef.MeshReplacement[2];
			int num11 = 0;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("VergilCoatlessMesh");
			meshReplacement.renderer = componentsInChildren[3];
			array6[num11] = meshReplacement;
			int num12 = 1;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("YamatoMesh");
			meshReplacement.renderer = componentsInChildren[4];
			array6[num12] = meshReplacement;
			skinDefInfo.MeshReplacements = array6;
			skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
			skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];

            #endregion

            #region Create Coatless EX Vergil Skin

            LoadoutAPI.SkinDefInfo skinDefInfo4 = skinDefInfo;
			skinDefInfo = default(LoadoutAPI.SkinDefInfo);
			skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.09f, 0.09f, 0.156f), new Color(0.62f, 0.62f, 0.62f), new Color(0.211f, 0.149f, 0.109f), new Color(0.89f, 0.713f, 0.56f));
			skinDefInfo.Name = "MercVergilCoatlessEx";
			skinDefInfo.NameToken = "MERC_VERGIL_COATLESSEX_SKIN";
			skinDefInfo.RootObject = gameObject;
			skinDefInfo.UnlockableDef = null;
			skinDefInfo.BaseSkins = new SkinDef[0];
			skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
			CharacterModel.RendererInfo[] array7 = new CharacterModel.RendererInfo[2];
			int num13 = 0;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = VergilEXMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[3];
			array7[num13] = rendererInfo;
			int num14 = 1;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = YamatoMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[4];
			array7[num14] = rendererInfo;
			skinDefInfo.RendererInfos = array7;
			SkinDef.MeshReplacement[] array8 = new SkinDef.MeshReplacement[2];
			int num15 = 0;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("VergilCoatlessMesh");
			meshReplacement.renderer = componentsInChildren[3];
			array8[num15] = meshReplacement;
			int num16 = 1;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("YamatoMesh");
			meshReplacement.renderer = componentsInChildren[4];
			array8[num16] = meshReplacement;
			skinDefInfo.MeshReplacements = array8;
			skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
			skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];

            #endregion

            #region Create Alt Vergil Skin
            LoadoutAPI.SkinDefInfo skinDefInfo5 = skinDefInfo;
			skinDefInfo = default(LoadoutAPI.SkinDefInfo);
			skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.145f, 0.27f, 0.423f), new Color(0.62f, 0.62f, 0.62f), new Color(0.211f, 0.149f, 0.109f), new Color(0.5333f, 0.415f, 0.215f));
			skinDefInfo.Name = "MercVergilAlt";
			skinDefInfo.NameToken = "MERC_VERGIL_ALT_SKIN";
			skinDefInfo.RootObject = gameObject;
			skinDefInfo.UnlockableDef = null;
			skinDefInfo.BaseSkins = new SkinDef[0];
			skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[0];
			CharacterModel.RendererInfo[] array9 = new CharacterModel.RendererInfo[2];
			int num17 = 0;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = VergilAltMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[3];
			array9[num17] = rendererInfo;
			int num18 = 1;
			rendererInfo = default(CharacterModel.RendererInfo);
			rendererInfo.defaultMaterial = YamatoMaterial;
			rendererInfo.defaultShadowCastingMode = ShadowCastingMode.On;
			rendererInfo.ignoreOverlays = false;
			rendererInfo.renderer = componentsInChildren[4];
			array9[num18] = rendererInfo;
			skinDefInfo.RendererInfos = array9;
			SkinDef.MeshReplacement[] array10 = new SkinDef.MeshReplacement[2];
			int num19 = 0;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("VergilAltMesh");
			meshReplacement.renderer = componentsInChildren[3];
			array10[num19] = meshReplacement;
			int num20 = 1;
			meshReplacement = default(SkinDef.MeshReplacement);
			meshReplacement.mesh = this.mainAssetBundle.LoadAsset<Mesh>("YamatoMesh");
			meshReplacement.renderer = componentsInChildren[4];
			array10[num20] = meshReplacement;
			skinDefInfo.MeshReplacements = array10;
			skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
			skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];

            #endregion

            #region Slot Skins into Merc Loadout
            LoadoutAPI.SkinDefInfo skinDefInfo6 = skinDefInfo;
			bool value = VergilMerc.vergilSkin.Value;
			if (value)
			{
				Array.Resize<SkinDef>(ref componentInChildren.skins, componentInChildren.skins.Length + 1);
				componentInChildren.skins[componentInChildren.skins.Length - 1] = LoadoutAPI.CreateNewSkinDef(skinDefInfo2);
			}
			bool value2 = VergilMerc.vergilExSkin.Value;
			if (value2)
			{
				Array.Resize<SkinDef>(ref componentInChildren.skins, componentInChildren.skins.Length + 1);
				componentInChildren.skins[componentInChildren.skins.Length - 1] = LoadoutAPI.CreateNewSkinDef(skinDefInfo3);
			}
			bool value3 = VergilMerc.vergilCoatlessSkin.Value;
			if (value3)
			{
				Array.Resize<SkinDef>(ref componentInChildren.skins, componentInChildren.skins.Length + 1);
				componentInChildren.skins[componentInChildren.skins.Length - 1] = LoadoutAPI.CreateNewSkinDef(skinDefInfo4);
			}
			bool value4 = VergilMerc.vergilCoatlessExSkin.Value;
			if (value4)
			{
				Array.Resize<SkinDef>(ref componentInChildren.skins, componentInChildren.skins.Length + 1);
				componentInChildren.skins[componentInChildren.skins.Length - 1] = LoadoutAPI.CreateNewSkinDef(skinDefInfo5);
			}
			bool value5 = VergilMerc.vergilAltSkin.Value;
			if (value5)
			{
				Array.Resize<SkinDef>(ref componentInChildren.skins, componentInChildren.skins.Length + 1);
				componentInChildren.skins[componentInChildren.skins.Length - 1] = LoadoutAPI.CreateNewSkinDef(skinDefInfo6);
			}
			SkinDef[][] fieldValue = Reflection.GetFieldValue<SkinDef[][]>(typeof(RoR2.BodyCatalog), "skins");
			fieldValue[(int)RoR2.BodyCatalog.FindBodyIndex(bodyPrefab)] = componentInChildren.skins;
			Reflection.SetFieldValue<SkinDef[][]>(typeof(RoR2.BodyCatalog), "skins", fieldValue);

            #endregion
        }
    }
}

