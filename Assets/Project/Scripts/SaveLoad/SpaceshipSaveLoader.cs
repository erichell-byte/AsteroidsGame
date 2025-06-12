using Character;
using UnityEngine;

namespace SaveLoad
{
    public class SpaceshipSaveLoader: SaveLoader<SpaceshipModel, SpaceshipData>
    {
        protected override SpaceshipData ConvertToData(SpaceshipModel model)
        {
            return new SpaceshipData()
            {
                positionX = model.Position.Value.x,
                positionY = model.Position.Value.y,
                rotationZ = model.Rotation.Value,
            };
        }

        protected override void SetupData(SpaceshipModel service, SpaceshipData data)
        {
            service.SetPosition(new Vector2(data.positionX, data.positionY));
            service.SetRotation(data.rotationZ);
        }
    }
}