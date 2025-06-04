using System;
using Character;
using Components;
using Zenject;

namespace SaveLoad
{
    public class CharacterMediator : IDisposable
    {
        private IRepository<CharacterStats> repository;
        private IDataContainer<CharacterStats> dataContainer;
        private SpaceshipController spaceshipController;
        private MoveComponent moveComponent;

        [Inject]
        public void Construct(
            IRepository<CharacterStats> repository,
            IDataContainer<CharacterStats> dataContainer,
            SpaceshipController spaceshipController,
            MoveComponent moveComponent)
        {
            this.repository = repository;
            this.dataContainer = dataContainer;
            this.spaceshipController = spaceshipController;
            this.moveComponent = moveComponent;
            
            LoadCharacterStats();
        }

        private void LoadCharacterStats()
        {
            CharacterStats stats;
            if (dataContainer.HasData())
            {
                stats = dataContainer.Data.Value;
            }
            else
            {
                stats = new CharacterStats();
            }

            spaceshipController.SetupStats(stats);
            moveComponent.SetInitialPositionAndRotation(stats.position, stats.rotationZ);
        }

        private void SaveCharacterStats()
        {
            var characterModel = spaceshipController.CharacterModel;
            var characterData = new CharacterStats
            {
                position = characterModel.Position.Value,
                rotationZ = characterModel.Rotation.Value,
            };

            repository.Save(characterData);
        }

        public void Dispose()
        {
            SaveCharacterStats();
        }
    }
}