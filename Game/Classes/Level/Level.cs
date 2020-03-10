using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SFML.Graphics;
using SFML.System;

/*

Level creating rules:
- min width:	30 tiles (1920px)
- min height:	17 tiles (1088px)
- has to contain exactly 1 enterance and 1 exit gate
- jump height: 4
- trampoline jump height: 9
- or just use level editor XD

Mechanics introduction <level/> <new/>:
1  :coins, score, life pickup and basic tiles
5  :monsters (knight), fake bricks
10 :monsters (archers), arrow+, 3arrow+
15 :silver key gates, mana, immortality
20 :teleports, monsters (ghost)
25 :golden key gates, petrifiers
30 :monsters (wizard)
Max 50 lvls;

Tiles:
. :empty space / air (can be white space)
W :wall / brick (unpassable)
v :enterance
^ :exit
+ :shop
o :coin
| :putifier
$ :sack of gold
# :spikes (kills immidietly)
R :wood (destructible)
X :stones (once stomped, disappears after couple of seconds)
L :life
0 :+1000
5 :+5000
m :+1 mana potion
@ :knight (monster moving back and forth)
> :archer shooting right
< :archer shooting left
f :ghost (killable only by energized arrow)
% :wizard (shoots projectiles that follow You)
1 :teleport 1 (teleports to teleport2)
2 :teleport 2 (teleports to teleport1)
3 :teleport 3 (teleports to teleport4)
4 :teleport 4 (teleports to teleport3)
s :silver key (unlocks silver lock)
S :silver lock (unpassable)
g :golden key (unlocks golden lock)
G :golden lock (unpassable)
a :arrow +1
A :arrow +3
H :crusher trap
_ :spikes trap
] :blowtorch blowing right
[ :blowtorch blowing left

*/

namespace ChendiAdventures
{
    public class Level : Drawable
    {
        public readonly List<Archer> Archers;
        public readonly List<Ghost> Ghosts;
        public readonly List<Wizard> Wizards;
        public readonly List<Monster> Monsters;
        public readonly List<Block> LevelObstacles;
        public readonly List<ParticleEffect> Particles;
        public readonly List<ScoreAdditionEffect> ScoreAdditionEffects;
        public readonly List<Trap> Traps;
       
        private readonly Sprite _background;
        private readonly MainCharacter _mainCharacter;
        private readonly View _view;
        private readonly Texture _texBackground;
        private TextLine _levelDescription;
        private Random rnd;

        public List<BlockType> UnableToPassl;
        public bool isShopOpened;

        public Level(MainCharacter character, View view)
        {
            _mainCharacter = character;
            _view = view; 
            rnd = new Random();

            ScoreAdditionEffects = new List<ScoreAdditionEffect>();
            Particles = new List<ParticleEffect>();

            LevelObstacles = new List<Block>();
            Traps = new List<Trap>();

            Monsters = new List<Monster>();
            Archers = new List<Archer>();
            Ghosts = new List<Ghost>();
            Wizards = new List<Wizard>();

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
            StartArrows = 0;
            StartMana = 0;
            StartCoins = 0;

            isShopOpened = false;
        }
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
        public int LevelNumber { get; set; }
        public int StartScore { get; set; }
        public int StartCoins { get; set; }
        public int StartArrows { get; set; }
        public int StartMana { get; set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_background);
            foreach (var i in Traps) target.Draw(i, states);

            foreach (var i in LevelObstacles)
            {
                target.Draw(i, states);
                if (i.Type == BlockType.Hint) target.Draw(i.Hint, states);
            }
            //monsters
            foreach (var i in Monsters) target.Draw(i, states);
            foreach (var i in Archers) target.Draw(i, states);
            foreach (var i in Ghosts) target.Draw(i, states);
            foreach (var i in Wizards) target.Draw(i, states);
            //

            foreach (var i in Particles) target.Draw(i, states);
            foreach (var i in ScoreAdditionEffects) target.Draw(i, states);

            target.Draw(_levelDescription);
        }

        public void LoadLevel(string level)
        {
            //Console.WriteLine("Level load start...");

            ScoreAdditionEffects.Clear();
            Particles.Clear();

            LevelObstacles.Clear();
            Traps.Clear();
            //this.Creatures.Clear();
            //this.Projectiles.Clear();

            Monsters.Clear();
            Archers.Clear();
            Ghosts.Clear();
            Wizards.Clear();

            UnableToPassl = new List<BlockType>
                {BlockType.Brick, BlockType.Wood, BlockType.Stone, BlockType.GoldDoor, BlockType.SilverDoor};

            //Console.WriteLine("Entity lists cleared");

            isShopOpened = false;

            LevelHeight = 0;
            LevelLenght = 0;

            MonsterCount = 0;

            var hintNumber = 0;

            try
            {
                Content = File.ReadAllText(@"levels/" + level + @".dat");
            }
            catch (Exception)
            {
                _mainCharacter.OutOfLives = true;
                LevelNumber--;
                return;
            }

            var X = 0;
            var Y = 0;

            //Console.WriteLine("Level txt loaded, generating level...");

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
                    case '|':
                    {
                        LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Purifier));
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
                    // Monsters
                    case '@':
                    {
                        LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                        Monsters.Add(new Monster(32 * X, 32 * Y, Entity.KnightTexture));
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
                        LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Grass));
                        break;
                    }
                    //shop
                    case '+':
                    {
                        int chance = rnd.Next(3) + 1;

                        if (LevelNumber == 20)
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Shop));
                        }
                        else if (chance == 2)
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Shop));
                        }
                        else
                        {
                            LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture));
                        }
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

            //Console.WriteLine("Done. Calculating additional values...");

            LevelHeight += 1;
            LevelWidth = LevelLenght - 1;
            _background.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(LevelWidth * 32, LevelHeight * 32));
            MonsterCount = Traps.Count * 2 + Monsters.Count * 3 + Archers.Count * 4 + Ghosts.Count * 5 + Wizards.Count * 6;

            _mainCharacter.SetStartingPosition(this);

            StartScore = _mainCharacter.Score;
            StartCoins = _mainCharacter.Coins;
            StartArrows = _mainCharacter.ArrowAmount;
            StartMana = _mainCharacter.Mana;

            _mainCharacter.HasSilverKey = false;
            _mainCharacter.HasGoldenKey = false;

            _levelDescription.MoveText(_view.Center.X - _view.Size.X/2 - 1000, _view.Center.Y - 100);
            _levelDescription.EditText($"LEVEL {LevelNumber}");

            //Console.WriteLine("Level {0} loaded succesfully ({1}:{2}  MC value: {3})", level, this.LevelWidth, this.LevelHeight, this.MonsterCount);
            //File.AppendAllText("log.txt", string.Format("Level {0} loaded succesfully ({1}:{2}  MC value: {3})\n", level, this.LevelWidth, this.LevelHeight, this.MonsterCount));
            LevelTime.Restart();

            //Console.WriteLine("Done");
        }

        public Block GetObstacle(float x, float y)
        {
            return LevelObstacles[(int) y * LevelLenght + (int) x];
        }

        public bool UnpassableContains(BlockType type)
        {
            if (UnableToPassl.Contains(type)) return true;
            return false;
        }

        public void LevelUpdate(bool isLevelEditor = false)
        {
            foreach (var obstacle in LevelObstacles)
            {
                if (obstacle.Type == BlockType.Coin || obstacle.Type == BlockType.SilverKey ||
                    obstacle.Type == BlockType.GoldenKey || obstacle.Type == BlockType.Life ||
                    obstacle.Type == BlockType.Score5000 || obstacle.Type == BlockType.Arrow ||
                    obstacle.Type == BlockType.TripleArrow || obstacle.Type == BlockType.Score1000 ||
                    obstacle.Type == BlockType.Mana || obstacle.Type == BlockType.Torch ||
                    obstacle.Type == BlockType.TripleMana || obstacle.Type == BlockType.SackOfGold ||
                    obstacle.Type == BlockType.Exit || obstacle.Type == BlockType.Teleport1 ||
                    obstacle.Type == BlockType.Teleport2 || obstacle.Type == BlockType.Purifier ||
                    obstacle.Type == BlockType.Teleport3 || obstacle.Type == BlockType.Teleport4)
                    obstacle.BlockAnimation.Animate();

                if (obstacle.Type == BlockType.Hint && !_mainCharacter.GetBoundingBox().Intersects(obstacle.GetBoundingBox())) HideHint(obstacle);

                if (obstacle.Type == BlockType.Stone)
                {
                    obstacle.StoneUpdate();
                    if (obstacle.IsDestroyed)
                        Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y,
                            new Color(150, 150, 150)));
                }

                if (obstacle.Type == BlockType.Trampoline)
                    if (obstacle.DefaultTimer.ElapsedTime.AsSeconds() > 0.1f)
                        obstacle.SetTextureRectanlge(64, 32, 32, 32);
            }

            if (isLevelEditor == false)
            {
                foreach (var trap in Traps) trap.TrapUpdate();

                foreach (var effect in Particles) effect.MakeParticles();

                foreach (var monster in Monsters) monster.UpdateCreature();
                foreach (var archer in Archers) archer.UpdateCreature(this);
                foreach (var ghost in Ghosts) ghost.UpdateCreature(this, _mainCharacter);
                foreach (var wizard in Wizards) wizard.UpdateCreature(_mainCharacter);
            }

            //details
            //text slide effect
            if (isLevelEditor == false)
            {
                if (LevelTime.ElapsedTime.AsSeconds() < 0.5f)_levelDescription.MoveText(_view.Center.X - _view.Size.X/2 - 300, _view.Center.Y - 100);
                if (_levelDescription.X < _view.Center.X - 1.2f * _levelDescription.Width / 2 && LevelTime.ElapsedTime.AsSeconds() > 0.5f)
                { _levelDescription.MoveText(_levelDescription.X + 50, _levelDescription.Y); }
                else if (LevelTime.ElapsedTime.AsSeconds() < 2.5f && LevelTime.ElapsedTime.AsSeconds() > 0.5f)
                { _levelDescription.MoveText(_view.Center.X - 1.2f * _levelDescription.Width / 2, _view.Center.Y - 100); }

                if (_levelDescription.X < _view.Center.X + _view.Size.X / 2 + 500 && LevelTime.ElapsedTime.AsSeconds() > 2.5f)
                { _levelDescription.MoveText(_levelDescription.X + 50, _levelDescription.Y); }
                if (LevelTime.ElapsedTime.AsSeconds() > 5)
                { _levelDescription.MoveText(-100, -100); }
            } 

            try
            {
                for (var i = 0; i < Particles.Count; i++)
                    if (Particles[i].Timer.ElapsedTime.AsSeconds() > 5)
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

        public int GetBonusForTime(double time) // do zmiany!!!!!
        {
            var value = 0f;
            var points = (int) ((LevelWidth * LevelHeight + MonsterCount * LevelNumber) / time) * LevelNumber * 10;

            switch (MainGameWindow.GameDifficulty)
            {
                case Difficulty.Easy:
                {
                    value = 0.75f;
                    break;
                }
                case Difficulty.Medium:
                {
                    value = 1f;
                    break;
                }
                case Difficulty.Hard:
                {
                    value = 1.3f;
                    break;
                }
            }

            points = (int)(points * value);
            return points < 0 ? 0 : points - points % 10;
        }

        public void SetHints(Block obstacle, MainCharacter character)
        {
            switch (LevelNumber)
            {
                case 1: // LEVEL 1
                {
                    switch (obstacle.HintNumber)
                    {
                        case 1:
                        {
                            ShowHint(obstacle, "STONE CRUBLES AFTER\nCOUPLE OF SECONDS...", -40, -22);
                            break;
                        }
                        case 2:
                        {
                            ShowHint(obstacle, "ALMOST THERE...", -40, -10);
                            break;
                        }
                        case 3:
                        {
                            ShowHint(obstacle, "USE TRAMPOLINE\nTO GET HIGHER", -40, -22);
                            break;
                        }
                        case 4:
                        {
                            ShowHint(obstacle, "THERE ARE A LOT\nOF OTHER PICKUPS...", -40, -22);
                            break;
                        }
                        case 5:
                        {
                            ShowHint(obstacle, "NOW PICKUP THOSE COINS", -50, -10);
                            break;
                        }
                        case 6:
                        {
                            ShowHint(obstacle, "WELCOME TO CHENDI ADVENTURES!\nUSE ARROW KEYS TO MOVE", -60, -22);
                            break;
                        }
                        case 7:
                        {
                            ShowHint(obstacle, string.Format("PRESS '{0}' TO JUMP", character.KeyJUMP.ToString()), -50,
                                -10);
                            break;
                        }
                        case 8:
                        {
                            ShowHint(obstacle, string.Format("PRESS '{0}' TO ATTACK", character.KeyATTACK.ToString()),
                                -50, -10);
                            break;
                        }
                        case 9:
                        {
                            ShowHint(obstacle,
                                string.Format("PRESS '{0}' TO SHOOT AN ARROW", character.KeyARROW.ToString()), -70,
                                -10);
                            break;
                        }
                        case 10:
                        {
                            ShowHint(obstacle, "BE CAREFUL, CUZ\nSPIKES HURTS\nOTRER TRAPS TOO", -50, -34);
                            break;
                        }
                    }

                    break;
                }
                case 2: // LEVEL 2
                {
                    switch (obstacle.HintNumber)
                    {
                        case 1:
                        {
                            ShowHint(obstacle,
                                string.Format("IF YOU FALL INTO A TRAP,\nJUST KYS BY PRESSING '{0}'",
                                    character.KeyDIE.ToString()), -60, -22);
                            break;
                        }
                    }

                    break;
                }
                case 3: // LEVEL 3
                {
                    switch (obstacle.HintNumber)
                    {
                        case 1:
                        {
                            ShowHint(obstacle, "THERE MUST BE THE WAY\nTO GET INSIDE...", -60, -22);
                            break;
                        }
                    }

                    break;
                }
                case 5:
                {
                    ShowHint(obstacle, "ROGUE KNIGHTS...\nSOMETIMES THEY'RE GONNA\nBLOCK YOUR WAY", -60, -34);
                    break;
                }
                case 10:
                {
                    ShowHint(obstacle, "ARCHERS...\nWATCH OUT", -40, -22);
                    break;
                }
                case 15:
                {
                    switch (obstacle.HintNumber)
                    {
                        case 1:
                        {
                            ShowHint(obstacle, "KEYS OPEN CERTAIN DOOR\nTHAT IS OBVIOUS", -50, -22);
                            break;
                        }
                        case 2:
                        {
                            ShowHint(obstacle, string.Format(
                                    "MANA POTION CAN BE USED IN TWO WAYS:\nTO ENCHANT YOUR ARROWS BY PRESSING '{0}'\n" +
                                    "OR BECOME INVINCIBLE FOR\nA COUPLE OF SECONDS BY PRESSING '{1}'",
                                    character.KeyTHUNDER.ToString(), character.KeyIMMORTALITY.ToString()),
                                -60, -44);
                            break;
                        }
                    }

                    break;
                }
                case 20:
                {
                    switch (obstacle.HintNumber)
                    {
                        case 1:
                        {
                            ShowHint(obstacle, "TELEPORTATION DEVICES\nWELL... GUESS HOW THEY WORK", -50, -22);
                            break;
                        }
                        case 2:
                        {
                            ShowHint(obstacle, "GHOSTS GONNA FOLLOW YOU\nALMOST UNKILLABLE", -50, -22);
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
                            ShowHint(obstacle, "DIFFERENT KEYS\nDIFFERENT LOCKS...", -50, -22);
                            break;
                        }
                        case 2:
                        {
                            ShowHint(obstacle, "THOSE DEVICES GONNA TAKE\nEVERYTHING YOU HAVE...", -50, -22);
                            break;
                        }
                    }
                    break;
                }
                case 30:
                {
                    ShowHint(obstacle, "MIGHTY WIZARDS...\nAND THEIR HOMING PROJECTILES...", -50, -22);
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

        public void SaveLevel()
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
                    case BlockType.Brick:
                    {
                        level.Append("W");
                        break;
                    }
                    case BlockType.Spike:
                    {
                        level.Append("#");
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
                    case BlockType.Purifier:
                    {
                        level.Append('|');
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
                    case BlockType.Grass:
                    {
                        level.Append('-');
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
            File.WriteAllText("levels/lvl0.dat", level.ToString());
            LevelNumber = 0;
        }

        public void ReloadLevelUponDeath()
        {
            if (!_mainCharacter.OutOfLives && _mainCharacter.IsDead) //death
            {
                _mainCharacter.Score = StartScore;
                _mainCharacter.ArrowAmount = StartArrows;
                _mainCharacter.Mana = StartMana;
                _mainCharacter.Coins = StartCoins;
                _mainCharacter.Respawn(this);
                LoadLevel(string.Format("lvl{0}", LevelNumber));
            }
        }
    }
}