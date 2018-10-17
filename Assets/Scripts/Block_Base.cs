using UnityEngine;

namespace Assets.Scripts
{
    abstract class Block_Base : MonoBehaviour
    {
        internal short currentPosition = 1, linesAmount;
        internal bool canMove;
        [SerializeField]
        internal AudioClip audioBadHit;

        internal short GridPosX
        {
            get
            {
                return (short)(Mathf.Round(this.gameObject.transform.position.x * 100) / 36);
            }
        }
        internal short GridPosY
        {
            get
            {
                return (short)(Mathf.Round(this.gameObject.transform.position.y * 100) / 36);
            }
        }

        public abstract bool MoveBlockDown(bool[,] _pitStack);
        public abstract void MoveBlockLeft(bool[,] _pitStack);
        public abstract void MoveBlockRight(bool[,] _pitStack);
        public abstract void TurnBlock(bool[,] _pitStack);

    }
}
