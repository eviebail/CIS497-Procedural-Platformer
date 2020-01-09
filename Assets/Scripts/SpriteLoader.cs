using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteLoader : MonoBehaviour
{
    public Texture2D texIdle1;
    public Texture2D texIdle2;
    public Texture2D texIdle3;
    public Texture2D texIdle4;
    public Texture2D texIdle5;
    public Texture2D texIdle6;
    public Texture2D texIdle7;
    public Texture2D texIdle8;

    public Texture2D texRun1R;
    public Texture2D texRun2R;
    public Texture2D texRun1L;
    public Texture2D texRun2L;

    public Texture2D texCrouch1R;
    public Texture2D texCrouch2R;
    public Texture2D texCrouch1L;
    public Texture2D texCrouch2L;

    public Texture2D texDashL;
    public Texture2D texDashR;

    public Texture2D texHurt1;
    public Texture2D texHurt2;

    public Texture2D texJumpL;
    public Texture2D texJumpR;
    public Texture2D texFallL;
    public Texture2D texFallR;

    public Texture2D texGround;
    public Texture2D texGroundL;
    public Texture2D texGroundR;
    public Texture2D texEnemy;
    public Texture2D texSpike;
    public Texture2D texCoin;
    public Texture2D texGnd;
    public Texture2D texUnderGnd;

    public Texture2D texLColumn;
    public Texture2D texRColumn;
    public Texture2D texLBottom;
    public Texture2D texBottom;
    public Texture2D texRBottom;

    public Texture2D texMoveGnd;
    public Texture2D texSlope;
    public Texture2D texSlope2;
    public Texture2D texUnderSlope;
    public Texture2D texUnderSlope2;
    public Texture2D texGoal;
    public Texture2D texPipe;
    public Texture2D texEnemy2;
    public Texture2D texEnemy3;
    public Texture2D texEnemy4;

    public Texture2D texEnemyAttackL1;
    public Texture2D texEnemyAttackL2;
    public Texture2D texEnemyAttackR1;
    public Texture2D texEnemyAttackR2;

    public Texture2D texEnemyAttackPellet;

    public Texture2D texEnemyDeath1;
    public Texture2D texEnemyDeath2;
    public Texture2D texEnemyDeath3;
    public Texture2D texEnemyDeath4;
    public Texture2D texEnemyDeath5;
    public Texture2D texEnemyDeath6;

    public static Sprite spriteIdle1;
    public static Sprite spriteIdle2;
    public static Sprite spriteIdle3;
    public static Sprite spriteIdle4;
    public static Sprite spriteIdle5;
    public static Sprite spriteIdle6;
    public static Sprite spriteIdle7;
    public static Sprite spriteIdle8;
                         
    public static Sprite spriteRun1R;
    public static Sprite spriteRun2R;
    public static Sprite spriteRun1L;
    public static Sprite spriteRun2L;
           
    public static Sprite spriteCrouch1R;
    public static Sprite spriteCrouch2R;
    public static Sprite spriteCrouch1L;
    public static Sprite spriteCrouch2L;
           
    public static Sprite spriteJumpL;
    public static Sprite spriteJumpR;
    public static Sprite spriteFallL;
    public static Sprite spriteFallR;
           
    public static Sprite spriteGround;
    public static Sprite spriteGroundL;
    public static Sprite spriteGroundR;
    public static Sprite spriteEnemy;
    public static Sprite spriteSpike;
    public static Sprite spriteCoin;
    public static Sprite spriteGnd;
    public static Sprite spriteUnderGnd;

    public static Sprite spriteLColumn;
    public static Sprite spriteRColumn;
    public static Sprite spriteLBottom;
    public static Sprite spriteBottom;
    public static Sprite spriteRBottom;

    public static Sprite spriteMoveGnd;
    public static Sprite spriteSlope;
    public static Sprite spriteSlope2;
    public static Sprite spriteUnderSlope;
    public static Sprite spriteUnderSlope2;
    public static Sprite spriteGoal;
    public static Sprite spritePipe;
    public static Sprite spriteEnemy2;
    public static Sprite spriteEnemy3;
    public static Sprite spriteEnemy4;

    public static Sprite spriteEnemyAttackL1;
    public static Sprite spriteEnemyAttackL2;
    public static Sprite spriteEnemyAttackR1;
    public static Sprite spriteEnemyAttackR2;
    public static Sprite spriteEnemyAttackPellet;

    public static Sprite spriteDashL;
    public static Sprite spriteDashR;

    public static Sprite spriteHurt1;
    public static Sprite spriteHurt2;

    public static Sprite spriteEnemyDeath1;
    public static Sprite spriteEnemyDeath2;
    public static Sprite spriteEnemyDeath3;
    public static Sprite spriteEnemyDeath4;
    public static Sprite spriteEnemyDeath5;
    public static Sprite spriteEnemyDeath6;

    void Start()
    {
        spriteIdle1 = Sprite.Create(texIdle1, new Rect(0.0f, 0.0f, texIdle1.width, texIdle1.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteIdle2 = Sprite.Create(texIdle2, new Rect(0.0f, 0.0f, texIdle2.width, texIdle2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteIdle3 = Sprite.Create(texIdle3, new Rect(0.0f, 0.0f, texIdle3.width, texIdle3.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteIdle4 = Sprite.Create(texIdle4, new Rect(0.0f, 0.0f, texIdle4.width, texIdle4.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteIdle5 = Sprite.Create(texIdle5, new Rect(0.0f, 0.0f, texIdle5.width, texIdle5.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteIdle6 = Sprite.Create(texIdle6, new Rect(0.0f, 0.0f, texIdle6.width, texIdle6.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteIdle7 = Sprite.Create(texIdle7, new Rect(0.0f, 0.0f, texIdle7.width, texIdle7.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteIdle8 = Sprite.Create(texIdle8, new Rect(0.0f, 0.0f, texIdle8.width, texIdle8.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteRun1R = Sprite.Create(texRun1R, new Rect(0.0f, 0.0f, texRun1R.width, texRun1R.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteRun2R = Sprite.Create(texRun2R, new Rect(0.0f, 0.0f, texRun2R.width, texRun2R.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteRun1L = Sprite.Create(texRun1L, new Rect(0.0f, 0.0f, texRun1L.width, texRun1L.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteRun2L = Sprite.Create(texRun2L, new Rect(0.0f, 0.0f, texRun2L.width, texRun2L.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteCrouch1R = Sprite.Create(texCrouch1R, new Rect(0.0f, 0.0f, texCrouch1R.width, texCrouch1R.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteCrouch2R = Sprite.Create(texCrouch2R, new Rect(0.0f, 0.0f, texCrouch2R.width, texCrouch2R.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteCrouch1L = Sprite.Create(texCrouch1L, new Rect(0.0f, 0.0f, texCrouch1L.width, texCrouch1L.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteCrouch2L = Sprite.Create(texCrouch2L, new Rect(0.0f, 0.0f, texCrouch2L.width, texCrouch2L.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteDashL = Sprite.Create(texDashL, new Rect(0.0f, 0.0f, texDashL.width, texDashL.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteDashR = Sprite.Create(texDashR, new Rect(0.0f, 0.0f, texDashR.width, texDashR.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteHurt1 = Sprite.Create(texHurt1, new Rect(0.0f, 0.0f, texHurt1.width, texHurt1.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteHurt2 = Sprite.Create(texHurt2, new Rect(0.0f, 0.0f, texHurt2.width, texHurt2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteJumpL = Sprite.Create(texJumpL, new Rect(0.0f, 0.0f, texJumpL.width, texJumpL.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteJumpR = Sprite.Create(texJumpR, new Rect(0.0f, 0.0f, texJumpR.width, texJumpR.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteFallL = Sprite.Create(texFallL, new Rect(0.0f, 0.0f, texFallL.width, texFallL.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteFallR = Sprite.Create(texFallR, new Rect(0.0f, 0.0f, texFallR.width, texFallR.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteGround = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteGroundL = Sprite.Create(texGroundL, new Rect(0.0f, 0.0f, texGroundL.width, texGroundL.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteGroundR = Sprite.Create(texGroundR, new Rect(0.0f, 0.0f, texGroundR.width, texGroundR.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteEnemy = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteSpike = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteCoin = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texCoin.width, texCoin.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteGnd = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteUnderGnd = Sprite.Create(texUnderGnd, new Rect(0.0f, 0.0f, texUnderGnd.width, texUnderGnd.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteLColumn = Sprite.Create(texLColumn, new Rect(0.0f, 0.0f, texLColumn.width, texLColumn.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteRColumn = Sprite.Create(texRColumn, new Rect(0.0f, 0.0f, texRColumn.width, texRColumn.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteLBottom = Sprite.Create(texLBottom, new Rect(0.0f, 0.0f, texLBottom.width, texLBottom.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteBottom = Sprite.Create(texBottom, new Rect(0.0f, 0.0f, texBottom.width, texBottom.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteRBottom = Sprite.Create(texRBottom, new Rect(0.0f, 0.0f, texRBottom.width, texRBottom.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteMoveGnd = Sprite.Create(texMoveGnd, new Rect(0.0f, 0.0f, texMoveGnd.width, texMoveGnd.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteSlope = Sprite.Create(texSlope, new Rect(0.0f, 0.0f, texSlope.width, texSlope.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteSlope2 = Sprite.Create(texSlope2, new Rect(0.0f, 0.0f, texSlope2.width, texSlope2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteUnderSlope = Sprite.Create(texUnderSlope, new Rect(0.0f, 0.0f, texUnderSlope.width, texUnderSlope.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteUnderSlope2 = Sprite.Create(texUnderSlope2, new Rect(0.0f, 0.0f, texUnderSlope2.width, texUnderSlope2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteGoal = Sprite.Create(texGoal, new Rect(0.0f, 0.0f, texGoal.width, texGoal.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spritePipe = Sprite.Create(texPipe, new Rect(0.0f, 0.0f, texPipe.width, texPipe.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemy2 = Sprite.Create(texEnemy2, new Rect(0.0f, 0.0f, texEnemy2.width, texEnemy2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemy3 = Sprite.Create(texEnemy3, new Rect(0.0f, 0.0f, texEnemy3.width, texEnemy3.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemy4 = Sprite.Create(texEnemy4, new Rect(0.0f, 0.0f, texEnemy4.width, texEnemy4.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteEnemyAttackL1 = Sprite.Create(texEnemyAttackL1, new Rect(0.0f, 0.0f, texEnemyAttackL1.width, texEnemyAttackL1.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyAttackL2 = Sprite.Create(texEnemyAttackL2, new Rect(0.0f, 0.0f, texEnemyAttackL2.width, texEnemyAttackL2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyAttackR1 = Sprite.Create(texEnemyAttackR1, new Rect(0.0f, 0.0f, texEnemyAttackR1.width, texEnemyAttackR1.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyAttackR2 = Sprite.Create(texEnemyAttackR2, new Rect(0.0f, 0.0f, texEnemyAttackR2.width, texEnemyAttackR2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyAttackPellet = Sprite.Create(texEnemyAttackPellet, new Rect(0.0f, 0.0f, texEnemyAttackPellet.width, texEnemyAttackPellet.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);

        spriteEnemyDeath1 = Sprite.Create(texEnemyDeath1, new Rect(0.0f, 0.0f, texEnemyDeath1.width, texEnemyDeath1.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyDeath2 = Sprite.Create(texEnemyDeath2, new Rect(0.0f, 0.0f, texEnemyDeath2.width, texEnemyDeath2.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyDeath3 = Sprite.Create(texEnemyDeath3, new Rect(0.0f, 0.0f, texEnemyDeath3.width, texEnemyDeath3.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyDeath4 = Sprite.Create(texEnemyDeath4, new Rect(0.0f, 0.0f, texEnemyDeath4.width, texEnemyDeath4.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyDeath5 = Sprite.Create(texEnemyDeath5, new Rect(0.0f, 0.0f, texEnemyDeath5.width, texEnemyDeath5.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        spriteEnemyDeath6 = Sprite.Create(texEnemyDeath6, new Rect(0.0f, 0.0f, texEnemyDeath6.width, texEnemyDeath6.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
    }

}