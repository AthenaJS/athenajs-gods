using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class EnemiesInfo {
    public List<EnemyInfo> _enemiesInfo = new List<EnemyInfo> ();

    public EnemiesInfo (byte [] buffer, int offset, string type) {
      for (int i = 0; ; i++) {
        if (buffer.ReadLong (offset) == 0xFFFFFFFF) {
          break;
        }
        EnemyInfo enemyInfo = new EnemyInfo (buffer, ref offset, i, type);
        _enemiesInfo.Add (enemyInfo);
      }
    }
  }
}
