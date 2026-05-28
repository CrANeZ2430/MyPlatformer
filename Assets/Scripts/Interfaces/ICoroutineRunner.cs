using System.Collections;

public interface ICoroutineRunner
{
    void ExecuteCoroutine(IEnumerator coroutine);
}
