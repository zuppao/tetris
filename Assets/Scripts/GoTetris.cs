using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region BACKLOG
/*
 * difficult enhancements
 * - add a line of RIGID blocks at the botton
 *      - it could be eliminates when the player eliminate 2 filled lines at same time
 */
#endregion

public class GoTetris : MonoBehaviour
{
    private bool[,] pitStackBinary;
    private SpriteRenderer[,] pitStackSprites;
    private GameObject currentBlock, nextBlock;
    private float elapsedTipe;
    private bool blockCanMoveDown, paused;
    private short level, linesClearedTemp;
    private ushort linesTotal;
    private int score;

    [SerializeField]
    private Text txt_LEVEL, txt_SCORE, txt_LINES;
    [SerializeField]
    private GameObject pf_Square, pf_LE, pf_LD, pf_S, pf_Z, pf_Podium, pf_I;
    [SerializeField]
    private AudioClip audioBlockFall, audioBlockStop, audioGameOver, audioLinesClearedSimple, audioLinesClearedDouble, audioLinesClearedTriple;

    //teste
    [SerializeField]
    private GameObject pf_Test;

    // Use this for initialization
    void Start()
    {
        this.level = 0;
        this.linesTotal = 0;
        this.linesClearedTemp = 0;
        this.score = 0;
        this.IncrementLevel();
        this.txt_LINES.text = this.linesTotal.ToString();
        this.txt_SCORE.text = this.score.ToString();

        this.elapsedTipe = 0;
        this.blockCanMoveDown = true;
        this.paused = false;

        this.pitStackBinary = new bool[12, 21];
        this.pitStackBinary.Initialize();
        this.pitStackSprites = new SpriteRenderer[12, 21];//teste
        //adding the walls to pit array
        for (int i = 0; i < 21; i++)
        {
            this.pitStackBinary[0, i] = true;
            this.pitStackBinary[11, i] = true;
        }
        for (int i = 1; i < 12; i++)
        {
            this.pitStackBinary[i, 20] = true;
        }

        //Next block
        this.nextBlock = Object.Instantiate(this.NewNextBlock(),
                                            new Vector3((float)5.4, (float)-0.5),
                                            Quaternion.identity);
        this.NewBlock();



        //criar prefabs de cada tipo de block: quadrado, L, Z, I
        //instanciar o prefab e ir jogando com ele (rodando, descendo, etc)
        //
        // qndo parar de descer, obter a instancia do prefab, obter os componentes SpriteRenderer
        // clonar esses block simples para ficarem na tela (como no exmeplo abaixo) e destruir o prefab
        //
        //GameObject opa1 = GameObject.Find("PF_Test");
        //opa1.transform.position = new Vector3(1, -1, opa1.transform.position.z);

        //Component[] comps = opa1.GetComponentsInChildren(typeof(SpriteRenderer));

        //Debug.Log("[" + comps[0].transform.position.x + "] [" + comps[0].transform.position.y + "]");
        //Debug.Log("[" + comps[1].transform.position.x + "] [" + comps[1].transform.position.y + "]");

        //GameObject.Instantiate(comps[1], comps[1].transform.position,Quaternion.identity);
        //Destroy(opa1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.paused)
        {
            this.elapsedTipe += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.currentBlock.GetComponent<Assets.Scripts.Block_Base>()
                                 .MoveBlockRight(this.pitStackBinary);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.currentBlock.GetComponent<Assets.Scripts.Block_Base>()
                                 .MoveBlockLeft(this.pitStackBinary);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.currentBlock.GetComponent<Assets.Scripts.Block_Base>()
                                 .MoveBlockDown(this.pitStackBinary);
                AudioSource.PlayClipAtPoint(this.audioBlockFall, Vector3.zero);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.currentBlock.GetComponent<Assets.Scripts.Block_Base>()
                                 .TurnBlock(this.pitStackBinary);
            }

            //the higher number, the lower difficult
            //the lower number, the higher difficult
            if (this.elapsedTipe >= (1 - ((float)this.level / 10)))//to do the 'block down' movement each 'x' sec.
            {
                this.elapsedTipe = 0;
                this.blockCanMoveDown = this.currentBlock.GetComponent<Assets.Scripts.Block_Base>()
                                                         .MoveBlockDown(this.pitStackBinary);
                if (!this.blockCanMoveDown)
                {
                    AudioSource.PlayClipAtPoint(this.audioBlockStop,Vector3.zero);
                    //finazlyze cycle, cloning the prefab objs and setting true to the position on the pit array
                    Component[] comps = this.currentBlock.GetComponentsInChildren(typeof(SpriteRenderer));

                    short x, y = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        x = (short)Mathf.Abs(Mathf.Round(comps[i].transform.position.x * 100) / 36);

                        y = (short)(Mathf.Round(comps[i].transform.position.y * 100) / 36);
                        if (y > 0)
                        {
                            this.GameOver();
                            return;
                        }
                        y = (short)Mathf.Abs(y);
                        //mark true on the representative array "this.pitStack"
                        this.pitStackBinary[x, y] = true;

                        //Clone the individual squares
                        this.pitStackSprites[x, y] = GameObject.Instantiate((SpriteRenderer)comps[i], comps[i].transform.position, Quaternion.identity);
                        //this.pitStackSprites[x, y] = GameObject.Instantiate<SpriteRenderer>((SpriteRenderer)comps[i], comps[i].transform.position, Quaternion.identity);
                    }
                    //Destroy the prefab
                    Destroy(this.currentBlock);

                    //check for complete lines
                    this.CheckCompletedRows((short)Mathf.Abs(Mathf.Round(this.currentBlock.transform.position.y * 100) / 36));

                    //Instantiate new block and restart the cycle
                    this.NewBlock();
                }
                else
                    AudioSource.PlayClipAtPoint(this.audioBlockFall, Vector3.zero);
            }




        }
    }



    void NewBlock()
    {
        this.currentBlock = this.nextBlock;
        this.currentBlock.transform.position = new Vector3((float)1.8, (float)0.72);

        this.nextBlock = Object.Instantiate(this.NewNextBlock(),
                                            new Vector3((float)5.4, (float)-0.5),
                                            Quaternion.identity);
    }

    GameObject NewNextBlock()
    {
        //todo: esse random nao esta tao RANDOM assim....
        int iii = Random.Range(1, 8);
        //Debug.Log("Random: " + iii);

        GameObject obj = null;
        switch (iii)//in the tests, the MAX number is never returned.
        {
            case 1:
                obj = pf_Square;
                break;
            case 2:
                obj = pf_LE;
                break;
            case 3:
                obj = pf_LD;
                break;
            case 4:
                obj = pf_S;
                break;
            case 5:
                obj = pf_Z;
                break;
            case 6:
                obj = pf_I;
                break;
            case 7:
                obj = pf_Podium;
                break;
            case 8:
                obj = pf_Test;
                break;
            default:
                break;
        }
        return obj;
    }
    void CheckCompletedRows(short _currentBlock_Y_Position)
    {
        short linesEliminated = 0;
        short amt = this.currentBlock.GetComponent<Assets.Scripts.Block_Base>().linesAmount;
        //Debug.Log("checar Y: " + _currentBlock_Y_Position + " e mais " + (qtd - 1) + " linhas abaixo");
        for (short i = _currentBlock_Y_Position; i < (_currentBlock_Y_Position + amt); i++)
        {
            for (short x = 1; x <= 10; x++)
            {
                if (this.pitStackBinary[x, i] == false)
                    break;

                if (x == 10)//all positions in this line are true
                {
                    this.DeleteLine(i);
                    linesEliminated++;
                }
            }

        }
        //update Line Count UI
        this.IncrementLineCountUI(linesEliminated);
        //update Socre UI
        this.IncrementScoreUI(linesEliminated);
    }

    void DeleteLine(short _line)
    {
        //Debug.Log("eliminando linha " + _line);// ok.. ta chegando aqui.....

        //delete the filled line
        for (short x = 1; x <= 10; x++)
        {
            this.pitStackBinary[x, _line] = false;
            Destroy(this.pitStackSprites[x, _line].gameObject);
        }


        //fix the position of ALL above blocks
        for (short y = _line; y >= 0; y--)
        {
            for (short x = 1; x <= 10; x++)
            {
                if (this.pitStackBinary[x, y] == true)
                {
                    this.pitStackBinary[x, y] = false;
                    this.pitStackBinary[x, y + 1] = true;

                    //Component _block = (Component)this.pitStackSprites[x, y];
                    SpriteRenderer _block = this.pitStackSprites[x, y];//teste

                    //move the object on the screen 1 position down
                    _block.transform.position = new Vector3(_block.transform.position.x,
                                                            _block.transform.position.y - (float)0.36);
                    //assign to this object their new position on the pitStackSprites
                    this.pitStackSprites[x, y + 1] = _block;

                    //the old position now is empty
                    _block = null;
                }
            }
        }


    }

    void GameOver()
    {
        AudioSource.PlayClipAtPoint(this.audioGameOver,Vector3.zero);
        Destroy(this.currentBlock);

        //delete the filled line
        for (int y = 19; y >= 0; y--)
        {
            for (short x = 1; x <= 10; x++)
            {
                if (this.pitStackBinary[x, y] == true)
                    Destroy(this.pitStackSprites[x, y].gameObject);
            }
        }

        this.paused = true;
        Debug.Log("GAME OVER......!");
    }
    void IncrementLineCountUI(short _linesCleared)
    {
        this.linesTotal += (ushort)_linesCleared;
        this.txt_LINES.text = this.linesTotal.ToString();

        this.linesClearedTemp += _linesCleared;
        if (this.linesClearedTemp >= 4)//difficult increase when 3 lines are eliminated
        {
            this.linesClearedTemp = 0;
            this.IncrementLevel();
        }
    }
    void IncrementScoreUI(short _linesCleared)
    {
        switch (_linesCleared)
        {
            case 1:
                this.score += 1;
                AudioSource.PlayClipAtPoint(this.audioLinesClearedSimple, Vector3.zero);
                break;
            case 2:
                this.score += 3;
                AudioSource.PlayClipAtPoint(this.audioLinesClearedDouble, Vector3.zero);
                break;
            case 3:
                this.score += 5;
                AudioSource.PlayClipAtPoint(this.audioLinesClearedTriple, Vector3.zero);
                break;
            case 4:
                this.score += 10;
                AudioSource.PlayClipAtPoint(this.audioLinesClearedTriple, Vector3.zero);
                break;
            default:
                break;
        }

        this.txt_SCORE.text = this.score.ToString();
    }

    void IncrementLevel()
    {
        if (this.level == 9)
            return;

        this.level++;
        this.txt_LEVEL.text = (this.level).ToString();
    }
}
