#ifndef FloorSquareBlend_
#define FloorSquareBlend_

//Custom shader graph node for floor square blend
void FloorSquareBlend_float(float2 gridUnit, float2 dimension, out float IndexOut)
{
    dimension -= 1;
    if (gridUnit.x > 0 && gridUnit.x < dimension.x && gridUnit.y > 0 && gridUnit.y < dimension.y) IndexOut = 2; //Middle
    else if (gridUnit.y == 0 && gridUnit.x > 0 && gridUnit.x < dimension.x) IndexOut = 1; //Bottom side
    else if (gridUnit.x == 0 && gridUnit.y > 0 && gridUnit.y < dimension.y) IndexOut = 4; //Left side
    else if (gridUnit.y == dimension.y && gridUnit.x > 0 && gridUnit.x < dimension.x) IndexOut = 6; //Top side
    else if (gridUnit.x == dimension.x && gridUnit.y > 0 && gridUnit.y < dimension.y) IndexOut = 8; //Right side
    else if (gridUnit.x == 0 && gridUnit.y == 0) IndexOut = 0; //Bottom left corner
    else if (gridUnit.x == 0 && gridUnit.y == dimension.y) IndexOut = 3; //Top left corner
    else if (gridUnit.x == dimension.x && gridUnit.y == dimension.y) IndexOut = 5; //Top right corner
    else if (gridUnit.x == dimension.x && gridUnit.y == 0) IndexOut = 7; //Bottom right corner
    else IndexOut = 2;
}
#endif