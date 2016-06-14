using BEPUphysics.Constraints.SingleEntity;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Paths;
using BEPUphysics.Paths.PathFollowing;
using Justice.Controls;
using Justice.Gameplay;
using Justice.Geometry;
using Justice.SpaceGame;
using Justice.Tools;
using Justice.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;

namespace Justice
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont myDebugFont;

        IScene myScene;
        ICamera myCamera;
        ParticleEngine myRain;
        Player myPlayer;
        
        BasicEffect effect;

        SoundEffect rainSound;
        SoundEffectInstance myRainSoundInstance;
        
        InterfaceManger myInterface;

        Ship ship;
        SASModule mySas;

        bool isRayIntersecting;
         
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

            SharedContent.Init(Content);

            graphics.SynchronizeWithVerticalRetrace = false;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            IsFixedTimeStep = false;

            KeyboardManager.ListenTo(Window);

            Window.ClientSizeChanged += (X, Y) => 
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();

                myInterface.HandleViewportResized(GraphicsDevice.Viewport);
            };

            base.Initialize();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myDebugFont = Content.Load<SpriteFont>("debug_font");

            rainSound = Content.Load<SoundEffect>("rain_bg_mike_koenig");
            myRainSoundInstance = rainSound.CreateInstance();
            myRainSoundInstance.Volume = 0.05f;
            myRainSoundInstance.IsLooped = true;

            Testing();

            BuildInterface();            

            BuildScene();

            // myRainSoundInstance.Play();
        }
        
        private void Testing()
        {
            Alignment[] aligments = Enum.GetValues(typeof(Alignment)).Cast<Alignment>().ToArray();

            for(int index = 0; index < aligments.Length; index ++)
            {
                Alignment element = aligments[index];

                HorizontalAlignment hor;
                VerticalAlignment ver;

                element.GetComponents(out hor, out ver);

                System.Diagnostics.Debug.WriteLine("{0} - {1} {2}", element, hor, ver);
            }

            State initState = new State("Inactive");
            State activeState = new State("Active");
            State pausedState = new State("Paused");
            State exitState = new State("Exit");

            StateChangeCommand resumeCommand = new StateChangeCommand("Resume");
            StateChangeCommand pauseCommand = new StateChangeCommand("Pause");
            StateChangeCommand beginCommand = new StateChangeCommand("Begin");
            StateChangeCommand endCommand = new StateChangeCommand("End");
            StateChangeCommand exitCommand = new StateChangeCommand("Exit");

            FiniteStateMachine stateMachine = new FiniteStateMachine(initState);

            stateMachine.AddTransition(initState, exitState, exitCommand);
            stateMachine.AddTransition(initState, activeState, beginCommand);

            stateMachine.AddTransition(pausedState, initState, endCommand);
            stateMachine.AddTransition(pausedState, activeState, resumeCommand);

            stateMachine.AddTransition(activeState, pausedState, pauseCommand);
            stateMachine.AddTransition(activeState, initState, endCommand);
        }

        private void BuildInterface()
        {
            myInterface = new InterfaceManger(GraphicsDevice);

            FrameRateMonitor fpsMonitor = new FrameRateMonitor(new Rectangle(0, 45, 120, 120), 0, 60, true);
            fpsMonitor.SampleRate = 30;

            VaryingTextElement fpsMax = new VaryingTextElement(() => { return fpsMonitor.Max.ToString("0.00"); }, new Vector2(fpsMonitor.Bounds.Right, fpsMonitor.Bounds.Top));
            VaryingTextElement fpsMin = new VaryingTextElement(() => { return fpsMonitor.Min.ToString("0.00"); }, new Vector2(fpsMonitor.Bounds.Right, fpsMonitor.Bounds.Bottom));
            VaryingTextElement fpsAvg = new VaryingTextElement(() => { return fpsMonitor.Average.ToString("0.00"); }, new Vector2(fpsMonitor.Bounds.Right, fpsMonitor.Bounds.Bottom));

            fpsMax.TextColor = Color.White;
            fpsMin.TextColor = Color.White;
            fpsAvg.TextColor = Color.White;

            ElementMover avgMover = new ElementMover((itm) => { return new Vector2(fpsMonitor.Bounds.Right, fpsMonitor.AveragePosition); }, fpsAvg);

            TestComponent test = new TestComponent();
            test.Bounds = new Rectangle(100, 100, 100, 100);
            
            //myInterface.AddElement(test);

            myInterface.AddElement(fpsMonitor);
            myInterface.AddElement(fpsMin);
            myInterface.AddElement(fpsMax);

            myInterface.AddElement(fpsAvg);
            myInterface.AddElement(avgMover);
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
            effect.TextureEnabled = true;
            effect.Texture = Content.Load<Texture2D>("concrete");
            effect.CurrentTechnique = effect.Techniques["BasicEffect_PixelLighting_Texture"];
            
            int contreteId = MaterialManager.RegisterMaterial(new Geometry.EffectMaterial("Concrete", Content.Load<Texture2D>("concrete")));
            int bloodId = MaterialManager.RegisterMaterial(new Geometry.EffectMaterial("Blood", Content.Load<Texture2D>("bloodSplat1")));
            int fenceId = MaterialManager.RegisterMaterial(new Geometry.EffectMaterial("ChainFence", Content.Load<Texture2D>("chainFence")));
            int skyboxId = MaterialManager.RegisterMaterial(new Geometry.EffectMaterial("Skybox", Content.Load<Texture2D>("skybox")));
            int redVelevetId = MaterialManager.RegisterMaterial(new Geometry.EffectMaterial("RedVelet", Content.Load<Texture2D>("carpet_herlen_gustahome")));
            
            geometryBuilder.Clear();
            geometryBuilder.AddSkyBox();
            SkyBox skyBox = new SkyBox(geometryBuilder.Bake(GraphicsDevice, skyboxId), Content.Load<Texture2D>("skyBox"));

            geometryBuilder.Clear();

            geometryBuilder.AddCube(new Vector3(-9.5f, -9.5f, 0), new Vector3(-9, -9, 0.255f));
            geometryBuilder.AddCube(new Vector3(-0.01f, 4f, 1.8f), new Vector3(0.2f, 4.5f, 2.5f));
            geometryBuilder.AddCube(new Vector3(-4.5f, 20f, 0), new Vector3(-4, 21f, 0.255f));
            geometryBuilder.AddCube(new Vector3(-9.99f, 45f, 1.8f), new Vector3(-10.2f, 46f, 2.5f));
            //geometryBuilder.DefaultEffect = effect2;
            GeometryMesh blood = geometryBuilder.Bake(GraphicsDevice, bloodId);

            geometryBuilder.Clear();
            //geometryBuilder.DefaultEffect = effect3;
            geometryBuilder.AddCube(new Vector3(-5, 20, 0), new Vector3(0, 20, 3.2f));
            geometryBuilder.AddCube(new Vector3(-5.25f, 19.85f, 0), new Vector3(-5.0f, 20.05f, 3.2f));
            GeometryMesh fence = geometryBuilder.Bake(GraphicsDevice, fenceId);

            geometryBuilder.Clear();
            
            geometryBuilder.AddCube(-10, -10, -0.25f, 10, 10, 0.25f);
            GeometryMesh lift = geometryBuilder.Bake(GraphicsDevice, contreteId);

            SimplePhysicsEntity liftEntity = new SimplePhysicsEntity(new BEPUphysics.Entities.Entity(lift.RenderBounds.GeneratePhysicsBox().Shape), 0, lift);
            liftEntity.Position = new Vector3(0, 20, 15);

            CardinalSpline3D elevatorPath = new CardinalSpline3D();
            elevatorPath.PreLoop = CurveEndpointBehavior.Mirror;
            elevatorPath.PostLoop = CurveEndpointBehavior.Mirror;
            elevatorPath.ControlPoints.Add(-1, new BEPUutilities.Vector3(0, 20, 0));
            elevatorPath.ControlPoints.Add(0, new BEPUutilities.Vector3(0, 20, 0));
            elevatorPath.ControlPoints.Add(2, new BEPUutilities.Vector3(0, 20, 0));
            elevatorPath.ControlPoints.Add(8, new BEPUutilities.Vector3(0, 20, 80));
            elevatorPath.ControlPoints.Add(10, new BEPUutilities.Vector3(0, 20, 80));

            EntityPather pather = new EntityPather(liftEntity);
            pather.SetPositionPath(elevatorPath);
            scene.Add(pather);

            SimpleCamera camera = new SimpleCamera();
            camera.FarPlane = 2000.0f;
            camera.Position = new Vector3(-10);
            camera.Normal = new Vector3(1, 1, 0.25f);

            myPlayer = new Player(new Vector3(-5, -5, 10));
            myPlayer.AddToScene(scene.PhysicsSpace);

            SimplePlayerController controller = new SimplePlayerController(myPlayer, camera);
            //SimpleCameraController controller = new SimpleCameraController(camera);
            
            geometryBuilder.Clear();
            geometryBuilder.AddCube(0, 0, 6, 10, 10, 6.5f);
            GeometryMesh awning = geometryBuilder.Bake(GraphicsDevice, redVelevetId);

            Vector3 center = (awning.RenderBounds.Max + awning.RenderBounds.Min) / 2.0f;
            scene.AddCollider(new Box(new BEPUutilities.Vector3(center.X, center.Y, center.Z), awning.RenderBounds.Max.X - awning.RenderBounds.Min.X, awning.RenderBounds.Max.Y - awning.RenderBounds.Min.Y, awning.RenderBounds.Max.Z - awning.RenderBounds.Min.Z));

            scene.AddUpdateable(controller);
            scene.AddUpdateable(myPlayer);

            myRain = new ParticleEngine(GraphicsDevice, scene, 500);

            BasicEffectInterface effectInterface = new BasicEffectInterface(effect, effect.CurrentTechnique.Name);
            effectInterface.InitBasicEffect();

            BasicEffectInterface effectInterface2 = new BasicEffectInterface(new BasicEffect(GraphicsDevice), "BasicEffect_Texture_NoFog");
            effectInterface2.InitBasicEffect();

            EffectGroup skyboxEffect = new EffectGroup(GraphicsDevice, effectInterface2, "skybox");
            skyboxEffect.DepthStencilState = DepthStencilState.None;
            EffectGroup opaqueEffect = new EffectGroup(GraphicsDevice, effectInterface, "opaque");
            opaqueEffect.BlendState = BlendState.Opaque;
            EffectGroup transparentEffect = new EffectGroup(GraphicsDevice, effectInterface, "transparent");
            transparentEffect.BlendState = BlendState.AlphaBlend;
            transparentEffect.Rasterizer = RasterizerState.CullNone;

            scene.AddEffectGroup(skyboxEffect);
            scene.AddEffectGroup(opaqueEffect);
            scene.AddEffectGroup(transparentEffect);

            CityGenerator gen = new CityGenerator();
            geometryBuilder.Clear();

            ship = new Ship();
            geometryBuilder.Clear();

            // Major blocks
            geometryBuilder.AddCube(-5, -5, -5,   5, 5, 5);
            geometryBuilder.AddCube(-1.5f, 15, -5,   1.5f, 25, 5);
            geometryBuilder.AddCubeTransformed(-5, -2, -0.5f,  5, 2, 0.5f, Matrix.CreateTranslation(0, 23f, 4.5f));
            geometryBuilder.AddCubeTransformed(-5, -2, -0.375f,  5, 2, 0.375f, Matrix.CreateTranslation(0, 7f, -4.625f));

            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(10, 10, 10));
            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(3, 10, 10), new Vector3(0.0f, 20, 0));
            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(10, 4f, 1), new Vector3(0.0f, 23f, 4.5f));
            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(10, 4f, 0.75f), new Vector3(0.0f, 7f, -4.625f));

            // Walkway
            geometryBuilder.AddCube(-1.5f, 5, 4,   1.5f, 15, 5);

            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(3, 10, 1), new Vector3(0, 10, 4.5f));

            // End caps
            geometryBuilder.AddCube(-5, -5, 5,   5, -4, 7);
            geometryBuilder.AddCube(-5, 24, 5,   5, 25, 7);

            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(10, 1, 2), new Vector3(0, -4.5f, 6));
            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(10, 1, 2), new Vector3(0, 24.5f, 6));

            // Walkway entrances
            //geometryBuilder.AddCube(-5, 5, 5,    -2, 4, 7);
            //geometryBuilder.AddCube(2, 5, 5,     5, 4, 7);
            //geometryBuilder.AddCube(-5, 15, 5, -2, 16, 7);
            //geometryBuilder.AddCube(2, 15, 5, 5, 16, 7);

            //ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(3f, 1, 2), new Vector3(-3.5f, 4.5f, 6));
            //ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(3, 1, 2), new Vector3(3.5f, 4.5f, 6));
            //ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(3f, 1, 2), new Vector3(-3.5f, 15.5f, 6));
            //ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(3, 1, 2), new Vector3(3.5f, 15.5f, 6));

            // Side bars
            geometryBuilder.AddCube(5, -4, 5,    4, 24, 7);
            geometryBuilder.AddCube(-5, -4, 5,   -4, 24, 7);
            //geometryBuilder.AddCube(5, 16, 5, 4, 24, 7);
            //geometryBuilder.AddCube(-5, 16, 5, -4, 24, 7);

            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(1, 30, 2), new Vector3(4.5f, 20, 6));
            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(1, 30, 2), new Vector3(-4.5f, 20, 6));
            //ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(1, 10, 2), new Vector3(4.5f, 20, 6));
            //ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(1, 10, 2), new Vector3(-4.5f, 20, 6));

            // Ramps
            Quaternion rotate = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(38.0f));
            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(4f, 15.0f, 1f), new Vector3(3f, 15, -0.05f), rotate);
            geometryBuilder.AddCubeTransformed(-2f, -7.5f, -0.5f, 2f, 7.5f, 0.5f, Matrix.CreateFromQuaternion(rotate) * Matrix.CreateTranslation(3f, 15, -0.05f));

            ship.AddShape(new BEPUphysics.CollisionShapes.ConvexShapes.BoxShape(4f, 15.0f, 1f), new Vector3(-3f, 15, -0.05f), rotate);
            geometryBuilder.AddCubeTransformed(-2f, -7.5f, -0.5f, 2f, 7.5f, 0.5f, Matrix.CreateFromQuaternion(rotate) * Matrix.CreateTranslation(-3f, 15, -0.05f));

            ship.SetRenderable(geometryBuilder.Bake(GraphicsDevice, contreteId));
            ship.BakeShape(-1.0f, new Vector3(-15, -15, 5));

            mySas = new SASModule(ship);
            //mySas.GravityCompensation = new Vector3(0, 0, 9.81f);
            scene.AddUpdateable(mySas);

            BinaryWriter writer = new BinaryWriter(File.OpenWrite("test.sgl"));
            ship.Save(writer);
            writer.Close();

            
            scene.AddRenderable("skybox", skyBox);
            scene.AddRenderable("opaque", myRain);
            scene.AddRenderable("opaque", awning);
            scene.Add("opaque", liftEntity);
            scene.Add("opaque", ship);
            gen.GenerateCity(scene, GraphicsDevice, effect, 10, 10);
            //scene.AddRenderable(mesh);
            scene.AddRenderable("transparent", blood);
            scene.AddRenderable("transparent", fence);

            BasicEffectInterface shadowInterface = new BasicEffectInterface(new BasicEffect(GraphicsDevice) { TextureEnabled = false, VertexColorEnabled = false }, "BasicEffect");
            shadowInterface.InitBasicEffect();
            scene.ShadowEffect = shadowInterface;

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

            myInterface.Update(gameTime);
            
            if (KeyboardManager.IsKeyDown(Keys.U))
            {
                float impulse = 5.0f;

                ship.ApplyRelativeImpulse(new Vector3(-5, -5, -5), Vector3.UnitY * impulse);
                ship.ApplyRelativeImpulse(new Vector3(-5, -5, -5), Vector3.UnitY * impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, -5, 5), Vector3.UnitY * impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, -5, 5), Vector3.UnitY * impulse);
            }
            if (KeyboardManager.IsKeyDown(Keys.J))
            {
                float impulse = 5.0f;

                ship.ApplyRelativeImpulse(new Vector3(-5, 5, -5), Vector3.UnitY * -impulse);
                ship.ApplyRelativeImpulse(new Vector3(-5, 5, -5), Vector3.UnitY * -impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, 5, 5), Vector3.UnitY * -impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, 5, 5), Vector3.UnitY * -impulse);
            }

            if (KeyboardManager.IsKeyDown(Keys.H))
            {
                float impulse = 5.0f;

                ship.ApplyRelativeImpulse(new Vector3(-5, -5, -5), Vector3.UnitX * -impulse);
                ship.ApplyRelativeImpulse(new Vector3(-5, 5, -5), Vector3.UnitX * -impulse);
                ship.ApplyRelativeImpulse(new Vector3(-5, 5, 5), Vector3.UnitX * -impulse);
                ship.ApplyRelativeImpulse(new Vector3(-5, -5, 5), Vector3.UnitX * -impulse);
            }
            if (KeyboardManager.IsKeyDown(Keys.K))
            {
                float impulse = 5.0f;

                ship.ApplyRelativeImpulse(new Vector3(5, -5, -5), Vector3.UnitX * impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, 5, -5), Vector3.UnitX * impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, 5, 5), Vector3.UnitX * impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, -5, 5), Vector3.UnitX * impulse);
            }

            if (KeyboardManager.IsKeyDown(Keys.P))
            {
                float impulse = 15.0f;

                ship.ApplyRelativeImpulse(new Vector3(-5, -5, -5), Vector3.UnitZ * impulse);
                ship.ApplyRelativeImpulse(new Vector3(-5, 5, -5), Vector3.UnitZ * impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, -5, -5), Vector3.UnitZ * impulse);
                ship.ApplyRelativeImpulse(new Vector3(5, 5, -5), Vector3.UnitZ * impulse);
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

            isRayIntersecting = myPlayer.RayCast(3.0f);

            myInterface.Render(GraphicsDevice, gameTime);

            spriteBatch.Begin();
            
            //spriteBatch.DrawString(myDebugFont, myFpsMonitor.Average.ToString("0.00"), new Vector2(myFpsMonitor.Bounds.Right, myFpsMonitor.AveragePosition), Color.White);

            spriteBatch.DrawString(myDebugFont, "POS: " + (myCamera as SimpleCamera).Position, Vector2.Zero, Color.White);
            spriteBatch.DrawString(myDebugFont, "DIR: " + (myCamera as SimpleCamera).Normal, new Vector2(0, 15), Color.White);

            if (isRayIntersecting)
            {
                spriteBatch.DrawString(myDebugFont, "Intersecting", new Vector2(0, 30), Color.Red);
            }

            spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}
