using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Crystal
{
    // Класс CrystalModel представляет модель данных для кристалла
    public class CrystalModel
    {
        // Счет, который добавляется к общему счету игрока при сборе кристалла
        private int _score;
        public int Score {get => _score; set => _score = value;}

        private CrystalType _crystalType;

        public CrystalType Type {get => _crystalType;}

        // Конструктор класса CrystalModel
        // Принимает счет кристалла
        public CrystalModel(int score, CrystalType crystalType)
        {
            _score = score;
            _crystalType = crystalType;
        }

        // Конструктор по умолчанию класса CrystalModel
        public CrystalModel()
        {
            _score = 0;
        }
    }

}

