using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class MapPuzzle : IObjectShared {
    public int _index;

    public int [] _conditionsFunctionIndex;
    public int [] _conditionsParam;
    public Point _pixelXY;
    public int _effectFunctionIndex_remove;
    public int _effectParam;
    public int _stringIndex;

    public int _effectFunctionIndex;
    public bool _remove;

    public PuzzleConditionTypeParam [] _conditionsTypeParam;
    public PuzzleEffectTypeEnum _effectType;

    public MapPuzzle (byte [] buffer, ref int offset, int index) {
      _index = index;

      _conditionsFunctionIndex = new int [3];
      _conditionsParam = new int [3];
      for (int i = 0; i < 3; i++) {
        _conditionsFunctionIndex [i] = buffer.ReadWord (offset);
        offset += 2;
        _conditionsParam [i] = buffer.ReadWord (offset);
        offset += 2;
      }
      int pixelX = buffer.ReadWord (offset);
      offset += 2;
      int pixelY = buffer.ReadWord (offset);
      offset += 2;
      _pixelXY = new Point (pixelX, pixelY);
      _effectFunctionIndex_remove = buffer.ReadWord (offset);
      offset += 2;
      _effectParam = buffer.ReadWord (offset);
      offset += 2;
      _stringIndex = buffer.ReadWord (offset);
      offset += 2;

      offset += 2;

      _conditionsTypeParam = new PuzzleConditionTypeParam [3];
      for (int i = 0; i < 3; i++) {
        _conditionsTypeParam [i] = new PuzzleConditionTypeParam ((ConditionTypeEnum) _conditionsFunctionIndex [i], _conditionsParam [i]);
      }
      _effectFunctionIndex = _effectFunctionIndex_remove & 0x7FFF;
      _effectType = (PuzzleEffectTypeEnum) _effectFunctionIndex;
      _remove = (_effectFunctionIndex_remove & 0x8000) != 0;
    }

    public void Dump (World world) {
      foreach (PuzzleConditionTypeParam conditionTypeParam in _conditionsTypeParam) {
        Console.WriteLine ("Condition: {0} {1}", Enum.GetName (typeof (ConditionTypeEnum), conditionTypeParam._type), conditionTypeParam._param);
      }
      Console.WriteLine ("Effect: {0} {1}", Enum.GetName (typeof (PuzzleEffectTypeEnum), _effectType), _effectParam);
      Console.WriteLine ("Remove: {0}", _remove);
      Console.WriteLine ("Message: {0}", world.GetPuzzleMessage (_stringIndex));
      Console.WriteLine ("(x/y): {0}/{1}", _pixelXY.X, _pixelXY.Y);
    }

    public bool IsCandidateForRemove (ConditionTypeEnum conditionType, int conditionParam, World world) {
      if ((_effectType == PuzzleEffectTypeEnum.OpenDoor_02) ||
          (_effectType == PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) ||
          (_effectType == PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04) ||
          (_effectType == PuzzleEffectTypeEnum.OpenTrapdoor_07) ||
          (_effectType == PuzzleEffectTypeEnum.CloseTrapdoor_08)) {
        if (conditionType == ConditionTypeEnum.Carrying_01) {
          //MapObject mapObject = world.GetMapObject (conditionParam);
          //ObjectInfo objectInfo = world.GetObjectInfo (conditionParam);
          //if (mapObject.IsObject) {
          if (ObjectInfo.IsKey (conditionParam)) {
            return (true);
          }
          //}
        }
      }

      if (_remove) {
        if (conditionType == ConditionTypeEnum.Carrying_01) { // reward
          // inventory usable or not
          ObjectInfo objectInfo = world.GetObjectInfo (conditionParam);
          if ((objectInfo.Type == ObjectInfoTypeEnum.Usable_00) ||
              (objectInfo.Type == ObjectInfoTypeEnum.Pickable_02)) {
            return (true);
          }
        }
      }
      return (false);
    }

    public Alfils02_Event GetEvent (World world) {
      List<Alfils02_Event> events = world.GetEventsWithEffect (EventTypeEnum.CheckPuzzle_02, _index);
      if (events.Count > 1)
        throw new Exception ();
      if (events.Count == 0)
        return (null);
      return (events [0]);
    }

    public bool IsObject {
      get {
        if (_effectType == PuzzleEffectTypeEnum.SpawnObject_00)
          return (true);
        return (false);
      }
    }

    public bool IsWeapon {
      get {
        if (_effectType == PuzzleEffectTypeEnum.SpawnWeapon_01)
          return (true);
        return (false);
      }
    }

    public int ObjectInfoIndex {
      get {
        if (!IsObject)
          throw new Exception ();
        return (_effectParam);
      }
    }

    public int WeaponInfoIndex {
      get {
        if (!IsWeapon)
          throw new Exception ();
        return (_effectParam);
      }
    }

    //public Bitmap GetBitmapObject (World world) {
    //  if (_effectType != PuzzleEffectTypeEnum.SpawnObject_00)
    //    throw new Exception ();
    //  return (world.GetBitmapObject (ObjectInfoIndex));
    //}

    //public Bitmap GetBitmapWeapon (World world) {
    //  if (_effectType != PuzzleEffectTypeEnum.SpawnWeapon_01)
    //    throw new Exception ();
    //  return (world.GetBitmapWeapon (WeaponInfoIndex));
    //}

    public Point ItemPixelXY {
      get {
        return (_pixelXY);
      }
    }

    public Point GetItemPixelXY (World world) {
      return (ItemPixelXY);
    }

    public string TitleComplement {
      get {
        return (string.Format ("(PUZZLE #{0})", _index));
      }
    }

    //*************************************************************************
    public void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      if (_effectType != PuzzleEffectTypeEnum.NoEffect_13) {
        if (_effectType == PuzzleEffectTypeEnum.SpawnObject_00) {
          SpawnedItem_DrawOnMap (g, cellXY, world, fromXY);
        }
        else if (_effectType == PuzzleEffectTypeEnum.SpawnWeapon_01) {
          SpawnedItem_DrawOnMap (g, cellXY, world, fromXY);
        }
        else if ((_effectType == PuzzleEffectTypeEnum.OpenDoor_02) ||
                 (_effectType == PuzzleEffectTypeEnum.CloseDoor_09)) {
          Door_DrawOnMap (g, world, fromXY);
        }
        else if ((_effectType == PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) ||
                 (_effectType == PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04)) {
          Backdoor_DrawOnMap (g, world, fromXY);
        }
        else if (_effectType == PuzzleEffectTypeEnum.TriggerEvent_05) {
          Alfils02_Event event_ = world.GetEvent (_effectParam - 1);
          if (event_ != null)
            event_.DrawOnMap (g, cellXY, world, null, false);

          //bool found = false;
          //foreach (Point point in world.KO_GetCellsTriggeringEvent (_effectParam - 1)) {
          //  found = true;
          //  Alfils02_Event event_ = world.KO_GetEventAtCell (point);
          //  event_.DrawOnMap (g, point, world, fromXY);
          //}
          //if (!found) {
          //  Alfils02_Event event_ = world.GetEvent (_effectParam - 1);
          //  if (event_ != null)
          //    event_.DrawOnMap (g, cellXY, world, fromXY);
          //}
        }
        else if (_effectType == PuzzleEffectTypeEnum.DestroyType4_06) {
          Explosion_DrawOnMap (g, world, fromXY);
        }
        else if ((_effectType == PuzzleEffectTypeEnum.OpenTrapdoor_07) ||
                 (_effectType == PuzzleEffectTypeEnum.CloseTrapdoor_08)) {
          Alfils07_Trapdoor trapdoor = world.GetTrapdoorAt (_pixelXY);
          if (trapdoor != null)
            trapdoor.DrawOnMap (g, world, fromXY);
        }
        else if (_effectType == PuzzleEffectTypeEnum.RemoveWeapon_10) {
        }
        else if (_effectType == PuzzleEffectTypeEnum.EnableRaster_11) {
        }
        else if (_effectType == PuzzleEffectTypeEnum.DisableRaster_12) {
        }
        else if (_effectType == PuzzleEffectTypeEnum.ResetGlobalTimer_14) {
        }
        else {
          throw new Exception ();
        }
      }
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      Alfils02_Event event_ = GetEvent (world);
      if (event_ == null)
        return;
      event_.DrawOnMap_Reverse (g, cellXY, world, toXY, state);
    }

    public void DrawOnMap_Reverse_FromEvent (Graphics g, Point cellXY, World world, Point toXY, bool state) {

      foreach (PuzzleConditionTypeParam conditionTypeParam in _conditionsTypeParam) {
        if (conditionTypeParam._type == ConditionTypeEnum.True_00) {
          continue;
        }
        else if ((conditionTypeParam._type == ConditionTypeEnum.Carrying_01) ||
                 (conditionTypeParam._type == ConditionTypeEnum.NotCarrying_02)) {
          // map object used in puzzle condition (carried)
          foreach (MapItem mapItem in ItemShared.GetItems (conditionTypeParam._param, world._map._mapItems)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Carrying_01)
              mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // spawned object used in puzzle condition (carried)
          foreach (MapPuzzle puzzle in ItemShared.GetItems (conditionTypeParam._param, world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.SpawnObject_00, -1))) {
            if (conditionTypeParam._type == ConditionTypeEnum.Carrying_01)
              puzzle.SpawnedItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              puzzle.SpawnedItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (walking) object used in puzzle condition (carried)
          foreach (Alfils03_WalkingWave wave in ItemShared.GetItems (conditionTypeParam._param, world._walkingWaves._walkingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Carrying_01)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (flying) object used in puzzle condition (carried)
          foreach (Alfils01_FlyingWave wave in ItemShared.GetItems (conditionTypeParam._param, world._flyingWaves._flyingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Carrying_01)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (intel walking) object used in puzzle condition (carried)
          foreach (Alfils04b_IntelWalkingWave wave in ItemShared.GetItems (conditionTypeParam._param, world._intelWalkingWaves._intelWalkingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Carrying_01)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (intel flying) object used in puzzle condition (carried)
          foreach (Alfils04a_IntelFlyingWave wave in ItemShared.GetItems (conditionTypeParam._param, world._intelFlyingWaves._intelFlyingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Carrying_01)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
        }
        else if ((conditionTypeParam._type == ConditionTypeEnum.Holding_03) ||
                 (conditionTypeParam._type == ConditionTypeEnum.NotHolding_04)) {
          // map weapon used in puzzle condition (held)
          foreach (MapItem mapItem in ItemShared.GetWeapons (conditionTypeParam._param, world._map._mapItems)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Holding_03)
              mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // spawned weapon used in puzzle condition (carried)
          foreach (MapPuzzle puzzle in ItemShared.GetWeapons (conditionTypeParam._param, world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.SpawnWeapon_01, -1))) {
            if (conditionTypeParam._type == ConditionTypeEnum.Holding_03)
              puzzle.SpawnedItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              puzzle.SpawnedItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (walking) weapon used in puzzle condition (carried)
          foreach (Alfils03_WalkingWave wave in ItemShared.GetWeapons (conditionTypeParam._param, world._walkingWaves._walkingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Holding_03)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (flying) weapon used in puzzle condition (carried)
          foreach (Alfils01_FlyingWave wave in ItemShared.GetWeapons (conditionTypeParam._param, world._flyingWaves._flyingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Holding_03)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (intel walking) weapon used in puzzle condition (carried)
          foreach (Alfils04b_IntelWalkingWave wave in ItemShared.GetWeapons (conditionTypeParam._param, world._intelWalkingWaves._intelWalkingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Holding_03)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
          // rewarded (intel flying) weapon used in puzzle condition (carried)
          foreach (Alfils04a_IntelFlyingWave wave in ItemShared.GetWeapons (conditionTypeParam._param, world._intelFlyingWaves._intelFlyingWaves)) {
            if (conditionTypeParam._type == ConditionTypeEnum.Holding_03)
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          }
        }
        else if ((conditionTypeParam._type == ConditionTypeEnum.EventTriggered_05) ||
                 (conditionTypeParam._type == ConditionTypeEnum.EventNotTriggered_06)) {
          Alfils02_Event event_ = world.GetEvent (conditionTypeParam._param - 1);
          //if (event_ != null) {
          if (event_ != null)
            if (conditionTypeParam._type == ConditionTypeEnum.EventTriggered_05)
              event_.DrawOnMap_Reverse (g, cellXY, world, toXY, false);
            else
              event_.DrawOnMap_Reverse (g, cellXY, world, toXY, true);
          //}
          //List<Point> cells = world.KO_GetCellsTriggeringEvent (conditionTypeParam._param - 1);
          //foreach (Point point in cells) {
          //  if (conditionTypeParam._type == ConditionTypeEnum.EventTriggered_05)
          //    g.DrawRectangle (Pens.Magenta, point.X * 32, point.Y * 16, 32 - 1, 16 - 1);
          //  else
          //    g.DrawRectangleCrossed (Pens.Magenta, point.X * 32, point.Y * 16, 32 - 1, 16 - 1);
          //  g.DrawArrowOutlinedSolid (Pens.Magenta, point.X * 32 + 16, point.Y * 16 + 8, cellXY.X * 32 + 16, cellXY.Y * 16 + 8);
          //}
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.HealthSup_07) {
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.HealthInf_08) {
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.TimeSup_09) {
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.TimeInf_10) {
        }
        else if ((conditionTypeParam._type == ConditionTypeEnum.SwitchOn_11) ||
                 (conditionTypeParam._type == ConditionTypeEnum.SwitchOff_12)) {
          Alfils05_Switch switch_ = world.GetSwitch (conditionTypeParam._param);
          if (switch_ != null) {
            foreach (MapItem mapItem in ItemShared.GetItems (switch_._objectInfoIndex, world._map._mapItems)) {
              if ((mapItem.ItemPixelXY.X != switch_._pixelX) || (mapItem.ItemPixelXY.Y != switch_._pixelY))
                continue;
              if (conditionTypeParam._type == ConditionTypeEnum.SwitchOn_11)
                mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, toXY, false);
              else
                mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, toXY, true);
            }
          }
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.ScoreSup_13) {
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.ScoreInf_14) {
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.LivesSup_15) {
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.LivesInf_16) {
        }
        else {
          throw new Exception ();
        }
      }
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("CHECK PUZZLE #{0}", _index));
      BoxHelper.AddSeparation ();
      //bool hasAtLeast1Condition = false;
      foreach (PuzzleConditionTypeParam conditionTypeParam in _conditionsTypeParam) {
        string textCondition = null;
        int objectInfoIndex = -1;
        int weaponInfoIndex = -1;
        if (conditionTypeParam._type == ConditionTypeEnum.True_00) {
          continue;
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.Carrying_01) {
          objectInfoIndex = conditionTypeParam._param;
          ObjectInfo objectInfo = world.GetObjectInfo (objectInfoIndex);
          textCondition = string.Format ("carrying \"{0}\" ", objectInfo.FullName);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.NotCarrying_02) {
          objectInfoIndex = conditionTypeParam._param;
          ObjectInfo objectInfo = world.GetObjectInfo (objectInfoIndex);
          textCondition = string.Format ("NOT carrying \"{0}\" ", objectInfo.FullName);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.Holding_03) {
          weaponInfoIndex = conditionTypeParam._param;
          WeaponInfo weaponInfo = world.GetWeaponInfo (weaponInfoIndex);
          textCondition = string.Format ("holding \"{0}\" ", weaponInfo.FullName);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.NotHolding_04) {
          weaponInfoIndex = conditionTypeParam._param;
          WeaponInfo weaponInfo = world.GetWeaponInfo (weaponInfoIndex);
          textCondition = string.Format ("NOT holding \"{0}\" ", weaponInfo.FullName);
        }
        else if ((conditionTypeParam._type == ConditionTypeEnum.EventTriggered_05) ||
                 (conditionTypeParam._type == ConditionTypeEnum.EventNotTriggered_06)) {
          //Alfils02_Event event_ = world.GetEvent (conditionTypeParam._param - 1);
          if (conditionTypeParam._type == ConditionTypeEnum.EventTriggered_05) {
            textCondition = string.Format ("event #{0} triggered", conditionTypeParam._param - 1);
          }
          else {
            textCondition = string.Format ("event #{0} NOT triggered", conditionTypeParam._param - 1);
          }

          //List<Point> cells = world.KO_GetCellsTriggeringEvent (conditionTypeParam._param - 1);
          //if (cells.Count == 0) {
          //  textCondition += string.Format (" MISSING");
          //}
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.HealthSup_07) {
          textCondition = string.Format ("health > {0}/24", conditionTypeParam._param);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.HealthInf_08) {
          textCondition = string.Format ("health < {0}/24", conditionTypeParam._param);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.TimeSup_09) {
          textCondition = string.Format ("time > {0} seconds", conditionTypeParam._param * 5);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.TimeInf_10) {
          textCondition = string.Format ("time < {0} seconds", conditionTypeParam._param * 5);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.SwitchOn_11) {
          int switchIndex = conditionTypeParam._param;
          Alfils05_Switch switch_ = world.GetSwitch (switchIndex);
          textCondition = string.Format ("switch #{0} is ON", switchIndex);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.SwitchOff_12) {
          int switchIndex = conditionTypeParam._param;
          Alfils05_Switch switch_ = world.GetSwitch (switchIndex);
          textCondition = string.Format ("switch #{0} is OFF", switchIndex);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.ScoreSup_13) {
          textCondition = string.Format ("score > {0}", conditionTypeParam._param * 5000);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.ScoreInf_14) {
          textCondition = string.Format ("score < {0}", conditionTypeParam._param * 5000);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.LivesSup_15) {
          textCondition = string.Format ("lives > {0}", conditionTypeParam._param);
        }
        else if (conditionTypeParam._type == ConditionTypeEnum.LivesInf_16) {
          textCondition = string.Format ("lives < {0}", conditionTypeParam._param);
        }
        else {
          throw new Exception ();
        }
        //hasAtLeast1Condition = true;

        // popup
        textCondition = string.Format ("Condition: {0}", textCondition);
        BoxHelper.AddString (g, textCondition);

        if (objectInfoIndex != -1) {
          Bitmap bitmap = world.GetBitmapObject (objectInfoIndex);
          BoxHelper.AddBitmap (g, bitmap);
          BoxHelper.AddString (g, string.Format (" (object info #{0})", objectInfoIndex));
        }

        if (weaponInfoIndex != -1) {
          Bitmap bitmap = world.GetBitmapWeapon (weaponInfoIndex);
          BoxHelper.AddBitmap (g, bitmap);
          BoxHelper.AddString (g, string.Format (" (weapon info #{0})", weaponInfoIndex));
        }

        if (IsCandidateForRemove (conditionTypeParam._type, conditionTypeParam._param, world)) {
          textCondition = string.Format (" (removed)");
          BoxHelper.AddString (g, textCondition);
        }
        BoxHelper.NextLine ();
      }
      //if (!hasAtLeast1Condition)
      //  BoxHelper.AddStringLine (g, "Condition: none");

      {
        Alfils02_Event eventToDraw = null;
        int objectInfoIndex = -1;
        int weaponInfoIndex = -1;
        if (_effectType == PuzzleEffectTypeEnum.NoEffect_13)
          BoxHelper.AddStringLine (g, "Effect: none");
        else {
          string textEffect = null;
          if (_effectType == PuzzleEffectTypeEnum.SpawnObject_00) {
            ObjectInfo objectInfo = world.GetObjectInfo (_effectParam);
            textEffect = string.Format ("spawn object \"{0}\" ", objectInfo.FullName);
            objectInfoIndex = _effectParam;
          }
          else if (_effectType == PuzzleEffectTypeEnum.SpawnWeapon_01) {
            WeaponInfo weaponInfo = world.GetWeaponInfo (_effectParam);
            textEffect = string.Format ("spawn weapon \"{0}\" ", weaponInfo.FullName);
            weaponInfoIndex = _effectParam;
          }
          else if (_effectType == PuzzleEffectTypeEnum.OpenDoor_02) {
            textEffect = string.Format ("open door at ({0},{1})", Door_PixelXY.X, Door_PixelXY.Y);
          }
          else if (_effectType == PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) {
            textEffect = string.Format ("open backdoor at ({0},{1})", Backdoor_PixelXY.X, Backdoor_PixelXY.Y);
          }
          else if (_effectType == PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04) {
            textEffect = string.Format ("open exit door at ({0},{1})", Door_PixelXY.X, Door_PixelXY.Y);
          }
          else if (_effectType == PuzzleEffectTypeEnum.TriggerEvent_05) {
            textEffect = string.Format ("trigger event #{0}", _effectParam - 1);
            Alfils02_Event event_ = world.GetEvent (_effectParam - 1);
            if (event_ != null) {
              List<Point> cellsXY = event_.GetMapCells (world);
              if (cellsXY.Count == 0) {
                textEffect += " (NOT on map)";
              }
              else {
                textEffect += " (on map)";
              }
              eventToDraw = event_;
            }
            //List<Point> cells = world.KO_GetCellsTriggeringEvent (_effectParam - 1);
            //if (cells.Count == 0) {
            //  textEffect += string.Format (" MISSING");
            //  eventToDraw = world.GetEvent (_effectParam - 1);
            //}
          }
          else if (_effectType == PuzzleEffectTypeEnum.DestroyType4_06) {
            textEffect = string.Format ("explosion at ({0},{1})", Explosion_PixelXY.X, Explosion_PixelXY.Y);
          }
          else if (_effectType == PuzzleEffectTypeEnum.OpenTrapdoor_07) {
            Alfils07_Trapdoor trapdoor = world.GetTrapdoorAt (_pixelXY);
            if (trapdoor != null)
              textEffect = string.Format ("open trap #{0}", trapdoor._index);
            else
              textEffect = string.Format ("open trap #?");
          }
          else if (_effectType == PuzzleEffectTypeEnum.CloseTrapdoor_08) {
            Alfils07_Trapdoor trapdoor = world.GetTrapdoorAt (_pixelXY);
            if (trapdoor != null)
              textEffect = string.Format ("close trapdoor #{0}", trapdoor._index);
            else
              textEffect = string.Format ("close trapdoor #?");
          }
          else if (_effectType == PuzzleEffectTypeEnum.CloseDoor_09) {
            textEffect = string.Format ("close door at ({0},{1})", Door_PixelXY.X, Door_PixelXY.Y);
          }
          else if (_effectType == PuzzleEffectTypeEnum.RemoveWeapon_10) {
            WeaponInfo weaponInfo = world.GetWeaponInfo (_effectParam);
            textEffect = string.Format ("remove weapon \"{0}\" ", weaponInfo.FullName);
            weaponInfoIndex = _effectParam;
          }
          else if (_effectType == PuzzleEffectTypeEnum.EnableRaster_11) {
            textEffect = string.Format ("enable raster");
          }
          else if (_effectType == PuzzleEffectTypeEnum.DisableRaster_12) {
            textEffect = string.Format ("disable raster");
          }
          else if (_effectType == PuzzleEffectTypeEnum.ResetGlobalTimer_14) {
            textEffect = string.Format ("reset elapsed time");
          }
          else {
            throw new Exception ();
          }

          textEffect = string.Format ("Effect: {0}", textEffect);
          BoxHelper.AddString (g, textEffect);

          if (objectInfoIndex != -1) {
            Bitmap bitmap = world.GetBitmapObject (objectInfoIndex);
            BoxHelper.AddBitmap (g, bitmap);
            BoxHelper.AddString (g, string.Format (" (object info #{0}) at ({1},{2})", objectInfoIndex, GetItemPixelXY(world).X, GetItemPixelXY(world).Y));
          }

          if (weaponInfoIndex != -1) {
            Bitmap bitmap = world.GetBitmapWeapon (weaponInfoIndex);
            BoxHelper.AddBitmap (g, bitmap);
            BoxHelper.AddString (g, string.Format (" (weapon info #{0}) at ({1},{2})", weaponInfoIndex, GetItemPixelXY(world).X, GetItemPixelXY(world).Y));
          }
          BoxHelper.NextLine ();
        }

        // message
        string textMessage = world.GetPuzzleMessage (_stringIndex);
        if (textMessage != null) {
          textMessage = string.Format ("Message: \"{0}\"", textMessage);
          BoxHelper.AddStringLine (g, textMessage);
        }
        BoxHelper.DrawBox (g);

        if (objectInfoIndex != -1) {
          ObjectInfo objectInfo = world.GetObjectInfo (objectInfoIndex);
          objectInfo.DrawBox (g, world);
        }
        if (weaponInfoIndex != -1) {
          WeaponInfo weaponInfo = world.GetWeaponInfo (weaponInfoIndex);
          weaponInfo.DrawBox (g, world);
        }

        if (eventToDraw != null) {
          //BoxHelper.DrawBox (eventToDraw.DrawOnBox, g, cellXY, pixelXY, world);
          eventToDraw.DrawBox (g, world);
        }
      }
    }

    //*************************************************************************
    // SPAWNED ITEM
    //*************************************************************************
    public void SpawnedItem_DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      if ((_effectType != PuzzleEffectTypeEnum.SpawnObject_00) &&
          (_effectType != PuzzleEffectTypeEnum.SpawnWeapon_01))
        throw new Exception ();

      ItemShared.DrawOnMap (g, cellXY, world, fromXY, this);
    }

    public void SpawnedItem_DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      if ((_effectType != PuzzleEffectTypeEnum.SpawnObject_00) &&
          (_effectType != PuzzleEffectTypeEnum.SpawnWeapon_01))
        throw new Exception ();

      ItemShared.DrawOnMap_Reverse (g, cellXY, world, toXY, state, this);
    }

    public void SpawnedItem_DrawBox (Graphics g, World world) {
      if ((_effectType != PuzzleEffectTypeEnum.SpawnObject_00) &&
          (_effectType != PuzzleEffectTypeEnum.SpawnWeapon_01))
        throw new Exception ();

      BoxHelper.Center ();
      BoxHelper.AddString (g, "SPAWNED ");
      ItemShared.DrawBox (g, world, this);

      //if (IsObject) {
      //  ObjectInfo objectInfo = world.GetObjectInfo (_effectParam);
      //  BoxHelper.Center ();
      //  BoxHelper.AddString (g, string.Format ("SPAWNED OBJECT"));
      //  BoxHelper.AddSeparation ();
      //  BoxHelper.AddStringLine (g, string.Format ("Object info: #{0}", ObjectInfoIndex));
      //  BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", _pixelXY.X, _pixelXY.Y));

      //  if (objectInfo.Type == ObjectInfoTypeEnum.Usable_00) {
      //    if (objectInfo.EffectType == ObjectEffectTypeEnum.RevealClues_18) {
      //      Alfils09_Hint hint = world.GetHintAt (_pixelXY.X);
      //      if (hint != null) {
      //        BoxHelper.AddStringLine (g, string.Format ("Hint: \"{0}\"", hint._string));
      //      }
      //    }
      //  }
      //  BoxHelper.DrawBox (g);

      //  objectInfo.DrawBox (g, world);
      //}
      //else if (IsWeapon) {
      //  WeaponInfo weaponInfo = world.GetWeaponInfo (_effectParam);
      //  BoxHelper.Center ();
      //  BoxHelper.AddString (g, string.Format ("SPAWNED WEAPON"));
      //  BoxHelper.AddSeparation ();
      //  BoxHelper.AddStringLine (g, string.Format ("Weapon info: #{0}", WeaponInfoIndex));
      //  BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", _pixelXY.X, _pixelXY.Y));
      //  BoxHelper.DrawBox (g);

      //  weaponInfo.DrawBox (g, world);
      //}
    }

    //*************************************************************************
    // DOOR
    //*************************************************************************
    public void Door_DrawOnMap (Graphics g, World world, Point fromXY) {
      if ((_effectType != PuzzleEffectTypeEnum.OpenDoor_02) &&
          (_effectType != PuzzleEffectTypeEnum.CloseDoor_09))
        throw new Exception ();
      Point doorPixelXY = Door_PixelXY;
      Size doorSize = Door_Size;
      g.DrawRectangle (Pens.Yellow, doorPixelXY.X, doorPixelXY.Y, doorSize.Width - 1, doorSize.Height - 1);
      Point centerXY = new Point (doorPixelXY.X + doorSize.Width / 2, doorPixelXY.Y + doorSize.Height / 2);
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.Yellow, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);
    }

    public void Door_DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      if ((_effectType != PuzzleEffectTypeEnum.OpenDoor_02) &&
          (_effectType != PuzzleEffectTypeEnum.CloseDoor_09))
        throw new Exception ();
      Point doorPixelXY = Door_PixelXY;
      Size doorSize = Door_Size;
      g.DrawRectangle (Pens.Yellow, doorPixelXY.X, doorPixelXY.Y, doorSize.Width - 1, doorSize.Height - 1);
      Point centerXY = new Point (doorPixelXY.X + doorSize.Width / 2, doorPixelXY.Y + doorSize.Height / 2);
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Yellow, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      // OK: door used in puzzle effect (open)
      //foreach (MapPuzzle puzzle in world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.OpenDoor_02, -1)) {
      //  if ((puzzle._pixelX != doorPixelXY.X) || (puzzle._pixelY != doorPixelXY.Y))
      //    continue;
      //  puzzle.DrawOnMap_Reverse (g, Point.Empty, world, centerXY);
      //}

      // OK: door used in puzzle effect (close)
      //foreach (MapPuzzle puzzle in world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.CloseDoor_09, -1)) {
      //  if ((puzzle._pixelX != _pixelX) || (puzzle._pixelY != _pixelY))
      //    continue;
      //  puzzle.DrawOnMap_Reverse (g, Point.Empty, world, centerXY);
      //}
      DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
    }

    public void Door_DrawBox (Graphics g, World world) {
      if ((_effectType != PuzzleEffectTypeEnum.OpenDoor_02) &&
          (_effectType != PuzzleEffectTypeEnum.CloseDoor_09))
        throw new Exception ();
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("DOOR (PUZZLE #{0})", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", Door_PixelXY.X, Door_PixelXY.Y));
      BoxHelper.DrawBox (g);
    }

    public Point Door_PixelXY {
      get {
        if ((_effectType != PuzzleEffectTypeEnum.OpenDoor_02) &&
            (_effectType != PuzzleEffectTypeEnum.CloseDoor_09))
          throw new Exception ();
        return (new Point (_pixelXY.X, _pixelXY.Y));
      }
    }

    public Size Door_Size {
      get {
        if ((_effectType != PuzzleEffectTypeEnum.OpenDoor_02) &&
            (_effectType != PuzzleEffectTypeEnum.CloseDoor_09))
          throw new Exception ();
        return (new Size (32, 48));
      }
    }

    public Rectangle Door_Rectangle {
      get {
        if ((_effectType != PuzzleEffectTypeEnum.OpenDoor_02) &&
            (_effectType != PuzzleEffectTypeEnum.CloseDoor_09))
          throw new Exception ();
        return (new Rectangle (Door_PixelXY, Door_Size));
      }
    }

    //*************************************************************************
    // BACKDOOR
    //*************************************************************************
    public void Backdoor_DrawOnMap (Graphics g, World world, Point fromXY) {
      if ((_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) &&
          (_effectType != PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04))
        throw new Exception ();
      Point backdoorPixelXY = Backdoor_PixelXY;
      Size backdoorSize = Backdoor_Size;
      g.DrawRectangle (Pens.Yellow, backdoorPixelXY.X, backdoorPixelXY.Y, backdoorSize.Width - 1, backdoorSize.Height - 1);
      Point centerXY = new Point (backdoorPixelXY.X + backdoorSize.Width / 2, backdoorPixelXY.Y + backdoorSize.Height / 2);
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.Yellow, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);

      if (_effectType == PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04) {
        g.DrawStringOutlinedCentered ("World", World._fontMap, Brushes.White, Brushes.Black, centerXY.X, centerXY.Y - backdoorSize.Height / 4);
        g.DrawStringOutlinedCentered ("Completed", World._fontMap, Brushes.White, Brushes.Black, centerXY.X, centerXY.Y + backdoorSize.Height / 4);
      }
      else {
        BackdoorTeleport_DrawOnMap (g, world, centerXY);
      }
    }

    public void Backdoor_DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      if ((_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) &&
          (_effectType != PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04))
        throw new Exception ();
      Point backdoorPixelXY = Backdoor_PixelXY;
      Size backdoorSize = Backdoor_Size;
      g.DrawRectangle (Pens.Yellow, backdoorPixelXY.X, backdoorPixelXY.Y, backdoorSize.Width - 1, backdoorSize.Height - 1);
      Point centerXY = new Point (backdoorPixelXY.X + backdoorSize.Width / 2, backdoorPixelXY.Y + backdoorSize.Height / 2);
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Yellow, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      // OK: backdoor used in puzzle effect (open teleport)
      //foreach (MapPuzzle puzzle in world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
      //  if ((puzzle.Backdoor_PixelXY.X != backdoorPixelXY.X) || (puzzle.Backdoor_PixelXY.Y != backdoorPixelXY.Y))
      //    continue;
      //  puzzle.DrawOnMap_Reverse (g, Point.Empty, world, centerXY);
      //}

      // OK: backdoor used in puzzle effect (open world completed)
      //foreach (MapPuzzle puzzle in world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04, -1)) {
      //  if ((puzzle.Backdoor_PixelXY.X != backdoorPixelXY.X) || (puzzle.Backdoor_PixelXY.Y != backdoorPixelXY.Y))
      //    continue;
      //  puzzle.DrawOnMap_Reverse (g, Point.Empty, world, centerXY);
      //}

      DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
    }

    public void Backdoor_DrawBox (Graphics g, World world) {
      if ((_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) &&
          (_effectType != PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04))
        throw new Exception ();
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("BACKDOOR (PUZZLE #{0})", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", Backdoor_PixelXY.X, Backdoor_PixelXY.Y));
      BoxHelper.AddStringLine (g, string.Format ("Teleport to (x,y): ({0},{1})", BackdoorTeleport_PixelXY.X, BackdoorTeleport_PixelXY.Y));
      BoxHelper.DrawBox (g);
    }

    public Point Backdoor_PixelXY {
      get {
        if ((_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) &&
            (_effectType != PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04))
          throw new Exception ();
        return (new Point (_pixelXY.X - 16, _pixelXY.Y));
      }
    }

    public Size Backdoor_Size {
      get {
        if ((_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) &&
            (_effectType != PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04))
          throw new Exception ();
        return (new Size (32, 48));
      }
    }

    public Rectangle Backdoor_Rectangle {
      get {
        if ((_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03) &&
            (_effectType != PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04))
          throw new Exception ();
        return (new Rectangle (Backdoor_PixelXY, Backdoor_Size));
      }
    }

    //*************************************************************************
    // BACKDOOR TELEPORT
    //*************************************************************************
    public void BackdoorTeleport_DrawOnMap (Graphics g, World world, Point fromXY) {
      if (_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03)
        throw new Exception ();
      Point backdoorTeleportPixelXY = BackdoorTeleport_PixelXY;
      Size backdoorTeleportSize = BackdoorTeleport_Size;
      g.DrawRectangle (Pens.Yellow, backdoorTeleportPixelXY.X, backdoorTeleportPixelXY.Y, backdoorTeleportSize.Width - 1, backdoorTeleportSize.Height - 1);
      Point centerXY = new Point (backdoorTeleportPixelXY.X + backdoorTeleportSize.Width / 2, backdoorTeleportPixelXY.Y + backdoorTeleportSize.Height / 2);
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.Yellow, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);
    }

    public void BackdoorTeleport_DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      if (_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03)
        throw new Exception ();
      Point backdoorTeleportPixelXY = BackdoorTeleport_PixelXY;
      Size backdoorTeleportSize = BackdoorTeleport_Size;
      g.DrawRectangle (Pens.Yellow, backdoorTeleportPixelXY.X, backdoorTeleportPixelXY.Y, backdoorTeleportSize.Width - 1, backdoorTeleportSize.Height - 1);
      Point centerXY = new Point (backdoorTeleportPixelXY.X + backdoorTeleportSize.Width / 2, backdoorTeleportPixelXY.Y + backdoorTeleportSize.Height / 2);
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Yellow, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      Backdoor_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
    }

    public void BackdoorTeleport_DrawBox (Graphics g, World world) {
      if (_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03)
        throw new Exception ();
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("BACKDOOR DESTINATION (PUZZLE #{0})", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", BackdoorTeleport_PixelXY.X, BackdoorTeleport_PixelXY.Y));
      BoxHelper.DrawBox (g);
    }

    public Point BackdoorTeleport_PixelXY {
      get {
        if (_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03)
          throw new Exception ();
        //int pixelX = (_effectParam >> 8) * 32 + 16;
        //int pixelY = (_effectParam & 0x00FF) * 16;
        return (World.DecodeTeleport (_effectParam));
      }
    }

    public Size BackdoorTeleport_Size {
      get {
        if (_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03)
          throw new Exception ();
        return (new Size (32, 48));
      }
    }

    public Rectangle BackdoorTeleport_Rectangle {
      get {
        if (_effectType != PuzzleEffectTypeEnum.OpenBackdoorTeleport_03)
          throw new Exception ();
        return (new Rectangle (BackdoorTeleport_PixelXY, BackdoorTeleport_Size));
      }
    }

    //*************************************************************************
    // EXPLOSION
    //*************************************************************************
    public void Explosion_DrawOnMap (Graphics g, World world, Point fromXY) {
      if (_effectType != PuzzleEffectTypeEnum.DestroyType4_06)
        throw new Exception ();
      Point explosionPixelXY = Explosion_PixelXY;
      Size explosionSize = Explosion_Size;
      g.DrawRectangle (Pens.Orange, explosionPixelXY.X, explosionPixelXY.Y, explosionSize.Width - 1, explosionSize.Height - 1);
      Point centerXY = new Point (explosionPixelXY.X + explosionSize.Width / 2, explosionPixelXY.Y + explosionSize.Height / 2);
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.Orange, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);
    }

    public void Explosion_DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      if (_effectType != PuzzleEffectTypeEnum.DestroyType4_06)
        throw new Exception ();
      Point explosionPixelXY = Explosion_PixelXY;
      Size explosionSize = Explosion_Size;
      g.DrawRectangle (Pens.Orange, explosionPixelXY.X, explosionPixelXY.Y, explosionSize.Width - 1, explosionSize.Height - 1);
      Point centerXY = new Point (explosionPixelXY.X + explosionSize.Width / 2, explosionPixelXY.Y + explosionSize.Height / 2);
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Orange, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      // OK: explosion used in puzzle effect
      //foreach (MapPuzzle puzzle in world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.DestroyType4_06, -1)) {
      //  if ((puzzle.Explosion_PixelXY.X != Explosion_PixelXY.X) || (puzzle.Explosion_PixelXY.Y != Explosion_PixelXY.Y))
      //    continue;
      //  puzzle.DrawOnMap_Reverse (g, Point.Empty, world, centerXY, false);
      //}
      DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
    }

    public void Explosion_DrawBox (Graphics g, World world) {
      if (_effectType != PuzzleEffectTypeEnum.DestroyType4_06)
        throw new Exception ();
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("EXPLOSION (PUZZLE #{0})", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", Explosion_PixelXY.X, Explosion_PixelXY.Y));
      BoxHelper.DrawBox (g);
    }

    public Point Explosion_PixelXY {
      get {
        if (_effectType != PuzzleEffectTypeEnum.DestroyType4_06)
          throw new Exception ();
        //int pixelX = (_effectParam >> 8) * 32 + 16;
        //int pixelY = (_effectParam & 0x00FF) * 16;
        return (new Point (_pixelXY.X - 12, _pixelXY.Y - 12));
      }
    }

    public Size Explosion_Size {
      get {
        if (_effectType != PuzzleEffectTypeEnum.DestroyType4_06)
          throw new Exception ();
        return (new Size (25, 25));
      }
    }

    public Rectangle Explosion_Rectangle {
      get {
        if (_effectType != PuzzleEffectTypeEnum.DestroyType4_06)
          throw new Exception ();
        return (new Rectangle (Explosion_PixelXY, Explosion_Size));
      }
    }
  }

  public enum ConditionTypeEnum {
    True_00,
    Carrying_01,
    NotCarrying_02,
    Holding_03,
    NotHolding_04,
    EventTriggered_05,
    EventNotTriggered_06,
    HealthSup_07,
    HealthInf_08,
    TimeSup_09,
    TimeInf_10,
    SwitchOn_11,
    SwitchOff_12,
    ScoreSup_13,
    ScoreInf_14,
    LivesSup_15,
    LivesInf_16
  }

  public enum PuzzleEffectTypeEnum {
    SpawnObject_00,
    SpawnWeapon_01,
    OpenDoor_02,
    OpenBackdoorTeleport_03,
    OpenBackdoorWorldCompleted_04,
    TriggerEvent_05,
    DestroyType4_06,
    OpenTrapdoor_07,
    CloseTrapdoor_08,
    CloseDoor_09,
    RemoveWeapon_10,
    EnableRaster_11,
    DisableRaster_12,
    NoEffect_13,
    ResetGlobalTimer_14
  }

  public class PuzzleConditionTypeParam {
    public ConditionTypeEnum _type;
    public int _param;

    public PuzzleConditionTypeParam (ConditionTypeEnum type, int param) {
      _type = type;
      _param = param;
    }
  }
}
