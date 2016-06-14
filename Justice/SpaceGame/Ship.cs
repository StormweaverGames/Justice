using Justice.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.CollisionShapes;
using Microsoft.Xna.Framework;
using Justice.Tools;
using BEPUphysics.Paths.PathFollowing;
using BEPUphysics.Entities.Prefabs;

using RigidTransform = BEPUutilities.RigidTransform;
using BepuQuaternion = BEPUutilities.Quaternion;
using System.IO;
using BEPUphysics.CollisionShapes.ConvexShapes;
using Justice.Geometry;
using BEPUphysics.UpdateableSystems.ForceFields;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.SpaceGame
{
    public class Ship : PhysicsEntity
    {
        private List<CompoundShapeEntry> myPhysicsShapes;
        private EntityMover myEntityMover;
        private EntityRotator myEntityRotator;

        private ShipGravityField myGravityField;
        private BEPUutilities.Matrix myCenterOffest;

        public override Vector3 Position
        {
            get;
            set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public Ship()
        {
            myPhysicsShapes = new List<CompoundShapeEntry>();

            Name = "Unknown";
        }

        public void BakeShape(float mass, Vector3 position)
        {
            CompoundShape shape = new CompoundShape(myPhysicsShapes);

            if (mass == -1.0f)
                mass = CalculateMass(shape, 1.0f);
            
            myPhysicsEntity = new CompoundBody(myPhysicsShapes, mass);

            System.Diagnostics.Debug.WriteLine(myPhysicsEntity.Position);

            myCenterOffest = BEPUutilities.Matrix.CreateTranslation(-myPhysicsEntity.Position);

            if (myRenderable != null)
                myRenderable.LocalTransform = myCenterOffest.Convert();

            myPhysicsEntity.Tag = this;
            myPhysicsEntity.Material = new BEPUphysics.Materials.Material(0.6f, 0.3f, 0.0f);
            myPhysicsEntity.Position = position.Convert();
            myPhysicsEntity.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            
            RigidTransform transform = new RigidTransform();
            BEPUutilities.BoundingBox shipBounds;
            shape.GetBoundingBox(ref transform, out shipBounds);
            shipBounds.Min *= 1.1f;
            shipBounds.Max *= 1.1f;
            
            myGravityField = new ShipGravityField(this, new BoundingBoxForceFieldShape(shipBounds));

            myEntityMover = new EntityMover(myPhysicsEntity);
            myEntityRotator = new EntityRotator(myPhysicsEntity);
        }

        private float CalculateMass(CompoundShape shape, float density)
        {
            return shape.Volume * density;
        }

        public override void AddToScene(Space space)
        {
            base.AddToScene(space);

            space.Add(myGravityField);
        }

        public void AddShape(EntityShape shape, float weight = -1)
        {
            if (weight == -1.0f)
                weight = shape.Volume;

            myPhysicsShapes.Add(new CompoundShapeEntry(shape, weight));
        }

        public void AddShape(EntityShape shape, Vector3 translation, float weight = -1)
        {
            if (weight == -1.0f)
                weight = shape.Volume;

            myPhysicsShapes.Add(new CompoundShapeEntry(shape, translation.Convert(), weight)); 
        }

        public void AddShape(EntityShape shape, Vector3 translation, Quaternion rotation, float weight = -1)
        {
            if (weight == -1.0f)
                weight = shape.Volume; 

            myPhysicsShapes.Add(new CompoundShapeEntry(shape, new RigidTransform(translation.Convert(), rotation.Convert()), weight));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            RigidTransform transform = new RigidTransform();
            BEPUutilities.BoundingBox shipBounds;
            myPhysicsEntity.CollisionInformation.Shape.GetBoundingBox(ref transform, out shipBounds);

            shipBounds.Min = BEPUutilities.Matrix.Transform(shipBounds.Min, myPhysicsEntity.WorldTransform).ToVector3();
            shipBounds.Max = BEPUutilities.Matrix.Transform(shipBounds.Max, myPhysicsEntity.WorldTransform).ToVector3();

            shipBounds.Min *= 1.1f;
            shipBounds.Max *= 1.1f;

            myGravityField.Shape = new BoundingBoxForceFieldShape(shipBounds);
        }

        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            base.Render(graphics, matrices);
        }

        public void ApplyImpulse(Vector3 position, Vector3 impulse)
        {
            BEPUutilities.Vector4 translated = BEPUutilities.Matrix.Transform(position.Convert(), myCenterOffest * myPhysicsEntity.WorldTransform);
            
            myPhysicsEntity.ApplyImpulse(new BEPUutilities.Vector3(translated.X, translated.Y, translated.Z), impulse.Convert());
        }

        public void ApplyRelativeImpulse(Vector3 position, Vector3 impulse)
        {
            BEPUutilities.Vector4 translatedPos = BEPUutilities.Matrix.Transform(position.Convert(), myCenterOffest * myPhysicsEntity.WorldTransform);
            BEPUutilities.Vector3 translatedImpulse = BEPUutilities.Matrix.TransformNormal(impulse.Convert(), myCenterOffest *  myPhysicsEntity.WorldTransform);

            myPhysicsEntity.ApplyImpulse(
                new BEPUutilities.Vector3(translatedPos.X, translatedPos.Y, translatedPos.Z), translatedImpulse * impulse.Length());
        }

        public void ApplyAngularImpulse(Vector3 impulse)
        {
            myPhysicsEntity.ActivityInformation.Activate();
            BEPUutilities.Vector3 BEPUimpulse = impulse.Convert();
            myPhysicsEntity.ApplyAngularImpulse(ref BEPUimpulse);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Mass);

            (myPhysicsEntity as CompoundBody).CollisionInformation.Shape.Serialize(writer);

            // TODO: serialize renderable mesh
        }

        public static Ship Load(BinaryReader reader)
        {
            Ship result = new Ship();

            string name = reader.ReadString();
            float mass = reader.ReadSingle();

            result.Name = name;
            result.Mass = mass;

            CompoundShape shape = PhysicsSerializers.DeserializeCompound(reader);
            result.myPhysicsEntity = new CompoundBody(shape.Shapes, mass);

            // TODO: deserialize renderable mesh

            return result;
        }

        public void SetRenderable(IRenderable renderable)
        {
            myRenderable = renderable;
        }
    }
}
