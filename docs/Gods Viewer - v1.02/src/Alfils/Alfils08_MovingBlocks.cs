using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils08_MovingBlocks : IAlfil {
    public List<Alfils08_MovingBlock> _movingBlocks = new List<Alfils08_MovingBlock> ();
    public int _offset;
    public int _length;

    public Alfils08_MovingBlocks (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x189C)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 25; i++) {
        Alfils08_MovingBlock movingBlock = new Alfils08_MovingBlock (buffer_alfils_0YX, ref offset, i);
        if ((movingBlock._mapSpriteIndex_min1 == 0x00) || (movingBlock._mapSpriteIndex_min1 == 0xFF)) {
          _movingBlocks.Add (null);
        }
        else {
          _movingBlocks.Add (movingBlock);
          //movingBlock.Dump ();
          //Console.WriteLine ();
        }
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils08_MovingBlocks");
      }
    }

    public int Offset {
      get {
        return (_offset);
      }
    }

    public int Length {
      get {
        return (_length);
      }
    }
  }
}

