using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Project1
{
    public class Field
    {
        Rectangle destRect;
        bool shown = false;
        Rectangle sourceRect;
        bool hov = false;
        int rotation = 0;
        bool pClicked = false;
        public Field()
        {
            this.destRect = new Rectangle(0, 0, 0, 0);
            this.shown = false;

        }

        public Field(Rectangle destRect, bool shown, bool available)
        {
            this.destRect = destRect;
            this.shown = shown;

        }
        public Field(Rectangle sourceRect, Rectangle destRect, bool shown)
        {
            this.sourceRect = sourceRect;
            this.destRect = destRect;
            this.shown = true;

        }
        public Field(Rectangle destRect)
        {
            this.destRect = destRect;
        }
        public void hover()
        {
            this.hov = true;

        }
        public void click()
        {
            this.pClicked = true;

        }
        public void unclick()
        {
            this.pClicked = false;

        }
        public void unhover()
        {
            this.hov = false;

        }
        public Rectangle getBoardRect()
        {
            return destRect;
        }
        public void Show()
        { shown = true; }
        public void Hide()
        {
            shown = false;
        }
        public Rectangle getSourceRect()
        {
            return sourceRect;
        }

        public void setSourceRect(Rectangle source)
        {
            sourceRect = source;
        }


        public bool isShown()
        {
            return shown;
        }
        internal void Draw(SpriteBatch spriteBatch, Texture2D board, Texture2D questionmark)
        {
            if (shown)
                if (hov)
                    spriteBatch.Draw(board, destRect, sourceRect, new Color(255, 255, 255, 0.1f));
                else
                {
                    if (rotation != 0)
                    {

                        Vector2 origin = new Vector2(destRect.Width / 2, destRect.Height / 2); // todo: update
                        spriteBatch.Draw(board, destRect, sourceRect, Color.White, rotation * MathHelper.Pi / 180, origin, SpriteEffects.None, 0);

                    }
                    else
                        spriteBatch.Draw(board, destRect, sourceRect, Color.White);

                }
            else
                spriteBatch.Draw(questionmark, destRect, Color.White);

        }

        public bool prevClicked()
        {
            return pClicked;
        }

        public void rotate()
        {
            rotation += 90;
            if (rotation >= 360)
                rotation = 0;
        }
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D background;
        Texture2D board;
        Texture2D questionmark;
        int cardSize = 0;
        List<Field> menuFields = new List<Field>();
        List<Field> boardFields = new List<Field>();
        Field selectedRectangle = new Field();


        int? currentIndex = null;

        bool mouseClickLocked = false;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>(@"background");
            board = Content.Load<Texture2D>(@"numbers");
            questionmark = Content.Load<Texture2D>(@"question");

            fillFields();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            MouseState mouse = Mouse.GetState();
            hover(mouse.X, mouse.Y);
            if (mouse.LeftButton == ButtonState.Pressed)
            {

                click(mouse.X, mouse.Y);
                mouseClickLocked = true;
            }
            else
            {
                mouseClickLocked = false;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            selectedRectangle.Draw(_spriteBatch, board, questionmark);
            drawCardCollection(boardFields);
            drawCardCollection(menuFields);

            _spriteBatch.End();
            base.Draw(gameTime);
        }


        void click(int x, int y)
        {

            if (selectedRectangle.getBoardRect().Contains(x, y))
            {
                selectedRectangle.Hide();
                return;
            }

            foreach (var field in menuFields)
            {
                if (field.getBoardRect().Contains(x, y))
                {
                    selectedRectangle.setSourceRect(field.getSourceRect());
                    selectedRectangle.Show();
                    return;
                }
            }

            foreach (var field in boardFields)
            {
                if (field.getBoardRect().Contains(x, y))
                {
                    if (selectedRectangle.isShown())
                    {
                        field.setSourceRect(selectedRectangle.getSourceRect());
                        field.Show();
                        if (field.prevClicked() == true && mouseClickLocked == false)
                            field.rotate();
                        field.click();
                    }
                    else
                    {
                        field.Hide();
                    }
                    //return;
                }
                else
                {
                    field.unclick();
                }

            }

        }

        void hover(int x, int y)
        {

            foreach (var field in menuFields)
            {
                if (field.getBoardRect().Contains(x, y))
                {
                    field.hover();

                }
                else
                {
                    field.unhover();
                }
            }
        }



        private void drawCardCollection(List<Field> fields)
        {
            foreach (var field in fields)
            {
                field.Draw(_spriteBatch, board, questionmark);
            }
        }

        private void fillFields()
        {
            int rows = 3;
            int cols = 4;
            int availableRows = 2;
            int gap = 10;
            int height = board.Height / rows;
            int width = board.Width / cols;
            int large = 20;

            //dane lewego menu
            int cardSize = 60;
            selectedRectangle = new Field(new Rectangle(gap, gap, cardSize + large, cardSize + large));

            for (int r = 0; r < availableRows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    //Rectangle currentRect = new Rectangle((c + 1) * gap + (c * bor), (r + 1) * gap + (r * height), width, height);

                    Rectangle sourceRect = new Rectangle(c * width, r * height, width, height);

                    Rectangle destRect = new Rectangle(gap, cardSize + large + 2 * gap + (r * cols + c) * cardSize, cardSize, cardSize);
                    Field field = new Field(sourceRect, destRect, true);
                    menuFields.Add(field);

                }
            }

            cardSize = 60;
            cols = 10;
            rows = 10;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Rectangle destRect = new Rectangle(10 + cardSize + 50 + c * cardSize, cardSize + 20 + (r) * cardSize, cardSize, cardSize);
                    Field field = new Field(destRect);
                    boardFields.Add(field);
                }
            }
        }
    }
}
