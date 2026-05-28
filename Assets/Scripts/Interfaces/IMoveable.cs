public interface IMoveable
{
    bool IsGrounded { get; }
    float ObjLastDir { get; }
    void Jump();
    void ChangeObjDir(float objDir);
}