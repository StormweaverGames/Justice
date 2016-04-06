using Justice.Controls;
using Justice.Gameplay;
using Justice.Geometry;
using Justice.Physics;
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

        GeometryMesh testMesh;

        BasicEffect effect;

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
            
            BuildScene();
        }

        private void BuildScene()
        {
            SimpleScene scene = new SimpleScene();
            
            GeometryBuilder<VertexPositionNormalTexture> geometryBuilder = new GeometryBuilder<VertexPositionNormalTexture>(PrimitiveType.TriangleList);

            effect = new BasicEffect(GraphicsDevice);
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
            
            // Building A
            geometryBuilder.AddCube(new Vector3(25, 25, 0), new Vector3(60, 50, 50));
            geometryBuilder.AddCube(new Vector3(25, 50, 0), new Vector3(50, 60, 50));

            // Building B
            geometryBuilder.AddCube(new Vector3(70, 25, 0), new Vector3(115, 50, 60));
            geometryBuilder.AddCube(new Vector3(90, 50, 0), new Vector3(115, 70, 60));

            // Building C
            geometryBuilder.AddCube(new Vector3(60, 60, 0), new Vector3(80, 105, 80));
            geometryBuilder.AddCube(new Vector3(25, 70, 0), new Vector3(60, 105, 80));

            // Building D
            geometryBuilder.AddCube(new Vector3(90, 80, 0), new Vector3(115, 105, 60));

            // Sidewalk
            geometryBuilder.AddCube(new Vector3(20, 20, 0), new Vector3(120, 110, 0.25f));

            // Road A
            geometryBuilder.AddCube(new Vector3(-150, -20, 0), new Vector3(250, 20, 0.1f));
            // Road B
            geometryBuilder.AddCube(new Vector3(-20, 20, 0), new Vector3(20, 110, 0.1f));
            // Road C
            geometryBuilder.AddCube(new Vector3(-20, -20, 0), new Vector3(20, -140, 0.1f));
            // Road D
            geometryBuilder.AddCube(new Vector3(120, 20, 0), new Vector3(160, 110, 0.1f));

            // Sidewalk 2
            geometryBuilder.AddCube(new Vector3(-150, 20, 0), new Vector3(-20, 110, 0.25f));

            // Building E
            geometryBuilder.AddCube(new Vector3(-145, 25, 0), new Vector3(-100, 105, 60));
            // Building F
            geometryBuilder.AddCube(new Vector3(-90, 25, 0), new Vector3(-25, 60, 60));
            // Building G
            geometryBuilder.AddCube(new Vector3(-90, 70, 0), new Vector3(-25, 105, 60));

            //Sidewalk 3
            geometryBuilder.AddCube(new Vector3(-150, -140, 0), new Vector3(-20, -20, 0.25f));

            // Building H
            geometryBuilder.AddCube(new Vector3(-145, -90, 0), new Vector3(-120, -25, 60));
            geometryBuilder.AddCube(new Vector3(-120, -60, 0), new Vector3(-110, -90, 60));
            // Building I
            geometryBuilder.AddCube(new Vector3(-110, -50, 0), new Vector3(-70, -25, 60));
            geometryBuilder.AddCube(new Vector3(-100, -70, 0), new Vector3(-70, -50, 60));
            // Building J
            geometryBuilder.AddCube(new Vector3(-60, -70, 0), new Vector3(-25, -25, 50));
            // Building K
            geometryBuilder.AddCube(new Vector3(-145, -100, 0), new Vector3(-110, -135, 50));
            // Building L
            geometryBuilder.AddCube(new Vector3(-100, -130, 0), new Vector3(-25, -80, 100));
            geometryBuilder.AddCube(new Vector3(-100, -135, 0), new Vector3(-70, -130, 100));
            geometryBuilder.AddCube(new Vector3(-50, -135, 0), new Vector3(-25, -130, 100));

            //Sidewalk 4
            geometryBuilder.AddCube(new Vector3(160, 20, 0), new Vector3(250, 110, 0.25f));

            // Building U
            geometryBuilder.AddCube(new Vector3(165, 70, 0), new Vector3(210, 105, 60));
            // Building V
            geometryBuilder.AddCube(new Vector3(165, 25, 0), new Vector3(200, 60, 60));
            // Building W
            geometryBuilder.AddCube(new Vector3(220, 25, 0), new Vector3(245, 105, 60));
            geometryBuilder.AddCube(new Vector3(210, 25, 0), new Vector3(220, 60, 60));

            //Sidewalk 5
            geometryBuilder.AddCube(new Vector3(20, -20, 0), new Vector3(250, -140, 0.25f));

            // Building M
            geometryBuilder.AddCube(new Vector3(25, -50, 0), new Vector3(80, -25, 60));
            geometryBuilder.AddCube(new Vector3(70, -50, 0), new Vector3(80, -70, 60));
            // Building N
            geometryBuilder.AddCube(new Vector3(25, -60, 0), new Vector3(50, -100, 60));
            geometryBuilder.AddCube(new Vector3(50, -60, 0), new Vector3(60, -70, 60));
            // Building O
            geometryBuilder.AddCube(new Vector3(25, -110, 0), new Vector3(80, -135, 60));
            geometryBuilder.AddCube(new Vector3(60, -80, 0), new Vector3(80, -110, 60));
            // Building P
            geometryBuilder.AddCube(new Vector3(90, -25, 0), new Vector3(120, -80, 60));
            // Building Q
            geometryBuilder.AddCube(new Vector3(130, -25, 0), new Vector3(200, -60, 60));
            // Building R
            geometryBuilder.AddCube(new Vector3(90, -90, 0), new Vector3(160, -135, 60));
            geometryBuilder.AddCube(new Vector3(130, -70, 0), new Vector3(160, -90, 60));
            // Building S
            geometryBuilder.AddCube(new Vector3(170, -70, 0), new Vector3(245, -100, 60));
            geometryBuilder.AddCube(new Vector3(210, -25, 0), new Vector3(245, -70, 60));
            // Building T
            geometryBuilder.AddCube(new Vector3(170, -110, 0), new Vector3(245, -135, 60));

            GeometryMesh mesh = geometryBuilder.Bake(GraphicsDevice);
            mesh.Bounds = geometryBuilder.GetBounds(); ;

            CityGenerator gen = new CityGenerator();
            geometryBuilder.Clear();
            gen.GenerateCity(geometryBuilder, 10, 10);
            testMesh = geometryBuilder.Bake(GraphicsDevice);
            //testMesh.Effect = effect;
            testMesh.Bounds = geometryBuilder.GetBounds();

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
            scene.AddRenderable(testMesh);
            //scene.AddRenderable(mesh);
            scene.AddRenderable(blood);
            scene.AddRenderable(fences);

            SimpleCamera camera = new SimpleCamera();
            camera.Position = new Vector3(-10);
            camera.Normal = new Vector3(1, 1, 0.25f);

            myPlayer = new Entity(null);
            myPlayer.Position = new Vector3(0, 0, 1);

            SimpleEntityController controller = new SimpleEntityController(myPlayer, camera);
            //SimpleCameraController controller = new SimpleCameraController(camera);

            scene.AddUpdateable(controller);
            scene.AddUpdateable(myPlayer);

            myRain = new ParticleEngine(GraphicsDevice, 2000);

            scene.AddRenderable(myRain);

            myScene = scene;
            myCamera = camera;

            myScene.IterateRenderables(X => X.Init(GraphicsDevice));

            BoundingBox box = new BoundingBox(new Vector3(0, 0, 0), new Vector3(10, 10, 10));
            OrientedBoundingBox box2 = new OrientedBoundingBox(new Vector3(15, 15, 15), new Vector3(25, 25, 25));
            box2.Transforms = Matrix.CreateTranslation(-7, -7, -7);

            System.Diagnostics.Debug.Write(box2.Intersects(box) ? "Intersests" : "Does not interesect");


            myRainSoundInstance.Play();
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

            if (KeyboardManager.IsKeyPressed(Keys.Tab))
            {
                myScene.RemoveRenderable(testMesh);

                GeometryBuilder<VertexPositionNormalTexture> geometryBuilder = new GeometryBuilder<VertexPositionNormalTexture>(PrimitiveType.TriangleList);
                geometryBuilder.DefaultEffect = effect;

                CityGenerator gen = new CityGenerator();
                gen.GenerateCity(geometryBuilder, 10, 10);
                testMesh = geometryBuilder.Bake(GraphicsDevice);
                //testMesh.Effect = effect;
                testMesh.Bounds = geometryBuilder.GetBounds();

                testMesh.Init(GraphicsDevice);

                myScene.AddRenderable(testMesh);
            }

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
