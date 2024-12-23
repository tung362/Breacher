#ifndef LightMapSample_
#define LightMapSample_

//Light map light data.
struct GPULight
{
    bool _Active;
    int2 _Position;
    int _Shape;
    int _Radius;
};

//Light instances buffer simulating lighting.
StructuredBuffer<GPULight> _Lights;

//Draw gradient line within a radius.
bool DrawGradientLine(int startX, int startY, int endX, int endY, int radius, float2 gridUnit, UnityTexture2D obstructionMap)
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

//Draw gradient line within a square.
bool DrawGradientSquare(int midpointX, int midpointY, int halfSquareLength, int radius, float2 gridUnit, UnityTexture2D obstructionMap)
{
    //If outside of square then is out of range.
    if (gridUnit.x < midpointX - halfSquareLength || gridUnit.x > midpointX + halfSquareLength || gridUnit.y < midpointY - halfSquareLength || gridUnit.y > midpointY + halfSquareLength) return false;
    
    //Instead of drawing gradient lines to every square edge coordinate for each pixel fragment which would be more expensive, we instead draw a single gradient line to the pixel fragment's grid coordinate.
    if (DrawGradientLine(midpointX, midpointY, (int) gridUnit.x, (int) gridUnit.y, radius, gridUnit, obstructionMap) == 1) return true;
    return false;
}

//Custom shader graph node for sampling a light map buffer using grid coordinates.
void LightMapSample_float(float2 gridUnit, UnityTexture2D obstructionMap, float lightCount, out bool lightOut)
{
    //Loop through each light.
    for (int i = 0; i < lightCount; i++)
    {
        //Skip if light is inactive
        if (!_Lights[i]._Active) continue;
        
        //Calculate light based on light shape.
        switch (_Lights[i]._Shape)
        {
            case 0:
                lightOut = DrawGradientSquare(_Lights[i]._Position.x, _Lights[i]._Position.y, _Lights[i]._Radius, _Lights[i]._Radius, gridUnit, obstructionMap);
                break;
            case 1:
                lightOut = DrawGradientSquare(_Lights[i]._Position.x, _Lights[i]._Position.y, _Lights[i]._Radius, -1, gridUnit, obstructionMap);
                break;
            default:
                lightOut = DrawGradientSquare(_Lights[i]._Position.x, _Lights[i]._Position.y, _Lights[i]._Radius, _Lights[i]._Radius, gridUnit, obstructionMap);
                break;
        }
        
        if (lightOut == true) return;
    }
    lightOut = lightCount < 0 ? true : false;
}
#endif