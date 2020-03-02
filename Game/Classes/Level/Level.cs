using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace Game
{
    public class Level : Drawable
    {
        public Vector2f EnterancePosition { get; private set; }
        public Vector2f ExitPosition { get; private set; }
        public Vector2f tp1Position { get; private set; }
        public Vector2f tp2Position { get; private set; }

        public readonly List<ScoreAdditionEffect> ScoreAdditionEffects;
        public readonly List<ParticleEffect> Particles;

        public readonly List<Block> LevelObstacles;
        public readonly List<Trap> Traps;

        //redundacja w chuj, powinnobyc polimorficznie
        public readonly List<Monster> Monsters;
        public readonly List<Archer> Archers;
        public readonly List<Ghost> Ghosts;
        public readonly List<Wizard> Wizards;

        private Texture _texBackground;
        private Sprite _background;

        private MainCahracter _mainCharacter;
        public Clock LevelTime { get; set; }
        public int MonsterCount { get; set; }
        public string Content { get; set; }
        public List<BlockType> UnableToPassl;
        public int LevelLenght { get; private set; }
        public int LevelWidth { get; private set; }
        public int LevelHeight { get; private set; }
        public int LevelNumber { get; set; }
        public int StartScore { get; set; }
        public int StartCoins { get; set; }
        public int StartArrows { get; set; }
        public int StartMana { get; set; }
        public Level(MainCahracter character)
        {
            this._mainCharacter = character;

            this.ScoreAdditionEffects = new List<ScoreAdditionEffect>();
            this.Particles = new List<ParticleEffect>();

            //this.Creatures = new List<Creature>();
            //this.Projectiles = new List<Projectile>();
            this.LevelObstacles = new List<Block>();
            this.Traps = new List<Trap>();

            //do wyjebania
            this.Monsters = new List<Monster>();
            this.Archers = new List<Archer>();
            this.Ghosts = new List<Ghost>();
            this.Wizards = new List<Wizard>();

            this.LevelTime = new Clock();

            this._texBackground = new Texture(@"img/tiles.png", new IntRect(new Vector2i(32,0), new Vector2i(32, 32)));
            this._texBackground.Repeated = true;

            this._background = new Sprite(this._texBackground);
            
            this.LevelLenght = 0;
            this.LevelHeight = 0;
            this.LevelNumber = 1;
            this.StartScore = 0;
            this.StartArrows = 0;
            this.StartMana = 0;
            this.StartCoins = 0;
        }
        public void LoadLevel(string level)
        {
            //Console.WriteLine("Level load start...");

            this.ScoreAdditionEffects.Clear();
            this.Particles.Clear();

            this.LevelObstacles.Clear();
            this.Traps.Clear();
            //this.Creatures.Clear();
            //this.Projectiles.Clear();

            this.Monsters.Clear();
            this.Archers.Clear();
            this.Ghosts.Clear();
            this.Wizards.Clear();

            this.UnableToPassl = new List<BlockType>() { BlockType.Brick, BlockType.Wood, BlockType.Stone, BlockType.GoldDoor, BlockType.SilverDoor };

            //Console.WriteLine("Entity lists cleared");

            this.LevelHeight = 0;
            this.LevelLenght = 0;

            this.MonsterCount = 0;

            int hintNumber = 0;

            try
            {
                this.Content = File.ReadAllText(@"levels/" + level + @".txt");
            }
            catch (Exception)
            {
                this._mainCharacter.OutOfLives = true;
                this.LevelNumber--;
                return;
            }

            var X = 0;
            var Y = 0;

            //Console.WriteLine("Level txt loaded, generating level...");

            foreach (char tile in this.Content)
            {
                //air and bricks
                if (tile == '\n') { this.LevelHeight++; this.LevelLenght = X; Y++; X = 0; continue; }

                //obstacles
                switch (tile)
                {
                    //standard blocks
                    case 'W': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Brick)); break; }
                    case '#': { this.MonsterCount++; this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Spike)); break; }
                    case 'X': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Stone)); break; }
                    case 'T': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Trampoline)); break; }
                    case 'R': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Wood)); break; }
                    case '=': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Illusion)); break; }
                    //traps
                    case 'H':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.Crusher));
                            break;
                        }
                    case '_':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.Spikes));
                            break;
                        }
                    case ']':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.BlowTorchRight));
                            break;
                        }
                    case '[':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Traps.Add(new Trap(32 * X, 32 * Y, Entity.TrapsTexture, TrapType.BlowTorchLeft));
                            break;
                        }

                    //teleports
                    case '1': 
                        { 
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Teleport1));
                            this.tp1Position = new Vector2f(32 * X, 32 * Y);
                            break;
                        }
                    case '2': 
                        { 
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Teleport2));
                            this.tp2Position = new Vector2f(32 * X, 32 * Y);
                            break; 
                        }
                    //drzwi i klucze
                    case 'S': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.SilverDoor)); break; }
                    case 's': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.SilverKey)); break; }
                    case 'G': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.GoldDoor)); break; }
                    case 'g': { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.GoldenKey)); break; }
                    // Monsters
                    case '@':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Monsters.Add(new Monster(32 * X, 32 * Y, Entity.KnightTexture));
                            break;
                        }
                    case '>':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Archers.Add(new Archer(32 * X, 32 * Y, Entity.ArcherTexture, Movement.Right));
                            break;
                        }
                    case '<':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Archers.Add(new Archer(32 * X, 32 * Y, Entity.ArcherTexture, Movement.Left));
                            break;
                        }
                    case 'f':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Ghosts.Add(new Ghost(32 * X, 32 * Y, Entity.GhostTexture));
                            break;
                        }
                    case '%':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None));
                            this.Wizards.Add(new Wizard(32 * X, 32 * Y, Entity.WizardTexture));
                            break;
                        }
                    // coins
                    case 'o':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Coin));
                            break;
                        }
                    case '$':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.SackOfGold));
                            break;
                        }
                    // life
                    case 'L':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Life));
                            break;
                        }
                    // score
                    case '0':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Score1000));
                            break;
                        }
                    case '5':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Score5000));
                            break;
                        }
                    //arrows
                    case 'a':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Arrow));
                            break;
                        }
                    case 'A':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.TripleArrow));
                            break;
                        }
                    //mana
                    case 'm':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Mana));
                            break;
                        }
                    case 'M':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.PickupsTexture, BlockType.Mana));
                            break;
                        }
                    //details
                    case '!':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Warning));
                            break;
                        }
                    case '?':
                        {
                            hintNumber++;
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Hint, hintNumber));
                            break;
                        }
                    case '*':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.Torch));
                            break;
                        }
                    case ',':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.EvilEyes));
                            break;
                        }
                    case '\\':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.RSpiderweb));
                            break;
                        }
                    case '/':
                        {
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.DetailsTexture, BlockType.LSpiderweb));
                            break;
                        }
                    //enterance
                    case 'v': 
                        { 
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Enterance));
                            this.EnterancePosition = new Vector2f(32 * X, 32 * Y);
                            break; 
                        }
                    //exit
                    case '^': 
                        { 
                            this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.Exit));
                            this.ExitPosition = new Vector2f(32 * X, 32 * Y);
                            break; 
                        }
                    default:
                        {
                            { this.LevelObstacles.Add(new Block(32 * X, 32 * Y, Entity.TilesTexture, BlockType.None)); ; break; }
                        }
                }
                X++;
            }
            
            //Console.WriteLine("Done. Calculating additional values...");
            
            this.LevelHeight += 1;
            this.LevelWidth = this.LevelLenght - 1;
            this._background.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(this.LevelWidth * 32, this.LevelHeight * 32));
            this.MonsterCount = this.Traps.Count*2 + this.Monsters.Count*3 + this.Archers.Count*4 + this.Ghosts.Count*5 + this.Wizards.Count*6;
            //Console.WriteLine("Level {0} loaded succesfully ({1}:{2}  MC value: {3})", level, this.LevelWidth, this.LevelHeight, this.MonsterCount);
            //File.AppendAllText("log.txt", string.Format("Level {0} loaded succesfully ({1}:{2}  MC value: {3})\n", level, this.LevelWidth, this.LevelHeight, this.MonsterCount));
            this.LevelTime.Restart();
            
            //Console.WriteLine("Done");
        }
        public Block GetObstacle(float x, float y)
        {
            return this.LevelObstacles[(int)y * this.LevelLenght + (int)x];
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(this._background);
            foreach (var i in this.Traps)
            {
                target.Draw(i, states);
            }
            foreach (var i in this.LevelObstacles)
            {
                target.Draw(i, states);
                if (i.Type == BlockType.Hint) target.Draw(i.Hint, states);
            }
            //monsters
            foreach(var i in this.Monsters)
            {
                target.Draw(i, states);
            }
            foreach(var i in this.Archers)
            {
                target.Draw(i, states);
            }
            foreach (var i in this.Ghosts)
            {
                target.Draw(i, states);
            }
            foreach (var i in this.Wizards)
            {
                target.Draw(i, states);
            }
            //
            foreach (var i in this.Particles)
            {
                target.Draw(i, states);
            }
            foreach (var i in this.ScoreAdditionEffects)
            {
                target.Draw(i, states);
            }
        }
        public bool UnpassableContains(BlockType type)
        {
            if (this.UnableToPassl.Contains(type)) return true;
            return false;
        }
        public void LevelUpdate(bool IsLevelEditor = false)
        {

            foreach (Block obstacle in this.LevelObstacles)
            {
                if (obstacle.Type == BlockType.Coin || obstacle.Type == BlockType.SilverKey ||
                    obstacle.Type == BlockType.GoldenKey || obstacle.Type == BlockType.Life ||
                    obstacle.Type == BlockType.Score5000 || obstacle.Type == BlockType.Arrow ||
                    obstacle.Type == BlockType.TripleArrow || obstacle.Type == BlockType.Score1000 ||
                    obstacle.Type == BlockType.Mana || obstacle.Type == BlockType.Torch || 
                    obstacle.Type == BlockType.TripleMana || obstacle.Type == BlockType.SackOfGold)
                    obstacle.BlockAnimation.Animate();

                if (obstacle.Type == BlockType.Stone)
                {
                    obstacle.StoneUpdate();
                    if (obstacle.IsDestroyed) this.Particles.Add(new ParticleEffect(obstacle.OriginalPos.X, obstacle.OriginalPos.Y, new Color(150,150,150)));
                }

                if (obstacle.Type == BlockType.Trampoline)
                    if (obstacle.DefaultTimer.ElapsedTime.AsSeconds() > 0.1f)
                        obstacle.SetTextureRectanlge(64, 32, 32, 32);
            }

            if (IsLevelEditor == false)
            {
                foreach (Trap trap in this.Traps)
                {
                    trap.TrapUpdate();
                }

                foreach (ParticleEffect effect in this.Particles)
                {
                    effect.MakeParticles();
                }

                foreach (Monster Monster in this.Monsters)
                {
                    Monster.MonsterUpdate();
                }
                foreach (Archer archer in this.Archers)
                {
                    archer.UpdateArcher(this);
                }
                foreach (Ghost ghost in this.Ghosts)
                {
                    ghost.UpdateGhost(this, this._mainCharacter);
                }
                foreach (Wizard wizard in this.Wizards)
                {
                    wizard.WizardUpdate(this._mainCharacter);
                } 
            }

            //details
            try
            {
                for (int i = 0; i < this.Particles.Count; i++)
                {
                    if (this.Particles[i].Timer.ElapsedTime.AsSeconds() > 5)
                    {
                        this.Particles.RemoveAt(i);
                        if (i > 0) i--;
                    }
                }

                for (int i = 0; i < this.ScoreAdditionEffects.Count; i++)
                {
                    ScoreAdditionEffects[i].UpdateScoreAdditionEffect();
                    if (ScoreAdditionEffects[i].toDestroy)
                    {
                        this.ScoreAdditionEffects.RemoveAt(i);
                        if (i > 0) i--;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("ERROR: Textural detail list index out of range");
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: Textural detail list fatal error");
            }
        }
        public int GetBonusForTime(double time) //do poprawy
        {
            int points = (int)((this.LevelWidth * this.LevelHeight + this.MonsterCount * this.LevelNumber) / time) * this.LevelNumber * 10;
            points -= points % 10;
            return points < 0 ? 0 : points - points % 10;
        }
        public void SetHints(Block obstacle, MainCahracter character)
        {
            switch (this.LevelNumber)
            {
                case 1: // LEVEL 1
                    {
                        switch (obstacle.HintNumber)
                        {
                            case 1:
                                {
                                    this.ShowHint(obstacle, "STONE CRUBLES AFTER\nCOUPLE OF SECONDS...", -40, -22);
                                    break;
                                }
                            case 2:
                                {
                                    this.ShowHint(obstacle, "HERE'S YOUR GOAL", -40, -10);
                                    break;
                                }
                            case 3:
                                {
                                    this.ShowHint(obstacle, "USE TRAMPOLINE\nTO GET HIGHER", -40, -22);
                                    break;
                                }
                            case 4:
                                {
                                    this.ShowHint(obstacle, "THERE ARE A LOT\nOF OTHER PICKUPS...", -40, -22);
                                    break;
                                }
                            case 5:
                                {
                                    this.ShowHint(obstacle, "NOW PICKUP THOSE COINS", -50, -10);
                                    break;
                                }
                            case 6:
                                {
                                    this.ShowHint(obstacle, "WELCOME TO CHENDI ADVENTURES!\nUSE ARROW KEYS TO MOVE", -60, -22);
                                    break;
                                }
                            case 7:
                                {
                                    this.ShowHint(obstacle, String.Format("PRESS '{0}' TO JUMP", character.KeyJUMP.ToString()), -50, -10);
                                    break;
                                }
                            case 8:
                                {
                                    this.ShowHint(obstacle, String.Format("PRESS '{0}' TO ATTACK", character.KeyATTACK.ToString()), -50, -10);
                                    break;
                                }
                            case 9:
                                {
                                    this.ShowHint(obstacle, String.Format("PRESS '{0}' TO SHOOT AN ARROW", character.KeyARROW.ToString()), -70, -10);
                                    break;
                                }
                            case 10:
                                {
                                    this.ShowHint(obstacle, "BE CAREFUL, CUZ\nSPIKES HURTS\nOTRER TRAPS TOO", -50, -34);
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
                                    this.ShowHint(obstacle, String.Format("IF YOU FALL INTO A TRAP,\nJUST KYS BY PRESSING '{0}'", character.KeyDIE.ToString()), -60, -22);
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
                                    this.ShowHint(obstacle, "THERE MUST BE THE WAY\nTO GET INSIDE...", -60, -22);
                                    break;
                                }
                        }
                        break;
                    }
                case 5:
                    {
                        this.ShowHint(obstacle, "ROGUE KNIGHTS...\nSOMETIMES THEY'RE GONNA\nBLOCK YOUR WAY", -60, -34);
                        break;
                    }
                case 10:
                    {
                        this.ShowHint(obstacle, "ARCHERS...\nWATCH OUT", -40, -22);
                        break;
                    }
                case 15:
                    {
                        switch (obstacle.HintNumber)
                        {
                            case 1:
                                {
                                    this.ShowHint(obstacle, "KEYS OPEN CERTAIN DOOR\nTHAT IS OBVIOUS", -50, -22);
                                    break;
                                }
                            case 2:
                                {
                                    this.ShowHint(obstacle, String.Format("MANA POTION CAN BE USED IN TWO WAYS:\nTO ENCHANT YOUR ARROWS BY PRESSING '{0}'\n" +
                                        "OR BECOME INVINCIBLE FOR\nA COUPLE OF SECONDS BY PRESSING '{1}'", character.KeyTHUNDER.ToString(), character.KeyIMMORTALITY.ToString()),
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
                                    this.ShowHint(obstacle, "TELEPORTATION DEVICES\nWELL... GUESS HOW THEY WORK", -50, -22);
                                    break;
                                }
                            case 2:
                                {
                                    this.ShowHint(obstacle, "GHOSTS GONNA FOLLOW YOU\nALMOST UNKILLABLE", -50, -22);
                                    break;
                                }
                        }
                        break;
                    }
                case 25:
                    {
                        this.ShowHint(obstacle, "DIFFERENT KEYS\nDIFFERENT LOCKS...", -50, -22);
                        break;
                    }
                case 30:
                    {
                        this.ShowHint(obstacle, "MIGHTY WIZARDS...\nAND THEIR HOMING PROJECTILES...", -50, -22);
                        break;
                    }
                default: { break; }
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
            this.Particles.Add(effect);
        }
        public void SaveLevel()
        {
            BlockType type;
            StringBuilder level = new StringBuilder();
            int tile = 0; //x
            int y = 0;
            bool monsterFlag = false;


            foreach (Block obstacle in this.LevelObstacles)
            {
                type = obstacle.Type;
                monsterFlag = false;

                //monsters
                foreach (var i in this.Monsters)
                {
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y) { level.Append("@"); monsterFlag = true; }
                }
                foreach (var i in this.Archers)
                {
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    { 
                        if (i.Direction == Movement.Left) { level.Append("<"); monsterFlag = true; }
                        else { level.Append(">"); monsterFlag = true; }
                    }
                }
                foreach (var i in this.Ghosts)
                {
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y) { level.Append("f"); monsterFlag = true; }
                }
                foreach (var i in this.Wizards)
                {
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y) { level.Append("%"); monsterFlag = true; }
                }
                //traps
                foreach (var i in this.Traps)
                {
                    if (i.Get32Position().X == tile && i.Get32Position().Y == y)
                    {
                        switch (i.Type)
                        {
                            case TrapType.Crusher: { level.Append("H"); monsterFlag = true; break; }
                            case TrapType.Spikes: { level.Append("_"); monsterFlag = true; break; }
                            case TrapType.BlowTorchLeft: { level.Append("["); monsterFlag = true; break; }
                            case TrapType.BlowTorchRight: { level.Append("]"); monsterFlag = true; break; }
                        }
                    }
                }
                
                if (monsterFlag)
                {
                    tile++;
                    if (tile == this.LevelWidth + 1)
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
                    default:
                        {
                            break;
                        }
                }
                tile++;
                if (tile == this.LevelWidth + 1)
                {
                    level.Remove(level.Length - 1, 1);
                    level.Append("\r\n");
                    tile = 0;
                    y++;
                }
            }
            File.WriteAllText("levels/edit.txt", level.ToString());
            File.WriteAllText("levels/lvl0.txt", level.ToString());
            this.LevelNumber = 0;
        }
    }
}
