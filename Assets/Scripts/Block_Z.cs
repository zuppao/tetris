using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Block_Z : Block_Base
    {
        public Block_Z()
        {
            this.linesAmount = 3;
        }
        #region INTERFACE IMoveBlock
        public override void MoveBlockRight(bool[,] _pitStack)
        {
            if(this.currentPosition==1)//vertical position
            {
                this.canMove = _pitStack[this.GridPosX + 2, Mathf.Abs(this.GridPosY)] == false &&
                               _pitStack[this.GridPosX + 2, Mathf.Abs(this.GridPosY - 1)] == false &&
                               _pitStack[this.GridPosX + 1, Mathf.Abs(this.GridPosY - 2)] == false;
            }
            else//horizontal position
            {
                this.canMove = _pitStack[this.GridPosX + 2, Mathf.Abs(this.GridPosY)] == false &&
                               _pitStack[this.GridPosX + 3, Mathf.Abs(this.GridPosY - 1)] == false;
            }
            //check
            if (this.canMove)
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
            if (this.currentPosition == 1)//vertical position
            {
                this.canMove = _pitStack[this.GridPosX, Mathf.Abs(this.GridPosY)] == false &&
                               _pitStack[this.GridPosX - 1, Mathf.Abs(this.GridPosY - 1)] == false &&
                               _pitStack[this.GridPosX - 1, Mathf.Abs(this.GridPosY - 2)] == false;
            }
            else//horizontal position
            {
                this.canMove = _pitStack[this.GridPosX - 1, Mathf.Abs(this.GridPosY)] == false &&
                               _pitStack[this.GridPosX, Mathf.Abs(this.GridPosY - 1)] == false;
            }
            //check
            if (this.canMove)
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
            if (this.currentPosition == 1)//vertical position
            {
                this.canMove = _pitStack[this.GridPosX, Mathf.Abs(this.GridPosY - 3)] == false &&
                               _pitStack[this.GridPosX + 1, Mathf.Abs(this.GridPosY - 2)] == false;
            }
            else//horizontal position
            {
                this.canMove = _pitStack[this.GridPosX, Mathf.Abs(this.GridPosY - 1)] == false &&
                               _pitStack[this.GridPosX + 1, Mathf.Abs(this.GridPosY - 2)] == false &&
                               _pitStack[this.GridPosX + 2, Mathf.Abs(this.GridPosY - 2)] == false;

            }

            //check
            if (this.canMove)
            {
                //move
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                                 this.gameObject.transform.position.y - (float)0.36);

            }

            return this.canMove;
        }
        public override void TurnBlock(bool[,] _pitStack)
        {
            if (this.currentPosition == 1)//vertical
            {
                //check if it can turn
                if (_pitStack[this.GridPosX, Mathf.Abs(this.GridPosY)] == false &&
                    _pitStack[this.GridPosX + 2, Mathf.Abs(this.GridPosY - 1)] == false)
                {
                    this.currentPosition = 2;
                    this.linesAmount = 2;
                    Component[] comps = this.gameObject.GetComponentsInChildren(typeof(SpriteRenderer));
                    comps[2].transform.position = new Vector3(comps[2].transform.position.x, comps[2].transform.position.y + (float)0.36);
                    comps[3].transform.position = new Vector3(comps[3].transform.position.x + (float)0.72, comps[3].transform.position.y + (float)0.36);
                }
                else
                    AudioSource.PlayClipAtPoint(this.audioBadHit, Vector3.zero);
            }
            else//horizontal
            {
                //check if it can turn
                if (_pitStack[this.GridPosX, Mathf.Abs(this.GridPosY - 1)] == false &&
                    _pitStack[this.GridPosX, Mathf.Abs(this.GridPosY - 2)] == false)
                {
                    this.currentPosition = 1;
                    this.linesAmount = 3;
                    Component[] comps = this.gameObject.GetComponentsInChildren(typeof(SpriteRenderer));
                    comps[2].transform.position = new Vector3(comps[2].transform.position.x, comps[2].transform.position.y - (float)0.36);
                    comps[3].transform.position = new Vector3(comps[3].transform.position.x - (float)0.72, comps[3].transform.position.y - (float)0.36);
                }
                else
                    AudioSource.PlayClipAtPoint(this.audioBadHit, Vector3.zero);
            }


        }
        #endregion
    }
}
