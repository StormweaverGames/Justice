using Justice.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Justice.Gameplay;

using IUpdateable = Justice.Gameplay.IUpdateable;
using Justice.Physics;
using BEPUphysics;

namespace Justice.Geometry
{
    /// <summary>
    /// Represents a scene that can be rendered
    /// </summary>
    public abstract class IScene
    {
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
        /// Creates a new instance of the scene, this must be called by child classes
        /// </summary>
        protected IScene()
        {
            myRenderables = new List<IRenderable>();
            myUpdateables = new List<IUpdateable>();

            myPhysicsSpace = new Space();
            myPhysicsSpace.ForceUpdater.Gravity = new BEPUutilities.Vector3(0, 0, -9.81f);
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
                if (myRenderables[index] != null && cameraFrustum.Intersects(myRenderables[index].Bounds) && myRenderables[index].ShouldRender(cameraFrustum))
                {
                    internalUseCollection.Add(myRenderables[index]);
                }
            }

            // Return the collection
            return internalUseCollection.ToArray();
        }

        public void AddCollider(ISpaceObject collider)
        {
            myPhysicsSpace.Add(collider);
        }
        
        /// <summary>
        /// Adds a renderable isntance to this scene
        /// </summary>
        /// <param name="renderable">The isntance to add to the scene</param>
        public void AddRenderable(IRenderable renderable)
        {
            myRenderables.Add(renderable);
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
                if (myRenderables[index] != null && cameraFrustum.Intersects(myRenderables[index].Bounds) && myRenderables[index].ShouldRender(cameraFrustum))
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
