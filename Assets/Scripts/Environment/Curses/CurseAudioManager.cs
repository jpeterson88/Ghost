using Assets.Scripts.Audio;
using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Actions.Implementations;
using System;
using UnityEngine;

namespace Assets.Scripts.Environment.Curses
{
    public class CurseAudioManager: MonoBehaviour
    {
        [SerializeField] private ActionCurseEnumScriptable startCurseScriptable, stopCurseScriptable;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private int curseCountBeforePlay = 1;
        [SerializeField] private string snapshotName;

        // TODO: This will likely be based on how spooked inhabitants are instead of active curses
        private int activeCurses = 0;

        private void Start()
        {
            startCurseScriptable.AddAction(HandleCurseStart);
            stopCurseScriptable.AddAction(HandleCurseStop);
        }

        private void HandleCurseStart(CurseTypeEnum curseType)
        {
            activeCurses += 1; ;

            if (activeCurses >= curseCountBeforePlay && !audioController.IsPlaying())
            {
                if(snapshotName != null)
                    audioController.StartSnapshot(snapshotName);
                audioController.PlayOneShot();
            }
        }

        private void HandleCurseStop(CurseTypeEnum curseType)
        {
            activeCurses -= 1;

            if (activeCurses < curseCountBeforePlay && audioController.IsPlaying())
            {
                audioController.Stop();

                if(snapshotName != null)
                    audioController.StopSnapshot(snapshotName);
            }
        }
    }
}
