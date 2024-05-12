using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer2D.Crystal;
using Platformer2D.Background;
using Platformer2D.Player;
using Platformer2D.Platform;


/*
Класс LevelView представляет визуальное отображение уровня в игре. 
Он содержит ссылки на префабы для кристаллов, платформ и фона, которые используются для создания этих элементов на уровне.
Необходима доработка - параметры будут задаваться генератором
*/

namespace Platformer2D.Level
{
    // Класс LevelView используется как префаб для сборки визуальной части уровня на игровой сцене
    public class LevelView : MonoBehaviour
    {
        public LevelModel model;
        // Префабы для кристаллов, платформ и фона - задаются на игровой сцене
        [SerializeField] public CrystalView crystalPrefab;
        [SerializeField] public PlatformView platformPrefab;
        [SerializeField] public PlatformView platformPrefabBounds;
        [SerializeField] public PlatformView platformPrefabSpecial;
        [SerializeField] public BackgroundView backgroundPrefab;

        [SerializeField] public DoorView[] doors;  // Все двери
        

        public LocationType crystalType;

        // Ссылка на контроллер уровня
        private LevelController controller;

        // Вызывается перед первым обновлением кадра
        void Start()
        {
            //CreateModel();
            
        }
        public void SetController()
        {   SetModel();
            //controller = new LevelController(model, this); 
        }

        private void CreateModel()
        {
            List<Vector3> coordinatesCrystal = new List<Vector3>
            {
                new Vector3(-1,-1, 0),
                new Vector3(0, -1, 0),
                new Vector3(1, -1, 0)
            };

            List<Vector3> coordinatesPlatforms = new List<Vector3>
            {
                new Vector3(-7,-2, 0),
                new Vector3(-6, -1, 0),
                new Vector3(-2, -0, 0),
                new Vector3(-2, -1, 0)
            };

            List<Vector3> coordinatesPlatformsSpecial = new List<Vector3>
            {
                new Vector3(-2,-2, 0),
                new Vector3(-1, -1, 0),
                new Vector3(4, -0, 0),
                new Vector3(5, -1, 0)
            };

            List<Vector3> coordinatesDoors = new List<Vector3>
            {
                new Vector3(10,2, 0),
                new Vector3(15,2, 0),
            };

            // Создание модели уровня
            
            

            // Установка модели
            SetModel();
        }

        public void SetModel()
        {
            // Создание GameObjectModel для каждого префаба
            model.Crystal.ForEach(c => c.Prefab = crystalPrefab);
            model.Platform.ForEach(p => p.Prefab = platformPrefab);
            model.SpecialPlatform.ForEach(sp => sp.Prefab = platformPrefabSpecial);
            model.Bounds.ForEach(sp => sp.Prefab = platformPrefabBounds);
            model.Background.Prefab = backgroundPrefab;
            model.Bounds.ForEach(b => b.Prefab = platformPrefabBounds);
            Debug.Log("model is " + (model == null ? "null" : "not null"));
            Debug.Log("model.Doors is " + (model.Doors == null ? "null" : "not null"));

            // Назначение префабов для model.Doors
            foreach (var door in model.Doors)
            {
                // Получение индекса префаба двери на основе типа локации
                int doorPrefabIndex = (int)door.Value.TypeDoor;
                // Назначение соответствующего префаба двери
                door.Key.Prefab = doors[doorPrefabIndex];
            }

            controller = new LevelController(model, this); 
        }


        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
