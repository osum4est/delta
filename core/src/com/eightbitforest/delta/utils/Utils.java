package com.eightbitforest.delta.utils;

import com.badlogic.gdx.utils.JsonValue;

import java.util.Random;


public class Utils {
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

    public static float getTriangleHeight(float triangleSide) {
        return triangleSide / (2 / (float) Math.sqrt(3));
    }

    public static String getJsonStringOrDefault(JsonValue json, String key, String def) {
        if (json.has(key))
            return json.getString(key);
        return def;
    }

    public static float getJsonFloatOrDefault(JsonValue json, String key, float def) {
        if (json.has(key))
            return json.getFloat(key);
        return def;
    }
}
