using System.Collections;
using System.Collections.Generic;
using Tangelo.Test.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Tangelo.Test.Managers
{
    // QUESTION 4: Add a new status called "Spawning" that should be called from a key press (for example Space) or from a button click
    public enum GameStatus { Loading, Playing, Spawning, Recycling }

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int m_MaxMassiveSpawningObjects; // Minimum number: 200

        [SerializeField]
        private int m_MaxRecycleObjects; // Minimum number: 200

        private GameStatus m_CurrentStatus = GameStatus.Loading;
        private GameStatus m_Status;

        public SpawnerComponent spawner_component_scr;
        public float radius_for_random_spawn = 10.0f;
        public Slider ui_slider;

        private void Awake()
        {
            m_CurrentStatus = GameStatus.Loading;
            PreparePools();
        }

        private void Update()
        {
            if (m_CurrentStatus == m_Status)
                return;
        }

        // QUESTION 8: Implement here a loading process (using a UI Slider) to show how the Pool is being populated.
        private void PreparePools()
        {
            //The slider loads in one frame because I populate the pool in a frame
            ui_slider.maxValue = m_MaxMassiveSpawningObjects;
            spawner_component_scr.PopulatePool(m_MaxMassiveSpawningObjects, ui_slider);
            m_CurrentStatus = GameStatus.Playing;
        }

        // QUESTION 6: Implement here a massive spawning of objects.
        // FPS Can't drop below 50fps when MassiveSpawning is being executed.
        // If there are not enough objects in the Pool, increment the size. During the increment of the Pool size,
        // the FPS can't drop below 30fps
        // Implement a UI Button to execute this action
        private void MassiveSpawn()
        {
            m_CurrentStatus = GameStatus.Spawning;
            for (int i = 0; i < m_MaxMassiveSpawningObjects; i++) {
                Vector3 position_spawn = Random.insideUnitSphere * radius_for_random_spawn;
                position_spawn.z = 0;
                spawner_component_scr.Spawn(position_spawn, m_MaxMassiveSpawningObjects);
            }
            spawner_component_scr.NotifyPoolStatus();
            m_CurrentStatus = GameStatus.Playing;
        }

        // QUESTION 7: Implement here a massive recycling of the objects currently visible in the screen
        // FPS Can't drop below 50fps when MassiveRecycle is being executed
        // Implement a UI Button to execute this action
        private void MassiveRecycle()
        {
            m_CurrentStatus = GameStatus.Recycling;
            spawner_component_scr.Recycle(m_MaxRecycleObjects);
            m_CurrentStatus = GameStatus.Playing;
        }

        //This actions are called by the onClick()
        public void SpawnAction()
        {
            MassiveSpawn();
        }

        public void RecycleAction()
        {
            MassiveRecycle();
        }
    }
}