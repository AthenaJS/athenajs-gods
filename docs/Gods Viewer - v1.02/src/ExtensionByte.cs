using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GodsViewer {
  public static class ExtensionByte {
    public static byte [] CreateFromFile (string filename) {
      return (CreateFromFile (filename, 0));
    }

    public static byte [] CreateFromFile (string filename, int offset) {
      using (Stream stream = new FileStream (filename, FileMode.Open, FileAccess.Read)) {
        byte [] buffer = new byte [stream.Length];
        stream.Seek (offset, SeekOrigin.Begin);
        stream.Read (buffer, 0, (int) stream.Length - offset);
        stream.Close ();
        return (buffer);
      }
    }

    public static byte [] Extract (this byte [] buffer, int offset) {
      return (buffer.Extract (offset, buffer.Length - offset));
    }

    public static byte [] Extract (this byte [] buffer, int offset, int length) {
      byte [] datas = new byte [length];
      buffer.Copy (offset, datas, 0, length);
      //for (int i = 0; i < length; i++) {
      //  datas [i] = buffer [offset + i];
      //}
      return (datas);
    }

    public static void Copy (this byte [] buffer, int offset, byte [] bufferDst, int offsetDst, int length) {
      for (int i = 0; i < length; i++) {
        bufferDst [offsetDst + i] = buffer [offset + i];
      }
    }

    public static void WriteToFile (this byte [] buffer, string filename) {
      using (Stream stream = new FileStream (filename, FileMode.Create, FileAccess.Write)) {
        stream.Write (buffer, 0, buffer.Length);
        stream.Close ();
      }
    }

    // byte
    public static int ReadByte (this byte [] buffer, int offset) {
      int value = buffer [offset];
      return (value);
    }

    public static int ReadByteSigned (this byte [] buffer, int offset) {
      int value = (sbyte) buffer.ReadByte (offset);
      return (value);
    }

    public static void WriteByte (this byte [] buffer, int offset, int value) {
      buffer [offset] = (byte) ((value >> 0) & 0xFF);
    }

    // word
    public static int ReadWord (this byte [] buffer, int offset) {
      int value = (buffer [offset + 0] << 8) |
                  (buffer [offset + 1] << 0);
      return (value);
    }

    public static int ReadWordLE (this byte [] buffer, int offset) {
      int value = (buffer [offset + 1] << 8) |
                  (buffer [offset + 0] << 0);
      return (value);
    }

    public static int ReadWordSigned (this byte [] buffer, int offset) {
      int value = (short) buffer.ReadWord (offset);
      return (value);
    }

    public static void WriteWord (this byte [] buffer, int offset, int value) {
      buffer [offset + 0] = (byte) ((value >> 8) & 0xFF);
      buffer [offset + 1] = (byte) ((value >> 0) & 0xFF);
    }

    // long
    public static long ReadLong (this byte [] buffer, int offset) {
      long value = ((buffer [offset + 0] << 24) |
                    (buffer [offset + 1] << 16) |
                    (buffer [offset + 2] << 08) |
                    (buffer [offset + 3] << 00)) & 0xFFFFFFFF;
      return (value);
    }

    public static long ReadLongSigned (this byte [] buffer, int offset) {
      long value = (int) buffer.ReadLong (offset);
      return (value);
    }

    public static void WriteLong (this byte [] buffer, int offset, long value) {
      buffer [offset + 0] = (byte) ((value >> 24) & 0xFF);
      buffer [offset + 1] = (byte) ((value >> 16) & 0xFF);
      buffer [offset + 2] = (byte) ((value >> 08) & 0xFF);
      buffer [offset + 3] = (byte) ((value >> 00) & 0xFF);
    }

    public static string ReadString (this byte [] buffer, int offset) {
      StringBuilder sb = new StringBuilder ();
      for (; ; ) {
        if (offset >= buffer.Length) {
          break;
        }
        int value = buffer.ReadByte (offset++);
        if (value == 0) {
          break;
        }
        sb.Append ((char) value);
      }
      if (sb.Length == 0) {
        return (null);
      }
      else {
        return (sb.ToString ());
      }
    }

    public static string Dump (this byte [] buffer, int offset, int length, int baseAddress, int typeSize, int width, bool showAddress) {
      StringBuilder sb = new StringBuilder ();
      int firstAddress = offset;
      if (length == -1) {
        length = buffer.Length - firstAddress;
      }
      int i = 0;
      for (; ; ) {
        if ((offset - firstAddress) >= length) {
          break;
        }
        if ((i == 0) && (offset != firstAddress)) {
          sb.AppendLine ();
        }
        long value = -1;
        if ((offset + typeSize) <= buffer.Length) {
          if (typeSize == 1) {
            value = buffer.ReadByte (offset);
          }
          else if (typeSize == 2) {
            value = buffer.ReadWord (offset);
          }
          else if (typeSize == 4) {
            value = buffer.ReadLong (offset);
          }
        }

        if (showAddress) {
          if (i == 0) {
            sb.Append (string.Format ("{0:X8}  ", baseAddress + offset - firstAddress));
          }
        }

        if (typeSize == 1) {
          sb.Append (string.Format ("{0:X2} ", value));
        }
        else if (typeSize == 2) {
          sb.Append (string.Format ("{0:X4} ", value));
        }
        else if (typeSize == 4) {
          sb.Append (string.Format ("{0:X8} ", value));
        }

        offset += typeSize;
        if (width != -1) {
          i = (i + 1) % width;
        }
        else {
          i = 1;
        }
      }

      return (sb.ToString ());
    }
  }
}