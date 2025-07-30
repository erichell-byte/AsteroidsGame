using Cysharp.Threading.Tasks;

namespace Systems
{
    public interface ISceneLoader
    {
        UniTask LoadNextScene();
        UniTask LoadPreviousScene();
        UniTask LoadFirstScene();
    }
}