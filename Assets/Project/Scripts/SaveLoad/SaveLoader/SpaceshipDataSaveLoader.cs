using Character;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SpaceshipDataSaveLoader : ISaveLoader
    {
        private SpaceshipModel _service;

        [Inject]
        private void Construct(SpaceshipModel service)
        {
            this._service = service;
        }
        
        public void SaveGame(IGameRepository repository)
        {
            SpaceshipData data = ConvertToData(_service);
            repository.SetData(data);
        }

        public void LoadGame(IGameRepository repository)
        {
            if (repository.TryGetData(out SpaceshipData data))
            {
                SetupData(_service, data);
            }
            else
            {
                SetupDefaultData(_service);
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
                PositionX = model.Position.Value.x,
                PositionY = model.Position.Value.y,
                RotationZ = model.Rotation.Value,
            };
        }

        private void SetupData(SpaceshipModel service, SpaceshipData data)
        {
            service.SetPosition(new Vector2(data.PositionX, data.PositionY));
            service.SetRotation(data.RotationZ);
        }

        protected void SetupDefaultData(SpaceshipModel service)
        {
            service.SetPosition(new Vector2(0, 0));
            service.SetRotation(0f);
        }
    }
}