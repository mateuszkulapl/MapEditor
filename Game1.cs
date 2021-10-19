using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Project1
{
    public class Field
    {
        Rectangle destRect;
        bool shown = false;
        Rectangle sourceRect;

        public Field()
        {
            this.destRect = new Rectangle(0,0,0,0);
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
        public bool isShown()
        {
                return false;
        }
        internal void Draw(SpriteBatch spriteBatch, Texture2D board, Texture2D questionmark)
        {
            if (shown)
                spriteBatch.Draw(board, destRect, sourceRect,Color.White);
                    else
                        spriteBatch.Draw(questionmark, destRect, Color.White);
            //spriteBatch.Draw(board, destRect, sourceRect, new Color(255, 255, 255, 0.1f));
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
            




            // TODO: Add your update logic here

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
            cardSize = 60;
            selectedRectangle = new Field(new Rectangle(gap, gap, cardSize + large, cardSize + large));
            
            for (int r = 0; r < availableRows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    //Rectangle currentRect = new Rectangle((c + 1) * gap + (c * bor), (r + 1) * gap + (r * height), width, height);

                    Rectangle sourceRect = new Rectangle(c * width, r * height, width, height);

                    Rectangle destRect = new Rectangle(gap, cardSize + large+2*gap + (r * cols + c) * cardSize, cardSize, cardSize);
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
