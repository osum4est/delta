package com.eightbitforest.delta.utils;

import java.util.Random;

/**
 * Created by osumf on 8/22/2015.
 */
public class DeltaUtils {
    public static int randomRange(float min, float max)
    {
        return randomRange((int)min, (int)max);
    }
    public static int randomRange(int min, int max)
    {

        return new Random().nextInt(max - min + 1) + min;
    }
}
