using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GodsViewer {
  public static class UnpackerC {
    public static byte [] _bufferTemp = new byte [65536];

    public static byte [] Unpack (byte [] bufferSrc, int offsetSrc) {
      int length = UnpackBlock (bufferSrc, ref offsetSrc);
      if (length == -1) {
        throw new Exception ();
      }
      byte [] bufferDst = _bufferTemp.Extract (0, length);
      int size = bufferSrc.ReadWordLE (offsetSrc);
      if (size != 0) {
        throw new Exception ();
      }
      return (bufferDst);
    }

    public static int UnpackBlock (byte [] bufferSrc, ref int offsetSrc) {
      int size = bufferSrc.ReadWordLE (offsetSrc);
      offsetSrc += 2;
      if (size == 0) {
        return (-1);
      }
      byte [] bufferDst = _bufferTemp;
      int offsetDst = 0;
      int offsetEnd = offsetSrc + size;
      while (offsetSrc != offsetEnd) {
        int length = bufferSrc.ReadByte (offsetSrc++);
        if (length < 0x80) { // 00-7F
          for (int i = 0; i < length; i++) {
            bufferDst [offsetDst++] = bufferSrc [offsetSrc++];
          }
        }
        else { // 80-FF
          if (length == 0xFF) {
            length = bufferSrc.ReadWordLE (offsetSrc);
            offsetSrc += 2;
          }
          else {
            length &= 0x7F;
          }
          int offsetFiller = bufferSrc.ReadWordLE (offsetSrc);
          offsetSrc += 2;
          for (int i = 0; i < length; i++) {
            bufferDst [offsetDst++] = bufferDst [offsetFiller++];
          }
        }
      }
      return (offsetDst);
    }
  }
}
