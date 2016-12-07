using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GodsViewer.Properties;
using System.IO;
using System.Drawing.Imaging;

namespace GodsViewer {
  public partial class Main : Form {
    private bool _internal = false;
    private World _world;
    private static Point _outside = new Point (-1000, -1000);
    public static Point _mouseXY = _outside;
    public static Point _cellXY = _outside;
    private Alfils02_Event _clickEvent;
    //private Alfils02_Event _hoverEvent;
    private Point _clickXY;
    public static Point _clickPixelXY;
    //public static int _viewX, _viewY;
    //public static int _viewW, _viewH;

    public Main () {
      InitializeComponent ();
      _menuViewShowRasterUI.Checked = Settings.Default.ShowRasterEffect;
      _menuViewHighlightUI.Checked = Settings.Default.HighlightWallsStairs;
      _menuViewShowVisualHintsUI.Checked = Settings.Default.ShowVisualHints;
      _menuViewShowEventsUI.Checked = Settings.Default.ShowEvents;
      _menuViewShowHiddenObjectsUI.Checked = Settings.Default.ShowHiddenObjects;

      _menuViewRecursiveEventsUI.Checked = Settings.Default.RecursiveEvents;
      _menuViewShowForwardLinksUI.Checked = Settings.Default.ShowForwardLinks;
      _menuViewShowBackwardLinksUI.Checked = Settings.Default.ShowBackwardLinks;
      _internal = true;
      _worldUI.SelectedIndex = Settings.Default.WorldSelectionIndex;
      _systemUI.SelectedIndex = Settings.Default.SystemSelectionIndex;
      _internal = false;
      LoadWorld (_worldUI.SelectedIndex, _systemUI.Text);
      //LoadWorld (_worldUI.SelectedIndex);
      //_panel2UI.Width = _world._bitmap.Width;
      //_panel2UI.Height = _world._bitmap.Height;
    }

    private void LoadWorld (int index, string system) {
      Cursor.Current = Cursors.WaitCursor;
      int level = (index / 2) + 1;
      int world = (index % 2) + 1;

      //_world = new World (level, world, "ST/godsd2", WorldDrawingInfo);
      //_world = new World (level, world, "Amiga/godsd2", WorldDrawingInfo);
      _world = new World (level, world, Path.Combine (system, "godsd2"), WorldDrawingInfo);

      _panel1UI.AutoScrollMinSize = new Size (_world._bitmap.Width, _world._bitmap.Height);
      RedrawMap ();
      Cursor.Current = Cursors.Arrow;
    }

    private void _panel2UI_Paint (object sender, PaintEventArgs e) {
      e.Graphics.DrawImage (_world._bitmap, 0, 0);

      //_viewX = -_panel1UI.AutoScrollPosition.X;
      //_viewY = -_panel1UI.AutoScrollPosition.Y;
      //_viewW = _panel1UI.ClientSize.Width;
      //_viewH = _panel1UI.ClientSize.Height;
      BoxHelper.Reset (-_panel1UI.AutoScrollPosition.X, -_panel1UI.AutoScrollPosition.Y);
      //BoxHelper._cropXY.X = -_panel1UI.AutoScrollPosition.X;
      //BoxHelper._cropXY.Y = -_panel1UI.AutoScrollPosition.Y;
      //BoxHelper._cropSize = _panel1UI.ClientSize;

      //if (_mouseXY.X > BoxHelper._cropSize.Width / 2)
      //  BoxHelper._deltaXY.X = -10000;
      //else
      //  BoxHelper._deltaXY.X = 10000;
      //if (_mouseXY.Y > BoxHelper._cropSize.Height / 2)
      //  BoxHelper._deltaXY.Y = -10000;
      //else
      //  BoxHelper._deltaXY.Y = 10000;
      //BoxHelper._deltaXY = new Point (-10000, -10000);
      //BoxHelper._deltaXY.X = 0;
      //if ((_mouseXY.Y - BoxHelper._cropXY.Y) > BoxHelper._cropSize.Height / 2)
      //  BoxHelper._deltaXY.Y = -10000;
      //else
      //  BoxHelper._deltaXY.Y = 10000;

      if ((_clickEvent != null) && (_clickEvent.Type == EventTypeEnum.SpawnFlyingWave_00)) {
        Point pixelXY = _mouseXY;
        if ((_clickXY.X * 32 - _mouseXY.X) > 80) {
          pixelXY.X = _clickXY.X * 32 - 80;
        }
        if ((_mouseXY.X - (_clickXY.X * 32 + 32)) > 80) {
          pixelXY.X = _clickXY.X * 32 + 32 + 80;
        }
        if ((_clickXY.Y * 16 - _mouseXY.Y) > 64) {
          pixelXY.Y = _clickXY.Y * 16 - 64;
        }
        if ((_mouseXY.Y - (_clickXY.Y * 16 + 16)) > 64) {
          pixelXY.Y = _clickXY.Y * 16 + 16 + 64;
        }
        _clickPixelXY = pixelXY;
        Alfils01_FlyingWave wave = _world.GetFlyingWave (_clickEvent._param);
        //Point pixelXY3 = new Point (cellXY.X * 32 + 16, cellXY.Y * 16 + 8);
        wave.DrawOnMap (e.Graphics, _clickXY, _world, Point.Empty);
        //_clickEvent.DrawOnMap (e.Graphics, _clickXY, _world, null);
      }
      else {
        _clickPixelXY = Point.Empty;
        World._recurseOnEvent = _menuViewRecursiveEventsUI.Checked;
        _world._drawType = DrawTypeEnum.DrawBitmaps;
        if (_menuViewShowForwardLinksUI.Checked) {
          _world.DrawOnMap (e.Graphics, _cellXY, _mouseXY);
        }
        if (_menuViewShowBackwardLinksUI.Checked) {
          _world.DrawOnMap_Reverse (e.Graphics, _cellXY, _mouseXY);
        }
        _world._drawType = DrawTypeEnum.DrawArrows;
        if (_menuViewShowForwardLinksUI.Checked) {
          _world.DrawOnMap (e.Graphics, _cellXY, _mouseXY);
        }
        if (_menuViewShowBackwardLinksUI.Checked) {
          _world.DrawOnMap_Reverse (e.Graphics, _cellXY, _mouseXY);
        }
        _world.DrawBox (e.Graphics, _cellXY, _mouseXY);
      }
    }

    private void _panel2UI_MouseMove (object sender, MouseEventArgs e) {
      _mouseXY = e.Location;
      _cellXY = new Point (_mouseXY.X / 32, _mouseXY.Y / 16);
      _statusLabelUI.Text = string.Format ("Cell: ({0},{1})  Mouse: ({2},{3})", _cellXY.X, _cellXY.Y, _mouseXY.X, _mouseXY.Y);
      if (e.Button == MouseButtons.None) {
        _clickEvent = null;
      }
      RedrawMap ();
    }

    private void _panel2UI_MouseDown (object sender, MouseEventArgs e) {
      Alfils02_Event event_ = _world.GetMapEventAt (_cellXY);
      if (event_ != null) {
        if ((event_.Type == EventTypeEnum.SpawnFlyingWave_00) ||
            (event_.Type == EventTypeEnum.SpawnWalkingWave_01)) {
          _clickEvent = event_;
          _clickXY = _cellXY;
        }
      }
      World._itemToShow++;
      RedrawMap ();
    }

    private void _panel2UI_MouseLeave (object sender, EventArgs e) {
      _mouseXY = _outside;
      _cellXY = _outside;
      _clickEvent = null;
      RedrawMap ();
    }

    private void _worldUI_SelectedIndexChanged (object sender, EventArgs e) {
      if (_internal)
        return;
      LoadWorld (_worldUI.SelectedIndex, _systemUI.Text);
    }

    private void _systemUI_SelectedIndexChanged (object sender, EventArgs e) {
      if (_internal)
        return;
      LoadWorld (_worldUI.SelectedIndex, _systemUI.Text);
    }

    private void _menuFileExitUI_Click (object sender, EventArgs e) {
      Close ();
    }

    private void _menuViewShowRasterUI_Click (object sender, EventArgs e) {
      RebuildMap ();
    }

    private void _menuViewHighlightUI_Click (object sender, EventArgs e) {
      RebuildMap ();
    }

    private void _menuViewShowVisualHintsUI_Click (object sender, EventArgs e) {
      RebuildMap ();
    }

    private void _menuViewShowEventsUI_Click (object sender, EventArgs e) {
      RebuildMap ();
    }

    private void _menuViewShowHiddenObjectsUI_Click (object sender, EventArgs e) {
      RebuildMap ();
    }

    private void _menuViewRecursiveEventsUI_Click (object sender, EventArgs e) {
      RebuildMap ();
    }

    private void _menuViewShowForwardLinksUI_Click (object sender, EventArgs e) {
      RebuildMap ();
    }

    private void _menuViewShowBackwardLinksUI_Click (object sender, EventArgs e) {
      RedrawMap ();
    }

    private void RebuildMap () {
      Cursor.Current = Cursors.WaitCursor;
      _world.DrawMap (WorldDrawingInfo);
      RedrawMap ();
      Cursor.Current = Cursors.Arrow;
    }

    private void RedrawMap () {
      _panel2UI.Invalidate ();
    }

    private WorldDrawingInfo WorldDrawingInfo {
      get {
        return (new WorldDrawingInfo (_menuViewShowRasterUI.Checked,
                                      _menuViewHighlightUI.Checked,
                                      _menuViewShowVisualHintsUI.Checked,
                                      _menuViewShowEventsUI.Checked,
                                      _menuViewShowHiddenObjectsUI.Checked,

                                      _menuViewRecursiveEventsUI.Checked,
                                      _menuViewShowForwardLinksUI.Checked,
                                      _menuViewShowBackwardLinksUI.Checked));
      }
    }

    private void Main_Load (object sender, EventArgs e) {
      Location = Settings.Default.WindowLocation;
      Size = Settings.Default.WindowSize;
    }

    private void Main_FormClosed (object sender, FormClosedEventArgs e) {
      Settings.Default.WindowLocation = Location;
      Settings.Default.WindowSize = Size;

      Settings.Default.ShowRasterEffect = _menuViewShowRasterUI.Checked;
      Settings.Default.HighlightWallsStairs = _menuViewHighlightUI.Checked;
      Settings.Default.ShowVisualHints = _menuViewShowVisualHintsUI.Checked;
      Settings.Default.ShowEvents = _menuViewShowEventsUI.Checked;
      Settings.Default.ShowHiddenObjects = _menuViewShowHiddenObjectsUI.Checked;

      Settings.Default.RecursiveEvents = _menuViewRecursiveEventsUI.Checked;
      Settings.Default.ShowForwardLinks = _menuViewShowForwardLinksUI.Checked;
      Settings.Default.ShowBackwardLinks = _menuViewShowBackwardLinksUI.Checked;

      Settings.Default.WorldSelectionIndex = _worldUI.SelectedIndex;
      Settings.Default.SystemSelectionIndex = _systemUI.SelectedIndex;
      Settings.Default.Save ();
    }

    private void _menuFileSavePngUI_Click (object sender, EventArgs e) {
      SaveFileDialog sfd = new SaveFileDialog ();
      sfd.DefaultExt = "png";
      sfd.AddExtension = true;
      sfd.CheckPathExists = true;
      sfd.Filter = "PNG files (*.png)|*.png";
      sfd.OverwritePrompt = true;
      string mapFullFilename = _world.GetFilename (string.Format ("World ?* - {0}.png", _world._baseFolder));
      sfd.FileName = mapFullFilename;

      DialogResult dg = sfd.ShowDialog ();
      if (dg == DialogResult.OK) {
        _world._bitmap.Save (sfd.FileName, ImageFormat.Png);
      }
    }
  }
}
