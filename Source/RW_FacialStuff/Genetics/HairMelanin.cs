﻿namespace FacialStuff.Genetics
{
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using RimWorld;
    using UnityEngine;
    using Verse;

    public static class HairMelanin
    {

        #region Public Fields

        public static List<Color> ArtificialHairColors;

        public static List<Color> NaturalHairColors;

        #endregion Public Fields

        #region Private Fields

        [NotNull]
        private static readonly Gradient GradientEuMelanin;

        [NotNull]
        private static readonly Gradient GradientPheoMelanin;
        private static readonly Color HairBlueSteel = new Color32(57, 115, 199, 255);
        private static readonly Color HairBurgundyBistro = new Color32(206, 38, 58, 255);
        private static readonly Color HairDarkBrown = new Color32(79, 47, 17, 255);
        private static readonly Color HairGreenGrape = new Color32(124, 189, 14, 255);
        private static readonly Color HairMidnightBlack = new Color32(30, 30, 30, 255);
        private static readonly Color HairMysticTurquois = new Color32(71, 191, 165, 255);
        private static readonly Color HairPlatinum = new Color32(255, 245, 226, 255);
        private static readonly Color HairPurplePassion = new Color32(145, 50, 191, 255);
        private static readonly Color HairRosaRosa = new Color32(215, 168, 255, 255);
        private static readonly Color HairTerraCotta = new Color32(185, 49, 4, 255);
        private static readonly Color HairUltraViolet = new Color32(191, 53, 132, 255);
        private static readonly Color HairYellowBlonde = new Color32(255, 203, 89, 255);

        #endregion Private Fields

        #region Public Constructors

        static HairMelanin()
        {
            // Build gradients
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = 1f;
            alphaKeys[0].time = 0.0f;
            alphaKeys[1].alpha = 1f;
            alphaKeys[1].time = 1f;

            GradientColorKey[] euMelaninGradientColorKeys = new GradientColorKey[5];
            euMelaninGradientColorKeys[0].color = HairPlatinum;
            euMelaninGradientColorKeys[0].time = 0.0f;
            euMelaninGradientColorKeys[1].color = new Color32(139, 108, 66, 255);
            euMelaninGradientColorKeys[1].time = 0.7f;
            euMelaninGradientColorKeys[2].color = new Color32(91, 60, 17, 255);
            euMelaninGradientColorKeys[2].time = 0.8f;
            euMelaninGradientColorKeys[3].color = new Color32(47, 30, 14, 255);
            euMelaninGradientColorKeys[3].time = 0.9f;
            euMelaninGradientColorKeys[4].color = new Color32(25, 16, 7, 255);
            euMelaninGradientColorKeys[4].time = 1f;

            // EuMelaninGradientColorKeys[4].color = new Color32(0, 0, 0, 255);
            // EuMelaninGradientColorKeys[4].time = 1f;
            GradientEuMelanin = new Gradient();
            GradientEuMelanin.SetKeys(euMelaninGradientColorKeys, alphaKeys);

            GradientColorKey[] phyoMelaninGradientColorKeys = new GradientColorKey[5];
            phyoMelaninGradientColorKeys[0].color = HairPlatinum;
            phyoMelaninGradientColorKeys[0].time = 0.0f;
            phyoMelaninGradientColorKeys[1].color = new Color32(226, 188, 116, 255);
            phyoMelaninGradientColorKeys[1].time = 0.3f;
            phyoMelaninGradientColorKeys[2].color = new Color32(231, 168, 84, 255);
            phyoMelaninGradientColorKeys[2].time = 0.6f;
            phyoMelaninGradientColorKeys[3].color = new Color32(173, 79, 9, 255);
            phyoMelaninGradientColorKeys[3].time = 0.8f;
            phyoMelaninGradientColorKeys[4].color = new Color32(157, 54, 0, 255);
            phyoMelaninGradientColorKeys[4].time = 1f;

            GradientPheoMelanin = new Gradient();
            GradientPheoMelanin.SetKeys(phyoMelaninGradientColorKeys, alphaKeys);

            NaturalHairColors = new List<Color>
                                    {
                                        HairPlatinum,
                                        HairYellowBlonde,
                                        HairTerraCotta,
                                        HairDarkBrown,
                                        HairMidnightBlack
                                    };

            ArtificialHairColors = new List<Color>()
                                       {
                                           HairGreenGrape,
                                           HairMysticTurquois,
                                           HairBlueSteel,
                                           HairPurplePassion,
                                           HairRosaRosa,
                                           HairUltraViolet,
                                           HairBurgundyBistro
                                       };
        }

        #endregion Public Constructors

        #region Public Methods

        public static HairDNA GenerateHairMelaninAndCuticula( Pawn pawn, bool sameBeardColor)
        {
            Color beardColor;

            SetInitialMelaninLevels(pawn, out HairColorRequest hair);

            // Log.Message(
            // pawn + " - " + melanin + " - " + face.euMelanin + " - " + face.pheoMelanin + " - " + mother?.euMelanin
            // + mother?.pheoMelanin + father?.euMelanin + father?.pheoMelanin);

            // Aging
            float ageFloat = pawn.ageTracker.AgeBiologicalYearsFloat / 100;
            float agingBeginGreyFloat = Rand.Range(0.35f, 0.5f);

            agingBeginGreyFloat += pawn.story.melanin * 0.1f + hair.EuMelanin * 0.05f + hair.PheoMelanin * 0.05f;

            float greySpan = Rand.Range(0.07f, 0.2f);

            greySpan += hair.EuMelanin * 0.15f;
            greySpan += pawn.story.melanin * 0.25f;
            if (ageFloat > agingBeginGreyFloat)
            {
                hair.Greyness = Mathf.InverseLerp(agingBeginGreyFloat, agingBeginGreyFloat + greySpan, ageFloat);
            }

            // Soften the greyness
            // greyness *= 0.95f;

            // Even more - melanin
            // if (PawnSkinColors.IsDarkSkin(pawn.story.SkinColor))
            // {
            // greyness *= Rand.Range(0.5f, 0.9f);
            // }
            // Log.Message(pawn.ToString());
            // Log.Message(ageFloat.ToString());
            // Log.Message(agingBeginGreyFloat.ToString());
            // Log.Message(greySpan.ToString());
            // Log.Message(greyness.ToString());
            Color hairColor = GetHairColor(hair);


            // Special hair colors
            float factionColor = Rand.Value;
            float limit = 0.98f;
            if (pawn.Faction.def.techLevel > TechLevel.Industrial)
            {
                limit *= pawn.gender == Gender.Female ? 0.7f : 0.9f;

                float techMod = (pawn.Faction.def.techLevel - TechLevel.Industrial) / 5f;
                SimpleCurve ageCure = new SimpleCurve { { 0.1f, 1f }, { 0.25f, 1f - techMod }, { 0.6f, 0.9f } };
                limit *= ageCure.Evaluate(pawn.ageTracker.AgeBiologicalYears / 100f);
            }

            if (factionColor > limit && pawn.ageTracker.AgeBiologicalYearsFloat > 16)
            {
                Color color2;
                float rand = Rand.Value;
                if (rand < 0.15f)
                {
                    color2 = HairBlueSteel;
                }
                else if (rand < 0.3f)
                {
                    color2 = HairBurgundyBistro;
                }
                else if (rand < 0.45f)
                {
                    color2 = HairGreenGrape;
                }
                else if (rand < 0.6f)
                {
                    color2 = HairMysticTurquois;
                }
                else if (rand < 0.75f)
                {
                    color2 = HairPurplePassion;
                }
                else if (rand < 0.9f)
                {
                    color2 = HairRosaRosa;
                }
                else
                {
                    color2 = HairUltraViolet;
                }

                hairColor = Color.Lerp(hairColor, color2, Rand.Range(0.66f, 1f));
            }


            if (sameBeardColor)
            {
                beardColor = FacialGraphics.DarkerBeardColor(hairColor);
            }
            else
            {
                Color color2 = GradientEuMelanin.Evaluate(hair.EuMelanin + Rand.Range(-0.2f, 0.2f));

                color2 *= GradientPheoMelanin.Evaluate(hair.PheoMelanin + Rand.Range(-0.2f, 0.2f));

                beardColor = Color.Lerp(color2, new Color(0.91f, 0.91f, 0.91f), hair.Greyness * Rand.Value);
            }

            HairDNA dna = new HairDNA
            {
                HairColorRequest = hair,
                HairColor = hairColor,
                BeardColor = beardColor
            };

            return dna;
        }

        public static Color GetHairColor(HairColorRequest hairColorRequest)
        {
            Color color = GradientEuMelanin.Evaluate(hairColorRequest.EuMelanin);

            color *= GradientPheoMelanin.Evaluate(hairColorRequest.PheoMelanin);

            Color.RGBToHSV(color, out float h, out float s, out float v);
            s *= hairColorRequest.Cuticula;

            color = Color.HSVToRGB(h, s, v);

            // limit the greyness to 70 %, else it's too much
            color = Color.Lerp(color, new Color(0.86f, 0.86f, 0.86f), Mathf.Min(hairColorRequest.Greyness, 0.7f));


            return color;
        }

        #endregion Public Methods

        #region Private Methods

        private static void HasOptimizedFather( Pawn pawn, out bool hasFather,  out PawnFace fatherPawnFace)
        {
            hasFather = false;
            fatherPawnFace = null;

            if (pawn.GetFather() != null)
            {
                CompFace fatherComp = pawn.GetFather().TryGetComp<CompFace>();

                if (fatherComp.PawnFace != null && fatherComp.PawnFace.Cuticula > 0)
                {
                    hasFather = true;
                    fatherPawnFace = fatherComp.PawnFace;
                }

            }
        }

        private static void HasOptimizedMother( Pawn pawn, out bool hasMother, out PawnFace motherPawnFace)
        {
            hasMother = false;
            motherPawnFace = null;
            if (pawn.GetMother() != null)
            {
                CompFace motherComp = pawn.GetMother().TryGetComp<CompFace>();
                if (motherComp.PawnFace != null && motherComp.PawnFace.Cuticula > 0)
                {
                    hasMother = true;
                    motherPawnFace = motherComp.PawnFace;
                }
            }

        }

        private static float GetRandomChildHairColor(float fatherMelanin, float motherMelanin)
        {
            float clampMin = Mathf.Min(fatherMelanin, motherMelanin);
            float clampMax = Mathf.Max(fatherMelanin, motherMelanin);
            float value = (fatherMelanin + motherMelanin) / 2f;
            return GetRandomMelaninSimilarTo(value, clampMin, clampMax);
        }

        private static float GetRandomMelaninSimilarTo(float value, float clampMin = 0f, float clampMax = 1f)
        {
            return Mathf.Clamp01(Mathf.Clamp(Rand.Gaussian(value, 0.05f), clampMin, clampMax));
        }

        private static bool GetMelaninSetRelationsByBlood( Pawn pawn, ref HairColorRequest hair)
        {
            if (pawn.relations.FamilyByBlood.Any())
            {
                Pawn relPawn = pawn.relations.FamilyByBlood.FirstOrDefault(x =>
                    {
                        // cuticula check to prevent old pawns with incomplete stats
                        PawnFace pawnFace = x.TryGetComp<CompFace>().PawnFace;
                        return pawnFace != null && pawnFace.Cuticula > 0.5f;
                    });

                if (relPawn != null)
                {
                    CompFace relatedPawn = relPawn.TryGetComp<CompFace>();

                    float melaninx1 = relatedPawn.PawnFace.EuMelanin;
                    float melaninx2 = relatedPawn.PawnFace.PheoMelanin;
                    float cuticulax = relatedPawn.PawnFace.Cuticula;
                    hair.EuMelanin = GetRandomMelaninSimilarTo(melaninx1);
                    hair.PheoMelanin = GetRandomMelaninSimilarTo(melaninx2);
                    hair.Cuticula = GetRandomMelaninSimilarTo(cuticulax);
                    return true;
                }
            }

            return false;
        }

        private static void SetInitialMelaninLevels( Pawn pawn, out HairColorRequest hair)
        {
            hair = new HairColorRequest(0f, 0f, 0f, 0f);

            HasOptimizedMother(pawn, out bool hasMother, out PawnFace motherPawnFace);

            HasOptimizedFather(pawn, out bool hasFather, out PawnFace fatherPawnFace);

            if (hasMother && hasFather)
            {
                hair.EuMelanin = GetRandomChildHairColor(motherPawnFace.EuMelanin, fatherPawnFace.EuMelanin);
                hair.PheoMelanin = GetRandomChildHairColor(motherPawnFace.PheoMelanin, fatherPawnFace.PheoMelanin);
                hair.Cuticula = GetRandomChildHairColor(motherPawnFace.Cuticula, fatherPawnFace.Cuticula);
            }
            else if (hasMother)
            {
                hair.EuMelanin = GetRandomMelaninSimilarTo(motherPawnFace.EuMelanin);
                hair.PheoMelanin = GetRandomMelaninSimilarTo(motherPawnFace.PheoMelanin);
                hair.Cuticula = GetRandomMelaninSimilarTo(motherPawnFace.Cuticula);
            }
            else if (hasFather)
            {
                hair.EuMelanin = GetRandomMelaninSimilarTo(fatherPawnFace.EuMelanin);
                hair.PheoMelanin = GetRandomMelaninSimilarTo(fatherPawnFace.PheoMelanin);
                hair.Cuticula = GetRandomMelaninSimilarTo(fatherPawnFace.Cuticula);
            }
            else
            {
                // Check for relatives, else randomize
                if (!GetMelaninSetRelationsByBlood(pawn, ref hair))
                {
                    hair.EuMelanin = Rand.Range(pawn.story.melanin * 0.5f, 1f);
                    hair.PheoMelanin = Rand.Range(pawn.story.melanin * 0.25f, 1f);
                    hair.Cuticula = Rand.Range(0.75f, 1.25f);
                }
            }

        }

        #endregion Private Methods

        // Deactivated for now, as there's no way to get the pawn's birth biome and full history, considered mostly racist
        /*
        public static void SkinGenetics(Pawn pawn, CompFace face, out float factionMelanin)
        {
            factionMelanin = pawn.story.melanin;
            bool isTribal = pawn.Faction?.def == FactionDefOf.Tribe || pawn.Faction?.def == FactionDefOf.PlayerTribe;
            bool isSpacer = pawn.Faction?.def == FactionDefOf.Spacer || pawn.Faction?.def == FactionDefOf.SpacerHostile;

            if (face == null)
            {
                return;
            }

            face.MelaninOrg = pawn.story.melanin;
            CompFace mother = null;
            CompFace father = null;
            bool hasMother = false;
            bool hasFather = false;

            if (pawn.GetMother() == null)
            {
                hasMother = true;
            }
            else
            {
                mother = pawn.GetMother().TryGetComp<CompFace>();
            }

            if (pawn.GetFather() == null)
            {
                hasFather = true;
            }
            else
            {
                father = pawn.GetFather().TryGetComp<CompFace>();
            }

            bool flag = true;

            if (!hasMother && mother.IsSkinDNAoptimized && !hasFather && father.IsSkinDNAoptimized)
            {
                factionMelanin = GetRandomChildHairColor(mother.FactionMelanin, father.FactionMelanin);
            }
            else if (!hasMother && mother.IsSkinDNAoptimized)
            {
                factionMelanin = GetRandomMelaninSimilarTo(mother.FactionMelanin);
            }
            else if (!hasFather && father.IsSkinDNAoptimized)
            {
                factionMelanin = GetRandomMelaninSimilarTo(father.FactionMelanin);
            }
            else
            {
                // if (hasMother && hasFather)
                if (pawn.relations.FamilyByBlood.Any())
                {
                    Pawn relPawn =
                        pawn.relations.FamilyByBlood.FirstOrDefault(x => x.TryGetComp<CompFace>().IsSkinDNAoptimized);
                    if (relPawn != null)
                    {
                        CompFace relatedPawn = relPawn.TryGetComp<CompFace>();

                        float melaninx1 = relatedPawn.FactionMelanin;
                        factionMelanin = GetRandomMelaninSimilarTo(melaninx1);
                        flag = false;
                    }
                }

                if (flag)
                {
                    if (isTribal)
                    {
                        SimpleCurve curve =
                            new SimpleCurve
                                {
                                    new CurvePoint(0f, 0f),
                                    new CurvePoint(0.2f, 0.5f),
                                    new CurvePoint(1f, 1f)
                                };
                        factionMelanin = curve.Evaluate(pawn.story.melanin);
                    }

                    if (isSpacer)
                    {
                        SimpleCurve curve =
                            new SimpleCurve
                                {
                                    new CurvePoint(0f, 0.0f),
                                    new CurvePoint(0.5f, 0.25f),
                                    new CurvePoint(1f, 1f)
                                };
                        factionMelanin = curve.Evaluate(pawn.story.melanin);
                    }
                }
            }

            if (Controller.settings.UseDNAByFaction)
            {
                if (Math.Abs(pawn.story.melanin - factionMelanin) > 0.01f)
                {
                    pawn.story.melanin = factionMelanin;
                }
            }

            // Log.Message(
            // pawn + " - " + melanin + " - " + face.euMelanin + " - " + face.pheoMelanin + " - " + mother?.euMelanin
            // + mother?.pheoMelanin + father?.euMelanin + father?.pheoMelanin);
        }
        */
    }
}