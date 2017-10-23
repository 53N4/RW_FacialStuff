namespace FacialStuff
{
    using FacialStuff.Enums;

    using JetBrains.Annotations;

    using Verse;

    public static class CrownTypeChecker
    {
        public static void SetHeadOffsets([NotNull] Pawn p, CompFace compFace)
        {
            switch (p.gender)
            {
                case Gender.Male:
                    CheckMaleCrownType(compFace);
                    break;

                case Gender.Female:
                    CheckFemaleCrownType(compFace);
                    break;

                default:
                    compFace.FullHeadType = FullHead.MaleAverageNormal;
                    break;
            }
        }

        private static void CheckFemaleCrownType(CompFace compFace)
        {
            switch (compFace.PawnCrownType)
            {
                case CrownType.Average:
                    CheckFemaleCrownTypeAverage(compFace);
                    break;

                case CrownType.Narrow:
                    CheckFemaleCrownTypeNarrow(compFace);
                    break;
            }
        }

        private static void CheckFemaleCrownTypeAverage(CompFace compFace)
        {
            switch (compFace.PawnHeadType)
            {
                case HeadType.Normal:
                    compFace.FullHeadType = FullHead.FemaleAverageNormal;
                    break;

                case HeadType.Pointy:
                    compFace.FullHeadType = FullHead.FemaleAveragePointy;
                    break;

                case HeadType.Wide:
                    compFace.FullHeadType = FullHead.FemaleAverageWide;
                    break;
            }
        }

        private static void CheckFemaleCrownTypeNarrow(CompFace compFace)
        {
            switch (compFace.PawnHeadType)
            {
                case HeadType.Normal:
                    compFace.FullHeadType = FullHead.FemaleNarrowNormal;
                    break;

                case HeadType.Pointy:
                    compFace.FullHeadType = FullHead.FemaleNarrowPointy;
                    break;

                case HeadType.Wide:
                    compFace.FullHeadType = FullHead.FemaleNarrowWide;
                    break;
            }
        }

        private static void CheckMaleCrownType(CompFace compFace)
        {
            switch (compFace.PawnCrownType)
            {
                case CrownType.Average:
                    CheckMaleCrownTypeAverage(compFace);
                    break;

                case CrownType.Narrow:
                    CheckMaleCrownTypeNarrow(compFace);
                    break;
            }
        }

        private static void CheckMaleCrownTypeAverage(CompFace compFace)
        {
            switch (compFace.PawnHeadType)
            {
                case HeadType.Normal:
                    compFace.FullHeadType = FullHead.MaleAverageNormal;
                    break;

                case HeadType.Pointy:
                    compFace.FullHeadType = FullHead.MaleAveragePointy;
                    break;

                case HeadType.Wide:
                    compFace.FullHeadType = FullHead.MaleAverageWide;
                    break;
            }
        }

        private static void CheckMaleCrownTypeNarrow(CompFace compFace)
        {
            switch (compFace.PawnHeadType)
            {
                case HeadType.Normal:
                    compFace.FullHeadType = FullHead.MaleNarrowNormal;
                    break;

                case HeadType.Pointy:
                    compFace.FullHeadType = FullHead.MaleNarrowPointy;
                    break;

                case HeadType.Wide:
                    compFace.FullHeadType = FullHead.MaleNarrowWide;
                    break;
            }
        }
    }
}