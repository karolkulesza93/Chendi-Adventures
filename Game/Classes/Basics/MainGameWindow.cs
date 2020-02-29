using System;
using System.Threading;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;

/// <summary>
/// do zrobienia:
/// - poprawa crushera
/// - levele
/// - scenka na poczatek
/// - settingsy (hotkeye?, poziom trudnosci?, rozdzielczosci?, vsync)
/// - sklep
/// - maszynka do losowania
/// - i moze cos jeszcze xd
/// </summary>

namespace Game
{
    public sealed class MainGameWindow
    {
        public static bool DevManipulation = true;
        //fields
        /*signleton field*/
        private static MainGameWindow _instance = null;
        /*signleton field*/
        private static readonly object _padlock = new object();
        private RenderWindow _window;

        private Music _mainTheme;
        private Music _menuTheme;
        private Sound _victory;
        private Sound _gameEnd;

        private bool isMenu;
        private bool isGame;
        private bool isPaused;
        private bool isHighscore;
        private bool isSettigs;
        private bool isQuit;

        private Sprite _background;
        private Sprite _gameLogo;

        //loading, summary, highscores
        private TextLine _pause;
        private TextLine _loading;
        private TextLine _gameOver;
        private TextLine _levelSummary;
        private Highscores _highscoreValues;

        //game window
        private MainCahracter _chendi;
        private MainCharacterUI _chendiUI;
        private Level _level;
        //quit game play
        private TextLine _quitQestion;
        private TextLine _yes;
        private TextLine _no;

        //main manu
        private TextLine _start;
        private TextLine _highscores;
        private TextLine _settings;
        private TextLine _quit;

        //window size (1920 x 1080)
        private int _windowWidth = 1920;
        private int _windowHeight = 1080;
        private Styles _windowStyle = Styles.Fullscreen;
        private View _view;
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
        //contructors
        public MainGameWindow()
        {
            this.isMenu = true;
            this.isGame = false;
            this.isHighscore = false;
            this.isSettigs = false;
            this.isQuit = false;
            this.isPaused = false;
        }
        public MainGameWindow(string title) : this()
        {
            this.DevManip();

            this.Title = title;
            this._window = new RenderWindow(new VideoMode((uint)this._windowWidth, (uint)this._windowHeight), this.Title, this._windowStyle);
            this._window.SetFramerateLimit(60);
            this._window.SetVisible(true);
            this._window.Closed += new EventHandler(OnClosed);
            this._window.SetMouseCursorVisible(false);
            this._window.SetKeyRepeatEnabled(false);
            this._window.SetVerticalSyncEnabled(true);
            this._view = new View(new FloatRect(0, 0, this._windowWidth, this._windowHeight));
        }
        //methods
        public void GameStart()
        {
            this._loading = new TextLine("LOADING...", 50, 1390, 990, Color.White);
            Texture tmp = new Texture("img/tiles.png", new IntRect(new Vector2i(32, 0), new Vector2i(32, 32)));
            tmp.Repeated = true;

            this._background = new Sprite(tmp);
            this._background.Scale = new Vector2f(2f, 2f);
            this._background.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(1000, 600));

            this._gameLogo = new Sprite(new Texture(@"img/logo.png"));
            this._gameLogo.Position = new Vector2f((this._windowWidth - this._gameLogo.Texture.Size.X) / 2, 50);

            this.DrawLoadingScreen();

            this.LoadContents();
            this.MainLoop();
        }
        private void OnClosed(object sender, EventArgs e)
        {
            this._window.Close();
            this.isMenu = false;
            this.isGame = false;
            this.isHighscore = false;
            this.isSettigs = false;
            this.isQuit = false;
        }
        private void LoadContents()
        {
            this._menuTheme = new Music("sfx/menutheme.wav");
            this._menuTheme.Loop = true;
            this._menuTheme.Volume = 40;

            this._mainTheme = new Music("sfx/main theme.wav");
            this._mainTheme.Loop = true;
            this._mainTheme.Volume = 30;

            this._quitQestion = new TextLine("QUIT GAME?", 50, 265, 230, Color.White); this._quitQestion.SetOutlineThickness(2f);
            this._yes = new TextLine("YES", 50, 435, 300, Color.White); this._yes.SetOutlineThickness(2f);
            this._no = new TextLine("NO", 50, 460, 370, Color.Green); this._no.SetOutlineThickness(2f);

            this._victory = new Sound(new SoundBuffer(@"sfx/victory.wav"));
            this._victory.Volume = 50;
            this._gameEnd = new Sound(new SoundBuffer(@"sfx/gameover.wav"));
            this._gameEnd.Volume = 50;

            this._gameOver = new TextLine("GAME OVER", 100, 500, 470, Color.Red); this._gameOver.SetOutlineThickness(3f);
            this._pause = new TextLine("PAUSE", 50, 0, 0, Color.Yellow);

            this._chendi = new MainCahracter(-100, -100, Entity.MainCharacterTexture);
            this._level = new Level(this._chendi);
            this._chendiUI = new MainCharacterUI(this._chendi, this._view, this._level);

            this._highscoreValues = new Highscores();
        }
        private void MainLoop()
        {
            while (this.isGame || this.isMenu || this.isHighscore || this.isSettigs)
            {
                if (this.isMenu) this.MainMenuLoop();
                if (this.isGame) this.GameLoop();
                if (this.isHighscore) this.HighScoreLoop();
                if (this.isSettigs) this.SettingsLoop();
                if (this.isQuit) break;
            }
            _window.Close();
            Console.Clear();
            Environment.Exit(0);
        }
        private void SettingsLoop()
        {
            TextLine Resolution = new TextLine("RESOLUTION: 1920x1080", 50, 50, 790, Color.Green);
            TextLine Vsync = new TextLine("VSYNC: ON", 50, 50, 850, Color.White);
            TextLine Difficulty = new TextLine("DIFFICULTY: MEDIUM", 50, 50, 910, Color.White);
            TextLine KeyBindings = new TextLine("KEY BINDINGS", 50, 50, 970, Color.White);

            int choice = 1;
            int delay = 0;
            bool flag = false;

            while (this._window.IsOpen && this.isSettigs)
            {
                this.ResetWindow();

                // na pewno poziom trudnosci
                // controllsy jak sie bedzie chcialo
                // moze pobawic sie rozdzielczoscia

                if (flag == true) delay++;
                if (delay > 10) { delay = 0; flag = false; }

                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Up)) { this._chendi.sAtk.Play(); flag = true; choice--; }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Down)) { this._chendi.sAtk.Play(); flag = true; choice++; }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    flag = true;
                    this._chendi.sCoin.Play();
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
                    case 1: { Resolution.ChangeColor(Color.Green); break; }
                    case 2: { Vsync.ChangeColor(Color.Green); break; }
                    case 3: { Difficulty.ChangeColor(Color.Green); break; }
                    case 4: { KeyBindings.ChangeColor(Color.Green); break; }
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    this.isSettigs = false;
                    this.isMenu = true;
                }
                this.AnimateBackground();
                this._window.Draw(this._background);

                //draw settings
                this._window.Draw(Resolution);
                this._window.Draw(Vsync);
                this._window.Draw(Difficulty);
                this._window.Draw(KeyBindings);

                this._window.Display();
            }
        }
        private void KeyBindingConfig()
        {
            while (this._window.IsOpen && this.isSettigs)
            {
                this.ResetWindow();
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

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    break;
                }
                this.AnimateBackground();
                this._window.Draw(this._background);


                //draw settings
                this._window.Display();
            }
        }
        private void HighScoreLoop()
        {
            TextLine hs = new TextLine("HIGHSCORES", 50, 700, 5, Color.Green);
            while (this._window.IsOpen && this.isHighscore)
            {
                this.ResetWindow();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    this.isHighscore = false;
                    this.isMenu = true;
                }

                this.AnimateBackground();
                this._window.Draw(this._background);

                this._window.Draw(this._highscoreValues);
                this._window.Draw(hs);
                this._window.Display();
            }
        }
        private void MainMenuLoop()
        {
            this._start = new TextLine("NEW GAME", 50, 50, 790, Color.Green);
            this._highscores = new TextLine("HIGHSCORES", 50, 50, 850, Color.White);
            this._settings = new TextLine("SETTINGS", 50, 50, 910, Color.White);
            this._quit = new TextLine("EXIT", 50, 50, 970, Color.White);

            if (this._menuTheme.Status != SoundStatus.Playing) this._menuTheme.Play();
            this.DrawLoadingScreen();

            int choice = 1;
            int delay = 0;
            bool flag = false;

            while (this._window.IsOpen && this.isMenu)
            {
                this.ResetWindow();

                //secret keys to enter level editor
                if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) && Keyboard.IsKeyPressed(Keyboard.Key.LAlt) && Keyboard.IsKeyPressed(Keyboard.Key.L))
                    this.LevelEditor();
                //

                this._start.ChangeColor(Color.White);
                this._highscores.ChangeColor(Color.White);
                this._settings.ChangeColor(Color.White);
                this._quit.ChangeColor(Color.White);

                if (flag == true) delay++;
                if (delay > 10) { delay = 0; flag = false; }

                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Up)) { this._chendi.sAtk.Play(); flag = true; choice--; }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Down)) { this._chendi.sAtk.Play(); flag = true; choice++; }
                else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    flag = true;
                    this._chendi.sCoin.Play();
                    switch (choice)
                    {
                        case 1: { this.isMenu = false; this.isGame = true; this._menuTheme.Stop(); break; }
                        case 2: { this.isMenu = false; this.isHighscore = true; break; }
                        case 3: { this.isMenu = false; this.isSettigs = true; break; }
                        case 4: { this.isMenu = false; this.isQuit = true; break; }
                    }
                }

                if (choice == 0) choice = 4;
                if (choice == 5) choice = 1;

                switch (choice)
                {
                    case 1: { this._start.ChangeColor(Color.Green); break; }
                    case 2: { this._highscores.ChangeColor(Color.Green); break; }
                    case 3: { this._settings.ChangeColor(Color.Green); break; }
                    case 4: { this._quit.ChangeColor(Color.Green); break; }
                }

                this.DrawMainMenu();
            }
        }
        private void GameLoop()
        {
            this.DrawLoadingScreen();

            this._levelSummary = new TextLine("", 25, -1000, -1000, Color.White);

            this.SetView(new Vector2f(960f, 540f), new Vector2f(480f, 270f));

            if (this._level.LevelNumber == 1) this.BegginingScene();
            /// SET LEVEL FOR TESTING
            //this._level.LevelNumber = 17;
            ///

            this._level.LoadLevel(string.Format("lvl{0}", this._level.LevelNumber));

            this._chendi.SetStartingPosition(this._level);

            this._level.StartScore = this._chendi.Score;
            this._level.StartCoins = this._chendi.Coins;
            this._level.StartArrows = this._chendi.ArrowAmount;
            this._level.StartMana = this._chendi.Mana;

            this._chendi.HasSilverKey = false;
            this._chendi.HasGoldenKey = false;

            this._mainTheme.Play();

            while (this._window.IsOpen && this.isGame)
            {
                if (this.CheckForGameBreak()) break;

                this.ResetWindow();

                this._chendi.MainCharactereUpdate(this._level);
                this._level.LevelUpdate();

                this.ViewManipulation(this._level);
                this._chendiUI.UpdateUI();

                this.DrawGame(this._chendi, true);

                if (this._chendi.IsDead) this._mainTheme.Stop();

                //pause and exit
                if (Keyboard.IsKeyPressed(Keyboard.Key.P)) this.isPaused = true;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    bool choice = false;
                    bool flag = false;
                    int delay = 0;

                    this._quitQestion.MoveText(this._view.Center.X - 230, this._view.Center.Y - 100);
                    this._yes.MoveText(this._view.Center.X - 65, this._view.Center.Y + 20);
                    this._no.MoveText(this._view.Center.X - 40, this._view.Center.Y + 90);

                    while (this._window.IsOpen && this.isGame)
                    {
                        this.ResetWindow();

                        if (flag == true) delay++;
                        if (delay > 10) { delay = 0; flag = false; }

                        if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed(Keyboard.Key.Down)))
                        {
                            this._chendi.sAtk.Play();
                            flag = true; choice = !choice;
                            if (choice)
                            {
                                this._yes.ChangeColor(Color.Green);
                                this._no.ChangeColor(Color.White);
                            }
                            else
                            {
                                this._yes.ChangeColor(Color.White);
                                this._no.ChangeColor(Color.Green);
                            }
                        }
                        else if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.Space))
                        {
                            flag = true;
                            this._chendi.sCoin.Play();

                            if (choice)
                            {
                                this.isGame = false;
                                this.isMenu = true;
                                this._level.LevelNumber = 1;
                                this._chendi.ResetMainCharacter();
                                Thread.Sleep(500);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }

                        this.DrawGame(this._chendi, false);

                        this._window.Draw(_quitQestion);
                        this._window.Draw(_yes);
                        this._window.Draw(_no);
                        this._window.Display();
                    }
                }

                if (this.isPaused) PauseLoop();
            }
            this._mainTheme.Stop();

            if (!this._chendi.OutOfLives && this._chendi.IsDead) //death
            {
                this.DrawLoadingScreen();
                this._chendi.Score = this._level.StartScore;
                this._chendi.ArrowAmount = this._level.StartArrows;
                this._chendi.Mana = this._level.StartMana;
                this._chendi.Coins = this._level.StartCoins;
                this._chendi.Respawn(this._level);
                this._level.LoadLevel(string.Format("lvl{0}", this._level.LevelNumber));
            }

            if (this._chendi.OutOfLives) //game over
            {
                this._highscoreValues.AddNewRecord(new HighscoreRecord(this._chendi.Score, this._level.LevelNumber));

                this._gameEnd.Play();
                this.DrawGameOver();
                this._level.LevelNumber = 1;
                this.isGame = false;
                this.isMenu = true;
                this._chendi.ResetMainCharacter();
                this.DrawLoadingScreen();
            }

            if (this._chendi.GotExit) //level complete
            {
                this._victory.Play();
                //set level summary
                double time = Math.Round(this._level.LevelTime.ElapsedTime.AsSeconds(), 2);
                int bonus = this._level.GetBonusForTime(time);

                this._chendi.Score += bonus;

                this._levelSummary.EditText(String.Format(
                    "LEVEL COMPLETED!\n" +
                    "TIME: {0}" + " SECONDS\n" +
                    "TIME BONUS: {1}\n" +
                    "SCORE GAINED: {2}\n" +
                    "LIVES LEFT: {3}\n" +
                    "OVERALL SCORE: {4}",
                time, bonus, this._chendi.Score - this._level.StartScore - bonus, this._chendi.Lives, this._chendi.Score));
                this._levelSummary.SetOutlineThickness(2);

                //summary to the center
                this._levelSummary.X = this._view.Center.X - 1000;
                this._levelSummary.Y = this._view.Center.Y - 100;

                //draw summary
                this._chendi.SetTextureRectanlge(96, 96, 32, 32);

            }

            Clock timer = new Clock();
            timer.Restart();
            this._chendi.sImmortality.Stop();
            while (this._window.IsOpen && this.isGame && this._chendi.GotExit) //to next level
            {
                this.ResetWindow();

                if (this._levelSummary.X < this._view.Center.X - 250)
                    this._levelSummary.MoveText(this._levelSummary.X + 35, this._levelSummary.Y);

                this.DrawGame(this._chendi, false);
                this._window.Draw(this._levelSummary);
                this._window.Display();

                if (timer.ElapsedTime.AsSeconds() > 6)
                {
                    this._level.LevelNumber++;
                    this._chendi.GotExit = false;
                    this.DrawLoadingScreen();
                    break;
                }
            }
            timer.Dispose();
        }
        private void PauseLoop()
        {
            this._pause.MoveText(this._view.Center.X - 100, this._view.Center.Y - 25);

            while (this._window.IsOpen && this.isPaused)
            {
                this.ResetWindow();
                //this.DrawGame(this._chendi);
                this._window.Draw(this._pause);
                this._window.Display();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) this.isPaused = false;
            }
        }
        public void ResetWindow()
        {
            this._window.DispatchEvents();
            this._window.Clear(Color.Black);
        }
        private void DrawLoadingScreen()
        {
            this.SetView(new Vector2f(this._windowWidth, this._windowHeight), new Vector2f(this._windowWidth / 2, this._windowHeight / 2));
            this.ResetWindow();



            this._window.Draw(this._loading);
            this._window.Display();
        }
        private void DrawGameOver()
        {
            Clock clock = new Clock();
            this.SetView(new Vector2f(this._windowWidth, this._windowHeight), new Vector2f(this._windowWidth / 2, this._windowHeight / 2));
            while (clock.ElapsedTime.AsSeconds() < 5)
            {
                this.ResetWindow();

                this.AnimateBackground();
                this._window.Draw(this._background);

                this._window.Draw(this._gameOver);
                this._window.Display();
            }
            clock.Dispose();
        }
        private void DrawGame(MainCahracter character, bool display, params Entity[] entities)
        {
            _window.Draw(this._level);
            _window.Draw(character);
            foreach (var entity in entities)
            {
                _window.Draw(entity);
            }
            this._window.Draw(_chendiUI);
            if (display) this._window.Display();
        }
        private void DrawMainMenu()
        {
            this.AnimateBackground();
            this._window.Draw(this._background);
            this._window.Draw(this._gameLogo);

            this._window.Draw(this._start);
            this._window.Draw(this._highscores);
            this._window.Draw(this._settings);
            this._window.Draw(this._quit);
            this._window.Display();

        }
        private void ViewManipulation(Level level)
        {
            float x = this._view.Center.X;
            float y = this._view.Center.Y;

            x += (this._chendi.GetCenterPosition().X - this._view.Center.X) / 10;
            if (x - this._view.Size.X / 2 <= 0) x = this._view.Size.X / 2;
            else if (x + this._view.Size.X / 2 >= level.LevelWidth * 32) x = level.LevelWidth * 32 - this._view.Size.X / 2;

            y += (this._chendi.GetCenterPosition().Y - this._view.Center.Y) / 10;
            if (y - this._view.Size.Y / 2 <= 0) y = this._view.Size.Y / 2;
            else if (y + this._view.Size.Y / 2 >= level.LevelHeight * 32) y = level.LevelHeight * 32 - this._view.Size.Y / 2;


            this._view.Center = new Vector2f(x, y);
            this._window.SetView(this._view);
        }
        private void SetView(Vector2f size, Vector2f center)
        {
            this._view.Size = size;
            this._view.Center = center;
            this._window.SetView(this._view);
        }
        private bool CheckForGameBreak()
        {
            if (this._chendi.GotExit || (this._chendi.OutOfLives && !this._chendi.IsDead)) return true;
            if (this._chendi.IsDead && this._chendi.DefaultClock.ElapsedTime.AsSeconds() > 3) return true;
            else return false;
        }
        private void AnimateBackground()
        {
            if (this._background.Position.X < -32 && this._background.Position.Y < -32)
                this._background.Position = new Vector2f(0, 0);
            else this._background.Position = new Vector2f(this._background.Position.X - 0.2f, this._background.Position.Y - 0.2f);
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
            this._menuTheme.Stop();
            this._level.LoadLevel(string.Format("edit", this._level.LevelNumber));
            
            Sprite choice = new Sprite(new Texture(@"img/edit.png"));
            choice.Position = new Vector2f(32, 32);
            
            RectangleShape cover = new RectangleShape(new Vector2f(1000, 500));
            cover.FillColor = Color.Black;
            cover.Position = new Vector2f(-201, -501);

            TextLine warning = new TextLine("TRAPS AND MONSTERS CAN ONLY BE ADDED MANUALLY USING TEXT EDITOR", 10, 0, -20, Color.Magenta);
            TextLine position = new TextLine("", 10, 0, 0, Color.Yellow);

            this._chendi.SetPosition(-100, -100);
            this._chendiUI.ResetPositions();

            this._window.SetKeyRepeatEnabled(false);

            int x = 1;
            int y = 1;
            bool flag = false;
            int delay = 0;

            bool view = true;

            BlockType type = 0;

            Random rnd = new Random();

            while (this._window.IsOpen)
            {
                this.ResetWindow();
                // moving editor
                if (!flag && x >1 && Keyboard.IsKeyPressed(Keyboard.Key.Left))
                { x--; flag = true; _chendi.sAtk.Play(); }
                if (!flag && x < this._level.LevelWidth-2 && Keyboard.IsKeyPressed(Keyboard.Key.Right))
                { x++; flag = true; _chendi.sAtk.Play(); }
                if (!flag && y > 1 && Keyboard.IsKeyPressed(Keyboard.Key.Up))
                { y--; flag = true; _chendi.sAtk.Play(); }
                if (!flag && y < this._level.LevelHeight-2 && Keyboard.IsKeyPressed(Keyboard.Key.Down))
                { y++; flag = true; _chendi.sAtk.Play(); }

                type = this._level.GetObstacle(x, y).Type;

                //changes
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.X))
                { 
                    flag = true; 
                    this._chendi.sCoin.Play();

                    type++;
                    if (type == (BlockType)29) type = 0;

                    if ((type >= (BlockType)0 && type <= (BlockType)12))
                    { this._level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture; this._level.GetObstacle(x, y).UseTexture(); }
                    else if (type >= (BlockType)13 && type <= (BlockType)22)
                    { this._level.GetObstacle(x, y).LoadedTexture = Entity.PickupsTexture; this._level.GetObstacle(x, y).UseTexture(); }
                    else
                    { this._level.GetObstacle(x, y).LoadedTexture = Entity.DetailsTexture; this._level.GetObstacle(x, y).UseTexture(); }

                    this._level.GetObstacle(x, y).Type = type;
                    this._level.GetObstacle(x, y).SetBlock(type);
                }
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Z))
                {
                    flag = true;
                    this._chendi.sCoin.Play();

                    if (type > 0) type--;
                    else type = (BlockType)28;

                    if ((type >= (BlockType)0 && type <= (BlockType)12))
                    { this._level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture; this._level.GetObstacle(x, y).UseTexture(); }
                    else if (type >= (BlockType)13 && type <= (BlockType)22)
                    { this._level.GetObstacle(x, y).LoadedTexture = Entity.PickupsTexture; this._level.GetObstacle(x, y).UseTexture(); }
                    else
                    { this._level.GetObstacle(x, y).LoadedTexture = Entity.DetailsTexture; this._level.GetObstacle(x, y).UseTexture(); }

                    this._level.GetObstacle(x, y).Type = type;
                    this._level.GetObstacle(x, y).SetBlock(type);
                }

                //viem manip
                if (view && !flag && Keyboard.IsKeyPressed(Keyboard.Key.A)) 
                {
                    flag = true;
                    this._chendi.sCoin.Play();
                    view = false;
                }
                if (!view && !flag && Keyboard.IsKeyPressed(Keyboard.Key.S)) 
                {
                    flag = true;
                    this._chendi.sCoin.Play();
                    view = true;
                }

                choice.Position = new Vector2f(x * 32, y * 32);
                position.MoveText(choice.Position.X + 34, choice.Position.Y - 12);
                position.EditText(string.Format("{0},{1}", x, y));

                if (view ) this.SetView(new Vector2f(this._windowWidth/2, this._windowHeight/2), new Vector2f(choice.Position.X-16, choice.Position.Y+16));
                else this.SetView(new Vector2f(this._windowWidth, this._windowHeight), new Vector2f(choice.Position.X + 16, choice.Position.Y + 16));

                choice.Color = new Color((byte)rnd.Next(100,255), (byte)rnd.Next(100, 255), (byte)rnd.Next(100, 255));
                //

                if (flag) delay++;
                if (flag && delay > 9) { flag = false; delay = 0; }

                this._level.LevelUpdate();

                this.DrawGame(this._chendi, false);
                this._window.Draw(choice);
                this._window.Draw(position);
                this._window.Draw(cover);
                this._window.Draw(warning);
                this._window.Display();
                //exit
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) { this.DrawLoadingScreen(); this._level.SaveLevel(); break; } 
            }
            this._window.SetKeyRepeatEnabled(true);
            this.SetView(new Vector2f(this._windowWidth, this._windowHeight), new Vector2f(this._windowWidth / 2, this._windowHeight / 2));
            this._menuTheme.Play();
        }
        private void DevManip()
        {
            string answer;
            if (DevManipulation)
            {
                Console.WriteLine("Chendi Adventures Manipulation:");

                Console.Write("Play on fullscreen? (y/n)\n>"); answer = Console.ReadLine();
                if (answer == "y") this._windowStyle = Styles.Fullscreen;
                else
                { 
                    this._windowStyle = Styles.Resize;
                    //this._windowHeight = 960;
                    //this._windowWidth = 540;
                }



                //Console.Write("Choose level\n> "); answer = Console.ReadLine();
            }
        }
    }
}