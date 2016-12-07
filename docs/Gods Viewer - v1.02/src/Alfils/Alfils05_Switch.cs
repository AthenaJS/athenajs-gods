using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils05_Switch {
    public int _index;

    public int _pixelX;
    public int _pixelY;
    public int _objectInfoIndex;

    public Alfils05_Switch (byte [] buffer, ref int offset, int index) {
      _index = index;

      _pixelX = buffer.ReadWord (offset);
      offset += 2;
      _pixelY = buffer.ReadWord (offset);
      offset += 2;
      _objectInfoIndex = buffer.ReadWord (offset);
      offset += 2;
    }
  }
}
