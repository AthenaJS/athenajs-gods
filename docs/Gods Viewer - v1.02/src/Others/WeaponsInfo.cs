using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class WeaponsInfo {
    public List<WeaponInfo> _weaponsInfo = new List<WeaponInfo> ();

    public WeaponsInfo (string filename) {
      byte [] buffer = ExtensionByte.CreateFromFile (filename);
      int offset = 0;
      for (int i = 0; i < 11; i++) {
        WeaponInfo weaponInfo = new WeaponInfo (buffer, ref offset, i);
        _weaponsInfo.Add (weaponInfo);
        //Console.WriteLine (weaponInfo.String);
      }
    }
  }
}
