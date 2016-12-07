using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace GodsViewer {
  public static class RectangleExtension {

    public static Point Center (this Rectangle rectangle) {
      return (new Point ((rectangle.Left + rectangle.Right) >> 1, (rectangle.Top + rectangle.Bottom) >> 1));
    }
  }

  public static class SizeFExtension {

    public static Size ToSizeCeiling (this SizeF sizeF) {
      return (new Size ((int) Math.Ceiling (sizeF.Width), (int) Math.Ceiling (sizeF.Height)));
    }
  }

  public static class BitmapExtension {
    //public unsafe static Rectangle GetBoundingBox (this Bitmap b) {
    //  Rectangle r = Rectangle.Empty;
    //  BitmapData bitmapData = b.LockBits (new Rectangle (0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);

    //  //UInt32* p = (UInt32*) bitmapData.Scan0.ToPointer ();
    //  byte* p1 = (byte*) bitmapData.Scan0.ToPointer ();
    //  for (int y = 0; y < b.Height; y++) {
    //    //long offset = y * bitmapData.Stride;
    //    byte* p2 = p1;
    //    for (int x = 0; x < b.Width; x++) {
    //      //UInt32 color = p [x];
    //      byte byte1 = p2 [0];
    //      byte byte2 = p2 [1];
    //      byte byte3 = p2 [2];
    //      byte byte4 = p2 [3];
    //      p2 += 4;
    //    }
    //    //p += bitmapData.Stride / 4;
    //    p1 += bitmapData.Stride;
    //  }

    //  b.UnlockBits (bitmapData);
    //  return (r);
    //}

    public static Rectangle GetBoundingBox (this Bitmap b) {
      Rectangle result = Rectangle.Empty;

      int xMin = int.MaxValue;
      int xMax = int.MinValue;
      int yMin = int.MaxValue;
      int yMax = int.MinValue;

      BitmapData bitmapData = b.LockBits (new Rectangle (0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);
      int offset1 = 0;
      for (int y = 0; y < bitmapData.Height; y++) {
        //long offset = y * bitmapData.Stride;
        int offset2 = offset1;
        bool yDone = false;
        for (int x = 0; x < bitmapData.Width; x++) {
          uint color = (uint) Marshal.ReadInt32 (bitmapData.Scan0, offset2);
          if ((color & 0xFF000000) != 0) {
            if (x < xMin) {
              xMin = x;
            }
            if (x > xMax) {
              xMax = x;
            }
            if (!yDone) {
              if (y < yMin) {
                yMin = y;
              }
              if (y > yMax) {
                yMax = y;
              }
            }
          }

          offset2 += 4;
        }
        //p += bitmapData.Stride / 4;
        offset1 += bitmapData.Stride;
      }
      b.UnlockBits (bitmapData);

      if (xMin <= xMax) {
        if (yMin <= yMax) {
          result = new Rectangle (xMin, yMin, xMax - xMin + 1, yMax - yMin + 1);
        }
        else {
          throw new Exception ("?");
        }
      }

      return (result);
    }

    public static Bitmap ExtractShape (this Bitmap b) {
      Rectangle rectangle = b.GetBoundingBox ();
      if (rectangle == Rectangle.Empty) {
        return (b);
      }
      if (rectangle.Size == b.Size) {
        return (b);
      }
      Bitmap b2 = new Bitmap (rectangle.Width, rectangle.Height, b.PixelFormat);
      using (Graphics g = Graphics.FromImage (b2)) {
        g.DrawImage (b, 0, 0, rectangle, GraphicsUnit.Pixel);
      }
      return (b2);
    }

    public static Bitmap FitInto (this Bitmap b, int w, int h) {
      Bitmap dst = new Bitmap (16, 16, PixelFormat.Format32bppPArgb);
      using (Graphics g = Graphics.FromImage (dst)) {
        float zoom = Math.Min (Math.Min (((float) w) / b.Width, ((float) h) / b.Height), 1.0f);
        //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        int newWidth = (int) (b.Width * zoom);
        int newHeight = (int) (b.Height * zoom);
        int x = (16 - newWidth) / 2;
        int y = (16 - newHeight) / 2;
        g.DrawImage (b, x, y, newWidth, newHeight);
      }
      return (dst);
    }

    public static Bitmap Copy (this Bitmap b) {
      return (b.Copy (PixelFormat.Format32bppPArgb));
    }

    public static Bitmap Copy (this Bitmap b, PixelFormat pixelFormat) {
      //Bitmap dst = b.Clone (new Rectangle (0, 0, b.Width, b.Height), PixelFormat.Format32bppPArgb);
      Bitmap dst = new Bitmap (b.Width, b.Height, pixelFormat);
      using (Graphics g = Graphics.FromImage (dst)) {
        g.DrawImage (b, 0, 0);
      }
      return (dst);
    }

    public static Bitmap ExtractBitmap (this Bitmap b, int x, int y, int width, int height) {
      Rectangle rectangle = new Rectangle (x, y, width, height);
      Bitmap b2 = new Bitmap (rectangle.Width, rectangle.Height, b.PixelFormat);
      using (Graphics g = Graphics.FromImage (b2)) {
        g.DrawImage (b, 0, 0, rectangle, GraphicsUnit.Pixel);
      }
      return (b2);
    }

    public static void ReplaceColor (this Bitmap b, Color src, Color dst, bool ignoreAlphaInCompare) {
      uint colorSrc = (uint) src.ToArgb ();
      uint colorDst = (uint) dst.ToArgb ();
      BitmapData bitmapData = b.LockBits (new Rectangle (0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
      int offset1 = 0;
      for (int y = 0; y < bitmapData.Height; y++) {
        int offset2 = offset1;
        for (int x = 0; x < bitmapData.Width; x++) {
          uint color = (uint) Marshal.ReadInt32 (bitmapData.Scan0, offset2);
          if (ignoreAlphaInCompare) {
            if ((color & 0x00FFFFFF) == (colorSrc & 0x00FFFFFF)) {
              colorDst = (colorDst & 0x00FFFFFF) | (colorSrc & 0xFF000000);
              Marshal.WriteInt32 (bitmapData.Scan0, offset2, (int) colorDst);
            }
          }
          else {
            if (color == colorSrc) {
              Marshal.WriteInt32 (bitmapData.Scan0, offset2, (int) colorDst);
            }
          }
          offset2 += 4;
        }
        offset1 += bitmapData.Stride;
      }
      b.UnlockBits (bitmapData);
    }

    public static int CountColor (this Bitmap b, Color color) {
      uint colorHex = (uint) color.ToArgb ();
      BitmapData bitmapData = b.LockBits (new Rectangle (0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);
      int nb = 0;
      int offset1 = 0;
      for (int y = 0; y < bitmapData.Height; y++) {
        int offset2 = offset1;
        for (int x = 0; x < bitmapData.Width; x++) {
          uint colorHex2 = (uint) Marshal.ReadInt32 (bitmapData.Scan0, offset2);
          if (colorHex == colorHex2) {
            nb++;
          }
          offset2 += 4;
        }
        offset1 += bitmapData.Stride;
      }
      b.UnlockBits (bitmapData);
      return (nb);
    }
  }

  public static class BitmapDataExtension {

    public static int GetOffset (this BitmapData bitmapData, int x, int y) {
      return (y * bitmapData.Stride + x * 4);
    }

    public static int GetPixel (this BitmapData bitmapData, int offset) {
      int color = Marshal.ReadInt32 (bitmapData.Scan0, offset);
      return (color);
    }

    public static void SetPixel (this BitmapData bitmapData, int offset, int color) {
      Marshal.WriteInt32 (bitmapData.Scan0, offset, color);
    }

    public static int GetPixel (this BitmapData bitmapData, int x, int y) {
      int color = Marshal.ReadInt32 (bitmapData.Scan0, y * bitmapData.Stride + x * 4);
      return (color);
    }

    public static void SetPixel (this BitmapData bitmapData, int x, int y, int color) {
      Marshal.WriteInt32 (bitmapData.Scan0, y * bitmapData.Stride + x * 4, color);
    }

    /**
     * Searches for value that is not equal to given color value.
     */
    public static int ScanRight (this BitmapData bitmapData,
        int x,			//!< x-coordinate.
        int y,			//!< y-coordinate.
        int xmax,		//!< x-maximum coordinate
        int dwValue	//!< Specifies color value.
      ) {
      //ASSERT_VALID(this);
      //ASSERT(m_pDib != NULL);

      if (xmax < 0)
        return ++xmax;

      int nWidth = bitmapData.Width;

      if (x < 0 || nWidth <= x || y < 0 || Math.Abs (bitmapData.Height) <= y)
        return ++xmax;

      if (nWidth <= xmax)
        return ++xmax;

      int offset = bitmapData.GetOffset (x, y);
      for (; x <= xmax; ++x, offset += 4) {
        if (dwValue != bitmapData.GetPixel (offset))
          break;
      }
      return x;
    }

    /**
     * Searches for value that is equal to given color value.
     */
    public static int SearchRight (this BitmapData bitmapData,
        int x,			//!< x-coordinate.
        int y,			//!< y-coordinate.
        int xmax,		//!< x-maximum coordinate
        int dwValue	//!< Specifies color value.
      ) {
      //ASSERT_VALID (this);
      //ASSERT (m_pDib != NULL);

      if (xmax < 0)
        return ++xmax;

      int nWidth = bitmapData.Width;

      if (x < 0 || nWidth <= x || y < 0 || Math.Abs (bitmapData.Height) <= y)
        return ++xmax;

      if (nWidth <= xmax)
        return ++xmax;

      int offset = bitmapData.GetOffset (x, y);
      for (; x <= xmax; ++x, offset += 4) {
        if (dwValue == bitmapData.GetPixel (offset))
          break;
      }
      return x;
    }

    /**
     * Searches for value that is not equal to given color value.
     */
    public static int ScanLeft (this BitmapData bitmapData,
        int x,			//!< x-coordinate.
        int y,			//!< y-coordinate.
        int xmin,		//!< x-minimum coordinate
        int dwValue	//!< Specifies color value.
      ) {
      //ASSERT_VALID (this);
      //ASSERT (m_pDib != NULL);

      if (xmin < 0)
        return --xmin;

      int nWidth = bitmapData.Width;

      if (x < 0 || nWidth <= x || y < 0 || Math.Abs (bitmapData.Height) <= y)
        return --xmin;

      if (nWidth <= xmin)
        return --xmin;

      int offset = bitmapData.GetOffset (x, y);
      for (; x >= xmin; --x, offset -= 4) {
        if (dwValue != bitmapData.GetPixel (offset))
          break;
      }
      return x;
    }

    /**
     * Searches for value that is equal to given color value.
     */
    public static int SearchLeft (this BitmapData bitmapData,
        int x,			//!< x-coordinate.
        int y,			//!< y-coordinate.
        int xmin,		//!< x-minimum coordinate
        int dwValue	//!< Specifies color value.
      ) {
      //ASSERT_VALID(this);
      //ASSERT(m_pDib != NULL);

      if (xmin < 0)
        return --xmin;

      int nWidth = bitmapData.Width;

      if (x < 0 || nWidth <= x || y < 0 || Math.Abs (bitmapData.Height) <= y)
        return --xmin;

      if (nWidth <= xmin)
        return --xmin;

      int offset = bitmapData.GetOffset (x, y);
      for (; x >= xmin; --x, offset -= 4) {
        if (dwValue == bitmapData.GetPixel (offset))
          break;
      }
      return x;
    }
  }

  public static class GraphicsExtension {
    public static ColorMatrix _colorMatrix;
    public static ImageAttributes _imageAttributes;

    static GraphicsExtension () {
      _colorMatrix = new ColorMatrix ();
      _imageAttributes = new ImageAttributes ();
    }

    public static RectangleF MeasureString2 (this Graphics g, string text, Font font) {
      //SizeF space1 = MeasureDisplayStringWidth (g, "A A", font);
      //SizeF space2 = MeasureDisplayStringWidth (g, "A  A", font);
      //float spaceWidth = space2 - space1;
      //SizeF size = SizeF.Empty;
      //foreach (char c in text) {
      //  //SizeF sizeC = g.MeasureString (c.ToString (), font);
      //  SizeF sizeC = g.MeasureString (c.ToString (), font);
      //  size.Width += sizeC.Width;
      //  if (size.Height < sizeC.Height)
      //    size.Height = sizeC.Height;
      //}
      //return (size);
      return (g.MeasureDisplayStringWidth (text, font));
    }

    public static Dictionary<Font, Point> _fontsPointDelta = new Dictionary<Font, Point> ();
    public static Dictionary<Font, Point> _fontsSizeDelta = new Dictionary<Font, Point> ();

    public static void AddDelta (this Font font, Point pointDelta, Point sizeDelta) {
      _fontsPointDelta.Add (font, pointDelta);
      _fontsSizeDelta.Add (font, sizeDelta);
    }

    public static RectangleF MeasureDisplayStringWidth (this Graphics graphics, string text, Font font) {
      System.Drawing.StringFormat format = new System.Drawing.StringFormat ();
      System.Drawing.RectangleF rect = new System.Drawing.RectangleF (0, 0, 1000, 1000);
      System.Drawing.CharacterRange [] ranges = { new System.Drawing.CharacterRange (0, text.Length) };
      //System.Drawing.Region [] regions = new System.Drawing.Region [1];

      format.SetMeasurableCharacterRanges (ranges);
      //format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.FitBlackBox;
      format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

      Region [] regions = graphics.MeasureCharacterRanges (text, font, rect, format);
      rect = regions [0].GetBounds (graphics);
      //SizeF sizeF2 = new SizeF (rect.Right + 1.0f, rect.Bottom + 1.0f);
      //SizeF sizeF = new SizeF (rect.Width + 2.0f, rect.Height + 2.0f);
      //SizeF sizeF = new SizeF (rect.Width, rect.Height);
      //if ((rect.Right + 1.0f) != (rect.Width + 2.0f))
      //  throw new Exception ();
      //SizeF sizeF = rect.Size;
      //sizeF.Width -= 4;
      //sizeF.Height--;
      //return (int) (rect.Right + 1.0f);
      //return (rect.Size);
      //Console.WriteLine ("[{0}] {1}", text, rect);
      //rect.Width--;
      //rect.Height--;

      if (_fontsPointDelta.ContainsKey (font)) {
        Point pointDelta = _fontsPointDelta [font];
        rect.X -= pointDelta.X;
        rect.Y -= pointDelta.Y;
      }

      if (_fontsSizeDelta.ContainsKey (font)) {
        Point sizeDelta = _fontsSizeDelta [font];
        rect.Width += sizeDelta.X;
        rect.Height += sizeDelta.Y;
      }

      return (rect);
    }

    public static void DrawImage (this Graphics g, Image image, int x, int y, float alpha) {
      _colorMatrix.Matrix33 = alpha;
      _imageAttributes.SetColorMatrix (_colorMatrix);
      g.DrawImage (image, new Rectangle (x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, _imageAttributes);
    }

    public static void DrawPoint (this Graphics g, Brush brush, int x, int y) {
      g.FillRectangle (brush, x, y, 1, 1);
    }

    public static void DrawRectangleCrossed (this Graphics g, Pen pen, int x, int y, int width, int height) {
      g.DrawRectangle (pen, x, y, width, height);
      g.DrawLine (pen, x, y, x + width, y + height);
      g.DrawLine (pen, x, y + height, x + width, y);
    }

    public static void DrawStringOutlined (this Graphics g, string s, Font font, Brush brushFg, Brush brushBg, float x, float y) {
      RectangleF rectangleF = g.MeasureString2 (s, font);
      g.DrawString (s, font, brushBg, x - 1 - rectangleF.X, y - rectangleF.Y);
      g.DrawString (s, font, brushBg, x + 1 - rectangleF.X, y - rectangleF.Y);
      g.DrawString (s, font, brushBg, x - rectangleF.X, y - 1 - rectangleF.Y);
      g.DrawString (s, font, brushBg, x - rectangleF.X, y + 1 - rectangleF.Y);
      //g.DrawRectangle (Pens.Magenta, x, y, rectangleF.Size.Width - 1, rectangleF.Size.Height - 1);
      g.DrawString (s, font, brushFg, x - rectangleF.X, y - rectangleF.Y);
    }

    public static void DrawStringOutlinedCentered (this Graphics g, string s, Font font, Brush brushFg, Brush brushBg, float x, float y) {
      RectangleF rectangleF = g.MeasureString2 (s, font);
      g.DrawStringOutlined (s, font, brushFg, brushBg, x - (rectangleF.Size.Width / 2), y - (rectangleF.Size.Height / 2));
      //g.DrawStringOutlined (s, font, brushFg, brushBg, x, y - (rectangleF.Size.Height / 2));
    }

    public static void DrawLineOutlined (this Graphics g, Pen penFg, Pen penBg, int x1, int y1, int x2, int y2) {
      g.DrawLine (penBg, x1 - 1, y1, x2 - 1, y2);
      g.DrawLine (penBg, x1 + 1, y1, x2 + 1, y2);
      g.DrawLine (penBg, x1, y1 - 1, x2, y2 - 1);
      g.DrawLine (penBg, x1, y1 + 1, x2, y2 + 1);
      g.DrawLine (penFg, x1, y1, x2, y2);
    }

    public static AdjustableArrowCap _arrow = new AdjustableArrowCap (3.5f, 4, true);
    //private static float [] _dashPattern = new float [] { float.Epsilon, float.MaxValue };
    private static float [] _dashPatternBug = new float [] { 1f, float.MaxValue };
    public static void DrawArrowOutlinedSolid (this Graphics g, Pen penFg, Pen penBg, int x1, int y1, int x2, int y2, bool cap, bool dash) {
      penFg = new Pen (penFg.Color);
      penBg = new Pen (penBg.Color);
      Pen penFg2 = new Pen (penFg.Color);
      Pen penBg2 = new Pen (penBg.Color);
      //penFg.Width = 2f;
      //penBg.Width = 2f;
      //penFg2.Width = 2f;
      //penBg2.Width = 2f;

      penFg.DashPattern = _dashPatternBug;
      penBg.DashPattern = _dashPatternBug;
      penFg.DashStyle = DashStyle.Custom;
      penBg.DashStyle = DashStyle.Custom;
      penFg.CustomEndCap = _arrow;
      penBg.CustomEndCap = _arrow;

      g.DrawLine (penBg, x1 - 1, y1, x2 - 1, y2);
      g.DrawLine (penBg, x1 + 1, y1, x2 + 1, y2);
      g.DrawLine (penBg, x1, y1 - 1, x2, y2 - 1);
      g.DrawLine (penBg, x1, y1 + 1, x2, y2 + 1);
      g.DrawLine (penBg2, x1 - 1, y1, x2 - 1, y2);
      g.DrawLine (penBg2, x1 + 1, y1, x2 + 1, y2);
      g.DrawLine (penBg2, x1, y1 - 1, x2, y2 - 1);
      g.DrawLine (penBg2, x1, y1 + 1, x2, y2 + 1);

      g.DrawLine (penFg, x1, y1, x2, y2);
      g.DrawLine (penFg2, x1, y1, x2, y2);



      //if (cap) {
      //  penFg.CustomEndCap = _arrow;
      //  penBg.CustomEndCap = _arrow;
      //  penFg.Width = 1.01f;
      //  penBg.Width = 1.01f;
      //  //penFg.EndCap = LineCap.ArrowAnchor;
      //  //penBg.EndCap = LineCap.ArrowAnchor;
      //}
      //else {
      //  penFg.EndCap = LineCap.NoAnchor;
      //  penBg.EndCap = LineCap.NoAnchor;
      //}
      //g.DrawLineOutlined (penFg, penBg, x1, y1, x2, y2);
    }

    public static void DrawArrowOutlinedSolid (this Graphics g, Pen pen, int x1, int y1, int x2, int y2) {
      g.DrawArrowOutlinedSolid (pen, Pens.Black, x1, y1, x2, y2, true, false);
    }

    public static float [] _dashPattern = new float [] { 2f, 2f };
    public static void DrawArrowOutlined (this Graphics g, Pen penFg, Pen penBg, int x1, int y1, int x2, int y2, bool cap, bool dash) {
      penFg = new Pen (penFg.Color);
      penBg = new Pen (penBg.Color);

      if (cap) {
        penFg.CustomEndCap = _arrow;
        penBg.CustomEndCap = _arrow;
        penFg.Width = 1.01f;
        penBg.Width = 1.01f;
        //penFg.EndCap = LineCap.ArrowAnchor;
        //penBg.EndCap = LineCap.ArrowAnchor;
      }
      else {
        penFg.EndCap = LineCap.NoAnchor;
        penBg.EndCap = LineCap.NoAnchor;
      }

      if (dash) {
        penFg.DashPattern = _dashPattern;
        penBg.DashPattern = _dashPattern;
        penFg.DashStyle = DashStyle.Custom;
        penBg.DashStyle = DashStyle.Custom;
        penFg.Width = 2f;
        penBg.Width = 2f;
      }
      else {
        penFg.DashStyle = DashStyle.Solid;
        penBg.DashStyle = DashStyle.Solid;
      }

      g.DrawLineOutlined (penFg, penBg, x1, y1, x2, y2);
    }

    public static void DrawArrowOutlined (this Graphics g, Pen pen, int x1, int y1, int x2, int y2) {
      g.DrawArrowOutlined (pen, Pens.Black, x1, y1, x2, y2, true, true);
    }

    public static void DrawArrowOutlinedState (this Graphics g, Pen pen, int x1, int y1, int x2, int y2, bool state) {
      if (state)
        g.DrawArrowOutlined (pen, x1, y1, x2, y2);
      else
        g.DrawArrowOutlinedSolid (pen, x1, y1, x2, y2);
    }
  }
}
