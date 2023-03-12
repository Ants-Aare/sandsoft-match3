using DG.Tweening;

namespace AAA.SDKs.SequenceBuilder.Runtime
{
    public interface ISequenceBuilder
    {
        Tween BuildSequence();
        int GetPriority();
        bool ShouldAddPauseBetweenPreviousSequence() => true;
    }
}