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

namespace Justice.SpaceGame
{
    public class Ship : PhysicsEntity
    {
        private List<CompoundShapeEntry> myPhysicsShapes;
        private EntityMover myEntityMover;
        private EntityRotator myEntityRotator;

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

        public void BakeShape(float mass)
        {
            CompoundShape shape = new CompoundShape(myPhysicsShapes);
            
            myPhysicsEntity = new CompoundBody(myPhysicsShapes, mass);

            myEntityMover = new EntityMover(myPhysicsEntity);
            myEntityRotator = new EntityRotator(myPhysicsEntity);
        }

        public void AddShape(EntityShape shape)
        {
            myPhysicsShapes.Add(new CompoundShapeEntry(shape, 1));
        }

        public void AddShape(EntityShape shape, Vector3 translation)
        {
            myPhysicsShapes.Add(new CompoundShapeEntry(shape, translation.Convert(), 1)); 
        }

        public void AddShape(EntityShape shape, Vector3 translation, Quaternion rotation)
        {
            myPhysicsShapes.Add(new CompoundShapeEntry(shape, new RigidTransform(translation.Convert(), rotation.Convert()), 1));
        }

        public override void Update(GameTime gameTime)
        {

        }

        public void ApplyImpulse(Vector3 position, Vector3 impulse)
        {
            myPhysicsEntity.ApplyImpulse(position.Convert(), impulse.Convert());
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
    }
}
