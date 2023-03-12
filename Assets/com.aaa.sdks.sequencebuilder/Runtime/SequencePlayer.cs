using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AAA.Utility.Extensions;
using DG.Tweening;
using UnityEngine;

namespace AAA.SDKs.SequenceBuilder.Runtime
{
    //This is temporarily a static class
    public static class SequencePlayer
    {
        public static float PauseDuration = 0.1f;

        private static Sequence _currentlyPlayingSequence;
        private static readonly List<ISequenceBuilder> SequenceBuilders = new();
        private static bool _isSequencePlayerReady;
        private static bool _hasStartedSequences;

        public static void AddSequenceBuilder(ISequenceBuilder sequenceBuilder)
        {
            SequenceBuilders.Add(sequenceBuilder);
        }

        public static void StartSequencesNextFrame()
        {
            if (_hasStartedSequences)
                return;
            WaitFrameAndStartAllSequences().RunAsync();
            _hasStartedSequences = true;
        }

        private static async Task WaitFrameAndStartAllSequences()
        {
            await Task.Yield();
            _hasStartedSequences = false;
            StartAllSequences();
        }

        public static void StartAllSequences()
        {
            if (SequenceBuilders.Count == 0)
            {
                Debug.Log("No Sequences to play");
                return;
            }
            StopAllSequences();

            _currentlyPlayingSequence = DOTween.Sequence();

            SequenceBuilders.Sort((builder1, builder2) => builder1.GetPriority().CompareTo(builder2.GetPriority()));

            var previousPriority = int.MinValue;
            for (var i = 0; i < SequenceBuilders.Count; i++)
            {
                var sequence = SequenceBuilders[i].BuildSequence();
                if (sequence == null)
                    continue;

                var currentPriority = SequenceBuilders[i].GetPriority();
                if (currentPriority > previousPriority)
                {
                    if (i != 0)
                        _currentlyPlayingSequence.AppendInterval(PauseDuration);
                    _currentlyPlayingSequence.Append(sequence);

                    previousPriority = currentPriority;
                }
                else if (currentPriority == previousPriority)
                {
                    _currentlyPlayingSequence.Join(sequence);
                }
            }

            SequenceBuilders.Clear();
        }

        public static void StopAllSequences()
        {
            _currentlyPlayingSequence?.Kill(true);
        }
    }
}