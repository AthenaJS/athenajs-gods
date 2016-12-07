using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace GodsViewer {
  public class Palette {
    private int _size;
    private int [] _colorsR;
    private int [] _colorsG;
    private int [] _colorsB;
    private uint [] _rawARGB;
    private int [] _rawRGB;

    private Color [] _colors;
    private Brush [] _brushes;

    private Bitmap _bitmap;
    //                                      0    36    73   109   146   182   219   255
    //                                      0    32    64    96   128   160   192   224
    //                                      0     1     2     3     4     5     6     7
    private static byte [] _lookUpST = { 0x00, 0x24, 0x49, 0x6D, 0x92, 0xB6, 0xDB, 0xFF };
    private static byte [] _lookUpAmigaToST = { 0x00, 0x00, 0x24, 0x00, 0x49, 0x00, 0x6D, 0x00, 0x92, 0x00, 0xB6, 0x00, 0xDB, 0x00, 0xFF, 0x00 };
    private static byte [] _lookUpSTe = {
  0x00, /* 0000 */
  0x22, /* 0001 */
  0x43, /* 0010 */
  0x66, /* 0011 */
  0x88, /* 0100 */
  0xAA, /* 0101 */
  0xCC, /* 0110 */
  0xEE, /* 0111 */
  0x11, /* 1000 */
  0x33, /* 1001 */
  0x44, /* 1010 */
  0x55, /* 1011 */
  0x99, /* 1100 */
  0xBB, /* 1101 */
  0xDD, /* 1110 */
  0xFF, /* 1111 */
};

    public enum PaletteLookup {
      ST,
      STe,
      AmigaToST,
      None
    }

    public Palette (byte [] bytes, int address, int size, PaletteLookup paletteLookup) {
      byte [] lookup = null;
      if (paletteLookup == PaletteLookup.ST) {
        lookup = _lookUpST;
      }
      else if (paletteLookup == PaletteLookup.STe) {
        lookup = _lookUpSTe;
      }
      else if (paletteLookup == PaletteLookup.AmigaToST) {
        lookup = _lookUpAmigaToST;
      }
      //int palettesBase;
      //int paletteAddress;

      _size = size;
      _colorsR = new int [_size];
      _colorsG = new int [_size];
      _colorsB = new int [_size];
      _rawARGB = new uint [_size];
      _rawRGB = new int [_size];
      _colors = new Color [_size];
      _brushes = new SolidBrush [_size];

      for (int i = 0; i < _size; i++) {
        int r, g, b;
        //if (version == VersionEnum.AmigaCD32) {
        //  r = dump.ReadByte (paletteAddress++);
        //  v = dump.ReadByte (paletteAddress++);
        //  b = dump.ReadByte (paletteAddress++);
        //}

        int rgb = bytes.ReadWord (address + 2 * i);
        //if ((rgb & 0xF000) != 0) {
        //  rgb -= 0xF888;
        //}
        //_rawRGB [i] = rgb;
        if (paletteLookup == PaletteLookup.ST) {
          //if ((rgb >> (8 + 3)) != 0)
          //  throw new Exception ();
          //if ((rgb >> (4 + 3)) != 0)
          //  throw new Exception ();
          //if ((rgb >> (0 + 3)) != 0)
          //  throw new Exception ();
          r = (rgb >> 8) & 0x0007;
          g = (rgb >> 4) & 0x0007;
          b = (rgb >> 0) & 0x0007;
        }
        else {
          r = (rgb >> 8) & 0x000F;
          g = (rgb >> 4) & 0x000F;
          b = (rgb >> 0) & 0x000F;
        }

        //r = (r << 5) + 16;
        //v = (v << 5) + 16;
        //b = (b << 5) + 16;
        if (lookup != null) {
          r = lookup [r];
          g = lookup [g];
          b = lookup [b];
        }
        _colorsR [i] = r;
        _colorsG [i] = g;
        _colorsB [i] = b;

        //_rawBGRA [i] = (uint) ((b << 24) | (v << 16) | (r << 8));
        _rawARGB [i] = (uint) ((0xFF << 24) | (r << 16) | (g << 8) | (b << 0));
        //if ((_colors1 [i] == 91) || (_colors2 [i] == 91) | (_colors3 [i] == 91)) {
        //  Console.WriteLine ("{0} {1} {2}", _colors1 [i], _colors2 [i], _colors3 [i]);
        //  int z = 0;
        //}
        //if (i < 32) {
        //  _colors1 [i] = (_colorsPatch [i] >> 16) & 0xFF;
        //  _colors2 [i] = (_colorsPatch [i] >> 8) & 0xFF;
        //  _colors3 [i] = (_colorsPatch [i] >> 0) & 0xFF;
        //}
        _colors [i] = Color.FromArgb (_colorsR [i], _colorsG [i], _colorsB [i]);
        _brushes [i] = new SolidBrush (_colors [i]);
      }

      _bitmap = new Bitmap (16 * 17 + 1, ((_size - 1) / 16 + 1) * 17 + 1, PixelFormat.Format32bppPArgb);
      Graphics graphics = Graphics.FromImage (_bitmap);
      graphics.Clear (Color.Black);
      for (int y = 0; y < 16; y++) {
        for (int x = 0; x < 16; x++) {
          if ((y * 16 + x) >= _size) {
            y = 16;
            break;
          }
          graphics.FillRectangle (_brushes [y * 16 + x], x * 17 + 1, y * 17 + 1, 16, 16);
        }
      }
    }

    public static Color GetColor (int rgb, PaletteLookup paletteLookup) {
      byte [] lookup = null;
      if (paletteLookup == PaletteLookup.ST) {
        lookup = _lookUpST;
      }
      else if (paletteLookup == PaletteLookup.AmigaToST) {
        lookup = _lookUpAmigaToST;
      }
      int r, g, b;
      r = (rgb >> 8) & 0x000F;
      g = (rgb >> 4) & 0x000F;
      b = (rgb >> 0) & 0x000F;

      //r = (r << 5) + 16;
      //v = (v << 5) + 16;
      //b = (b << 5) + 16;
      if (lookup != null) {
        r = lookup [r];
        g = lookup [g];
        b = lookup [b];
      }

      return (Color.FromArgb (r, g, b));
    }

    public Bitmap Bitmap {
      get {
        return (_bitmap);
      }
    }

    public int Size {
      get {
        return (_size);
      }
    }

    public Color GetColor (int i) {
      if ((i >= 0) && (i < _colors.Length)) {
        return (_colors [i]);
      }
      else {
        return (Color.Red);
      }
    }

    public uint GetRawARGB (int i) {
      if ((i >= 0) && (i < _rawARGB.Length)) {
        return (_rawARGB [i]);
      }
      else {
        return (0xFFFF0000);
      }
    }

    public Brush GetBrush (int i) {
      if ((i >= 0) && (i < _brushes.Length)) {
        return (_brushes [i]);
      }
      else {
        return (Brushes.Red);
      }
    }

    public static Palette DefaultST {
      get {
        byte [] bytes = new byte [] {0x07, 0x77, 0x07, 0x00, 0x00, 0x70, 0x07, 0x70, 0x00, 0x07, 0x07, 0x07, 0x00, 0x77, 0x05, 0x55,
                                     0x03, 0x33, 0x07, 0x33, 0x03, 0x73, 0x07, 0x73, 0x03, 0x37, 0x07, 0x37, 0x03, 0x77, 0x00, 0x00};
        return (new Palette (bytes, 0, 16, PaletteLookup.ST));
      }
    }
  }
}
