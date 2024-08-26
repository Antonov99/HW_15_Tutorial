using System;
using Entities;
using JetBrains.Annotations;
using Lessons.Game.Events;
using UnityEngine;
using VContainer.Unity;
using PlayerService = Lessons.Game.Services.PlayerService;

namespace Lessons.Game
{
    [UsedImplicitly]
    public sealed class PlayerController : IStartable, IDisposable
    {
        private readonly KeyboardInput _input;
        private readonly IEntity _player;

        private readonly EventBus _eventBus;

        public PlayerController(KeyboardInput input, 
            EventBus eventBus, PlayerService playerService)
        {
            _input = input;
            _player = playerService.Player;

            _eventBus = eventBus;
        }
        
        void IStartable.Start()
        {
            _input.OnMove += OnMovePreformed;
        }

        void IDisposable.Dispose()
        {
            _input.OnMove -= OnMovePreformed;
        }

        private void OnMovePreformed(Vector2Int direction)
        {
            _eventBus.RaiseEvent(new ApplyDirectionEvent(_player, direction));
        }
    }
}