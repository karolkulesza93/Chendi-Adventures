using SFML.Graphics;
using SFML.System;

namespace Game
{
    public class MainCharacterUI : Drawable
    {
        private readonly MainCharacter _character;
        private readonly Level _level;
        private readonly View _view;

        public MainCharacterUI(MainCharacter character, View view, Level level)
        {
            _character = character;
            _view = view;
            _level = level;

            LivesCount =
                new TextLine("LIVES: " + _character.Lives, 10, 0, 0,
                    Color.White); //this.LivesCount.SetOutlineThickness(0.5f);
            Score = new TextLine("SCORE: " + _character.Score, 10, 0, 0,
                Color.White); //this.Score.SetOutlineThickness(0.5f);
            CurrentLevel =
                new TextLine("LEVEL: " + _level.LevelNumber, 10, 0, 0,
                    Color.White); //this.CurrentLevel.SetOutlineThickness(0.5f);
            Arrows = new TextLine("X " + _character.ArrowAmount, 10, 0, 0,
                Color.White); //this.Arrows.SetOutlineThickness(0.5f);
            Mana = new TextLine("X " + _character.Mana, 10, 0, 0, Color.White); //this.Mana.SetOutlineThickness(0.5f);
            Coins = new TextLine("X " + _character.Coins, 10, 0, 0, Color.Yellow);

            SilverKey = new Sprite(new Texture(@"img/pickups.png",
                new IntRect(new Vector2i(0, 32), new Vector2i(32, 32))));
            GoldenKey = new Sprite(new Texture(@"img/pickups.png",
                new IntRect(new Vector2i(0, 64), new Vector2i(32, 32))));
            Arrow = new Sprite(new Texture(@"img/arrow.png", new IntRect(new Vector2i(0, 0), new Vector2i(32, 7))));
            ManaBottle =
                new Sprite(new Texture(@"img/mana.png", new IntRect(new Vector2i(0, 0), new Vector2i(20, 20))));
            Coins3 = new Sprite(new Texture(@"img/coins.png"));
        }

        public TextLine LivesCount { get; }
        public TextLine Score { get; }
        public TextLine CurrentLevel { get; }
        public TextLine Arrows { get; }
        public TextLine Mana { get; }
        public TextLine Coins { get; }
        public Sprite SilverKey { get; }
        public Sprite GoldenKey { get; }
        public Sprite Arrow { get; }
        public Sprite ManaBottle { get; }
        public Sprite Coins3 { get; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(LivesCount, states);
            target.Draw(Score, states);
            target.Draw(CurrentLevel, states);
            target.Draw(Arrows, states);
            target.Draw(Mana, states);
            target.Draw(Coins, states);

            if (_character.HasSilverKey) target.Draw(SilverKey, states);
            if (_character.HasGoldenKey) target.Draw(GoldenKey, states);
            target.Draw(Arrow, states);
            target.Draw(ManaBottle, states);
            target.Draw(Coins3, states);
        }

        public void UpdateUI()
        {
            if (_character.Lives < 2) LivesCount.ChangeColor(Color.Red);
            else LivesCount.ChangeColor(Color.White);
            if (_character.Lives < 0) LivesCount.EditText("LIVES: " + 0);
            else LivesCount.EditText("LIVES: " + _character.Lives);

            Score.EditText("SCORE: " + _character.Score);
            CurrentLevel.EditText("LEVEL: " + _level.LevelNumber);
            Arrows.EditText("X " + _character.ArrowAmount);
            Coins.EditText("X " + _character.Coins);
            if (_character.ArrowAmount == 0) Arrows.ChangeColor(Color.Red);
            else Arrows.ChangeColor(Color.White);
            Mana.EditText("X " + _character.Mana);
            if (_character.Mana == 0) Mana.ChangeColor(Color.Red);
            else Mana.ChangeColor(Color.White);

            LivesCount.MoveText(
                _view.Center.X + _view.Size.X / 2 - 100,
                _view.Center.Y - _view.Size.Y / 2 + 7
            );

            Score.MoveText(
                _view.Center.X - _view.Size.X / 2 + 10,
                _view.Center.Y - _view.Size.Y / 2 + 7
            );

            CurrentLevel.MoveText(
                _view.Center.X + _view.Size.X / 2 - 230,
                _view.Center.Y - _view.Size.Y / 2 + 7
            );

            Arrows.MoveText(
                _view.Center.X + _view.Size.X / 2 - 300,
                _view.Center.Y - _view.Size.Y / 2 + 7
            );

            Mana.MoveText(
                _view.Center.X + _view.Size.X / 2 - 400,
                _view.Center.Y - _view.Size.Y / 2 + 7
            );

            Coins.MoveText(
                _view.Center.X + _view.Size.X / 2 - 500,
                _view.Center.Y - _view.Size.Y / 2 + 7
            );

            SilverKey.Position =
                new Vector2f(_view.Center.X - _view.Size.X / 2 + 5, _view.Center.Y - _view.Size.Y / 2 + 20);
            GoldenKey.Position =
                new Vector2f(_view.Center.X - _view.Size.X / 2 + 5, _view.Center.Y - _view.Size.Y / 2 + 40);
            Arrow.Position = new Vector2f(_view.Center.X + _view.Size.X / 2 - 342,
                _view.Center.Y - _view.Size.Y / 2 + 9);
            ManaBottle.Position = new Vector2f(_view.Center.X + _view.Size.X / 2 - 430,
                _view.Center.Y - _view.Size.Y / 2 + 3);
            Coins3.Position = new Vector2f(_view.Center.X + _view.Size.X / 2 - 530,
                _view.Center.Y - _view.Size.Y / 2 + 3);
        }

        public void ResetPositions()
        {
            LivesCount.MoveText(-100, -100);
            Score.MoveText(-100, -100);
            CurrentLevel.MoveText(-100, -100);
            Arrows.MoveText(-100, -100);
            Mana.MoveText(-100, -100);
            Coins.MoveText(-100, -100);

            SilverKey.Position = new Vector2f(-100, -100);
            GoldenKey.Position = new Vector2f(-100, -100);
            Arrow.Position = new Vector2f(-100, -100);
            ManaBottle.Position = new Vector2f(-100, -100);
            Coins3.Position = new Vector2f(-100, -100);
        }
    }
}