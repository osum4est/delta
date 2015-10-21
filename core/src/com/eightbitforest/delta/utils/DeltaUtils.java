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

    public static int arrayContains(Object[] array, Object value) {
        for (int i = 0; i < array.length; i++)
            if (array[i] == value)
                return i;

        return -1;
    }

    public static boolean arrayContainsMultiple(Object[] array, Object[] values) {

        int matches = 0;
        for (Object o : array)
            for (Object v : values)
                if (o == v)
                    matches++;

        return matches == values.length;

    }
}
