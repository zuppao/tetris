using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Block_Square : Block_Base
    {
        public Block_Square()
        {
            this.linesAmount = 2;
        }
        #region INTERFACE IMoveBlock
        public override void MoveBlockRight(bool[,] _pitStack)
        {
            //check
            if (_pitStack[this.GridPosX + 2, Mathf.Abs(this.GridPosY)] == false &&
               _pitStack[this.GridPosX + 2, Mathf.Abs(this.GridPosY - 1)] == false)
            {
                //move
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (float)0.36,
                                                                 this.gameObject.transform.position.y);
            }
            else
                AudioSource.PlayClipAtPoint(this.audioBadHit, Vector3.zero);
        }
        public override void MoveBlockLeft(bool[,] _pitStack)
        {
            //check
            if (_pitStack[this.GridPosX - 1, Mathf.Abs(this.GridPosY)] == false &&
               _pitStack[this.GridPosX - 1, Mathf.Abs(this.GridPosY - 1)] == false)
            {
                //move
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - (float)0.36,
                                                                 this.gameObject.transform.position.y);
            }
            else
                AudioSource.PlayClipAtPoint(this.audioBadHit, Vector3.zero);

        }
        public override bool MoveBlockDown(bool[,] _pitStack)
        {
            bool ret = _pitStack[this.GridPosX, Mathf.Abs(this.GridPosY - 2)] == false &&
                       _pitStack[this.GridPosX + 1, Mathf.Abs(this.GridPosY - 2)] == false;

            //check
            if (ret)
            {
                //move
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                                 this.gameObject.transform.position.y - (float)0.36);

            }

            return ret;
        }
        public override void TurnBlock(bool[,] _pitStack)
        {
            //nothing to do. This block do not turn.
            AudioSource.PlayClipAtPoint(this.audioBadHit, Vector3.zero);
        }
        #endregion
    }
}
