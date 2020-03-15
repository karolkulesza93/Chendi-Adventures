using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

/*

 PRACA INŻYNIERSKA - KAROL KULESZA
 Temat: Realizacja dwuwymiarowej gry platformowej z użyciem biblioteki SFML
 Promotor: dr Piotr Jastrzębski


PROPOZYCJE DO ZROBIENIA:
- generowanie poziomu***
- jak sie uda to shadery/swiatlo ogarnac aby ladnie wygladalo**
- po smierci resp kilka sekund przed*

DO ZROBIENIA:
- levele
- scenka na poczatek i koniec

*/

namespace ChendiAdventures
{
    public sealed class MainGameWindow
    {
        private static MainGameWindow _instance;
        private static readonly object Padlock = new object();
        public static MainGameWindow Instance
        {
            get
            {
                if (_instance == null)
                    lock (Padlock)
                    {
                        if (_instance == null)
                            _instance = new MainGameWindow("Chendi Adventures");
                    }

                return _instance;
            }
        }

        private readonly View _view;
        private readonly RenderWindow _window;
        public static readonly Random Randomizer = new Random();
        private Sprite _background;
        private ScreenChange _screenChange;
        private MainCharacter _chendi;
        private MainCharacterUI _chendiUi;
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
        public static Sound Victory = new Sound(new SoundBuffer(@"sfx/victory.wav"));
        public static Sound sChoice = new Sound(new SoundBuffer(@"sfx/choice.wav"));
        private int _windowHeight;
        private Styles _windowStyle = Styles.Fullscreen;
        private int _windowWidth;
        private bool _isGame;
        private bool _isLevelSelection;
        private bool _isHighscore;
        private bool _isMenu;
        private bool _isPaused;
        private bool _isQuit;
        private bool _isSettigs;
        public string Title { get; set; }
        public static Difficulty GameDifficulty { get; set; }
        public static bool IsVsync { get; set; }

        private MainGameWindow()
        {
            _isMenu = true;
            _isGame = false;
            _isLevelSelection = false;
            _isHighscore = false;
            _isSettigs = false;
            _isQuit = false;
            _isPaused = false;
        }

        private MainGameWindow(string title) : this()
        {
            Title = title;
            _window = new RenderWindow(VideoMode.DesktopMode, Title, _windowStyle);
            _windowWidth = (int)_window.Size.X;
            _windowHeight = (int) _window.Size.Y;
            _window.SetFramerateLimit(60);
            _window.SetVisible(true);

            //events
            _window.Closed += OnClosed;
            _window.KeyPressed += OnKeyPress;
            //

            _window.SetMouseCursorVisible(false);
            _window.SetKeyRepeatEnabled(false);
            _window.SetVerticalSyncEnabled(true);
            _view = new View(new FloatRect(0, 0, _windowWidth, _windowHeight));
            _screenChange = new ScreenChange(ref _view);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _window.Close();
            _isMenu = false;
            _isGame = false;
            _isHighscore = false;
            _isSettigs = false;
            _isQuit = false;
        }

        [Obsolete]
        private void OnKeyPress(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
            {
                Image img = _window.Capture();
                img.SaveToFile(string.Format(@"screenshots/img_{0}.png", DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")));
            }
        }
        
        public void GameStart()
        {

            _loading = new TextLine("LOADING...", 50, 1390, 990, new Color(150,150,150));
            _loading.SetOutlineThickness(2);
            _loading.SetOutlineColor(new Color(50,50,50));
            var tmp = new Texture("img/tiles.png", new IntRect(new Vector2i(32, 0), new Vector2i(32, 32)));
            tmp.Repeated = true;

            _background = new Sprite(tmp);
            _background.Scale = new Vector2f(2f, 2f);
            _background.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(1000, 600));

            _gameLogo = new Sprite(new Texture(@"img/logo.png"));
            _gameLogo.Position = new Vector2f((_windowWidth - _gameLogo.Texture.Size.X) / 2, -300);

            DrawLoadingScreen();
            LoadContents();
            MainLoop();
        }

        private void ProductionInfoLoop() //do poprawy/zrobienia
        {
            TextLine author = new TextLine("KAROL KULESZA XD PRODUCTIONS", 50, _view.Center.X - 500, _view.Center.Y - 50, Color.White);

            while (_window.IsOpen)
            {
                ResetWindow();
                _screenChange.AppearIn();

                _window.Draw(author);
                _window.Display();
            }
        }

        private void LoadContents()
        {
            _menuTheme = new Music("sfx/menutheme.wav");
            _menuTheme.Loop = true;
            _menuTheme.Volume = 40;

            _mainTheme = new Music("sfx/main theme.wav");
            _mainTheme.Loop = true;
            _mainTheme.Volume = 30;

            _quitQestion = new TextLine("QUIT GAME?", 50, 265, 230, new Color(150,150,150));
            _quitQestion.SetOutlineThickness(5f);
            _yes = new TextLine("YES", 50, 435, 300, Color.White);
            _yes.SetOutlineThickness(5f);
            _no = new TextLine("NO", 50, 460, 370, Color.Green);
            _no.SetOutlineThickness(5f);

            Victory.Volume = 50;
            _gameEnd = new Sound(new SoundBuffer(@"sfx/gameover.wav"));
            _gameEnd.Volume = 50;

            _gameOver = new TextLine("GAME OVER", 100, 500, 470, Color.Red);
            _gameOver.SetOutlineThickness(3f);
            _pause = new TextLine("PAUSE", 50, 0, 0, Color.Yellow);
            _pause.SetOutlineThickness(5f);
            _continue = new TextLine("CONTINUE?", 50, 650, 590, Color.Yellow);
            _continue.SetOutlineThickness(5f);

            _chendi = new MainCharacter(-100, -100, Entity.MainCharacterTexture);
            _level = new Level(_chendi, _view);
            _chendiUi = new MainCharacterUI(_chendi, _view, _level);

            _highscoreValues = new Highscores();
            LoadSettings();
        }

        private void MainLoop()
        {
            while (_isGame || _isLevelSelection || _isMenu || _isHighscore || _isSettigs)
            {
                if (_isMenu) MainMenuLoop();
                if (_isLevelSelection) LevelSelectionLoop();
                if (_isGame) GameLoop();
                if (_isHighscore) HighScoreLoop();
                if (_isSettigs) SettingsLoop();
                if (_isQuit) break;
            }

            _window.Close();
            Console.Clear();
            Environment.Exit(0);
        }

        private void LoadSettings()
        {
            switch (Settings.Default.Difficulty)
            {
                case 1: { GameDifficulty = Difficulty.Easy; break; }
                case 2: { GameDifficulty = Difficulty.Medium; break; }
                case 3: { GameDifficulty = Difficulty.Hard; break; }
            }

            IsVsync = Settings.Default.Vsync;
            _window.SetVerticalSyncEnabled(IsVsync);

            //key bindings
        }

        private void SaveSettings()
        {
            Settings.Default.Difficulty = (int)GameDifficulty;

            Settings.Default.Vsync = IsVsync;
            _window.SetVerticalSyncEnabled(IsVsync);


            Settings.Default.Save();
        }

        private void SettingsLoop()
        {
            //resolution - auto
            var resolution = new TextLine("RESOLUTION: 1920x1080", 50, -1000, _view.Center.Y + _view.Size.Y / 2 - 290, Color.Green); resolution.SetOutlineThickness(5);
            var vsync = new TextLine($"VSYNC: {(IsVsync ? "ON" : "OFF")}", 50, -1100, _view.Center.Y + _view.Size.Y / 2 - 230, Color.White); vsync.SetOutlineThickness(5);
            var difficulty = new TextLine("DIFFICULTY: MEDIUM", 50, -1200, _view.Center.Y + _view.Size.Y / 2 - 170, Color.White); difficulty.SetOutlineThickness(5);
            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    difficulty.EditText("DIFFICULTY: EASY");
                    break;
                }
                case Difficulty.Medium:
                {
                    difficulty.EditText("DIFFICULTY: MEDIUM");
                        break;
                }
                case Difficulty.Hard:
                {
                    difficulty.EditText("DIFFICULTY: HARD");
                        break;
                }
            }
            var keyBindings = new TextLine("KEY BINDINGS", 50, -1300, _view.Center.Y + _view.Size.Y / 2 - 110, Color.White); keyBindings.SetOutlineThickness(5);


            var choice = 1;
            var delay = 0;
            var flag = false;

            
            while (_window.IsOpen && _isSettigs)
            {
                ResetWindow();

                // na pewno poziom trudnosci
                // controllsy jak sie bedzie chcialo
                // moze pobawic sie rozdzielczoscia

                //slide text effect
                if (resolution.X < 50) { resolution.MoveText(resolution.X + 50, resolution.Y); }
                if (vsync.X < 50) { vsync.MoveText(vsync.X + 50, vsync.Y); }
                if (difficulty.X < 50) { difficulty.MoveText(difficulty.X + 50, difficulty.Y); }
                if (keyBindings.X < 50) { keyBindings.MoveText(keyBindings.X + 50, keyBindings.Y);}

                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }

                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP)))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    switch (choice)
                    {
                        case 1: //resolution
                        {
                            break;
                        }
                        case 2: //vsync
                        {
                            IsVsync = !IsVsync;
                            vsync.EditText($"VSYNC: {(IsVsync ? "ON" : "OFF")}");
                            break;
                        }
                        case 3: // difficulty
                        {
                            GameDifficulty++;
                            if (GameDifficulty == (Difficulty)4) GameDifficulty = (Difficulty)1;

                            switch (GameDifficulty)
                            {
                                case Difficulty.Easy:
                                {
                                    difficulty.EditText("DIFFICULTY: EASY");
                                    break;
                                }
                                case Difficulty.Medium:
                                {
                                    difficulty.EditText("DIFFICULTY: MEDIUM");
                                    break;
                                }
                                case Difficulty.Hard:
                                {
                                    difficulty.EditText("DIFFICULTY: HARD");
                                    break;
                                }
                            }

                                break;
                        }
                        case 4: //key bindings
                        {
                            KeyBindingConfig();
                            break;
                        }
                    }
                }

                if (choice == 0) choice = 4;
                if (choice == 5) choice = 1;

                resolution.ChangeColor(new Color(150,150, 150));
                vsync.ChangeColor(Color.White);
                difficulty.ChangeColor(Color.White);
                keyBindings.ChangeColor(Color.White);

                switch (choice)
                {
                    case 1:
                    {
                        resolution.ChangeColor(Color.Green);
                        break;
                    }
                    case 2:
                    {
                        vsync.ChangeColor(Color.Green);
                        break;
                    }
                    case 3:
                    {
                        difficulty.ChangeColor(Color.Green);
                        break;
                    }
                    case 4:
                    {
                        keyBindings.ChangeColor(Color.Green);
                        break;
                    }
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER))
                {
                    _chendi.sPickup.Play();
                    SaveSettings();
                    _isSettigs = false;
                    _isMenu = true;
                }

                AnimateBackground();
                _window.Draw(_background);

                //draw settings
                _window.Draw(resolution);
                _window.Draw(vsync);
                _window.Draw(difficulty);
                _window.Draw(keyBindings);

                _window.Display();
            }
        }

        private void KeyBindingConfig()
        {
            TextLine keys = new TextLine("", 50, -1000, _view.Center.Y + _windowHeight/2 - 500, new Color(150,150,150));
            keys.SetOutlineThickness(5);

            StringBuilder str = new StringBuilder();
            str.Append($"'{MainCharacter.KeyLEFT.ToString().ToUpper()}' : MOVE LEFT\n");
            str.Append($"'{MainCharacter.KeyRIGHT.ToString().ToUpper()}' : MOVE RIGHT\n");
            str.Append($"'{MainCharacter.KeyUP.ToString().ToUpper()}' : ACTION OR LOOK UP\n");
            str.Append($"'{MainCharacter.KeyJUMP.ToString().ToUpper()}' : JUMP\n");
            str.Append($"'{MainCharacter.KeyATTACK.ToString().ToUpper()}' : SWORD ATTACK\n");
            str.Append($"'{MainCharacter.KeyARROW.ToString().ToUpper()}' : SHOOT AN ARROW\n");
            str.Append($"'{MainCharacter.KeyTHUNDER.ToString().ToUpper()}' : SHOOT AN ENERGIZED ARROW\n");
            str.Append($"'{MainCharacter.KeyIMMORTALITY.ToString().ToUpper()}' : BECOME IMMORTAL\n");
            str.Append($"'{MainCharacter.KeyDIE.ToString().ToUpper()}' : KILL YOURSELF");

            keys.EditText(str.ToString());

            while (_window.IsOpen && _isSettigs)
            {
                ResetWindow();

                //slide text effect
                if (keys.X < 50) keys.MoveText(keys.X + 50, keys.Y);

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER)) break;
                AnimateBackground();
                _window.Draw(_background);

                //draw settings
                _window.Draw(keys);
                _window.Display();
            }
        }

        private void HighScoreLoop()
        {
            var hs = new TextLine("HIGHSCORES", 50, _view.Center.X - 250, -400, Color.Green); hs.SetOutlineThickness(5);
            _highscoreValues.X = -900;

            while (_window.IsOpen && _isHighscore)
            {
                ResetWindow();

                //slide text effect
                if (hs.Y < 5) { hs.MoveText(hs.X, hs.Y + 10);}
                if (_highscoreValues.X < _view.Center.X - 750) _highscoreValues.X += 50;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER))
                {
                    _chendi.sPickup.Play();
                    _isHighscore = false;
                    _isMenu = true;
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
            _start = new TextLine("NEW GAME", 50, -500, _view.Center.Y + _view.Size.Y/2 - 290, Color.Green);  //790
            _start.SetOutlineThickness(5);
            _highscores = new TextLine("HIGHSCORES", 50, -600, _view.Center.Y + _view.Size.Y / 2 - 230, Color.White); //850
            _highscores.SetOutlineThickness(5);
            _settings = new TextLine("SETTINGS", 50, -700, _view.Center.Y + _view.Size.Y / 2 - 170, Color.White); //910
            _settings.SetOutlineThickness(5);
            _quit = new TextLine("QUIT", 50, -800, _view.Center.Y + _view.Size.Y / 2 - 110, Color.White); //970
            _quit.SetOutlineThickness(5);

            _gameLogo.Position = new Vector2f((_windowWidth - _gameLogo.Texture.Size.X) / 2, -300);

            if (_menuTheme.Status != SoundStatus.Playing) _menuTheme.Play();

            var choice = 1;
            var delay = 0;
            var flag = false;

            while (_window.IsOpen && _isMenu)
            {
                ResetWindow();

                _screenChange.AppearIn();

                // slide text effect
                if (_start.X < 50) { _start.MoveText(_start.X + 50, _start.Y); }
                if (_highscores.X < 50) { _highscores.MoveText(_highscores.X + 50, _highscores.Y); }
                if (_settings.X < 50) { _settings.MoveText(_settings.X + 50, _settings.Y); }
                if (_quit.X < 50) { _quit.MoveText(_quit.X + 50, _quit.Y); }
                if (_gameLogo.Position.Y < 50) _gameLogo.Position = new Vector2f((_windowWidth - _gameLogo.Texture.Size.X) / 2, _gameLogo.Position.Y + 25);

                //secret keys to enter level editor
                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.LShift) && Keyboard.IsKeyPressed(Keyboard.Key.LAlt) &&
                    Keyboard.IsKeyPressed(Keyboard.Key.L))
                {
                    LevelEditor();
                    _chendi.sPickup.Play();
                    flag = true;
                }
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
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP)))
                {
                    
                    flag = true;
                    _chendi.sPickup.Play();
                    switch (choice)
                    {
                        case 1:
                        {
                            if (Settings.Default.HighestLevel >= 5 && _level.LevelNumber != 0)
                            {
                                _isMenu = false;
                                _isLevelSelection = true;
                            }
                            else
                            {
                                DrawLoadingScreen();
                                Thread.Sleep(200);
                                _isMenu = false;
                                _isGame = true;
                                if (_level.LevelNumber != 0)
                                {
                                    _level.LevelNumber = 1;
                                }
                                _chendi.ResetMainCharacter();
                                _menuTheme.Stop();
                            }
                            break;
                        }
                        case 2:
                        {
                            _isMenu = false;
                            _isHighscore = true;
                            break;
                        }
                        case 3:
                        {
                            _isMenu = false;
                            _isSettigs = true;
                            break;
                        }
                        case 4:
                        {
                            _isMenu = false;
                            _isQuit = true;
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

                if (_isMenu) DrawMainMenu();
            }
        }

        private void LevelSelectionLoop()
        {
            int maxLevel = Settings.Default.HighestLevel;
            List<TextLine> levels = new List<TextLine>();

            for (int i = 0; i <= maxLevel; i += 5)
            {
                levels.Add(new TextLine(string.Format("LEVEL {0}", i == 0 ? 1 : i), 50, -400 - (i/5)*100, (i / 5) * 60, Color.White));
            }

            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].MoveText(levels[i].X, _view.Size.Y - 50 - 60 * (levels.Count - i));
                levels[i].SetOutlineThickness(5);
            }

            var choice = 0;
            var delay = 0;
            var flag = true;

            levels[0].ChangeColor(Color.Green);

            while (_window.IsOpen && _isLevelSelection)
            {
                ResetWindow();

                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }

                //text slide effect
                foreach (var line in levels)
                {
                    if (line.X < 50) line.MoveText(line.X + 50, line.Y);
                    line.ChangeColor(Color.White);
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }

                if (choice < 0) choice = levels.Count - 1;
                if (choice > levels.Count - 1) choice = 0;

                levels[choice].ChangeColor(Color.Green);

                if (!flag && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP)))
                {
                    _chendi.sPickup.Play();
                    DrawLoadingScreen();
                    Thread.Sleep(200);
                    _isLevelSelection = false;
                    _isGame = true;
                    if (_level.LevelNumber != 0)
                    {
                        _level.LevelNumber = choice == 0 ? 1 : choice * 5;
                    }
                    _chendi.ResetMainCharacter();
                    _menuTheme.Stop();
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER))
                {
                    _chendi.sPickup.Play();
                    _isLevelSelection = false;
                    _isMenu = true;
                }


                if (_isLevelSelection)
                {
                    AnimateBackground();
                    _window.Draw(_background);
                    foreach (var line in levels)
                    {
                        _window.Draw(line);
                    }
                    _window.Display();
                }
            }
        }

        private void GameLoop()
        {
            DrawLoadingScreen();

            _levelSummary = new TextLine("", 25, -1000, -1000, Color.White);

            SetView(new Vector2f(_windowWidth/2, _windowHeight/2), _view.Center);
            _screenChange.Reset();

            _level.LoadLevel($"lvl{_level.LevelNumber}");

            _mainTheme.Play();

            while (_window.IsOpen && _isGame)
            {
                if (CheckForGameBreak()) break;
                if (_level.isShopOpened) ShopLoop();

                ResetWindow();

                _chendi.MainCharacterUpdate(_level);
                _level.LevelUpdate();

                ViewManipulation(_level);
                _chendiUi.UpdateUI();

                DrawGame(_chendi, true);

                ExitLoop();
            }

            _mainTheme.Stop();

            _level.ReloadLevelUponDeath();

            GameOverAndAddToHighscore();
            
            ProceedToNextLevel();
        }

        private void PauseLoop()
        {
            _pause.MoveText(_view.Center.X - 700, _view.Center.Y - 25);

            while (_window.IsOpen && _isPaused)
            {
                ResetWindow();

                //slide text effect
                if (_pause.X < _view.Center.X - 100) { _pause.MoveText(_pause.X + 30, _pause.Y);}

                this.DrawGame(_chendi, false);
                _window.Draw(_pause);
                _window.Display();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER) || Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    _chendi.sPickup.Play();
                    _isPaused = false;
                    Thread.Sleep(200);
                }
            }
        }

        private void ResetWindow()
        {
            _window.DispatchEvents();
            _window.Clear(Color.Black);
        }

        private void DrawLoadingScreen()
        {
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));
            ResetWindow();

            _loading.MoveText(_view.Center.X +_view.Size.X/2 - _loading.Width*1.3f, _view.Center.Y + _view.Size.Y / 2 - 70);

            _window.Draw(_loading);
            _window.Display();
        }

        private void DrawGameOver()
        {
            var clock = new Clock();
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));


            if (_chendi.Continues == 0)
            {
                _isGame = false;
                _isMenu = true;
                _chendi.Continues = 2;
                _level.LevelNumber = 1;
                _chendi.ResetMainCharacter();
                _screenChange.Reset();

                while (clock.ElapsedTime.AsSeconds() < 8f)
                {
                    ResetWindow();
                    AnimateBackground();

                    _screenChange.AppearIn();

                    _window.Draw(_background);
                    _window.Draw(_gameOver);
                    _window.Draw(_screenChange);

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
                    _screenChange.AppearIn();

                    _continue.EditText(string.Format("CONTINUE? {0}", 10 - (int)clock.ElapsedTime.AsSeconds()));

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP))
                    {
                        _chendi.sPickup.Play();
                        _chendi.ResetMainCharacter();
                        _chendi.Continues--;
                        _gameEnd.Stop();
                        DrawLoadingScreen();
                        return;
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER))
                    {
                        _chendi.sPickup.Play();
                        _chendi.ResetMainCharacter();
                        _gameEnd.Stop();
                        DrawLoadingScreen();
                        _isGame = false;
                        _isMenu = true;
                        return;
                    }

                    _window.Draw(_gameOver);
                    _window.Draw(_continue);
                    _window.Draw(_screenChange);
                    _window.Display();
                }
                _isGame = false;
                _isMenu = true;
            }

            clock.Dispose();
        }

        private void DrawGame(MainCharacter character, bool display, params Entity[] entities)
        {
            _window.Draw(_level);
            _window.Draw(character);
            foreach (var entity in entities) _window.Draw(entity);
            _window.Draw(_chendiUi);
            _window.Draw(_screenChange);
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

            _window.Draw(_screenChange);
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

            if (!_screenChange.Done) _screenChange.AppearIn();
        }

        private void SetView(Vector2f size, Vector2f center)
        {
            _view.Size = size;
            _view.Center = center;
            _window.SetView(_view);
        }

        private bool CheckForGameBreak()
        {
            if (_chendi.IsDead && _chendi.DefaultClock.ElapsedTime.AsSeconds() > 2.5f) _screenChange.BlackOut();

            if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) 
            {
                Creature.sKill.Play();
                _isPaused = true;
                return false;
            }
            if (_chendi.IsDead)
            {
                _mainTheme.Stop();
            }
            if (_chendi.GotExit || _chendi.OutOfLives && !_chendi.IsDead)
            {
                return true;
            }
            if (_chendi.IsDead && _chendi.DefaultClock.ElapsedTime.AsSeconds() > 3)
            {
                return true;
            }
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
            DrawLoadingScreen();
            _menuTheme.Stop();

            if (!File.Exists(@"levels/edit.dat")) return;

            _level.LoadLevel("edit");

            var choice = new Sprite(new Texture(@"img/edit.png"));
            choice.Position = new Vector2f(32, 32);

            var cover = new RectangleShape(new Vector2f(1000, 500));
            cover.FillColor = Color.Black;
            cover.Position = new Vector2f(-201, -520);

            var position = new TextLine("", 10, 0, 0, Color.Yellow);

            TextLine instructions;
            try
            {
                instructions = new TextLine(File.ReadAllText(@"levels/instructions.dat"), 10, -300, 0, Color.White);
            }
            catch (Exception)
            {
                instructions = new TextLine("", 10, -300, 0, Color.White);
            }

            _chendi.SetPosition(-100, -100);
            _chendi.IsDead = true;
            _chendiUi.ResetPositions();

            _window.SetKeyRepeatEnabled(false);

            var x = 1;
            var y = 1;
            var flag = false;
            var delay = 0;

            var view = true;

            BlockType type = 0;
            BlockType typeM = 0;

            var rnd = new Random();

            //tiles 
            int t1 = 1;
            int t2 = 21;
            //pickups
            int p1 = 22;
            int p2 = 32;
            //details
            int d1 = 33;
            int d2 = 39;

            while (_window.IsOpen)
            {
                ResetWindow();
                // moving editor
                if (!flag && x > 1 && Keyboard.IsKeyPressed(Keyboard.Key.Left))
                {
                    x--;
                    flag = true;
                    sChoice.Play();
                }

                if (!flag && x < _level.LevelWidth - 2 && Keyboard.IsKeyPressed(Keyboard.Key.Right))
                {
                    x++;
                    flag = true;
                    sChoice.Play();
                }

                if (!flag && y > 1 && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    y--;
                    flag = true;
                    sChoice.Play();
                }

                if (!flag && y < _level.LevelHeight - 2 && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    y++;
                    flag = true;
                    sChoice.Play();
                }

                type = _level.GetObstacle(x, y).Type;

                //changes - tiles (edit+, edit-, edit_remembered, delete)
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.X))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    type++;
                    if (type == (BlockType) d2+1) type = 0;

                    typeM = type;

                    if (type >= (BlockType)t1 && type <= (BlockType) t2)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (type >= (BlockType) p1 && type <= (BlockType) p2)
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
                    _chendi.sPickup.Play();

                    if (type > 0) type--;
                    else type = (BlockType) d2;

                    typeM = type;

                    if (type >= (BlockType)t1 && type <= (BlockType) t2)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (type >= (BlockType) p1 && type <= (BlockType) p2)
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
                    _chendi.sPickup.Play();

                    typeM = _level.GetObstacle(x, y).Type;
                }



                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.V))
                {
                    flag = true;
                    _chendi.sPickup.Play();


                    if (typeM >= (BlockType) t1 && typeM <= (BlockType) t2)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (typeM >= (BlockType) p1 && typeM <= (BlockType) p2)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.PickupsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.DetailsTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }

                    _level.GetObstacle(x, y).Type = typeM;
                    _level.GetObstacle(x, y).SetBlock(typeM);
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    flag = true;
                    _chendi.sPickup.Play();

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
                    _chendi.sPickup.Play();

                    _level.Monsters.Add(new Monster(x * 32, y * 32, Entity.KnightTexture));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Archers.Add(new Archer(x * 32, y * 32, Entity.ArcherTexture, Movement.Left));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Archers.Add(new Archer(x * 32, y * 32, Entity.ArcherTexture, Movement.Right));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num4))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Ghosts.Add(new Ghost(x * 32, y * 32, Entity.GhostTexture));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num5))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Wizards.Add(new Wizard(x * 32, y * 32, Entity.WizardTexture));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num6))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Golems.Add(new Golem(x * 32, y * 32, Entity.GolemTexture));
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

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F6))
                {
                    flag = true;
                    Creature.sKill.Play();
                    _level.Golems.Clear();
                }

                //changes - traps
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num7))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.Crusher));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num8))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.Spikes));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num9))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.BlowTorchLeft));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Num0))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.BlowTorchRight));
                }

                //clear traps
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F7))
                {
                    flag = true;
                    Creature.sKill.Play();
                    _level.Traps.Clear();
                }
                //mirror image
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F10))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    BlockType typeMirror;

                    for (x = 1; x < (_level.LevelWidth - 2) / 2 + 1; x++)
                    for (y = 1; y < _level.LevelHeight - 1; y++)
                    {
                        typeMirror = _level.GetObstacle(x, y).Type;
                        _level.GetObstacle(_level.LevelWidth - x -1, y).Type = typeMirror;

                        if (typeMirror >= (BlockType)t1 && typeMirror <= (BlockType)t2)
                        {
                            _level.GetObstacle(_level.LevelWidth - x-1, y).LoadedTexture = Entity.TilesTexture;
                            _level.GetObstacle(_level.LevelWidth - x-1, y).UseTexture();
                        }
                        else if (typeMirror >= (BlockType)p1 && typeMirror <= (BlockType)p2)
                        {
                            _level.GetObstacle(_level.LevelWidth - x-1, y).LoadedTexture = Entity.PickupsTexture;
                            _level.GetObstacle(_level.LevelWidth - x-1, y).UseTexture();
                        }
                        else
                        {
                            _level.GetObstacle(_level.LevelWidth - x-1, y).LoadedTexture = Entity.DetailsTexture;
                            _level.GetObstacle(_level.LevelWidth - x-1, y).UseTexture();
                        }

                        _level.GetObstacle(_level.LevelWidth - x-1, y).Type = typeMirror;
                        _level.GetObstacle(_level.LevelWidth - x-1, y).SetBlock(typeMirror);

                    }

                    x = (_level.LevelWidth - 2) / 2 + 1;
                    y = (_level.LevelHeight - 2) / 2 + 1;
                }
                //generate
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F11))
                {
                    //x = 1; y = 1;
                    /*
                    string level = LevelManager.Generator.GenerateLevel("GENERATED_LEVEL", _level.LevelWidth,
                        _level.LevelHeight,
                        _randomizer.Next(2) == 1 ? true : false, _randomizer.Next(2) == 1 ? true : false,
                        _randomizer.Next(2) == 1 ? true : false, _randomizer.Next(2) == 1 ? true : false,
                        _randomizer.Next(2) == 1 ? true : false, _randomizer.Next(2) == 1 ? true : false,
                        _randomizer.Next(2) == 1 ? true : false,
                        _randomizer.Next(2) == 1 ? true : false, _randomizer.Next(2) == 1 ? true : false,
                        _randomizer.Next(2) == 1 ? true : false, _randomizer.Next(2) == 1 ? true : false,
                        _randomizer.Next(2) == 1 ? true : false, _randomizer.Next(2) == 1 ? true : false);
                    _level.LoadLevel(string.Format("GENERATED_LEVEL_{0}x{1}", _level.LevelWidth, _level.LevelHeight));*/
                }
                //clear blocks
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F12))
                {
                    flag = true;
                    Creature.sKill.Play();

                    for (x = 1; x < _level.LevelWidth - 1; x++)
                    for (y = 1; y < _level.LevelHeight - 1; y++)
                    {
                        type = BlockType.None;

                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();

                        _level.GetObstacle(x, y).Type = type;
                        _level.GetObstacle(x, y).SetBlock(type);
                    }

                    x = 1; y = 1;
                }

                //viem manip (+/-)
                if (view && !flag && Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    view = false;
                }

                if (!view && !flag && Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    view = true;
                }

                //left up corner
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.I)) 
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    x = 1;
                    y = 1;
                }
                //right up corner
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.O))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    x = _level.LevelWidth - 2;
                    y = 1;
                }
                //left down corner
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.K))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    x = 1;
                    y = _level.LevelHeight - 2;
                }
                //right down corner
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.L))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    x = _level.LevelWidth - 2;
                    y = _level.LevelHeight - 2;
                }
                //center
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.P))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    x = (_level.LevelWidth - 2)/2 + 1;
                    y = (_level.LevelHeight - 2)/2 + 1;
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

        private void ExitLoop()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                var choice = false;
                var flag = false;
                var delay = 0;

                _quitQestion.MoveText(_view.Center.X -800, _view.Center.Y - 100); //-230
                _yes.MoveText(_view.Center.X -900, _view.Center.Y - 30); //-65
                _no.MoveText(_view.Center.X -1000, _view.Center.Y + 40); //-40

                _yes.ChangeColor(Color.White);
                _no.ChangeColor(Color.Green);

                while (_window.IsOpen && _isGame)
                {
                    ResetWindow();

                    //slide text effect
                    if (_quitQestion.X < _view.Center.X - 230) { _quitQestion.MoveText(_quitQestion.X + 20, _quitQestion.Y);}
                    if (_yes.X < _view.Center.X - 65) { _yes.MoveText(_yes.X + 20, _yes.Y);}
                    if (_no.X < _view.Center.X - 40) { _no.MoveText(_no.X + 20, _no.Y);}

                    if (flag) delay++;
                    if (delay > 10)
                    {
                        delay = 0;
                        flag = false;
                    }

                    if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) ||
                                          Keyboard.IsKeyPressed(Keyboard.Key.Down)))
                    {
                        sChoice.Play();
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
                    else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP)))
                    {
                        flag = true;
                        _chendi.sPickup.Play();

                        if (choice)
                        {
                            DrawLoadingScreen();
                            if (_level.LevelNumber > Settings.Default.HighestLevel)
                            {
                                Settings.Default.HighestLevel = _level.LevelNumber;
                                Settings.Default.Save();
                            }
                            _isGame = false;
                            _isMenu = true;
                            flag = false;
                            _level.LevelNumber = 1;
                            _chendi.ResetMainCharacter();
                            SetView(new Vector2f(_windowWidth,_windowHeight), new Vector2f(_windowWidth/2, _windowHeight/2));
                            Thread.Sleep(300);
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

            if (_isPaused) PauseLoop();
        }

        private void GameOverAndAddToHighscore()
        {
            if (_chendi.OutOfLives) //game over
            {
                _highscoreValues.AddNewRecord(new HighscoreRecord(_chendi.Score, _level.LevelNumber, GameDifficulty.ToString().ToUpper()));
                if (_level.LevelNumber > Settings.Default.HighestLevel)
                {
                    Settings.Default.HighestLevel = _level.LevelNumber;
                    Settings.Default.Save();
                }

                _gameEnd.Play();

                DrawGameOver();
                _chendi.ResetMainCharacter();
                DrawLoadingScreen();
            }
        }

        private void ProceedToNextLevel()
        {
            if (_chendi.GotExit) //level complete
            {
                Victory.Play();
                var time = Math.Round(_level.LevelTime.ElapsedTime.AsSeconds(), 2);
                var bonus = _level.GetBonusForTime(time);

                _chendi.Score += bonus;
                if (_level.LevelNumber == 0) _chendi.Score = 0;

                _levelSummary.EditText(string.Format(
                    "LEVEL COMPLETED!\n" +
                    "TIME: {0}" + " SECONDS\n" +
                    "TIME BONUS: {1}\n" +
                    "SCORE GAINED: {2}\n" +
                    "LIVES LEFT: {3}\n" +
                    "OVERALL SCORE: {4}",
                    time, bonus, _chendi.Score - _level.StartScore - bonus, _chendi.Lives, _chendi.Score));
                _levelSummary.SetOutlineThickness(2);

                _chendiUi.UpdateUI();

                //summary to the center
                _levelSummary.X = _view.Center.X - 1000;
                _levelSummary.Y = _view.Center.Y - 100;
            }

            var timer = new Clock();
            timer.Restart();
            _chendi.sImmortality.Stop();
            bool isLottery;
            if (_level.LevelNumber < 51 && _level.LevelNumber > 15 && Randomizer.Next(100)+1 > 50)
            {
                isLottery = true;
            }
            else if (_level.LevelNumber == 15)
            {
                isLottery = true;
            }
            else
            {
                isLottery = false;
            }

            while (_window.IsOpen && _isGame && _chendi.GotExit) //to next level
            {
                ResetWindow();

                if (_levelSummary.X < _view.Center.X - 250)
                    _levelSummary.MoveText(_levelSummary.X + 35, _levelSummary.Y);

                if (!isLottery && timer.ElapsedTime.AsSeconds() > 5) _screenChange.BlackOut();

                if (Victory.Status != SoundStatus.Playing) _chendi.GrantAdditionalLifeDependingOnScore();

                _chendi.MainCharacterUpdate(_level);
                _level.LevelUpdate();

                DrawGame(_chendi, false);
                _window.Draw(_levelSummary);
                _window.Draw(_screenChange);
                _window.Display();

                if (timer.ElapsedTime.AsSeconds() > 6)
                {
                    _level.LevelNumber++;
                    //lottery
                    if (isLottery)
                    {
                        LotteryLoop();
                    }
                    _chendi.GotExit = false;
                    DrawLoadingScreen();
                    break;
                }
            }
            _screenChange.Reset();
            timer.Dispose();
        }

        private void LotteryLoop()
        {
            Clock clock = new Clock();
            GameMachine gameMachine = new GameMachine(_view.Center.X - _view.Size.X/2 -70, _view.Center.Y + 80, Entity.GameMachineTexture, _view);
            bool isRolling = true;
            bool done = false;
            int time = 50;

            if (Randomizer.Next(100) > 0)
            {
                while (_window.IsOpen && _isGame)
                {
                    ResetWindow();

                    if (isRolling && !done && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP)))
                    {
                        Creature.sKill.Play();
                        isRolling = false;
                    }

                    if (clock.ElapsedTime.AsMilliseconds() > time && isRolling && !done)
                    {
                        clock.Restart();
                        gameMachine.Roll();
                    }

                    if (clock.ElapsedTime.AsMilliseconds() > time && !isRolling && !done)
                    {
                        clock.Restart();
                        gameMachine.Roll();

                        time += 30;
                        if (time > 500)
                        {
                            done = true;
                            gameMachine.GrantReward(_chendi);
                            _chendiUi.UpdateUI();
                        }
                    }

                    if (_chendi.sPickup.Status != SoundStatus.Playing) _chendi.GrantAdditionalLifeDependingOnScore();

                    if (done && clock.ElapsedTime.AsSeconds() > 2) _screenChange.BlackOut();

                    if (done && clock.ElapsedTime.AsSeconds() > 3)
                    {
                        DrawLoadingScreen();
                        clock.Dispose();
                        return;
                    }

                    gameMachine.GameMachineUpdate();
                    _chendi.MainCharacterUpdate(_level);
                    _level.LevelUpdate();

                    DrawGame(_chendi, false);
                    _window.Draw(_levelSummary);
                    _window.Draw(gameMachine);
                    _window.Draw(_screenChange);
                    _window.Display();
                }
            }
        }

        private void ShopLoop()
        {
            var merchant = new Merchant(_view.Center.X - _view.Size.X / 2 - 256, _view.Center.Y - 100, Entity.ShopTexture, _chendi);
            merchant.SetTextureRectanlge(0,0, 256,128);


            var choice = 1;
            var delay = 0;
            var flag = false;

            while (_window.IsOpen && _isGame)
            {
                ResetWindow();

                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }

                

                if (merchant.X < _view.Center.X - 128) merchant.X += 32;
                if (merchant.X > _view.Center.X + _view.Size.X / 2) break;

                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }

                if (choice == 0) choice = 4;
                if (choice == 5) choice = 1;

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP)))
                {
                    merchant.SellWares(choice);
                    flag = true;
                }



                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER))
                {
                    _level.isShopOpened = false;
                }
                if (!_level.isShopOpened) merchant.X += 32;

                _level.LevelUpdate();
                _chendiUi.UpdateUI();
                merchant.ShopUpdate(choice);

                DrawGame(_chendi, false);
                _window.Draw(merchant);
                _window.Display();
            }
        }

    }
}