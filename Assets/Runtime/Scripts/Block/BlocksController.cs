using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlockSpawnController))]
public class BlocksController : MonoBehaviour
{
    [SerializeField] private List<Block> avaliableBlocks;
    [SerializeField] private Block selectedMainBlock;

    private readonly int avaliableBlocksAmount = 3;
    private BlockSpawnController blockSpawnController;

    public static event Action<List<Block>> AvaliableBlocksEvent;
    public static event Action<Block> SelectedMainBlockEvent;

    private void Awake()
    {
        blockSpawnController = GetComponent<BlockSpawnController>();
        BlockSelectionController.BlockSelectedEvent += OnBlockSelectedEvent;
    }

    private void OnDestroy()
    {
        BlockSelectionController.BlockSelectedEvent -= OnBlockSelectedEvent;
    }

    private void Start()
    {
        if (blockSpawnController)
        {
            blockSpawnController.SpawnAllBlocks();
            GetAvaliableBlocks();                       
        }                
    }    
    private void OnBlockSelectedEvent(Block block)
    {
        if (selectedMainBlock != block)
        {
            selectedMainBlock = block;
            SelectedMainBlockEvent?.Invoke(block);
        }
    }

    private void GetAvaliableBlocks()
    {
        int selectedBlocksAmount = 0;
        if (blockSpawnController.SpawnedBlocks.Count > 0)
        {
            while (selectedBlocksAmount < avaliableBlocksAmount)
            {
                Block block = GetRandomBlock();
                if (block)
                {
                    if (NotContains(block, avaliableBlocks))
                    {
                        avaliableBlocks.Add(block);
                        selectedBlocksAmount++;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        
        if (avaliableBlocks.Count >= avaliableBlocksAmount)
        {            
            AvaliableBlocksEvent?.Invoke(avaliableBlocks);
        }
    }

    private Block GetRandomBlock()
    {
        Block block = null;
        if (blockSpawnController)
        {
            int blockAmount = blockSpawnController.SpawnedBlocks.Count;
            if (blockAmount > 0)
            {
                int randomBlockIndex = UnityEngine.Random.Range(0, blockAmount);
                if (randomBlockIndex >= 0 && randomBlockIndex < blockAmount)
                {
                    block = blockSpawnController.SpawnedBlocks[randomBlockIndex];
                }
            }
        }        
        return block;
    }

    private bool NotContains(Block block, List<Block> blocks)
    {
        return !blocks.Contains(block);
    }
}