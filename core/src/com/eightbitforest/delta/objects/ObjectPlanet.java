package com.eightbitforest.delta.objects;

import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.objects.base.GameObjectDynamic;
import com.eightbitforest.delta.objects.base.GameObjectDynamicTriangle;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.GameRegistry;

/**
 * Created by osum4est on 9/18/16.
 */
public class ObjectPlanet {

    public static final float SIZE = 1.5f;

    public ObjectPlanet(float x, float y)
    {
        float unit = SIZE * 1/3;
        float side = (2 / (float) Math.sqrt(3)) * SIZE / 2;
        GameRegistry.registerObject(new ObjectPlanetPart(new Vector2(x, y + SIZE * 2/3), 180f));
        GameRegistry.registerObject(new ObjectPlanetPart(new Vector2(x + side, y + unit), 0f));
        GameRegistry.registerObject(new ObjectPlanetPart(new Vector2(x + side, y - unit), 180f));
        GameRegistry.registerObject(new ObjectPlanetPart(new Vector2(x, y - SIZE * 2/3), 0f));
        GameRegistry.registerObject(new ObjectPlanetPart(new Vector2(x - side, y - unit), 180f));
        GameRegistry.registerObject(new ObjectPlanetPart(new Vector2(x - side, y + unit), 0f));

    }

}
