using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class WeaponInfo {
    public int _index;

    public int _usedInSlot1Or2; // 0: used in slot 0, 1: used in slot 1 or 2
    public int _functionIndex_Update2;
    public int _ingame_animIndex;
    public int _basePower;
    public int _ingame_currentPower; // = base power + 1 per upgrade
    public int _field_A;
    public int _spriteIndexFirstRight;
    public int _ingame_facing;
    public int _functionIndex_UseLeft;
    public int _functionIndex_UseRight;
    public int _functionIndex_Update;
    public int _animIndexMax;
    public int _spriteIndexFirstLeft;
    public string _name;
    public string _description;
    public int _removeOnWallHit;
    public int _removeOnEnemyHit;
    public int _value;

    public WeaponInfo (byte [] buffer, ref int offset, int index) {
      _index = index;

      _usedInSlot1Or2 = buffer.ReadWord (offset);
      offset += 2;

      _functionIndex_Update2 = buffer.ReadWord (offset);
      offset += 2;

      _ingame_animIndex = buffer.ReadWord (offset);
      offset += 2;

      _basePower = buffer.ReadWord (offset);
      offset += 2;

      _ingame_currentPower = buffer.ReadWord (offset);
      offset += 2;

      _field_A = buffer.ReadWord (offset);
      offset += 2;

      _spriteIndexFirstRight = buffer.ReadWord (offset);
      offset += 2;

      _ingame_facing = buffer.ReadWord (offset);
      offset += 2;

      _functionIndex_UseLeft = (int) buffer.ReadLong (offset);
      offset += 4;
      _functionIndex_UseRight = (int) buffer.ReadLong (offset);
      offset += 4;
      _functionIndex_Update = (int) buffer.ReadLong (offset);
      offset += 4;

      _animIndexMax = buffer.ReadWord (offset);
      offset += 2;

      _spriteIndexFirstLeft = buffer.ReadWord (offset);
      offset += 2;

      _name = buffer.ReadString (offset);
      offset += 16;
      _description = buffer.ReadString (offset);
      offset += 16;

      offset += 8;

      _removeOnWallHit = buffer.ReadByte (offset++);
      _removeOnEnemyHit = buffer.ReadByte (offset++);

      _value = buffer.ReadWord (offset);
      offset += 2;

      offset += 4;
    }

    public string String {
      get {
        return (string.Format ("#{0,-2} {1,-16} ({2}) PowerB: {3,2} PowerC: {4,2} SpriteUnk: {5} UseLeft: {6,2} UseRight: {7,2} Update: {8,2} usedInSlot1Or2: {9} Update2: {10} unk04: {11} unk1E: {12}",
          _index, _name, _description, _basePower, _ingame_currentPower, _spriteIndexFirstRight, _functionIndex_UseLeft, _functionIndex_UseRight, _functionIndex_Update, _usedInSlot1Or2, _functionIndex_Update2, _ingame_animIndex, _animIndexMax));
      }
    }

    public string FullName {
      get {
        string fullName = null;
        if (_name != null) {
          fullName = _name;
        }
        if (_description != null) {
          if (fullName != null) {
            fullName += " ";
          }
          fullName += string.Format ("({0})", _description);
        }
        return (fullName);
      }
    }

    public Bitmap GetBitmap (World world) {
      //return (world.GetBitmap (_spriteIndexFirstRight));
      return (world.GetBitmapWeapon (_index));
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("WEAPON INFO #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddString (g, string.Format ("\"{0}\" ", FullName));
      BoxHelper.AddBitmap (g, GetBitmap (world));
      //if ((_spriteIndexFirstLeft != 0) && (_spriteIndexFirstRight != 0)) {
      //  BoxHelper.AddString (g, string.Format (" "));
      //  BoxHelper.AddBitmap (g, world.GetBitmap (_spriteIndexFirstRight));
      //}
      BoxHelper.NextLine ();
      BoxHelper.AddStringLine (g, string.Format ("Used in slot: {0}", (_usedInSlot1Or2 == 0) ? "0" : "1 or 2"));
      BoxHelper.AddStringLine (g, string.Format ("Power: {0}", _basePower));
      BoxHelper.AddStringLine (g, string.Format ("Remove on wall hit: {0}", (_removeOnWallHit != 0) ? "yes" : "no"));
      BoxHelper.AddStringLine (g, string.Format ("Remove on enemy hit: {0}", (_removeOnEnemyHit != 0) ? "yes" : "no"));
      BoxHelper.AddStringLine (g, string.Format ("Price: {0} gold", _value));
      BoxHelper.DrawBox (g);
    }
  }
}
