using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class EnemyInfo {
    public int _index;
    public string _type;

    public int _spriteIndex;
    public int _functionIndexDrawSprite;
    public long _pFunctionThink;
    public int _width;
    public int _height;
    public long _pFunctionTimedAction;

    public EnemyActionTypeEnum _actionType;

    public static Dictionary<string, int> _facingNbAnim_FromLevelSpriteIndex;

    static EnemyInfo () {
      _facingNbAnim_FromLevelSpriteIndex = new Dictionary<string, int> ();
      _facingNbAnim_FromLevelSpriteIndex.Add ("1_448", 3);
      _facingNbAnim_FromLevelSpriteIndex.Add ("1_454", 3);
      _facingNbAnim_FromLevelSpriteIndex.Add ("1_460", 9);
      _facingNbAnim_FromLevelSpriteIndex.Add ("1_478", 9);
      _facingNbAnim_FromLevelSpriteIndex.Add ("1_496", 9);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("1_514", 8);
      _facingNbAnim_FromLevelSpriteIndex.Add ("1_522", 4);
      _facingNbAnim_FromLevelSpriteIndex.Add ("1_530", 3);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("1_536", 1);

      _facingNbAnim_FromLevelSpriteIndex.Add ("2_448", 9);
      _facingNbAnim_FromLevelSpriteIndex.Add ("2_466", 9);
      _facingNbAnim_FromLevelSpriteIndex.Add ("2_484", 9);
      _facingNbAnim_FromLevelSpriteIndex.Add ("2_502", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("2_510", 9);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("2_519", 4);
      _facingNbAnim_FromLevelSpriteIndex.Add ("2_523", 3);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("2_529", 3);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("2_532", 1);
      _facingNbAnim_FromLevelSpriteIndex.Add ("2_533", 3);

      _facingNbAnim_FromLevelSpriteIndex.Add ("3_448", 9);
      _facingNbAnim_FromLevelSpriteIndex.Add ("3_466", 9);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_484", 6);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_490", 5);
      _facingNbAnim_FromLevelSpriteIndex.Add ("3_495", 4);
      _facingNbAnim_FromLevelSpriteIndex.Add ("3_503", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_511", 3);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_514", 3);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_517", 4);
      _facingNbAnim_FromLevelSpriteIndex.Add ("3_521", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_529", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_533", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("3_537", 1);

      _facingNbAnim_FromLevelSpriteIndex.Add ("4_448", 9);
      _facingNbAnim_FromLevelSpriteIndex.Add ("4_466", 9);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("4_484", 10);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("4_494", 8);
      _facingNbAnim_FromLevelSpriteIndex.Add ("4_502", 4);
      _facingNbAnim_FromLevelSpriteIndex.Add ("4_510", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("4_518", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("4_522", 12);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("4_541", 4);
      _facingNbAnim_FromLevelSpriteIndex.Add ("4_545", 4);
      //_facingNbAnim_FromLevelSpriteIndex.Add ("4_553", 1);
    }

    public int GetSpriteIndex (int level, int facing) {
      // mouths are inversed
      if ((_spriteIndex == 523) || (_spriteIndex == 530) || (_spriteIndex == 545)) {
        facing = 1 - facing;
      }
      string key = string.Format ("{0}_{1}", level, _spriteIndex.ToString ().PadLeft (3, '0'));
      if (_facingNbAnim_FromLevelSpriteIndex.ContainsKey (key)) {
        return (_spriteIndex + facing * _facingNbAnim_FromLevelSpriteIndex [key]);
      }
      else {
        return (_spriteIndex);
      }
    }

    public EnemyInfo (byte [] buffer, ref int offset, int index, string type) {
      _index = index;
      _type = type;

      _spriteIndex = buffer.ReadWord (offset);
      offset += 2;
      _functionIndexDrawSprite = buffer.ReadWord (offset);
      offset += 2;
      _pFunctionThink = buffer.ReadLong (offset);
      offset += 4;
      _width = buffer.ReadWord (offset);
      offset += 2;
      _height = buffer.ReadWord (offset);
      offset += 2;
      _pFunctionTimedAction = buffer.ReadLong (offset);
      offset += 4;

      _actionType = (EnemyActionTypeEnum) _pFunctionTimedAction;
      if (!Enum.IsDefined (typeof (EnemyActionTypeEnum), _actionType))
        throw new Exception ();
    }

    public Bitmap GetBitmap (World world) {
      return (world.GetBitmap (_spriteIndex));
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("{0} ENEMY INFO #{1}", _type, _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddString (g, string.Format ("Sprite index: #{0} ", _spriteIndex));
      BoxHelper.AddBitmap (g, GetBitmap (world));
      BoxHelper.NextLine ();
      BoxHelper.AddStringLine (g, string.Format ("Think: 0x{0:X8}", _pFunctionThink));
      BoxHelper.AddStringLine (g, string.Format ("Action: {0}", _actionType));
      BoxHelper.DrawBox (g);
    }
  }

  public enum EnemyActionTypeEnum {
    Walking_Fire_01 = 0x75FC,
    Walking_Fire_02 = 0x7F26,
    Walking_RewindAnim_03 = 0x7924,
    Walking_RewindAnim_04 = 0x799E,
    Flying_Fire_01 = 0x8358,
    Flying_AnimUnk_02 = 0x85F0,
    Flying_AnimUnk_03 = 0x857E,
    Flying_NoAction_04 = 0x8536
  }

  //public enum WalkingEnemyThinkEnum {
  //  Level1Type0_460_Walking = 0,
  //  Level1Type1_460_Walking = 0,
  //  Level1Type2_3_496_Walking = 0,
  //  Level1Type4_Rts = 0,
  //  Level1Type5_6_7_Level2Type3_530_523_MouthTurret,
  //  Level2Type0_448_Walking,
  //  Level2Type1_466_Walking,
  //  Level2Type2_5_6_7_519_MouthDripping,
  //  Level3Type0_1_4_5_6_448_Walking,
  //  Level3Type2_466_Walking,
  //  Level3Type3_514_MouthBitting,
  //  Level4Type1_4_5_6_484_Walking,
  //  Level4Type2_466_Walking,
  //  Level4Type3_545_Turret,
  //}
}
