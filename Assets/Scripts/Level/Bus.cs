using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
namespace Platformer2D
{
    public class Bus 
    {
        public static Bus Instance {get; private set;} = new Bus();

        
        public void Destroy()
        {
            Instance = null;
        }
        

        public event Action <int> SendScore;

        public void Send(int inpt)
        {
            SendScore?.Invoke(inpt);
        }

        public event Action <int> SendBackgroundScore;
        public void SendBackground(int inpt)
        {
            SendBackgroundScore?.Invoke(inpt);
        }

        public event Action <int> SendPlatformsScore;
        public void SendForPlatform(int inpt)
        {
            SendPlatformsScore?.Invoke(inpt);
        }
        
        //Событие для инертирования управления
        public event Action InvertControls;
        public void SendInvertControls()
        {
            InvertControls?.Invoke();
        }

        public event Action <LocationType> UpdateCrystal;
        public void SendCrystal(LocationType type)
        {
            UpdateCrystal?.Invoke(type);
        }

        public event Action <int> UdateTotalScore;
        public void SendAllScore(int count)
        {
            UdateTotalScore?.Invoke(count);
        }

        public event Action <int> UdateLevel;
        public void SendLevelPercent(int count)
        {
            UdateLevel?.Invoke(count);
        }

        public event Action <int> IndexNextLocation;
        public void SendNextIndexLocation (int index)
        {
            IndexNextLocation?.Invoke(index);
        }

        public event Action<bool> PlayerFell;
        public void SendMessage(bool isFell)
        {
            PlayerFell?.Invoke(isFell);
        }


        public event Action<int> GameWin;
        public void SendGameWin(int totalScore)
        {
            GameWin?.Invoke(totalScore);
        }


        public event Action<string> GameCompletedResult; //событие для отправки информации
        public Func<string> GetGameResults; //делегат для получения информации
        public void SendGameCompletedResult(string roomInfo) 
        {
            GameCompletedResult?.Invoke(roomInfo);
        }

        
    }
}

