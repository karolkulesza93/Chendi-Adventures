using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Threading;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

/*
do zrobienia:
- nerf rewardow z loterri
- zmiana formatu leveli z txt na cos mniej dostepnego dla casuala
- levele
- moze bossy
- scenka na poczatek
- settingsy (hotkeye, poziom trudnosci, rozdzielczosci, vsync)
- sklep
*/

namespace Game
{
    public sealed class MainGameWindow
    {
        public static bool DevManipulation = false;

        //fields
        /*signleton field*/
        private static MainGameWindow _instance;

        /*signleton field*/
        private static readonly object _padlock = new object();

        private readonly View _view;
        private readonly RenderWindow _window;
        private readonly Random _randomizer;
        private Sprite _background;
        private MainCharacter _chendi;
        private MainCharacterUI _chendiUI;
        private Sound _gameEnd;
        private Sprite _gameLogo;
        private TextLine _gameOver;
        private TextLine _highscores;
        private Highscores _highscoreValues;
        private Level _level;
        private TextLine _levelSummary;
        private TextLine _loading;
        private Music _mainTheme;
        private Music _menuTheme;
        private TextLine _no;
        private TextLine _yes;
        private TextLine _continue;
        private TextLine _pause;
        private TextLine _quit;
        private TextLine _quitQestion;
        private TextLine _settings;
        private TextLine _start;
        private Sound _victory;
        private int _windowHeight = 1080;
        private Styles _windowStyle = Styles.Fullscreen;
        private int _windowWidth = 1920;
        private bool isGame;
        private bool isHighscore;
        private bool isMenu;
        private bool isPaused;
        private bool isQuit;
        private bool isSettigs;

        //contructors
        public MainGameWindow()
        {
            isMenu = true;
            isGame = false;
            isHighscore = false;
            isSettigs = false;
            isQuit = false;
            isPaused = false;
            _randomizer = new Random();
        }

        public MainGameWindow(string title) : this()
        {
            DevManip();

            Title = title;
            _window = new RenderWindow(new VideoMode((uint) _windowWidth, (uint) _windowHeight), Title, _windowStyle);
            _window.SetFramerateLimit(60);
            _window.SetVisible(true);
            _window.Closed += OnClosed;
            _window.SetMouseCursorVisible(false);
            _window.SetKeyRepeatEnabled(false);
            _window.SetVerticalSyncEnabled(true);
            _view = new View(new FloatRect(0, 0, _windowWidth, _windowHeight));
        }

        //properties
        /*signleton property*/
        public static MainGameWindow Instance
        {
            get
            {
                if (_instance == null)
                    lock (_padlock)
                    {
                        if (_instance == null)
                            _instance = new MainGameWindow();
                    }

                return _instance;
            }
        }

        public string Title { get; set; }

        //methods
        public void GameStart()
        {
            _loading = new TextLine("LOADING...", 50, 1390, 990, Color.White);
            var tmp = new Texture("img/tiles.png", new IntRect(new Vector2i(32, 0), new Vector2i(32, 32)));
            tmp.Repeated = true;

            _background = new Sprite(tmp);
            _background.Scale = new Vector2f(2f, 2f);
            _background.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(1000, 600));

            _gameLogo = new Sprite(new Texture(@"img/logo.png"));
            _gameLogo.Position = new Vector2f((_windowWidth - _gameLogo.Texture.Size.X) / 2, 50);

            DrawLoadingScreen();

            LoadContents();
            MainLoop();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _window.Close();
            isMenu = false;
            isGame = false;
            isHighscore = false;
            isSettigs = false;
            isQuit = false;
        }

        private void LoadContents()
        {
            _menuTheme = new Music("sfx/menutheme.wav");
            _menuTheme.Loop = true;
            _menuTheme.Volume = 40;

            _mainTheme = new Music("sfx/main theme.wav");
            _mainTheme.Loop = true;
            _mainTheme.Volume = 30;

            _quitQestion = new TextLine("QUIT GAME?", 50, 265, 230, Color.White);
            _quitQestion.SetOutlineThickness(2f);
            _yes = new TextLine("YES", 50, 435, 300, Color.White);
            _yes.SetOutlineThickness(2f);
            _no = new TextLine("NO", 50, 460, 370, Color.Green);
            _no.SetOutlineThickness(2f);

            _victory = new Sound(new SoundBuffer(@"sfx/victory.wav"));
            _victory.Volume = 50;
            _gameEnd = new Sound(new SoundBuffer(@"sfx/gameover.wav"));
            _gameEnd.Volume = 50;
            

            _gameOver = new TextLine("GAME OVER", 100, 500, 470, Color.Red);
            _gameOver.SetOutlineThickness(3f);
            _pause = new TextLine("PAUSE", 50, 0, 0, Color.Yellow);
            _continue = new TextLine("CONTINUE?", 50, 650, 590, Color.Yellow);
            _continue.SetOutlineThickness(3f);

            _chendi = new MainCharacter(-100, -100, Entity.MainCharacterTexture);
            _level = new Level(_chendi);
            _chendiUI = new MainCharacterUI(_chendi, _view, _level);

            _highscoreValues = new Highscores();

        }

        private void MainLoop()
        {
            while (isGame || isMenu || isHighscore || isSettigs)
            {
                if (isMenu) MainMenuLoop();
                if (isGame) GameLoop();
                if (isHighscore) HighScoreLoop();
                if (isSettigs) SettingsLoop();
                if (isQuit) break;
            }

            _window.Close();
            Console.Clear();
            Environment.Exit(0);
        }

        private void SettingsLoop()
        {
            var Resolution = new TextLine("RESOLUTION: 1920x1080", 50, 50, 790, Color.Green);
            var Vsync = new TextLine("VSYNC: ON", 50, 50, 850, Color.White);
            var Difficulty = new TextLine("DIFFICULTY: MEDIUM", 50, 50, 910, Color.White);
            var KeyBindings = new TextLine("KEY BINDINGS", 50, 50, 970, Color.White);

            var choice = 1;
            var delay = 0;
            var flag = false;

            while (_window.IsOpen && isSettigs)
            {
                ResetWindow();

                // na pewno poziom trudnosci
                // controllsy jak sie bedzie chcialo
                // moze pobawic sie rozdzielczoscia

                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }

                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    _chendi.sAtk.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    _chendi.sAtk.Play();
                    flag = true;
                    choice++;
                }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    flag = true;
                    _chendi.sCoin.Play();
                    switch (choice)
                    {
                        case 1: //resolution
                        {
                            break;
                        }
                        case 2: //vsync
                        {
                            break;
                        }
                        case 3: // difficulty
                        {
                            break;
                        }
                        case 4: //key bindings
                        {
                            break;
                        }
                    }
                }

                if (choice == 0) choice = 4;
                if (choice == 5) choice = 1;

                Resolution.ChangeColor(Color.White);
                Vsync.ChangeColor(Color.White);
                Difficulty.ChangeColor(Color.White);
                KeyBindings.ChangeColor(Color.White);

                switch (choice)
                {
                    case 1:
                    {
                        Resolution.ChangeColor(Color.Green);
                        break;
                    }
                    case 2:
                    {
                        Vsync.ChangeColor(Color.Green);
                        break;
                    }
                    case 3:
                    {
                        Difficulty.ChangeColor(Color.Green);
                        break;
                    }
                    case 4:
                    {
                        KeyBindings.ChangeColor(Color.Green);
                        break;
                    }
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    _chendi.sCoin.Play();
                    isSettigs = false;
                    isMenu = true;
                }

                AnimateBackground();
                _window.Draw(_background);

                //draw settings
                _window.Draw(Resolution);
                _window.Draw(Vsync);
                _window.Draw(Difficulty);
                _window.Draw(KeyBindings);

                _window.Display();
            }
        }

        private void KeyBindingConfig()
        {
            while (_window.IsOpen && isSettigs)
            {
                ResetWindow();
                /*
                public Sound sTramp { get; private set; }
                public Sound sAtk { get; private set; }
                public Sound sDie { get; private set; }
                public Sound sCoin { get; private set; }
                public Sound sTp { get; private set; }
                public Sound sKey { get; private set; }
                public Sound sLife { get; private set; }
                public Sound sImmortality { get; private set; }
                 */

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) break;
                AnimateBackground();
                _window.Draw(_background);


                //draw settings
                _window.Display();
            }
        }

        private void HighScoreLoop()
        {
            var hs = new TextLine("HIGHSCORES", 50, 700, 5, Color.Green);
            while (_window.IsOpen && isHighscore)
            {
                ResetWindow();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    _chendi.sCoin.Play();
                    isHighscore = false;
                    isMenu = true;
                }

                AnimateBackground();
                _window.Draw(_background);

                _window.Draw(_highscoreValues);
                _window.Draw(hs);
                _window.Display();
            }
        }

        private void MainMenuLoop()
        {
            _start = new TextLine("NEW GAME", 50, 50, 790, Color.Green);
            _highscores = new TextLine("HIGHSCORES", 50, 50, 850, Color.White);
            _settings = new TextLine("SETTINGS", 50, 50, 910, Color.White);
            _quit = new TextLine("EXIT", 50, 50, 970, Color.White);

            if (_menuTheme.Status != SoundStatus.Playing) _menuTheme.Play();
            DrawLoadingScreen();

            var choice = 1;
            var delay = 0;
            var flag = false;

            while (_window.IsOpen && isMenu)
            {
                ResetWindow();

                //secret keys to enter level editor
                if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) && Keyboard.IsKeyPressed(Keyboard.Key.LAlt) &&
                    Keyboard.IsKeyPressed(Keyboard.Key.L))
                    LevelEditor();
                //

                _start.ChangeColor(Color.White);
                _highscores.ChangeColor(Color.White);
                _settings.ChangeColor(Color.White);
                _quit.ChangeColor(Color.White);

                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }

                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    _chendi.sAtk.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    _chendi.sAtk.Play();
                    flag = true;
                    choice++;
                }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    DrawLoadingScreen();
                    flag = true;
                    _chendi.sCoin.Play();
                    switch (choice)
                    {
                        case 1:
                        {
                            isMenu = false;
                            isGame = true;
                            if (_level.LevelNumber != 0)
                            {
                                _level.LevelNumber = 1;
                            }
                            _chendi.ResetMainCharacter();
                            _menuTheme.Stop();
                            break;
                        }
                        case 2:
                        {
                            isMenu = false;
                            isHighscore = true;
                            break;
                        }
                        case 3:
                        {
                            isMenu = false;
                            isSettigs = true;
                            break;
                        }
                        case 4:
                        {
                            isMenu = false;
                            isQuit = true;
                            break;
                        }
                    }
                }

                if (choice == 0) choice = 4;
                if (choice == 5) choice = 1;

                switch (choice)
                {
                    case 1:
                    {
                        _start.ChangeColor(Color.Green);
                        break;
                    }
                    case 2:
                    {
                        _highscores.ChangeColor(Color.Green);
                        break;
                    }
                    case 3:
                    {
                        _settings.ChangeColor(Color.Green);
                        break;
                    }
                    case 4:
                    {
                        _quit.ChangeColor(Color.Green);
                        break;
                    }
                }

                if (isMenu) DrawMainMenu();
            }
        }

        private void GameLoop()
        {
            //DrawLoadingScreen();

            _levelSummary = new TextLine("", 25, -1000, -1000, Color.White);

            SetView(new Vector2f(960f, 540f), _view.Center);

            if (_level.LevelNumber == 1) BegginingScene();
            /// SET LEVEL FOR TESTING
            //this._level.LevelNumber = 17;
            ///

            _level.LoadLevel(string.Format("lvl{0}", _level.LevelNumber));

            _chendi.SetStartingPosition(_level);

            _level.StartScore = _chendi.Score;
            _level.StartCoins = _chendi.Coins;
            _level.StartArrows = _chendi.ArrowAmount;
            _level.StartMana = _chendi.Mana;

            _chendi.HasSilverKey = false;
            _chendi.HasGoldenKey = false;

            _mainTheme.Play();

            while (_window.IsOpen && isGame)
            {
                if (CheckForGameBreak()) break;

                ResetWindow();

                _chendi.MainCharactereUpdate(_level);
                _level.LevelUpdate();

                ViewManipulation(_level);
                _chendiUI.UpdateUI();

                DrawGame(_chendi, true);

                if (_chendi.IsDead) _mainTheme.Stop();

                //pause and exit
                if (Keyboard.IsKeyPressed(Keyboard.Key.P)) isPaused = true;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    var choice = false;
                    var flag = false;
                    var delay = 0;

                    _quitQestion.MoveText(_view.Center.X - 230, _view.Center.Y - 100);
                    _yes.MoveText(_view.Center.X - 65, _view.Center.Y + 20);
                    _no.MoveText(_view.Center.X - 40, _view.Center.Y + 90);

                    _yes.ChangeColor(Color.White);
                    _no.ChangeColor(Color.Green);

                    while (_window.IsOpen && isGame)
                    {
                        ResetWindow();

                        if (flag) delay++;
                        if (delay > 10)
                        {
                            delay = 0;
                            flag = false;
                        }

                        if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) ||
                                              Keyboard.IsKeyPressed(Keyboard.Key.Down)))
                        {
                            _chendi.sAtk.Play();
                            flag = true;
                            choice = !choice;
                            if (choice)
                            {
                                _yes.ChangeColor(Color.Green);
                                _no.ChangeColor(Color.White);
                            }
                            else
                            {
                                _yes.ChangeColor(Color.White);
                                _no.ChangeColor(Color.Green);
                            }
                        }
                        else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Space))
                        {
                            flag = true;
                            _chendi.sCoin.Play();

                            if (choice)
                            {
                                isGame = false;
                                isMenu = true;
                                flag = false;
                                _level.LevelNumber = 1;
                                _chendi.ResetMainCharacter();
                                Thread.Sleep(500);
                            }

                            break;
                        }

                        DrawGame(_chendi, false);

                        _window.Draw(_quitQestion);
                        _window.Draw(_yes);
                        _window.Draw(_no);
                        _window.Display();
                    }
                }

                if (isPaused) PauseLoop();
            }

            _mainTheme.Stop();

            if (!_chendi.OutOfLives && _chendi.IsDead) //death
            {
                //DrawLoadingScreen();
                _chendi.Score = _level.StartScore;
                _chendi.ArrowAmount = _level.StartArrows;
                _chendi.Mana = _level.StartMana;
                _chendi.Coins = _level.StartCoins;
                _chendi.Respawn(_level);
                _level.LoadLevel(string.Format("lvl{0}", _level.LevelNumber));
            }

            if (_chendi.OutOfLives) //game over
            {
                _highscoreValues.AddNewRecord(new HighscoreRecord(_chendi.Score, _level.LevelNumber));

                _gameEnd.Play();
                
                DrawGameOver();
                _chendi.ResetMainCharacter();
                DrawLoadingScreen();
            }

            if (_chendi.GotExit) //level complete
            {
                _victory.Play();
                //set level summary
                var time = Math.Round(_level.LevelTime.ElapsedTime.AsSeconds(), 2);
                var bonus = _level.GetBonusForTime(time);

                _chendi.Score += bonus;

                _levelSummary.EditText(string.Format(
                    "LEVEL COMPLETED!\n" +
                    "TIME: {0}" + " SECONDS\n" +
                    "TIME BONUS: {1}\n" +
                    "SCORE GAINED: {2}\n" +
                    "LIVES LEFT: {3}\n" +
                    "OVERALL SCORE: {4}",
                    time, bonus, _chendi.Score - _level.StartScore - bonus, _chendi.Lives, _chendi.Score));
                _levelSummary.SetOutlineThickness(2);

                //summary to the center
                _levelSummary.X = _view.Center.X - 1000;
                _levelSummary.Y = _view.Center.Y - 100;

                //draw summary
                _chendi.SetTextureRectanlge(96, 96, 32, 32);
            }

            var timer = new Clock();
            timer.Restart();
            _chendi.sImmortality.Stop();
            while (_window.IsOpen && isGame && _chendi.GotExit) //to next level
            {
                ResetWindow();

                if (_levelSummary.X < _view.Center.X - 250)
                    _levelSummary.MoveText(_levelSummary.X + 35, _levelSummary.Y);

                DrawGame(_chendi, false);
                _window.Draw(_levelSummary);
                _window.Display();

                if (timer.ElapsedTime.AsSeconds() > 6)
                {
                    _level.LevelNumber++;
                    _chendi.GotExit = false;
                    DrawLoadingScreen();
                    if (_level.LevelNumber < 51 && _level.LevelNumber > 0 && _randomizer.Next(101) > 0) Lottery();
                    break;
                }
            }

            timer.Dispose();
        }

        private void PauseLoop()
        {
            _pause.MoveText(_view.Center.X - 100, _view.Center.Y - 25);

            while (_window.IsOpen && isPaused)
            {
                ResetWindow();
                //this.DrawGame(this._chendi);
                _window.Draw(_pause);
                _window.Display();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) isPaused = false;
            }
        }

        public void ResetWindow()
        {
            _window.DispatchEvents();
            _window.Clear(Color.Black);
        }

        private void DrawLoadingScreen()
        {
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));
            ResetWindow();


            _window.Draw(_loading);
            _window.Display();
        }

        private void DrawGameOver()
        {
            var clock = new Clock();
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));

            if (_chendi.Continues == 0)
            {
                isGame = false;
                isMenu = true;
                _chendi.Continues = 2;
                _level.LevelNumber = 1;
                _chendi.ResetMainCharacter();

                while (clock.ElapsedTime.AsSeconds() < 8f)
                {
                    ResetWindow();
                    AnimateBackground();
                    _window.Draw(_background);

                    _window.Draw(_gameOver);
                    _window.Display();
                }
            }
            else
            {
                while (clock.ElapsedTime.AsSeconds() < 11f)
                {
                    ResetWindow();
                    AnimateBackground();
                    _window.Draw(_background);

                    _continue.EditText(string.Format("CONTINUE? {0}", 10 - (int)clock.ElapsedTime.AsSeconds()));

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    {
                        _chendi.sCoin.Play();
                        _chendi.ResetMainCharacter();
                        _chendi.Continues--;
                        _gameEnd.Stop();
                        DrawLoadingScreen();
                        return;
                    }

                    _window.Draw(_gameOver);
                    _window.Draw(_continue);
                    _window.Display();
                }
                isGame = false;
                isMenu = true;
            }

            clock.Dispose();
        }

        private void DrawGame(MainCharacter character, bool display, params Entity[] entities)
        {
            _window.Draw(_level);
            _window.Draw(character);
            foreach (var entity in entities) _window.Draw(entity);
            _window.Draw(_chendiUI);
            if (display) _window.Display();
        }

        private void DrawMainMenu()
        {
            AnimateBackground();
            _window.Draw(_background);
            _window.Draw(_gameLogo);

            _window.Draw(_start);
            _window.Draw(_highscores);
            _window.Draw(_settings);
            _window.Draw(_quit);
            _window.Display();
        }

        private void ViewManipulation(Level level)
        {
            var x = _view.Center.X;
            var y = _view.Center.Y;

            x += (_chendi.GetCenterPosition().X - _view.Center.X) / 10;
            if (x - _view.Size.X / 2 <= 0) x = _view.Size.X / 2;
            else if (x + _view.Size.X / 2 >= level.LevelWidth * 32) x = level.LevelWidth * 32 - _view.Size.X / 2;

            y += (_chendi.GetCenterPosition().Y - _view.Center.Y) / 10;
            if (y - _view.Size.Y / 2 <= 0) y = _view.Size.Y / 2;
            else if (y + _view.Size.Y / 2 >= level.LevelHeight * 32) y = level.LevelHeight * 32 - _view.Size.Y / 2;


            _view.Center = new Vector2f(x, y);
            _window.SetView(_view);
        }

        private void SetView(Vector2f size, Vector2f center)
        {
            _view.Size = size;
            _view.Center = center;
            _window.SetView(_view);
        }

        private bool CheckForGameBreak()
        {
            if (_chendi.GotExit || _chendi.OutOfLives && !_chendi.IsDead) return true;
            if (_chendi.IsDead && _chendi.DefaultClock.ElapsedTime.AsSeconds() > 3) return true;
            return false;
        }

        private void AnimateBackground()
        {
            if (_background.Position.X < -32 && _background.Position.Y < -32)
                _background.Position = new Vector2f(0, 0);
            else _background.Position = new Vector2f(_background.Position.X - 0.2f, _background.Position.Y - 0.2f);
        }

        private void BegginingScene()
        {
            //music
            //textures
            //skip button
            //clear draw display
        }

        private void LevelEditor()
        {
            _menuTheme.Stop();
            _level.LoadLevel(string.Format("edit", _level.LevelNumber));

            var choice = new Sprite(new Texture(@"img/edit.png"));
            choice.Position = new Vector2f(32, 32);

            var cover = new RectangleShape(new Vector2f(1000, 500));
            cover.FillColor = Color.Black;
            cover.Position = new Vector2f(-201, -520);

            var position = new TextLine("", 10, 0, 0, Color.Yellow);
            var instructions = new TextLine(File.ReadAllText(@"levels/instructions.dat"), 10, -300, 0, Color.White);

            _chendi.SetPosition(-100, -100);
            _chendi.IsDead = true;
            _chendiUI.ResetPositions();

            _window.SetKeyRepeatEnabled(false);

            var x = 1;
            var y = 1;
            var flag = false;
            var delay = 0;

            var view = true;

            BlockType type = 0;
            BlockType type_m = 0;

            var rnd = new Random();

            while (_window.IsOpen)
            {
                ResetWindow();
                // moving editor
                if (!flag && x > 1 && Keyboard.IsKeyPressed(Keyboard.Key.Left))
                {
                    x--;
                    flag = true;
                    _chendi.sAtk.Play();
                }

                if (!flag && x < _level.LevelWidth - 2 && Keyboard.IsKeyPressed(Keyboard.Key.Right))
                {
                    x++;
                    flag = true;
                    _chendi.sAtk.Play();
                }

                if (!flag && y > 1 && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    y--;
                    flag = true;
                    _chendi.sAtk.Play();
                }

                if (!flag && y < _level.LevelHeight - 2 && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    y++;
                    flag = true;
                    _chendi.sAtk.Play();
                }

                type = _level.GetObstacle(x, y).Type;

                //changes - tiles (edit+, edit-, edit_remembered, delete)
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.X))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    type++;
                    if (type == (BlockType) 31) type = 0;

                    type_m = type;

                    if (type >= 0 && type <= (BlockType) 13)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (type >= (BlockType) 14 && type <= (BlockType) 24)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.PickupsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.DetailsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }

                    _level.GetObstacle(x, y).Type = type;
                    _level.GetObstacle(x, y).SetBlock(type);
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Z))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    if (type > 0) type--;
                    else type = (BlockType) 30;

                    type_m = type;

                    if (type >= 0 && type <= (BlockType) 13)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (type >= (BlockType) 14 && type <= (BlockType) 24)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.PickupsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.DetailsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }

                    _level.GetObstacle(x, y).Type = type;
                    _level.GetObstacle(x, y).SetBlock(type);
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.C))
                {
                    flag = true;
                    _chendi.sCoin.Play();


                    if (type_m >= 0 && type_m <= (BlockType) 13)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (type_m >= (BlockType) 14 && type_m <= (BlockType) 24)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.PickupsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.DetailsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }

                    _level.GetObstacle(x, y).Type = type_m;
                    _level.GetObstacle(x, y).SetBlock(type_m);
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    type = BlockType.None;

                    _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                    _level.GetObstacle(x, y).UseTexture();

                    _level.GetObstacle(x, y).Type = type;
                    _level.GetObstacle(x, y).SetBlock(type);
                }

                //changes - monsters
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Monsters.Add(new Monster(x * 32, y * 32, Entity.KnightTexture));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Archers.Add(new Archer(x * 32, y * 32, Entity.ArcherTexture, Movement.Left));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Archers.Add(new Archer(x * 32, y * 32, Entity.ArcherTexture, Movement.Right));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num4))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Ghosts.Add(new Ghost(x * 32, y * 32, Entity.GhostTexture));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num5))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Wizards.Add(new Wizard(x * 32, y * 32, Entity.WizardTexture));
                }

                //clear mosters
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F1))
                {
                    flag = true;
                    Creature.sKill.Play();
                    _level.Monsters.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F2))
                {
                    flag = true;
                    Creature.sKill.Play();
                    _level.Archers.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F4))
                {
                    flag = true;
                    Creature.sKill.Play();
                    _level.Ghosts.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F5))
                {
                    flag = true;
                    Creature.sKill.Play();
                    _level.Wizards.Clear();
                }

                //changes - traps
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num7))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.Crusher));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num8))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.Spikes));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num9))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.BlowTorchLeft));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num0))
                {
                    flag = true;
                    _chendi.sCoin.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.BlowTorchRight));
                }

                //clear traps
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F7))
                {
                    flag = true;
                    Creature.sKill.Play();
                    _level.Traps.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F12))
                {
                    flag = true;
                    Creature.sKill.Play();

                    for (x = 1; x < _level.LevelWidth - 2; x++)
                    for (y = 1; y < _level.LevelHeight - 2; y++)
                    {
                        type = BlockType.None;

                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();

                        _level.GetObstacle(x, y).Type = type;
                        _level.GetObstacle(x, y).SetBlock(type);
                    }

                    x = 1;
                    y = 1;
                }

                //viem manip (+/-)
                if (view && !flag && Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    flag = true;
                    _chendi.sCoin.Play();
                    view = false;
                }

                if (!view && !flag && Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    flag = true;
                    _chendi.sCoin.Play();
                    view = true;
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.I))
                {
                    flag = true;
                    _chendi.sCoin.Play();
                    x = 1;
                    y = 1;
                }

                choice.Position = new Vector2f(x * 32, y * 32);
                position.MoveText(choice.Position.X + 34, choice.Position.Y - 36);
                position.EditText(string.Format("{0}\nX:{1}:{2}\nY:{3}:{4}",
                    _level.GetObstacle(x, y).Type.ToString().ToUpper(), x, _level.LevelWidth - 2, y,
                    _level.LevelHeight - 2));

                if (view)
                    SetView(new Vector2f(_windowWidth / 2, _windowHeight / 2),
                        new Vector2f(choice.Position.X - 16, choice.Position.Y + 16));
                else
                    SetView(new Vector2f(_windowWidth, _windowHeight),
                        new Vector2f(choice.Position.X + 16, choice.Position.Y + 16));

                //choice.Color = new Color((byte)rnd.Next(100,255), (byte)rnd.Next(100, 255), (byte)rnd.Next(100, 255));
                //

                if (flag) delay++;
                if (flag && delay > 9)
                {
                    flag = false;
                    delay = 0;
                }

                _level.LevelUpdate(true);

                DrawGame(_chendi, false);
                _window.Draw(choice);
                _window.Draw(position);
                _window.Draw(cover);
                _window.Draw(instructions);
                _window.Display();
                //exit
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    DrawLoadingScreen();
                    _level.SaveLevel();
                    break;
                }
            }

            _window.SetKeyRepeatEnabled(true);
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));
            _menuTheme.Play();
        }

        private void Lottery()
        {
            Clock timer = new Clock();
            SetView(new Vector2f(480f, 270f), new Vector2f(240f, 135f));
            GameMachine gameMachine = new GameMachine(-70, 71, Entity.GameMachineTexture);
            bool isRolling = true;
            bool done = false;
            int time = 50;

            if (_randomizer.Next(100) > 0)
            {
                while (_window.IsOpen && isGame)
                {
                    ResetWindow();

                    AnimateBackground();
                    _window.Draw(_background);

                    if (isRolling && !done && Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    {
                        Creature.sKill.Play();
                        isRolling = false;
                    }

                    if (timer.ElapsedTime.AsMilliseconds() > time && isRolling && !done)
                    {
                        timer.Restart();
                        gameMachine.Roll();
                    }

                    if (timer.ElapsedTime.AsMilliseconds() > time && !isRolling && !done)
                    {
                        timer.Restart();
                        gameMachine.Roll();

                        time += 20;
                        if (time > 500)
                        {
                            done = true;
                            _chendi.sCoin.Play();
                            gameMachine.GrantReward(_chendi);
                        }
                    }

                    if (done && timer.ElapsedTime.AsSeconds() > 3)
                    {
                        DrawLoadingScreen();
                        timer.Dispose();
                        return;
                    }

                    gameMachine.GameMachineUpdate();

                    _window.Draw(gameMachine);
                    _window.Display();
                }
            }
        }

        private void DevManip()
        {
            string answer;
            if (DevManipulation)
            {
                Console.WriteLine("Chendi Adventures Manipulation:");

                Console.Write("Play on fullscreen? (y/n)\n>");
                answer = Console.ReadLine();
                if (answer == "y")
                {
                    _windowStyle = Styles.Fullscreen;
                }
                else
                {
                    _windowStyle = Styles.Resize;
                    _windowHeight = 540;
                    _windowWidth = 960;
                }

                //Console.Write("Choose level\n> "); answer = Console.ReadLine();
            }
        }
    }
}