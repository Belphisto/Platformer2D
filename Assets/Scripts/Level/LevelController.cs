using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Crystal;
using Platformer2D.Background;
using UnityEngine.Animations;
using System;

using Platformer2D.Player;
using Platformer2D.Platform;

/*
Класс LevelController отвечает за управление уровнем игры. 
Он содержит модель данных и представление уровня, а также методы для создания:
    - кристаллов,
    - фона
    - и платформ на уровне. 
Класс также подписывается на событие обновления счета игрока и обрабатывает это событие, увеличивая счет уровня и вызывая событие обновления счета платформ.
Этот класс является ключевым элементом единицы игрового уровня, связывающим вместе все игровые объекты.
*/

namespace Platformer2D.Level
{
    // Класс LevelController управляет уровнем игры
    public class LevelController 
    {
        // Модель данных уровня
        private LevelModel model;
        private LocationModel modelLocation;
        // Представление уровня в игровой сцене
        private LevelView view;
        //Событие для вызова обновления счетчика очков платформы
        public static event Action<int> OnScoreUpdatePlatfroms;
        
        // Конструктор класса
        // Принимает модель и представление
        // Вызывает методы создания объектов на сцене
        public LevelController(LevelModel model, LevelView view)
        {
            this.model = model;
            this.view = view;

            SpawnCrystals();
            SpawnColorChangeBackground();
            SpawnPlatforms();
            SpawnPlayer();
            //Подписка на событие отправки обновления счетчика очков на уровне в платформу
            Bus.Instance.SendScore += HandleScoreUpdate;
        }

        public LevelController(LocationModel model, LevelView view)
        {
            this.modelLocation = model;
            this.view = view;

            SpawnCrystals2();
            SpawnColorChangeBackground2();
            SpawnPlatforms2();
            SpawnPlayer2();
            SpawnDoor();
            SpawnChest();
            
            //Подписка на событие отправки обновления счетчика очков на уровне в платформу
            Bus.Instance.SendScore += HandleScoreUpdate;
        }

        // Метод для создания кристаллов на сцене
        private void SpawnCrystals()
        {
            int scorePerCrystal = model.TotalScore / model.CrystalCount;
            Debug.Log($"LevelController :SpawnCrystals() scorePerCrystal= {scorePerCrystal}");
            foreach (var gameObjectModel in model.Crystal)
            {
                var crystal = (CrystalView) UnityEngine.Object.Instantiate(gameObjectModel.Prefab, gameObjectModel.Position, Quaternion.identity);
                var crystalModel = new CrystalModel(scorePerCrystal, view.crystalType);
                // Созданная с заданными параметрами модель кристалла передается CrystalModel в представление CrystalView
                crystal.SetModel(crystalModel); 
            }
        }
        private void SpawnCrystals2()
        {
            foreach (var positionCrystal in modelLocation.PositionCrystal)
            {
                var crystal = (CrystalView) UnityEngine.Object.Instantiate(view.crystalPrefab, positionCrystal.Key, Quaternion.identity);
                var crystalModel = positionCrystal.Value;
                // Созданная с заданными параметрами модель кристалла передается CrystalModel в представление CrystalView
                crystal.SetModel(crystalModel); 
            }
        }

        // Метод для создания изменяющегося фона на сцене
        private void SpawnColorChangeBackground()
        {
            var colorChangeBackground = UnityEngine.Object.Instantiate(view.backgroundPrefab);
            var controller = new BackgroundControlller(new BackgroundModel(), colorChangeBackground);
            controller.SetTargerScore(model.TotalScore);
            colorChangeBackground.SetController(controller);
        }
        private void SpawnColorChangeBackground2()
        {
            var colorChangeBackground = UnityEngine.Object.Instantiate(view.backgroundPrefab, modelLocation.PositionBackground, Quaternion.identity);
            var controller = new BackgroundControlller(new BackgroundModel(), colorChangeBackground);
            controller.SetTargerScore(model.TotalScore);
            colorChangeBackground.SetController(controller);
        }

        // Метод для создания платформ на уровне
        public void SpawnPlatforms()
        {
            SpawnPlatform(model.Platform, 20, 0);
            SpawnPlatform(model.SpecialPlatform, 40, 0.8f);
            SpawnPlatform(model.Bounds, 40, 0f);

        }
        // Метод для создания платформ на уровне
        public void SpawnPlatforms2()
        {
            SpawnPlatform2(modelLocation.PositionPlatformStatic, view.platformPrefab);
            SpawnPlatform2(modelLocation.PositionPlatformSpecial, view.platformPrefabSpecial);
            SpawnPlatform2(modelLocation.PositionPlatformBounds, view.platformPrefabBounds);
        }

        // Метод для создания платформы
        private void SpawnPlatform(List<GameObjectModel> platformModels, int score, float speed)
        {
            foreach (var gameObjectModel in platformModels)
            {
                var platform = (PlatformView) UnityEngine.Object.Instantiate(gameObjectModel.Prefab, gameObjectModel.Position, Quaternion.identity);
                var platformModel = new PlatformModel(score, speed);
                platformModel.StartPosition = gameObjectModel.Position;
                platform.SetModel(platformModel);
            }
        }
        private void SpawnPlatform2(Dictionary<Vector3, PlatformModel> platformModels, PlatformView platformPrefab)
        {
            foreach (var position in platformModels)
            {
                var platform = (PlatformView) UnityEngine.Object.Instantiate(platformPrefab, position.Key, Quaternion.identity);
                var platformModel = position.Value;
                platform.SetModel(platformModel);
            }
        }

        // Метод для обработки обновления счета от игрока
        public void HandleScoreUpdate(int score)
        {
            // Увеличивает счетчик текущего количества очков, собранных на уровне
            model.IncrementScore(score);
            // Вызывает событие для передачи текущего счета на уровне в платформу
            // Обновляет счет платформ, когда счет игрока меняется
            OnScoreUpdatePlatfroms?.Invoke(model.CurrentScore);
            Bus.Instance.SendLevelPercent(model.GetPercentLevel());
        }

        private void SpawnPlayer()
        {
            // Найти персонажа по тегу
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Проверить, найден ли персонаж
            if (player != null)
            {
                // Изменить координаты персонажа
                player.transform.position = new Vector3(2, 2, 0);
            }
            else
            {
                Debug.Log("Персонаж с тегом 'Player' не найден");
            }
        }
        private void SpawnPlayer2()
        {
            // Найти персонажа по тегу
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Проверить, найден ли персонаж
            if (player != null)
            {
                // Изменить координаты персонажа
                player.transform.position = modelLocation.PlayerPositionStart;
            }
            else
            {
                Debug.Log("Персонаж с тегом 'Player' не найден");
            }
        }

        private void SpawnDoor()
        {
            foreach (var positionDoor in modelLocation.PositionDoors)
            {
                // Находим соответствующий префаб двери
                DoorView doorPrefab = Array.Find(view.doors, door => door.type == positionDoor.Value.TypeLocation);
                if (doorPrefab != null)
                {
                    var door = (DoorView) UnityEngine.Object.Instantiate(doorPrefab, positionDoor.Key, Quaternion.identity);
                    var doorModel = positionDoor.Value;
                    door.SetModel(doorModel);
                }
            }
        }
        private void SpawnChest()
        {
            if (modelLocation.IsChestLocation)
            {
                // случайную платформу из всех платформ на уровне
                var platforms = new List<Vector3>(modelLocation.PositionPlatformStatic.Keys);
                var randomPlatformPosition = platforms[UnityEngine.Random.Range(0, platforms.Count)];

                // сундук находится непосредственно над платформой
                var chestPosition = new Vector3(randomPlatformPosition.x, randomPlatformPosition.y + 1, randomPlatformPosition.z);
                var chest = (ChestView) UnityEngine.Object.Instantiate(view.chestPrefab, chestPosition, Quaternion.identity);
            }
        }

        private void HandleUpdateLevelPercent()
        {

        }
    }
}

