using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils01_FlyingWave : IObjectShared {
    public int _index;

    public int _nbEnemies;
    public int _indexInGod0x_pat;
    public int _delayBetweenSpawns;
    public int _health;
    public int _enemyInfoIndex;
    public int _missileType_speed;
    public int _reward;

    public int _missileType;
    public int _speedValue;

    public Alfils01_FlyingWave (byte [] buffer, ref int offset, int index) {
      _index = index;

      _nbEnemies = buffer.ReadByte (offset++);
      _indexInGod0x_pat = buffer.ReadByte (offset++);
      _delayBetweenSpawns = buffer.ReadWord (offset);
      offset += 2;
      _health = buffer.ReadByte (offset++);
      _enemyInfoIndex = buffer.ReadByte (offset++);
      _missileType_speed = buffer.ReadByte (offset++);
      _reward = buffer.ReadByte (offset++);

      _missileType = (_missileType_speed >> 4) & 0x0F;
      _speedValue = (_missileType_speed >> 0) & 0x0F;

      if (_missileType > 1)
        throw new Exception ();
      if (_speedValue > 14)
        throw new Exception ();
    }

    public EnemyInfo GetEnemyInfo (World world) {
      return (world.GetFlyingEnemyInfo (_enemyInfoIndex));
    }

    public int GetSpriteIndex (World world) {
      EnemyInfo enemyInfo = GetEnemyInfo (world);
      return (GetSpriteIndex (enemyInfo, world));
    }

    public int GetSpriteIndex (EnemyInfo enemyInfo, World world) {
      return (enemyInfo._spriteIndex);
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
      Point pixelXY = new Point (Main._cellXY.X * 32 + 16, Main._cellXY.Y * 16 + 8);
      return (GetItemPixelXY (pixelXY, world));
    }

    public Point GetItemPixelXY (Point pixelXY, World world) {
      Bitmap bitmap = null;
      if (IsObject)
        bitmap = world.GetBitmapObject (ObjectInfoIndex);
      else if (IsWeapon)
        bitmap = world.GetBitmapWeapon (WeaponInfoIndex);
      else
        throw new Exception ();
      Point firstPixelXY = GetFlyingPath (world).GetFirstPixelXY (pixelXY);
      Bitmap bitmapEnemy = GetBitmap (world);
      return (new Point (firstPixelXY.X + (bitmapEnemy.Width - bitmap.Width) / 2, firstPixelXY.Y - bitmap.Height));
    }

    public string TitleComplement {
      get {
        return (string.Format ("(FLYING WAVE #{0})", _index));
      }
    }

    public FlyingPath GetFlyingPath (World world) {
      return (world.GetFlyingPath (_indexInGod0x_pat));
    }

    public void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      Point pixelXY = new Point (cellXY.X * 32 + 16, cellXY.Y * 16 + 8);
      if (Main._clickPixelXY != Point.Empty) {
        pixelXY = Main._clickPixelXY;
      }
      FlyingPath flyingPath = GetFlyingPath (world);
      Point firstPixelXY = flyingPath.GetFirstPixelXY (pixelXY);

      EnemyInfo enemyInfo = GetEnemyInfo (world);
      Bitmap bitmap = GetBitmap (enemyInfo, world);
      if (world._drawType == DrawTypeEnum.DrawBitmaps) {
        g.DrawImage (bitmap, firstPixelXY.X, firstPixelXY.Y);
        flyingPath.Draw (g, pixelXY.X + bitmap.Width / 2, pixelXY.Y + bitmap.Height / 2);
        g.DrawRectangle (Pens.Red, firstPixelXY.X, firstPixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
      }

      if (Main._clickPixelXY == Point.Empty) {
        Point centerXY = new Point (firstPixelXY.X + bitmap.Width / 2, firstPixelXY.Y + bitmap.Height / 2);
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
          g.DrawRectangle (Pens.White, World._xo + pixelXY.X, World._yo + pixelXY.Y, 320 - 1, 192 - 1);
          g.DrawRectangle (Pens.Silver, World._xo + pixelXY.X + 80, World._yo + pixelXY.Y + 32, 320 - 1 - 80 * 2, 192 - 1 - 32 * 2);
        }
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      Point pixelXY = new Point (cellXY.X * 32 + 16, cellXY.Y * 16 + 8);
      FlyingPath flyingPath = GetFlyingPath (world);
      Point firstPixelXY = flyingPath.GetFirstPixelXY (pixelXY);

      EnemyInfo enemyInfo = GetEnemyInfo (world);
      Bitmap bitmap = GetBitmap (enemyInfo, world);
      if (world._drawType == DrawTypeEnum.DrawBitmaps) {
        g.DrawImage (bitmap, firstPixelXY.X, firstPixelXY.Y);
        flyingPath.Draw (g, pixelXY.X + bitmap.Width / 2, pixelXY.Y + bitmap.Height / 2);
        g.DrawRectangle (Pens.Red, firstPixelXY.X, firstPixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
      }
      Point centerXY = new Point (firstPixelXY.X + bitmap.Width / 2, firstPixelXY.Y + bitmap.Height / 2);
      if (world._drawType == DrawTypeEnum.DrawArrows)
        if (!toXY.IsEmpty)
          g.DrawArrowOutlinedState (Pens.Red, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      // OK: flying wave used in event
      foreach (Alfils02_Event event_ in world.GetEventsWithEffect (EventTypeEnum.SpawnFlyingWave_00, _index)) {
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
      BoxHelper.AddString (g, string.Format ("SPAWN FLYING WAVE #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddBitmap (g, enemyInfo.GetBitmap (world));
      BoxHelper.NextLine ();
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
      FlyingPath flyingPath = GetFlyingPath (world);
      BoxHelper.AddStringLine (g, string.Format ("Flying path: #{0}", _indexInGod0x_pat));
      BoxHelper.AddStringLine (g, string.Format ("Type: {0}", Enum.GetName (typeof (FlyingPath.TypeEnum), flyingPath._type)));
      BoxHelper.AddStringLine (g, string.Format ("Nodes: {0}", flyingPath._deltaX.Length));

      BoxHelper.AddSeparation ();
      if (_speedValue == 0)
        BoxHelper.AddStringLine (g, string.Format ("Action type: no action"));
      else {
        BoxHelper.AddStringLine (g, string.Format ("Action type: {0}", enemyInfo._actionType));
        int actionRate = (16 - _speedValue) * 2;
        BoxHelper.AddStringLine (g, string.Format ("Action rate: {0:0.0} seconds ({1} frames) [=(16-speed value)*2]", World.FramesToSeconds_3VBL (actionRate), actionRate), World._brushComputed);

        if (enemyInfo._actionType == EnemyActionTypeEnum.Flying_Fire_01) {
          if (_missileType == 0) {
            BoxHelper.AddString (g, string.Format ("Shot type: "));
            BoxHelper.AddBitmap (g, world.GetBitmap (69));
            BoxHelper.NextLine ();
            BoxHelper.AddStringLine (g, string.Format ("Target: none (fire ahead)"));
            int shotVelocity = 4 + (_speedValue >> 1);
            BoxHelper.AddStringLine (g, string.Format ("Shot velocity: {0} pixels/frame [=4+(speed value/2)]", shotVelocity), World._brushComputed);
          }
          else {
            BoxHelper.AddString (g, string.Format ("Shot type: "));
            BoxHelper.AddBitmap (g, world.GetBitmap (75));
            BoxHelper.NextLine ();
            BoxHelper.AddStringLine (g, string.Format ("Target: the player"));
            int shotVelocity = 4 + (_speedValue >> 1);
            BoxHelper.AddStringLine (g, string.Format ("Shot velocity: {0} pixels/frame [=4+(speed value/2)]", shotVelocity), World._brushComputed);
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
