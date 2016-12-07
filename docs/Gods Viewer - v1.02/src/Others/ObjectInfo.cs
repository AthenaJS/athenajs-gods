using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class ObjectInfo {
    public int _index;

    public int _spriteIndex;
    public int _unk02_always0;
    public int _type;
    public int _value;
    public string _name;
    public string _description;

    public int _effectIndex;

    public ObjectInfo (byte [] buffer, ref int offset, ref int effectIndex, int index) {
      _index = index;

      _spriteIndex = buffer.ReadWord (offset);
      offset += 2;
      _unk02_always0 = buffer.ReadWord (offset);
      offset += 2;
      // 0: usable (inventory)
      // 1: treasure (not in inventory)
      // 2: not usable (inventory)
      // 3: switch
      // 4: destructable (spikes, beehive, serpent vase, bat)
      // 5: decoration
      _type = buffer.ReadWord (offset);
      offset += 2;
      _value = buffer.ReadWord (offset);
      offset += 2;
      _name = buffer.ReadString (offset + 0);
      _description = buffer.ReadString (offset + 16);
      offset += 40;

      if (Type == ObjectInfoTypeEnum.Usable_00) {
        _effectIndex = effectIndex;
        effectIndex++;
      }
      else {
        _effectIndex = -1;
      }

      //if (Type == ObjectInfoTypeEnum.Usable_00) {
      //  if (_value == 0) {
      //    int i = 0;
      //  }
      //}
      //else if (Type == ObjectInfoTypeEnum.Treasure_01) {
      //  if (_value == 0) {
      //    int i = 0;
      //  }
      //}
      //else {
      //  if (_value != 0)
      //    throw new Exception ();
      //}
    }

    public ObjectInfoTypeEnum Type {
      get {
        return ((ObjectInfoTypeEnum) _type);
      }
    }

    public ObjectEffectTypeEnum EffectType {
      get {
        if (Type != ObjectInfoTypeEnum.Usable_00)
          throw new Exception ();
        if (_effectIndex == -1)
          throw new Exception ();
        return ((ObjectEffectTypeEnum) _effectIndex);
      }
    }

    public static string [] _typesName = new string [] { "USABLE ITEM", "TREASURE", "PICKABLE ITEM", "SWITCH", "DESTRUCTABLE", "DECORATION" };
    public string TypeName {
      get {
        return (_typesName [(int) _type]);
      }
    }

    public string String {
      get {
        return (string.Format ("SpriteIndex: {0,3} Type: {2,1} Price: {3,5} '{4,-16}' ('{5}')", _spriteIndex, _unk02_always0, _type, _value, _name, _description));
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

    public static bool IsKey (int objectInfoIndex) {
      if (IsChestKey (objectInfoIndex) || IsSpecialKey (objectInfoIndex))
        return (true);
      return (false);
    }

    public static bool IsChestKey (int objectInfoIndex) {
      if ((objectInfoIndex >= 18) && (objectInfoIndex <= 23))
        return (true);
      return (false);
    }

    public static bool IsSpecialKey (int objectInfoIndex) {
      if ((objectInfoIndex >= 127) && (objectInfoIndex <= 129))
        return (true);
      return (false);
    }

    public static bool IsChest (int objectInfoIndex) {
      if ((objectInfoIndex >= 12) && (objectInfoIndex <= 17))
        return (true);
      return (false);
    }

    public static bool IsKeyOfChest (int objectInfoIndexChestKey, int objectInfoIndexChest) {
      if (!IsChestKey (objectInfoIndexChestKey))
        throw new Exception ();
      if (!IsChest (objectInfoIndexChest))
        throw new Exception ();
      if ((objectInfoIndexChestKey - 21) == ((objectInfoIndexChest - 12) / 2))
        return (true);
      return (false);
    }

    public Bitmap GetBitmap (World world) {
      return (world.GetBitmapObject (_index));
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("OBJECT INFO #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddString (g, string.Format ("\"{0}\" ", FullName));
      BoxHelper.AddBitmap (g, GetBitmap (world));
      BoxHelper.NextLine ();
      BoxHelper.AddStringLine (g, string.Format ("Type: {0}", Enum.GetName (typeof (ObjectInfoTypeEnum), Type)));
      if (Type == ObjectInfoTypeEnum.Usable_00) {
        BoxHelper.AddStringLine (g, string.Format ("Effect: {0}", Enum.GetName (typeof (ObjectEffectTypeEnum), EffectType)));
        if (_value != 0) {
          BoxHelper.AddStringLine (g, string.Format ("Price: {0} gold", _value));
        }
        else {
          BoxHelper.AddStringLine (g, string.Format ("Price: cannot be purchased"));
        }
        if (EffectType == ObjectEffectTypeEnum.RestoreHalfHealth_00) {
          BoxHelper.AddStringLine (g, string.Format ("Restore 12 hp"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Missing health = Min (12, 24 - player health)"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Player health: + missing health"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Familiar health: + 32 * (12 - missing health)"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.IncreasePowerBy1_01) {
          BoxHelper.AddStringLine (g, string.Format ("Weapon upgrade: +1 upgrade for all weapons"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.IncreasePowerBy2_02) {
          BoxHelper.AddStringLine (g, string.Format ("Weapon upgrade: +2 upgrade for all weapons"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.RestoreFullHealth_03) {
          BoxHelper.AddStringLine (g, string.Format ("Restore 24 hp"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Missing health = Min (24, 24 - player health)"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Player health: + missing health"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Familiar health: + 32 * (24 - missing health)"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.ShieldReduceDamage_05) {
          BoxHelper.AddStringLine (g, string.Format ("All incoming damage are halved for the current world"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.ShieldInvulnerability_06) {
          BoxHelper.AddStringLine (g, string.Format ("All incoming damage are ignored for {0:0.0} seconds ({1} frames)", World.FramesToSeconds_3VBL (255), 255), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.FamiliarMagicWings_08) {
          BoxHelper.AddStringLine (g, string.Format ("Familiar max velocity: +1"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.FamiliarPowerClaws_09) {
          BoxHelper.AddStringLine (g, string.Format ("Familiar claws damage: +2"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.FreezeAliens_12) {
          BoxHelper.AddStringLine (g, string.Format ("Duration: {0:0.0} seconds ({1} frames)", World.FramesToSeconds_3VBL (170), 170), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.GiantJump_13) {
          BoxHelper.AddStringLine (g, string.Format ("Duration: {0:0.0} seconds ({1} VBL)", World.FramesToSeconds (1000), 1000), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.FoodRestoreEnergy_19) {
          BoxHelper.AddStringLine (g, string.Format ("Restore 2 hp"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Missing health = Min (2, 24 - player health)"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Player health: + missing health"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Familiar health: + 32 * (2 - missing health)"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.FoodRestoreEnergy_20) {
          BoxHelper.AddStringLine (g, string.Format ("Restore 4 hp"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Missing health = Min (4, 24 - player health)"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Player health: + missing health"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Familiar health: + 32 * (4 - missing health)"), World._brushComputed);
        }
        else if (EffectType == ObjectEffectTypeEnum.FoodRestoreEnergy_21) {
          BoxHelper.AddStringLine (g, string.Format ("Restore 3 hp"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Missing health = Min (3, 24 - player health)"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Player health: + missing health"), World._brushComputed);
          BoxHelper.AddStringLine (g, string.Format ("Familiar health: + 32 * (3 - missing health)"), World._brushComputed);
        }
      }
      else if (Type == ObjectInfoTypeEnum.Treasure_01) {
        BoxHelper.AddStringLine (g, string.Format ("Gold: +{0}", _value));
        BoxHelper.AddStringLine (g, string.Format ("Score: +{0} [=gold/8]", _value >> 3), World._brushComputed);
      }
      BoxHelper.DrawBox (g);
    }
  }

  public enum ObjectInfoTypeEnum {
    Usable_00,
    Treasure_01,
    Pickable_02,
    Switch_03,
    Destructable_04,
    Decoration_05,
  }

  public enum ObjectEffectTypeEnum {
    RestoreHalfHealth_00,
    IncreasePowerBy1_01,
    IncreasePowerBy2_02,
    RestoreFullHealth_03,
    Starburst_04,
    ShieldReduceDamage_05,
    ShieldInvulnerability_06,
    Familiar_07,
    FamiliarMagicWings_08,
    FamiliarPowerClaws_09,
    ExtraLife_10,
    SlowMonsters_CheckTrigger_Scroll_DrawPlayer_11,
    FreezeAliens_12,
    GiantJump_13,
    WeaponArc_Standard_14,
    WeaponArc_Intense_15,
    WeaponArc_Wide_16,
    TeleportStone_17,
    RevealClues_18,
    FoodRestoreEnergy_19,
    FoodRestoreEnergy_20,
    FoodRestoreEnergy_21,
    SummonShopkeeper_22
  }
}
