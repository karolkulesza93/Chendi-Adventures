using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ChendiAdventures
{
    public sealed class MainGameWindow
    {
        public void DevManip()
        {
            bool doThings = false;
            if (doThings)
            {
                Settings.Default.HighestLevel = 25;
                Settings.Default.Save();
                //..
            }
        }
        //asdasdsadasd
        public static readonly Random Randomizer = new Random();
        public static Sound Victory = new Sound(new SoundBuffer(@"sfx/victory.wav"));
        public static Sound sChoice = new Sound(new SoundBuffer(@"sfx/choice.wav"));

        private MainGameWindow()
        {
            _isMenu = true;
            _isGame = false;
            _isLevelSelection = false;
            _isChallengeSelection = false;
            _isHighscore = false;
            _isSettigs = false;
            _isQuit = false;
            _isRestarting = false;
        }

        private MainGameWindow(string title) : this()
        {
            Title = title;
            _window = new RenderWindow(VideoMode.DesktopMode, Title, _windowStyle);

            if (Math.Round((double)_window.Size.Y / (double)_window.Size.X, 4) == 0.5625)
            {
                _windowWidth = 1920;
                _windowHeight = 1080;
            }
            else if (_window.Size.X > 1920)
            {
                _window = new RenderWindow(new VideoMode((uint)((double)_window.Size.Y / 0.5625d), _window.Size.Y), Title, _windowStyle);
                _windowWidth = (int)_window.Size.X;
                _windowHeight = (int)_window.Size.Y;
            }
            else
            {
                _windowWidth = (int)_window.Size.X;
                _windowHeight = (int)_window.Size.Y;
            }

            _window.SetFramerateLimit(60);
            _window.SetVisible(true);

            Joystick.Update();
            if (Joystick.IsConnected(0))
            {
                IsControllerConnected = true;
            }

            //events
            _window.Closed += OnClosed;
            _window.KeyPressed += OnKeyPress;
            _window.JoystickConnected += OnControllerConnection;
            _window.JoystickDisconnected += OnControllerDisconnection;
            //

            _window.SetMouseCursorVisible(false);
            _window.SetKeyRepeatEnabled(false);
            _window.SetVerticalSyncEnabled(true);
            MainView = new View(new FloatRect(0, 0, _windowWidth, _windowHeight));
            _screenChange = new ScreenChange(ref MainView);
        }

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

        public string Title { get; set; }
        public static Difficulty GameDifficulty { get; set; }
        public static bool IsVsync { get; set; }
        public static bool IsCamera { get; set; }
        public void Close()
        {
            _window.Close();
            Environment.Exit(0);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _window.Close();
            _isMenu = false;
            _isGame = false;
            _isLevelSelection = false;
            _isChallengeSelection = false;
            _isHighscore = false;
            _isSettigs = false;
            _isQuit = false;
        }

        private void OnKeyPress(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
            {
                var img = _window.Capture();
                img.SaveToFile(string.Format(@"screenshots/img_{0}.png", DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")));
            }
        }

        private void OnControllerConnection(object sender, EventArgs e)
        {
            IsControllerConnected = true;
            sChoice.Play();
        }

        private void OnControllerDisconnection(object sender, EventArgs e)
        {
            IsControllerConnected = false;
            sChoice.Play();
        }

        public void GameStart()
        {
            _loading = new TextLine("LOADING...", 50, 1390, 990, new Color(50, 50, 50));
            var tmp = new Texture("img/tiles.png", new IntRect(new Vector2i(32, 0), new Vector2i(32, 32)));
            tmp.Repeated = true;

            _waiting = new Sprite(new Texture(@"img/art/waiting.png"));
            _waiting.Color = new Color(255, 255, 255, 100);
            _waiting.Position = new Vector2f(50, _windowHeight - 330);

            _background = new Sprite(tmp);
            _background.Scale = new Vector2f(2f, 2f);
            _background.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(_windowWidth / 2 + 100, _windowHeight / 2 + 100));

            _gameLogo = new Sprite(new Texture(@"img/art/logo.png"));
            _gameLogo.Position = new Vector2f((_windowWidth - _gameLogo.Texture.Size.X) / 2, -300);

            _settingGear = new Sprite(new Texture(@"img/art/settinggear.png"));

            DrawLoadingScreen();
            LoadContents();
            ProductionInfoLoop();
            MainLoop();
        }

        private void ProductionInfoLoop()
        {
            Music sceneMusic = new Music(@"sfx/scene.wav") { Volume = 60 };
            sceneMusic.Loop = true;

            List<Sprite> artList = new List<Sprite>();
            for (int i = 1; i <= 6; i++)
            {
                artList.Add(new Sprite(new Texture(@"img/art/scene/" + i + ".png")));
            }

            for (int i = 0; i < artList.Count; i++)
            {
                artList[i].Position = new Vector2f(_windowWidth - 1024 - 10,  -artList[i].Texture.Size.Y);
            }

            var castle = new Sprite(new Texture(@"img/art/castle.png"));
            castle.Scale = new Vector2f((float)_windowWidth / (float)castle.Texture.Size.X, (float)_windowHeight / (float)castle.Texture.Size.Y);
            castle.Position = new Vector2f(0, _windowHeight);

            var line = new TextLine(
                
                "TUTAJ WSTAWIC HISTORIE\n"+
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" +
                "TUTAJ WSTAWIC HISTORIE\n" 

                , 50, 50, _windowHeight + 10, Color.White);
            line.SetOutlineThickness(3);

            var author = new TextLine("CREATED BY KAROL KULESZA", 50, MainView.Center.X - 600, MainView.Center.Y - 50,
                Color.White);

            var sfml = new Sprite(new Texture(@"img/pegi/sfml.png")) {Scale = new Vector2f(0.5f, 0.5f)};
            sfml.Position = new Vector2f(MainView.Center.X - sfml.Texture.Size.X / 4, MainView.Center.Y + 50);

            var pegi = new Sprite(new Texture(@"img/pegi/pegi.jpg"));
            pegi.Position = new Vector2f(MainView.Center.X - pegi.Texture.Size.X / 2, MainView.Center.Y - pegi.Texture.Size.Y / 2);

            var violence = new Sprite(new Texture(@"img/pegi/violence.jpg"));
            violence.Position = new Vector2f(MainView.Center.X - violence.Texture.Size.X / 2, MainView.Center.Y - violence.Texture.Size.Y / 2);

            var gambling = new Sprite(new Texture(@"img/pegi/gambling.jpg"));
            gambling.Position = new Vector2f(MainView.Center.X - gambling.Texture.Size.X / 2, MainView.Center.Y - gambling.Texture.Size.Y / 2);

            var timer = new Clock();
            var slideTime = 3;
            var artTime = 7;

            while (_window.IsOpen)
            {
                ResetWindow();

                if (timer.ElapsedTime.AsSeconds() > slideTime * 4)
                {
                    _window.Clear(new Color(184, 226, 242));
                    if (timer.ElapsedTime.AsSeconds() < slideTime * 4 + 1) _screenChange.AppearIn();
                    _window.Draw(castle);
                    if (sceneMusic.Status != SoundStatus.Playing)
                    {
                        sceneMusic.Play();
                    }
                    if (castle.Position.Y > 0)
                    {
                        castle.Position = new Vector2f(castle.Position.X, castle.Position.Y - 0.5f);
                    }
                    line.MoveText(line.X, line.Y - 1);
                }

                for (int i = 0; i < artList.Count; i++)
                {
                    if (timer.ElapsedTime.AsSeconds() > 4 * slideTime + i * artTime + 1)
                    {
                        if (artList[i].Position.X < _windowWidth + 10)
                        {
                            artList[i].Position = new Vector2f(artList[i].Position.X + 1, artList[i].Position.Y + 1);
                        }
                    }
                }



                if (timer.ElapsedTime.AsSeconds() > 4 * slideTime + 45)
                {
                    sceneMusic.Volume -= 0.5f;
                    _screenChange.BlackOut();
                }
                if (timer.ElapsedTime.AsSeconds() > 4 * slideTime + 47)
                {
                    break;
                }



                if (timer.ElapsedTime.AsSeconds() < 1)
                {
                    _screenChange.AppearIn();
                    _window.Draw(author);
                    _window.Draw(sfml);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime - 1)
                {
                    _window.Draw(author);
                    _window.Draw(sfml);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime)
                {
                    _window.Draw(author);
                    _window.Draw(sfml);
                    _screenChange.BlackOut();
                }

                else if (timer.ElapsedTime.AsSeconds() < slideTime + 1)
                {
                    _screenChange.AppearIn();
                    _window.Draw(pegi);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime*2 -1)
                {
                    _window.Draw(pegi);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime * 2)
                {
                    _screenChange.BlackOut();
                    _window.Draw(pegi);
                }

                else if (timer.ElapsedTime.AsSeconds() < slideTime*2+1)
                {
                    _screenChange.AppearIn();
                    _window.Draw(violence);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime * 3 -1)
                {
                    _window.Draw(violence);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime * 3)
                {
                    _screenChange.BlackOut();
                    _window.Draw(violence);
                }

                else if (timer.ElapsedTime.AsSeconds() < slideTime*3 + 1)
                {
                    _screenChange.AppearIn();
                    _window.Draw(gambling);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime * 4 - 1)
                {
                    _window.Draw(gambling);
                }
                else if (timer.ElapsedTime.AsSeconds() < slideTime * 4)
                {
                    _screenChange.BlackOut();
                    _window.Draw(gambling);
                }

                










                if (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(Keyboard.Key.Escape) ||
                Keyboard.IsKeyPressed(Keyboard.Key.Return) || Keyboard.IsKeyPressed(Keyboard.Key.Z) ||
                Joystick.IsButtonPressed(0, 7) || Joystick.IsButtonPressed(0, 0))
                {
                    _chendi.sPickup.Play();
                    break;
                }

                foreach (var art in artList)
                {
                    _window.Draw(art);
                }
                _window.Draw(line);
                _window.Draw(_screenChange);
                _window.Display();
            }
            
            sceneMusic.Stop();
            DrawLoadingScreen();
            Thread.Sleep(100);
        }

        private void GameEndingLoop()
        {

        }

        private void LicenceCheck()
        {
            string[] key;
            string msg = "";
            bool isValid;
            try
            {
                key = File.ReadAllLines(@"licence.txt");
                isValid = LicenceValidation(key[0]);
                if (!isValid) msg = "LICENCE ERROR:\nINVALID LICENCE NUMBER";
                else _licence = key[0];
            }
            catch (FileNotFoundException)
            {
                isValid = false;
                msg = "LICENCE ERROR:\nCOULD NOT FIND LICENCE FILE";
            }
            catch (Exception)
            {
                isValid = false;
                msg = "LICENCE ERROR:\nINVALID FORMAT OF LICENCE NUMBER";
            }


            if (isValid) return;
            else
            {
                _isMenu = false;
                TextLine message = new TextLine(msg, 50, 50, MainView.Center.Y + MainView.Size.Y / 2 - 130, Color.Red);
                message.SetOutlineThickness(5);
                _chendi.sError.Play();
                while (_window.IsOpen)
                {
                    ResetWindow();
                    _screenChange.AppearIn();

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    {
                        _chendi.sPickup.Play();
                        break;
                    }

                    AnimateBackground();
                    _window.Draw(_background);
                    _window.Draw(message);
                    _window.Display();
                }
            }
        }

        private bool LicenceValidation(string key)
        {
            try
            {
                var parts = key.Split('-');
                List<char> part1 = new List<char>();
                List<char> part2 = new List<char>();
                List<char> part3 = new List<char>();
                List<char> part4 = new List<char>();

                List<char> chars = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

                var indx1 = chars.IndexOf(parts[0][0]);
                var indx2 = chars.IndexOf(parts[1][0]);
                var indx3 = chars.IndexOf(parts[2][0]);
                var indx4 = chars.IndexOf(parts[3][0]);

                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 += indx3; indx1 = indx1 >= chars.Count ? indx1 - chars.Count : indx1;
                indx2 -= indx4; indx2 = indx2 < 0 ? indx2 + chars.Count : indx2;
                indx3 += indx1; indx3 = indx3 >= chars.Count ? indx3 - chars.Count : indx3;
                indx4 -= indx2; indx4 = indx4 < 0 ? indx4 + chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 += indx2; indx1 = indx1 >= chars.Count ? indx1 - chars.Count : indx1;
                indx2 += indx3; indx2 = indx2 >= chars.Count ? indx2 - chars.Count : indx2;
                indx3 += indx4; indx3 = indx3 >= chars.Count ? indx3 - chars.Count : indx3;
                indx4 += indx1; indx4 = indx4 >= chars.Count ? indx4 - chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 -= indx3; indx1 = indx1 < 0 ? indx1 + chars.Count : indx1;
                indx2 += indx4; indx2 = indx2 >= chars.Count ? indx2 - chars.Count : indx2;
                indx3 -= indx1; indx3 = indx3 < 0 ? indx3 + chars.Count : indx3;
                indx4 += indx2; indx4 = indx4 >= chars.Count ? indx4 - chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                indx1 -= indx2; indx1 = indx1 < 0 ? indx1 + chars.Count : indx1;
                indx2 -= indx3; indx2 = indx2 < 0 ? indx2 + chars.Count : indx2;
                indx3 -= indx4; indx3 = indx3 < 0 ? indx3 + chars.Count : indx3;
                indx4 -= indx1; indx4 = indx4 < 0 ? indx4 + chars.Count : indx4;
                part1.Add(chars[indx1]);
                part2.Add(chars[indx2]);
                part3.Add(chars[indx3]);
                part4.Add(chars[indx4]);

                for (int i = 0; i < 5; i++)
                {
                    if (part1[i] != parts[0][i]) return false;
                    if (part2[i] != parts[1][i]) return false;
                    if (part3[i] != parts[2][i]) return false;
                    if (part4[i] != parts[3][i]) return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
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

            Victory.Volume = 50;
            _gameEnd = new Sound(new SoundBuffer(@"sfx/gameover.wav"));
            _gameEnd.Volume = 50;

            _gameOver = new TextLine("GAME OVER", 100, 500, 470, Color.Red);
            _gameOver.SetOutlineThickness(3f);
            _continue = new TextLine("CONTINUE?", 50, 650, 590, Color.Yellow);
            _continue.SetOutlineThickness(5f);

            _restartLevel = new TextLine("RESTART LEVEL", 25, 0, 0, Color.White); _restartLevel.SetOutlineThickness(2.5f);
            _gotoMenu = new TextLine("MAIN MENU", 25, 0, 0, Color.White); _gotoMenu.SetOutlineThickness(2.5f);
            _resume = new TextLine("RESUME", 25, 0, 0, Color.White); _resume.SetOutlineThickness(2.5f);
            _yes = new TextLine("YES", 25, 0, 0, Color.White); _yes.SetOutlineThickness(2.5f);
            _no = new TextLine("NO", 25, 0, 0, Color.White); _no.SetOutlineThickness(2.5f);

            _joy = new TextLine("CONTROLLER CONNECTED", 20, MainView.Center.X + MainView.Size.X / 2 - 415, MainView.Center.Y - MainView.Size.Y / 2 + 5, new Color(0, 138, 198));

            _chendi = new MainCharacter(-100, -100, Entity.MainCharacterTexture);
            _level = new Level(_chendi, MainView);
            _chendiUi = new MainCharacterUI(_chendi, MainView, _level);

            _highscoreValues = new Highscores();
            LoadSettings();
        }

        private void MainLoop()
        {
            LicenceCheck();
            DevManip();

            while (_isGame || _isLevelSelection || _isChallengeSelection || _isMenu || _isHighscore || _isSettigs)
            {
                if (_isMenu) MainMenuLoop();
                if (_isLevelSelection) LevelSelectionLoop();
                if (_isChallengeSelection) ChallengeSelectionLoop();
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
                case 1:
                    {
                        GameDifficulty = Difficulty.Easy;
                        break;
                    }
                case 2:
                    {
                        GameDifficulty = Difficulty.Medium;
                        break;
                    }
                case 3:
                    {
                        GameDifficulty = Difficulty.Hard;
                        break;
                    }
            }

            IsVsync = Settings.Default.Vsync;
            IsCamera = Settings.Default.Camera;
            _window.SetVerticalSyncEnabled(IsVsync);


            //key bindings
        }

        private void SaveSettings()
        {
            Settings.Default.Difficulty = (int)GameDifficulty;

            Settings.Default.Vsync = IsVsync;
            _window.SetVerticalSyncEnabled(IsVsync);

            Settings.Default.Camera = IsCamera;

            Settings.Default.Save();
        }

        private void SettingsLoop()
        {
            //licence
            var licence = new TextLine("LICENCE NUMBER: " + _licence, 15, MainView.Center.X + MainView.Size.X / 2 - 600, MainView.Center.Y + MainView.Size.Y / 2 - 20, new Color(70, 70, 70));
            licence.SetOutlineThickness(1);

            //resolution - auto
            var resolution = new TextLine($"RESOLUTION: {_windowWidth}x{_windowHeight}", 50, -1000, MainView.Center.Y + MainView.Size.Y / 2 - 350, new Color(0, 138, 198));
            resolution.SetOutlineThickness(5);
            var vsync = new TextLine($"VSYNC: {(IsVsync ? "ON" : "OFF")}", 50, -1100, MainView.Center.Y + MainView.Size.Y / 2 - 290, Color.White);
            vsync.SetOutlineThickness(5);
            var camera = new TextLine($"CAMERA: {(IsCamera ? "SMOOTH" : "FIXED")}", 50, -1200, MainView.Center.Y + MainView.Size.Y / 2 - 230, Color.White);
            camera.SetOutlineThickness(5);
            var difficulty = new TextLine("", 50, -1300, MainView.Center.Y + MainView.Size.Y / 2 - 170, Color.White);
            difficulty.SetOutlineThickness(5);
            var keyBindings = new TextLine("KEY BINDINGS", 50, -1400, MainView.Center.Y + MainView.Size.Y / 2 - 110, Color.White);
            keyBindings.SetOutlineThickness(5);

            _settingGear.Position = new Vector2f(-300, MainView.Center.Y + MainView.Size.Y / 2 - 650);

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

            var choice = 1;
            var delay = 0;
            var flag = true;

            while (_window.IsOpen && _isSettigs)
            {
                ResetWindow();

                //slide text effect
                if (resolution.X < 50) resolution.MoveText(resolution.X + 50, resolution.Y);
                if (vsync.X < 50) vsync.MoveText(vsync.X + 50, vsync.Y);
                if (camera.X < 50) camera.MoveText(camera.X + 50, camera.Y);
                if (difficulty.X < 50) difficulty.MoveText(difficulty.X + 50, difficulty.Y);
                if (keyBindings.X < 50) keyBindings.MoveText(keyBindings.X + 50, keyBindings.Y);
                if (_settingGear.Position.X < 50) _settingGear.Position = new Vector2f(_settingGear.Position.X + 50, _settingGear.Position.Y);


                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5 || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50))
                {
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) < -5 || Joystick.GetAxisPosition(0, Joystick.Axis.Y) > 50))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP) || Joystick.IsButtonPressed(0, 0)))
                {
                    flag = true;
                    switch (choice)
                    {
                        case 1: //resolution
                            {
                                _chendi.sError.Play();
                                break;
                            }
                        case 2: //vsync
                            {
                                IsVsync = !IsVsync;
                                _chendi.sPickup.Play();
                                vsync.EditText($"VSYNC: {(IsVsync ? "ON" : "OFF")}");
                                break;
                            }
                        case 3: //camera
                            {
                                IsCamera = !IsCamera;
                                _chendi.sPickup.Play();
                                camera.EditText($"CAMERA: {(IsCamera ? "SMOOTH" : "FIXED")}");
                                break;
                            }
                        case 4: // difficulty
                            {
                                _chendi.sPickup.Play();
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
                        case 5: //key bindings
                            {
                                _chendi.sPickup.Play();
                                KeyBindingConfig();
                                break;
                            }
                    }
                }

                if (choice == 0) choice = 5;
                if (choice == 6) choice = 1;

                resolution.ChangeColor(new Color(100, 100, 100));
                vsync.ChangeColor(Color.White);
                camera.ChangeColor(Color.White);
                difficulty.ChangeColor(Color.White);
                keyBindings.ChangeColor(Color.White);

                switch (choice)
                {
                    case 1:
                        {
                            resolution.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 2:
                        {
                            vsync.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 3:
                        {
                            camera.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 4:
                        {
                            difficulty.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 5:
                        {
                            keyBindings.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 1))
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
                _window.Draw(camera);
                _window.Draw(difficulty);
                _window.Draw(keyBindings);
                _window.Draw(licence);
                _window.Draw(_settingGear);
                if (IsControllerConnected) _window.Draw(_joy);
                _window.Display();
            }
        }

        private void KeyBindingConfig()
        {
            var keys = new TextLine("", 50, -1000, MainView.Center.Y + MainView.Size.Y / 2 - 430, new Color(150, 150, 150));
            keys.SetOutlineThickness(5);

            var str = new StringBuilder();

            if (IsControllerConnected)
            {
                str.Append($"'{MainCharacter.KeyLEFT.ToString().ToUpper()}' : MOVE LEFT\n");
                str.Append($"'{MainCharacter.KeyRIGHT.ToString().ToUpper()}' : MOVE RIGHT\n");
                str.Append($"'{MainCharacter.KeyUP.ToString().ToUpper()}' : ACTION OR LOOK UP\n");
                str.Append("'A' : JUMP\n");
                str.Append($"'X' : SWORD ATTACK\n");
                str.Append($"'Y' : SHOOT AN ARROW\n");
                str.Append($"'B' : SHOOT AN ENERGIZED ARROW\n");
                str.Append($"'L1' OR 'R1' : BECOME IMMORTAL\n");
            }
            else
            {
                str.Append($"'{MainCharacter.KeyLEFT.ToString().ToUpper()}' : MOVE LEFT\n");
                str.Append($"'{MainCharacter.KeyRIGHT.ToString().ToUpper()}' : MOVE RIGHT\n");
                str.Append($"'{MainCharacter.KeyUP.ToString().ToUpper()}' : ACTION OR LOOK UP\n");
                str.Append($"'{MainCharacter.KeyJUMP.ToString().ToUpper()}' : JUMP\n");
                str.Append($"'{MainCharacter.KeyATTACK.ToString().ToUpper()}' : SWORD ATTACK\n");
                str.Append($"'{MainCharacter.KeyARROW.ToString().ToUpper()}' : SHOOT AN ARROW\n");
                str.Append($"'{MainCharacter.KeyTHUNDER.ToString().ToUpper()}' : SHOOT AN ENERGIZED ARROW\n");
                str.Append($"'{MainCharacter.KeyIMMORTALITY.ToString().ToUpper()}' : BECOME IMMORTAL\n");
            }

            keys.EditText(str.ToString());

            while (_window.IsOpen && _isSettigs)
            {
                ResetWindow();

                //slide text effect
                if (keys.X < 50) keys.MoveText(keys.X + 50, keys.Y);

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 1))
                {
                    _chendi.sPickup.Play();
                    break;
                }
                AnimateBackground();
                _window.Draw(_background);

                //draw settings
                _window.Draw(keys);
                if (IsControllerConnected) _window.Draw(_joy);
                _window.Display();
            }
            Thread.Sleep(100);
        }

        private void HighScoreLoop()
        {
            var hs = new TextLine("HIGHSCORES", 50, MainView.Center.X - 250, -400, new Color(0, 138, 198));
            hs.SetOutlineThickness(5);
            _highscoreValues.X = -900;

            while (_window.IsOpen && _isHighscore)
            {
                ResetWindow();

                //slide text effect
                if (hs.Y < 5) hs.MoveText(hs.X, hs.Y + 10);
                if (_highscoreValues.X < MainView.Center.X - 750) _highscoreValues.X += 50;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 1))
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
            _start = new TextLine("ADVENTURE MODE", 50, -500, MainView.Center.Y + MainView.Size.Y / 2 - 400, new Color(0, 138, 198)); //790
            _start.SetOutlineThickness(5);

            _genericGame = new TextLine("ENDLESS MODE", 50, -600, MainView.Center.Y + MainView.Size.Y / 2 - 340, Color.White);
            _genericGame.SetOutlineThickness(5);

            _challengeGame = new TextLine("WORKSHOP", 50, -700, MainView.Center.Y + MainView.Size.Y / 2 - 280, Color.White);
            _challengeGame.SetOutlineThickness(5);

            _highscores = new TextLine("HIGHSCORES", 50, -800, MainView.Center.Y + MainView.Size.Y / 2 - 220, Color.White); //850
            _highscores.SetOutlineThickness(5);

            _settings = new TextLine("OPTIONS", 50, -900, MainView.Center.Y + MainView.Size.Y / 2 - 160, Color.White); //910
            _settings.SetOutlineThickness(5);

            _quit = new TextLine("QUIT", 50, -1000, MainView.Center.Y + MainView.Size.Y / 2 - 100, Color.White); //970
            _quit.SetOutlineThickness(5);

            _gameLogo.Position = new Vector2f((_windowWidth - _gameLogo.Texture.Size.X) / 2, -300);

            if (_menuTheme.Status != SoundStatus.Playing) _menuTheme.Play();

            var choice = 1;
            var delay = 0;
            var flag = true;

            while (_window.IsOpen && _isMenu)
            {
                ResetWindow();

                _screenChange.AppearIn();

                // slide text effect
                if (_start.X < 50) _start.MoveText(_start.X + 50, _start.Y);
                if (_genericGame.X < 50) _genericGame.MoveText(_genericGame.X + 50, _genericGame.Y);
                if (_challengeGame.X < 50) _challengeGame.MoveText(_challengeGame.X + 50, _challengeGame.Y);
                if (_highscores.X < 50) _highscores.MoveText(_highscores.X + 50, _highscores.Y);
                if (_settings.X < 50) _settings.MoveText(_settings.X + 50, _settings.Y);
                if (_quit.X < 50) _quit.MoveText(_quit.X + 50, _quit.Y);
                if (_gameLogo.Position.Y < 50)
                    _gameLogo.Position = new Vector2f((_windowWidth - _gameLogo.Texture.Size.X) / 2,
                        _gameLogo.Position.Y + 25);

                //secret keys to enter level editor
                if (flag == false && Keyboard.IsKeyPressed(Keyboard.Key.LShift) &&
                    Keyboard.IsKeyPressed(Keyboard.Key.LAlt) &&
                    Keyboard.IsKeyPressed(Keyboard.Key.L))
                {
                    Level.LevelGameMode = GameMode.Adventure;
                    _chendi.sPickup.Play();
                    flag = true;
                    LevelEditor();
                    _chendi.sPickup.Play();
                }
                //

                _start.ChangeColor(Color.White);
                _genericGame.ChangeColor(new Color(100, 100, 100));
                _challengeGame.ChangeColor(Color.White);
                _highscores.ChangeColor(Color.White);
                _settings.ChangeColor(Color.White);
                _quit.ChangeColor(Color.White);

                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5))
                {
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) < -5))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Joystick.IsButtonPressed(0, 0)))
                {
                    flag = true;
                    _chendi.sPickup.Play();
                    switch (choice)
                    {
                        case 1:
                            {
                                if (Settings.Default.HighestLevel >= 5 && Level.LevelNumber != 0)
                                {
                                    _isMenu = false;
                                    _isLevelSelection = true;
                                }
                                else
                                {
                                    DrawLoadingScreen();
                                    Thread.Sleep(100);
                                    _isMenu = false;
                                    _isGame = true;
                                    if (Level.LevelNumber != 0) Level.LevelNumber = 1;
                                    _chendi.ResetMainCharacter();
                                    _menuTheme.Stop();
                                }
                                Level.LevelGameMode = GameMode.Adventure;
                                break;
                            }
                        case 2:
                            {
                                Level.LevelGameMode = GameMode.Generic;
                                break;
                            }
                        case 3:
                            {
                                _isMenu = false;
                                _isChallengeSelection = true;
                                Level.LevelGameMode = GameMode.Challenge;
                                break;
                            }
                        case 4:
                            {
                                _isMenu = false;
                                _isHighscore = true;
                                break;
                            }
                        case 5:
                            {
                                _isMenu = false;
                                _isSettigs = true;
                                break;
                            }
                        case 6:
                            {
                                _isMenu = false;
                                _isQuit = true;
                                break;
                            }
                    }
                }

                if (choice == 0) choice = 6;
                if (choice == 7) choice = 1;

                switch (choice)
                {
                    case 1:
                        {
                            _start.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 2:
                        {
                            _genericGame.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 3:
                        {
                            _challengeGame.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 4:
                        {
                            _highscores.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 5:
                        {
                            _settings.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                    case 6:
                        {
                            _quit.ChangeColor(new Color(0, 138, 198));
                            break;
                        }
                }

                if (_isMenu) DrawMainMenu();
            }
        }

        private void LevelSelectionLoop()
        {
            var maxLevel = Settings.Default.HighestLevel;
            var levels = new List<TextLine>();

            for (var i = 0; i <= maxLevel; i += 5)
                levels.Add(new TextLine(string.Format("LEVEL {0}", i == 0 ? 1 : i), 50, -400 - i / 5 * 100, i / 5 * 60,
                    Color.White));

            for (var i = 0; i < levels.Count; i++)
            {
                levels[i].MoveText(levels[i].X, MainView.Size.Y - 50 - 60 * (levels.Count - i));
                levels[i].SetOutlineThickness(5);
            }

            var choice = 0;
            var delay = 0;
            var flag = true;

            levels[0].ChangeColor(new Color(0, 138, 198));

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

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5))
                {
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) < -5))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }

                if (choice < 0) choice = levels.Count - 1;
                if (choice > levels.Count - 1) choice = 0;

                levels[choice].ChangeColor(new Color(0, 138, 198));

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Joystick.IsButtonPressed(0, 0)))
                {
                    _chendi.sPickup.Play();
                    DrawLoadingScreen();
                    Thread.Sleep(100);
                    _isLevelSelection = false;
                    _isGame = true;
                    if (Level.LevelNumber != 0) Level.LevelNumber = choice == 0 ? 1 : choice * 5;
                    _chendi.ResetMainCharacter();
                    _menuTheme.Stop();
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 1))
                {
                    _chendi.sPickup.Play();
                    _isLevelSelection = false;
                    _isMenu = true;
                }


                if (_isLevelSelection)
                {
                    AnimateBackground();
                    _window.Draw(_background);
                    foreach (var line in levels) _window.Draw(line);
                    if (IsControllerConnected) _window.Draw(_joy);
                    _window.Display();
                }
            }
        }

        private void ChallengeSelectionLoop()
        {
            DirectoryInfo info = new DirectoryInfo(@"levels/challenges");
            FileInfo[] challenges = info.GetFiles("*.dat");
            var levels = new List<TextLine>();

            foreach (var file in challenges)
            {
                levels.Add(new TextLine(file.Name, 50, 0, 0, Color.White));
            }

            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].MoveText(-1 * i * 100 - 700, MainView.Size.Y - 50 - 60 * (levels.Count - i));
                levels[i].EditText(levels[i].ToString().Remove(levels[i].ToString().Length - 4));
                levels[i].SetOutlineThickness(5);
            }

            if (levels.Count == 0)
            {
                levels.Add(new TextLine("NO CHALLENGES FOUND", 50, -700, MainView.Size.Y - 110, Color.Red));
                levels[0].SetOutlineThickness(5);
            }

            var choice = 0;
            var delay = 0;
            var flag = true;

            while (_window.IsOpen && _isChallengeSelection)
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

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5))
                {
                    if (levels.Count > 1) sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) < -5))
                {
                    if (levels.Count > 1) sChoice.Play();
                    flag = true;
                    choice++;
                }

                if (choice < 0) choice = levels.Count - 1;
                if (choice > levels.Count - 1) choice = 0;

                levels[choice].ChangeColor(new Color(0, 138, 198));

                if (flag == false && levels.Count > 1 && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Joystick.IsButtonPressed(0, 0)))
                {
                    _chendi.sPickup.Play();
                    DrawLoadingScreen();
                    Thread.Sleep(100);
                    _isChallengeSelection = false;
                    _isGame = true;

                    Level.ChallengeName = levels[choice].ToString();

                    _chendi.ResetMainCharacter();
                    _menuTheme.Stop();
                }


                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 1))
                {
                    _chendi.sPickup.Play();
                    _isChallengeSelection = false;
                    _isMenu = true;
                }

                if (_isChallengeSelection)
                {
                    AnimateBackground();
                    _window.Draw(_background);
                    foreach (var line in levels) _window.Draw(line);
                    if (IsControllerConnected) _window.Draw(_joy);
                    _window.Display();
                }
            }
        }

        private void GameLoop()
        {
            //DrawLoadingScreen();

            _levelSummary = new TextLine("", 25, -1000, -1000, Color.White);

            _chendi.SetColor(Color.White);

            SetView(new Vector2f(_windowWidth / 2, _windowHeight / 2), MainView.Center);
            _screenChange.Reset();

            _level.LoadLevel($"lvl{Level.LevelNumber}");

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

            GameOverAndAddToHighscore();

            ProceedToNextLevel();

            if (_isRestarting) RestartLevel();
        }

        private void ResetWindow()
        {
            _window.DispatchEvents();
            _window.Clear(Color.Black);
            if (IsControllerConnected) Joystick.Update();
        }

        private void DrawLoadingScreen()
        {
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));
            ResetWindow();

            _loading.MoveText(MainView.Center.X + MainView.Size.X / 2 - _loading.Width * 1.3f,
                MainView.Center.Y + MainView.Size.Y / 2 - 70);

            _window.Draw(_loading);
            _window.Draw(_waiting);
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
                Level.LevelNumber = 1;
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

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Joystick.IsButtonPressed(0, 0))
                    {
                        _chendi.sPickup.Play();
                        _chendi.ResetMainCharacter();
                        _chendi.Continues--;
                        _gameEnd.Stop();
                        DrawLoadingScreen();
                        return;
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 1))
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

        private void DrawGame(MainCharacter character, bool display)
        {
            _window.Draw(_level);
            _window.Draw(character);
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
            _window.Draw(_genericGame);
            _window.Draw(_challengeGame);
            _window.Draw(_highscores);
            _window.Draw(_settings);
            _window.Draw(_quit);
            if (IsControllerConnected) _window.Draw(_joy);

            _window.Draw(_screenChange);
            _window.Display();
        }

        private void ViewManipulation(Level level)
        {
            float x = MainView.Center.X;
            float y = MainView.Center.Y;

            if (IsCamera)
            {
                x += (_chendi.GetCenterPosition().X - MainView.Center.X) / 10;
                y += (_chendi.GetCenterPosition().Y - MainView.Center.Y) / 10;
            }
            else
            {
                x = _chendi.GetCenterPosition().X;
                y = _chendi.GetCenterPosition().Y;
            }

            if (x - MainView.Size.X / 2 <= 0) x = MainView.Size.X / 2;
            else if (x + MainView.Size.X / 2 >= level.LevelWidth * 32) x = level.LevelWidth * 32 - MainView.Size.X / 2;

            if (level.LevelWidth * 64 < _windowWidth)
            {
                x = level.LevelWidth * 16;
            }

            if (y - MainView.Size.Y / 2 <= 0) y = MainView.Size.Y / 2;
            else if (y + MainView.Size.Y / 2 >= level.LevelHeight * 32) y = level.LevelHeight * 32 - MainView.Size.Y / 2;

            if (level.LevelHeight * 64 < _windowHeight)
            {
                y = level.LevelHeight * 16;
            }

            MainView.Center = new Vector2f(x, y);
            _window.SetView(MainView);

            if (!_screenChange.Done) _screenChange.AppearIn();
        }

        private void SetView(Vector2f size, Vector2f center)
        {
            MainView.Size = size;
            MainView.Center = center;
            _window.SetView(MainView);
        }

        private bool CheckForGameBreak()
        {
            if (_chendi.IsDead && _chendi.DefaultClock.ElapsedTime.AsSeconds() > 2.5f)
            {
                _screenChange.BlackOut();
                if (_chendi.OutOfLives && _chendi.DefaultClock.ElapsedTime.AsSeconds() > 3f) return true;
            }

            if (_chendi.IsDead) _mainTheme.Stop();
            if (_chendi.IsDead && !_chendi.OutOfLives && _chendi.DefaultClock.ElapsedTime.AsSeconds() > 3)
            {
                _chendi.Respawn();
                _mainTheme.Play();
            }
            if (_chendi.GotExit || (_chendi.OutOfLives && !_chendi.IsDead) || _isRestarting) return true;
            return false;
        }

        private void AnimateBackground()
        {
            if (_background.Position.X < -32 && _background.Position.Y < -32)
                _background.Position = new Vector2f(0, 0);
            else _background.Position = new Vector2f(_background.Position.X - 0.2f, _background.Position.Y - 0.2f);
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
            var flag = true;
            var delay = 0;

            var view = true;

            BlockType type = 0;
            BlockType typeM = 0;

            var rnd = new Random();

            //tiles 
            var t1 = 1;
            var t2 = 26;
            //pickups
            var p1 = 27;
            var p2 = 39;
            //details
            var d1 = 40;
            var d2 = 51;

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
                    if (type == (BlockType)d2 + 1) type = 0;

                    typeM = type;

                    if (type >= (BlockType)t1 && type <= (BlockType)t2)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (type >= (BlockType)p1 && type <= (BlockType)p2)
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
                    else type = (BlockType)d2;

                    typeM = type;

                    if (type >= (BlockType)t1 && type <= (BlockType)t2)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (type >= (BlockType)p1 && type <= (BlockType)p2)
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


                    if (typeM >= (BlockType)t1 && typeM <= (BlockType)t2)
                    {
                        _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                        _level.GetObstacle(x, y).UseTexture();
                    }
                    else if (typeM >= (BlockType)p1 && typeM <= (BlockType)p2)
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

                    _level.Monsters.Add(new Knight(x * 32, y * 32, Entity.KnightTexture));
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

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Walkers.Add(new Walker(x * 32, y * 32, Entity.WalkerTexture, Movement.Left));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.E))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Walkers.Add(new Walker(x * 32, y * 32, Entity.WalkerTexture, Movement.Right));
                }


                //clear monsters
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F1))
                {
                    flag = true;
                    _chendi.sError.Play();
                    _level.Monsters.Clear();
                }

                if (!flag && (Keyboard.IsKeyPressed(Keyboard.Key.F2) || Keyboard.IsKeyPressed(Keyboard.Key.F3)))
                {
                    flag = true;
                    _chendi.sError.Play();
                    _level.Archers.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F4))
                {
                    flag = true;
                    _chendi.sError.Play();
                    _level.Ghosts.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F5))
                {
                    flag = true;
                    _chendi.sError.Play();
                    _level.Wizards.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F6))
                {
                    flag = true;
                    _chendi.sError.Play();
                    _level.Golems.Clear();
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.R))
                {
                    flag = true;
                    _chendi.sError.Play();
                    _level.Walkers.Clear();
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

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Dash))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.BlowerLeft));
                }

                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.Equal))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    _level.Traps.Add(new Trap(x * 32, y * 32, Entity.TrapsTexture, TrapType.BlowerRight));
                }
                //clear traps
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F7))
                {
                    flag = true;
                    _chendi.sError.Play();

                    try
                    {
                        for (int i = 0; i < _level.Traps.Count; i++)
                        {
                            if (_level.Traps[i].Type == TrapType.Crusher)
                            {
                                _level.Traps.RemoveAt(i);
                                if (i + 1 > 0) i--;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F8))
                {
                    flag = true;
                    _chendi.sError.Play();

                    try
                    {
                        for (int i = 0; i < _level.Traps.Count; i++)
                        {
                            if (_level.Traps[i].Type == TrapType.Spikes)
                            {
                                _level.Traps.RemoveAt(i);
                                if (i + 1 > 0) i--;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F9))
                {
                    flag = true;
                    _chendi.sError.Play();

                    try
                    {
                        for (int i = 0; i < _level.Traps.Count; i++)
                        {
                            if (_level.Traps[i].Type == TrapType.BlowTorchLeft || _level.Traps[i].Type == TrapType.BlowTorchRight)
                            {
                                _level.Traps.RemoveAt(i);
                                if (i + 1 > 0) i--;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F10))
                {
                    flag = true;
                    _chendi.sError.Play();

                    try
                    {
                        for (int i = 0; i < _level.Traps.Count; i++)
                        {
                            if (_level.Traps[i].Type == TrapType.BlowerLeft || _level.Traps[i].Type == TrapType.BlowerRight)
                            {
                                _level.Traps.RemoveAt(i);
                                if (i + 1 > 0) i--;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
                //mirror image
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F11))
                {
                    flag = true;
                    _chendi.sPickup.Play();

                    BlockType typeMirror;

                    for (x = 1; x < (_level.LevelWidth - 2) / 2 + 1; x++)
                        for (y = 1; y < _level.LevelHeight - 1; y++)
                        {
                            typeMirror = _level.GetObstacle(x, y).Type;
                            _level.GetObstacle(_level.LevelWidth - x - 1, y).Type = typeMirror;

                            if (typeMirror >= (BlockType)t1 && typeMirror <= (BlockType)t2)
                            {
                                _level.GetObstacle(_level.LevelWidth - x - 1, y).LoadedTexture = Entity.TilesTexture;
                                _level.GetObstacle(_level.LevelWidth - x - 1, y).UseTexture();
                            }
                            else if (typeMirror >= (BlockType)p1 && typeMirror <= (BlockType)p2)
                            {
                                _level.GetObstacle(_level.LevelWidth - x - 1, y).LoadedTexture = Entity.PickupsTexture;
                                _level.GetObstacle(_level.LevelWidth - x - 1, y).UseTexture();
                            }
                            else
                            {
                                _level.GetObstacle(_level.LevelWidth - x - 1, y).LoadedTexture = Entity.DetailsTexture;
                                _level.GetObstacle(_level.LevelWidth - x - 1, y).UseTexture();
                            }

                            _level.GetObstacle(_level.LevelWidth - x - 1, y).Type = typeMirror;
                            _level.GetObstacle(_level.LevelWidth - x - 1, y).SetBlock(typeMirror);
                        }

                    x = (_level.LevelWidth - 2) / 2 + 1;
                    y = (_level.LevelHeight - 2) / 2 + 1;
                }

                //generate
                //if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F11))
                //{
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
                //}

                //clear blocks
                if (!flag && Keyboard.IsKeyPressed(Keyboard.Key.F12))
                {
                    flag = true;
                    _chendi.sError.Play();

                    for (x = 1; x < _level.LevelWidth - 1; x++)
                        for (y = 1; y < _level.LevelHeight - 1; y++)
                        {
                            type = BlockType.None;

                            _level.GetObstacle(x, y).LoadedTexture = Entity.TilesTexture;
                            _level.GetObstacle(x, y).UseTexture();

                            _level.GetObstacle(x, y).Type = type;
                            _level.GetObstacle(x, y).SetBlock(type);
                        }

                    x = (_level.LevelWidth - 2) / 2 + 1;
                    y = (_level.LevelHeight - 2) / 2 + 1;
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
                    x = (_level.LevelWidth - 2) / 2 + 1;
                    y = (_level.LevelHeight - 2) / 2 + 1;
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
                    _level.SaveLevel(false);
                    break;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {
                    DrawLoadingScreen();
                    _level.SaveLevel(true);
                    break;
                }
            }

            _window.SetKeyRepeatEnabled(true);
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));
            _menuTheme.Play();
        }

        private void ExitLoop()
        {
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 6) || Joystick.IsButtonPressed(0, 7)) && !_chendi.IsDead)
            {
                bool isQuitMenu = true;
                var choice = 1;
                bool yesNo = false;
                bool menu = true;
                var flag = true;
                var delay = 0;

                _resume.MoveText(MainView.Center.X - MainView.Size.X / 2 - 450, MainView.Center.Y + MainView.Size.Y / 2 - 110);
                _restartLevel.MoveText(MainView.Center.X - MainView.Size.X / 2 - 500, MainView.Center.Y + MainView.Size.Y / 2 - 80);
                _gotoMenu.MoveText(MainView.Center.X - MainView.Size.X / 2 - 550, MainView.Center.Y + MainView.Size.Y / 2 - 50);

                while (_window.IsOpen && _isGame && isQuitMenu)
                {
                    ResetWindow();

                    //textslide effect
                    if (_resume.X < MainView.Center.X - MainView.Size.X / 2 + 25) _resume.MoveText(_resume.X + 25, _resume.Y);
                    if (_restartLevel.X < MainView.Center.X - MainView.Size.X / 2 + 25) _restartLevel.MoveText(_restartLevel.X + 25, _restartLevel.Y);
                    if (_gotoMenu.X < MainView.Center.X - MainView.Size.X / 2 + 25) _gotoMenu.MoveText(_gotoMenu.X + 25, _gotoMenu.Y);

                    _yes.MoveText(-100, 0);
                    _no.MoveText(-100, 0);

                    if (menu)
                    {
                        _resume.ChangeColor(Color.White);
                        _restartLevel.ChangeColor(Color.White);
                        _gotoMenu.ChangeColor(Color.White);
                        _yes.ChangeColor(Color.White);
                        _no.ChangeColor(Color.White);

                        switch (choice)
                        {
                            case 1:
                                {
                                    _resume.ChangeColor(new Color(0, 138, 198));
                                    break;
                                }
                            case 2:
                                {
                                    _restartLevel.ChangeColor(new Color(0, 138, 198));
                                    break;
                                }
                            case 3:
                                {
                                    _gotoMenu.ChangeColor(new Color(0, 138, 198));
                                    break;
                                }
                        }

                        if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) < -5))
                        {
                            sChoice.Play();
                            flag = true;
                            choice++;
                        }
                        else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5))
                        {
                            sChoice.Play();
                            flag = true;
                            choice--;
                        }
                        else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Joystick.IsButtonPressed(0, 0)))
                        {
                            _chendi.sPickup.Play();
                            menu = false;
                            flag = true;
                            yesNo = false;
                        }

                        if (flag) delay++;
                        if (delay > 10)
                        {
                            delay = 0;
                            flag = false;
                        }

                        if (choice == 4) choice = 1;
                        else if (choice == 0) choice = 3;
                    }
                    else
                    {
                        _yes.ChangeColor(Color.White);
                        _no.ChangeColor(Color.White);

                        if (yesNo) _yes.ChangeColor(new Color(0, 138, 198));
                        else _no.ChangeColor(new Color(0, 138, 198));

                        switch (choice)
                        {
                            case 1:
                                {
                                    isQuitMenu = false;
                                    break;
                                }
                            case 2:
                                {
                                    _yes.MoveText(MainView.Center.X - MainView.Size.X / 2 + 455, MainView.Center.Y + MainView.Size.Y / 2 - 80);
                                    _no.MoveText(MainView.Center.X - MainView.Size.X / 2 + 390, MainView.Center.Y + MainView.Size.Y / 2 - 80);

                                    _resume.ChangeColor(new Color(100, 100, 100));
                                    _restartLevel.ChangeColor(new Color(0, 38, 98));
                                    _gotoMenu.ChangeColor(new Color(100, 100, 100));

                                    break;
                                }
                            case 3:
                                {
                                    _yes.MoveText(MainView.Center.X - MainView.Size.X / 2 + 455, MainView.Center.Y + MainView.Size.Y / 2 - 50);
                                    _no.MoveText(MainView.Center.X - MainView.Size.X / 2 + 390, MainView.Center.Y + MainView.Size.Y / 2 - 50);

                                    _resume.ChangeColor(new Color(100, 100, 100));
                                    _restartLevel.ChangeColor(new Color(100, 100, 100));
                                    _gotoMenu.ChangeColor(new Color(0, 38, 98));
                                    break;
                                }
                        }

                        if (!flag && yesNo == true && (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Joystick.GetAxisPosition(0, Joystick.Axis.X) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) < -5))
                        {
                            flag = true;
                            sChoice.Play();
                            yesNo = !yesNo;
                        }
                        else if (!flag && yesNo == false && (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Joystick.GetAxisPosition(0, Joystick.Axis.X) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovX) > 5))
                        {
                            flag = true;
                            sChoice.Play();
                            yesNo = !yesNo;
                        }
                        else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Joystick.IsButtonPressed(0, 0)))
                        {
                            _chendi.sPickup.Play();
                            flag = true;
                            if (yesNo)
                            {
                                isQuitMenu = false;
                                switch (choice)
                                {
                                    case 2:
                                        {
                                            _isRestarting = true;
                                            _isGame = false;
                                            isQuitMenu = false;
                                            _mainTheme.Stop();
                                            break;
                                        }
                                    case 3:
                                        {
                                            DrawLoadingScreen();

                                            _highscoreValues.AddNewRecord(new HighscoreRecord(_chendi.Score, Level.LevelNumber,
                                                GameDifficulty.ToString().ToUpper()));
                                            if (Level.LevelNumber > Settings.Default.HighestLevel)
                                            {
                                                Settings.Default.HighestLevel = Level.LevelNumber;
                                                Settings.Default.Save();
                                            }

                                            _isGame = false;
                                            _isMenu = true;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                menu = true;
                            }
                        }
                        if (!flag && (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Joystick.IsButtonPressed(0, 1)))
                        {
                            _chendi.sPickup.Play();
                            flag = true;
                            menu = true;
                        }


                        if (flag) delay++;
                        if (delay > 10)
                        {
                            delay = 0;
                            flag = false;
                        }
                    }

                    DrawGame(_chendi, false);
                    _window.Draw(_resume);
                    _window.Draw(_restartLevel);
                    _window.Draw(_gotoMenu);

                    _window.Draw(_yes);
                    _window.Draw(_no);

                    if (isQuitMenu) _window.Display();
                }
                Thread.Sleep(100);
            }
        }

        private void RestartLevel()
        {
            DrawLoadingScreen();
            SetView(new Vector2f(_windowWidth, _windowHeight), new Vector2f(_windowWidth / 2, _windowHeight / 2));
            _levelSummary = new TextLine("", 25, -1000, -1000, Color.White);
            _screenChange.Reset();

            _chendi.Score = _level.StartScore;
            _chendi.Coins = _level.StartCoins;
            _chendi.ArrowAmount = _level.StartArrow;
            _chendi.Mana = _level.StartMana;
            _chendi.SetColor(Color.White);

            _isGame = true;
            _isRestarting = false;
        }

        private void GameOverAndAddToHighscore()
        {
            if (_chendi.OutOfLives) //game over
            {
                _highscoreValues.AddNewRecord(new HighscoreRecord(_chendi.Score, Level.LevelNumber,
                    GameDifficulty.ToString().ToUpper()));
                if (Level.LevelNumber > Settings.Default.HighestLevel)
                {
                    Settings.Default.HighestLevel = Level.LevelNumber;
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
                if (Level.LevelNumber == 0) _chendi.Score = 0;

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
                _levelSummary.X = MainView.Center.X - 1000;
                _levelSummary.Y = MainView.Center.Y - 100;


                var timer = new Clock();
                timer.Restart();
                _chendi.sImmortality.Stop();
                bool isLottery;
                if (Level.LevelNumber < 51 && Level.LevelNumber > 5 && Randomizer.Next(100) + 1 > 66 && Level.LevelGameMode != GameMode.Challenge)
                    isLottery = true;
                else if (Level.LevelNumber == 5)
                    isLottery = true;
                else
                    isLottery = false;

                while (_window.IsOpen && _isGame && _chendi.GotExit) //to next level
                {
                    ResetWindow();

                    if (_levelSummary.X < MainView.Center.X - 250)
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
                        switch (Level.LevelGameMode)
                        {
                            case GameMode.Adventure:
                                {
                                    Level.LevelNumber++;
                                    break;
                                }
                            case GameMode.Challenge:
                                {
                                    _isGame = false;
                                    _isMenu = true;
                                    break;
                                }
                            case GameMode.Generic:
                                {
                                    break;
                                }
                        }
                        //lottery
                        if (isLottery) LotteryLoop();
                        _chendi.GotExit = false;
                        DrawLoadingScreen();
                        break;
                    }
                }
                _screenChange.Reset();
                timer.Dispose();
            }
        }

        private void LotteryLoop()
        {
            var clock = new Clock();
            var gameMachine = new GameMachine(MainView.Center.X - MainView.Size.X / 2 - 70, MainView.Center.Y + 80,
                Entity.GameMachineTexture, MainView);
            var isRolling = true;
            var done = false;
            var time = 50;

            if (Randomizer.Next(100) > 0)
                while (_window.IsOpen && _isGame)
                {
                    ResetWindow();

                    if (isRolling && !done && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP) || Joystick.IsButtonPressed(0, 0)))
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

        private void ShopLoop()
        {
            var merchant = new Merchant(MainView.Center.X - MainView.Size.X / 2 - 406, MainView.Center.Y - 140,
                Entity.ShopTexture, _chendi);
            merchant.SetTextureRectangle(0, 0, 322, 158);


            var choice = 1;
            var delay = 0;
            var flag = true;

            while (_window.IsOpen && _isGame)
            {
                ResetWindow();

                if (flag) delay++;
                if (delay > 10)
                {
                    delay = 0;
                    flag = false;
                }


                if (merchant.X < MainView.Center.X - 178) merchant.X += 32;
                if (merchant.X > MainView.Center.X + MainView.Size.X / 2) break;

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) < -50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) > 5))
                {
                    sChoice.Play();
                    flag = true;
                    choice--;
                }
                else if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Joystick.GetAxisPosition(0, Joystick.Axis.Y) > 50 || Joystick.GetAxisPosition(0, Joystick.Axis.PovY) < -5))
                {
                    sChoice.Play();
                    flag = true;
                    choice++;
                }

                if (choice == 0) choice = 4;
                if (choice == 5) choice = 1;

                if (flag == false && (Keyboard.IsKeyPressed(Keyboard.Key.Space) || Keyboard.IsKeyPressed(MainCharacter.KeyJUMP) || Joystick.IsButtonPressed(0, 0)))
                {
                    merchant.SellWares(choice);
                    flag = true;
                }


                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(MainCharacter.KeyTHUNDER) || Joystick.IsButtonPressed(0, 1))
                    _level.isShopOpened = false;
                if (!_level.isShopOpened) merchant.X += 32;

                _level.LevelUpdate();
                _chendiUi.UpdateUI();
                merchant.ShopUpdate(choice);

                DrawGame(_chendi, false);
                _window.Draw(merchant);
                _window.Display();
            }
        }

        private static MainGameWindow _instance;
        private static readonly object Padlock = new object();

        public static bool IsControllerConnected;
        public static View MainView;
        private readonly RenderWindow _window;
        private Sprite _background;
        private MainCharacter _chendi;
        private MainCharacterUI _chendiUi;
        private TextLine _continue;
        private Sound _gameEnd;
        private Sprite _gameLogo;
        private Sprite _settingGear;
        private Sprite _waiting;
        private TextLine _gameOver;
        private TextLine _highscores;
        private Highscores _highscoreValues;
        private bool _isGame;
        private bool _isHighscore;
        private bool _isLevelSelection;
        private bool _isChallengeSelection;
        private bool _isMenu;
        private bool _isQuit;
        private bool _isSettigs;
        private bool _isRestarting;
        private Level _level;
        private TextLine _levelSummary;
        private TextLine _loading;
        private Music _mainTheme;
        private Music _menuTheme;
        private TextLine _quit;
        private readonly ScreenChange _screenChange;
        private TextLine _settings;
        private TextLine _start;
        private TextLine _genericGame;
        private TextLine _challengeGame;
        private readonly int _windowHeight;
        private readonly Styles _windowStyle = Styles.Fullscreen;
        private readonly int _windowWidth;
        private string _licence;
        private TextLine _joy;

        private TextLine _yes;
        private TextLine _no;
        private TextLine _restartLevel;
        private TextLine _gotoMenu;
        private TextLine _resume;
    }
}