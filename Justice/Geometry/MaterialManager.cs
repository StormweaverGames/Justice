using System;
using System.Collections.Generic;

namespace Justice.Geometry
{
    public class MaterialManager
    {
        public static MaterialManager Instance
        {
            get;
            private set;
        }

        static MaterialManager()
        {
            Instance = new MaterialManager();
        }

        /// <summary>
        /// Registers a new material to the material manager
        /// </summary>
        /// <param name="material">The material</param>
        /// <returns>The unique ID for the material</returns>
        public static int RegisterMaterial(EffectMaterial material)
        {
            return Instance.__registerMaterial(material);
        }

        private List<EffectMaterial> myMaterials;
        private Dictionary<string, int> myMaterialIds;

        public EffectMaterial this[int index]
        {
            get { return myMaterials[index]; }
        }

        public MaterialManager()
        {
            myMaterials = new List<EffectMaterial>();
            myMaterialIds = new Dictionary<string, int>();
        }

        /// <summary>
        /// Registers a new material to the material manager
        /// </summary>
        /// <param name="material">The material</param>
        /// <returns>The unique ID for the material</returns>
        private int __registerMaterial(EffectMaterial material)
        {
            if (myMaterials.Contains(material))
                return myMaterials.IndexOf(material);

            if (myMaterialIds.ContainsKey(material.Name) && material != myMaterials[myMaterialIds[material.Name]])
                throw new InvalidOperationException("Material by that name already exists");

            int materialId = myMaterials.Count;
            myMaterialIds.Add(material.Name, materialId);
            myMaterials.Add(material);
            return materialId;
        }
    }
}