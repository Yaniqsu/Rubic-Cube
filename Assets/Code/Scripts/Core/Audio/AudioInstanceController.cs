using System;
using UnityEngine;

namespace SoundSystem
{
	public class AudioInstanceController : MonoBehaviour
	{
		private SoundProcess _process;

		public Action<SoundProcess> onStopSoundProcess;

		public SoundProcess SoundProcess
		{
			set => _process = value;
		}

        private void OnDestroy()
        {
			if (_process != null)
			{
				onStopSoundProcess?.Invoke(_process);
			}
        }
    } 
}
