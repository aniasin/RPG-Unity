
namespace RPG.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerControl playerControl);
        CursorType GetCursorType();
    }
}
