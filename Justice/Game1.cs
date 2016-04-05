using Justice.Controls;
using Justice.Gameplay;
using Justice.Geometry;
using Justice.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Justice
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        IScene myScene;
        ICamera myCamera;
        ParticleEngine myRain;
        Entity myPlayer;
        
        SoundEffect rainSound;
        SoundEffectInstance myRainSoundInstance;
         
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //RasterizerState state = new RasterizerState();
            //state.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = state;

            KeyboardManager.ListenTo(Window);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            rainSound = Content.Load<SoundEffect>("rain_bg_mike_koenig");
            myRainSoundInstance = rainSound.CreateInstance();
            myRainSoundInstance.Volume = 0.5f;
            myRainSoundInstance.IsLooped = true;
            myRainSoundInstance.Play();
            
            BuildScene();
        }

        private void BuildScene()
        {
            SimpleScene scene = new SimpleScene();
            
            GeometryBuilder<VertexPositionNormalTexture> geometryBuilder = new GeometryBuilder<VertexPositionNormalTexture>(PrimitiveType.TriangleList);

            BasicEffect effect = new BasicEffect(GraphicsDevice);
            effect.LightingEnabled = true;
            effect.PreferPerPixelLighting = true;
            effect.DirectionalLight0.Direction = new Vector3(-0.5f, -0.7f, -0.4f);
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.45f, 0.45f, 0.6f) * 0.15f;
            effect.DirectionalLight0.SpecularColor = new Vector3(0.7f, 0.95f, 0.9f) * 0.15f;
            effect.SpecularPower = 0.12f;
            effect.AmbientLightColor = new Vector3(0.15f);

            BasicEffect effect2 = effect.Clone() as BasicEffect;
            effect2.TextureEnabled = true;
            effect2.Texture = Content.Load<Texture2D>("bloodSplat1");

            BasicEffect effect3 = effect.Clone() as BasicEffect;
            effect3.TextureEnabled = true;
            effect3.Texture = Content.Load<Texture2D>("chainFence");

            geometryBuilder.DefaultEffect = effect;

            geometryBuilder.AddCube(new Vector3(0, 0, 0), new Vector3(100, 100, 60));
            geometryBuilder.AddCube(new Vector3(-200, 0, 0), new Vector3(-10, 100, 60));
            geometryBuilder.AddCube(new Vector3(-10, 0, 0), new Vector3(0, 100, 0.25f));
            geometryBuilder.AddCube(new Vector3(-200, -10, 0), new Vector3(100, 0, 0.25f));
            geometryBuilder.AddCube(new Vector3(-200, -60, 0), new Vector3(100, -10, 0.05f));
            geometryBuilder.AddCube(new Vector3(-200, -70, 0), new Vector3(100, -60, 0.25f));
            
            GeometryMesh mesh = geometryBuilder.Bake(GraphicsDevice);
            mesh.Bounds = geometryBuilder.GetBounds(); ;

            geometryBuilder.Clear();
            geometryBuilder.AddSkyBox();
            SkyBox skyBox = new SkyBox(geometryBuilder.Bake(GraphicsDevice), Content.Load<Texture2D>("skyBox"));

            geometryBuilder.Clear();

            geometryBuilder.AddCube(new Vector3(-9.5f, -9.5f, 0), new Vector3(-9, -9, 0.255f));
            geometryBuilder.AddCube(new Vector3(-0.01f, 4f, 1.8f), new Vector3(0.2f, 4.5f, 2.5f));
            geometryBuilder.AddCube(new Vector3(-4.5f, 20f, 0), new Vector3(-4, 21f, 0.255f));
            geometryBuilder.AddCube(new Vector3(-9.99f, 45f, 1.8f), new Vector3(-10.2f, 46f, 2.5f));
            geometryBuilder.DefaultEffect = effect2;
            GeometryMesh bloodMesh = geometryBuilder.Bake(GraphicsDevice);
            bloodMesh.Bounds = geometryBuilder.GetBounds();
            Decal blood = new Decal(bloodMesh);

            geometryBuilder.Clear();
            geometryBuilder.DefaultEffect = effect3;
            geometryBuilder.AddCube(new Vector3(-5, 20, 0), new Vector3(0, 20, 3.2f));
            geometryBuilder.AddCube(new Vector3(-5.25f, 19.85f, 0), new Vector3(-5.0f, 20.05f, 3.2f));
            GeometryMesh fenceMesh = geometryBuilder.Bake(GraphicsDevice);
            fenceMesh.Bounds = geometryBuilder.GetBounds();
            Decal fences = new Decal(fenceMesh);

            scene.AddRenderable(skyBox);
            scene.AddRenderable(mesh);
            scene.AddRenderable(blood);
            scene.AddRenderable(fences);

            SimpleCamera camera = new SimpleCamera();
            camera.Position = new Vector3(-10);
            camera.Normal = new Vector3(1, 1, 0.25f);

            myPlayer = new Entity(null);
            myPlayer.Position = new Vector3(-5, -5, 1);

            SimpleEntityController controller = new SimpleEntityController(myPlayer, camera);

            scene.AddUpdateable(controller);
            scene.AddUpdateable(myPlayer);

            myRain = new ParticleEngine(GraphicsDevice, 2000);

            scene.AddRenderable(myRain);

            myScene = scene;
            myCamera = camera;

            myScene.IterateRenderables(X => X.Init(GraphicsDevice));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardManager.Poll();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            myScene.Update(gameTime);

            myRain.Update(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            
            myScene.Render(GraphicsDevice, myCamera);

            base.Draw(gameTime);
        }
    }
}
