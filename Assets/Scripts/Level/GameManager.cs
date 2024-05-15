using System.Collections;
using System.Collections.Generic;
using Platformer2D.Level;
using UnityEngine;
using Platformer2D.Generator;
using Platformer2D.Platform;
using System.Linq;

namespace Platformer2D
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public LevelView redLevelPrefab;
        [SerializeField] public LevelView greenLevelPrefab;
        [SerializeField] public LevelView blueLevelPrefab;
        [SerializeField] public LevelView skyLevelPrefab;

        private Dictionary<LocationType, LevelView> levelPrefabs;
        private GeneratorGraph _locationNetwork;

        private GeneratorLocation generatorLocations;
        private Dictionary<int, (int, int)> doorToLocations;
        private Dictionary<int, LevelView> createdLevels;
        private Dictionary<int, DoorModel> doorModels;
        private List<int> indexCreatedLocation;
        private List<LevelView> levelViews;
        private int indexCurrentLocation;

        // Start is called before the first frame update
        void Start()
        {
            Bus.Instance.IndexNextLocation += CreateOrLoadLocation;
            levelPrefabs = new Dictionary<LocationType, LevelView>
            {
                { LocationType.Red, redLevelPrefab},
                { LocationType.Green, greenLevelPrefab},
                { LocationType.Blue, blueLevelPrefab},
                { LocationType.Sky, skyLevelPrefab},
            };
            var (countLocation, countStartVertix) = GetCountLocationWithDifficilty();

            _locationNetwork = new GeneratorGraph(countLocation, countStartVertix);
            generatorLocations = new GeneratorLocation();

            doorToLocations = GeneratorGraph.GraphToLocations(_locationNetwork);

            createdLevels = new Dictionary<int, LevelView>();
            levelViews = new List<LevelView>();
            doorModels = new Dictionary<int, DoorModel>();
            indexCreatedLocation = new List<int>();


            CreateDoorModels();

            indexCurrentLocation = 0;
            StartNewLocation(indexCurrentLocation);
        }

        private void CreateOrLoadLocation(int indexNext)
        {

            DeactivateAllLevels();
            if (createdLevels.ContainsKey(indexNext))
            {
                createdLevels[indexNext].gameObject.SetActive(true);
            }
            else
            {
                StartNewLocation(indexNext);
            }
        }

        private void StartNewLocation(int index)
        {


            indexCurrentLocation = index;
            var doorInNewModel = GetDoorModelsForIndexLocation(index);
            LocationType locationType = _locationNetwork.Rooms[index];
            LevelView levelPrefab = levelPrefabs[locationType];
            LevelModel newModel = generatorLocations.GenerateNewLocation(2, index, doorInNewModel, 3);
            LevelView levelInstance = Instantiate(levelPrefab);
            createdLevels[index] = levelInstance; //  добавление уровня в словарь
            levelInstance.model = newModel;
            levelInstance.SetModel();
            levelInstance.gameObject.SetActive(true);



        }

        private void DeactivateAllLevels()
        {
            foreach (var level in createdLevels.Values)
            {
                level.gameObject.SetActive(false);
            }
        }

        private void CreateDoorModels()
        {
            Debug.Log(doorToLocations.ToString());
            /*foreach (var door in doorToLocations)
            {
                //модель двери
                DoorModel doorModel = new DoorModel();
                doorModel.IndexDoor = door.Key;
                doorModel.TypeDoor = _locationNetwork.Rooms[door.Key];
                doorModel.TypeLocation = _locationNetwork.Rooms[door.Value.Item1];
                doorModel.IndexLocation = door.Value.Item1;

                //модель двери в словарь
                doorModels[door.Key] = doorModel;
            }*/

            foreach (var entry in doorToLocations)
            {
                //модель двери
                DoorModel doorModel = new DoorModel();
                doorModel.IndexDoor = entry.Key;
                doorModel.IndexLocation = entry.Value.Item1;

                doorModel.IndexesLocation = (entry.Value.Item1, entry.Value.Item2);
                Debug.Log($"IndexDoor = {doorModel.IndexDoor}, doorModel.IndexesLocation = {doorModel.IndexesLocation.Item1}, {doorModel.IndexesLocation.Item2} ");
                
                doorModel.TypesLocation = (_locationNetwork.Rooms[entry.Value.Item1], _locationNetwork.Rooms[entry.Value.Item2]);
                
                doorModel.TypesDoors = (_locationNetwork.Rooms[entry.Value.Item2], _locationNetwork.Rooms[entry.Value.Item1]);

                //doorModel.TypeLocation = _locationNetwork.Rooms[entry.Value.Item2];
                //doorModel.TypeDoor = _locationNetwork.Rooms[entry.Value.Item1];
                //модель двери в словарь
                doorModels[entry.Key] = doorModel;
            }

        }

        private List<DoorModel> GetDoorModelsForLocation(int currentLocationIndex)
        {
            var doorsForLocation = doorToLocations.Where(
                d=> d.Value.Item2 == currentLocationIndex || d.Value.Item1
                 == currentLocationIndex).Select(d => d.Key);

            var doorModelsForLocation = doorsForLocation.Select(d => doorModels[d]).ToList();
            return doorModelsForLocation;
        }

        private List<DoorModel> GetDoorModelsForIndexLocation(int currentLocationIndex)
        {
            var models = new List<DoorModel>();
            foreach (var doorModel in doorModels.Values)
            {
                if (doorModel.IndexesLocation.Item1 == currentLocationIndex || doorModel.IndexesLocation.Item2 == currentLocationIndex)
                {
                    models.Add(doorModel);
                }
            }
            return models;
        }

        /*private void TestCreateLocation()
        {
            // индекс и тип локации
            int locationIndex = _locationNetwork.ChestLocationIndex;
            LocationType locationType = _locationNetwork.Rooms[locationIndex];
            int edgeCount = _locationNetwork.Transitions[locationIndex].Count;
            //  соответствующий префаб LevelView
            LevelView levelPrefab = levelPrefabs[locationType];
            //  экземпляр префаба LevelView
            LevelModel newModel = generatorLocations.GenerateNewLocation(2, 0);
            LevelView levelInstance = Instantiate(levelPrefab);
            levelInstance.model = newModel;
            levelInstance.SetModel();
            // добавить как-то двери
        }*/

        // Update is called once per frame
        void Update()
        {
            
        }
        private (int,int) GetCountLocationWithDifficilty()
        {
            int difficulty = PlayerPrefs.GetInt("Difficulty");
            switch (difficulty)
            {
                case 1:
                    return (5, 1); // для уровня сложности 1
                case 2:
                    return (10, 1); // для уровня сложности 2
                case 3:
                    return (15, 2); // для уровня сложности 3
                default:
                    return (10, 1); // значения по умолчанию
            }
        }
    }

}
