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

            EffectTechnique technique = myEffect.Technique;

            // Apply the view and projection matrices
            myEffect.SetProjectionMatrix(camera.Matrices.Projection);
            myEffect.SetViewMatrix(camera.Matrices.View);

            myEffect.CalculateFrameParams();

            EffectMaterial currentMaterial = new EffectMaterial();

            // Iterate over each pass in the technique
            for (int passIndex = 0; passIndex < technique.Passes.Count; passIndex++)
            {
                // Applies the current effect pass
                technique.Passes[passIndex].Apply();
                                
                // Iterate over all renderables
                for (int index = 0; index < myRenderables.Count; index++)
                {
                    // Check first for null, then perform a rough pass filter, then fine pass
                    if (myRenderables[index] != null && myRenderables[index].IsVisible && myRenderables[index].ShouldRender(cameraFrustum))
                    {
                        if (MaterialManager.Instance[myRenderables[index].MaterialId] != currentMaterial)
                        {
                            currentMaterial = MaterialManager.Instance[myRenderables[index].MaterialId];
                            myEffect.ApplyMaterial(currentMaterial);
                            graphics.SamplerStates[0] = currentMaterial.DiffuseSampler;
                        }

                        myRenderables[index].PreRender(graphics, camera);

                        myEffect.SetWorldMatrix(myRenderables[index].WorldTransform);
                        myEffect.CalculateInstanceParams();

                        technique.Passes[passIndex].Apply();

                        // If the item should be rendered, draw that bitch
                        myRenderables[index].Render(graphics, camera.Matrices);
                    }
                }
            }

            OnPostRender?.Invoke(graphics, cameraFrustum, camera);
        }
    }
}
