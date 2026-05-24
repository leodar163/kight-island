using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using Utils;

namespace WaveSystem
{
    public class WaveManager :  Singleton<WaveManager>
    {
        [SerializeField] private WaveSequence sequence;
        [SerializeField] private Vector2[] spawnPoints;
        [SerializeField] private GameObject enemyTemplate;
        
        private int waveNbr;
        private int enemyCount;
        private WaveData currentWave;
        private float triggerWaveTime;
        private readonly List<SpawnData> triggeredSpawns = new();
        private readonly List<SpawnData> finishedSpawns = new();
        
        public int WaveNbr =>  waveNbr;

        public bool IsEndOfSequence => waveNbr >= sequence.Waves.Count;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.orange;
            foreach (Vector3 point in spawnPoints)
            {
                Gizmos.DrawWireSphere(point, 0.5f);
            }
        }
        
        private void Update()
        {
            if (currentWave != null)
            {
                HandleWave(currentWave);

                if (finishedSpawns.Count >= currentWave.spawns.Count && enemyCount == 0)
                {
                    EndCurrentWave();
                }
            }
        }

        public void TriggerNextWave()
        {
            if (IsEndOfSequence) return;
            
           TriggerWave(sequence.Waves[waveNbr]);
        }

        private void TriggerWave(WaveData wave)
        {
            waveNbr++;
            currentWave = wave;
            triggerWaveTime = Time.time;
            triggeredSpawns.Clear();
            finishedSpawns.Clear();
            enemyCount = 0;
        }
        
        private void EndCurrentWave()
        {
            currentWave = null;
        }

        private void HandleWave(WaveData wave)
        {
            float time = Time.time;
            foreach (SpawnData spawn in currentWave.spawns)
            {
                if (time >= spawn.spawnTime && !triggeredSpawns.Contains(spawn))
                {
                    StartCoroutine(SpawnRoutine(spawn));
                    triggeredSpawns.Add(spawn);
                }
            }
        }

        private IEnumerator SpawnRoutine(SpawnData spawn)
        {
            for (int i = 1; i < spawn.nbrToSpawn; i++)
            {
                if (Instantiate(enemyTemplate, spawnPoints[spawn.spawnPoint], new Quaternion()).TryGetComponent(out Health health))
                {
                    enemyCount++;
                    health.onHealthReachZero.AddListener(() => { enemyCount--;});
                }
                yield return new WaitForSeconds(1f);
            }
            
            finishedSpawns.Add(spawn);
        }
        
    }
}