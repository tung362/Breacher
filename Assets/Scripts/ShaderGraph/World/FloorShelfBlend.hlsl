#ifndef FloorShelfBlend_
#define FloorShelfBlend_

//Custom shader graph node for floor shelf blend
void FloorShelfBlend_float(float2 gridUnit, float2 dimension, out float IndexOut)
{
    IndexOut = (gridUnit.y * dimension.x) + gridUnit.x;
    
    /*if (gridUnit.y == 0)
    {
        if (gridUnit.x == 0)
        {
            IndexOut = 0;
        }
        else if (gridUnit.x == 1)
        {
            IndexOut = 1;
        }
        else
        {
            IndexOut = 2;
        }
    }
    else if (gridUnit.y == 1)
    {
        if (gridUnit.x == 0)
        {
            IndexOut = 3;
        }
        else if (gridUnit.x == 1)
        {
            IndexOut = 4;
        }
        else
        {
            IndexOut = 5;
        }
    }
    else
    {
        if (gridUnit.x == 0)
        {
            IndexOut = 6;
        }
        else if (gridUnit.x == 1)
        {
            IndexOut = 7;
        }
        else
        {
            IndexOut = 8;
        }
    }*/
}
#endif