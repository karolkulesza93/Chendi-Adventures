using SFML.Graphics;
using SFML.System;

namespace Game
{
    public class MainCharacterUI : Drawable
    {
        private MainCahracter _character;
        private View _view;
        private Level _level;
        public TextLine LivesCount { get; private set; }
        public TextLine Score { get; private set; }
        public TextLine CurrentLevel { get; private set; }
        public TextLine Arrows { get; private set; }
        public TextLine Mana { get; private set; }
        public TextLine Coins { get; private set; }
        public Sprite SilverKey { get; private set; }
        public Sprite GoldenKey { get; private set; }
        public Sprite Arrow { get; private set; }
        public Sprite ManaBottle { get; private set; }
        public Sprite Coins3 { get; private set; }
        public MainCharacterUI(MainCahracter character, View view, Level level)
        {
            this._character = character;
            this._view = view;
            this._level = level;

            this.LivesCount = new TextLine("LIVES: " + (this._character.Lives).ToString(), 10, 0, 0, Color.White); //this.LivesCount.SetOutlineThickness(0.5f);
            this.Score = new TextLine("SCORE: " + this._character.Score.ToString(), 10, 0, 0, Color.White); //this.Score.SetOutlineThickness(0.5f);
            this.CurrentLevel = new TextLine("LEVEL: " + this._level.LevelNumber.ToString(), 10, 0, 0, Color.White); //this.CurrentLevel.SetOutlineThickness(0.5f);
            this.Arrows = new TextLine("X " + this._character.ArrowAmount.ToString(), 10, 0, 0, Color.White); //this.Arrows.SetOutlineThickness(0.5f);
            this.Mana = new TextLine("X " + this._character.Mana.ToString(), 10, 0, 0, Color.White); //this.Mana.SetOutlineThickness(0.5f);
            this.Coins = new TextLine("X " + this._character.Coins.ToString(), 10, 0, 0, Color.Yellow);

            this.SilverKey = new Sprite(new Texture(@"img/pickups.png", new IntRect(new Vector2i(0, 32), new Vector2i(32, 32))));
            this.GoldenKey = new Sprite(new Texture(@"img/pickups.png", new IntRect(new Vector2i(0, 64), new Vector2i(32, 32))));
            this.Arrow = new Sprite(new Texture(@"img/arrow.png", new IntRect(new Vector2i(0,0), new Vector2i(32,7))));
            this.ManaBottle = new Sprite(new Texture(@"img/mana.png", new IntRect(new Vector2i(0, 0), new Vector2i(20, 20))));
            this.Coins3 = new Sprite(new Texture(@"img/coins.png"));
        }
        public void UpdateUI()
        {
            if (this._character.Lives < 2) this.LivesCount.ChangeColor(Color.Red);
            else this.LivesCount.ChangeColor(Color.White);
            if (this._character.Lives < 0) this.LivesCount.EditText("LIVES: " + 0);
            else this.LivesCount.EditText("LIVES: " + (this._character.Lives).ToString());

            this.Score.EditText("SCORE: " + this._character.Score.ToString());
            this.CurrentLevel.EditText("LEVEL: " + this._level.LevelNumber.ToString());
            this.Arrows.EditText("X " + this._character.ArrowAmount.ToString());
            this.Coins.EditText(("X " ) + this._character.Coins.ToString());
            if (this._character.ArrowAmount == 0) this.Arrows.ChangeColor(Color.Red);
            else this.Arrows.ChangeColor(Color.White);
            this.Mana.EditText("X " + this._character.Mana.ToString());
            if (this._character.Mana == 0) this.Mana.ChangeColor(Color.Red);
            else this.Mana.ChangeColor(Color.White);

            this.LivesCount.MoveText(
                this._view.Center.X + this._view.Size.X / 2 - 100,
                this._view.Center.Y - this._view.Size.Y / 2 + 7
                );

            this.Score.MoveText(
                this._view.Center.X - this._view.Size.X / 2 + 10,
                this._view.Center.Y - this._view.Size.Y / 2 + 7
                );

            this.CurrentLevel.MoveText(
                this._view.Center.X + this._view.Size.X / 2 - 230,
                this._view.Center.Y - this._view.Size.Y / 2 + 7
                );

            this.Arrows.MoveText(
                this._view.Center.X + this._view.Size.X / 2 - 300,
                this._view.Center.Y - this._view.Size.Y / 2 + 7
                );

            this.Mana.MoveText(
                this._view.Center.X + this._view.Size.X / 2 - 400,
                this._view.Center.Y - this._view.Size.Y / 2 + 7
                );

            this.Coins.MoveText(
                this._view.Center.X + this._view.Size.X / 2 - 500,
                this._view.Center.Y - this._view.Size.Y / 2 + 7
                );

            this.SilverKey.Position = new Vector2f(this._view.Center.X - this._view.Size.X / 2 + 5, this._view.Center.Y - this._view.Size.Y / 2 + 20);
            this.GoldenKey.Position = new Vector2f(this._view.Center.X - this._view.Size.X / 2 + 5, this._view.Center.Y - this._view.Size.Y / 2 + 40);
            this.Arrow.Position = new Vector2f(this._view.Center.X + this._view.Size.X / 2 - 342, this._view.Center.Y - this._view.Size.Y / 2 + 9);
            this.ManaBottle.Position = new Vector2f(this._view.Center.X + this._view.Size.X / 2 - 430, this._view.Center.Y - this._view.Size.Y / 2 + 3);
            this.Coins3.Position = new Vector2f(this._view.Center.X + this._view.Size.X / 2 - 530, this._view.Center.Y - this._view.Size.Y / 2 + 3);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(this.LivesCount, states);
            target.Draw(this.Score, states);
            target.Draw(this.CurrentLevel, states);
            target.Draw(this.Arrows, states);
            target.Draw(this.Mana, states);
            target.Draw(this.Coins, states);

            if (this._character.HasSilverKey) target.Draw(this.SilverKey, states);
            if (this._character.HasGoldenKey) target.Draw(this.GoldenKey, states);
            target.Draw(this.Arrow, states);
            target.Draw(this.ManaBottle, states);
            target.Draw(this.Coins3, states);
        }
        public void ResetPositions()
        {
            this.LivesCount.MoveText(-100, -100);
            this.Score.MoveText(-100, -100);
            this.CurrentLevel.MoveText(-100, -100);
            this.Arrows.MoveText(-100, -100);
            this.Mana.MoveText(-100, -100);
            this.Coins.MoveText(-100, -100);

            this.SilverKey.Position = new Vector2f(-100, -100);
            this.GoldenKey.Position = new Vector2f(-100, -100);
            this.Arrow.Position = new Vector2f(-100, -100);
            this.ManaBottle.Position = new Vector2f(-100, -100);
            this.Coins3.Position = new Vector2f(-100, -100);
        }
    }
}
