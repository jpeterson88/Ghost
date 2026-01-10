using Assets.Scripts.Data.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    internal class MusicPlayer: MonoBehaviour
    {
        [SerializeField] private CachedAudioController mainSong, gettingSpookySong1;
        [SerializeField] private string snapshotName;
        [SerializeField] private float playDelay;

        private void Start()
        {
            mainSong.PlayOneShot();
        }

        public void HandleThematicRise() 
        {
            if (!gettingSpookySong1.IsPlaying())
            {
                mainSong.Stop();

                if (snapshotName != null)
                    gettingSpookySong1.StartSnapshot(snapshotName);
                LeanTween.value(0, 1, playDelay).setOnComplete(() => { gettingSpookySong1.PlayOneShot(); });
            }
        }

        public void HandleThematicFall()
        {
            if (gettingSpookySong1.IsPlaying()){

                if (snapshotName != null)
                    gettingSpookySong1.StopSnapshot(snapshotName);

                gettingSpookySong1.Stop();
                mainSong.PlayOneShot();
            }
        }

        [ContextMenu("Start Normal")]
        private void HandleNormalSong()
        {
            HandleThematicFall();
        }
    }
}
