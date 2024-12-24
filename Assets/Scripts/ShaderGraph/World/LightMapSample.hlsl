#ifndef LightMapSample_
#define LightMapSample_

//Light map light data.
struct GPULight
{
    bool _Active;
    int2 _Position;
    int _Shape;
    int _Radius;
    float4 _GradientColor;
    float _Intensity;
};

//Light instances buffer simulating lighting.
StructuredBuffer<GPULight> _Lights;

//Draw gradient line within a radius.
float4 DrawGradientLine(int startX, int startY, int endX, int endY, GPULight light, float2 gridUnit, UnityTexture2D obstructionMap)
{
    int dx = abs(endX - startX);
    int dy = abs(endY - startY);
    int sx = startX < endX ? 1 : -1;
    int sy = startY < endY ? 1 : -1;
    int err = dx - dy;
    int x = startX;
    int y = startY;
    float4 lightColor = light._GradientColor * light._Intensity;
    int radiusSquared = light._Radius * light._Radius;
    int steps = 0;
    while (true)
    {
        //If light shape requires a radius check.
        if (light._Shape == 0)
        {
            int dxCurrent = x - startX;
            int dyCurrent = y - startY;
            if (dxCurrent * dxCurrent + dyCurrent * dyCurrent > radiusSquared) break;
        }

        //If line step is within pixel fragment's targeted grid coordinate.
        if (x == gridUnit.x && y == gridUnit.y)
        {
            float gradient = (float)steps / (float)light._Radius;
            return lerp(lightColor, float4(0, 0, 0, 0), gradient);
        }
        
        //If line step is obstructed.
        float4 obstruction = obstructionMap.Load(int3(x, y, 0));
        if (obstruction.x == 0) break;

        //If line step reached the end grid coordinate.
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
        steps++;
    }
    return float4(0, 0, 0, 0);
}

//Draw gradient line within a square.
float4 DrawGradientSquare(GPULight light, float2 gridUnit, UnityTexture2D obstructionMap)
{
    //If outside of square then is out of range.
    if (gridUnit.x < light._Position.x - light._Radius || gridUnit.x > light._Position.x + light._Radius || 
        gridUnit.y < light._Position.y - light._Radius || gridUnit.y > light._Position.y + light._Radius)
    {
        return float4(0, 0, 0, 0);
    }
    
    //Instead of drawing gradient lines to every square edge coordinate for each pixel fragment which would be more expensive, we instead draw a single gradient line to the pixel fragment's grid coordinate.
    return DrawGradientLine(light._Position.x, light._Position.y, (int)gridUnit.x, (int)gridUnit.y, light, gridUnit, obstructionMap);
}

//Custom shader graph node for sampling a light map buffer using grid coordinates.
void LightMapSample_float(float2 gridUnit, UnityTexture2D obstructionMap, float lightCount, out float4 lightOut)
{
    //Loop through each light.
    lightOut = float4(0, 0, 0, 0);
    for (int i = 0; i < lightCount; i++)
    {
        //Skip if light is inactive
        if (!_Lights[i]._Active) continue;
        
        //Calculate light based on light shape.
        lightOut += DrawGradientSquare(_Lights[i], gridUnit, obstructionMap);
    }
}
#endif