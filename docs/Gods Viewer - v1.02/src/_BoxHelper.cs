using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GodsViewer {
  public class BoxHelper {
    public int _deltaY;

    public Size _lineSize;
    public Size _totalSize;
    public Size _totalSizeOld;
    public int _titleWidth;

    public List<int> _separations;

    public delegate void DrawOnBoxDelegate (Graphics g, Point cellXY, Point pixelXY, World world);
    public static Brush _popupBrush = new SolidBrush (Color.FromArgb (128, 0, 0, 0));
    //public static int _previousBoxHeight = 0;
    //public static int _nextBoxDeltaY = 0;
    private static BoxHelper _boxHelper;
    public static Size _boxesSize;
    public static Point _boxesXY;
    public static Point _deltaXY;
    public static int _recursion;
    private static bool _simulating;

    public static int _spaceBetweenBoxY = 1;
    public static int _margin = 1;

    public static Point _cropXY;
    public static Size _cropSize;
    //public BoxHelper (Point pixelXY)
    //  : this (pixelXY, 3) {
    //}

    public static void Reset () {
      //_previousBoxHeight = 0;
      _boxHelper = null;
      _boxesSize = Size.Empty;
      _boxesXY = Point.Empty;
      _deltaXY = new Point (0, 48);
      _recursion = -1;
      _simulating = true;
    }

    public BoxHelper (int deltaY) {
      _deltaY = deltaY;
      _titleWidth = -1;
      Rewind ();
    }

    public void Rewind () {
      _lineSize = Size.Empty;
      _totalSizeOld = _totalSize;
      _totalSize = Size.Empty;
      _separations = new List<int> ();
    }

    //private void SimulationDone () {
    //  _totalWidth = 0;
    //  _totalHeight = 0;
    //  _lineWidth = 0;
    //  _lineHeight = 0;

    //  _draw = true;
    //}

    //private void AddToLine (int width, int height) {
    //  _lineSize.Width += width;
    //  if (_lineHeight < height) {
    //    _lineHeight = height;
    //  }
    //}

    private void AddToLine (Size size) {
      _lineSize.Width += size.Width;
      if (_lineSize.Height < size.Height) {
        _lineSize.Height = size.Height;
      }
    }

    public static void NextLine () {
      _boxHelper.NextLine_ ();
    }

    private void NextLine_ () {
      if (_totalSize.Width < _lineSize.Width) {
        _totalSize.Width = _lineSize.Width;
      }
      _totalSize.Height += _lineSize.Height;

      _lineSize = Size.Empty;
    }

    private Point BoxesXY {
      get {
        return (new Point (_boxesXY.X + _deltaXY.X, _boxesXY.Y + _deltaXY.Y + _deltaY));
      }
    }

    private Point LineXY {
      get {
        return (new Point (_boxesXY.X + _deltaXY.X + 2 + _margin + _lineSize.Width, _boxesXY.Y + _deltaXY.Y + 2 + _margin + _totalSize.Height + _deltaY));
      }
    }

    public Size Size {
      get {
        return (new Size ((2 + _margin) * 2 + _totalSize.Width, (2 + _margin) * 2 + _totalSize.Height));
      }
    }

    private static void Crop () {
      if (_deltaXY.X == 0) {
        _deltaXY.X = -_boxesSize.Width / 2;
      }
      if (_deltaXY.Y == 0) {
        _deltaXY.Y = -_boxesSize.Height / 2;
      }

      if ((_boxesXY.X - _cropXY.X + _deltaXY.X + _boxesSize.Width) > _cropSize.Width) {
        _boxesXY.X = _cropXY.X + _cropSize.Width - _boxesSize.Width - _deltaXY.X;
      }
      if ((_boxesXY.Y - _cropXY.Y + _deltaXY.Y + _boxesSize.Height) > _cropSize.Height) {
        _boxesXY.Y = _cropXY.Y + _cropSize.Height - _boxesSize.Height - _deltaXY.Y;
      }
      if ((_boxesXY.X - _cropXY.X + _deltaXY.X) < 0) {
        _boxesXY.X = _cropXY.X - _deltaXY.X;
      }
      if ((_boxesXY.Y - _cropXY.Y + _deltaXY.Y) < 0) {
        _boxesXY.Y = _cropXY.Y - _deltaXY.Y;
      }
    }

    private void DrawBox (Graphics g) {
      Size size = Size;
      Point boxesXY = BoxesXY;
      g.FillRectangle (_popupBrush, boxesXY.X, boxesXY.Y, size.Width, size.Height);
      g.DrawRectangle (Pens.Black, boxesXY.X, boxesXY.Y, size.Width - 1, size.Height - 1);
      g.DrawRectangle (Pens.LimeGreen, boxesXY.X + 1, boxesXY.Y + 1, size.Width - 2 - 1, size.Height - 2 - 1);
      foreach (int separation in _separations) {
        Point pointXY = new Point (boxesXY.X + 1, boxesXY.Y + 2 + _margin + separation + _margin);
        g.DrawLine (Pens.LimeGreen, pointXY.X, pointXY.Y, pointXY.X + size.Width - 3, pointXY.Y);
      }
    }

    public static void AddSeparation (Graphics g) {
      _boxHelper.AddSeparation_ (g);
    }

    public void AddSeparation_ (Graphics g) {
      if (_lineSize.IsEmpty)
        throw new Exception ();
      if (_titleWidth == -1) {
        _titleWidth = _lineSize.Width;
      }
      NextLine_ ();
      _separations.Add (_totalSize.Height);
      _totalSize.Height += _margin * 2 + 1;
    }

    public static void AddString (Graphics g, string text) {
      _boxHelper.AddString_ (g, text, Brushes.White);
    }

    public static void AddString (Graphics g, string text, Brush brush) {
      _boxHelper.AddString_ (g, text, brush);
    }

    public static void AddStringLine (Graphics g, string text) {
      _boxHelper.AddString_ (g, text, Brushes.White);
      _boxHelper.NextLine_ ();
    }

    public static void AddStringLine (Graphics g, string text, Brush brush) {
      _boxHelper.AddString_ (g, text, brush);
      _boxHelper.NextLine_ ();
    }

    private void AddString_ (Graphics g, string text, Brush brush) {
      Size size = g.MeasureString2 (text, World._fontBox).ToSizeCeiling ();
      if (!_simulating) {
        int dx = 0;
        if ((_titleWidth != -1) && (_totalSize.Height == 0))
          dx = (_totalSizeOld.Width - _titleWidth) / 2;
        g.DrawStringOutlined (text, World._fontBox, brush, Brushes.Black, LineXY.X + dx, LineXY.Y);
      }
      AddToLine (size);
    }

    public static void AddBitmap (Graphics g, Bitmap bitmap) {
      _boxHelper.AddBitmap_ (g, bitmap);
    }

    private void AddBitmap_ (Graphics g, Bitmap bitmap) {
      if (!_simulating) {
        int dx = 0;
        if ((_titleWidth != -1) && (_totalSize.Height == 0))
          dx = (_totalSizeOld.Width - _titleWidth) / 2;
        g.DrawImage (bitmap, LineXY.X + dx, LineXY.Y);
      }
      AddToLine (bitmap.Size);
    }

    public static void DrawBox (DrawOnBoxDelegate drawOnBox, Graphics g, Point cellXY, Point pixelXY, World world) {
      if (!_simulating)
        return;
      _recursion++;
      //if (_simulating) {
      if (_recursion == 0) {
        _boxesXY = pixelXY;
      }
      if (_recursion > 0) {
        if (_boxesSize.Width < _boxHelper.Size.Width) {
          _boxesSize.Width = _boxHelper.Size.Width;
        }
        _boxesSize.Height += _boxHelper.Size.Height;
        _boxesSize.Height += _spaceBetweenBoxY;
      }
      //}

      BoxHelper boxHelper = new BoxHelper (_boxesSize.Height);

      if (_simulating) {
        _boxHelper = boxHelper;
        drawOnBox (g, cellXY, pixelXY, world);
        if (_simulating) {
          if (_boxesSize.Width < _boxHelper.Size.Width) {
            _boxesSize.Width = _boxHelper.Size.Width;
          }
          _boxesSize.Height += _boxHelper.Size.Height;
          //_boxesSize.Height -= _spaceBetweenBoxY;
          Crop ();
          _simulating = false;
        }
      }

      //if (_recursion == 0) {
      boxHelper.DrawBox (g);
      boxHelper.Rewind ();
      //Reset ();
      //_recursion++;
      _boxHelper = boxHelper;
      drawOnBox (g, cellXY, pixelXY, world);
      //}
      _recursion--;
    }
  }
}
