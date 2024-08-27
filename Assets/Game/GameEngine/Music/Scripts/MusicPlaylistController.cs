using System.Collections;
using GameSystem;
using UnityEngine;

namespace Game.GameEngine
{
    public sealed class MusicPlaylistController : MonoBehaviour,
        IGameStartElement,
        IGameFinishElement
    {
        [SerializeField]
        private MusicPlaylist playlist;
        
        [SerializeField]
        private float pauseBetweenTracks = 1.5f;

        private int trackPointer;

        void IGameStartElement.StartGame()
        {
            MusicManager.OnFinsihed += OnMusicFinished;
            
            var track = playlist.trackList[0];
            MusicManager.Play(track);
        }

        void IGameFinishElement.FinishGame()
        {
            MusicManager.OnFinsihed -= OnMusicFinished;
            MusicManager.Stop();
        }

        private void OnMusicFinished()
        {
            trackPointer++;
            if (trackPointer >= playlist.trackList.Length)
            {
                trackPointer = 0;
            }

            StartCoroutine(PlayNextTrack());
        }

        private IEnumerator PlayNextTrack()
        {
            yield return new WaitForSeconds(pauseBetweenTracks);
            var nextTrack = playlist.trackList[trackPointer];
            MusicManager.Play(nextTrack);
        }
    }
}