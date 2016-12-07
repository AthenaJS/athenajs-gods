using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils03_WalkingWave : IObjectShared {
    public int _index;

    public Point _pixelXY;
    public int _facing;
    public int _functionIndexUnk;
    public int _delayBetweenSpawns;
    public int _nbEnemies;
    public int _health;
    public int _enemyInfoIndex;
    public int _missileType;
    public int _speedValue;
    public int _reward;

    public Alfils03_WalkingWave (byte [] buffer, ref int offset, int index) {
      _index = index;

      int pixelX = buffer.ReadWord (offset);
      offset += 2;
      int pixelY = buffer.ReadWord (offset);
      offset += 2;
      _pixelXY = new Point (pixelX, pixelY);
      _facing = buffer.ReadByte (offset++);
      _functionIndexUnk = buffer.ReadByte (offset++);
      _delayBetweenSpawns = buffer.ReadWord (offset);
      offset += 2;
      _nbEnemies = buffer.ReadByte (offset++);
      _health = buffer.ReadByte (offset++);
      _enemyInfoIndex = buffer.ReadWord (offset);
      offset += 2;
      _missileType = buffer.ReadByte (offset++);
      _speedValue = buffer.ReadByte (offset++);
      _reward = buffer.ReadByte (offset++);

      offset++;

      if (_missileType > 1)
        throw new Exception ();
      if (_speedValue > 14)
        throw new Exception ();
    }

    public EnemyInfo GetEnemyInfo (World world) {
      return (world.GetWalkingEnemyInfo (_enemyInfoIndex));
    }

    public int GetSpriteIndex (World world) {
      EnemyInfo enemyInfo = GetEnemyInfo (world);
      return (GetSpriteIndex (enemyInfo, world));
    }

    public int GetSpriteIndex (EnemyInfo enemyInfo, World world) {
      return (enemyInfo.GetSpriteIndex (world.Level, _facing));
    }

    public Bitmap GetBitmap (World world) {
      EnemyInfo enemyInfo = GetEnemyInfo (world);
      int spriteIndex = GetSpriteIndex (world);
      return (world.GetBitmap (spriteIndex));
    }

    public Bitmap GetBitmap (EnemyInfo enemyInfo, World world) {
      int spriteIndex = GetSpriteIndex (enemyInfo, world);
      return (world.GetBitmap (spriteIndex));
    }

    public Point PixelXY {
      get {
        return (_pixelXY);
      }
    }

    public bool HasReward {
      get {
        return (_reward != 0xFF);
      }
    }

    public bool IsObject {
      get {
        if (!HasReward)
          return false;
        if (_reward < 11)
          return (false);
        return (true);
      }
    }

    public bool IsWeapon {
      get {
        if (!HasReward)
          return false;
        if (_reward > 10)
          return (false);
        return (true);
      }
    }

    public int ObjectInfoIndex {
      get {
        if (!IsObject)
          throw new Exception ();
        return (_reward - 11);
      }
    }

    public int WeaponInfoIndex {
      get {
        if (!IsWeapon)
          throw new Exception ();
        return (_reward);
      }
    }

    public Point GetItemPixelXY (World world) {
      Bitmap bitmap = null;
      if (IsObject)
        bitmap = world.GetBitmapObject (ObjectInfoIndex);
      else if (IsWeapon)
        bitmap = world.GetBitmapWeapon (WeaponInfoIndex);
      else
        throw new Exception ();
      Bitmap bitmapEnemy = GetBitmap (world);
      return (new Point (_pixelXY.X + (bitmapEnemy.Width - bitmap.Width) / 2, _pixelXY.Y - bitmap.Height));
    }

    public string TitleComplement {
      get {
        return (string.Format ("(WALKING WAVE #{0})", _index));
      }
    }

    public void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      Bitmap bitmap = GetBitmap (world);
      if (world._drawType == DrawTypeEnum.DrawBitmaps) {
        g.DrawImage (bitmap, _pixelXY.X, _pixelXY.Y);
        g.DrawRectangle (Pens.Red, _pixelXY.X, _pixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
      }
      if (Main._clickPixelXY == Point.Empty) {
        Point centerXY = new Point (_pixelXY.X + bitmap.Width / 2, _pixelXY.Y + bitmap.Height / 2);
        if (world._drawType == DrawTypeEnum.DrawArrows)
          if (!fromXY.IsEmpty)
            g.DrawArrowOutlinedSolid (Pens.Red, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);

        if (!fromXY.IsEmpty)
          if (HasReward) {
            RewardItem_DrawOnMap (g, cellXY, world, Point.Empty);
            //RewardItem_DrawOnMap_Reverse (g, cellXY, world, Point.Empty, false);
          }
      }

      if (world._drawType == DrawTypeEnum.DrawBitmaps)
        if (Main._clickPixelXY != Point.Empty) {
          Point pixelXY = Main._clickPixelXY;
          g.DrawRectangle (Pens.White, World._xo + pixelXY.X, World._yo + pixelXY.Y, 320 - 1, 192 - 1);
          g.DrawRectangle (Pens.Silver, World._xo + pixelXY.X + 80, World._yo + pixelXY.Y + 32, 320 - 1 - 80 * 2, 192 - 1 - 32 * 2);
        }
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      EnemyInfo enemyInfo = GetEnemyInfo (world);
      Bitmap bitmap = GetBitmap (enemyInfo, world);
      if (world._drawType == DrawTypeEnum.DrawBitmaps) {
        g.DrawImage (bitmap, _pixelXY.X, _pixelXY.Y);
        g.DrawRectangle (Pens.Red, _pixelXY.X, _pixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
      }
      Point centerXY = new Point (_pixelXY.X + bitmap.Width / 2, _pixelXY.Y + bitmap.Height / 2);
      if (world._drawType == DrawTypeEnum.DrawArrows)
        if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Red, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);


      // OK: walking wave used in event
      foreach (Alfils02_Event event_ in world.GetEventsWithEffect (EventTypeEnum.SpawnWalkingWave_01, _index)) {
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }
    }

    public void RewardItem_DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      if (!fromXY.IsEmpty)
        DrawOnMap (g, cellXY, world, Point.Empty);
      ItemShared.DrawOnMap (g, cellXY, world, fromXY, this);
    }

    public void RewardItem_DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      ItemShared.DrawOnMap_Reverse (g, cellXY, world, toXY, state, this);
    }

    public void DrawBox (Graphics g, World world) {
      EnemyInfo enemyInfo = GetEnemyInfo (world);

      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("SPAWN WALKING WAVE #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddBitmap (g, GetBitmap (enemyInfo, world));
      BoxHelper.NextLine ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", _pixelXY.X, _pixelXY.Y));
      BoxHelper.AddStringLine (g, string.Format ("Nb enemies: {0}", _nbEnemies));
      BoxHelper.AddStringLine (g, string.Format ("Delay between spawns: {0:0.0} seconds ({1} frames)", World.FramesToSeconds_3VBL (_delayBetweenSpawns), _delayBetweenSpawns));
      BoxHelper.AddStringLine (g, string.Format ("Health: {0}", _health));
      BoxHelper.AddStringLine (g, string.Format ("Speed value: {0} (0-16)", _speedValue));
      BoxHelper.AddStringLine (g, string.Format ("Score: +{0} [=(health/2+1)*100]", World.CalcEnemyScoreValue (_health)), World._brushComputed);

      if (IsObject) {
        ObjectInfo objectInfo = world.GetObjectInfo (ObjectInfoIndex);
        BoxHelper.AddString (g, string.Format ("Reward: \"{0}\" ", objectInfo.FullName));
        BoxHelper.AddBitmap (g, objectInfo.GetBitmap (world));
        BoxHelper.AddStringLine (g, string.Format (" (object info #{0})", ObjectInfoIndex));
      }
      else if (IsWeapon) {
        WeaponInfo weaponInfo = world.GetWeaponInfo (WeaponInfoIndex);
        BoxHelper.AddString (g, string.Format ("Reward: \"{0}\" ", weaponInfo.FullName));
        BoxHelper.AddBitmap (g, weaponInfo.GetBitmap (world));
        BoxHelper.AddStringLine (g, string.Format (" (weapon info #{0})", WeaponInfoIndex));
      }
      else {
        BoxHelper.AddStringLine (g, string.Format ("Reward: none"));
      }

      BoxHelper.AddSeparation ();
      if (_speedValue == 0)
        BoxHelper.AddStringLine (g, string.Format ("Action type: no action"));
      else {
        BoxHelper.AddStringLine (g, string.Format ("Action type: {0}", enemyInfo._actionType));
        int actionRate = (16 - _speedValue) * 4;
        BoxHelper.AddStringLine (g, string.Format ("Action rate: {0:0.0} seconds ({1} frames) [=(16-speed value)*4]", World.FramesToSeconds_3VBL (actionRate), actionRate), World._brushComputed);

        if ((enemyInfo._actionType == EnemyActionTypeEnum.Walking_Fire_01) ||
            (enemyInfo._actionType == EnemyActionTypeEnum.Walking_Fire_02)) {
          if (_missileType == 0) {
            BoxHelper.AddString (g, string.Format ("Shot type: "));
            BoxHelper.AddBitmap (g, world.GetBitmap (69));
            BoxHelper.NextLine ();
            BoxHelper.AddStringLine (g, string.Format ("Target: none (fire ahead)"));
            int shotVelocity = 4 + (_speedValue >> 2);
            BoxHelper.AddStringLine (g, string.Format ("Shot velocity: {0} pixels/frame [=4+(speed value/4)]", shotVelocity), World._brushComputed);
          }
          else {
            BoxHelper.AddString (g, string.Format ("Shot type: "));
            BoxHelper.AddBitmap (g, world.GetBitmap (75));
            BoxHelper.NextLine ();
            BoxHelper.AddStringLine (g, string.Format ("Target: the player"));
            int shotVelocity = 2 + (_speedValue >> 1);
            BoxHelper.AddStringLine (g, string.Format ("Shot velocity: {0} pixels/frame [=2+(speed value/2)]", shotVelocity), World._brushComputed);
          }
        }
      }
      BoxHelper.DrawBox (g);

      //enemyInfo.DrawBox (g, world);

      if (IsObject) {
        ObjectInfo objectInfo = world.GetObjectInfo (ObjectInfoIndex);
        objectInfo.DrawBox (g, world);
      }
      else if (IsWeapon) {
        WeaponInfo weaponInfo = world.GetWeaponInfo (WeaponInfoIndex);
        weaponInfo.DrawBox (g, world);
      }
    }
  }
}
