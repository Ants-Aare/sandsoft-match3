namespace AAA.SDKs.Match3.Runtime
{
    public interface ITileFactory<out T>
    {
        T GetTile();
    }
}