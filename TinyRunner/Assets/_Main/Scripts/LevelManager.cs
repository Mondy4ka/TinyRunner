using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LevelManager
{
    private readonly GameManager _gameManager;
    private readonly PlayerMovement _playerMovement;
    private readonly IReadOnlyList<GameObject> _levelChunkPrefabs;
    private readonly float _chunkLength;

    private List<GameObject> _activeChunks = new();

    public LevelManager(GameManager gameManager, List<GameObject> levelChunkPrefabs, float chunkLength, PlayerMovement playerMovement)
    {
        _gameManager = gameManager;
        _levelChunkPrefabs = levelChunkPrefabs;
        _chunkLength = chunkLength;
        _playerMovement = playerMovement;
    }

    public void Initialize()
    {
        _playerMovement.OnDistanceChanged += SpawnChunk;
        _gameManager.OnStartGame += StartGame;
    }

    public void Deinitialize()
    {
        _playerMovement.OnDistanceChanged -= SpawnChunk;
        _gameManager.OnStartGame -= StartGame;
    }

    private void StartGame()
    {
        ClearLevel();
        SpawnChunk();
    }

    private void SpawnChunk()
    {
        Vector2 position;

        if (_activeChunks.Count > 0)
        {
            position = _activeChunks.Last().transform.position;
            position.x += _chunkLength;
        }
        else
        {
            position = new(20, 0);
        }

        var randomChunk = _levelChunkPrefabs[UnityEngine.Random.Range(0, _levelChunkPrefabs.Count)];

        _activeChunks.Add(UnityEngine.Object.Instantiate(randomChunk, position, Quaternion.identity));

        if (_activeChunks.Count > 3)
        {
            DeleteLastChunk();
        }
    }

    private void DeleteLastChunk()
    {
        var chunk = _activeChunks[0];
        _activeChunks.Remove(chunk);
        UnityEngine.Object.Destroy(chunk);
    }

    private void ClearLevel()
    {
        foreach (var chunk in _activeChunks)
            UnityEngine.Object.Destroy(chunk);

        _activeChunks.Clear();
    }
}