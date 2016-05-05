using Justice.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Justice.Gameplay;

using IUpdateable = Justice.Gameplay.IUpdateable;
using BEPUphysics;

namespace Justice.Geometry
{
    /// <summary>
    /// Represents a scene that can be rendered
    /// </summary>
    public abstract class IScene
    {
        private class NullTextureBinder : IRenderable
        {
            public override bool IsPreRendered
            {
                get { return true; }
            }

            private static readonly BoundingBox BOUNDS = new BoundingBox();
            public override BoundingBox RenderBounds
            {
                get { return BOUNDS; }
            }

            public override void Init(GraphicsDevice graphics)
            {
            }

            public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
            {
                graphics.Textures[0] = null;
            }
        }


        /// <summary>
        /// Stores the list of human input controlers in this scene
        /// </summary>
        protected List<IUpdateable> myUpdateables;
        /// <summary>
        /// Adds a new effect group to this scene
        /// </summary>
        protected Dictionary<string, EffectGroup> myEffectGroups;
        protected List<EffectGroup> myEffectList;

        protected Space myPhysicsSpace;

        /// <summary>
        /// Gets this scene's physics space
        /// </summary>
        public Space PhysicsSpace
        {
            get { return myPhysicsSpace; }
        }

        /// <summary>
        /// Creates a new instance of the scene, this must be called by child classes
        /// </summary>
        protected IScene()
        {
            myUpdateables = new List<IUpdateable>();

            myEffectGroups = new Dictionary<string, EffectGroup>();
            myEffectList = new List<EffectGroup>();

            myPhysicsSpace = new Space();
            myPhysicsSpace.ForceUpdater.Gravity = new BEPUutilities.Vector3(0, 0, -9.81f);
        }

        /// <summary>
        /// Iterate over all renderable instances in this scene
        /// </summary>
        /// <param name="action">The action to perform on all renderables</param>
        public void IterateRenderables(Action<IRenderable> action)
        {
            for (int index = 0; index < myEffectList.Count; index++)
                myEffectList[index].IterateRenderables(action);

            // Iterate over all renderables
            //for(int index = 0; index < myRenderables.Count; index ++)
            //{
            //    // If the renderable is not null
            //    if (myRenderables[index] != null)
            //    {
            //        // Perform the action on the device
            //        action(myRenderables[index]);
            //    }
            //}
        }
        
        public void Add(string effectGroup, PhysicsEntity entity)
        {
            entity.AddToScene(myPhysicsSpace);
            myUpdateables.Add(entity);
            AddRenderable(effectGroup, entity);
        }

        public void Add(string effectGroup, Entity entity)
        {
            myUpdateables.Add(entity);
            AddRenderable(effectGroup, entity);
        }

        /// <summary>
        /// Adds a new effect group that entities can be added to
        /// </summary>
        /// <param name="group"></param>
        public void AddEffectGroup(EffectGroup group)
        {
            myEffectGroups.Add(group.EffectName, group);
            myEffectList.Add(group);
            myEffectList.Sort((X, Y) => X.RenderPriority.CompareTo(Y.RenderPriority));
        }

        public void Add(EntityPather pather)
        {
            pather.AddToScene(myPhysicsSpace);
            myUpdateables.Add(pather);
        }

        public void AddCollider(ISpaceObject collider)
        {
            myPhysicsSpace.Add(collider);
        }

        BEPUutilities.Ray __myIntersectRay;
        /// <summary>
        /// Gets whether a ray intersects this scene's physics space
        /// </summary>
        /// <param name="rayCast">The ray to raycast against</param>
        /// <returns>True if the ray intersects the scene, false if otherwise</returns>
        public bool Intersects(Ray rayCast)
        {
            __myIntersectRay.Position.X = rayCast.Position.X;
            __myIntersectRay.Position.Y = rayCast.Position.Y;
            __myIntersectRay.Position.Z = rayCast.Position.Z;

            __myIntersectRay.Direction.X = rayCast.Direction.X;
            __myIntersectRay.Direction.Y = rayCast.Direction.Y;
            __myIntersectRay.Direction.Z = rayCast.Direction.Z;

            RayCastResult result;

            return myPhysicsSpace.RayCast(__myIntersectRay, out result);
        }

        /// <summary>
        /// Gets whether a ray intersects this scene's physics space
        /// </summary>
        /// <param name="rayCast">The ray to raycast against</param>
        /// <param name="maxLength">The maximum length of the array</param>
        /// <returns>True if the ray intersects the scene, false if otherwise</returns>
        public bool Intersects(Ray rayCast, float maxLength)
        {
            __myIntersectRay.Position.X = rayCast.Position.X;
            __myIntersectRay.Position.Y = rayCast.Position.Y;
            __myIntersectRay.Position.Z = rayCast.Position.Z;

            __myIntersectRay.Direction.X = rayCast.Direction.X;
            __myIntersectRay.Direction.Y = rayCast.Direction.Y;
            __myIntersectRay.Direction.Z = rayCast.Direction.Z;

            RayCastResult result;

            return myPhysicsSpace.RayCast(__myIntersectRay, maxLength, out result);
        }
        
        /// <summary>
        /// Adds a new renderable instance to the given effect group
        /// </summary>
        /// <param name="effectName">The name of the effect group to add to</param>
        /// <param name="renderable">The renderable instance to add</param>
        public void AddRenderable(string effectName, IRenderable renderable)
        {
            myEffectGroups[effectName].AddRenderable(renderable);
        }

        /// <summary>
        /// Adds a renderable isntance to this scene
        /// </summary>
        /// <param name="renderable">The isntance to add to the scene</param>
        //public void AddRenderable(IRenderable renderable)
        //{
        //    int pos = __getRenderableInsert(renderable);
        //    myRenderables.Insert(pos, renderable);
        //}
        
        /// <summary>
        /// Adds an updateable isntance to this scene
        /// </summary>
        /// <param name="instance">The instance to add</param>
        public void AddUpdateable(IUpdateable instance)
        {
            myUpdateables.Add(instance);
        }

        /// <summary>
        /// Renders this scence with the given camera
        /// </summary>
        /// <param name="graphics">The graphics device to render with</param>
        /// <param name="camera">The camera to render with</param>
        public void Render(GraphicsDevice graphics, ICamera camera)
        {
            // Prepare the camera for the frame
            camera.InitFrame();

            // Creates the camera frustum to use
            BoundingFrustum cameraFrustum = new BoundingFrustum(camera.Matrices.ViewProj);

            for (int index = 0; index < myEffectList.Count; index++)
            {
                myEffectList[index].Render(graphics, cameraFrustum, camera);
            }
        }

        /// <summary>
        /// Updates this scene and all of it's entities
        /// </summary>
        /// <param name="gameTime">The current timing snapshot</param>
        public void Update(GameTime gameTime)
        {
            myPhysicsSpace.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            for (int index = 0; index < myUpdateables.Count; index++)
                if (myUpdateables[index].IsActive)
                    myUpdateables[index].Update(gameTime);
        }
    }
}
