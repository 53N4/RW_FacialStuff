﻿namespace FacialStuff
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using JetBrains.Annotations;

    using UnityEngine;

    using Verse;

    [StaticConstructorOnStartup]

    // ReSharper disable once InconsistentNaming
    public static class CutHairDB
    {
        #region Private Fields

        [NotNull]
        private static readonly Dictionary<GraphicRequest, Graphic> AllGraphics =
            new Dictionary<GraphicRequest, Graphic>();

        [NotNull]
        private static readonly List<HairCutPawn> PawnHairCache = new List<HairCutPawn>();

        [CanBeNull]
        private static Texture2D maskTexFrontBack;

        [CanBeNull]
        private static Texture2D maskTexSide;

        [CanBeNull]
        private static string modPath = null;

        #endregion Private Fields

        #region Private Properties

        [CanBeNull]
        private static string ModPath
        {
            get
            {
                if (modPath == null)
                {
                    ModMetaData mod =
                        ModLister.AllInstalledMods.FirstOrDefault(x => x.Active && x.Name.StartsWith("Facial Stuff"));
                    modPath = mod.RootDir + "/Textures/MergedHair/";
                }

                return modPath;
            }
        }

        #endregion Private Properties

        #region Public Methods

        // ReSharper disable once MissingXmlDoc
        public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color)
            where T : Graphic, new()
        {
            // Added second 'color' to get a separate graphic
            GraphicRequest req = new GraphicRequest(typeof(T), path, shader, drawSize, color, color, null, 0);
            return GetInner<T>(req);
        }

        [NotNull]
        public static HairCutPawn GetHairCache([NotNull] Pawn pawn)
        {
            foreach (HairCutPawn c in PawnHairCache)
            {
                if (c.Pawn == pawn)
                {
                    return c;
                }
            }

            HairCutPawn n = new HairCutPawn { Pawn = pawn };
            PawnHairCache.Add(n);
            return n;
        }

        #endregion Public Methods

        #region Private Methods

        private static void CutOutHair([NotNull] ref Texture2D hairTex, [NotNull] Texture2D maskTex)
        {
            for (int x = 0; x < hairTex.width; x++)
            {
                for (int y = 0; y < hairTex.height; y++)
                {
                    Color maskColor = maskTex.GetPixel(x, y);
                    Color hairColor = hairTex.GetPixel(x, y);

                    Color finalColor1 = hairColor * maskColor;

                    hairTex.SetPixel(x, y, finalColor1);
                }
            }

            hairTex.Apply();
        }

        [NotNull]
        private static T GetInner<T>(GraphicRequest req)
            where T : Graphic, new()
        {
            if (Controller.settings.UseCaching)
            {
                string oldPath = req.path;
                string name = Path.GetFileNameWithoutExtension(oldPath);

                req.path = ModPath + name;
                if (!AllGraphics.TryGetValue(req, out Graphic graphic))
                {
                    graphic = Activator.CreateInstance<T>();

                    // // Check if textures already present and readable, else create
                    if (ContentFinder<Texture2D>.Get(req.path + "_back", false) != null)
                    {
                        graphic.Init(req);

                        // graphic.MatFront.mainTexture = ContentFinder<Texture2D>.Get(newPath + "_front");
                        // graphic.MatSide.mainTexture = ContentFinder<Texture2D>.Get(newPath + "_side");
                        // graphic.MatBack.mainTexture = ContentFinder<Texture2D>.Get(newPath + "_back");
                    }
                    else
                    {
                        req.path = oldPath;
                        graphic.Init(req);

                        Texture2D temptexturefront = graphic.MatFront.mainTexture as Texture2D;
                        Texture2D temptextureside = graphic.MatSide.mainTexture as Texture2D;
                        Texture2D temptextureback = graphic.MatBack.mainTexture as Texture2D;

                        temptexturefront = FacialGraphics.MakeReadable(temptexturefront);
                        temptextureside = FacialGraphics.MakeReadable(temptextureside);
                        temptextureback = FacialGraphics.MakeReadable(temptextureback);

                        // intentional, only 1 mask tex. todo: rename, cleanup
                        maskTexFrontBack = FacialGraphics.MaskTex_Average_FrontBack;
                        maskTexSide = FacialGraphics.MaskTex_Narrow_Side;

                        CutOutHair(ref temptexturefront, maskTexFrontBack);

                        CutOutHair(ref temptextureside, maskTexSide);

                        CutOutHair(ref temptextureback, maskTexFrontBack);

                        req.path = ModPath + name;

                        if (!name.NullOrEmpty() && !File.Exists(req.path + "_front.png"))
                        {
                            byte[] bytes = temptexturefront.EncodeToPNG();
                            File.WriteAllBytes(req.path + "_front.png", bytes);
                            byte[] bytes2 = temptextureside.EncodeToPNG();
                            File.WriteAllBytes(req.path + "_side.png", bytes2);
                            byte[] bytes3 = temptextureback.EncodeToPNG();
                            File.WriteAllBytes(req.path + "_back.png", bytes3);
                        }

                        temptexturefront.Compress(true);
                        temptextureside.Compress(true);
                        temptextureback.Compress(true);

                        temptexturefront.mipMapBias = 0.5f;
                        temptextureside.mipMapBias = 0.5f;
                        temptextureback.mipMapBias = 0.5f;

                        temptexturefront.Apply(false, true);
                        temptextureside.Apply(false, true);
                        temptextureback.Apply(false, true);

                        graphic.MatFront.mainTexture = temptexturefront;
                        graphic.MatSide.mainTexture = temptextureside;
                        graphic.MatBack.mainTexture = temptextureback;

                        // Object.Destroy(temptexturefront);
                        // Object.Destroy(temptextureside);
                        // Object.Destroy(temptextureback);
                    }

                    AllGraphics.Add(req, graphic);

                    // }
                }

                return (T)graphic;
            }
            else
            {
                string oldPath = req.path;
                string name = Path.GetFileNameWithoutExtension(oldPath);

                if (!AllGraphics.TryGetValue(req, out Graphic graphic))
                {
                    graphic = Activator.CreateInstance<T>();

                    // Check if textures already present and readable, else create
                    if (ContentFinder<Texture2D>.Get(ModPath + name + "_back", false) != null)
                    {
                        req.path = ModPath + name;
                        graphic.Init(req);
                    }
                    else
                    {
                        graphic.Init(req);

                        Texture2D temptexturefront = graphic.MatFront.mainTexture as Texture2D;
                        Texture2D temptextureside = graphic.MatSide.mainTexture as Texture2D;
                        Texture2D temptextureback = graphic.MatBack.mainTexture as Texture2D;

                        temptexturefront = FacialGraphics.MakeReadable(temptexturefront);
                        temptextureside = FacialGraphics.MakeReadable(temptextureside);
                        temptextureback = FacialGraphics.MakeReadable(temptextureback);

                        // intentional, only 1 mask tex. todo: rename, cleanup
                        maskTexFrontBack = FacialGraphics.MaskTex_Average_FrontBack;
                        maskTexSide = FacialGraphics.MaskTex_Narrow_Side;

                        CutOutHair(ref temptexturefront, maskTexFrontBack);

                        CutOutHair(ref temptextureside, maskTexSide);

                        CutOutHair(ref temptextureback, maskTexFrontBack);

                        temptexturefront.Compress(true);
                        temptextureside.Compress(true);
                        temptextureback.Compress(true);

                        temptexturefront.mipMapBias = 0.5f;
                        temptextureside.mipMapBias = 0.5f;
                        temptextureback.mipMapBias = 0.5f;

                        temptexturefront.Apply(false, true);
                        temptextureside.Apply(false, true);
                        temptextureback.Apply(false, true);

                        graphic.MatFront.mainTexture = temptexturefront;
                        graphic.MatSide.mainTexture = temptextureside;
                        graphic.MatBack.mainTexture = temptextureback;

                        // Object.Destroy(temptexturefront);
                        // Object.Destroy(temptextureside);
                        // Object.Destroy(temptextureback);
                    }

                    AllGraphics.Add(req, graphic);

                    // }
                }

                return (T)graphic;
            }
        }

        #endregion Private Methods
    }
}
