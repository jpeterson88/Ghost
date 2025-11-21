using System;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class FacingDirection: MonoBehaviour
    {
        [SerializeField] private bool startFacingLeft;

        private FacingDirectionEnum currentFacing;

        private void Awake()
        {
            currentFacing = startFacingLeft ? FacingDirectionEnum.Left : FacingDirectionEnum.Right;
        }


        public FacingDirectionEnum GetCurrentFacing()
        {
            return currentFacing;
        }

        public FacingDirectionEnum FlipFacing()
        {
            currentFacing = currentFacing == FacingDirectionEnum.Right ? FacingDirectionEnum.Left : FacingDirectionEnum.Right;
            return currentFacing;
        }


        public bool SetFacingDirection(FacingDirectionEnum facingDirection)
        {
            bool flipped = false;
            if(currentFacing == FacingDirectionEnum.Left && facingDirection == FacingDirectionEnum.Right)
            {
                flipped = true;
                currentFacing = FacingDirectionEnum.Right;
            }
            else if(currentFacing == FacingDirectionEnum.Right && facingDirection == FacingDirectionEnum.Left)
            {
                flipped = true;
                currentFacing = FacingDirectionEnum.Left;
            }

            return flipped;
        }
    }

    [Serializable]
    public enum FacingDirectionEnum
    {
        Left = 0,
        Right = 1,
    }
}