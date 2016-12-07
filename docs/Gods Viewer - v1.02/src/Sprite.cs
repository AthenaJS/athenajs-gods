using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using System.Runtime.InteropServices;

namespace GodsViewer {
  public class Sprite : IDisposable {
    //private static bool _showInfos;
    //private static Color _borderColor;
    //private static Color _backwalkingColor;
    //private static Color _keyedColor;
    private static uint _keyedARGB = 0xFFFF00FF;
    private static uint _transparentARGB = 0x00FF00FF;
    public static byte _keyIndexColor = 0;
    //private static int _cols;

    //private Bitmap [] _bitmaps;
    private Bitmap _bitmap;
    //private static Color _transparent = Color.FromArgb (255, 255, 0, 255);
    private int _width;
    private int _height;
    //private string _title;
    //public string _fileName;

    public enum ModesEnum {
      Chunky,
      Interleaved,
      Scanline,
      Planar,

      //Interleaved_08Pixels_04Bytes,
      //Interleaved_16Pixels_08Bytes,
      //Interleaved_32Pixels_16Bytes,
      //Planar_5bp,
      //Planar_6bp,
      //Planar_7bp,
    }

    public enum MasksEnum {
      Opaque,
      Transparent,
      Keyed
    }

    //static SpriteBin () {
    //  _showInfos = Properties.Settings.Default.ShowInfos;
    //  _borderColor = Properties.Settings.Default.BorderColor;
    //  _backwalkingColor = Properties.Settings.Default.BackwalkingColor;
    //  KeyedColor = Properties.Settings.Default.KeyedColor;
    //}

    //public SpriteBin (int address, int paletteIndex, int paletteOffset) {
    //public SpriteBin (string fileName, int nb, Palette palette, MasksEnum mask, ) {
    //  int cols = 8;

    //  _fileName = fileName;
    //  //SpriteFileDef spriteFileDef = Achaos.GetSpriteFileDef (address);

    //  //string fileName = Path.Combine (Settings.Default.BaseFolder, fileNameShort);
    //  //Dump dump_SPRITES_BIN = new Dump (FilesManager.GetFile (fileName));
    //  //Palette palette = Achaos.GetPalette (paletteIndex);


    //  // type  bytes WxH
    //  //   0     0   0x0
    //  //   1   192   16x24         (8 bytes per line)
    //  //   2    32   8x8   Method3 (4 bytes per line)
    //  //   3   128   16x16 Method2
    //  //   4   512   32x32 Method1 (16 bytes per line = 512 bytes)
    //  //   5   256   16x32 Method2
    //  //   6   384   16x48 Method2
    //  //   7    32   8x8 (size 64?)
    //  //   8   768   32x48 Method1 (16 bytes per line = 768 bytes)

    //  //if ((dump.Size % 128) != 0) {
    //  //  throw new Exception ();
    //  //}
    //  //Console.WriteLine (string.Format ("Type: {0}", type));
    //  int width = -1;
    //  int height = -1;
    //  int bp = -1;
    //  ModesEnum mode;
    //  int interleave = -1;
    //  //if (version == VersionEnum.AmigaCD32) {
    //  //  int size = Achaos.GetBlockSize (version, type);
    //  //  if (type == 3) {
    //  //    width = 16;
    //  //    height = 16;
    //  //    bp = 4;
    //  //    mode = ModesEnum.Scanline;
    //  //    interleave = 0;
    //  //    //mode = ModesEnum.Interleaved_16Pixels_08Bytes;
    //  //    //_bitmaps = ReadMultipleInterleaved_8Bytes16Pixels (dump_SPRITES_BIN, nb, width, height, palette, paletteOffset, 1);
    //  //  }
    //  //  else if (type == 2) {
    //  //    width = 8;
    //  //    height = 8;
    //  //    bp = 4;
    //  //    mode = ModesEnum.Scanline;
    //  //    interleave = 0;
    //  //    //mode = ModesEnum.Interleaved_08Pixels_04Bytes;
    //  //    //_bitmaps = ReadMultipleInterleaved_4Bytes8Pixels (dump_SPRITES_BIN, nb, width, height, palette, paletteOffset, 1);
    //  //  }
    //  //  else if (type == 5) {
    //  //    width = 16;
    //  //    height = 32;
    //  //    bp = 4;
    //  //    mode = ModesEnum.Scanline;
    //  //    interleave = 0;
    //  //    //mode = ModesEnum.Interleaved_16Pixels_08Bytes;
    //  //    //_bitmaps = ReadMultipleInterleaved_8Bytes16Pixels (dump_SPRITES_BIN, nb, width, height, palette, paletteOffset, 1);
    //  //  }
    //  //  else if (type == 6) {
    //  //    width = 16;
    //  //    height = 48;
    //  //    bp = 4;
    //  //    mode = ModesEnum.Scanline;
    //  //    interleave = 0;
    //  //    //mode = ModesEnum.Interleaved_16Pixels_08Bytes;
    //  //    //_bitmaps = ReadMultipleInterleaved_8Bytes16Pixels (dump_SPRITES_BIN, nb, width, height, palette, paletteOffset, 1);
    //  //  }
    //  //  else if ((type == 4) || (type == 8)) {
    //  //    width = 32;
    //  //    height = size / 16; // 16 bytes for 32 pixels
    //  //    bp = 4;
    //  //    mode = ModesEnum.Chunky;
    //  //    interleave = 0;
    //  //    //_bitmaps = ReadMultipleChunky_1Byte2Pixels (dump_SPRITES_BIN, nb, width, height, palette, paletteOffset, 1);
    //  //  }
    //  //  else {
    //  //    throw new Exception ();
    //  //  }
    //  //}
    //  //else if ((version == VersionEnum.AmigaOCS) ||
    //  //         (version == VersionEnum.AtariST)) {
    //  //  bp = 4;
    //  //  mode = ModesEnum.Interleaved;
    //  //  interleave = 2;
    //  //  //   0     0   0x0
    //  //  //   1   192   16x24         (8 bytes per line)
    //  //  //   2    32   8x8   Method3 (4 bytes per line)
    //  //  //   3   128   16x16 Method2
    //  //  //   4   512   32x32 Method1 (16 bytes per line = 512 bytes)
    //  //  //   5   256   16x32 Method2
    //  //  //   6   384   16x48 Method2
    //  //  //   7    32   8x8 (size 64?)
    //  //  //   8   768   32x48 Method1 (16 bytes per line = 768 bytes)
    //  //  if (type == 3) {
    //  //    width = 16;
    //  //    height = 16;
    //  //  }
    //  //  else if (type == 2) {
    //  //    width = 8;
    //  //    height = 8;
    //  //  }
    //  //  else if (type == 5) {
    //  //    width = 16;
    //  //    height = 32;
    //  //  }
    //  //  else if (type == 6) {
    //  //    width = 16;
    //  //    height = 48;
    //  //  }
    //  //  else if (type == 4) {
    //  //    //else if ((type == 4) || (type == 8)) {
    //  //    width = 32;
    //  //    height = 32; // 16 bytes for 32 pixels
    //  //  }
    //  //  else if (type == 8) {
    //  //    width = 32;
    //  //    height = 48; // 16 bytes for 32 pixels
    //  //  }
    //  //  else {
    //  //    throw new Exception ();
    //  //  }
    //  //}
    //  //else {
    //  //  throw new Exception ();
    //  //}
    //  _widthSingle = width;
    //  _heightSingle = height;
    //  _bitmaps = CreateBitmaps (version, fileName, null, 0, nb, width, height, bp, mode, interleave, palette, paletteOffset, mask);
    //  _title = null;
    //  _cols = cols;
    //  //CreateSummaryBitmap (cols);
    //}

    //public SpriteBin (string fileName, int offset, int nb, int width, int height, int bp, ModesEnum mode, int interleave, Palette palette, int paletteOffset, MasksEnum mask, int cols) {
    //  _widthSingle = width;
    //  _heightSingle = height;
    //  _bitmaps = CreateBitmaps (fileName, offset, nb, width, height, bp, mode, interleave, palette, paletteOffset, mask);
    //  _bitmap = CreateSummaryBitmap (_bitmaps, 640, cols);
    //}

    public Sprite (byte [] bytes, int offset, int width, int height, int bp, ModesEnum mode, int interleave, Palette palette, int paletteOffset, MasksEnum mask) {
      //_fileName = fileName;
      _width = width;
      _height = height;
      //_title = title;
      _bitmap = CreateBitmap (bytes, offset, width, height, bp, mode, interleave, palette, paletteOffset, mask);
      //_bitmaps = CreateBitmaps (fileName, offset, nb, width, height, bp, mode, interleave, palette, paletteOffset, mask);
      //CreateSummaryBitmap (cols);
    }

    //public static bool ShowInfos {
    //  get {
    //    return (_showInfos);
    //  }
    //  set {
    //    _showInfos = value;
    //  }
    //}

    //public static Color BorderColor {
    //  get {
    //    return (_borderColor);
    //  }
    //  set {
    //    _borderColor = value;
    //  }
    //}

    //public static Color BackwalkingColor {
    //  get {
    //    return (_backwalkingColor);
    //  }
    //  set {
    //    _backwalkingColor = value;
    //  }
    //}

    //public static Color KeyedColor {
    //  get {
    //    return (_keyedColor);
    //  }
    //  set {
    //    _keyedColor = value;
    //    _keyedARGB = (uint) value.ToArgb ();
    //  }
    //}

    //public static Bitmap [] CreateBitmaps (string fileName, int? archive, int offset, int nb, int width, int height, int bp, ModesEnum mode, int interleave, Palette palette, int paletteOffset, MasksEnum mask) {
    //  Dump dump = new Dump (FilesManager.GetFile (fileName));
    //  return (CreateBitmaps (dump, offset, nb, width, height, bp, mode, interleave, palette, paletteOffset, mask));
    //}

    //public static Bitmap [] CreateBitmaps (string fileName, int offset, int nb, int width, int height, int bp, ModesEnum mode, int interleave, Palette palette, int paletteOffset, MasksEnum mask) {
    //  //Dump dump = new Dump (FilesManager.GetFile (version, fileName, archive));
    //  return (CreateBitmaps (dump, offset, nb, width, height, bp, mode, interleave, palette, paletteOffset, mask));
    //}

    //public static Bitmap [] CreateBitmaps (Dump dump, int offset, int nb, int width, int height, int bp, ModesEnum mode, int interleave, Palette palette, int paletteOffset, MasksEnum mask) {
    //  Bitmap [] bitmaps = null;
    //  if (mode == ModesEnum.Chunky) {
    //    if (bp == 4) {
    //      bitmaps = ReadMultipleChunky_4bp (dump, offset, nb, width, height, palette, paletteOffset, mask);
    //    }
    //    else {
    //      throw new Exception ();
    //    }
    //  }
    //  else if (mode == ModesEnum.Interleaved) {
    //    int interleaveSize = interleave;
    //    int scanlineStride = width / 8 * bp;
    //    int bitplaneStride = interleaveSize;
    //    bitmaps = ReadBitmaps (dump, offset, nb, width, height, interleaveSize, scanlineStride, bitplaneStride, bp, palette, paletteOffset, mask);
    //  }
    //  else if (mode == ModesEnum.Scanline) {
    //    int interleaveSize = width / 8;
    //    int scanlineStride = width / 8 * bp;
    //    int bitplaneStride = interleaveSize;
    //    bitmaps = ReadBitmaps (dump, offset, nb, width, height, interleaveSize, scanlineStride, bitplaneStride, bp, palette, paletteOffset, mask);
    //  }
    //  else if (mode == ModesEnum.Planar) {
    //    int interleaveSize = width / 8;
    //    int scanlineStride = width / 8;
    //    int bitplaneStride = width * height / 8;
    //    bitmaps = ReadBitmaps (dump, offset, nb, width, height, interleaveSize, scanlineStride, bitplaneStride, bp, palette, paletteOffset, mask);
    //  }
    //  else {
    //    throw new Exception ();
    //  }
    //  return (bitmaps);
    //}

    public static Bitmap [] CreateBitmaps (byte [] bytes, int offset, int nb, int width, int height, int bp, ModesEnum mode, int interleave, Palette palette, int paletteOffset, MasksEnum mask) {
      Bitmap [] bitmaps = new Bitmap [nb];
      int size = CalcSizeInBytes (width, height, bp, mode);
      for (int i = 0; i < nb; i++) {
        bitmaps [i] = CreateBitmap (bytes, offset + size * i, width, height, bp, mode, interleave, palette, paletteOffset, mask);
      }
      return (bitmaps);
    }

    public static Bitmap CreateBitmap (byte [] bytes, int offset, int width, int height, int bp, ModesEnum mode, int interleave, Palette palette, int paletteOffset, MasksEnum mask) {
      Bitmap bitmap = null;
      if (mode == ModesEnum.Chunky) {
        if (bp == 4) {
          bitmap = ReadChunky_4bp (bytes, offset, width, height, palette, paletteOffset, mask);
        }
        else {
          throw new Exception ();
        }
      }
      else if (mode == ModesEnum.Interleaved) {
        int interleaveSize = interleave;
        int scanlineStride = width / 8 * bp;
        int bitplaneStride = interleaveSize;
        bitmap = ReadBitmap (bytes, offset, width, height, interleaveSize, scanlineStride, bitplaneStride, bp, palette, paletteOffset, mask);
      }
      else if (mode == ModesEnum.Scanline) {
        int interleaveSize = width / 8;
        int scanlineStride = width / 8 * bp;
        int bitplaneStride = interleaveSize;
        bitmap = ReadBitmap (bytes, offset, width, height, interleaveSize, scanlineStride, bitplaneStride, bp, palette, paletteOffset, mask);
      }
      else if (mode == ModesEnum.Planar) {
        int interleaveSize = width / 8;
        int scanlineStride = width / 8;
        int bitplaneStride = width * height / 8;
        bitmap = ReadBitmap (bytes, offset, width, height, interleaveSize, scanlineStride, bitplaneStride, bp, palette, paletteOffset, mask);
      }
      else {
        throw new Exception ();
      }
      return (bitmap);
    }

    public static int CalcSizeInBytes (int width, int height, int nbBitplanes, ModesEnum mode) {
      if (mode == ModesEnum.Chunky) {
        return (width * height / 2); // 16 bytes for 32 pixels
      }
      else {
        return (width * height / 8 * nbBitplanes); // 1 byte for 8 pixels, X bitplanes
      }
    }

    //public void CreateSummaryBitmap (int cols) {
    //  _bitmap = CreateSummaryBitmap (_bitmaps, cols, _title);
    //}

    //public static Bitmap CreateSummaryBitmap (Bitmap [] bitmaps, int cols, string title) {
    //  int widthMax = 1024;
    //  StringDrawer sd = new StringDrawer ();
    //  int widthSingle = bitmaps [0].Width;
    //  int heightSingle = bitmaps [0].Height;
    //  int widthSingle2 = widthSingle;
    //  int xText = 0;
    //  if (_showInfos) {
    //    if (widthSingle2 < 16) {
    //      widthSingle2 = 16;
    //      sd.SetTextAlignment (StringAlignment.Near, StringAlignment.Center);
    //    }
    //    else {
    //      sd.SetTextAlignment (StringAlignment.Center, StringAlignment.Center);
    //      xText = widthSingle2 / 2;
    //    }
    //  }

    //  int nbPerRow = cols;
    //  if (nbPerRow == 0) {
    //    nbPerRow = (widthMax - 1) / (widthSingle2 + 1);
    //  }
    //  if (nbPerRow == 0) {
    //    nbPerRow = 1;
    //  }
    //  //int nbPerRow = width / widthSingle;
    //  int nbRows = (bitmaps.Length - 1) / nbPerRow + 1;
    //  if (bitmaps.Length < nbPerRow) {
    //    nbPerRow = bitmaps.Length;
    //  }
    //  int width = nbPerRow * (widthSingle2 + 1) + 1;
    //  int height = nbRows * (heightSingle + 1) + 1;
    //  if (_showInfos) {
    //    height += 8 * nbRows - 1;
    //  }
    //  if (_showInfos) {
    //    if (title != null) {
    //      height += 7;
    //      Graphics g2 = Graphics.FromImage (bitmaps [0]);
    //      int s = (int) g2.MeasureString (title, StringDrawer._font).Width;
    //      if (width < (s)) {
    //        width = s;
    //      }
    //    }
    //  }
    //  Bitmap bitmap = new Bitmap (width, height, PixelFormat.Format32bppPArgb);
    //  Graphics g = Graphics.FromImage (bitmap);
    //  g.Clear (Color.Transparent);
    //  Pen pen = new Pen (_borderColor);
    //  //Brush brush = new SolidBrush (_backwalkingColor);
    //  Brush brush = new SolidBrush (Color.Transparent);
    //  int y2 = 0;
    //  if (_showInfos) {
    //    y2 = 7;
    //  }
    //  for (int i = 0; i < bitmaps.Length; i++) {
    //    int x1 = i % nbPerRow;
    //    int y1 = i / nbPerRow;
    //    int x = x1 * (widthSingle2 + 1) + 1;
    //    int y = y1 * (heightSingle + 1) + 1;
    //    if (_showInfos) {
    //      y += y1 * 8;
    //      if (title != null) {
    //        y += 7;
    //      }
    //      sd.DrawString (g, Brushes.White, i.ToString (), x + xText, y + 3);
    //    }
    //    g.FillRectangle (brush, x, y + y2, widthSingle, heightSingle);
    //    g.DrawImage (bitmaps [i], x, y + y2);
    //    g.DrawRectangle (pen, x - 1, y - 1 + y2, widthSingle + 1, heightSingle + 1);
    //  }
    //  if (_showInfos) {
    //    if (title != null) {
    //      sd.SetTextAlignment (StringAlignment.Near, StringAlignment.Near);
    //      sd.DrawString (g, Brushes.White, title, 1, 0);
    //    }
    //  }

    //  return (bitmap);
    //}

    private static Bitmap AllocBitmap (int width, int height, out uint [] buffer) {
      // 4 bytes per pixel, format: ARGB
      buffer = new uint [width * height];
      // if not pinned the GC can move around the array
      GCHandle handle = GCHandle.Alloc (buffer, GCHandleType.Pinned);
      IntPtr pointer = Marshal.UnsafeAddrOfPinnedArrayElement (buffer, 0);
      Bitmap bitmap = new Bitmap (width, height, width * 4, PixelFormat.Format32bppPArgb, pointer);
      return (bitmap);
    }

    private static void WriteColor (uint [] buffer, int dst, uint color, bool isZero, MasksEnum mask) {
      if ((!isZero) || (mask == MasksEnum.Opaque)) {
        buffer [dst] = color;
      }
      else {
        if (mask == MasksEnum.Transparent) {
          buffer [dst] = _transparentARGB;
        }
        else if (mask == MasksEnum.Keyed) {
          buffer [dst] = _keyedARGB;
        }
      }
    }

    //private static Bitmap [] ReadMultipleChunky_4bp (Dump dump, int offset, int nb, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
    //  int size = width * height / 2; // 16 bytes for 32 pixels
    //  Bitmap [] bitmaps = new Bitmap [nb];
    //  for (int j = 0; j < nb; j++) {
    //    int address = j * size;
    //    bitmaps [j] = ReadChunky_4bp (dump, offset + address, width, height, palette, paletteOffset, mask);
    //  }
    //  return (bitmaps);
    //}

    private static Bitmap ReadChunky_4bp (byte [] bytes, int src, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
      uint [] buffer;
      Bitmap bitmap = AllocBitmap (width, height, out buffer);

      int dst = 0;
      for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x += 2) {
          byte b = (byte) bytes.ReadByte (src++);
          byte color1 = (byte) ((b >> 4) & 0x0F);
          byte color2 = (byte) ((b >> 0) & 0x0F);
          WriteColor (buffer, dst, palette.GetRawARGB (color1 + paletteOffset), color1 == _keyIndexColor, mask);
          dst++;
          WriteColor (buffer, dst, palette.GetRawARGB (color2 + paletteOffset), color2 == _keyIndexColor, mask);
          dst++;
        }
      }
      return (bitmap);
    }

    //private static Bitmap [] ReadMultipleInterleaved_08Pixels_04Bytes (Dump dump, int offset, int nb, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
    //  int size = width * height / 2; // 4 bytes for 8 pixels
    //  Bitmap [] bitmaps = new Bitmap [nb];
    //  for (int j = 0; j < nb; j++) {
    //    int address = j * size;
    //    bitmaps [j] = ReadInterleaved_08Pixels_04Bytes (dump, offset + address, width, height, palette, paletteOffset, mask);
    //  }
    //  return (bitmaps);
    //}

    //Elements' Summary
    //ScanLines 	Number of scanlines for this bitmap. This value must not be negative  
    //ScanLineBytes 	Number of data bytes per scanline. This value must not be negative  
    //ScanLineStride 	Byte offset between the start of two consecutive scanlines. This value is permitted to be negative, denoting a bitmap whose content is flipped at the x axis.  
    //PlaneStride 	Byte offset between the start of two consecutive planes. This value is permitted to be negative. If this value is zero, the bitmap is assumed to be in chunky format, otherwise it is assumed to be planar. The difference between chunky and planar layout lies in the way how color channels are interleaved. For a chunky format, all channel data for a single pixel lies consecutively in memory. For a planar layout, the first channel of all pixel is stored consecutive, followed by the second channel, and so forth. 
    //ColorSpace 	Color space the bitmap colors shall be interpreted within. 
    //Palette 	This member determines whether the bitmap data are actually indices into a color map. 
    //IsMsbFirst 	This member determines the bit order (only relevant if a pixel uses less than 8 bits, of course). 
    private static byte GetPixelColor (byte [] bytes, int address, int x, int y, int interleaveSize, int scanlineStride, int bitplaneStride, int nbBitplanes) {
      byte color = 0;
      int byteX = x >> 3; // byteX = X / 8
      for (int bitplane = 0; bitplane < nbBitplanes; bitplane++) {
        // offset of the byte in the interleave
        int offset = (byteX % interleaveSize);
        // offset of the interleave
        offset += (byteX / interleaveSize) * (nbBitplanes * interleaveSize);
        // offset of the bitplane
        offset += bitplane * bitplaneStride;
        // offset of the scanline
        offset += y * scanlineStride;
        if ((bytes.ReadByte (address + offset) & (1 << (7 - (x % 8)))) != 0) {
          color |= (byte) (1 << bitplane);
        }
      }
      return (color);
    }

    //private static byte getpixelcol (Dump dump, int baseAddress, int x, int y, ModesEnum mode, int width, int height, int nbBitplanes) {
    //  byte col = 0;
    //  for (int bitplane = 0; bitplane < nbBitplanes; bitplane++) {
    //    int bitplaneOffset = 0;
    //    int bitNth = 0;
    //    if (mode == ModesEnum.AM) {
    //      // Amiga type bitplanes
    //      bitplaneOffset = (width / 8 * height) * bitplane;
    //      bitNth = x + (y * width);
    //    }
    //    else if (mode == ModesEnum.ST) {
    //      // ST Type bitplanes
    //      bitplaneOffset = (width / 8) * bitplane;
    //      bitNth = x + (y * width * nbBitplanes);
    //    }
    //    else {
    //      throw new Exception ();
    //    }

    //    int byteNth = bitNth / 8;
    //    int bitNthInByteNth = 7 - (bitNth % 8);
    //    if ((dump.ReadByte (baseAddress + bitplaneOffset + byteNth) & (1 << bitNthInByteNth)) != 0) {
    //      col |= (byte) (1 << bitplane);
    //    }
    //  }
    //  return (col);
    //}

    //private static byte [] _lookUp = { 0, 2, 4, 6, 8, 10, 12, 14, 1, 3, 5, 7, 9, 11, 13, 15 };
    //private static Bitmap ReadInterleaved_08Pixels_04Bytes (Dump dump, int src, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
    //  uint [] buffer;
    //  Bitmap bitmap = AllocBitmap (width, height, out buffer);

    //  int dst = 0;
    //  for (int y = 0; y < height; y++) {
    //    for (int x = 0; x < width; x += 8) {
    //      byte byte1 = (byte) dump.ReadByte (src);
    //      src++;
    //      byte byte2 = (byte) dump.ReadByte (src);
    //      src++;
    //      byte byte3 = (byte) dump.ReadByte (src);
    //      src++;
    //      byte byte4 = (byte) dump.ReadByte (src);
    //      src++;
    //      for (int i = 0; i < 8; i++) {
    //        byte bit1 = (byte) ((byte1 >> (7 - i)) & 0x01);
    //        byte bit2 = (byte) ((byte2 >> (7 - i)) & 0x01);
    //        byte bit3 = (byte) ((byte3 >> (7 - i)) & 0x01);
    //        byte bit4 = (byte) ((byte4 >> (7 - i)) & 0x01);
    //        byte color = (byte) ((bit4 << 3) + (bit3 << 2) + (bit2 << 1) + bit1);
    //        WriteColor (buffer, dst, palette.GetRawARGB (color + paletteOffset), color == 0, mask);
    //        dst++;
    //      }
    //    }
    //  }
    //  return (bitmap);
    //}

    //private static Bitmap [] ReadMultipleInterleaved_16Pixels_08Bytes (Dump dump, int offset, int nb, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
    //  int size = width * height / 2; // 8 bytes for 16 pixels
    //  Bitmap [] bitmaps = new Bitmap [nb];
    //  for (int j = 0; j < nb; j++) {
    //    int address = j * size;
    //    bitmaps [j] = ReadInterleaved_16Pixels_08Bytes (dump, offset + address, width, height, palette, paletteOffset, mask);
    //  }
    //  return (bitmaps);
    //}

    //private static Bitmap ReadInterleaved_16Pixels_08Bytes (Dump dump, int src, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
    //  uint [] buffer;
    //  Bitmap bitmap = AllocBitmap (width, height, out buffer);

    //  int dst = 0;
    //  for (int y = 0; y < height; y++) {
    //    for (int x = 0; x < width; x += 16) {
    //      ushort word1 = (ushort) dump.ReadWord (src);
    //      src += 2;
    //      ushort word2 = (ushort) dump.ReadWord (src);
    //      src += 2;
    //      ushort word3 = (ushort) dump.ReadWord (src);
    //      src += 2;
    //      ushort word4 = (ushort) dump.ReadWord (src);
    //      src += 2;
    //      for (int i = 0; i < 16; i++) {
    //        byte bit1 = (byte) ((word1 >> (15 - i)) & 0x0001);
    //        byte bit2 = (byte) ((word2 >> (15 - i)) & 0x0001);
    //        byte bit3 = (byte) ((word3 >> (15 - i)) & 0x0001);
    //        byte bit4 = (byte) ((word4 >> (15 - i)) & 0x0001);
    //        byte color = (byte) ((bit4 << 3) + (bit3 << 2) + (bit2 << 1) + bit1);
    //        WriteColor (buffer, dst, palette.GetRawARGB (color + paletteOffset), color == 0, mask);
    //        dst++;
    //      }
    //    }
    //  }
    //  return (bitmap);
    //}

    //private static Bitmap [] ReadMultipleInterleaved_32Pixels_16Bytes (Dump dump, int offset, int nb, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
    //  int size = width * height / 2; // 16 bytes for 32 pixels
    //  Bitmap [] bitmaps = new Bitmap [nb];
    //  for (int j = 0; j < nb; j++) {
    //    int address = j * size;
    //    bitmaps [j] = ReadInterleaved_32Pixels_16Bytes (dump, offset + address, width, height, palette, paletteOffset, mask);
    //  }
    //  return (bitmaps);
    //}

    //private static Bitmap ReadInterleaved_32Pixels_16Bytes (Dump dump, int src, int width, int height, Palette palette, int paletteOffset, MasksEnum mask) {
    //  uint [] buffer;
    //  Bitmap bitmap = AllocBitmap (width, height, out buffer);

    //  int dst = 0;
    //  for (int y = 0; y < height; y++) {
    //    for (int x = 0; x < width; x += 32) {
    //      uint long1 = (uint) dump.ReadLong (src);
    //      src += 4;
    //      uint long2 = (uint) dump.ReadLong (src);
    //      src += 4;
    //      uint long3 = (uint) dump.ReadLong (src);
    //      src += 4;
    //      uint long4 = (uint) dump.ReadLong (src);
    //      src += 4;
    //      for (int i = 0; i < 32; i++) {
    //        byte bit1 = (byte) ((long1 >> (31 - i)) & 0x0001);
    //        byte bit2 = (byte) ((long2 >> (31 - i)) & 0x0001);
    //        byte bit3 = (byte) ((long3 >> (31 - i)) & 0x0001);
    //        byte bit4 = (byte) ((long4 >> (31 - i)) & 0x0001);
    //        byte color = (byte) ((bit4 << 3) + (bit3 << 2) + (bit2 << 1) + bit1);
    //        WriteColor (buffer, dst, palette.GetRawARGB (color + paletteOffset), color == 0, mask);
    //        dst++;
    //      }
    //    }
    //  }
    //  return (bitmap);
    //}

    //private static Bitmap [] ReadBitmaps (Dump dump, int address, int nb, int width, int height, int interleaveSize, int scanlineStride, int bitplaneStride, int nbBitplanes, Palette palette, int paletteOffset, MasksEnum mask) {
    //  if ((width % 8) != 0) {
    //    throw new Exception ();
    //  }
    //  int size = width * height / 8 * nbBitplanes; // 1 byte for 8 pixels, X bitplanes
    //  Bitmap [] bitmaps = new Bitmap [nb];
    //  for (int i = 0; i < nb; i++) {
    //    int offset = i * size;
    //    bitmaps [i] = ReadBitmap (dump, address + offset, width, height, interleaveSize, scanlineStride, bitplaneStride, nbBitplanes, palette, paletteOffset, mask);
    //  }
    //  return (bitmaps);
    //}

    private static Bitmap ReadBitmap (byte [] bytes, int address, int width, int height, int interleaveSize, int scanlineStride, int bitplaneStride, int nbBitplanes, Palette palette, int paletteOffset, MasksEnum mask) {
      uint [] buffer;
      Bitmap bitmap = AllocBitmap (width, height, out buffer);

      int dst = 0;
      for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
          byte color = GetPixelColor (bytes, address, x, y, interleaveSize, scanlineStride, bitplaneStride, nbBitplanes);
          WriteColor (buffer, dst, palette.GetRawARGB (color + paletteOffset), color == _keyIndexColor, mask);
          dst++;
        }
      }
      return (bitmap);
    }

    //private static Bitmap [] ReadMultiplePlanar_Xbp (Dump dump, int offset, int nb, int width, int height, Palette palette, int paletteOffset, MasksEnum mask, int nbBitplanes) {
    //  if ((width % 8) != 0) {
    //    throw new Exception ();
    //  }
    //  int size = width * height / 8 * nbBitplanes; // 1 byte for 8 pixels, X bitplanes
    //  Bitmap [] bitmaps = new Bitmap [nb];
    //  for (int j = 0; j < nb; j++) {
    //    int address = j * size;
    //    bitmaps [j] = ReadPlanar_Xbp (dump, offset + address, width, height, palette, paletteOffset, mask, nbBitplanes);
    //  }
    //  return (bitmaps);
    //}

    //private static Bitmap ReadPlanar_Xbp (Dump dump, int src, int width, int height, Palette palette, int paletteOffset, MasksEnum mask, int nbBitplanes) {
    //  uint [] buffer;
    //  Bitmap bitmap = AllocBitmap (width, height, out buffer);

    //  int dst = 0;
    //  int bitplanSize = width * height / 8;
    //  byte [] bytes = new byte [nbBitplanes];
    //  for (int y = 0; y < height; y++) {
    //    for (int x = 0; x < width; x += 8) {
    //      for (int j = 0; j < nbBitplanes; j++) {
    //        bytes [j] = (byte) dump.ReadByte (src + bitplanSize * j);
    //      }
    //      //byte byte1 = (byte) dump.ReadByte (src);
    //      //byte byte2 = (byte) dump.ReadByte (src + bitplanSize);
    //      //byte byte3 = (byte) dump.ReadByte (src + bitplanSize * 2);
    //      //byte byte4 = (byte) dump.ReadByte (src + bitplanSize * 3);
    //      //byte byte5 = (byte) dump.ReadByte (src + bitplanSize * 4);
    //      //byte byte6 = (byte) dump.ReadByte (src + bitplanSize * 5);
    //      //byte byte7 = (byte) dump.ReadByte (src + bitplanSize * 6);
    //      src++;
    //      for (int i = 7; i >= 0; i--) {
    //        byte color = 0;
    //        for (int j = nbBitplanes - 1; j >= 0; j--) {
    //          color = (byte) ((color << 1) | ((byte) ((bytes [j] >> i) & 0X01)));
    //        }
    //        //byte bit1 = (byte) ((byte1 >> (7 - i)) & 0x01);
    //        //byte bit2 = (byte) ((byte2 >> (7 - i)) & 0x01);
    //        //byte bit3 = (byte) ((byte3 >> (7 - i)) & 0x01);
    //        //byte bit4 = (byte) ((byte4 >> (7 - i)) & 0x01);
    //        //byte bit5 = (byte) ((byte5 >> (7 - i)) & 0x01);
    //        //byte bit6 = (byte) ((byte6 >> (7 - i)) & 0x01);
    //        //byte bit7 = (byte) ((byte7 >> (7 - i)) & 0x01);
    //        //byte color = (byte) ((bit7 << 6) + (bit6 << 5) + (bit5 << 4) + (bit4 << 3) + (bit3 << 2) + (bit2 << 1) + bit1);
    //        WriteColor (buffer, dst, palette.GetRawARGB (color + paletteOffset), color == 0, mask);
    //        dst++;
    //      }
    //    }
    //  }
    //  return (bitmap);
    //}

    //public Bitmap GetBitmap (int i) {
    //  if ((i >= 0) && (i < _bitmaps.Length)) {
    //    return (_bitmaps [i]);
    //  }
    //  else {
    //    return (null);
    //  }
    //}

    public Bitmap Bitmap {
      get {
        //if (_bitmap == null) {
        //  CreateSummaryBitmap (_cols);
        //}
        return (_bitmap);
      }
    }

    public int Width {
      get {
        return (_width);
      }
    }

    public int Height {
      get {
        return (_height);
      }
    }

    public void Dispose () {
      //foreach (Bitmap bitmap in _bitmaps) {
      //  bitmap.Dispose ();
      //}
      _bitmap.Dispose ();
    }
  }
}
