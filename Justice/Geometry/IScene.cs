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
        /// Stores the intenal list of renderable items in this scene
        /// </summary>
        protected List<IRenderable> myRenderables;

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
            myRenderables = new List<IRenderable>();
            myUpdateables = new List<IUpdateable>();

            myPhysicsSpace = new Space();
            myPhysicsSpace.ForceUpdater.Gravity = new BEPUutilities.Vector3(0, 0, -9.81f);

            myRenderables.Add(new NullTextureBinder());
        }

        /// <summary>
        /// Iterate over all renderable instances in this scene
        /// </summary>
        /// <param name="action">The action to perform on all renderables</param>
        public void IterateRenderables(Action<IRenderable> action)
        {
            // Iterate over all renderables
            for(int index = 0; index < myRenderables.Count; index ++)
            {
                // If the renderable is not null
                if (myRenderables[index] != null)
                {
                    // Perform the action on the device
                    action(myRenderables[index]);
                }
            }
        }

        private List<IRenderable> internalUseCollection;
        /// <summary>
        /// Gets an array of visible renderable elements in a camera's view
        /// This is a slow-ass operation, so use sparingly
        /// </summary>
        /// <param name="camera">The camera to check for visibility against</param>
        /// <returns></returns>
        public IRenderable[] GetVisible(ICamera camera)
        {
            // If the internal collection is null, then make one
            if (internalUseCollection != null)
                internalUseCollection = new List<IRenderable>(100);

            // Clear the internal use collection
            internalUseCollection.Clear();

            // Creates the camera frustum to use
            BoundingFrustum cameraFrustum = new BoundingFrustum(camera.Matrices.ViewProj);

            // Iterate over all renderables
            for (int index = 0; index < myRenderables.Count; index++)
            {
                // Check first for null, then perform a rough pass filter, then fine pass
                if (myRenderables[index] != null && cameraFrustum.Intersects(myRenderables[index].RenderBounds) && myRenderables[index].ShouldRender(cameraFrustum))
                {
                    internalUseCollection.Add(myRenderables[index]);
                }
            }

            // Return the collection
            return internalUseCollection.ToArray();
        }

        public void Add(PhysicsEntity entity)
        {
            entity.AddToScene(myPhysicsSpace);
            myUpdateables.Add(entity);
            myRenderables.Add(entity);
        }

        public void Add(Entity entity)
        {
            myUpdateables.Add(entity);
            myRenderables.Add(entity);
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
        /// Adds a renderable isntance to this scene
        /// </summary>
        /// <param name="renderable">The isntance to add to the scene</param>
        public void AddRenderable(IRenderable renderable)
        {
            int pos = __getRenderableInsert(renderable);
            myRenderables.Insert(pos, renderable);
        }
        
        private static IRenderable __getRenderableInsertCached;
        protected int __getRenderableInsert(IRenderable renderable, int start = 0, int end = -1)
        {
            if (renderable.IsPreRendered)
                return 1;

            if (renderable.IsTransparent)
                return myRenderables.Count;

            if (renderable.Texture == null)
            {
                int pos = myRenderables.FindIndex(1, x => !x.IsPreRendered);
                return pos == -1 ? myRenderables.Count : pos;
            }
            else
            {
                start = myRenderables.FindIndex(start + 1, x => x.Texture != null);

                if (start == -1)
                    return myRenderables.Count;
            }

            end = end == -1 ? myRenderables.Count - 1 : end;

            int mid = (start + end) / 2;

            if (mid == start)
                return mid + 1;

            __getRenderableInsertCached = myRenderables[mid];

            if (__getRenderableInsertCached.Texture == null)
                return __getRenderableInsert(renderable, mid, end);
            else
            {
                int comparison = __getRenderableInsertCached.Texture.GetHashCode().CompareTo(renderable.GetHashCode());

                if (comparison == 0)
                    return mid;
                else if (comparison < 0)
                    return __getRenderableInsert(renderable, start, mid);
                else
                    return __getRenderableInsert(renderable, mid, end);
            }
        }

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

            // Iterate over all renderables
            for (int index = 0; index < myRenderables.Count; index++)
            {
                // Check first for null, then perform a rough pass filter, then fine pass
                if (myRenderables[index] != null && myRenderables[index].IsVisible && myRenderables[index].ShouldRender(cameraFrustum))
                {
                    // If the item should be rendered, draw that bitch
                    myRenderables[index].Render(graphics, camera.Matrices);
                }
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

        internal void RemoveRenderable(IRenderable item)
        {
            myRenderables.Remove(item);
        }
    }
}
