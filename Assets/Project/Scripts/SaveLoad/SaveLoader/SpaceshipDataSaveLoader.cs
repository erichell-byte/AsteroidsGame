using Character;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SpaceshipDataSaveLoader : ISaveLoader
    {
        private SpaceshipModel service;

        [Inject]
        private void Construct(SpaceshipModel service)
        {
            this.service = service;
        }
        
        public void SaveGame(IGameRepository repository)
        {
            SpaceshipData data = ConvertToData(service);
            repository.SetData(data);
        }

        public void LoadGame(IGameRepository repository)
        {
            if (repository.TryGetData(out SpaceshipData data))
            {
                SetupData(service, data);
            }
            else
            {
                SetupDefaultData(service);
            }
        }

        public string GetSavedDataName()
        {
            return nameof(SpaceshipData);
        }

        private SpaceshipData ConvertToData(SpaceshipModel model)
        {
            return new SpaceshipData()
            {
                positionX = model.Position.Value.x,
                positionY = model.Position.Value.y,
                rotationZ = model.Rotation.Value,
            };
        }

        private void SetupData(SpaceshipModel service, SpaceshipData data)
        {
            service.SetPosition(new Vector2(data.positionX, data.positionY));
            service.SetRotation(data.rotationZ);
        }

        protected void SetupDefaultData(SpaceshipModel service)
        {
            service.SetPosition(new Vector2(0, 0));
            service.SetRotation(0f);
        }
    }
}