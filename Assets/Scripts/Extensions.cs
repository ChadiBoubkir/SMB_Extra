using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    private static LayerMask playerMask = LayerMask.GetMask("Player");
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        float radius = 0.25f;
        float distance = 0.375f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool KoopaRaycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        float radius = 0.5f;
        float distance = 0.255f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool VenoombaRaycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        float radius = 0.25f;
        float distance = 1.88f;
        if (direction == Vector2.right || direction == Vector2.left) {
            direction = Vector2.down;
            radius = 0.15f;
            distance = 1.85f;
        }

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool BlockRaycast(this Rigidbody2D rigidbody, Vector2 direction, string objName)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        float radius = 0.15f;
        float distance = 1.75f;
        if (objName == "Venoomba") {
            if (direction == Vector2.right || direction == Vector2.left) {
                direction = Vector2.down;
                radius = 0.15f;
                distance = 1.75f;
            }
        }

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        if (testDirection == Vector2.up)
        {
            return Vector2.Dot(direction.normalized, testDirection) > 0.785f;
        }else {
            return Vector2.Dot(direction.normalized, testDirection) > 0.5f;
        }
    }

    public static bool DetectPlayer(this Rigidbody2D rigidbody)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        float radius = 13f;
        float distance = 0f;

        RaycastHit2D hitU = Physics2D.CircleCast(rigidbody.position, radius, Vector2.up.normalized, distance, playerMask);
        RaycastHit2D hitD = Physics2D.CircleCast(rigidbody.position, radius, Vector2.down.normalized, distance, playerMask);
        RaycastHit2D hitR = Physics2D.CircleCast(rigidbody.position, radius, Vector2.right.normalized, distance, playerMask);
        RaycastHit2D hitL = Physics2D.CircleCast(rigidbody.position, radius, Vector2.left.normalized, distance, playerMask);

        return hitU.collider != null && hitU.rigidbody != rigidbody &&
        hitD.collider != null && hitD.rigidbody != rigidbody &&
        hitR.collider != null && hitR.rigidbody != rigidbody &&
        hitL.collider != null && hitL.rigidbody != rigidbody;
    }
}