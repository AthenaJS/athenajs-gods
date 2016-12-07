using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class MapItem : IObjectShared {
    public int _index;

    public Point _pixelXY;
    public int _objectOrWeaponInfoIndex;

    public MapItem (byte [] buffer, ref int offset, int index) {
      _index = index;

      int pixelX = buffer.ReadWord (offset);
      offset += 2;
      int pixelY = buffer.ReadWord (offset);
      offset += 2;
      _pixelXY = new Point (pixelX, pixelY);
      // < 192: object info
      // >=192: weapon info
      _objectOrWeaponInfoIndex = buffer.ReadWord (offset);
      offset += 2;
    }

    public bool IsObject {
      get {
        if (_objectOrWeaponInfoIndex < 192)
          return (true);
        return (false);
      }
    }

    public bool IsWeapon {
      get {
        if (_objectOrWeaponInfoIndex >= 192)
          return (true);
        return (false);
      }
    }

    public int ObjectInfoIndex {
      get {
        if (!IsObject)
          throw new Exception ();
        return (_objectOrWeaponInfoIndex);
      }
    }

    public int WeaponInfoIndex {
      get {
        if (!IsWeapon)
          throw new Exception ();
        return (_objectOrWeaponInfoIndex - 192);
      }
    }

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
        return (string.Format ("(MAP ITEM #{0})", _index));
      }
    }

    //*****************************************************************************************************************
    public void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      ItemShared.DrawOnMap (g, cellXY, world, fromXY, this);
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
    }

    public void Item_DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      ItemShared.DrawOnMap_Reverse (g, cellXY, world, toXY, state, this);
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, "MAP ");
      ItemShared.DrawBox (g, world, this);
    }
  }
}
