using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace GodsViewer {
  public class Map {
    public int _rasterNbColors;
    public int _rasterHeight;
    public Palette _rasterPalette;
    public int [,] _layerA;
    public int [,] _layerB;
    public List<MapItem> _mapItems;
    public string [] _stringsPuzzle;
    public List<MapPuzzle> _mapPuzzles;

    //public Palette _palette;
    public List<Bitmap> _mapSprites = new List<Bitmap> ();
    //public List<Bitmap> _mapSprites2 = new List<Bitmap> ();

    public Map (string mapFilename, string sprites1Filename, string sprites2Filename, Palette palette, int nbSprites) {
      LoadMap (mapFilename);
      LoadSprites (sprites1Filename, palette, Math.Min (nbSprites, 120));
      if (File.Exists (sprites2Filename)) {
        if (nbSprites <= 120)
          throw new Exception ();
        LoadSprites (sprites2Filename, palette, nbSprites - 120);
      }
      else {
        if (nbSprites > 120)
          throw new Exception ();
      }
    }

    private void LoadMap (string filename) {
      string basefolder = filename.Split (Path.DirectorySeparatorChar) [0];
      string folderMapParts = Path.Combine (basefolder, "map");
      if (!Directory.Exists (folderMapParts))
        Directory.CreateDirectory (folderMapParts);
      string baseFilename = Path.GetFileNameWithoutExtension (filename);

      byte [] buffer = ExtensionByte.CreateFromFile (filename);
      int offset = 0;
      int offsetStart = 0;
      _rasterNbColors = buffer.ReadWord (offset);
      offset += 2;
      _rasterHeight = buffer.ReadWord (offset);
      offset += 2;
      _rasterPalette = new Palette (buffer, offset, _rasterNbColors, Palette.PaletteLookup.STe);
      offset += _rasterNbColors * 2;
      int length = offset - offsetStart;
      string filenameMapPart = Path.Combine (folderMapParts, string.Format ("1_Raster {0}.map", baseFilename));
      if (!File.Exists (filenameMapPart))
        buffer.Extract (offsetStart, length).WriteToFile (filenameMapPart);

      offsetStart = offset;
      _layerA = new int [128, 64];
      for (int y = 0; y < 64; y++) {
        for (int x = 0; x < 128; x++) {
          _layerA [x, y] = buffer.ReadByte (offset++);
        }
      }
      length = offset - offsetStart;
      filenameMapPart = Path.Combine (folderMapParts, string.Format ("2_LayerA {0}.map", baseFilename));
      if (!File.Exists (filenameMapPart))
        buffer.Extract (offsetStart, length).WriteToFile (filenameMapPart);

      offsetStart = offset;
      _layerB = new int [128, 64];
      for (int y = 0; y < 64; y++) {
        for (int x = 0; x < 128; x++) {
          _layerB [x, y] = buffer.ReadByte (offset++);
        }
      }
      length = offset - offsetStart;
      filenameMapPart = Path.Combine (folderMapParts, string.Format ("3_LayerB {0}.map", baseFilename));
      if (!File.Exists (filenameMapPart))
        buffer.Extract (offsetStart, length).WriteToFile (filenameMapPart);

      // map objects
      offsetStart = offset;
      _mapItems = new List<MapItem> ();
      for (int i = 0; i < 200; i++) {
        MapItem mapItem = new MapItem (buffer, ref offset, i);
        if (mapItem._objectOrWeaponInfoIndex == 0xFFFF) {
          _mapItems.Add (null);
        }
        else {
          _mapItems.Add (mapItem);
        }
      }
      length = offset - offsetStart;
      filenameMapPart = Path.Combine (folderMapParts, string.Format ("4_Items {0}.map", baseFilename));
      if (!File.Exists (filenameMapPart))
        buffer.Extract (offsetStart, length).WriteToFile (filenameMapPart);

      offsetStart = offset;
      _stringsPuzzle = new string [40];
      int offsetTemp = offset;
      for (int i = 0; i < 40; i++) {
        int offset2 = (int) buffer.ReadLongSigned (offset);
        offset += 4;
        if (offset2 <= 0) {
          continue;
        }
        if (buffer.ReadByte (offsetTemp + offset2) == 0) {
          continue;
        }
        _stringsPuzzle [i] = buffer.ReadString (offsetTemp + offset2);
      }
      offset = offsetTemp + 4 * 40 + 42 * 40;
      length = offset - offsetStart;
      filenameMapPart = Path.Combine (folderMapParts, string.Format ("5_Puzzles strings {0}.map", baseFilename));
      if (!File.Exists (filenameMapPart))
        buffer.Extract (offsetStart, length).WriteToFile (filenameMapPart);

      // map puzzles
      offsetStart = offset;
      _mapPuzzles = new List<MapPuzzle> ();
      for (int i = 0; i < 100; i++) {
        MapPuzzle mapPuzzle = new MapPuzzle (buffer, ref offset, i);
        _mapPuzzles.Add (mapPuzzle);
      }
      length = offset - offsetStart;
      filenameMapPart = Path.Combine (folderMapParts, string.Format ("6_Puzzles {0}.map", baseFilename));
      if (!File.Exists (filenameMapPart))
        buffer.Extract (offsetStart, length).WriteToFile (filenameMapPart);

      if (offset != buffer.Length) {
        throw new Exception ();
      }
    }

    private void LoadSprites (string filename, Palette palette, int nbSprites) {
      Sprite._keyIndexColor = 1;
      Pi1 bitsXY = new Pi1 (filename, palette, Sprite.MasksEnum.Transparent);
      Sprite._keyIndexColor = 0;
      //_palette = bitsXY._palette;
      for (int i = 0; i < nbSprites; i++) {
        Bitmap bitmap = bitsXY._bitmap.ExtractBitmap ((i % 10) * 32, (i / 10) * 16, 32, 16);
        _mapSprites.Add (bitmap);
        //bitmap.Save (i.ToString ().PadLeft (3, '0') + ".png", ImageFormat.Png);
      }
    }

    //public Alfils02_Event GetEventAt (int x, int y, World world) {
    //  int indexB = _layerB [x, y];
    //  if (indexB < 3) {
    //    return (null);
    //  }
    //  return (world._events._events [indexB - 3]);
    //}

    //public void DrawMapObjects (Graphics g, Point pixelXY, World world) {
    //  DrawMapObjectsMap (g, pixelXY, world);
    //  DrawMapObjectsBox (g, pixelXY, world);
    //}

    //public void DrawMapObjectsOnMap (Graphics g, Point pixelXY, World world) {
    //  foreach (MapObject mapObject in _mapObjects) {
    //    if (mapObject == null) {
    //      continue;
    //    }
    //    Bitmap bitmap = mapObject.GetBitmap (world);
    //    if ((pixelXY.X >= mapObject._pixelX) && (pixelXY.X < (mapObject._pixelX + bitmap.Width))) {
    //      if ((pixelXY.Y >= mapObject._pixelY) && (pixelXY.Y < (mapObject._pixelY + bitmap.Height))) {
    //        mapObject.DrawOnMap (g, pixelXY, world);
    //      }
    //    }
    //  }
    //}

    //public void DrawMapObjectsOnBox (Graphics g, Point pixelXY, World world) {
    //  foreach (MapObject mapObject in _mapObjects) {
    //    if (mapObject == null) {
    //      continue;
    //    }
    //    Bitmap bitmap = mapObject.GetBitmap (world);
    //    if ((pixelXY.X >= mapObject._pixelX) && (pixelXY.X < (mapObject._pixelX + bitmap.Width))) {
    //      if ((pixelXY.Y >= mapObject._pixelY) && (pixelXY.Y < (mapObject._pixelY + bitmap.Height))) {
    //        mapObject.DrawOnBox (g, pixelXY, world);
    //      }
    //    }
    //  }
    //}
  }
}
