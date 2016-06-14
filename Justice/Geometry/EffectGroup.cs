using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    public delegate void SceneRenderEvent(GraphicsDevice graphics, BoundingFrustum cameraFrustum, ICamera camera);

    /// <summary>
    /// Represents a grouping of renderable isntances grouped by effect
    /// </summary>
    public class EffectGroup
    {
        protected IEffectInterface myEffect;
        protected string myEffectName;
        protected List<IRenderable> myRenderables;

        public event SceneRenderEvent OnPreRender;
        public event SceneRenderEvent OnPostRender;

        public string EffectName
        {
            get { return myEffectName; }
        }

        public int RenderPriority
        {
            get;
            protected set;
        }
        
        public RasterizerState Rasterizer
        {
            get;
            set;
        }

        public BlendState BlendState
        {
            get;
            set;
        }

        public DepthStencilState DepthStencilState
        {
            get;
            set;
        }
        

        public EffectGroup(GraphicsDevice graphics, IEffectInterface effect, string effectName)
        {
            myEffect = effect;
            myEffectName = effectName;

            Rasterizer = graphics.RasterizerState;
            BlendState = graphics.BlendState;
            DepthStencilState = graphics.DepthStencilState;

            myRenderables = new List<IRenderable>();            
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

        protected static IRenderable __getRenderableInsertCached;
        protected int __getRenderableInsert(IRenderable renderable, int start = 0, int end = -1)
        {
            if (renderable.IsPreRendered)
                return 0;

            if (renderable.IsTransparent)
                return myRenderables.Count;

            if (renderable.Texture == null)
            {
                int pos = myRenderables.FindIndex(0, x => !x.IsPreRendered);
                return pos == -1 ? myRenderables.Count : pos;
            }
            else
            {
                start = myRenderables.FindIndex(start, x => x.Texture != null);

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

        public void IterateRenderables(Action<IRenderable> action)
        {
            for (int index = 0; index < myRenderables.Count; index++)
            {
                // If the renderable is not null
                if (myRenderables[index] != null)
                {
                    // Perform the action on the device
                    action(myRenderables[index]);
                }
            }
        }

        public void RenderShadows(GraphicsDevice graphics, IEffectInterface shadowEffect, BoundingFrustum viewFrustum, ICamera view)
        {
            IRenderable renderable;

            // Iterate over each pass in the technique
            for (int passIndex = 0; passIndex < shadowEffect.Technique.Passes.Count; passIndex++)
            {
                // Iterate over all renderables
                for (int index = 0; index < myRenderables.Count; index++)
                {
                    renderable = myRenderables[index];

                    // Check first for null, then perform a rough pass filter, then fine pass
                    if (renderable != null && renderable.IsVisible && renderable.ShouldRender(viewFrustum))
                    {
                        // Set the world matrix and apply the per-instance parameters
                        shadowEffect.SetWorldMatrix(renderable.WorldTransform);
                        shadowEffect.SetLocalMatrix(renderable.LocalTransform);
                        shadowEffect.CalculateInstanceParams();

                        // Apply the pas so prepare for rendering
                        shadowEffect.Technique.Passes[passIndex].Apply();

                        // If the item should be rendered, draw that bitch
                        renderable.RenderShadow(graphics, view.Matrices);
                    }
                }
            }
        }

        /// <summary>
        /// Renders this effect grouping
        /// </summary>
        /// <param name="graphics">The graphics device to use for rendering</param>
        /// <param name="cameraFrustum">The bounding frustum of the camera's view</param>
        /// <param name="camera">The camera instance to render with</param>
        public void Render(GraphicsDevice graphics, BoundingFrustum cameraFrustum, ICamera camera)
        {
            OnPreRender?.Invoke(graphics, cameraFrustum, camera);

            if (Rasterizer != null)
                graphics.RasterizerState = Rasterizer;

            if (BlendState != null)
                graphics.BlendState = BlendState;

            if (DepthStencilState != null)
                graphics.DepthStencilState = DepthStencilState;
            
            // Apply the view and projection matrices
            myEffect.SetProjectionMatrix(camera.Matrices.Projection);
            myEffect.SetViewMatrix(camera.Matrices.View);

            myEffect.CalculateFrameParams();

            EffectMaterial currentMaterial = new EffectMaterial();
            int currentMaterialId = -1;

            IRenderable renderable;

            // Iterate over each pass in the technique
            for (int passIndex = 0; passIndex < myEffect.Technique.Passes.Count; passIndex++)
            {                                
                // Iterate over all renderables
                for (int index = 0; index < myRenderables.Count; index++)
                {
                    renderable = myRenderables[index];

                    // Check first for null, then perform a rough pass filter, then fine pass
                    if (renderable != null && renderable.IsVisible && renderable.ShouldRender(cameraFrustum))
                    {
                        if (renderable.MaterialId != currentMaterialId)
                        {
                            currentMaterialId = renderable.MaterialId;
                            currentMaterial = renderable.Material;
                            myEffect.ApplyMaterial(currentMaterial);
                            graphics.SamplerStates[0] = currentMaterial.DiffuseSampler;
                        }

                        // Performe the pre-render operation
                        renderable.PreRender(graphics, camera);

                        // Set the world matrix and apply the per-instance parameters
                        myEffect.SetWorldMatrix(renderable.WorldTransform);
                        myEffect.SetLocalMatrix(renderable.LocalTransform);
                        myEffect.CalculateInstanceParams();

                        // Apply the pas so prepare for rendering
                        myEffect.Technique.Passes[passIndex].Apply();

                        // If the item should be rendered, draw that bitch
                        renderable.Render(graphics, camera.Matrices);
                    }
                }
            }

            OnPostRender?.Invoke(graphics, cameraFrustum, camera);
        }
    }
}
