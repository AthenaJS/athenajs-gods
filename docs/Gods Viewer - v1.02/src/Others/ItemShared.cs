using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {

  public interface IObjectShared {
    //Bitmap GetBitmapObject (World world);
    //Bitmap GetBitmapWeapon (World world);
    Point GetItemPixelXY (World world);
    bool IsObject {
      get;
    }
    bool IsWeapon {
      get;
    }
    int ObjectInfoIndex {
      get;
    }
    int WeaponInfoIndex {
      get;
    }
    void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state);
    string TitleComplement {
      get;
    }
  }

  public static class ItemShared {

    public static void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY, IObjectShared itemShared) {
      Bitmap bitmap = null;
      if (itemShared.IsObject)
        bitmap = world.GetBitmapObject (itemShared.ObjectInfoIndex);
      else if (itemShared.IsWeapon)
        bitmap = world.GetBitmapWeapon (itemShared.WeaponInfoIndex);
      else
        throw new Exception ();
      Point pixelXY = itemShared.GetItemPixelXY (world);
      if (world._drawType == DrawTypeEnum.DrawBitmaps) {
        g.DrawImage (bitmap, pixelXY.X, pixelXY.Y);
        g.DrawRectangle (Pens.Lime, pixelXY.X, pixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
      }
      Point centerXY = new Point (pixelXY.X + bitmap.Width / 2, pixelXY.Y + bitmap.Height / 2);
      if (world._drawType == DrawTypeEnum.DrawArrows)
        if (!fromXY.IsEmpty)
          g.DrawArrowOutlinedSolid (Pens.Lime, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);

      if (itemShared.IsObject) {
        ObjectInfo objectInfo = world.GetObjectInfo (itemShared.ObjectInfoIndex);
        if ((objectInfo.Type == ObjectInfoTypeEnum.Usable_00) ||
            (objectInfo.Type == ObjectInfoTypeEnum.Pickable_02)) {
          // OK: object used in puzzle condition (carried)
          foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.Carrying_01, itemShared.ObjectInfoIndex)) {
            event_.DrawOnMap (g, cellXY, world, centerXY, false);
          }
          foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.NotCarrying_02, itemShared.ObjectInfoIndex)) {
            event_.DrawOnMap (g, cellXY, world, centerXY, true);
          }
        }

        // OK: key (chest)
        if (ObjectInfo.IsChestKey (itemShared.ObjectInfoIndex)) {
          // OK: key (chest) used in map chest
          foreach (MapItem mapItem in GetChests (itemShared.ObjectInfoIndex, world._map._mapItems)) {
            mapItem.DrawOnMap (g, cellXY, world, centerXY);
          }
          // OK: key (chest) used in spawned chest
          foreach (MapPuzzle puzzle in GetChests (itemShared.ObjectInfoIndex, world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.SpawnObject_00, -1))) {
            puzzle.SpawnedItem_DrawOnMap (g, cellXY, world, centerXY);
          }
          // OK: key (chest) used in rewarded chest (walking)
          foreach (Alfils03_WalkingWave wave in GetChests (itemShared.ObjectInfoIndex, world._walkingWaves._walkingWaves)) {
            wave.RewardItem_DrawOnMap (g, cellXY, world, centerXY);
          }
          // OK: key (chest) used in rewarded chest (flying)
          foreach (Alfils01_FlyingWave wave in GetChests (itemShared.ObjectInfoIndex, world._flyingWaves._flyingWaves)) {
            wave.RewardItem_DrawOnMap (g, cellXY, world, centerXY);
          }
          // OK: key (chest) used in rewarded chest (intel walking)
          foreach (Alfils04b_IntelWalkingWave wave in GetChests (itemShared.ObjectInfoIndex, world._intelWalkingWaves._intelWalkingWaves)) {
            wave.RewardItem_DrawOnMap (g, cellXY, world, centerXY);
          }
          // OK: key (chest) used in rewarded chest (intel flying)
          foreach (Alfils04a_IntelFlyingWave wave in GetChests (itemShared.ObjectInfoIndex, world._intelFlyingWaves._intelFlyingWaves)) {
            wave.RewardItem_DrawOnMap (g, cellXY, world, centerXY);
          }
        }

        // OK: object effect
        if (objectInfo.Type == ObjectInfoTypeEnum.Usable_00) {
          if (objectInfo.EffectType == ObjectEffectTypeEnum.TeleportStone_17) {
            Alfils06_Teleport teleport = world.GetTeleportAt (pixelXY.X);
            if (teleport == null) {
              if (world._drawType == DrawTypeEnum.DrawBitmaps)
                g.DrawRectangle (Pens.Red, pixelXY.X, pixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
            }
            else
              teleport.DrawOnMap (g, cellXY, world, centerXY);
          }
        }

        // OK: switch used in puzzle condition (on/off)
        if (objectInfo.Type == ObjectInfoTypeEnum.Switch_03) {
          Alfils05_Switch switch_ = world.GetSwitchAt (pixelXY);
          if (switch_ == null)
            throw new Exception ();
          foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.SwitchOn_11, switch_._index)) {
            event_.DrawOnMap (g, cellXY, world, centerXY, false);
          }
          foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.SwitchOff_12, switch_._index)) {
            event_.DrawOnMap (g, cellXY, world, centerXY, true);
          }
        }
      }
      else if (itemShared.IsWeapon) {
        // OK: weapon used in puzzle condition (held)
        foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.Holding_03, itemShared.WeaponInfoIndex)) {
          event_.DrawOnMap (g, cellXY, world, centerXY, false);
        }
        foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.NotHolding_04, itemShared.WeaponInfoIndex)) {
          event_.DrawOnMap (g, cellXY, world, centerXY, true);
        }
      }
    }

    public static void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state, IObjectShared itemShared) {
      Bitmap bitmap = null;
      if (itemShared.IsObject)
        bitmap = world.GetBitmapObject (itemShared.ObjectInfoIndex);
      else if (itemShared.IsWeapon)
        bitmap = world.GetBitmapWeapon (itemShared.WeaponInfoIndex);
      else
        throw new Exception ();
      Point pixelXY = itemShared.GetItemPixelXY (world);
      if (world._drawType == DrawTypeEnum.DrawBitmaps) {
        g.DrawImage (bitmap, pixelXY.X, pixelXY.Y);
        g.DrawRectangle (Pens.Lime, pixelXY.X, pixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
      }
      Point centerXY = new Point (pixelXY.X + bitmap.Width / 2, pixelXY.Y + bitmap.Height / 2);
      if (world._drawType == DrawTypeEnum.DrawArrows)
        if (!toXY.IsEmpty)
          g.DrawArrowOutlinedState (Pens.Lime, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      itemShared.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);

      if (itemShared.IsObject) {
        ObjectInfo objectInfo = world.GetObjectInfo (itemShared.ObjectInfoIndex);
        // OK: chest
        if (ObjectInfo.IsChest (itemShared.ObjectInfoIndex)) {
          // OK: chest opened with map key (chest)
          foreach (MapItem mapItem in GetChestKeys (itemShared.ObjectInfoIndex, world._map._mapItems)) {
            mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
          }
          // OK: chest opened with spawned key (chest)
          foreach (MapPuzzle puzzle in GetChestKeys (itemShared.ObjectInfoIndex, world.GetPuzzlesWithEffect (PuzzleEffectTypeEnum.SpawnObject_00, -1))) {
            puzzle.SpawnedItem_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
          }
          // OK: chest opened with rewarded key (chest) (walking)
          foreach (Alfils03_WalkingWave wave in GetChestKeys (itemShared.ObjectInfoIndex, world._walkingWaves._walkingWaves)) {
            wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
          }
          // OK: chest opened with rewarded key (chest) (flying)
          foreach (Alfils01_FlyingWave wave in GetChestKeys (itemShared.ObjectInfoIndex, world._flyingWaves._flyingWaves)) {
            wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
          }
          // OK: chest opened with rewarded key (chest) (intel walking)
          foreach (Alfils04b_IntelWalkingWave wave in GetChestKeys (itemShared.ObjectInfoIndex, world._intelWalkingWaves._intelWalkingWaves)) {
            wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
          }
          // OK: chest opened with rewarded key (chest) (intel flying)
          foreach (Alfils04a_IntelFlyingWave wave in GetChestKeys (itemShared.ObjectInfoIndex, world._intelFlyingWaves._intelFlyingWaves)) {
            wave.RewardItem_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
          }
        }

        // OK: mark the teleport stone as RED
        if (objectInfo.Type == ObjectInfoTypeEnum.Usable_00) {
          if (objectInfo.EffectType == ObjectEffectTypeEnum.TeleportStone_17) {
            Alfils06_Teleport teleport = world.GetTeleportAt (pixelXY.X);
            if (teleport == null)
              if (world._drawType == DrawTypeEnum.DrawBitmaps)
                g.DrawRectangle (Pens.Red, pixelXY.X, pixelXY.Y, bitmap.Width - 1, bitmap.Height - 1);
          }
        }
      }
      else if (itemShared.IsWeapon) {
      }
    }

    public static void DrawBox (Graphics g, World world, IObjectShared objectShared) {
      Point pixelXY = objectShared.GetItemPixelXY (world);
      if (objectShared.IsObject) {
        ObjectInfo objectInfo = world.GetObjectInfo (objectShared.ObjectInfoIndex);
        BoxHelper.Center ();
        BoxHelper.AddString (g, string.Format ("OBJECT"));
        if (objectShared.TitleComplement != null)
          BoxHelper.AddString (g, string.Format (" {0}", objectShared.TitleComplement));
        BoxHelper.AddSeparation ();
        BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", pixelXY.X, pixelXY.Y));
        BoxHelper.AddString (g, string.Format ("\"{0}\" ", objectInfo.FullName));
        BoxHelper.AddBitmap (g, objectInfo.GetBitmap (world));
        BoxHelper.AddStringLine (g, string.Format (" (object info #{0})", objectShared.ObjectInfoIndex));
        //BoxHelper.AddStringLine (g, string.Format ("Object info: #{0}", objectShared.ObjectInfoIndex));

        if (objectInfo.Type == ObjectInfoTypeEnum.Usable_00) {
          if (objectInfo.EffectType == ObjectEffectTypeEnum.RevealClues_18) {
            Alfils09_Hint hint = world.GetHintAt (pixelXY.X);
            if (hint != null) {
              BoxHelper.AddSeparation ();
              BoxHelper.AddStringLine (g, string.Format ("Hint #{0}", hint._index));
              BoxHelper.DrawBox (g);

              hint.DrawBox (g, world);
            }
          }
          else if (objectInfo.EffectType == ObjectEffectTypeEnum.TeleportStone_17) {
            Alfils06_Teleport teleport = world.GetTeleportAt (pixelXY.X);
            if (teleport != null) {
              BoxHelper.AddSeparation ();
              BoxHelper.AddStringLine (g, string.Format ("Teleport #{0}", teleport._index));
              BoxHelper.DrawBox (g);

              teleport.DrawBox (g, world);
              //BoxHelper.AddStringLine (g, string.Format ("Teleport to (x,y): ({0},{1})", teleport.PixelXY.X, teleport.PixelXY.Y));
            }
            else {
              BoxHelper.AddSeparation ();
              BoxHelper.AddStringLine (g, string.Format ("Teleport to next hardcoded coordinate"));
              BoxHelper.DrawBox (g);
            }
          }
          else
            BoxHelper.DrawBox (g);
        }
        else if (objectInfo.Type == ObjectInfoTypeEnum.Switch_03) {
          Alfils05_Switch switch_ = world.GetSwitchAt (pixelXY);
          if (switch_ != null) {
            BoxHelper.AddSeparation ();
            BoxHelper.AddStringLine (g, string.Format ("Switch #{0}", switch_._index));
            BoxHelper.DrawBox (g);
          }
        }
        else
          BoxHelper.DrawBox (g);

        objectInfo.DrawBox (g, world);
      }
      else if (objectShared.IsWeapon) {
        WeaponInfo weaponInfo = world.GetWeaponInfo (objectShared.WeaponInfoIndex);
        BoxHelper.Center ();
        BoxHelper.AddString (g, string.Format ("WEAPON"));
        if (objectShared.TitleComplement != null)
          BoxHelper.AddString (g, string.Format (" {0}", objectShared.TitleComplement));
        BoxHelper.AddSeparation ();
        BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", pixelXY.X, pixelXY.Y));
        BoxHelper.AddString (g, string.Format ("\"{0}\" ", weaponInfo.FullName));
        BoxHelper.AddBitmap (g, weaponInfo.GetBitmap (world));
        BoxHelper.AddStringLine (g, string.Format (" (weapon info #{0})", objectShared.WeaponInfoIndex));
        //BoxHelper.AddStringLine (g, string.Format ("Weapon info: #{0}", objectShared.WeaponInfoIndex));
        BoxHelper.DrawBox (g);

        weaponInfo.DrawBox (g, world);
      }
    }

    public static List<T> GetChests<T> (int keyObjectInfoIndex, List<T> items) where T : IObjectShared {
      List<T> chests = new List<T> ();
      foreach (T item in items) {
        if (item == null)
          continue;
        if (!item.IsObject)
          continue;
        if (!ObjectInfo.IsChest (item.ObjectInfoIndex))
          continue;
        if (keyObjectInfoIndex != -1) {
          if (!ObjectInfo.IsKeyOfChest (keyObjectInfoIndex, item.ObjectInfoIndex))
            continue;
        }
        chests.Add (item);
      }
      return (chests);
    }

    public static List<T> GetChestKeys<T> (int chestObjectInfoIndex, List<T> items) where T : IObjectShared {
      List<T> objects = new List<T> ();
      foreach (T item in items) {
        if (item == null)
          continue;
        if (!item.IsObject)
          continue;
        if (!ObjectInfo.IsChestKey (item.ObjectInfoIndex))
          continue;
        if (chestObjectInfoIndex != -1) {
          if (!ObjectInfo.IsKeyOfChest (item.ObjectInfoIndex, chestObjectInfoIndex))
            continue;
        }
        objects.Add (item);
      }
      return (objects);
    }

    public static List<T> GetItems<T> (int objectInfoIndex, List<T> items) where T : IObjectShared {
      List<T> objects = new List<T> ();
      foreach (T item in items) {
        if (item == null)
          continue;
        if (!item.IsObject)
          continue;
        if (objectInfoIndex != -1) {
          if (item.ObjectInfoIndex != objectInfoIndex)
            continue;
        }
        objects.Add (item);
      }
      return (objects);
    }

    public static List<T> GetWeapons<T> (int weaponInfoIndex, List<T> items) where T : IObjectShared {
      List<T> weapons = new List<T> ();
      foreach (T item in items) {
        if (item == null)
          continue;
        if (!item.IsObject)
          continue;
        if (weaponInfoIndex != -1) {
          if (item.ObjectInfoIndex != weaponInfoIndex)
            continue;
        }
        weapons.Add (item);
      }
      return (weapons);
    }
  }
}
