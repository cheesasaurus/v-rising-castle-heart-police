using Unity.Mathematics;

namespace CastleHeartPolice.Utils;

public static class TerritoryUtil {

    // One unit on the block grid is equivalent to 5 units on the world grid
    private static int BlockSize = 5;

    // I have no idea why, but the block coordinates origin and world coordinates origin don't exactly line up
    private static int2 BlockOffsetFromWorld = new int2(639, 639);

    public static int2 BlockCoordinatesFromWorldPosition(float3 worldPos) {
        var intWorldPos = new int2((int)worldPos.x, (int)worldPos.z);
        return (intWorldPos / BlockSize) + BlockOffsetFromWorld;
    }

}
