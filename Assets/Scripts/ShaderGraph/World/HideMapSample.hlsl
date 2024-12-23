#ifndef HideMapSample_
#define HideMapSample_

//Hide map eye vision data.
struct GPUEye
{
    bool _Active;
    int2 _Position;
    int _Shape;
    int _Radius;
};

//Eye instances buffer simulating vision.
StructuredBuffer<GPUEye> _Eyes;

//Draw sight line within a radius.
bool DrawSightLine(int startX, int startY, int endX, int endY, int radius, float2 gridUnit, UnityTexture2D obstructionMap)
{
    int dx = abs(endX - startX);
    int dy = abs(endY - startY);
    int sx = startX < endX ? 1 : -1;
    int sy = startY < endY ? 1 : -1;
    int err = dx - dy;
    int radiusSquared = radius * radius;
    int x = startX;
    int y = startY;
    while (true)
    {
        if (radius > 0)
        {
            //float distance = Mathf.Sqrt(Mathf.Pow(x - startX, 2) + Mathf.Pow(y - startY, 2));
            //if (distance > radius) break;

            int dxCurrent = x - startX;
            int dyCurrent = y - startY;
            if (dxCurrent * dxCurrent + dyCurrent * dyCurrent > radiusSquared) break;
        }

        if (x == gridUnit.x && y == gridUnit.y) return true;
        
        float4 obstruction = obstructionMap.Load(int3(x, y, 0));
        if (obstruction.x == 0) break;

        if (x == endX && y == endY) break;

        int e2 = 2 * err;
        if (e2 > -dy)
        {
            err -= dy;
            x += sx;
        }

        if (e2 < dx)
        {
            err += dx;
            y += sy;
        }
    }
    return false;
}

//Draw sight line within a square.
bool DrawSightSquare(int midpointX, int midpointY, int halfSquareLength, int radius, float2 gridUnit, UnityTexture2D obstructionMap)
{
    //If outside of sight square then is out of sight.
    if (gridUnit.x < midpointX - halfSquareLength || gridUnit.x > midpointX + halfSquareLength || gridUnit.y < midpointY - halfSquareLength || gridUnit.y > midpointY + halfSquareLength) return false;
    
    //Instead of drawing sight lines to every square edge coordinate for each pixel fragment which would be more expensive, we instead draw a single sight line to the pixel fragment's grid coordinate.
    if (DrawSightLine(midpointX, midpointY, (int)gridUnit.x, (int)gridUnit.y, radius, gridUnit, obstructionMap) == 1) return true;
    return false;
}

//Custom shader graph node for sampling a hide map texture using grid coordinates.
void HideMapSampleTexture_float(float2 gridUnit, UnityTexture2D hideMap, out bool hideOut)
{
    hideOut = hideMap.Load(int3(gridUnit.x, gridUnit.y, 0));
}

//Custom shader graph node for sampling a hide map buffer using grid coordinates.
void HideMapSample_float(float2 gridUnit, UnityTexture2D obstructionMap, float eyeCount, out bool hideOut)
{
    //Loop through each eye vision.
    for (int i = 0; i < eyeCount; i++)
    {
        //Skip if eye is inactive
        if (!_Eyes[i]._Active) continue;
        
        //Calculate vision based on eye shape.
        switch (_Eyes[i]._Shape)
        {
            case 0:
                hideOut = DrawSightSquare(_Eyes[i]._Position.x, _Eyes[i]._Position.y, _Eyes[i]._Radius, _Eyes[i]._Radius, gridUnit, obstructionMap);
                break;
            case 1:
                hideOut = DrawSightSquare(_Eyes[i]._Position.x, _Eyes[i]._Position.y, _Eyes[i]._Radius, -1, gridUnit, obstructionMap);
                break;
            default:
                hideOut = DrawSightSquare(_Eyes[i]._Position.x, _Eyes[i]._Position.y, _Eyes[i]._Radius, _Eyes[i]._Radius, gridUnit, obstructionMap);
                break;
        }
        
        if (hideOut == true) return;
    }
    hideOut = eyeCount < 0 ? true : false;
}
#endif