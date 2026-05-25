using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace WaveSystem
{
    public class WaveManager :  Singleton<WaveManager>
    {
        [SerializeField] private WaveSequence sequence;
        [SerializeField] private Vector2[] spawnPoints;
        [SerializeField] private GameObject enemyTemplate;
        
        private int waveNbr;
        private WaveData currentWave;
        private float triggerWaveTime;
        private readonly List<SpawnData> triggeredSpawns = new();
        private readonly List<SpawnData> finishedSpawns = new();
        
        public int WaveNbr =>  waveNbr;

        public bool IsEndOfSequence => waveNbr >= sequence.Waves.Count;
        public bool IsDuringWave => currentWave != null;

        [SerializeField] public UnityEvent onWaveEnds;
        [SerializeField] public UnityEvent onWaveBegins;

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

                if (finishedSpawns.Count >= currentWave.spawns.Count && EnemyFactory.EnemyCount == 0)
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
            onWaveBegins.Invoke();
        }
        
        private void EndCurrentWave()
        {
            currentWave = null;
            onWaveEnds.Invoke();
        }

        private void HandleWave(WaveData wave)
        {
            float time = Time.time;
            foreach (SpawnData spawn in currentWave.spawns)
            {
                if (!(time - triggerWaveTime >= spawn.spawnTime) || triggeredSpawns.Contains(spawn)) continue;
                
                StartCoroutine(SpawnRoutine(spawn));
                triggeredSpawns.Add(spawn);
            }
        }

        private IEnumerator SpawnRoutine(SpawnData spawn)
        {
            for (int i = 1; i < spawn.nbrToSpawn; i++)
            {
                EnemyFactory.TryInstantiateEnemy(spawnPoints[spawn.spawnPoint], out Health _);
                yield return new WaitForSeconds(1f);
            }
            
            finishedSpawns.Add(spawn);
        }
        
    }
}