﻿namespace FacialStuff.Animator
{
    using Verse;
    using Verse.AI;

    public class BodyAnimator
    {
        // public float cyclePercent = 0;

        // public bool Finished;

        private Pawn pawn;

        private CompFace compFace;

        public BodyAnimator(Pawn p, CompFace compface)
        {
            this.pawn = p;
            this.compFace = compface;
        }

        // Verse.PawnTweener
        public bool IsMoving(out float movedPercent)
        {
            movedPercent = 0f;

            if (this.compFace.AnimatorOpen)
            {
                movedPercent = this.compFace.AnimationPercent;
                return true;
            }

            Pawn_PathFollower pather = this.pawn.pather;
            if (pather == null)
            {
                return false;
            }

            if (!pather.Moving)
            {
                return false;
            }

            if (this.pawn.stances.FullBodyBusy)
            {
                return false;
            }
            if (pather.BuildingBlockingNextPathCell() != null)
            {
                return false;
            }
            if (pather.NextCellDoorToManuallyOpen() != null)
            {
                return false;
            }
            if (pather.WillCollideWithPawnOnNextPathCell())
            {
                return false;
            }
            movedPercent = 1f - pather.nextCellCostLeft / pather.nextCellCostTotal;
            return true;
        }

        public void AnimatorTick()
        {
            // if (this.pawn.pather.Moving)
            // {
            //     float left = this.pawn.pather.nextCellCostLeft;
            //     float total = this.pawn.pather.nextCellCostTotal;
            //     this.cyclePercent = 1f - left / total;
            //
            //     this.Finished = false;
            // }
            // else
            // {
            //     this.Finished = true;
            // }
        }

    }
}
