using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace GodsViewer {
  public class World {
    private int _level;
    private int _world;
    private Palette _palette;
    public string _baseFolder;
    private string _baseFolderDisk2;
    private bool _st;

    private int _nbMapSprites;
    private int _mapSpriteIndex_DoorOpened;
    private int _mapSpriteIndex_DoorClosed;
    private int _mapSpriteIndex_TrapdoorOpened;
    private int _mapSpriteIndex_TrapdoorClosed;
    private int _mapSpriteIndex_DoorClosedShadow;
    private int _spriteIndex_WallDestructible;
    private EnemiesInfo _flyingEnemiesInfo;
    private EnemiesInfo _walkingEnemiesInfo;
    private int _startingPixelX;
    private int _startingPixelY;
    public List<Point> _teleportsSpecial;
    public List<ObjectiveLocation> _objectiveLocations;

    private List<SpriteData> _spritesData;
    private int _enemiesSpritesDataIndex;

    public Map _map;

    private ObjectsInfo _objectsInfo;
    private WeaponsInfo _weaponsInfo;
    private FlyingPaths _flyingPaths;

    public Alfils01_FlyingWaves _flyingWaves;
    private Alfils02_Events _events;
    public Alfils03_WalkingWaves _walkingWaves;
    public Alfils04a_IntelFlyingWaves _intelFlyingWaves;
    public Alfils04b_IntelWalkingWaves _intelWalkingWaves;
    private Alfils05_Switches _switches;
    private Alfils06_Teleports _teleports;
    private Alfils07_Trapdoors _trapdoors;
    private Alfils08_MovingBlocks _movingBlocks;
    private Alfils09_Hints _hints;

    public Bitmap _bitmap;
    public Graphics _g;

    public static Font _fontMap;
    public static Font _fontBox;
    public static PrivateFontCollection _pfc;

    public static int _xo = -320 / 2;
    public static int _yo = -192 / 2;

    private const int _objectSpriteIndexBase = 170;
    private const int _weaponSpriteIndexBase = 302;

    public static Brush _brushComputed = new SolidBrush (Color.FromArgb (192, 192, 192));

    public static World _worldInstance;

    public DrawTypeEnum _drawType;

    static World () {
      //_font = new Font (FontFamily.GenericMonospace, 8, FontStyle.Bold);
      _pfc = new PrivateFontCollection ();
      _pfc.AddFontFile ("Fonts/04B_11__.ttf");
      _pfc.AddFontFile ("Fonts/Pic0 1.110.ttf");
      _fontMap = new Font (_pfc.Families [0], 8, FontStyle.Regular, GraphicsUnit.Pixel);
      _fontBox = new Font (_pfc.Families [1], 16, FontStyle.Regular, GraphicsUnit.Pixel);
      //_fontBox = new Font (_pfc.Families [0], 8, FontStyle.Regular, GraphicsUnit.Pixel);
      Pi1.PaletteLookup = Palette.PaletteLookup.ST;

      _fontMap.AddDelta (new Point (0, -1), new Point (-1, 0));
      _fontBox.AddDelta (new Point (0, -1), new Point (0, -1));
    }

    public World (int level, int world, string baseFolderDisk2, WorldDrawingInfo worldDrawingInfo) {
      _worldInstance = this;

      _level = level;
      _world = world;
      _baseFolderDisk2 = baseFolderDisk2;
      _baseFolder = _baseFolderDisk2.Split (Path.DirectorySeparatorChar) [0];
      if (_baseFolder == "Atari")
        _st = true;
      else
        _st = false;


      // LOAD

      // init hardcoded
      int offset1, offset2, offset4, offset5, offset6;
      byte [] dump;
      if (_st) {
        dump = Resource._04_Level_1_dmp;
        offset1 = 0x18CFC;
        offset2 = 0x2F0E + (_level - 1) * 12;
        offset4 = 0x2E0A;
        offset5 = 0x10FAA;
        offset6 = 0xAC22;
      }
      else {
        dump = Resource.Amiga_Level_1;
        offset1 = 0x17A6E;
        offset2 = 0x29DA + (_level - 1) * 12;
        offset4 = 0x28DA;
        offset5 = 0x1078A;
        offset6 = 0xA770;
      }
      // offset1
      _nbMapSprites = dump.ReadWord (offset1 + (_level - 1) * 4 + (_world - 1) * 2);
      // offset2
      _mapSpriteIndex_DoorOpened = dump.ReadWord (offset2 + 0);
      _mapSpriteIndex_DoorClosed = dump.ReadWord (offset2 + 2);
      _mapSpriteIndex_TrapdoorOpened = dump.ReadWord (offset2 + 4);
      _mapSpriteIndex_TrapdoorClosed = dump.ReadWord (offset2 + 6);
      _mapSpriteIndex_DoorClosedShadow = dump.ReadWord (offset2 + 8);
      _spriteIndex_WallDestructible = dump.ReadWord (offset2 + 10);
      // offset3: use the same data for ST & Amiga for enemies info
      int offset3 = 0x1DAE0 + 8 * (_level - 1);
      offset3 = (int) Resource._04_Level_1_dmp.ReadLong (offset3);
      _flyingEnemiesInfo = new EnemiesInfo (Resource._04_Level_1_dmp, offset3, "FLYING");
      offset3 = 0x1DAE0 + 8 * (_level - 1) + 4;
      offset3 = (int) Resource._04_Level_1_dmp.ReadLong (offset3);
      _walkingEnemiesInfo = new EnemiesInfo (Resource._04_Level_1_dmp, offset3, "WALKING");
      // offset4: starting position
      _startingPixelX = dump.ReadWord (offset4 + (_level - 1) * 4 + 0);
      _startingPixelY = dump.ReadWord (offset4 + (_level - 1) * 4 + 2);
      // offset5: hardcoded teleport
      _teleportsSpecial = new List<Point> ();
      int nb = 0;
      if (_level == 1) {
        nb = 8;
      }
      if (_level == 2) {
        nb = 5;
        offset5 += 8 * 2;
      }
      if (nb != 0) {
        for (int i = 0; i < nb; i++) {
          _teleportsSpecial.Add (DecodeTeleport (dump.ReadWord (offset5)));
          offset5 += 2;
          //Console.WriteLine (string.Format ("X: {0} Y: {1}", _teleportsSpecial [i].X, _teleportsSpecial [i].Y));
        }
        // patch 1 hardcoded teleport: 32/46
        if (_level == 2) {
          _teleportsSpecial.Add (DecodeTeleport (0x202E));
        }
      }
      // objective locations
      _objectiveLocations = new List<ObjectiveLocation> ();
      offset6 = offset6 + (_level - 1) * 4;
      offset6 = (int) dump.ReadLong (offset6);
      for (; ; ) {
        ObjectiveLocation objectiveLocation = new ObjectiveLocation (dump, ref offset6);
        if (objectiveLocation._pixelXY.IsEmpty)
          break;
        _objectiveLocations.Add (objectiveLocation);
      }
      //}
      //else {
      //  _nbMapSprites = 120;
      //  _a = _b = _mapSpriteIndex_TrapdoorOpened = _mapSpriteIndex_TrapdoorClosed = _c = _d = -1;
      //}

      // init palette
      using (Pi1 pi1 = new Pi1 (GetFullFilename ("level?b.pi1"), null, Sprite.MasksEnum.Transparent)) {
        _palette = pi1._palette;
      }

      // init data
      _map = new Map (GetFullFilename ("lev?*.map"), GetFullFilename ("bits?*.pi1"), GetFullFilename ("xtra?*.pi1"), _palette, _nbMapSprites);
      _objectsInfo = new ObjectsInfo (GetFullFilename ("objects.00?"));
      _weaponsInfo = new WeaponsInfo (GetFullFilename ("weapons.00?"));
      _flyingPaths = new FlyingPaths (GetFullFilename ("god0?.pat"));

      // init sprites
      _spritesData = new List<SpriteData> ();
      SpritesData spritesDataAlways1 = new SpritesData (GetFullFilename ("always1"), _palette);
      SpritesData spritesDataAlways2 = new SpritesData (GetFullFilename ("always2"), _palette);
      SpritesData spritesDataAlways3 = new SpritesData (GetFullFilename ("always3"), _palette);
      SpritesData spritesDataGodfont = new SpritesData (GetFullFilename ("godfont"), _palette);
      SpritesData spritesDataObj1 = new SpritesData (GetFullFilename ("obj1"), _palette);
      SpritesData spritesDataObj2 = new SpritesData (GetFullFilename ("obj2"), _palette);
      _spritesData.AddRange (spritesDataAlways1._spritesData);
      _spritesData.AddRange (spritesDataAlways2._spritesData);
      _spritesData.AddRange (spritesDataAlways3._spritesData);
      _spritesData.AddRange (spritesDataGodfont._spritesData);
      _spritesData.AddRange (spritesDataObj1._spritesData);
      _spritesData.AddRange (spritesDataObj2._spritesData);
      _enemiesSpritesDataIndex = _spritesData.Count;
      SpritesData spritesDatalevelXa = new SpritesData (GetFullFilename ("level?a"), _palette);
      SpritesData spritesDatalevelXb = new SpritesData (GetFullFilename ("level?b"), _palette);
      _spritesData.AddRange (spritesDatalevelXa._spritesData);
      _spritesData.AddRange (spritesDatalevelXb._spritesData);

      // alfils_0YX
      byte [] buffer_alfils_0YX = ExtensionByte.CreateFromFile (GetFullFilename ("alfils.0*?"));
      int offset = 0;
      _flyingWaves = new Alfils01_FlyingWaves (buffer_alfils_0YX, ref offset);
      _events = new Alfils02_Events (buffer_alfils_0YX, ref offset);
      _walkingWaves = new Alfils03_WalkingWaves (buffer_alfils_0YX, ref offset);
      _intelFlyingWaves = new Alfils04a_IntelFlyingWaves (buffer_alfils_0YX, ref offset);
      _intelWalkingWaves = new Alfils04b_IntelWalkingWaves (buffer_alfils_0YX, ref offset);
      _switches = new Alfils05_Switches (buffer_alfils_0YX, ref offset);
      _teleports = new Alfils06_Teleports (buffer_alfils_0YX, ref offset);
      _trapdoors = new Alfils07_Trapdoors (buffer_alfils_0YX, ref offset);
      _movingBlocks = new Alfils08_MovingBlocks (buffer_alfils_0YX, ref offset);
      _hints = new Alfils09_Hints (buffer_alfils_0YX, ref offset);
      // 80 unused bytes?
      for (int i = 0; i < 80; i++) {
        if (buffer_alfils_0YX.ReadByte (offset++) != 0)
          throw new Exception ();
      }
      if (offset != buffer_alfils_0YX.Length)
        throw new Exception ();
      //_events.CheckEventsTriggered (this);

      CheckPuzzleUsedInMultipleEvents ();
      //RemovePuzzleUsedIn0Events ();
      RemoveOrphanEvents ();
      RemovePuzzleUsedIn0Events ();
      CheckOrphanEvents ();
      CheckEventTriggeredFromMapXorEffect ();
      RemoveUnusedMovingBlocks ();
      MarkUnusedMovingBlockActions ();

      // DRAW
      DrawMap (worldDrawingInfo);
      //string mapFullFilename = GetFilename (string.Format ("World ?* {0}.png", _baseFolder));
      //_bitmap.Save (mapFullFilename, ImageFormat.Png);

      // write common sprites
      string folderSprites = Path.Combine (_baseFolder, "sprites");
      if (!Directory.Exists (folderSprites)) {
        Directory.CreateDirectory (folderSprites);
        for (int i = 0; i < _enemiesSpritesDataIndex; i++) {
          SpriteData spriteData = _spritesData [i];
          string filename = string.Format ("spr_{0}.png", i.ToString ().PadLeft (3, '0'));
          spriteData._bitmap.Save (Path.Combine (folderSprites, filename), ImageFormat.Png);
          //Console.WriteLine (string.Format ("{3,-3} {2:X6} <{0}, {1}, ?>", -(spriteData._width - 1), spriteData._height - 1, 0x685E2 + i * 8, i));
        }
      }

      // write enemy sprites
      string folderEnemies = Path.Combine (folderSprites, string.Format ("enemies{0}", GetFilename ("?")));
      if (!Directory.Exists (folderEnemies)) {
        Directory.CreateDirectory (folderEnemies);
        for (int i = _enemiesSpritesDataIndex; i < _spritesData.Count; i++) {
          SpriteData spriteData = _spritesData [i];
          string filename = string.Format ("spr_{0}.png", i.ToString ().PadLeft (3, '0'));
          spriteData._bitmap.Save (Path.Combine (folderEnemies, filename), ImageFormat.Png);
          //Console.WriteLine (string.Format ("{3,-3} {2:X6} <{0}, {1}, ?>", -(spriteData._width - 1), spriteData._height - 1, 0x685E2 + i * 8, i));
        }
      }

      // write map sprites
      string folderMap = Path.Combine (folderSprites, string.Format ("map{0}", GetFilename ("?*")));
      if (!Directory.Exists (folderMap)) {
        Directory.CreateDirectory (folderMap);
        for (int i = 0; i < _map._mapSprites.Count; i++) {
          Bitmap bitmap = _map._mapSprites [i];
          string filename = string.Format ("spr_{0}.png", (i + 1).ToString ().PadLeft (3, '0'));
          bitmap.Save (Path.Combine (folderMap, filename), ImageFormat.Png);
          //Console.WriteLine (string.Format ("{3,-3} {2:X6} <{0}, {1}, ?>", -(spriteData._width - 1), spriteData._height - 1, 0x685E2 + i * 8, i));
        }
      }

      // write alfils files
      string folderAlfils = Path.Combine (_baseFolder, "alfils");
      if (!Directory.Exists (folderAlfils)) {
        Directory.CreateDirectory (folderAlfils);
      }
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _flyingWaves);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _events);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _walkingWaves);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _intelFlyingWaves);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _intelWalkingWaves);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _switches);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _teleports);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _trapdoors);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _movingBlocks);
      SaveAlfil (buffer_alfils_0YX, folderAlfils, _hints);

      //foreach (MapObject mapObject in _map._objects) {
      //  int index = mapObject._objectTemplateIndex;
      //  if (index < _objectTemplates._objectTemplates.Count) {
      //    Console.WriteLine (_objectTemplates._objectTemplates [index].String);
      //  }
      //  else {
      //    Console.WriteLine (index);
      //  }
      //}

      //foreach (Weapon weapon in _weapons._weapons) {
      //  Console.WriteLine (weapon.String);
      //}

      // write object sprites
      string folderObjects = Path.Combine (_baseFolder, "objects");
      if (!Directory.Exists (folderObjects)) {
        Directory.CreateDirectory (folderObjects);
        foreach (ObjectInfo objectInfo in _objectsInfo._objectsInfo) {
          Console.WriteLine (objectInfo.String);
          SpriteData spriteData = _spritesData [objectInfo._spriteIndex];
          string filename = string.Format ("{0} {1}", objectInfo._spriteIndex.ToString ().PadLeft (3, '0'), (objectInfo._spriteIndex - 170).ToString ().PadLeft (3, '0'));
          string fullName = objectInfo.FullName;
          if (fullName != null) {
            filename += string.Format (" {0}", fullName);
          }
          spriteData._bitmap.Save (Path.Combine (folderObjects, Path.ChangeExtension (filename, "png")), ImageFormat.Png);
        }
      }

      // look for follow the leader waves (only boss?)
      //foreach (Alfils04a_IntelFlyingWave wave in _intelFlyingWaves._intelFlyingWaves) {
      //  if (wave == null)
      //    continue;
      //  if ((wave._primaryObjective == IntelFlyingObjectiveEnum.FollowLeader_4) ||
      //      (wave._secondaryObjective == IntelFlyingObjectiveEnum.FollowLeader_4)) {
      //  }
      //  else if ((wave._primaryObjective == IntelFlyingObjectiveEnum.UpdateLeader_3) ||
      //           (wave._secondaryObjective == IntelFlyingObjectiveEnum.UpdateLeader_3)) {
      //  }
      //  else {
      //    Console.WriteLine (string.Format ("{0} {1}: {2}", wave._primaryObjective, wave._secondaryObjective, wave._index));
      //    continue;
      //  }
      //  List<Alfils02_Event> events = GetEventsWithEffect (EventTypeEnum.SpawnIntelFlyingWave_04, wave._index);
      //  if (events == null)
      //    continue;
      //  foreach (Alfils02_Event event_ in _events._events) {
      //    if (event_ == null)
      //      continue;
      //    List<Point> cells = event_.GetCells (this);
      //    foreach (Point cell in cells) {
      //      Console.WriteLine (string.Format ("{0} {1}: {2}", wave._primaryObjective, wave._secondaryObjective, cell));
      //    }
      //  }
      //}
    }

    private void SaveAlfil (byte [] buffer_alfils_0YX, string folderAlfils, IAlfil alfil) {
      byte [] buffer_alfil;
      buffer_alfil = buffer_alfils_0YX.Extract (alfil.Offset, alfil.Length);
      string fullFilename = Path.Combine (folderAlfils, GetFilename (string.Format ("{0}.0*?", alfil.Name)));
      if (File.Exists (fullFilename))
        return;
      buffer_alfil.WriteToFile (fullFilename);
    }

    public void CheckOrphanEvents () {
      foreach (Alfils02_Event event_ in _events._events) {
        if (event_ == null)
          continue;
        List<Point> cellsXY = event_.GetCells (this);
        if (cellsXY.Count > 0)
          continue;
        throw new Exception ();
      }
    }

    public void RemoveOrphanEvents () {
      List<int> orphans = new List<int> ();
      for (int i = 0; i < _events._events.Count; i++) {
        Alfils02_Event event_ = _events._events [i];
        if (event_ == null)
          continue;
        List<Point> cellsXY = event_.GetCells (this);
        if (cellsXY.Count > 0)
          continue;
        orphans.Add (i);
      }
      foreach (int i in orphans) {
        _events._events [i] = null;
      }
    }

    public void CheckPuzzleUsedInMultipleEvents () {
      foreach (MapPuzzle puzzle in _map._mapPuzzles) {
        if (puzzle == null)
          continue;
        List<Alfils02_Event> events = GetEventsWithEffect (EventTypeEnum.CheckPuzzle_02, puzzle._index);
        if (events.Count > 1)
          throw new Exception ();
      }
    }

    public void RemovePuzzleUsedIn0Events () {
      for (int i = 0; i < _map._mapPuzzles.Count; i++) {
        MapPuzzle puzzle = _map._mapPuzzles [i];
        if (puzzle == null)
          continue;
        List<Alfils02_Event> events = GetEventsWithEffect (EventTypeEnum.CheckPuzzle_02, puzzle._index);
        if (events.Count > 0)
          continue;
        _map._mapPuzzles [i] = null;
      }
    }

    public void CheckEventTriggeredFromMapXorEffect () {
      List<Alfils02_Event> eventsTriggeredFromMap = new List<Alfils02_Event> ();
      foreach (Alfils02_Event event_ in _events._events) {
        if (event_ == null)
          continue;
        if (event_.GetMapCells (this).Count == 0)
          continue;
        eventsTriggeredFromMap.Add (event_);
      }

      List<Alfils02_Event> eventsTriggeredFromEffect = new List<Alfils02_Event> ();
      foreach (Alfils02_Event event_ in _events._events) {
        if (event_ == null)
          continue;
        if (GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.TriggerEvent_05, event_._index + 1).Count == 0)
          continue;
        eventsTriggeredFromEffect.Add (event_);
      }

      foreach (Alfils02_Event event_ in _events._events) {
        if (event_ == null)
          continue;
        if (eventsTriggeredFromMap.Contains (event_) && eventsTriggeredFromEffect.Contains (event_)) {
          //throw new Exception ();
          //Console.WriteLine ("Event #{0}: ", event_._index);
          //foreach (Point cellXY in event_.GetCells (this))
          //  Console.Write ("({0},{1}) ", cellXY.X, cellXY.Y);
          //Console.WriteLine ();
        }
        if (!eventsTriggeredFromMap.Contains (event_) && !eventsTriggeredFromEffect.Contains (event_))
          throw new Exception ();
      }
    }

    public void RemoveUnusedMovingBlocks () {
      //foreach (Alfils08_MovingBlock movingBlock in _movingBlocks._movingBlocks) {
      for (int i = 0; i < _movingBlocks._movingBlocks.Count; i++) {
        Alfils08_MovingBlock movingBlock = _movingBlocks._movingBlocks [i];
        if (movingBlock == null)
          continue;
        if ((GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction0_06, movingBlock._index).Count != 0) ||
            (GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction1_07, movingBlock._index).Count != 0) ||
            (GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction2_08, movingBlock._index).Count != 0) ||
            (GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction3_09, movingBlock._index).Count != 0))
          continue;
        _movingBlocks._movingBlocks [i] = null;
      }
    }

    public void MarkUnusedMovingBlockActions () {
      foreach (Alfils08_MovingBlock movingBlock in _movingBlocks._movingBlocks) {
        if (movingBlock == null)
          continue;
        if (GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction0_06, movingBlock._index).Count == 0)
          movingBlock._actions [0] = 0xFE;
        if (GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction1_07, movingBlock._index).Count == 0)
          movingBlock._actions [1] = 0xFE;
        if (GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction2_08, movingBlock._index).Count == 0)
          movingBlock._actions [2] = 0xFE;
        if (GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction3_09, movingBlock._index).Count == 0)
          movingBlock._actions [3] = 0xFE;
      }
    }

    public string GetFullFilename (string s) {
      return (Path.Combine (_baseFolderDisk2, GetFilename (s)));
    }

    public string GetFilename (string s) {
      return (s.Replace ('?', (char) ('0' + _level)).Replace ('*', (char) ('a' + _world - 1)));
    }

    //*****************************************************************************************************************
    public void DrawMapSprite (int pixelX, int pixelY, int index) {
      if (index > _map._mapSprites.Count) {
        _g.FillRectangle (Brushes.Red, pixelX, pixelY, 32, 16);
        _g.DrawStringOutlinedCentered (index.ToString (), _fontMap, Brushes.Black, Brushes.Red, pixelX + 16, pixelY + 8);
        //g.DrawImage (_mapSprites2 [index - 89], x * 32, y * 16);
      }
      else if (index == 0) {
        _g.FillRectangle (Brushes.Orange, pixelX, pixelY, 32, 16);
      }
      else {
        _g.DrawImage (_map._mapSprites [index - 1], pixelX, pixelY);
      }
    }

    public void DrawMapObjectSprite (int pixelX, int pixelY, int objectOrWeaponInfoIndex) {
      int spriteIndex = -1;
      if (objectOrWeaponInfoIndex < 192) {
        //ObjectInfo objectInfo = _objectsInfo._objectsInfo [mapObject._objectOrWeaponInfoIndex];
        //if (objectInfo._type > 3) {
        //  throw new Exception ();
        //}
        spriteIndex = objectOrWeaponInfoIndex + World._objectSpriteIndexBase;
      }
      else {
        spriteIndex = objectOrWeaponInfoIndex - 192 + World._weaponSpriteIndexBase;
      }
      _g.DrawImage (_spritesData [spriteIndex]._bitmap, pixelX, pixelY);
    }

    public void DrawMap (WorldDrawingInfo worldDrawingInfo) {
      _bitmap = new Bitmap (128 * 32, 64 * 16, PixelFormat.Format32bppPArgb);
      _g = Graphics.FromImage (_bitmap);
      _g.Clear (Color.Black);
      if (worldDrawingInfo._showRaster) {
        _g.Clear (_map._rasterPalette.GetColor (_map._rasterNbColors - 1));
        int rasterHeight = 20;
        for (int j = 0; j < (_map._rasterPalette.Size - 1); j++) {
          Color color1 = _map._rasterPalette.GetColor (j);
          Color color2 = _map._rasterPalette.GetColor (j + 1);
          using (Brush brush = new LinearGradientBrush (new Rectangle (0, 0, 1, rasterHeight), color1, color2, LinearGradientMode.Vertical))
            _g.FillRectangle (brush, 0, j * rasterHeight, _bitmap.Width, rasterHeight);
          //_g.FillRectangle (new SolidBrush (color), 0, j * rasterHeight, _bitmap.Width, rasterHeight);
        }
      }

      Brush brushImpassable = new SolidBrush (Color.FromArgb (128, 255, 255, 255));
      Brush brushStairs = new SolidBrush (Color.FromArgb (128, 255, 128, 0));

      Brush brushSwitch = new SolidBrush (Color.FromArgb (128, 0, 128, 255));
      Brush brushTrapdoor = new SolidBrush (Color.FromArgb (128, 255, 255, 0));
      Brush brushMovingBlock = new SolidBrush (Color.FromArgb (128, 0, 255, 255));

      Brush brushUnk = new SolidBrush (Color.FromArgb (128, 255, 0, 255));

      Pen penBackdoorTeleport = new Pen (Color.Yellow);
      penBackdoorTeleport.DashStyle = DashStyle.Custom;
      penBackdoorTeleport.DashPattern = new float [] { 3.0f, 4.0f };
      Pen penStoneTeleport = new Pen (Color.Cyan);
      penStoneTeleport.DashStyle = DashStyle.Custom;
      penStoneTeleport.DashPattern = new float [] { 3.0f, 4.0f };
      Pen penSpecialTeleport = new Pen (Color.Red);
      penSpecialTeleport.DashStyle = DashStyle.Custom;
      penSpecialTeleport.DashPattern = new float [] { 3.0f, 4.0f };
      Pen penExplosion = new Pen (Color.Orange);
      penExplosion.DashStyle = DashStyle.Custom;
      penExplosion.DashPattern = new float [] { 3.0f, 4.0f };
      Pen penMovingBlock = new Pen (Color.White);
      penMovingBlock.DashStyle = DashStyle.Custom;
      penMovingBlock.DashPattern = new float [] { 3.0f, 4.0f };
      Pen penObjectiveLocation = new Pen (Color.White);
      penObjectiveLocation.DashStyle = DashStyle.Custom;
      penObjectiveLocation.DashPattern = new float [] { 3.0f, 4.0f };
      //penObjectiveLocation.Width = 3;


      // map
      for (int y = 0; y < 64; y++) {
        for (int x = 0; x < 128; x++) {
          int pixelX = x * 32;
          int pixelY = y * 16;
        }
      }

      for (int y = 0; y < 64; y++) {
        for (int x = 0; x < 128; x++) {
          int pixelX = x * 32;
          int pixelY = y * 16;
          int indexA = _map._layerA [x, y];
          DrawMapSprite (pixelX, pixelY, indexA);
          if (worldDrawingInfo._highlightWallsStairs) {
            int indexB = _map._layerB [x, y];
            if (indexB == 0x00) { // empty
            }
            else if (indexB == 0x01) { // impassable
              if ((indexA == 21) && ((_level == 3) || (_level == 4))) // destructable wall
                _g.DrawRectangle (Pens.White, pixelX, pixelY, 32 - 1, 16 - 1);
              else // wall
                _g.FillRectangle (brushImpassable, pixelX, pixelY, 32, 16);
            }
            else if (indexB == 0x02) // stairs
              _g.FillRectangle (brushStairs, pixelX, pixelY, 32, 16);
          }
        }
      }

      // map objects
      foreach (MapItem mapItem in _map._mapItems) {
        if (mapItem == null) {
          continue;
        }
        DrawMapObjectSprite (mapItem.ItemPixelXY.X, mapItem.ItemPixelXY.Y, mapItem._objectOrWeaponInfoIndex);
      }

      // objects & weapons spawned by events
      if (worldDrawingInfo._showHiddenObjects) {
        foreach (Alfils02_Event event_ in _events._events) {
          if (event_ == null) {
            continue;
          }
          if (event_.Type == EventTypeEnum.CheckPuzzle_02) {
            MapPuzzle puzzle = _map._mapPuzzles [event_._param];
            if (puzzle._effectType == PuzzleEffectTypeEnum.SpawnObject_00) {
              Bitmap bitmap = _spritesData [World._objectSpriteIndexBase + puzzle._effectParam]._bitmap;
              _g.DrawImage (bitmap, puzzle.ItemPixelXY.X, puzzle.ItemPixelXY.Y, 0.5f);
            }
            else if (puzzle._effectType == PuzzleEffectTypeEnum.SpawnWeapon_01) {
              Bitmap bitmap = _spritesData [World._weaponSpriteIndexBase + puzzle._effectParam]._bitmap;
              _g.DrawImage (bitmap, puzzle.ItemPixelXY.X, puzzle.ItemPixelXY.Y, 0.5f);
            }
          }
        }
      }


      // switches
      //foreach (Switch switch_ in _switches._switches) {
      //  if (switch_ == null) {
      //    continue;
      //  }
      //  if (_st) {
      //    int spriteIndex = switch_._spriteIndex_min170 + 170;
      //    DrawSprite (switch_._pixelX, switch_._pixelY, spriteIndex);
      //  }
      //  _g.DrawRectangle (Pens.Blue, switch_._pixelX, switch_._pixelY, 16 - 1, 16 - 1);
      //}

      // trapdoors
      //foreach (Alfils07_Trapdoor trapdoor in _trapdoors._trapdoors) {
      //  if (trapdoor == null) {
      //    continue;
      //  }
      //  if (_st) {
      //    DrawMapSprite (trapdoor._pixelX, trapdoor._pixelY, trapdoor._isOpened ? _mapSpriteIndex_TrapdoorOpened : _mapSpriteIndex_TrapdoorClosed);
      //  }
      //  if (trapdoor._isOpened) {
      //    _g.DrawRectangle (Pens.Yellow, trapdoor._pixelX, trapdoor._pixelY, 32 - 1, 16 - 1);
      //  }
      //  else {
      //    _g.FillRectangle (brushTrapdoor, trapdoor._pixelX, trapdoor._pixelY, 32, 16);
      //  }
      //}

      // moving blocks
      foreach (Alfils08_MovingBlock movingBlock in _movingBlocks._movingBlocks) {
        if (movingBlock == null) {
          continue;
        }
        movingBlock.Draw (_g, movingBlock.PixelXY, this);
        if (worldDrawingInfo._highlightWallsStairs) {
          _g.FillRectangle (brushMovingBlock, movingBlock.PixelXY.X, movingBlock.PixelXY.Y, (movingBlock._nbBlocksX_min1 + 1) * 32, (movingBlock._nbBlocksY_min1 + 1) * 16);
        }
        if (worldDrawingInfo._showVisualHints) {
          _g.DrawRectangle (penMovingBlock, movingBlock.PixelXY.X, movingBlock.PixelXY.Y, (movingBlock._nbBlocksX_min1 + 1) * 32 - 1, (movingBlock._nbBlocksY_min1 + 1) * 16 - 1);
          //_g.DrawStringOutlinedCentered (movingBlock._index.ToString (), _fontMap, Brushes.Cyan, Brushes.Black, movingBlock.PixelXY.X + 16, movingBlock.PixelXY.Y + 8);
        }
      }

      // doors
      //for (int y = 0; y < 64; y++) {
      //  for (int x = 0; x < 128; x++) {
      //    if ((_map._layerA [x, y] == _a) || (_map._layerA [x, y] == _b)) {
      //      int pixelX = x * 32;
      //      int pixelY = y * 16;
      //      DrawMapSprite (pixelX, pixelY, _b);
      //      DrawMapSprite (pixelX, pixelY + 16, _b + 10);
      //      DrawMapSprite (pixelX, pixelY + 32, _b + 20);
      //      DrawMapSprite (pixelX + 32, pixelY, _c);
      //      DrawMapSprite (pixelX + 32, pixelY + 16, _c);
      //      DrawMapSprite (pixelX + 32, pixelY + 32, _c);
      //      _g.FillRectangle (brushUnk, pixelX, pixelY, 32 * 2, 16 * 3);
      //    }
      //  }
      //}

      // player
      if (_world == 1)
        _g.DrawImage (_spritesData [9]._bitmap, _startingPixelX, _startingPixelY);

      // events
      if (worldDrawingInfo._showEvents) {
        for (int y = 0; y < 64; y++) {
          for (int x = 0; x < 128; x++) {
            int pixelX = x * 32;
            int pixelY = y * 16;
            int indexB = _map._layerB [x, y];
            if (indexB >= 0x03) {
              //_g.DrawStringOutlinedCentered ((indexB - 3).ToString (), _font, Brushes.White, Brushes.Black, pixelX + 16, pixelY + 8);
              Alfils02_Event event_ = _events._events [indexB - 3];
              if (event_ == null) {
                _g.DrawStringOutlinedCentered ((indexB - 3).ToString (), _fontMap, Brushes.Red, Brushes.Black, pixelX + 16, pixelY + 8 + 1);
              }
              else {
                if (event_._functionIndex_min1 == -1) { // inactive
                  _g.DrawStringOutlinedCentered (event_._param.ToString (), _fontMap, Brushes.Orange, Brushes.Black, pixelX + 16, pixelY + 12 + 1);
                }
                else {
                  int eventIndex = event_._functionIndex_min1 - 1;
                  if (eventIndex > 10) {
                    throw new Exception ();
                  }
                  //_g.DrawStringOutlinedCentered (event_.Acronym, _fontMap, Brushes.Yellow, Brushes.Black, pixelX + 16, pixelY + 4);
                  //_g.DrawStringOutlinedCentered (event_._param.ToString (), _fontMap, Brushes.White, Brushes.Black, pixelX + 16, pixelY + 12);

                  _g.DrawStringOutlinedCentered (string.Format ("#{0}", event_._index), _fontMap, Brushes.Yellow, Brushes.Black, pixelX + 16, pixelY + 4 + 1);
                  _g.DrawStringOutlinedCentered (string.Format ("{0}:{1}", event_.Acronym, event_._param), _fontMap, Brushes.White, Brushes.Black, pixelX + 16, pixelY + 12 + 1);

                  // flying paths
                  //if (eventIndex == 0) {
                  //  //if ((event_._param != 1) && (event_._param != 0) && (event_._param != 3)) {
                  //  if (event_._param != -1) {
                  //    continue;
                  //  }
                  //  Alfils01_FlyingWave flyingWave = _flyingWaves._flyingWaves [event_._param];
                  //  flyingWave.Draw (_g, pixelX + 16, pixelY + 8, this);
                  //}
                  //if (eventIndex == 1) {
                  //  //if ((event_._param != 1) && (event_._param != 0) && (event_._param != 3)) {
                  //  //if (event_._param != 5) {
                  //  //  continue;
                  //  //}
                  //  Alfils03_WalkingWave walkingWave = _walkingWaves._walkingWaves [event_._param];
                  //  walkingWave.Draw (_g, this);
                  //}
                }
              }
            }
          }
        }
      }

      // teleport stone special
      if (worldDrawingInfo._showVisualHints) {
        int i = 0;
        Dictionary<Point, string> link = new Dictionary<Point, string> ();
        foreach (Point pixelXY in _teleportsSpecial) {
          if (link.ContainsKey (pixelXY)) {
            link [pixelXY] += string.Format (",{0}", i);
          }
          else {
            link.Add (pixelXY, i.ToString ());
          }
          i++;
        }
        foreach (KeyValuePair<Point, string> kvp in link) {
          Point pixelXY = kvp.Key;
          string text = kvp.Value;
          _g.DrawRectangle (penSpecialTeleport, pixelXY.X, pixelXY.Y, Alfils06_Teleport.Size.Width - 1, Alfils06_Teleport.Size.Height - 1);
          _g.DrawStringOutlinedCentered (text, World._fontMap, Brushes.Red, Brushes.Black, pixelXY.X + Alfils06_Teleport.Size.Width / 2, pixelXY.Y + Alfils06_Teleport.Size.Height / 2);
        }
      }

      if (worldDrawingInfo._showVisualHints) {
        // teleport destination (door + stone)
        foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
          MapPuzzle puzzle = event_.GetPuzzle (this);
          _g.DrawRectangle (penBackdoorTeleport, puzzle.BackdoorTeleport_PixelXY.X, puzzle.BackdoorTeleport_PixelXY.Y, puzzle.Backdoor_Size.Width - 1, puzzle.Backdoor_Size.Height - 1);
        }
        foreach (Alfils06_Teleport teleport in _teleports._teleports) {
          if (teleport == null)
            continue;
          _g.DrawRectangle (penStoneTeleport, teleport.PixelXY.X, teleport.PixelXY.Y, Alfils06_Teleport.Size.Width - 1, Alfils06_Teleport.Size.Height - 1);
        }

        // explosion
        foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.DestroyType4_06, -1)) {
          MapPuzzle puzzle = event_.GetPuzzle (this);
          _g.DrawRectangle (penExplosion, puzzle.Explosion_PixelXY.X, puzzle.Explosion_PixelXY.Y, puzzle.Explosion_Size.Width - 1, puzzle.Explosion_Size.Height - 1);
        }

        // objective locations
        foreach (ObjectiveLocation objectiveLocation in _objectiveLocations) {
          _g.DrawRectangle (penObjectiveLocation, objectiveLocation.PixelXY.X, objectiveLocation.PixelXY.Y, ObjectiveLocation.Size.Width - 1, ObjectiveLocation.Size.Height - 1);
        }
      }

      brushImpassable.Dispose ();
      brushStairs.Dispose ();

      brushSwitch.Dispose ();
      brushTrapdoor.Dispose ();
      brushMovingBlock.Dispose ();

      brushUnk.Dispose ();

    }
    //*****************************************************************************************************************
    public static int _itemToShow = 0;
    public static bool _recurseOnEvent = true;
    public static Dictionary<Alfils02_Event, object> _eventsVisited = new Dictionary<Alfils02_Event, object> ();
    public void DrawOnMap (Graphics g, Point cellXY, Point pixelXY) {
      _eventsVisited.Clear ();
      int i = 0;
      // map objects
      foreach (MapItem mapObject in GetMapObjectsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          mapObject.DrawOnMap (g, Point.Empty, this, Point.Empty);
          return;
        }
      }

      // map weapons
      foreach (MapItem mapWeapon in GetMapWeaponsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          mapWeapon.DrawOnMap (g, Point.Empty, this, Point.Empty);
          return;
        }
      }

      // spawned objects
      foreach (MapPuzzle puzzle in GetSpawnedObjectsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          puzzle.SpawnedItem_DrawOnMap (g, Point.Empty, this, Point.Empty);
          return;
        }
      }

      // spawned weapons
      foreach (MapPuzzle puzzle in GetSpawnedWeaponsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          puzzle.SpawnedItem_DrawOnMap (g, Point.Empty, this, Point.Empty);
          return;
        }
      }

      // event
      {
        Alfils02_Event event_ = GetMapEventAt (cellXY);
        if (event_ != null) {
          if (i++ == _itemToShow) {
            event_.DrawOnMap (g, cellXY, this, null, false);
            return;
          }
        }
      }

      // trapdoor
      Alfils07_Trapdoor trapdoor = GetTrapdoorAtCell (cellXY);
      if (trapdoor != null) {
        if (i++ == _itemToShow) {
          trapdoor.DrawOnMap (g, this, Point.Empty);
          return;
        }
      }

      // door
      bool found = false;
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenDoor_02, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Door_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Door_DrawOnMap (g, this, Point.Empty);
            found = true;
          }
        }
      }
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.CloseDoor_09, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Door_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Door_DrawOnMap (g, this, Point.Empty);
            found = true;
          }
        }
      }
      if (found)
        return;

      // backdoor
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Backdoor_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Backdoor_DrawOnMap (g, this, Point.Empty);
            return;
          }
        }
      }
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Backdoor_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Backdoor_DrawOnMap (g, this, Point.Empty);
            return;
          }
        }
      }

      // backdoor teleport
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.BackdoorTeleport_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.BackdoorTeleport_DrawOnMap (g, this, Point.Empty);
            return;
          }
        }
      }

      // stone teleport
      foreach (Alfils06_Teleport teleport in _teleports._teleports) {
        if (teleport == null)
          continue;
        if (teleport.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            teleport.DrawOnMap (g, Point.Empty, this, Point.Empty);
            return;
          }
        }
      }

      // objective location
      foreach (ObjectiveLocation objectiveLocation in _objectiveLocations) {
        if (objectiveLocation.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            objectiveLocation.DrawOnMap (g, Point.Empty, this, Point.Empty);
            return;
          }
        }
      }

      // explosion
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.DestroyType4_06, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Explosion_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Explosion_DrawOnMap (g, this, Point.Empty);
            return;
          }
        }
      }

      // moving block
      foreach (Alfils08_MovingBlock movingBlock in _movingBlocks._movingBlocks) {
        if (movingBlock == null)
          continue;
        if (movingBlock.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            movingBlock.DrawOnMap (g, -1, this, Point.Empty);
            return;
          }
        }
      }

      if (_itemToShow != 0) {
        _itemToShow = 0;
        DrawOnMap (g, cellXY, pixelXY);
      }
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, Point pixelXY) {
      _eventsVisited.Clear ();
      int i = 0;

      // map objects
      foreach (MapItem mapObject in GetMapObjectsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          mapObject.Item_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
          return;
        }
      }

      // map weapons
      foreach (MapItem mapWeapon in GetMapWeaponsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          mapWeapon.Item_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
          return;
        }
      }

      // spawned objects
      foreach (MapPuzzle puzzle in GetSpawnedObjectsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          puzzle.SpawnedItem_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
          return;
        }
      }

      // spawned weapons
      foreach (MapPuzzle puzzle in GetSpawnedWeaponsAt (pixelXY)) {
        if (i++ == _itemToShow) {
          puzzle.SpawnedItem_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
          return;
        }
      }

      // event
      {
        Alfils02_Event event_ = GetMapEventAt (cellXY);
        if (event_ != null) {
          if (i++ == _itemToShow) {
            event_.DrawOnMap_Reverse (g, cellXY, this, Point.Empty, false);
            return;
          }
        }
      }

      // trapdoor
      Alfils07_Trapdoor trapdoor = GetTrapdoorAtCell (cellXY);
      if (trapdoor != null) {
        if (i++ == _itemToShow) {
          trapdoor.DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
          return;
        }
      }

      // door
      bool found = false;
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenDoor_02, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Door_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Door_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            found = true;
          }
        }
      }
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.CloseDoor_09, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Door_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Door_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            found = true;
          }
        }
      }
      if (found)
        return;

      // backdoor
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Backdoor_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Backdoor_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            return;
          }
        }
      }
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Backdoor_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Backdoor_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            return;
          }
        }
      }

      // backdoor teleport
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.BackdoorTeleport_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.BackdoorTeleport_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            return;
          }
        }
      }

      // stone teleport
      foreach (Alfils06_Teleport teleport in _teleports._teleports) {
        if (teleport == null)
          continue;
        if (teleport.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            teleport.DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            return;
          }
        }
      }

      // objective location
      foreach (ObjectiveLocation objectiveLocation in _objectiveLocations) {
        if (objectiveLocation.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            objectiveLocation.DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            return;
          }
        }
      }

      // explosion
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.DestroyType4_06, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Explosion_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            puzzle.Explosion_DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            return;
          }
        }
      }

      // moving block
      foreach (Alfils08_MovingBlock movingBlock in _movingBlocks._movingBlocks) {
        if (movingBlock == null)
          continue;
        if (movingBlock.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow) {
            movingBlock.DrawOnMap_Reverse (g, Point.Empty, this, Point.Empty, false);
            return;
          }
        }
      }
    }

    public void DrawBox (Graphics g, Point cellXY, Point pixelXY) {
      _eventsVisited.Clear ();
      int i = 0;
      //if (Alfils02_Event._recursion > 100) {
      //  Alfils02_Event._recursion = 0;
      //  BoxHelper.DrawBox (InfiniteLoop_DrawOnBox, g, cellXY, pixelXY, this);
      //  return;
      //}

      // map objects
      foreach (MapItem mapObject in GetMapObjectsAt (pixelXY)) {
        if (i++ == _itemToShow)
          BoxHelper._isActive = true;
        else
          BoxHelper._isActive = false;
        BoxHelper.Brush (Brushes.Orange);
        mapObject.DrawBox (g, this);
        //  return;
        //}
      }

      // map weapons
      foreach (MapItem mapWeapon in GetMapWeaponsAt (pixelXY)) {
        if (i++ == _itemToShow)
          BoxHelper._isActive = true;
        else
          BoxHelper._isActive = false;
        BoxHelper.Brush (Brushes.Orange);
        mapWeapon.DrawBox (g, this);
        //  return;
        //}
      }

      // spawned objects
      foreach (MapPuzzle puzzle in GetSpawnedObjectsAt (pixelXY)) {
        if (i++ == _itemToShow)
          BoxHelper._isActive = true;
        else
          BoxHelper._isActive = false;
        BoxHelper.Brush (Brushes.Orange);
        puzzle.SpawnedItem_DrawBox (g, this);
        //  return;
        //}
      }

      // spawned weapons
      foreach (MapPuzzle puzzle in GetSpawnedWeaponsAt (pixelXY)) {
        if (i++ == _itemToShow)
          BoxHelper._isActive = true;
        else
          BoxHelper._isActive = false;
        BoxHelper.Brush (Brushes.Orange);
        puzzle.SpawnedItem_DrawBox (g, this);
        //  return;
        //}
      }

      // event
      {
        Alfils02_Event event_ = GetMapEventAt (cellXY);
        if (event_ != null) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          event_.DrawBox (g, this);
          //  return;
          //}
        }
      }

      // trapdoor
      Alfils07_Trapdoor trapdoor = GetTrapdoorAtCell (cellXY);
      if (trapdoor != null) {
        if (i++ == _itemToShow)
          BoxHelper._isActive = true;
        else
          BoxHelper._isActive = false;
        BoxHelper.Brush (Brushes.Orange);
        trapdoor.DrawBox (g, this);
        //  return;
        //}
      }

      // door
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenDoor_02, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Door_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          puzzle.Door_DrawBox (g, this);
          //  return;
          //}
        }
      }
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.CloseDoor_09, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Door_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          puzzle.Door_DrawBox (g, this);
          //  return;
          //}
        }
      }

      // backdoor
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Backdoor_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          puzzle.Backdoor_DrawBox (g, this);
          //  return;
          //}
        }
      }
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorWorldCompleted_04, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Backdoor_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          puzzle.Backdoor_DrawBox (g, this);
          //  return;
          //}
        }
      }

      // backdoor teleport
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenBackdoorTeleport_03, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.BackdoorTeleport_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          puzzle.BackdoorTeleport_DrawBox (g, this);
          //  return;
          //}
        }
      }

      // stone teleport
      foreach (Alfils06_Teleport teleport in _teleports._teleports) {
        if (teleport == null)
          continue;
        if (teleport.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          teleport.DrawBox (g, this);
          //  return;
          //}
        }
      }

      // objective location
      foreach (ObjectiveLocation objectiveLocation in _objectiveLocations) {
        if (objectiveLocation.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          objectiveLocation.DrawBox (g, this);
          //  return;
          //}
        }
      }

      // explosion
      foreach (Alfils02_Event event_ in GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.DestroyType4_06, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle.Explosion_Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          puzzle.Explosion_DrawBox (g, this);
          //  return;
          //}
        }
      }

      // moving block
      foreach (Alfils08_MovingBlock movingBlock in _movingBlocks._movingBlocks) {
        if (movingBlock == null)
          continue;
        if (movingBlock.Rectangle.Contains (pixelXY)) {
          if (i++ == _itemToShow)
            BoxHelper._isActive = true;
          else
            BoxHelper._isActive = false;
          BoxHelper.Brush (Brushes.Orange);
          movingBlock.DrawBox (g, this);
          //  return;
          //}
        }
      }
    }

    public void InfiniteLoop_DrawOnBox (Graphics g, Point cellXY, Point pixelXY, World world) {
      BoxHelper.AddStringLine (g, string.Format ("INFINITE LOOP"));
    }
    //*****************************************************************************************************************
    public int Level {
      get {
        return (_level);
      }
    }

    public ObjectInfo GetObjectInfo (int objectInfoIndex) {
      return (_objectsInfo._objectsInfo [objectInfoIndex]);
    }

    public WeaponInfo GetWeaponInfo (int weaponInfoIndex) {
      return (_weaponsInfo._weaponsInfo [weaponInfoIndex]);
    }

    public EnemyInfo GetWalkingEnemyInfo (int enemyInfoIndex) {
      return (_walkingEnemiesInfo._enemiesInfo [enemyInfoIndex]);
    }

    public EnemyInfo GetFlyingEnemyInfo (int enemyInfoIndex) {
      return (_flyingEnemiesInfo._enemiesInfo [enemyInfoIndex]);
    }

    public FlyingPath GetFlyingPath (int flyingPathIndex) {
      return (_flyingPaths._flyingPath [flyingPathIndex]);
    }

    public Alfils01_FlyingWave GetFlyingWave (int flyingWaveIndex) {
      return (_flyingWaves._flyingWaves [flyingWaveIndex]);
    }

    public Alfils04a_IntelFlyingWave GetIntelFlyingWave (int intelFlyingWaveIndex) {
      return (_intelFlyingWaves._intelFlyingWaves [intelFlyingWaveIndex]);
    }

    public Alfils03_WalkingWave GetWalkingWave (int walkingWaveIndex) {
      return (_walkingWaves._walkingWaves [walkingWaveIndex]);
    }

    public Alfils04b_IntelWalkingWave GetIntelWalkingWave (int intelWalkingWaveIndex) {
      return (_intelWalkingWaves._intelWalkingWaves [intelWalkingWaveIndex]);
    }

    public MapItem GetMapItem (int mapItemIndex) {
      return (_map._mapItems [mapItemIndex]);
    }

    public Alfils05_Switch GetSwitch (int switchIndex) {
      return (_switches._switches [switchIndex]);
    }

    public MapPuzzle GetPuzzle (int puzzleIndex) {
      return (_map._mapPuzzles [puzzleIndex]);
    }

    public Alfils02_Event GetEvent (int eventIndex) {
      return (_events._events [eventIndex]);
    }

    public Alfils08_MovingBlock GetMovingBlock (int movingBlockIndex) {
      return (_movingBlocks._movingBlocks [movingBlockIndex]);
    }

    public Bitmap GetBitmap (int spriteIndex) {
      return (_spritesData [spriteIndex]._bitmap);
    }

    public Bitmap GetBitmapObject (int objectInfoIndex) {
      return (_spritesData [World._objectSpriteIndexBase + objectInfoIndex]._bitmap);
    }

    public Bitmap GetBitmapWeapon (int weaponInfoIndex) {
      return (_spritesData [World._weaponSpriteIndexBase + weaponInfoIndex]._bitmap);
    }

    public Bitmap GetBitmapMap (int mapSpriteIndex) {
      if ((mapSpriteIndex - 1) >= _map._mapSprites.Count)
        return (null);
      return (_map._mapSprites [mapSpriteIndex - 1]);
    }

    //*****************************************************************************************************************
    public int GetMapEventIndexAt (Point cellXY) {
      if ((cellXY.X < 0) || (cellXY.Y < 0) || (cellXY.X >= 128) || (cellXY.Y >= 64))
        return (-1);
      int eventIndex = _map._layerB [cellXY.X, cellXY.Y] - 3;
      if (eventIndex < 0)
        return (-1);
      return (eventIndex);
    }

    public Alfils02_Event GetMapEventAt (Point cellXY) {
      int eventIndex = GetMapEventIndexAt (cellXY);
      if (eventIndex == -1)
        return (null);
      return (GetEvent (eventIndex));
    }

    public static Point DecodeTeleport (int coded) {
      int pixelX = (coded >> 8) * 32 + 16;
      int pixelY = (coded & 0x00FF) * 16;
      return (new Point (pixelX, pixelY));
    }

    public static float FramesToSeconds_3VBL (int nbFrames) {
      // at 17 fps: 170 -> 10s
      // Every 3 VBL = 50/3 = 16.666 ~= 17
      return (nbFrames / (50f / 3));
    }

    public static float FramesToSeconds (int nbFrames) {
      // Every VBL = 50 fps
      return (nbFrames / 50f);
    }

    public static int CalcEnemyScoreValue (int health) {
      return ((((health & 0xFF) >> 1) + 1) * 100);
    }

    public static int [] _jumpHeights = new int [] { 0, 1, 2, 4, 8, 16, 24, 32, 40, 48 };
    public static int CalcEnemyJumpHeight (int jumpValue) {
      return (_jumpHeights [jumpValue]);
    }

    public static string GetPickupType (int pickupType) {
      //00: can't pickup (drop always)
      //01: 1/128 to drop
      //02: can't drop
      if (pickupType == 0)
        return ("no");
      else if (pickupType == 1)
        return ("yes, may randomly drop (1/128 % every frame)");
      else if (pickupType == 2)
        return ("yes");
      else
        throw new Exception ();
    }

    public Alfils06_Teleport GetTeleportAt (int pixelX) {
      foreach (Alfils06_Teleport teleport in _teleports._teleports) {
        if (teleport == null)
          continue;
        if (teleport._srcPixelX == pixelX) {
          return (teleport);
        }
      }
      return (null);
    }

    public Alfils09_Hint GetHintAt (int pixelX) {
      foreach (Alfils09_Hint hint in _hints._hints) {
        if (hint == null)
          continue;
        if (hint._pixelX == pixelX) {
          return (hint);
        }
      }
      return (null);
    }

    public Alfils05_Switch GetSwitchAt (Point pixelXY) {
      foreach (Alfils05_Switch switch_ in _switches._switches) {
        if (switch_ == null)
          continue;
        if ((switch_._pixelX == pixelXY.X) && (switch_._pixelY == pixelXY.Y)) {
          return (switch_);
        }
      }
      return (null);
    }

    //*****************************************************************************************************************
    public List<MapItem> GetMapItemsWithEffect (ObjectEffectTypeEnum effectType) {
      List<MapItem> mapItems = new List<MapItem> ();
      foreach (MapItem mapItem in _map._mapItems) {
        if (mapItem == null)
          continue;
        if (!mapItem.IsObject)
          continue;
        ObjectInfo objectInfo = GetObjectInfo (mapItem.ObjectInfoIndex);
        if (objectInfo.Type != ObjectInfoTypeEnum.Usable_00)
          continue;
        if (objectInfo.EffectType != effectType)
          continue;
        mapItems.Add (mapItem);
      }
      return (mapItems);
    }

    public string GetPuzzleMessage (int stringIndex) {
      return (_map._stringsPuzzle [stringIndex]);
    }

    public List<Alfils02_Event> GetEventsWithEffect (EventTypeEnum eventType, int eventParam) {
      List<Alfils02_Event> events = new List<Alfils02_Event> ();
      foreach (Alfils02_Event event_ in _events._events) {
        if (event_ == null)
          continue;
        if (event_.Type != eventType)
          continue;
        if (eventParam != -1) {
          if (event_._param != eventParam)
            continue;
        }
        events.Add (event_);
      }
      return (events);
    }

    public List<Alfils02_Event> GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum effectType, int effectParam) {
      List<Alfils02_Event> events = new List<Alfils02_Event> ();
      foreach (Alfils02_Event event_ in _events._events) {
        if (event_ == null)
          continue;
        if (event_.Type != EventTypeEnum.CheckPuzzle_02)
          continue;
        MapPuzzle puzzle = event_.GetPuzzle (this);
        if (puzzle._effectType != effectType)
          continue;
        if (effectParam != -1) {
          if (puzzle._effectParam != effectParam)
            continue;
        }
        events.Add (event_);
      }
      return (events);
    }

    public List<Alfils02_Event> GetEventsWithPuzzleCondition (ConditionTypeEnum conditionType, int conditionParam) {
      List<Alfils02_Event> events = new List<Alfils02_Event> ();
      foreach (Alfils02_Event event_ in _events._events) {
        if (event_ == null)
          continue;
        if (event_.Type != EventTypeEnum.CheckPuzzle_02)
          continue;
        MapPuzzle puzzle = event_.GetPuzzle (this);
        foreach (PuzzleConditionTypeParam conditionTypeParam in puzzle._conditionsTypeParam) {
          if (conditionTypeParam._type != conditionType)
            continue;
          if (conditionTypeParam._param != conditionParam)
            continue;
          events.Add (event_);
          break;
        }
      }
      return (events);
    }

    public List<MapPuzzle> GetPuzzlesWithEffect (PuzzleEffectTypeEnum effectType, int effectParam) {
      List<MapPuzzle> puzzles = new List<MapPuzzle> ();
      foreach (MapPuzzle puzzle in _map._mapPuzzles) {
        if (puzzle == null)
          continue;
        if (puzzle._effectType != effectType)
          continue;
        if (effectParam != -1) {
          if (puzzle._effectParam != effectParam)
            continue;
        }
        puzzles.Add (puzzle);
      }
      return (puzzles);
    }

    //*****************************************************************************************************************
    public List<MapItem> GetMapWeapons (int weaponInfoIndex) {
      List<MapItem> mapItems = new List<MapItem> ();
      foreach (MapItem mapItem in _map._mapItems) {
        if (mapItem == null)
          continue;
        if (!mapItem.IsWeapon)
          continue;
        if (weaponInfoIndex != -1) {
          if (mapItem.WeaponInfoIndex != weaponInfoIndex)
            continue;
        }
        mapItems.Add (mapItem);
      }
      return (mapItems);
    }

    public Alfils07_Trapdoor GetTrapdoorAtCell (Point cellXY) {
      foreach (Alfils07_Trapdoor trapdoor in _trapdoors._trapdoors) {
        if (trapdoor == null)
          continue;
        if ((trapdoor._pixelXY.X == (cellXY.X * 32)) && (trapdoor._pixelXY.Y == (cellXY.Y * 16))) {
          return (trapdoor);
        }
      }
      return (null);
    }

    public Alfils07_Trapdoor GetTrapdoorAt (Point pixelXY) {
      foreach (Alfils07_Trapdoor trapdoor in _trapdoors._trapdoors) {
        if (trapdoor == null)
          continue;
        if ((trapdoor._pixelXY.X == pixelXY.X) && (trapdoor._pixelXY.Y == pixelXY.Y)) {
          return (trapdoor);
        }
      }
      return (null);
    }

    public List<MapItem> GetMapObjectsAt (Point pixelXY) {
      List<MapItem> mapObjects = new List<MapItem> ();
      foreach (MapItem mapItem in _map._mapItems) {
        if (mapItem == null)
          continue;
        if (!mapItem.IsObject)
          continue;
        Bitmap bitmap = GetBitmapObject (mapItem.ObjectInfoIndex);
        if ((pixelXY.X >= mapItem.ItemPixelXY.X) && (pixelXY.X < (mapItem.ItemPixelXY.X + bitmap.Width))) {
          if ((pixelXY.Y >= mapItem.ItemPixelXY.Y) && (pixelXY.Y < (mapItem.ItemPixelXY.Y + bitmap.Height))) {
            mapObjects.Add (mapItem);
          }
        }
      }
      return (mapObjects);
    }

    public List<MapItem> GetMapWeaponsAt (Point pixelXY) {
      List<MapItem> mapWeapons = new List<MapItem> ();
      foreach (MapItem mapItem in _map._mapItems) {
        if (mapItem == null)
          continue;
        if (!mapItem.IsWeapon)
          continue;
        Bitmap bitmap = GetBitmapWeapon (mapItem.WeaponInfoIndex);
        if ((pixelXY.X >= mapItem.ItemPixelXY.X) && (pixelXY.X < (mapItem.ItemPixelXY.X + bitmap.Width))) {
          if ((pixelXY.Y >= mapItem.ItemPixelXY.Y) && (pixelXY.Y < (mapItem.ItemPixelXY.Y + bitmap.Height))) {
            mapWeapons.Add (mapItem);
          }
        }
      }
      return (mapWeapons);
    }

    public List<MapPuzzle> GetSpawnedObjectsAt (Point pixelXY) {
      List<MapPuzzle> mapPuzzles = new List<MapPuzzle> ();
      foreach (MapPuzzle puzzle in GetPuzzlesWithEffect (PuzzleEffectTypeEnum.SpawnObject_00, -1)) {
        Bitmap bitmap = GetBitmapObject (puzzle.ObjectInfoIndex);
        if ((pixelXY.X >= puzzle.ItemPixelXY.X) && (pixelXY.X < (puzzle.ItemPixelXY.X + bitmap.Width))) {
          if ((pixelXY.Y >= puzzle.ItemPixelXY.Y) && (pixelXY.Y < (puzzle.ItemPixelXY.Y + bitmap.Height))) {
            mapPuzzles.Add (puzzle);
          }
        }
      }
      return (mapPuzzles);
    }

    public List<MapPuzzle> GetSpawnedWeaponsAt (Point pixelXY) {
      List<MapPuzzle> mapPuzzles = new List<MapPuzzle> ();
      foreach (MapPuzzle puzzle in GetPuzzlesWithEffect (PuzzleEffectTypeEnum.SpawnWeapon_01, -1)) {
        Bitmap bitmap = GetBitmapWeapon (puzzle.WeaponInfoIndex);
        if ((pixelXY.X >= puzzle.ItemPixelXY.X) && (pixelXY.X < (puzzle.ItemPixelXY.X + bitmap.Width))) {
          if ((pixelXY.Y >= puzzle.ItemPixelXY.Y) && (pixelXY.Y < (puzzle.ItemPixelXY.Y + bitmap.Height))) {
            mapPuzzles.Add (puzzle);
          }
        }
      }
      return (mapPuzzles);
    }
  }

  public class WorldDrawingInfo {
    public bool _showRaster;
    public bool _highlightWallsStairs;
    public bool _showVisualHints;
    public bool _showEvents;
    public bool _showHiddenObjects;

    public bool _recursiveEvents;
    public bool _showForwardLinks;
    public bool _showBackwardLinks;

    public WorldDrawingInfo (bool showRaster, bool highlightWallsStairs, bool showVisualHints, bool showEvents, bool showHiddenObjects, bool recursiveEvents, bool showForwardLinks, bool showBackwardLinks) {
      _showRaster = showRaster;
      _highlightWallsStairs = highlightWallsStairs;
      _showVisualHints = showVisualHints;
      _showEvents = showEvents;
      _showHiddenObjects = showHiddenObjects;

      _recursiveEvents = recursiveEvents;
      _showForwardLinks = showForwardLinks;
      _showBackwardLinks = showBackwardLinks;
    }
  }

  public enum DrawTypeEnum {
    DrawBitmaps,
    DrawArrows
  }
}
