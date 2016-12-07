using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils04b_IntelWalkingWave : IObjectShared {
    public int _index;

    public int _nbEnemies;
    public int _health;
    public int _field_2;
    public int _field_3;
    public int _flags;
    public int _shotVelocity_fireRateValue;
    public int _enemyInfoIndex;
    public int _objectives;
    public int _objectiveBalance;
    public int _field_A;
    public int _speedValue;
    public int _jumpValue;
    public int _distanceMaxToScan;
    public int _field_E;
    public int _field_F;
    public int _reward;
    public int _cellXY;
    public int _delayBetweenSpawns;
    public int _field_15;

    public int _facing;
    public int _fixedLeader;
    public int _followTheLeaderType;
    public int _missileType;
    public int _pickUpWeaponType;
    public int _pickUpObjectType;
    public Point _pixelXY;
    public int _shotVelocity;
    public int _fireRateValue;
    public IntelWalkingObjectiveEnum _primaryObjective;
    public IntelWalkingObjectiveEnum _secondaryObjective;

    public Alfils04b_IntelWalkingWave (byte [] buffer, ref int offset, int index) {
      _index = index;

      _nbEnemies = buffer.ReadByte (offset++);
      _health = buffer.ReadByte (offset++);
      _field_2 = buffer.ReadByte (offset++);
      _field_3 = buffer.ReadByte (offset++);
      _flags = buffer.ReadWord (offset);
      offset += 2;
      _shotVelocity_fireRateValue = buffer.ReadByte (offset++);
      _enemyInfoIndex = buffer.ReadByte (offset++);
      _objectives = buffer.ReadByte (offset++);
      _objectiveBalance = buffer.ReadByte (offset++);
      _field_A = buffer.ReadByte (offset++);
      _speedValue = buffer.ReadByte (offset++);
      _jumpValue = buffer.ReadByte (offset++);
      _distanceMaxToScan = buffer.ReadByte (offset++);
      _field_E = buffer.ReadByte (offset++);
      _field_F = buffer.ReadByte (offset++);
      _reward = buffer.ReadWord (offset);
      offset += 2;
      _cellXY = buffer.ReadWord (offset);
      offset += 2;
      _delayBetweenSpawns = buffer.ReadByte (offset++);
      _field_15 = buffer.ReadByte (offset++);

      // flags: ??????A? XXXXYYZZ
      _facing = (_flags >> 9) & 0x0001;
      _fixedLeader = (_flags >> 8) & 0x0001;
      _followTheLeaderType = (_flags >> 6) & 0x0003;
      _missileType = (_flags >> 4) & 0x0003;
      _pickUpWeaponType = (_flags >> 2) & 0x0003;
      _pickUpObjectType = (_flags >> 0) & 0x0003;

      int pixelX = ((_cellXY >> 8) * 32) & 0xFFE0;
      int pixelY = ((_cellXY & 0x00FF) * 16) & 0xFFF0;
      _pixelXY = new Point (pixelX, pixelY);

      _shotVelocity = _shotVelocity_fireRateValue >> 4;
      _fireRateValue = _shotVelocity_fireRateValue & 0x0F;

      _primaryObjective = (IntelWalkingObjectiveEnum) (_objectives & 0x0F);
      if (!Enum.IsDefined (typeof (IntelWalkingObjectiveEnum), _primaryObjective))
        throw new Exception ();
      _secondaryObjective = (IntelWalkingObjectiveEnum) (_objectives >> 4);
      if (!Enum.IsDefined (typeof (IntelWalkingObjectiveEnum), _secondaryObjective))
        throw new Exception ();
      if (_objectiveBalance > 127)
        throw new Exception ();

      if (_speedValue > 10)
        throw new Exception ();
      if (_jumpValue > 10)
        throw new Exception ();
      if (_pickUpObjectType >= 3)
        throw new Exception ();
      if (_pickUpWeaponType >= 3)
        throw new Exception ();
      if (_fixedLeader != 0)
        throw new Exception ();
      //if (_followTheLeaderType != 0)
      //    throw new Exception ();
      if (_missileType > 2)
        throw new Exception ();
      //if (_field_A != 0)
      //  throw new Exception ();
      //if (_primaryObjective == IntelWalkingObjectiveEnum.GoToLocation_3)
      //  throw new Exception ();
      //if (_secondaryObjective == IntelWalkingObjectiveEnum.GoToLocation_3)
      //  throw new Exception ();

      //if ((_primaryObjective == IntelWalkingObjectiveEnum.GoToObjectiveLocation_3) ||
      //    (_secondaryObjective == IntelWalkingObjectiveEnum.GoToObjectiveLocation_3)) {
      //  Console.WriteLine (string.Format ("(x,y): ({0},{1})", _pixelXY.X, _pixelXY.Y));
      //}
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

    public bool HasReward {
      get {
        return (_reward != 0xFFFF);
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
        return (string.Format ("(INTEL WALKING WAVE #{0})", _index));
      }
    }

    public void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      EnemyInfo enemyInfo = GetEnemyInfo (world);
      Bitmap bitmap = GetBitmap (enemyInfo, world);
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

      // OK: intel flying wave used in event
      foreach (Alfils02_Event event_ in world.GetEventsWithEffect (EventTypeEnum.SpawnIntelFlyingWave_04, _index)) {
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
      BoxHelper.AddString (g, string.Format ("SPAWN INTEL WALKING WAVE #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddBitmap (g, GetBitmap (enemyInfo, world));
      BoxHelper.NextLine ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", _pixelXY.X, _pixelXY.Y));
      BoxHelper.AddStringLine (g, string.Format ("Nb enemies: {0}", _nbEnemies));
      BoxHelper.AddStringLine (g, string.Format ("Delay between spawns: {0:0.0} seconds ({1} frames)", World.FramesToSeconds_3VBL (_delayBetweenSpawns), _delayBetweenSpawns));
      BoxHelper.AddStringLine (g, string.Format ("Health: {0}", _health));
      BoxHelper.AddStringLine (g, string.Format ("Speed value: {0} (0-10)", _speedValue));
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
      if (_fireRateValue == 0)
        BoxHelper.AddStringLine (g, string.Format ("Fire rate value: 0 (0-16) (cannot fire)"));
      else {
        BoxHelper.AddStringLine (g, string.Format ("Fire rate value: {0} (0-16)", _fireRateValue));
        int _fireRate = (16 - _fireRateValue) * 4;
        BoxHelper.AddStringLine (g, string.Format ("Fire rate: {0:0.0} seconds ({1} frames) [=(16-fire rate value)*4]", World.FramesToSeconds_3VBL (_fireRate), _fireRate), World._brushComputed);
        if (_missileType == 0) {
          BoxHelper.AddString (g, string.Format ("Shot type: "));
          BoxHelper.AddBitmap (g, world.GetBitmap (69));
          BoxHelper.NextLine ();
          BoxHelper.AddStringLine (g, string.Format ("Target: none (fire ahead)"));
          BoxHelper.AddStringLine (g, string.Format ("Shot velocity: {0} pixels/frame", _shotVelocity));
        }
        else if (_missileType == 1) {
          BoxHelper.AddString (g, string.Format ("Shot type: "));
          BoxHelper.AddBitmap (g, world.GetBitmap (75));
          BoxHelper.NextLine ();
          BoxHelper.AddStringLine (g, string.Format ("Target: the player"));
          BoxHelper.AddStringLine (g, string.Format ("Shot velocity: {0} pixels/frame", _shotVelocity));
        }
        else if (_missileType == 2) {
          BoxHelper.AddString (g, string.Format ("Shot type: "));
          BoxHelper.AddBitmap (g, world.GetBitmap (75));
          BoxHelper.NextLine ();
          BoxHelper.AddStringLine (g, string.Format ("Target: random in 64x64 pixels square centered on player"));
          BoxHelper.AddStringLine (g, string.Format ("Shot velocity: {0} pixels/frame", _shotVelocity));
        }
      }

      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("Path depth: {0} (0-127)", _distanceMaxToScan));
      if (_speedValue == 10)
        BoxHelper.AddStringLine (g, string.Format ("Move every frame [=11-speed value]"), World._brushComputed);
      else
        BoxHelper.AddStringLine (g, string.Format ("Move every {0} frames [=11-speed value]", 11 - _speedValue), World._brushComputed);
      BoxHelper.AddStringLine (g, string.Format ("Jump value: {0} (0-10)", _jumpValue));
      BoxHelper.AddStringLine (g, string.Format ("Jump height: {0} [<=4: low (2 blocks), >=5: high (5 blocks)]", (_jumpValue <= 4) ? "low (2 blocks)" : "high (5 blocks)"), World._brushComputed);
      BoxHelper.AddStringLine (g, string.Format ("Pickup object: {0}", World.GetPickupType (_pickUpObjectType)));
      BoxHelper.AddStringLine (g, string.Format ("Pickup weapon: {0}", World.GetPickupType (_pickUpWeaponType)));

      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("Primary objective: {0}", _primaryObjective));
      BoxHelper.AddStringLine (g, string.Format ("Secondary objective: {0}", _secondaryObjective));
      BoxHelper.AddStringLine (g, string.Format ("Objective balance: {0}% ({1}/127)", _objectiveBalance * 100 / 127, _objectiveBalance));
      BoxHelper.DrawBox (g);

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

  public enum IntelWalkingObjectiveEnum {
    GoToPlayer_0,
    StayAway_1,
    PickUp_2,
    GoToObjectiveLocation_3
  }
}
