using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace ChendiAdventures
{
    public class Level : Drawable
    {
        public static List<Block> SteelGates = new List<Block>();
        public static List<Block> Levers = new List<Block>();
        public static GameMode LevelGameMode;
        public static string ChallengeName;

        public static int LeverInterval = 10;
        public static bool IsUnderground = true;
        public static Color LevelColor { get; set; }

        public static List<BlockType> AnimateableBlocks = new List<BlockType>
        {
            BlockType.Coin, BlockType.SilverKey, BlockType.GoldenKey, BlockType.Life, BlockType.Score5000,
            BlockType.Arrow, BlockType.TripleArrow, BlockType.Score1000, BlockType.Mana, BlockType.Torch,
            BlockType.TripleMana, BlockType.SackOfGold, BlockType.Exit, BlockType.Teleport1, BlockType.Teleport2,
            BlockType.Purifier, BlockType.Teleport3, BlockType.Teleport4, BlockType.EnergyBall, BlockType.CrystalKey,
            BlockType.CrystalDoor, BlockType.EvilEyes, BlockType.SpectralCrystal, BlockType.SwordEnchant
        };

        public readonly List<Archer> Archers;
        public readonly List<Ghost> Ghosts;
        public readonly List<Golem> Golems;
        public readonly List<Walker> Walkers;
        public readonly List<Block> LevelObstacles;
        public readonly List<Knight> Monsters;
        public readonly List<ParticleEffect> Particles;
        public readonly List<ScoreAdditionEffect> ScoreAdditionEffects;
        public readonly List<Trap> Traps;
        public readonly List<Wizard> Wizards;
        public Boss Argoth;
        public bool isShopOpened;

        public List<BlockType> UnableToPassl;

        public Level(MainCharacter character, View view)
        {
            _mainCharacter = character;
            _view = view;
            rnd = new Random();
            IsUnderground = true;

            ScoreAdditionEffects = new List<ScoreAdditionEffect>();
            Particles = new List<ParticleEffect>();

            LevelObstacles = new List<Block>();
            Traps = new List<Trap>();

            Monsters = new List<Knight>();
            Archers = new List<Archer>();
            Ghosts = new List<Ghost>();
            Wizards = new List<Wizard>();
            Golems = new List<Golem>();
            Walkers = new List<Walker>();

            LevelTime = new Clock();

            _texBackground = new Texture(@"img/tiles.png", new IntRect(new Vector2i(32, 0), new Vector2i(32, 32)));
            _texBackground.Repeated = true;

            _levelDescription = new TextLine("", 30, -100, -100, Color.White);
            _levelDescription.SetOutlineThickness(3);

            _background = new Sprite(_texBackground);

            LevelLenght = 0;
            LevelHeight = 0;
            LevelNumber = 1;

            StartScore = 0;
            StartArrow = 0;
            StartCoins = 0;
            StartMana = 0;

            isShopOpened = false;
        }

        public static bool IsLeverOn { get; set; }
        public Vector2f EnterancePosition { get; private set; }
        public Vector2f ExitPosition { get; private set; }
        public Vector2f tp1Position { get; private set; }
        public Vector2f tp2Position { get; private set; }
        public Vector2f tp3Position { get; private set; }
        public Vector2f tp4Position { get; private set; }
        public Clock LevelTime { get; set; }
        public int MonsterCount { get; set; }
        public string Content { get; set; }
        public int LevelLenght { get; private set; }
        public int LevelWidth { get; private set; }
        public int LevelHeight { get; private set; }
        public static int LevelNumber;
        public int StartScore { get; set; }
        public int StartCoins { get; set; }
        public int StartMana { get; set; }
        public int StartArrow { get; set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_background);
            foreach (var i in Traps) target.Draw(i);

            foreach (var i in LevelObstacles)
            {
                target.Draw(i);
                if (i.Type == BlockType.Hint) target.Draw(i.Hint);
            }

            //monsters
            foreach (var i in Monsters) target.Draw(i);
            foreach (var i in Archers) target.Draw(i);
            foreach (var i in Ghosts) target.Draw(i);
            foreach (var i in Wizards) target.Draw(i);
            foreach (var i in Golems) target.Draw(i);
            foreach (var i in Walkers) target.Draw(i);
            //

            foreach (var i in Particles) target.Draw(i);
            foreach (var i in ScoreAdditionEffects) target.Draw(i);

            target.Draw(_levelDescription);
        }

        public void LoadLevel(string level)
        {
            Levers.Clear();
            SteelGates.Clear();

            switch (LevelGameMode)
            {
                case GameMode.Adventure:
                    {
                        if (LevelNumber >= 50) LevelColor = Color.Magenta;
                        else if (LevelNumber >= 40) LevelColor = Color.Red;
                        else if (LevelNumber >= 30) LevelColor = new Color(75, 75, 75);
                        else if (LevelNumber >= 20) LevelColor = Color.Cyan;
                        else if (LevelNumber >= 10) LevelColor = Color.Yellow;
                        else LevelColor = Color.White;
                        break;
                    }
                case GameMode.Challenge:
                    {
                        LevelColor = new Color((byte)(MainGameWindow.Randomizer.Next(200) + 50), (byte)(MainGameWindow.Randomizer.Next(200) + 50), (byte)(MainGameWindow.Randomizer.Next(200) + 50));
                        break;
                    }
                default:
                    {
                        LevelColor = Color.White;
                        break;
                    }
            }
            _background.Color = LevelColor;

            ScoreAdditionEffects.Clear();
            Particles.Clear();

            LevelObstacles.Clear();
            Traps.Clear();

            Monsters.Clear();
            Archers.Clear();
            Ghosts.Clear();
            Wizards.Clear();
            Golems.Clear();
            Walkers.Clear();

            UnableToPassl = new List<BlockType>
            {
                BlockType.Dirt, BlockType.Brick, BlockType.Wood, BlockType.Stone, BlockType.GoldDoor, BlockType.SilverDoor,
                BlockType.CrystalDoor,
                BlockType.TransparentBrick, BlockType.HardBlock, BlockType.SteelGate, BlockType.EnergyBall, BlockType.BrokenBrick, BlockType.SpectralCrystal
            };

            isShopOpened = false;

            LevelHeight = 0;
            LevelLenght = 0;

            MonsterCount = 0;

            var hintNumber = 0;

            try
            {
                switch (LevelGameMode)
                {
                    case GameMode.Adventure:
                        {
                            Content = File.ReadAllText(@"levels/" + level + @".dat");
                            break;
                        }
                    case GameMode.Challenge:
                        {
                            Content = File.ReadAllText(@"levels/challenges/" + ChallengeName + @".dat");
                            break;
                        }
                    default:
                        {
                            Content = File.ReadAllText(@"levels/" + level + @".dat");
                            break;
                        }
                }
            }
            catch (Exception)
            {
                _mainCharacter.OutOfLives = true;
                LevelNumber--;
                return;
            }

            var X = 0;
            var Y = 0;

            foreach (var tile in Content)
            {
                //air and bricks
                if (tile == '\n')
                {
                    LevelHeight++;
                    LevelLenght = X;
                    Y++;
                    X = 0;
                    continue;
                }

                //obstacles
                switch (tile)
                {
                    //standard blocks
                    case 'D':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Dirt));
                            break;
                        }
                    case 'W':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Brick));
                            break;
                        }
                    case '#':
                        {
                            MonsterCount++;
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Spike));
                            break;
                        }
                    case 'x':
                        {
                            MonsterCount++;
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.WoodenSpike));
                            break;
                        }
                    case 'X':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Stone));
                            break;
                        }
                    case 'T':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Trampoline));
                            break;
                        }
                    case 'R':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Wood));
                            break;
                        }
                    case '=':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Illusion));
                            break;
                        }
                    case '~':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.BrokenBrick));
                            break;
                        }
                    case '|':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Purifier));
                            break;
                        }
                    case 'w':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.TransparentBrick));
                            break;
                        }
                    case 'Z':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.HardBlock));
                            break;
                        }
                    case '8':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.SteelGate));
                            break;
                        }
                    case 'l':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Lever));
                            break;
                        }
                    case 'E':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.EnergyBall));
                            break;
                        }
                    case 'F':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.SpectralCrystal));
                            break;
                        }
                    //traps
                    case 'H':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.Crusher));
                            break;
                        }
                    case '_':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.Spikes));
                            break;
                        }
                    case ']':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.BlowTorchRight));
                            break;
                        }
                    case '[':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.BlowTorchLeft));
                            break;
                        }
                    case '{':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.BlowerLeft));
                            break;
                        }
                    case '}':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.BlowerRight));
                            break;
                        }
                    //teleports
                    case '1':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Teleport1));
                            tp1Position = new Vector2f(32 * X, 32 * Y);
                            break;
                        }
                    case '2':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Teleport2));
                            tp2Position = new Vector2f(32 * X, 32 * Y);
                            break;
                        }
                    case '3':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Teleport3));
                            tp3Position = new Vector2f(32 * X, 32 * Y);
                            break;
                        }
                    case '4':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Teleport4));
                            tp4Position = new Vector2f(32 * X, 32 * Y);
                            break;
                        }
                    //drzwi i klucze
                    case 'S':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.SilverDoor));
                            break;
                        }
                    case 's':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.SilverKey));
                            break;
                        }
                    case 'G':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.GoldDoor));
                            break;
                        }
                    case 'g':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.GoldenKey));
                            break;
                        }
                    case 'C':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.CrystalDoor));
                            break;
                        }
                    case 'c':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.CrystalKey));
                            break;
                        }
                    // Monsters
                    case '@':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Monsters.Add(new Knight(32 * X, 32 * Y, Entity.KnightTexture));
                            break;
                        }
                    case '>':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Archers.Add(new Archer(32 * X, 32 * Y, Entity.ArcherTexture, Movement.Right));
                            break;
                        }
                    case '<':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Archers.Add(new Archer(32 * X, 32 * Y, Entity.ArcherTexture, Movement.Left));
                            break;
                        }
                    case 'f':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Ghosts.Add(new Ghost(32 * X, 32 * Y, Entity.GhostTexture));
                            break;
                        }
                    case '%':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Wizards.Add(new Wizard(32 * X, 32 * Y, Entity.WizardTexture));
                            break;
                        }
                    case 'B':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Golems.Add(new Golem(32 * X, 32 * Y, Entity.GolemTexture));
                            break;
                        }
                    case '(':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Walkers.Add(new Walker(32 * X, 32 * Y, Entity.WalkerTexture, Movement.Left));
                            break;
                        }
                    case ')':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                            Walkers.Add(new Walker(32 * X, 32 * Y, Entity.WalkerTexture, Movement.Right));
                            break;
                        }
                    // coins
                    case 'o':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Coin));
                            break;
                        }
                    case '$':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.SackOfGold));
                            break;
                        }
                    // life
                    case 'L':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Life));
                            break;
                        }
                    // score
                    case '0':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Score1000));
                            break;
                        }
                    case '5':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Score5000));
                            break;
                        }
                    //arrows
                    case 'a':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Arrow));
                            break;
                        }
                    case 'A':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.TripleArrow));
                            break;
                        }
                    //mana
                    case 'm':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Mana));
                            break;
                        }
                    case 'M':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.TripleMana));
                            break;
                        }
                    //sword enchant
                    case 'n':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.SwordEnchant));
                            break;
                        }
                    //details
                    case '!':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Warning));
                            break;
                        }
                    case '?':
                        {
                            hintNumber++;
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Hint,
                                hintNumber));
                            break;
                        }
                    case '*':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Torch));
                            break;
                        }
                    case ',':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.EvilEyes));
                            break;
                        }
                    case '\\':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.RSpiderweb));
                            break;
                        }
                    case '/':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.LSpiderweb));
                            break;
                        }
                    case '-':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Grass1));
                            break;
                        }
                    case 'r':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Grass2));
                            break;
                        }
                    case 'Y':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.SmallTree));
                            break;
                        }
                    case 'O':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Rock));
                            break;
                        }
                    case 'b':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Bush));
                            break;
                        }
                    case 'V':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Stalactite));
                            break;
                        }
                    //shop
                    case '+':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Shop));
                            break;
                        }
                    //enterance
                    case 'v':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Enterance));
                            EnterancePosition = new Vector2f(32 * X, 32 * Y);
                            break;
                        }
                    //exit
                    case '^':
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Exit));
                            ExitPosition = new Vector2f(32 * X, 32 * Y);
                            break;
                        }
                    default:
                        {
                            {
                                LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                                break;
                            }
                        }
                }

                X++;
            }

            LevelHeight += 1;
            LevelWidth = LevelLenght - 1;
            _background.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(LevelWidth * 32, LevelHeight * 32));
            MonsterCount = Traps.Count * 2 + Monsters.Count * 3 + Archers.Count * 4 + Ghosts.Count * 5 +
                           Wizards.Count * 6 + Golems.Count * 7 + Walkers.Count * 8;

            _mainCharacter.SetStartingPosition(this);
            _mainCharacter.SafePosition = EnterancePosition;

            StartScore = _mainCharacter.Score;
            StartCoins = _mainCharacter.Coins;
            StartArrow = _mainCharacter.ArrowAmount;
            StartMana = _mainCharacter.Mana;

            _mainCharacter.HasSilverKey = false;
            _mainCharacter.HasGoldenKey = false;

            _levelDescription.MoveText(_view.Center.X - _view.Size.X / 2 - 1000, _view.Center.Y - 100);

            switch (LevelGameMode)
            {
                case GameMode.Adventure:
                    {
                        _levelDescription.EditText($"LEVEL {LevelNumber}");
                        break;
                    }
                case GameMode.Challenge:
                    {
                        _levelDescription.EditText($"CHALLENGE");
                        break;
                    }
                default:
                    {
                        _levelDescription.EditText($"LEVEL {LevelNumber}");
                        break;
                    }
            }

            foreach (var archer in Archers) archer.DefaultClock.Restart();
            foreach (var ghost in Ghosts) ghost.DefaultClock.Restart();
            foreach (var wizard in Wizards) wizard.DefaultClock.Restart();
            foreach (var golem in Golems) golem.DefaultClock.Restart();
            foreach (var walker in Walkers) walker.DefaultClock.Restart();
            foreach (var trap in Traps) trap.DefaultTimer.Restart();

            LevelTime.Restart();
        }

        public Block GetObstacle(float x, float y)
        {
            try
            {
                return LevelObstacles[(int)y * LevelLenght + (int)x];
            }
            catch (Exception)
            {
                return new Block(-100, -100, null);
            }
        }

        public bool UnpassableContains(BlockType type)
        {
            return UnableToPassl.Contains(type);
        }

        public void LevelUpdate(bool isLevelEditor = false)
        {
            foreach (var obstacle in LevelObstacles)
            {
                if (AnimateableBlocks.Contains(obstacle.Type))
                    obstacle.BlockAnimation.Animate();

                if (obstacle.Type == BlockType.Hint &&
                    !_mainCharacter.GetBoundingBox().Intersects(obstacle.GetBoundingBox())) HideHint(obstacle);


                obstacle.BlockUpdate(this);

                Block.LeverMechanismUpdate();

                if (obstacle.Type == BlockType.Trampoline)
                    if (obstacle.DefaultTimer.ElapsedTime.AsSeconds() > 0.1f)
                        obstacle.SetTextureRectangle(64, 32);
            }

            if (isLevelEditor == false)
            {
                foreach (var trap in Traps) trap.TrapUpdate();

                foreach (var effect in Particles) effect.MakeParticles();

                foreach (var monster in Monsters) monster.UpdateCreature();
                foreach (var archer in Archers) archer.UpdateCreature(this);
                foreach (var ghost in Ghosts) ghost.UpdateCreature(this, _mainCharacter);
                foreach (var wizard in Wizards) wizard.UpdateCreature(_mainCharacter, this);
                foreach (var golem in Golems) golem.UpdateCreature(_mainCharacter, this);
                foreach (var walker in Walkers) walker.UpdateCreature(_mainCharacter, this);
            }

            //details
            //text slide effect
            if (isLevelEditor == false)
            {
                if (LevelTime.ElapsedTime.AsSeconds() > 5)
                    _levelDescription.MoveText(-100, -100);
                else if (LevelTime.ElapsedTime.AsSeconds() > 4)
                    _levelDescription.MoveText(_levelDescription.X + 50, _view.Center.Y - 100);
                else if (LevelTime.ElapsedTime.AsSeconds() > 0.5f)
                    _levelDescription.MoveText(_view.Center.X - 1.2f * _levelDescription.Width / 2,
                        _view.Center.Y - 100);
                else if (_levelDescription.X < _view.Center.X - 1.2f * _levelDescription.Width / 2)
                    _levelDescription.MoveText(_levelDescription.X + 50, _view.Center.Y - 100);
            }

            try
            {
                for (var i = 0; i < Particles.Count; i++)
                    if (Particles[i].ToDestroy)
                    {
                        Particles.RemoveAt(i);
                        if (i > 0) i--;
                    }

                for (var i = 0; i < ScoreAdditionEffects.Count; i++)
                {
                    ScoreAdditionEffects[i].UpdateScoreAdditionEffect();
                    if (ScoreAdditionEffects[i].toDestroy)
                    {
                        ScoreAdditionEffects.RemoveAt(i);
                        if (i > 0) i--;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("ERROR: Textural detail list index out of range");
                Particles.Clear();
                ScoreAdditionEffects.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: Textural detail list fatal error");
                Particles.Clear();
                ScoreAdditionEffects.Clear();
            }
        }

        public int GetBonusForTime(double time)
        {
            var value = 0f;
            var points = 100 * (int)((LevelWidth * LevelWidth + MonsterCount) / time);

            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                    {
                        value = 0.8f;
                        break;
                    }
                case Difficulty.Medium:
                    {
                        value = 1f;
                        break;
                    }
                case Difficulty.Hard:
                    {
                        value = 1.2f;
                        break;
                    }
            }

            points = (int)(points * value);
            return points < 0 ? 0 : points - points % 10;
        }

        public void SetHints(Block obstacle)
        {
            switch (LevelNumber)
            {
                case 1: // LEVEL 1
                    {
                        switch (obstacle.HintNumber)
                        {
                            case 1:
                                {
                                    ShowHint(obstacle,
                                        "AND THERE IS YOUR GOAL!\n" +
                                        $"PRESS '{MainCharacter.KeyUP.ToString().ToUpper()}' TO INTERACT\n" +
                                        "WITH DIFFERENT OBJECTS."
                                        , -60, -26);
                                    break;
                                }
                            case 2:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "B";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyTHUNDER.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        $"PRESS '{tmp}' TO SHOOT\n" +
                                        "AN ENCHANTED ARROW, THAT CAN\n" +
                                        "PIERCE THROUGH FLESH,\n" +
                                        "MANA POTION IS REQUIRED"
                                        , -50, -34);
                                    break;
                                }
                            case 3:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "L1 OR R1";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyIMMORTALITY.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        "MANA CAN ALSO BE USED\n" +
                                        "TO MAKE YOU IMMORTAL FOR\n" +
                                        $"SEVERAL SECONDS BY PRESSING '{tmp}'.\n" +
                                        "IT CONSUMES 3 MANA POTIONS,"
                                        , -60, -34);
                                    break;
                                }
                            case 4:
                                {
                                    ShowHint(obstacle,
                                        "IF YOU SOMEHOW GET STUCK,\n" +
                                        $"IT IS ALWAYS POSSIBLE TO\n" +
                                        $"RESTART ANY LEVEL."
                                        , -80, -26);
                                    break;
                                }
                            case 5:
                                {
                                    ShowHint(obstacle,
                                        "FURTHER LEVELS WILL REVEAL\n" +
                                        "MORE INTERESTING MECHANICS."
                                        , -50, -18);
                                    break;
                                }
                            case 6:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "Y";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyARROW.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        "THERE ARE A LOT OF DIFFERENT\n" +
                                        "CREATURES. THOSE KNIGHTS JUST WALK AROUND\n" +
                                        "FOR NO REASON... THIS ONE IS TO FAR FROM YOU\n" +
                                        $"TO USE YOUR SWORD, SO PRESS '{tmp}'\n" +
                                        "TO SHOT AN ARROW."
                                        , -40, -42);
                                    break;
                                }
                            case 7:
                                {
                                    ShowHint(obstacle,
                                        "STOMPING ON STONES WILL CAUSE\n" +
                                        "TO CRUMBLE AFTER FEW SECONDS.\n" +
                                        "THEY RESPAWN LATER ON."
                                        , -90, -26);
                                    break;
                                }
                            case 8:
                                {
                                    ShowHint(obstacle,
                                        "LET US BE HONEST,\n" +
                                        "THIS IN NOT A FRIENDLY PLACE,\n" +
                                        "TRAPS, SPIKES, FLAMES, MONSTERS..."
                                        , -80, -26);
                                    break;
                                }
                            case 9:
                                {
                                    ShowHint(obstacle,
                                        "SOMETIMES A WALL CAN BE\n" +
                                        "AN ILLUSION, WHICH LEADS TO SECRET\n" +
                                        "ROOMS OR TREASURES, OR MADNESS..."
                                        , -80, -26);
                                    break;
                                }
                            case 10:
                                {
                                    ShowHint(obstacle,
                                        "THERE ARE A LOT OF PICKUPS.\n" +
                                        "ONE OF THEM ARE THOSE COINS,\n" +
                                        "PERHAPS YOU CAN BUY SOMETHING..."
                                        , -80, -26);
                                    break;
                                }
                            case 11:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "X";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyATTACK.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        "WHILE IN MIDAIR,\n" +
                                        $"PRESS '{MainCharacter.KeyDOWN.ToString().ToUpper()}' AND '{tmp}' TO PERFORM\n" +
                                        "DOWNWARDS ATTACK."
                                        , -40, -18);
                                    break;
                                }
                            case 12:
                                {
                                    ShowHint(obstacle,
                                        "WELCOME TO THE GAME!\n" +
                                        $"PRESS '{MainCharacter.KeyLEFT.ToString().ToUpper()}' OR '{MainCharacter.KeyRIGHT.ToString().ToUpper()}' TO MOVE."
                                        , -80, -18);
                                    break;
                                }
                            case 13:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "A";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyJUMP.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        $"PRESS '{tmp}' TO JUMP."
                                        , -50, -10);
                                    break;
                                }
                            case 14:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "X";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyATTACK.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        "CRATES CAN BE EASILY DESTROYED.\n" +
                                        $"PRESS '{tmp}' TO ATTACK."
                                        , -80, -18);
                                    break;
                                }
                            case 15:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "X";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyATTACK.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        $"PRESS '{MainCharacter.KeyUP.ToString().ToUpper()}' AND '{tmp}' TO\n" +
                                        "PERFORM UPWARDS ATTACK."
                                        , -50, -18);
                                    break;
                                }
                            case 16:
                                {
                                    string tmp;
                                    if (MainGameWindow.IsControllerConnected)
                                    {
                                        tmp = "A";
                                    }
                                    else
                                    {
                                        tmp = MainCharacter.KeyJUMP.ToString().ToUpper();
                                    }
                                    ShowHint(obstacle,
                                        "TO JUMP HIGHER,\n" +
                                        "USE TRAMPOLINES.\n" +
                                        $"HOLD '{tmp}' TO JUMP HIGHER.\n" +
                                        $"OR HOLD '{MainCharacter.KeyDOWN.ToString().ToUpper()}' IF YOU\n" +
                                        "DON'T NEED TO BOUNCE."
                                        , -50, -42);
                                    break;
                                }
                        }

                        break;
                    }
                case 5:
                    {
                        ShowHint(obstacle,
                            "THOSE BLOCKS REQUIRE\n" +
                            "MORE EFFORT TO DESTROY..."
                            , -50, -18);
                        break;
                    }
                case 10:
                    {
                        switch (obstacle.HintNumber)
                        {
                            case 1:
                                {
                                    ShowHint(obstacle,
                                        "BE CAREFUL, THOSE ARCHERS\n" +
                                        "GONNA HUNT YOU DOWN!"
                                        , -60, -18);
                                    break;
                                }
                            case 2:
                                {
                                    ShowHint(obstacle,
                                        "POWERFUL MAGICAL BARRIERS,\n" +
                                        "ONLY STRONG MAGIC CAN\n" +
                                        "SHATTER THEM..."
                                        , -60, -26);
                                    break;
                                }
                        }

                        break;
                    }
                case 15:
                    {
                        ShowHint(obstacle,
                            "SOME PARTS OF DUNGEON\n" +
                            "MAY BE LOCKED, BUT PROPER\n" +
                            "KEY CAN UNLOCK THEM."
                            , -60, -24);
                        break;
                    }
                case 20:
                    {
                        switch (obstacle.HintNumber)
                        {
                            case 1:
                                {
                                    ShowHint(obstacle,
                                        "LOOKS LIKE DUNGEON IS HAUNTED!\n" +
                                        "GHOSTS GONNA FOLLOW YOU\n" +
                                        "UNTIL YOU DIE OR...\n" +
                                        "STRONG MAGIC MIGHT HELP..."
                                        , -60, -34);
                                    break;
                                }
                            case 2:
                                {
                                    ShowHint(obstacle,
                                        "WHO WOULD EXPECT A SHOP\n" +
                                        "IN THESE DUNGEONS... "
                                        , -50, -18);
                                    break;
                                }
                        }

                        break;
                    }
                case 25:
                    {
                        switch (obstacle.HintNumber)
                        {
                            case 1:
                            {
                                ShowHint(obstacle,
                                    "ANOTHER SOURCE OF\n" +
                                    "STRONG MAGIC! GREAT NEWS!\n" +
                                    "AND MORE POSSIBILITIES..."
                                    , -60, -26);
                                break;
                            }
                            case 2:
                            {
                                ShowHint(obstacle,
                                    "WHAT ARE THOSE STRANGE\n" +
                                    "PORTALS... NO ONE KNOWS\n" +
                                    "WHAT WAITS FOR YOU\n" +
                                    "ON THE OTHER SIDE."
                                    , -60, -34);
                                break;
                            }
                        }
                        break;
                    }
                case 30:
                    {
                        switch (obstacle.HintNumber)
                        {
                            case 1:
                                {
                                    ShowHint(obstacle,
                                        "OLD STEEL GATES AND\n" +
                                        "RUSTY LEVERS, IF MECHANISM\n" +
                                        "STILL WORSK, IT MIGHT OPEN\n" +
                                        "ALL THOSE HEAVY GATES."
                                        , -60, -34);
                                    break;
                                }
                            case 2:
                                {
                                    ShowHint(obstacle,
                                        "YOU ARE NOT THE ONLY\n" +
                                        "ONE WHO CAN USE STRONG MAGIC.\n" +
                                        "POWERFUL MAGICIANS, MIGHTY WIZARDS\n" +
                                        "AND THEIR DREADFUL SPELLS..."
                                        , -70, -34);
                                    break;
                                }
                        }

                        break;
                    }
                case 35:
                    {
                        ShowHint(obstacle,
                            "THOSE GATES WILL NOT\n" +
                            "HELP EITHER... FROM A DISTANCE\n" +
                            "IT FEELS LIKE IT IS GOING TO\n" +
                            "TAKE EVERYTHING FROM YOU..."
                            , -70, -34);
                        break;
                    }
                case 40:
                    {
                        ShowHint(obstacle,
                            "GOLEMS ARE RESPONSIBLE FOR\n" +
                            "THAT EARTH QUAKE! THE STRONGEST\n" +
                            "CREATURE THAT LIVES HERE...\n" +
                            "BUT WHO HAS SUMMONED THEM?"
                            , -70, -34);
                        break;
                    }
            }
        }

        public void HideHint(Block obstacle)
        {
            if (obstacle.Hint.Alpha > 15) obstacle.Hint.Alpha -= 15;
            if (obstacle.Hint.Alpha <= 15) obstacle.Hint.MoveText(-100, -100);
        }

        public void ShowHint(Block obstacle, string hint, float xMod, float yMod)
        {
            obstacle.Hint.EditText(hint);
            obstacle.Hint.MoveText(obstacle.X + xMod, obstacle.Y + yMod);
            if (obstacle.Hint.Alpha < 245) obstacle.Hint.Alpha += 15;
        }

        public void AddParticleEffect(ParticleEffect effect)
        {
            Particles.Add(effect);
        }

        public void SaveLevel(bool saveChall = false)
        {
            BlockType type;
            var level = new StringBuilder();
            var tile = 0; //x
            var y = 0;
            var monsterFlag = false;

            foreach (var obstacle in LevelObstacles)
            {
                type = obstacle.Type;
                monsterFlag = false;

                //monsters
                foreach (var i in Monsters)
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    {
                        level.Append("@");
                        monsterFlag = true;
                    }

                foreach (var i in Archers)
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    {
                        if (i.Direction == Movement.Left)
                        {
                            level.Append("<");
                            monsterFlag = true;
                        }
                        else
                        {
                            level.Append(">");
                            monsterFlag = true;
                        }
                    }

                foreach (var i in Ghosts)
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    {
                        level.Append("f");
                        monsterFlag = true;
                    }

                foreach (var i in Wizards)
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    {
                        level.Append("%");
                        monsterFlag = true;
                    }

                foreach (var i in Golems)
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    {
                        level.Append("B");
                        monsterFlag = true;
                    }

                foreach (var i in Walkers)
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    {
                        if (i.Direction == Movement.Left)
                        {
                            level.Append("(");
                            monsterFlag = true;
                        }
                        else
                        {
                            level.Append(")");
                            monsterFlag = true;
                        }
                    }

                //traps
                foreach (var i in Traps)
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                        switch (i.Type)
                        {
                            case TrapType.Crusher:
                                {
                                    level.Append("H");
                                    monsterFlag = true;
                                    break;
                                }
                            case TrapType.Spikes:
                                {
                                    level.Append("_");
                                    monsterFlag = true;
                                    break;
                                }
                            case TrapType.BlowTorchLeft:
                                {
                                    level.Append("[");
                                    monsterFlag = true;
                                    break;
                                }
                            case TrapType.BlowTorchRight:
                                {
                                    level.Append("]");
                                    monsterFlag = true;
                                    break;
                                }
                            case TrapType.BlowerLeft:
                                {
                                    level.Append("{");
                                    monsterFlag = true;
                                    break;
                                }
                            case TrapType.BlowerRight:
                                {
                                    level.Append("}");
                                    monsterFlag = true;
                                    break;
                                }
                        }

                if (monsterFlag)
                {
                    tile++;
                    if (tile == LevelWidth + 1)
                    {
                        monsterFlag = false;
                        level.Remove(level.Length - 1, 1);
                        level.Append("\r\n");
                        tile = 0;
                        y++;
                    }

                    continue;
                }

                switch (type)
                {
                    case BlockType.Dirt:
                        {
                            level.Append("D");
                            break;
                        }
                    case BlockType.Brick:
                        {
                            level.Append("W");
                            break;
                        }
                    case BlockType.TransparentBrick:
                        {
                            level.Append("w");
                            break;
                        }
                    case BlockType.Spike:
                        {
                            level.Append("#");
                            break;
                        }
                    case BlockType.WoodenSpike:
                        {
                            level.Append("x");
                            break;
                        }
                    case BlockType.HardBlock:
                        {
                            level.Append("Z");
                            break;
                        }
                    case BlockType.EnergyBall:
                        {
                            level.Append("E");
                            break;
                        }
                    case BlockType.SpectralCrystal:
                        {
                            level.Append("F");
                            break;
                        }
                    case BlockType.Enterance:
                        {
                            level.Append("v");
                            break;
                        }
                    case BlockType.Shop:
                        {
                            level.Append("+");
                            break;
                        }
                    case BlockType.Coin:
                        {
                            level.Append("o");
                            break;
                        }
                    case BlockType.Life:
                        {
                            level.Append("L");
                            break;
                        }
                    case BlockType.SwordEnchant:
                        {
                            level.Append("n");
                            break;
                        }
                    case BlockType.Arrow:
                        {
                            level.Append("a");
                            break;
                        }
                    case BlockType.TripleArrow:
                        {
                            level.Append("A");
                            break;
                        }
                    case BlockType.TripleMana:
                        {
                            level.Append("M");
                            break;
                        }
                    case BlockType.Score1000:
                        {
                            level.Append("0");
                            break;
                        }
                    case BlockType.Mana:
                        {
                            level.Append("m");
                            break;
                        }
                    case BlockType.Score5000:
                        {
                            level.Append("5");
                            break;
                        }
                    case BlockType.Stone:
                        {
                            level.Append("X");
                            break;
                        }
                    case BlockType.Illusion:
                        {
                            level.Append("=");
                            break;
                        }
                    case BlockType.BrokenBrick:
                        {
                            level.Append("~");
                            break;
                        }
                    case BlockType.Purifier:
                        {
                            level.Append('|');
                            break;
                        }
                    case BlockType.SteelGate:
                        {
                            level.Append('8');
                            break;
                        }
                    case BlockType.Lever:
                        {
                            level.Append('l');
                            break;
                        }
                    case BlockType.Wood:
                        {
                            level.Append("R");
                            break;
                        }
                    case BlockType.Trampoline:
                        {
                            level.Append("T");
                            break;
                        }
                    case BlockType.Exit:
                        {
                            level.Append("^");
                            break;
                        }
                    case BlockType.SilverDoor:
                        {
                            level.Append("S");
                            break;
                        }
                    case BlockType.SilverKey:
                        {
                            level.Append("s");
                            break;
                        }
                    case BlockType.GoldDoor:
                        {
                            level.Append("G");
                            break;
                        }
                    case BlockType.GoldenKey:
                        {
                            level.Append("g");
                            break;
                        }
                    case BlockType.CrystalDoor:
                        {
                            level.Append("C");
                            break;
                        }
                    case BlockType.CrystalKey:
                        {
                            level.Append("c");
                            break;
                        }
                    case BlockType.SackOfGold:
                        {
                            level.Append("$");
                            break;
                        }
                    //teleports
                    case BlockType.Teleport1:
                        {
                            level.Append("1");
                            break;
                        }
                    case BlockType.Teleport2:
                        {
                            level.Append("2");
                            break;
                        }
                    case BlockType.Teleport3:
                        {
                            level.Append("3");
                            break;
                        }
                    case BlockType.Teleport4:
                        {
                            level.Append("4");
                            break;
                        }
                    case BlockType.Warning:
                        {
                            level.Append("!");
                            break;
                        }
                    case BlockType.Hint:
                        {
                            level.Append("?");
                            break;
                        }
                    case BlockType.LSpiderweb:
                        {
                            level.Append("/");
                            break;
                        }
                    case BlockType.RSpiderweb:
                        {
                            level.Append("\\");
                            break;
                        }
                    case BlockType.Grass1:
                        {
                            level.Append('-');
                            break;
                        }
                    case BlockType.Grass2:
                        {
                            level.Append("r");
                            break;
                        }
                    case BlockType.Torch:
                        {
                            level.Append("*");
                            break;
                        }
                    case BlockType.EvilEyes:
                        {
                            level.Append(",");
                            break;
                        }
                    case BlockType.SmallTree:
                        {
                            level.Append("Y");
                            break;
                        }
                    case BlockType.Rock:
                        {
                            level.Append("O");
                            break;
                        }
                    case BlockType.Bush:
                        {
                            level.Append("b");
                            break;
                        }
                    case BlockType.Stalactite:
                        {
                            level.Append("V");
                            break;
                        }
                    case BlockType.None:
                        {
                            level.Append(".");
                            break;
                        }
                }

                tile++;
                if (tile == LevelWidth + 1)
                {
                    level.Remove(level.Length - 1, 1);
                    level.Append("\r\n");
                    tile = 0;
                    y++;
                }
            }

            File.WriteAllText("levels/edit.dat", level.ToString());
            var number = 0;
            var name = "DUNGEON " + number;
            while (File.Exists(@"levels/challenges/" + name + ".dat"))
            {
                number++;
                name = "DUNGEON " + number;
            }
            if (saveChall == true) File.WriteAllText("levels/challenges/" + name + ".dat", level.ToString());
        }

        private Sprite _background;
        private readonly MainCharacter _mainCharacter;
        private Texture _texBackground;
        private readonly View _view;
        private readonly TextLine _levelDescription;
        private readonly Random rnd;
    }
}